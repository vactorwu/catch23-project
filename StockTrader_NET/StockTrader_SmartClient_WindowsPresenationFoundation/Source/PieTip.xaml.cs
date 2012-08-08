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

namespace StockTrader
{
	public partial class PieTip
	{
        Storyboard FadeIn;
        Storyboard FadeOut;

        bool FadingIn;
        bool FadingOut;

		public PieTip()
		{
			this.InitializeComponent();
            IsHitTestVisible = false;
            FadingIn = FadingOut = false;

            FadeIn = (Storyboard)FindResource("FadeIn");
            FadeOut = (Storyboard)FindResource("FadeOut");
            FadeIn.Completed += new EventHandler(FadeIn_Completed);
            FadeOut.Completed += new EventHandler(FadeOut_Completed);
		}

        public void setData(List<PieElement> data)
        {
            Graph.setData(data);
        }

        void FadeOut_Completed(object sender, EventArgs e)
        {
            FadingOut = false;
        }

        void FadeIn_Completed(object sender, EventArgs e)
        {
            FadingIn = false;
        }

        public void Select(int id)
        {
            Graph.SelectPiece(id);
            if (id == -1)
                return;
            Message.Text = Graph.Data[id].label + " " + string.Format("{0:C}", Graph.Data[id].value) + "/" + Math.Round(Graph.Data[id].value / Graph.Sum*100, 2) + "%";
        }

        public void Show( bool t )
        {
            if (t)
            {
                if (FadingIn) return;
                FadeIn.Begin(this);
                FadingIn = true;
                FadingOut = false;
            }
            else
            {
                if (FadingOut) return;
                FadeOut.Begin(this);
                FadingOut = true;
                FadingIn = false;
            }
        }
	}
}