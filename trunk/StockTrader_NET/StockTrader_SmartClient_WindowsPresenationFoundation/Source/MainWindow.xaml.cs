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
using System.Reflection;
 
namespace StockTrader
{
	public partial class MainWindow
	{
        SectionTransition LastTransition;
        static public MainWindow Instance;

        private Storyboard Build;

        public MainWindow()
		{
			this.InitializeComponent();
            
            Instance = this;

            Build = (Storyboard)FindResource("build");
            Build.Completed += new EventHandler(Build_Completed);

            NavBar.ChangeSection += new EventHandler(onChangeSection);
		}

        void Build_Completed(object sender, EventArgs e)
        {
            Nav.SetSection(0);
        }

        void onChangeSection(object sender, EventArgs e)
        {
            try
            {
                if (NavBar.SelectedItem.Content != null)
                {
                    SectionTransition transition = new SectionTransition(NavBar.SelectedItem.Content);
                    Contents.Children.Add(transition);
                    if (LastTransition != null)
                        LastTransition.Close();
                    LastTransition = transition;
                }
                else
                {
                    SectionTransition transition = new SectionTransition(Activator.CreateInstance(NavBar.SelectedItem.ContentClass) as Section);
                    Contents.Children.Add(transition);
                    if (LastTransition != null)
                        LastTransition.Close();
                    LastTransition = transition;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public UIElementCollection Children
        {
            get
            {
                return Contents.Children;
            }
        }
	}
}