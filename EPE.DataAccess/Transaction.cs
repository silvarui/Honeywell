using System;
using System.Data;
using System.Data.SqlClient;

namespace EPE.DataAccess
{
    public class Transaction : IDisposable
    {
        #region Fields

        //Track whether _internalTransaction was Commited or Rolled back. We cannot control it fully because of the property CurrentTransaction.
        //For example, if the property is used to roll back transaction: new Transaction().CurrentTransaction.Rollback(), then Transaction class will never know about that.
        private bool _isCompleted;
        // Track whether Dispose has been called.
        private bool _disposed;

        private SqlTransaction _internalTransaction;
        private SqlConnection _sqlConn;

        #endregion Fields


        public event EventHandler TransactionCompleted;


        #region Ctor

        //Make sure the object cannot be instantiated outside the DataAcessLayer or BusinessLayer
        internal Transaction(string connString, IsolationLevel isolationLevel)
        {
            if (string.IsNullOrWhiteSpace(connString))
                throw new ArgumentNullException("connString");

            _sqlConn = new SqlConnection(connString);
            _sqlConn.Open();
            _internalTransaction = _sqlConn.BeginTransaction(isolationLevel);
        }

        //Make sure the object cannot be instantiated outside the DataAcessLayer or BusinessLayer
        internal Transaction(string connString) : this(connString, IsolationLevel.ReadUncommitted)
        {
        }

        #endregion Ctor


        #region Properties

        //Make sure the property cannot be called outside the DataAcessLayer or BusinessLayer
        internal SqlTransaction CurrentTransaction
        {
            get { return _internalTransaction; }
        }

        #endregion Properties


        #region Methods

        /// <summary>
        ///   Rolls back the current transaction.
        /// </summary>
        public void Rollback()
        {
            if (_disposed)
                return;

            try
            {
                _internalTransaction.Rollback();
                _sqlConn.Close();
                OnTransactionCompleted(EventArgs.Empty);
            }
            finally
            {
                _isCompleted = true;
            }
        }

        /// <summary>
        ///   Commits the current transaction.
        /// </summary>
        public void Commit()
        {
            if (_disposed)
                return;

            try
            {
                _internalTransaction.Commit();
                _sqlConn.Close();
                OnTransactionCompleted(EventArgs.Empty);
            }
            finally
            {
                _isCompleted = true;
            }
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // Use SupressFinalize in case a subclass 
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        #endregion Methods


        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer (in case a subclass of this type 
        // implements a finalizer) and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources.
                    if (!_isCompleted)
                    {
                        // Attempt to roll back the transaction.
                        try
                        {
                            // Rollback transaction explicitly to raise an event TransactionCompleted
                            _internalTransaction.Rollback();
                            OnTransactionCompleted(EventArgs.Empty);
                        }
                        catch (Exception)
                        {
                            //Try/Catch exception handling should always be used when committing or rolling back a SqlTransaction. 
                            //Both Commit and Rollback generate an InvalidOperationException if the connection is terminated or if the transaction has already been rolled back on the server.
                        }
                    }

                    //SqlConnection.Dispose() call Close() method internally (see this in Reflector). The SqlConnection.Close method rolls back any pending transactions. 
                    //It then releases the connection to the connection pool, or closes the connection if connection pooling is disabled. 
                    //See: http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
                    _sqlConn.Dispose();

                    _internalTransaction.Dispose(); //The SqlTransaction.Dispose method rolls back any pending transactions.
                }

                // Free unmanaged resources


                // Indicate that the instance has been disposed.
                _disposed = true;
                _sqlConn = null;
                _internalTransaction = null;
                TransactionCompleted = null;
            }
        }

        // This type does not need a finalizer because it does not 
        // directly create a native resource. See: http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx
        //~Transaction()
        //{
        //   // In case the client forgets to call Dispose, 
        //   // destructor will be invoked.
        //   Dispose(false);
        //}

        /// <summary>
        ///   Raises the <see cref = "E:IPS.Core.DtAccess.Transaction.TransactionCompleted"></see> event.
        /// </summary>
        /// <param name = "e">A <see cref = "T:System.EventArgs"></see> object containing event data.</param>
        protected virtual void OnTransactionCompleted(EventArgs e)
        {
            if (TransactionCompleted != null)
            {
                TransactionCompleted(this, EventArgs.Empty);
            }
        }
    }
}
