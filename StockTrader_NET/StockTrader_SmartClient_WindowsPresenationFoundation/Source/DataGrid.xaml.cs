using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using StockTrader.Graphing;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Input;

namespace StockTrader
{
    //List<List<GridColumnData>> data = new List<List<GridColumnData>>( 3 );
    public class FieldGrid : Grid
    {
        public int rowId;
        public FieldGrid()
        {
            IsHitTestVisible = false;
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class DataProviderComparer : IComparer<RowDataProvider>
    {

        private SortDirection Direction = SortDirection.Ascending;
        private int SortColumn;
        public DataProviderComparer() : base() { }

        public DataProviderComparer( int idx, SortDirection direction)
        {
            SortColumn = idx;
            Direction = direction;
        }

        public int Compare(RowDataProvider rowA, RowDataProvider rowB)
        {
            /*if (rowA == null && rowB == null)
            {
                return 0;
            }
            else if (rowA == null && rowB != null)
            {
                return (Direction == SortDirection.Ascending) ? -1 : 1;
            }
            else if (rowA != null && rowB == null)
            {
                return (Direction == SortDirection.Ascending) ? 1 : -1;
            }
            else*/
            {
                return (Direction == SortDirection.Ascending) ?
                 rowA[SortColumn].numeric.CompareTo(rowB[SortColumn].numeric) :
                 rowB[SortColumn].numeric.CompareTo(rowA[SortColumn].numeric);
            }
        }
    }

    public class DataProviderAlphaComparer : IComparer<RowDataProvider>
    {

        private SortDirection Direction = SortDirection.Ascending;
        private int SortColumn;
        public DataProviderAlphaComparer() : base() { }

        public DataProviderAlphaComparer(int idx, SortDirection direction)
        {
            SortColumn = idx;
            Direction = direction;
        }

        public int Compare(RowDataProvider rowA, RowDataProvider rowB)
        {
            /*if (rowA == null && rowB == null)
            {
                return 0;
            }
            else if (rowA == null && rowB != null)
            {
                return (Direction == SortDirection.Ascending) ? -1 : 1;
            }
            else if (rowA != null && rowB == null)
            {
                return (Direction == SortDirection.Ascending) ? 1 : -1;
            }
            else*/
            {
                return (Direction == SortDirection.Ascending) ?
                 rowA[SortColumn].value.CompareTo(rowB[SortColumn].value) :
                 rowB[SortColumn].value.CompareTo(rowA[SortColumn].value);
            }
        }
    }

    public class DataProvider : List<RowDataProvider>
    {
        public int SortColumn;

        public DataProvider()
        {
        }

        public DataProvider(IEnumerable<RowDataProvider> rows) : base(rows)
        {
            
        }

        public void SortAlphaOn(int columnIdx, SortDirection direction)
        {
            SortColumn = columnIdx;
            Sort(new DataProviderAlphaComparer(columnIdx, direction));
        }

        public void SortOn(int columnIdx, SortDirection direction)
        {
            SortColumn = columnIdx;
            Sort( new DataProviderComparer( columnIdx, direction ) );
        }

        public new void Add(RowDataProvider item)
        {
            item.dataProvider = this;
            base.Add(item);
        }
    }

    public class RowDataProvider : List<GridColumnData>, IComparable<RowDataProvider>
    {
        public DataProvider dataProvider;
        public int id;

        public int CompareTo(RowDataProvider other)
        {
            return other[dataProvider.SortColumn].numeric.CompareTo(this[dataProvider.SortColumn].numeric);
        }
    }

	public partial class DataGrid
	{
        //List<GridColumn> columns;

        public event MouseEventHandler RowMouseMove;
        public event MouseEventHandler RowHighLighted;
        public event EventHandler ColumnSortChanged;
        public event EventHandler SellRow;
        public int HighLightedRow;
        public DataProvider data;
        public double RowHeight;
        public double RowWidth;
        public bool AlternateColors;

        private Rectangle HoverRect;

        public GridColumn SortColumn;

		public DataGrid()
		{
            AlternateColors = true;
            HighLightedRow = -1;
			this.InitializeComponent();
            HoverRect = new Rectangle();
            HoverRect.Fill = new SolidColorBrush(Color.FromArgb(77,198, 229, 255));
            HoverRect.Visibility = Visibility.Hidden;
            /*OuterGlowBitmapEffect glow = new OuterGlowBitmapEffect();
            glow.GlowColor = Color.FromRgb(0, 0, 0);
            glow.GlowSize = 5;
            glow.Opacity = 0.4;
            White.BitmapEffect = glow;

            DropShadowBitmapEffect shadow = new DropShadowBitmapEffect();
            shadow.Color = Color.FromRgb(0, 0, 0);
            shadow.Softness = 0.4;
            shadow.ShadowDepth = 2;
            shadow.Opacity = 0.7;
            BlueBG.BitmapEffect = shadow;*/

            RowHeight = 30;
          

            SizeChanged += new SizeChangedEventHandler(DataGrid_SizeChanged);

            Scroller.MouseMove += new System.Windows.Input.MouseEventHandler(onRowsMouseMove);
            Scroller.MouseLeave += new MouseEventHandler(Scroller_MouseLeave);
            LayoutRoot.MouseDown += new MouseButtonEventHandler(onMouseDown);
		}

        void Scroller_MouseLeave(object sender, MouseEventArgs e)
        {
            int idx = -1;

            bool changed = HighLightedRow != idx;
            HighLightedRow = idx;
            if (changed && RowHighLighted != null)
                RowHighLighted(this, e);
            if (RowMouseMove != null)
                RowMouseMove(this, e);

            if (HighLightedRow == -1)
                HoverRect.Visibility = Visibility.Hidden;
            else
            {
                HoverRect.Width = RowWidth - 5;
                HoverRect.Height = RowHeight;
                HoverRect.Margin = new Thickness(0, HighLightedRow * RowHeight, 0, 0);
                HoverRect.Visibility = Visibility.Visible;
            }
        }

        public void SortAlphabetically()
        {
            data.SortAlphaOn(data.SortColumn, SortColumn.Sort == ColumnSortOrder.Ascending ? SortDirection.Ascending : SortDirection.Descending);
            setData(data);
        }

        public void Sort()
        {
            data.SortOn(data.SortColumn, SortColumn.Sort == ColumnSortOrder.Ascending ? SortDirection.Ascending : SortDirection.Descending);
            setData(data);
        }

        void onMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point move = e.GetPosition(Columns);
            if (move.X < 0 || move.X > Columns.ActualWidth || move.Y < 0 || move.Y > Columns.ActualHeight)
                return;

            for (int i = 0; i < Columns.Children.Count; i++)
            {
                move.X -= (Columns.Children[i] as GridColumn).Width;
                if (move.X <= 0)
                {
                    if (SortColumn != null && SortColumn != Columns.Children[i] as GridColumn)
                        SortColumn.Sort = ColumnSortOrder.Off;

                    SortColumn = Columns.Children[i] as GridColumn;
                    if (SortColumn.Sort == ColumnSortOrder.Ascending)
                        SortColumn.Sort = ColumnSortOrder.Descending;
                    else
                        SortColumn.Sort = ColumnSortOrder.Ascending;

                    data.SortOn(i, SortColumn.Sort == ColumnSortOrder.Ascending ? SortDirection.Ascending : SortDirection.Descending);
                    setData(data);
                    if( ColumnSortChanged != null )
                     ColumnSortChanged(this, null);
                    return;
                }
            }
        }

        void onRowsMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        //    public event EventHandler RowMouseMove;
        //public event EventHandler RowHighLighted;
        //public int HighLightedRow;

            Point move = e.GetPosition(Rows);
            int idx = (int)Math.Floor(move.Y / RowHeight);
            if (move.Y < 0 || move.Y > Rows.Children.Count*RowHeight || move.X < 0 || move.X > Scroller.ActualWidth)
                idx = -1;            
            if (idx > Rows.Children.Count - 1)
                idx = -1;

            bool changed = HighLightedRow != idx;
            HighLightedRow = idx;
            if (changed && RowHighLighted != null )
                RowHighLighted(this, e);
            if( RowMouseMove != null )
                RowMouseMove(this, e);

            if (HighLightedRow == -1)
                HoverRect.Visibility = Visibility.Hidden;
            else
            {
                HoverRect.Width = RowWidth - 5;
                HoverRect.Height = RowHeight;
                HoverRect.Margin = new Thickness(0, HighLightedRow * RowHeight, 0, 0);
                HoverRect.Visibility = Visibility.Visible;
            }
        }

        public void ApplyEffects()
        {
            //OuterGlowBitmapEffect glow = new OuterGlowBitmapEffect();
            //glow.GlowColor = Color.FromRgb(0, 0, 0);
            //glow.GlowSize = 5;
            //glow.Opacity = 0.4;
            //White.BitmapEffect = glow;

            //DropShadowBitmapEffect shadow = new DropShadowBitmapEffect();
            //shadow.Color = Color.FromRgb(0, 0, 0);
            //shadow.Softness = 0.4;
            //shadow.ShadowDepth = 2;
            //shadow.Opacity = 0.7;
            //BlueBG.BitmapEffect = shadow;
        }

        public void RemoveEffects()
        {
            //White.BitmapEffect = 
            //BlueBG.BitmapEffect = null;
        }

        public void setData( DataProvider dat )
        {
            try
            {
                data = dat;
                Rows.Children.RemoveRange(0, Rows.Children.Count);
                RowsBackground.Children.RemoveRange(0, RowsBackground.Children.Count);
                HoverRect.Visibility = Visibility.Hidden;
                if (data == null)
                {
                    RowsBackground.Children.Add(HoverRect);
                    return;
                }
                int r;
                int off = 0;
                for (r = 0; r < data.Count; r++)
                {
                    List<GridColumnData> row = data[r];
                    Canvas rowCanvas = new Canvas();
                    if (row[0].value.Equals(""))
                    {
                        off++;
                        Rectangle rect = new Rectangle();
                        rect.Height = RowHeight;
                        rect.Width = Columns.ActualWidth;
                        rect.Fill = new SolidColorBrush(Color.FromRgb(237, 239, 243));
                        rect.Margin = new Thickness(0, r * RowHeight, 0, 0);
                        RowsBackground.Children.Add(rect);
                    }
                    else if (AlternateColors && (r/*+off*/) % 2 == 1)
                    {
                        Rectangle rect = new Rectangle();
                        rect.Height = RowHeight;
                        rect.Width = Columns.ActualWidth;
                        rect.Fill = new SolidColorBrush(Color.FromRgb(237, 239, 243));
                        rect.Margin = new Thickness(0, r * RowHeight, 0, 0);
                        RowsBackground.Children.Add(rect);
                    }
                    double x = 0;
                    for (int c = 0; c < row.Count; c++)
                    {
                        GridColumn column = Columns.Children[c] as GridColumn;

                        FieldGrid field = column.addField(row[c]);
                        field.rowId = r;
                        if (field == null) continue;
                        //field.RenderTransform = new TranslateTransform( x, 0 );
                        field.Margin = new Thickness(x, 0, 0, 0);
                        field.Width = column.Width;
                        field.Height = RowHeight;
                        rowCanvas.Children.Add(field);
                        x += column.Width;
                    }
                    rowCanvas.Margin = new Thickness(0, r * RowHeight, 0, 0);

                    Rows.Children.Add(rowCanvas);
                }
                RowsBackground.Children.Add(HoverRect);

                Content.Height = r * RowHeight;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        void DataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            for (int i = 0; i < Columns.Children.Count; i++)
                (Columns.Children[i] as GridColumn).Height = Columns.ActualHeight;
            setData(data);
        }

        public void DoSellRow()
        {
            if( SellRow != null )
                SellRow( this, null );
        }

        public void addColumn(GridColumn column)
        {
            try
            {
                RowWidth = 0;
                for (int i = 0; i < Columns.Children.Count; i++)
                {
                    RowWidth += (Columns.Children[i] as GridColumn).Width;
                    (Columns.Children[i] as GridColumn).ShowSeparator = true;
                }

                column.Translation.X = RowWidth;
                column.dataGrid = this;
                Columns.Children.Add(column);
                column.ShowSeparator = false;
                RowWidth += column.Width;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
	}
}