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
	public partial class QuoteBox
	{
        public event EventHandler ClickGetQuote;
        private Storyboard _Over;
        private Storyboard _Out;

		public QuoteBox()
		{
			this.InitializeComponent();
            GetButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(onClickGetButton);

            GetButton.MouseEnter += new System.Windows.Input.MouseEventHandler(onMouseOver);
            GetButton.MouseLeave += new System.Windows.Input.MouseEventHandler(onMouseOut);

            _Over = (Storyboard)FindResource("Over");
            _Out = (Storyboard)FindResource("Out");
		}

        void onMouseOut(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _Out.Begin(this);
            //Off.Visibility = Visibility.Visible;
            //Over.Visibility = Visibility.Hidden;
        }

        void onMouseOver(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _Over.Begin(this);
            //Over.Visibility = Visibility.Visible;
            //Off.Visibility = Visibility.Hidden;
        }

        void onClickGetButton(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if( ClickGetQuote != null )
                ClickGetQuote(this, e);
        }

        public string Text
        {
            get
            {
                return Input.Text;
            }
            set
            {
                Input.Text = value;
            }
        }
	}
}