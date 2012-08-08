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
    class GridColumnSell : GridColumn
    {
        public override FieldGrid addField(GridColumnData data)
        {
            if (dataGrid == null) return null;
            FieldGrid control = new FieldGrid();
            control.IsHitTestVisible = true;
            SellButton button = new SellButton();
            button.Label = data.value;
            if (data.value.Equals(""))
                button.Visibility = Visibility.Hidden;
            button.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(button_MouseLeftButtonDown);
            control.Children.Add(button);
            return control;
        }

        void button_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dataGrid.DoSellRow();
        }
    }
}
