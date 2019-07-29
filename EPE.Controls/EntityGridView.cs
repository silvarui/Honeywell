using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EPE.BusinessLayer;

namespace EPE.Controls
{
    public partial class EntityGridView : UserControl
    {
        private const int MinRowHeight = 20;
        private const int SelectColumnWidth = 50;
        public string[] ColumnInfos { get; set; }

        public EntityGridView()
        {
            InitializeComponent();
        }

        private enum OnlineEditType
        {
            Normal, //either text or check box
            Combo //combo box
        }

        private class OnlineEditInfo
        {
            public string ColumnName;
            public OnlineEditType EditType;
        }

        private Dictionary<string, OnlineEditInfo> editableColumns;
        
        //the entity list
        private IList entityList;

        //the entity type
        private Type entityType;

        private int lastWidth = 0;
        
        //ability to make individual cells read only, not just entire columns;
        IEntityGridViewEnabledCell entityGridViewEnabledCell;

        public void Initialize(Type entType, IList entList)
        {
            Initialize(entType, entList, null, null);
        }

        public void Initialize(Type entType, IList entList, IEntityGridViewEnabledCell entGridViewEnabledCell)
        {
            Initialize(entType, entList, null, null);
        }

        public void Initialize(Type entType, IList entList, EntityGridViewSort entityGridViewSort)
        {
            Initialize(entType, entList, null, entityGridViewSort);
        }

        public void Initialize(Type entType, IList entList, IEntityGridViewEnabledCell entGridViewEnabledCell, EntityGridViewSort entityGridViewSort)
        {
            ClearGridData();

            entityList = entList;
            entityType = entType;
            entityGridViewEnabledCell = entGridViewEnabledCell;

            //get the first entity to use to create headers and retrieve the type
            Entity entity = ((entList != null && entList.Count > 0) ? (Entity)entList[0] : null);
            if (entity == null)
            {
                entity = (Entity)Activator.CreateInstance(entType);
            }
            
            //set the default row height
            dataGrid.RowTemplate.Height = MinRowHeight;

            if (this.lastWidth != GridWidth())
                ColumnInfos = null;

            if (ColumnInfos == null)
                GetDefaultColumnInfos(entity); //reads the entity's column names to be displayed in the gid

            CreateGridColumns(entity); //creates grid's columns
            PopulateGrid(); //populate grid with rows (if the initial entity list passed was not empty)
            
            //if (HasContextMenu)
            //    FillContextMenu(); //fill context menu
            dataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            SelectEntity(null);

            SortGrid(entityGridViewSort);

            this.lastWidth = GridWidth();
        }

        private void SortGrid(EntityGridViewSort sorting)
        {
            if (sorting == null) return;

            switch (sorting.SortOrder)
            {
                case SortOrder.Ascending:
                    dataGrid.Sort(dataGrid.Columns[sorting.SortedColumn], ListSortDirection.Ascending);
                    break;

                case SortOrder.Descending:
                    dataGrid.Sort(dataGrid.Columns[sorting.SortedColumn], ListSortDirection.Descending);
                    break;
            }
        }

        //gets the entity from row
        private Entity FindEntity(DataGridViewRow row)
        {
            return (Entity)row.Tag;
        }

        //finds a row from an entity
        private DataGridViewRow FindRow(Entity entity)
        {
            // check if there is a UEID match
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (entity != null && ((Entity)row.Tag).UEID == entity.UEID)
                    return row;
            }

            // if no UEID matches, use entity's implementation of equals
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (((Entity)row.Tag) == entity)
                    return row;
            }

            return null;
        }

        //this event will be fired when the selection changes in the grid        
        public event EventHandler GridSelectionChanged;

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                GridSelectionChanged?.Invoke(this, e);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void SelectEntity(Entity entity)
        {
            if (entity == null)
            {
                foreach (DataGridViewRow row in dataGrid.Rows)
                {
                    if (row.Selected == true)
                        row.Selected = false;
                }
                dataGrid.CurrentCell = null;
                //apparently, due to a bug in .NET, the SelectionChanged is not fired when the CurrentCell is set to null
                //we must force the call
                OnSelectionChanged(this, null);
            }
            else
            {
                DataGridViewRow row = FindRow(entity);
                if (row != null)
                {
                    row.Selected = true;
                    dataGrid.CurrentCell = row.Cells[2];
                }
            }
        }

        private void ShowCount()
        {
            lblGridText.Text = string.Format("Total de registos: {0}", Count.ToString());
        }

        public int Count
        {
            get
            {
                return dataGrid.Rows.Count;
            }
        }

        //clears all the grid
        private void ClearGridData()
        {
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
        }

        private int GridWidth()
        {
            int gridWidth = dataGrid.Width - SystemInformation.VerticalScrollBarWidth - 2/*from border fixed style*/;

            return gridWidth;
        }

        private void GetDefaultColumnInfos(Entity entity)
        {
            ColumnInfos = entity.GetColumnNames().Where(n => !n.Contains("Id")).ToArray();            
        }

        //sets a column as editable in a text box
        public void SetEditableColumn(string colName)
        {
            if (editableColumns == null)
                editableColumns = new Dictionary<string, OnlineEditInfo>();
            OnlineEditInfo editInfo = new OnlineEditInfo();
            editInfo.ColumnName = colName;
            editInfo.EditType = OnlineEditType.Normal;
            editableColumns[colName] = editInfo;
        }

        private void CreateGridColumns(Entity entity)
        {
            dataGrid.Columns.Clear();
                        
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            //The real Entity's columns            
            DataGridViewColumn gridColumn;
            OnlineEditInfo editInfo;
            foreach (string colInfo in ColumnInfos)
            {
                if (!entity.GetGridViewColumns().Contains(colInfo))
                    continue;

                gridColumn = null;

                editInfo = null;
                if (editableColumns != null && editableColumns.ContainsKey(colInfo))
                {
                    editInfo = editableColumns[colInfo];
                }

                gridColumn = new DataGridViewTextBoxColumn();

                gridColumn.Name = colInfo;
                gridColumn.HeaderText = entity.GetPLTitle(colInfo);

                gridColumn.ReadOnly = (editInfo == null);

                if (gridColumn.ReadOnly)
                {
                    gridColumn.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }
                
                dataGrid.Columns.Add(gridColumn);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            DataGridViewRow row = FindRow(entity);

            if (row == null)
                return;

            dataGrid.Rows.Remove(row);
        }

        public void AddEntity(Entity entity)
        {
            object[] rowArray = CreateEntityRow(entity);

            DataGridViewRow newRow = new DataGridViewRow();
            newRow.CreateCells(dataGrid, rowArray);
            newRow.Tag = entity;
            if (entityGridViewEnabledCell != null)
                entityGridViewEnabledCell.EnableCell(newRow, ColumnInfos);
            dataGrid.Rows.Add(newRow);

            //int rowHeight = CalculateRowHeight();
            //if (rowHeight > 0)
            //    newRow.Height = rowHeight;

            //scroll to the last added row             
            dataGrid.CurrentCell = newRow.Cells[2];
        }

        //populate grid with the initial list of entities
        private void PopulateGrid()
        {
            if (entityList != null)
            {
                foreach (object entity in entityList)
                    AddEntityNoScroll((Entity)entity);
            }
        }

        private void AddEntityNoScroll(Entity entity)
        {
            object[] rowArray = CreateEntityRow(entity);
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.CreateCells(dataGrid, rowArray);

            newRow.Tag = entity;
            if (entityGridViewEnabledCell != null)
                entityGridViewEnabledCell.EnableCell(newRow, ColumnInfos);
            dataGrid.Rows.Add(newRow);
        }

        private object[] CreateEntityRow(Entity entity)
        {
            object[] rowArray = new object[ColumnInfos.Length];
            
            int i = 0;
            foreach (string colInfo in ColumnInfos)
            {
                if (!entity.GetGridViewColumns().Contains(colInfo))
                    continue;

                object value = entity.GetPLValue(colInfo);
                rowArray[i++] = value;
            }
            return rowArray;
        }

        private Dictionary<string, Color> cellSelectionBgColor = new Dictionary<string, Color>();
        private Dictionary<string, Color> cellBgColor = new Dictionary<string, Color>();

        private void OnCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            string cellName = string.Format(e.ColumnIndex.ToString("0000") + e.RowIndex.ToString("0000"));

            cellSelectionBgColor[cellName] = dataGrid.CurrentCell.Style.SelectionBackColor;
            cellBgColor[cellName] = dataGrid.CurrentCell.Style.BackColor;

            if (!dataGrid.CurrentCell.ReadOnly && dataGrid.CurrentCell is DataGridViewCheckBoxCell == false)
            {
                //dataGrid.CurrentCell.Style.BackColor = Color.Yellow;
                dataGrid.CurrentCell.Style.SelectionBackColor = Color.Blue;

                dataGrid.BeginEdit(true);
            }
        }

        private void OnCellLeave(object sender, DataGridViewCellEventArgs e)
        {
            string cellName = string.Format(e.ColumnIndex.ToString("0000") + e.RowIndex.ToString("0000"));

            dataGrid.CurrentCell.Style.BackColor = cellBgColor[cellName];
            dataGrid.CurrentCell.Style.SelectionBackColor = cellSelectionBgColor[cellName];
        }

        //this event will be fired when cell is clicked
        public event DataGridViewCellEventHandler CellClicked;
        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGrid.CurrentCell != null)
                {
                    dataGrid.BeginEdit(true);

                    CellClicked?.Invoke(this, new DataGridViewCellEventArgs(dataGrid.CurrentCell.ColumnIndex, dataGrid.CurrentCell.RowIndex));
                }
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private object cellValueOnBeginEdit;
        private void OnCellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                cellValueOnBeginEdit = null;
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    cellValueOnBeginEdit = dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        //this event will be fired a cell value is changed in the grid
        public event EventHandler<GridValueChangedEventArgs> GridValueChanged;

        public event EventHandler<DataGridViewCellEventArgs> OnCellEditEnded;
        private void OnCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object crtValue = null;
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    crtValue = dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                }
                cellValueOnBeginEdit = null;

                OnCellEditEnded?.Invoke(this, e);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void EntityGridView_Load(object sender, EventArgs e)
        {
            ShowCount();
        }

        private void OnRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ShowCount();
        }

        private void OnRowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            ShowCount();
        }

        public DataGridViewRow GetRow(int rowIndex)
        {
            return dataGrid.Rows[rowIndex];
        }

        public event EventHandler<DataGridViewCellEventArgs> OnCellDoubleClicked;
        private void OnCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGrid.CurrentCell != null)
                {
                    OnCellDoubleClicked?.Invoke(this, new DataGridViewCellEventArgs(dataGrid.CurrentCell.ColumnIndex, dataGrid.CurrentCell.RowIndex));
                }
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }

    public class GridValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridValueChangedEventArgs"/> class.
        /// </summary>
        /// <param name="sourceEntity">The source entity.</param>
        public GridValueChangedEventArgs(Entity sourceEntity)
        {
            SourceEntity = sourceEntity;
        }

        private Entity _sourceEntity;

        /// <summary>
        /// Gets or sets the source entity.
        /// </summary>
        /// <value>The source entity.</value>
        public Entity SourceEntity
        {
            get { return _sourceEntity; }
            set { _sourceEntity = value; }
        }

    }

    public class EntityGridViewSort
    {
        public SortOrder SortOrder { get; set; }
        public int SortedColumn { get; set; }
    }
}
