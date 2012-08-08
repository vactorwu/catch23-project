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
	public partial class Register
	{
        Alert AlertBox;

		public Register()
		{
			this.InitializeComponent();

            RegisterBtn.Label = "REGISTER";
            RegisterBtn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(RegisterBtn_MouseLeftButtonDown);
            
		}

        void RegisterBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Name.Text.Length == 0)
                AlertMessage("Please enter your full name.");
            else if (Address.Text.Length == 0)
                AlertMessage("Please enter your address.");
            else if (UserID.Text.Length == 0)
                AlertMessage("Please enter a user id.");
            else if (Email.Text.Length == 0)
                AlertMessage("Please enter your email address.");
            else if (Password.Password.Length == 0)
                AlertMessage("Please enter a password.");
            else if (!Password.Password.Equals(Confirm.Password))
                AlertMessage("Passwords do not match.");
            else
            {
                decimal balance = 0;
                try
                {
                    balance = Convert.ToDecimal(Balance.Text);
                    if (balance > 10000000 || balance < 1000)
                        AlertMessage("Enter a value between $1,000 and $10,000,000.");
                    else
                    {
                        try
                        {
                            AccountDataModel customer = App.BSL.register(UserID.Text, Password.Password, Name.Text, Address.Text, Email.Text, CreditCard.Text, balance);
                            App.timer.Start();
                            Nav.SetSection(8);
                        }
                        catch(Exception)
                        {
                            AlertMessage("Register failed, please try again with a different username.");
                        }
                    }
                }
                catch(Exception)
                {
                    AlertMessage("Opening balance is not a valid number.");
                }
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
        }
	}
}