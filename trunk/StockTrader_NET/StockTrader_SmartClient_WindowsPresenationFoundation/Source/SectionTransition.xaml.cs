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
	public partial class SectionTransition
	{
        //Storyboard Enter;
        //Storyboard Exit;

        bool Exiting;

		public SectionTransition( Section contents )
		{
			this.InitializeComponent();

            Exiting = false;
            contents.Cleanup();
            LayoutRoot.Children.Add(contents);
            contents.ChangeParents += new EventHandler(onContentsChangeParents);

            //Enter = (Storyboard)FindResource("Enter");
            //Exit = (Storyboard)FindResource("Exit");
            //Exit.Completed += new EventHandler(onClosed);
            //Enter.Completed += new EventHandler(onEntered);
            Loaded += new RoutedEventHandler(onLoaded);

            RenderTransform = new TranslateTransform( 0, -500 );

//            translate.BeginAnimation(TranslateTransform3D.OffsetZProperty, daz);
		}

        void onContentsChangeParents(object sender, EventArgs e)
        {
            if( LayoutRoot.Children.Contains(sender as Section) )
                LayoutRoot.Children.Remove(sender as Section);
        }

        void onEntered(object sender, EventArgs e)
        {
            (LayoutRoot.Children[0] as Section).onSectionReady();
        }

        void onLoaded(object sender, RoutedEventArgs e)
        {
            (LayoutRoot.Children[0] as Section).onSectionRemove();
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            da.Duration = TimeSpan.FromMilliseconds(300);
            da.KeyFrames.Add(new SplineDoubleKeyFrame(100, TimeSpan.FromMilliseconds(300), new KeySpline(0.0, 0.6, 0.0, 0.9)));
            da.Completed += new EventHandler(onEntered);
            RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);
        }

        public SectionTransition()
        {
            this.InitializeComponent();
        }

        public void Close()
        {
            (LayoutRoot.Children[0] as Section).onSectionRemove();
            //Exit.Begin(this);
            Exiting = true;

            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            da.Duration = TimeSpan.FromMilliseconds(300);
            da.KeyFrames.Add(new SplineDoubleKeyFrame(700, TimeSpan.FromMilliseconds(300), new KeySpline(0.0, 0.6, 0.0, 0.9)));
            da.Completed += new EventHandler(onClosed);
            RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);
        }

        void onClosed(object sender, EventArgs e)
        {
            if (Exiting)
                MainWindow.Instance.Children.Remove(this);
            //(this.Parent as Grid).Children.Remove(this);
        }
	}
}