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


namespace FishNoty
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _baseUrl = "https://api.quizlet.com/";
        private string redirectedUrl = "www.snipclip.pl";
        public string Token { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();

            webBrowser.Source = new Uri("https://quizlet.com/authorize?response_type=code&client_id=JupbbrWR6n&scope=read&state=RANDOM_STRING");

            webBrowser.Navigating += webBrowser_Navigating;
        }

        void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (String.Equals(e.Uri.Host, redirectedUrl))
            {
                NameValueCollection prms = HttpUtility.ParseQueryString(e.Uri.Query);
                Token = prms["code"];

                var client = new RestClient(_baseUrl);                
                //if (!String.IsNullOrEmpty(_userName))
                //{
                    //client.Authenticator = new HttpBasicAuthenticator(_userName, _password);
                //}

                var request = new RestRequest("/2.0/search/sets?q=turkish");
                request.AddHeader("Authorization", "Bearer " + Token);
                var response = client.Execute(request);
                var keyResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
                //response.Data;

            }
        }
    }
}
