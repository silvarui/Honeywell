using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using EPE.BusinessLayer;
using System.Diagnostics;

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
            public object[] Values;
        }

        private Dictionary<string, OnlineEditInfo> editableColumns;
        
        //the entity list
        private IList entityList;

        //the entity type
        private Type entityType;

        private int lastWidth = 0;
        private int imgIndex = 0;

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
            
            ////ExpandHorizontallyIfNeeded(); //check if there is need for horizontal scroll
            //dataGrid.MultiSelect = AllowMultiSelect;
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
            
            //fitGridColumnsToScreen = true;
        }

        private void CreateGridColumns(Entity entity)
        {
            dataGrid.Columns.Clear();
            //dataGrid.ColumnHeadersVisible = ShowColumnHeaders;

            //if (!fitGridColumnsToScreen)
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //else
            //    dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            //The real Entity's columns            
            DataGridViewColumn gridColumn;
            OnlineEditInfo editInfo;
            foreach (string colInfo in ColumnInfos)
            {
                gridColumn = null;
                //columnValueType = entity.GetPLColumnType(colInfo.Name);

                editInfo = null;
                if (editableColumns != null && editableColumns.ContainsKey(colInfo))
                {
                    editInfo = editableColumns[colInfo];
                }

                gridColumn = new DataGridViewTextBoxColumn();

                gridColumn.Name = colInfo;
                gridColumn.HeaderText = colInfo;
                //gridColumn.HeaderText = (colInfo.Name != Entity.colGuiSymbol ?
                //                 entity.GetPLColumnTitle(colInfo.Name, Entity.PLTypes.Gui, this.CurrentDS, this.CurrentCtxt) : "");
                gridColumn.ReadOnly = (editInfo == null);

                if (gridColumn.ReadOnly)
                {
                    gridColumn.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }
                
                dataGrid.Columns.Add(gridColumn);
            }
        }

        //populate grid with the initial list of entities
        private void PopulateGrid()
        {
            bool find = false;
           
            if (!find) this.imgIndex = -1;

            if (entityList != null)
            {
                foreach (object entity in entityList)
                    AddEntityNoScroll((Entity)entity);
            }
        }

        //private void ExpandHorizontallyIfNeeded()
        //{
        //    if (!fitGridColumnsToScreen)
        //    {
        //        dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        //        double enlgargementFactor = (double)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / (double)this.ParentForm.Size.Width;
        //        //dirty algorithm to determine the enlargement factor
        //        if (dataGrid.Columns.Count >= 20)
        //            enlgargementFactor *= 1.5;

        //        int missedWidth = 0;
        //        foreach (DataGridViewColumn col in dataGrid.Columns)
        //        {
        //            if (col.Resizable == DataGridViewTriState.True)
        //            {
        //                col.Width = (int)(col.Width * enlgargementFactor) + missedWidth; //enlarge column
        //                missedWidth = 0;
        //            }
        //            else
        //                missedWidth += (int)(col.Width * enlgargementFactor) - col.Width; // This is the width of the not resizable columns
        //        }
        //    }
        //    else
        //    {
        //        int i = 0;
        //        List<int> colIdx = new List<int>();
        //        List<int> colWidth = new List<int>();

        //        //Before change to AutoSize = Fill, I save the width for the Image Coluns
        //        //to restore then after the auto size, to keep then unchangeable and more nice.

        //        foreach (DataGridViewColumn col in dataGrid.Columns)
        //        {
        //            if (col is DataGridViewImageColumn)
        //            {
        //                colIdx.Add(i);
        //                colWidth.Add(col.Width);
        //            }
        //            ++i;
        //        }

        //        //dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        //        for (i = 0; i < colIdx.Count; ++i)
        //        {
        //            dataGrid.Columns[colIdx[i]].Width = GetMaxStateImageWidth(); // colWidth[i];
        //        }

        //        if (showSelectComboBox)
        //        {
        //            dataGrid.Columns[0].Width = 50;
        //        }
        //    }
        //}
        
        //adds an entity to the list
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
                object value = entity[colInfo];
                rowArray[i++] = value;
            }
            return rowArray;
        }
    }

    public class EntityGridViewSort
    {
        public SortOrder SortOrder { get; set; }
        public int SortedColumn { get; set; }
    }
}
