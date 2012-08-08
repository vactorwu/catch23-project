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
	public partial class SellButton
	{
        private Storyboard _Over;
        private Storyboard Out;

		public SellButton()
		{
			this.InitializeComponent();

            LayoutRoot.MouseEnter += new System.Windows.Input.MouseEventHandler(onMouseOver);
            LayoutRoot.MouseLeave += new System.Windows.Input.MouseEventHandler(onMouseOff);

            _Over = (Storyboard)FindResource("Over");
            Out = (Storyboard)FindResource("Out");
		}

        public string Label
        {
            set
            {
                LabelOver.Text = LabelOff.Text = value;
            }
        }
        void onMouseOff(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Out.Begin(this);
            //Off.Visibility = Visibility.Visible;
            //Over.Visibility = Visibility.Hidden;
        }

        void onMouseOver(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _Over.Begin(this);
            //Over.Visibility = Visibility.Visible;
            //Off.Visibility = Visibility.Hidden;
        }
	}
}