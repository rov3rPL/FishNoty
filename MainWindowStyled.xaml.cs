using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using FishNoty.ViewModels;
using System.Windows.Controls;
using System;
using FishNoty.Views;


namespace FishNoty
{
    public partial class MainWindowStyled : Window
    {
        //public MainWindowStyled([Dependency]MainWindowViewModel viewModel)
        //{
        //    InitializeComponent();
        //    this.DataContext = viewModel;
        //    this.Loaded += (s, e) => { (viewModel as IViewLoadedAware).CreateItems(); };
        //}

        public MainWindowStyled()
        {
            InitializeComponent();

            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            m_notifyIcon.BalloonTipTitle = "FishNoty";
            m_notifyIcon.Text = "The FishNoty App";
            m_notifyIcon.Icon = new System.Drawing.Icon("fish2.ico");
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);
        }


        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private WindowState m_storedWindowState = WindowState.Normal;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            switch (item.Header.ToString())
            {
                case "Home":
                    contentControl.Content = new home();
                    break;
                case "Search":
                    contentControl.Content = new search();
                    break;
                case "Statistics":
                    contentControl.Content = new statistics();
                    break;
                case "Config":
                    contentControl.Content = new configView();
                    break;
                case "homeView":
                    contentControl.Content = new homeView();
                    break;
                case "searchView":
                    contentControl.Content = new searchView();
                    break;
            }

            this.Title = "FishNoty - " + item.Header.ToString();
        }

        #region Window actions

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_notifyIcon.Dispose();
            m_notifyIcon = null;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (m_notifyIcon != null)
                    m_notifyIcon.ShowBalloonTip(2000);
            }
            else
                m_storedWindowState = WindowState;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CheckTrayIcon();
        }

        void m_notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }

        #endregion
    }
}
