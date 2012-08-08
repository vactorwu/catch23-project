using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Media.Effects;

namespace StockTrader
{
	public partial class Nav
	{
        public event EventHandler ChangeSection;
        public NavItem SelectedItem;

        public NavItem TradeSection;
        public NavItem OrderSection;
        public NavItem RegisterSection;

        static Nav instance;

        public Nav()
		{
			this.InitializeComponent();
            instance = this;

            NavItem.NumItems = 0;
            Sections.Children.Add(new NavItem("Welcome", typeof(Welcome)));
            Sections.Children.Add(new NavItem("Home", typeof(Home)));
            Sections.Children.Add(new NavItem("Account", typeof(Account)));
            Sections.Children.Add(new NavItem("Portfolio", new Portfolio()));
            Sections.Children.Add(new NavItem("Orders", new Order()));
            Sections.Children.Add(new NavItem("Quotes/Trade", new Quotes()));
            Sections.Children.Add(new NavItem("Config", new Configuration()));
            Sections.Children.Add(new NavItem("Glossary", typeof(Glossary)));
            Sections.Children.Add(new NavItem("Login/Logout", new Login()));

            TradeSection = new NavItem("Trade", typeof(Trade));
            OrderSection = new NavItem("Order", typeof(Order));
            RegisterSection = new NavItem("Register", typeof(Register));

            for (int i = 0; i < Sections.Children.Count; i++)
            {
                (Sections.Children[i] as NavItem).Click += new EventHandler(onClickNav);
            }
		}

        static public void setRegisterSection()
        {
            instance.SelectedItem = instance.RegisterSection;
            instance.ChangeSection(instance, null);
        }

        static public void SetOrderSection()
        {
            instance.SelectedItem = instance.OrderSection;
            instance.ChangeSection(instance, null);
        }

        static public void SetTradeSection()
        {
            instance.SelectedItem = instance.TradeSection;
            instance.ChangeSection(instance, null);
        }

        static public void SetSection(int i)
        {
            (instance.Sections.Children[i] as NavItem).Activate();
        }

        void onClickNav(object sender, EventArgs e)
        {
            SelectedItem = sender as NavItem;
            ChangeSection(this, e);
        }
    }
}