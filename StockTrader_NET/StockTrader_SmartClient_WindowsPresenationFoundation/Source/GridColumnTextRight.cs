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
    class GridColumnTextRight : GridColumn
    {
        public override FieldGrid addField(GridColumnData data)
        {
            if (dataGrid == null) return null;
            FieldGrid control = new FieldGrid();
            TextBlock text = new TextBlock();
            text.TextAlignment = TextAlignment.Right;
            text.TextWrapping = TextWrapping.Wrap;
            text.Text = data.value;
            text.FontFamily = new FontFamily("Trebuchet MS");
            text.Margin = new Thickness(10, dataGrid.RowHeight / 2 - text.FontSize / 2, 10, 0);
            control.Children.Add(text);
            return control;
        }
    }
}
