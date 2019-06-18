using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPE.DataAccess;

namespace EPE.BusinessLayer
{
    public abstract class CacheableEntityAdapter<T> : EntityDbAdapter<T> 
        where T : Entity, new()
    {
        // represents the stored procedure to be called to cache data
        protected string storedProcedure;

        // Parameters for the stored procedure used to cache data
        protected Parameters storedProcParams;

        // Reference to the static list used to keep cached data
        protected Dictionary<object, T> refCache;
        public bool AutomaticallyRefreshCacheIfKeyNotFound { get; set; } = true;

        public CacheableEntityAdapter(Dictionary<object, T> cache, string connectionString)
            : base(connectionString)
        {
            this.refCache = cache ?? throw new ArgumentNullException("Cache cannot be null.");

            this.storedProcedure = null;
            this.storedProcParams = null;
        }

        public CacheableEntityAdapter(Dictionary<object, T> cache, string connectionString, string storedProcedure)
            : this(cache, connectionString, storedProcedure, null)
        {
        }

        public CacheableEntityAdapter(Dictionary<object, T> cache, string connectionString, string storedProcedure, Parameters storedProcedureParameters)
            : base(connectionString)
        {
            if (string.IsNullOrEmpty(storedProcedure))
                throw new ArgumentNullException("Stored procedure name cannot be null or empty.");

            this.refCache = cache ?? throw new ArgumentNullException("Cache cannot be null.");

            this.storedProcedure = storedProcedure;
            this.storedProcParams = storedProcedureParameters;
        }

        public List<T> Load()
        {
            return new List<T>(this.LoadCache().Values);
        }

        private delegate T LoadSingleEntityFromCache(Dictionary<object, T> cache);
        private T LoadSingle(LoadSingleEntityFromCache loadSingleEntityFromCache)
        {
            bool cacheWereInitialized = this.IsCacheInitialized();

            T entity = loadSingleEntityFromCache(this.LoadCache());
            if (entity != null)
                return entity;

            if (cacheWereInitialized == true && this.AutomaticallyRefreshCacheIfKeyNotFound)
            {
                this.ClearCache();
                entity = loadSingleEntityFromCache(this.LoadCache());
                if (entity != null)
                    return entity;
            }

            return null;
        }

        public virtual T Load(object code)
        {
            if (code == null)
                return null;

            // Converting code to string for value types is needed (?) for backward compatibility.

            string strCode = null;
            if (code.GetType().IsValueType)
                strCode = code.ToString();

            return this.LoadSingle(
                delegate (Dictionary<object, T> cache)
                {
                    if (cache.ContainsKey(code))
                        return cache[code];

                    else if (strCode != null && cache.ContainsKey(strCode))
                        return cache[strCode];

                    else return null;
                });
        }

        protected T LoadSingle(Predicate<T> match)
        {
            return this.LoadSingle(
                delegate (Dictionary<object, T> cache)
                {
                    return new List<T>(cache.Values).Find(match);
                });
        }

        /// <summary>
        /// Clears the local cache so that the next time data is requested, cache would be refreshed.
        /// </summary>
        public void ClearCache()
        {
            if (this.refCache != null)
            {
                lock (this.refCache)
                {
                    this.refCache.Clear();
                }
            }
        }

        public virtual void AppendToCache(T entity)
        {
            if (this.refCache != null && IsCacheInitialized())
            {
                lock (this.refCache)
                {
                    if (this.refCache.ContainsKey(this.GetEntityKey(entity)))
                        this.refCache[this.GetEntityKey(entity)] = entity;
                    else
                        this.refCache.Add(this.GetEntityKey(entity), entity);
                }
            }
        }

        public virtual void RemoveFromCache(T entity)
        {
            if (this.refCache != null && IsCacheInitialized())
            {
                lock (this.refCache)
                {
                    this.refCache.Remove(this.GetEntityKey(entity));
                }
            }
        }
        
        protected bool IsCacheInitialized()
        {
            return this.refCache.Any();
        }

        /// <summary>
        /// Loads the cache from the database.
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, T> LoadCache()
        {
            lock (this.refCache)
            {
                if (!this.refCache.Any())
                {
                    List<T> entities = this.GetEntitiesForCache();

                    Dictionary<object, T> cache = CreateCache(entities.Count);
                    foreach (T entity in entities)
                    {
                        cache.Add(this.GetEntityKey(entity), entity);
                    }

                    this.refCache = cache;

                    foreach (T entity in entities)
                    {
                        this.CachedEntityIntialization(entity);
                    }
                }
                return this.refCache;
            }
        }

        protected virtual Dictionary<object, T> CreateCache(int initialCount)
        {
            return new Dictionary<object, T>(initialCount);
        }

        protected virtual List<T> GetEntitiesForCache()
        {
            if (string.IsNullOrEmpty(this.storedProcedure))
                throw new InvalidOperationException(string.Format("Stored procedure name cannot be null or empty. Either provide stored procedure name in {0} constructor or override GetEntitiesForCache method.", this.GetType().Name));

            List<T> entities = new List<T>();
            this.LoadList(ref entities, this.storedProcedure, this.storedProcParams);
            return entities;
        }

        public virtual object GetEntityKey(T entity)
        {
            if (entity == null) return null;
            return entity.GetEntityKey();
        }

        protected virtual void CachedEntityIntialization(T entity)
        {
        }
    }
}
