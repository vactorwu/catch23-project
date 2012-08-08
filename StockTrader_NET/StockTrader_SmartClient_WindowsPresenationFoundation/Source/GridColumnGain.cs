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
    public class GridColumnGainData : GridColumnData
    {
        public int increased;
    }

    class GridColumnGain : GridColumn
    {
        public override FieldGrid addField(GridColumnData data)
        {
            GridColumnGainData d = null;
            if (data is GridColumnGainData)
                d = data as GridColumnGainData;

            if (dataGrid == null) return null;
            FieldGrid control = new FieldGrid();
            TextBlock text = new TextBlock();
            text.TextAlignment = TextAlignment.Right;
            text.TextWrapping = TextWrapping.Wrap;
            text.Text = data.value;
            text.FontFamily = new FontFamily("Trebuchet MS");
            text.Margin = new Thickness(10, dataGrid.RowHeight / 2 - text.FontSize / 2, 22, 0);

            if (d != null)
            {
                if (d.increased == 1)
                {
                    GainIcon icon = new GainIcon();
                    icon.Margin = new Thickness(0, 0, 8, 0);
                    control.Children.Add(icon);
                    text.Foreground = new SolidColorBrush(Color.FromRgb(37, 120, 32));
                }
                else if (d.increased == -1)
                {
                    LossIcon icon = new LossIcon();
                    icon.Margin = new Thickness(0, 0, 8, 0);
                    control.Children.Add(icon);
                    text.Foreground = new SolidColorBrush(Color.FromRgb(191, 0, 0));
                }
                else
                {
                    text.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
            }
            control.Children.Add(text);
            return control;
        }
    }
}
