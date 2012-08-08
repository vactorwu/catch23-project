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
	public partial class NavItem
	{
        public event EventHandler Click;

        Storyboard Select;
        Storyboard DeSelect;
        public Type ContentClass;
        new public Section Content;
        static NavItem SelectedItem;
        const double ITEM_WIDTH = 90;

        public static int NumItems = 0;

        public NavItem( string label, Type content )
        {
            Initialize();
            Label.Text = label;
            ContentClass = content;
        }

        public NavItem(string label, Section content)
        {
            Initialize();
            Label.Text = label;
            Content = content;
        }

		public NavItem()
		{
            Initialize();
		}

        void Initialize()
        {
            this.InitializeComponent();

            MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(onClick);
            Margin = new Thickness(ITEM_WIDTH * NumItems, 0, 0, 0);
            Select = (Storyboard)FindResource("Select");
            DeSelect = (Storyboard)FindResource("DeSelect");
            NumItems++;
        }

        void onClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedItem == this) return;
            if (SelectedItem != null)
                DeSelect.Begin(SelectedItem);
            Select.Begin(this);
            SelectedItem = this;
            Click(this, e);
        }

        public void Activate()
        {
            if (SelectedItem != this)
            {
                if (SelectedItem != null)
                    DeSelect.Begin(SelectedItem);
                Select.Begin(this);
                SelectedItem = this;
            }
            Click(this, null);
        }
	}
}