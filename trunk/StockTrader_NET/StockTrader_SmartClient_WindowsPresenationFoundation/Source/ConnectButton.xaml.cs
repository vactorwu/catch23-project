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
    public partial class ConnectButton
    {
        private Storyboard _Over;
        private Storyboard _Out;

        public ConnectButton()
        {
            this.InitializeComponent();

            LayoutRoot.MouseEnter += new System.Windows.Input.MouseEventHandler(onMouseOver);
            LayoutRoot.MouseLeave += new System.Windows.Input.MouseEventHandler(onMouseOff);

            _Over = (Storyboard)FindResource("Over");
            _Out = (Storyboard)FindResource("Out");
        }

        void onMouseOff(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _Out.Begin(this);
        }

        void onMouseOver(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _Over.Begin(this);
        }
    }
}