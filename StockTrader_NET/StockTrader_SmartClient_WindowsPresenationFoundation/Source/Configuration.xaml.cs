using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Configuration;
using System.Security;
using System.Security.Permissions;
using Trade.StockTraderWebApplicationServiceClient;
using Trade.BusinessServiceDataContract;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace StockTrader
{
    public static class CertCheck
    {
        public static bool EasyCertCheck(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error) { return true; }
    }

    public class ConfigInformation
    {
        public string url;
        public bool isWebSphere=false;
        public static bool isAzure=false;
        public int selected;
        public static string userName;
        public static string password;

        public ConfigInformation()
        {
            if (userName==null)
                userName = System.Configuration.ConfigurationManager.AppSettings["BSL_USER"];
            if (password==null)
                password = System.Configuration.ConfigurationManager.AppSettings["BSL_PASSWORD"];
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(CertCheck.EasyCertCheck);
            
        }
    }

	public partial class Configuration
	{
        //public static bool isWebSphere = false;
        const int WEBSPHERE = 1;
        const int NET_SVC = 2;
        const int NET_IIS = 3;
        const int NET_AZURE = 4;
       

        Alert AlertBox;
        string CustomConfigFile;
        //string LastURL;

        public static ConfigInformation info = new ConfigInformation();

		public Configuration()
		{
			this.InitializeComponent();
            CustomConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\StockTrader.xml";
            if (File.Exists(CustomConfigFile))
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ConfigInformation));
                    TextReader reader = new StreamReader(CustomConfigFile);
                    info = ser.Deserialize(reader) as ConfigInformation;
                    reader.Close();
                }
                catch (Exception)
                {
                    if( info != null )
                        info.url = null;
                }
            }
            WebSphere.Group = "Server";
            NET.Group = "Server";
            NETIIS.Group = "Server";
            NETAZURE.Group = "Server";
            LabelWebSphere.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LabelWebSphere_MouseLeftButtonDown);
            LabelNET.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LabelNET_MouseLeftButtonDown);
            LabelNETIIS.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LabelNETIIS_MouseLeftButtonDown);
            LabelNETAZURE.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LabelNETAZURE_MouseLeftButtonDown);
            WebSphere.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LabelWebSphere_MouseLeftButtonDown);
            NET.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LabelNET_MouseLeftButtonDown);
            NETIIS.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LabelNETIIS_MouseLeftButtonDown);
            NETAZURE.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LabelNETAZURE_MouseLeftButtonDown);
            URL.Text = System.Configuration.ConfigurationManager.AppSettings[".NET"];
            URL.TextChanged += new TextChangedEventHandler(URL_TextChanged);

            Connect.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(onClickConnect);

            if (info != null)
            {
                switch (info.selected)
                {
                    case WEBSPHERE:
                        WebSphere.Selected = true;
                        break;
                    case NET_SVC:
                        NET.Selected = true;
                        break;
                    case NET_IIS:
                        NETIIS.Selected = true;
                        break;
                    case NET_AZURE:
                        NETAZURE.Selected = true;
                        break;
                }
                if (info.url != null)
                {
                    URL.Text = info.url;
                    //AttemptConnect();
                }
            }
            else
                NET.Selected = true;
            
		}

        void LabelNETAZURE_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NETAZURE.Selected = true;
            URL.Text = System.Configuration.ConfigurationManager.AppSettings[".NET_AZURE"];
            info.selected = NET_AZURE;
        }

        

        void LabelNETIIS_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NETIIS.Selected = true;
            URL.Text = System.Configuration.ConfigurationManager.AppSettings[".NET_IIS"];
            info.selected = NET_IIS;
        }

        void URL_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        void AttemptConnect()
        {
            if (info == null) return;
            Login login = new Login();
            login.onClickLogoff(null,null);
            Login.UserInfo = null;
            BusinessServiceClient.Address = URL.Text;
            info.isWebSphere = WebSphere.Selected;
            ConfigInformation.isAzure = NETAZURE.Selected;
            try
            {
                App.BSL = new BusinessServiceClient();
                if (info != null)
                {
                    switch (info.selected)
                    {
                        case WEBSPHERE:
                            App.BSL.getQuote("s:1");
                            break;

                        default:
                            App.BSL.isOnline();
                            break;
                    }
                }
                AlertMessage("Succesfully connected to " + URL.Text );

                //if (info.url == null || !info.url.Equals(URL.Text))
                {
                    info.url = URL.Text;
                    try
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(ConfigInformation));
                        TextWriter writer = new StreamWriter(CustomConfigFile);
                        ser.Serialize(writer, info);
                        writer.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

            }
            catch(Exception ex)
            {
                App.BSL = null;
                AlertMessage("Failed to connect to " + URL.Text + ". Exception: " + ex.ToString());
            }
        }

        void onClickConnect(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AttemptConnect();
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

        void LabelWebSphere_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WebSphere.Selected = true;
            URL.Text = System.Configuration.ConfigurationManager.AppSettings["WebSphere"];
            info.selected = WEBSPHERE;
        }

        void LabelNET_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NET.Selected = true;
            URL.Text = System.Configuration.ConfigurationManager.AppSettings[".NET"];
            info.selected = NET_SVC;
        }

        public override void onSectionReady()
        {
            if (info != null)
            {
                switch (info.selected)
                {
                    case WEBSPHERE:
                        {
                            WebSphere.Selected = true;
                            break;
                        }
                    case NET_SVC:
                        NET.Selected = true;
                        break;
                    case NET_IIS:
                        NETIIS.Selected = true;
                        break;
                    case NET_AZURE:
                        NETAZURE.Selected = true;
                        break;
                }
                if (info.url != null)
                {
                    URL.Text = info.url;
                    //AttemptConnect();
                }
            }
            else
                NET.Selected = true;
        }
	}
}