using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace StockTrader
{
    public class GridColumnData
    {
        public string value;
        public int numeric;
    }

    public enum ColumnSortOrder
    {
        Off,
        Ascending,
        Descending
    }

	public partial class GridColumn
	{
        public TranslateTransform Translation;
        public DataGrid dataGrid;

       

        ColumnSortOrder SortOrder;

		public GridColumn( )
		{
			this.InitializeComponent();
            RenderTransform = Translation = new TranslateTransform();
			// Insert code required on object creation below this point.
            Divider.SizeChanged += new SizeChangedEventHandler(onSizeChanged);
            IsHitTestVisible = false;
		}

        public ColumnSortOrder Sort
        {
            get
            {
                return SortOrder;
            }
            set
            {
                SortOrder = value;
                if (SortOrder == ColumnSortOrder.Off)
                {
                    sortDown.Visibility = sortUp.Visibility = Visibility.Hidden;
                }
                else if (SortOrder == ColumnSortOrder.Ascending)
                {
                    sortDown.Visibility = Visibility.Visible;
                    sortUp.Visibility = Visibility.Hidden;
                }
                else if (SortOrder == ColumnSortOrder.Descending)
                {
                    sortDown.Visibility = Visibility.Hidden;
                    sortUp.Visibility = Visibility.Visible;
                }
            }
        }
        public void onSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Line2.Height = Line1.Height = Divider.ActualHeight;
        }

        public void setLabel( string l )
        {
            Label.Text = l;

            int offset = 0;
            int lines = 0;
            while (offset >= 0)
            {
                offset = l.IndexOf("\n", offset+1);
                lines++;
            }
            Label.Margin = new Thickness(0, 37.0 / 2.0 - lines * Label.FontSize / 2, 0, 0);
        }

        public virtual FieldGrid addField(GridColumnData data)
        {
            if (dataGrid == null) return null;
            FieldGrid control = new FieldGrid();
            TextBlock text = new TextBlock();
            text.TextAlignment = TextAlignment.Center;
            text.TextWrapping = TextWrapping.Wrap;
            text.Text = data.value;
            text.FontFamily = new FontFamily( "Trebuchet MS" );
            text.Margin = new Thickness(10, dataGrid.RowHeight / 2 - text.FontSize / 2, 10, 0);
            control.Children.Add(text);
            return control;
        }

        public bool ShowSeparator
        {
            set
            {
                Divider.Visibility = Sep.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }

        
	}
}