
using System;
using System.Windows;
using VirtualEarthWPFControl;

namespace VirtualEarthWPFControlDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.map.OnKeyPress += new VEEventHandler(map_OnKeyPress);
        }

        void map_OnKeyPress(object sender, VEEventArgs e)
        {
            textBox1.Text += Convert.ToChar(e.KeyCode);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.map.Find(textBox1.Text);
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            this.map.HideDashboard();
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if(this.map.MapLoaded) //ignore the component initialization
                this.map.ShowDashboard();
        }

       
    }
}
