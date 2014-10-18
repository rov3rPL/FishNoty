using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Web;
using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;
using FishNoty.Core;
using FishNotificationSources;

namespace FishNoty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _baseUrl = "https://api.quizlet.com/";
        private string clientID = "JupbbrWR6n";
        private string secretKey = "9nhMc2uwMRT59uJZPrFEZF";
        private string redirectedUrl = "localhost";
        public string AuthorizationCode { get; set; }
        public string Token { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();

            Process.Start("https://quizlet.com/authorize?response_type=code&client_id=JupbbrWR6n&scope=read&state=5bukj76nkbjnbac&approval_prompt=force&access_type=offline");
            //Token = "QKEaahvNjQRGhqquBfewJ9GhXYVZTJDectKG6R6Z";
            //NotificationSources sources = new NotificationSources();
            //Set set = sources.GetSet(19553107);

            webBrowser.Source = new Uri("https://quizlet.com/authorize?response_type=code&client_id=JupbbrWR6n&scope=read&state=5bukj76nkbjnbac");

            //Token = "yaecHbnuzb3ppG4u3SJpDXpcXVt2HWvPRX5eRKEQ";
            //tests();

            webBrowser.Navigating += webBrowser_Navigating;
        }

        void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (String.Equals(e.Uri.Host, redirectedUrl))
            {
                NameValueCollection prms = HttpUtility.ParseQueryString(e.Uri.Query);
                AuthorizationCode = prms["code"];

            }
        }

        
    }
    

}
