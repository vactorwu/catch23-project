using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Collections.Generic;
using StockTrader.Graphing;
using Trade.BusinessServiceDataContract;

namespace StockTrader
{
	public partial class Account
	{
        Alert AlertBox;

		public Account()
		{
			this.InitializeComponent();

            SaveBtn.Label = "SAVE";
            SaveBtn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(SaveBtn_MouseLeftButtonDown);
		}

        void SaveBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Email.Text.Length == 0)
                AlertMessage("Please enter an email address.");
            else if (CreditCard.Text.Length == 0)
                AlertMessage("Please enter a credit card number.");
            else if (Address.Text.Length == 0)
                AlertMessage("Please enter an address.");
            else if (Name.Text.Length == 0)
                AlertMessage("Please enter your full name.");
            else if (Password.Password.Length == 0)
                AlertMessage("Please enter a password.");
            else if (!Password.Password.Equals(Confirm.Password))
                AlertMessage("Passwords do not match.");
            else
            {
                try
                {
                    AccountProfileDataModel serviceLayerCustomerProfile = new AccountProfileDataModel(Login.UserInfo.profileID, Password.Password, Name.Text, Address.Text, Email.Text, CreditCard.Text);
                    serviceLayerCustomerProfile = App.BSL.updateAccountProfile(serviceLayerCustomerProfile);
                    AlertMessage("Account updated.");
                    Update.Text = "as of " + DateTime.Now;
                }
                catch (Exception ex)
                {
                    AlertMessage("Failed to update account - " + ex.Message);
                }
            }
        }

        public override void onSectionReady()
        {
            try
            {
                if (App.BSL == null || Login.UserInfo == null)
                {
                    this.Visibility = Visibility.Hidden;
                    return;
                }

                AccountDataModel customer = App.BSL.getAccountData(Login.UserInfo.profileID);
                AccountProfileDataModel customerprofile = App.BSL.getAccountProfileData(Login.UserInfo.profileID);

                AccountID.Text = customer.accountID.ToString();
                Username.Text = customer.profileID;
                AccountID.Text = customer.accountID.ToString();
                CreationDate.Text = customer.creationDate.ToString("f");
                LoginCount.Text = customer.loginCount.ToString();
                OpenBalance.Text = string.Format("{0:C}", customer.openBalance);
                Balance.Text = string.Format("{0:C}", customer.balance);
                LogoutCount.Text = customer.logoutCount.ToString();
                LastLogin.Text = customer.lastLogin.ToString("f");
                //Password.Password = customerprofile.password;
                //Confirm.Password = customerprofile.password;
                Email.Text = customerprofile.email;
                Username.Text = customer.profileID;
                Name.Text = customerprofile.fullName;
                Address.Text = customerprofile.address;
                CreditCard.Text = customerprofile.creditCard;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Update.Text = "as of " + DateTime.Now;
            this.Visibility = Visibility.Visible;
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