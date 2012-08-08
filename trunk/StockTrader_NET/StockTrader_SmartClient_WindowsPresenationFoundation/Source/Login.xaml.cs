using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Trade.BusinessServiceDataContract;

namespace StockTrader
{
	public partial class Login
	{
        public Alert AlertBox;
        public static AccountDataModel UserInfo;
        public static DateTime LoginTime;

		public Login()
		{
			this.InitializeComponent();

            LoginBtn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(onClickLogin);
            RegisterBtn.Label = "REGISTER";
            LogoffBtn.Label = "LOGOUT";
            RegisterBtn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(onClickRegister);
            LogoffBtn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(onClickLogoff);
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(onVisibleChanged);

            setVisibility();
		}

        public void onVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (App.BSL == null)
            {
                AlertMessage("Not Connected to a Server. Configure first!");
                AlertBox.Visibility = Visibility.Visible;
            }
            else
                if (AlertBox!=null)
                    AlertBox.Visibility = Visibility.Hidden;
        }

        public void onClickLogoff(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (App.BSL == null)
                {
                    AlertMessage("Not Connected to a Server. Configure first!");
                    AlertBox.Visibility = Visibility.Visible;
                }
                else
                {
                    if (AlertBox!=null)
                        AlertBox.Visibility = Visibility.Hidden;
                    if (UserInfo!=null && UserInfo.profileID!=null)
                        App.BSL.logout(UserInfo.profileID);
                    App.timer.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            UserInfo = null;
            Username.Text = null;
            Password.Password = null;
            setVisibility();
        }

        void setVisibility()
        {
            LoginForm.Visibility = UserInfo == null ? Visibility.Visible : Visibility.Hidden;
            LogoffBtn.Visibility = UserInfo != null ? Visibility.Visible : Visibility.Hidden;
        }

        void onClickRegister(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Nav.setRegisterSection();
        }

        void onClickLogin(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                UserInfo = null;
                if (App.BSL == null)
                {
                    AlertMessage("Not Connected to a Server. Configure first!");
                    AlertBox.Visibility = Visibility.Visible;
                }
                else
                {
                    UserInfo = App.BSL.login(Username.Text, Password.Password);
                    if (UserInfo == null)
                    {
                        AlertMessage("Login failed.  Check username/password.");
                    }
                    else
                    {
                        if (AlertBox!=null)
                            AlertBox.Visibility = Visibility.Hidden;
                        LoginTime = DateTime.Now;
                        Nav.SetSection(1);
                        App.timer.Start();
                    }
                    setVisibility();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        

        void AlertMessage(string m)
        {
            if (AlertBox == null)
            {
                AlertBox = new Alert();
                AlertBox.RenderTransform = new TranslateTransform(0, -100);
                LayoutRoot.Children.Add(AlertBox);


                DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
                da.Duration = TimeSpan.FromMilliseconds(300);
                da.KeyFrames.Add(new SplineDoubleKeyFrame(0, TimeSpan.FromMilliseconds(300), new KeySpline(0.0, 0.6, 0.0, 0.9)));
                //da.Completed += new EventHandler(onEntered);
                AlertBox.RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);

                da = new DoubleAnimationUsingKeyFrames();
                da.Duration = TimeSpan.FromMilliseconds(300);
                da.KeyFrames.Add(new SplineDoubleKeyFrame(100, TimeSpan.FromMilliseconds(300), new KeySpline(0.0, 0.6, 0.0, 0.9)));
                //da.Completed += new EventHandler(onEntered);
                Pane.RenderTransform = new TranslateTransform();
                Pane.RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);
            }
            AlertBox.Text = m;
            AlertBox.Visibility = Visibility.Visible;
        }

	}
}