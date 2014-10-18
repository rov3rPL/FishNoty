using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Szotar;
using Szotar.Quizlet;
using Szotar.WindowsForms;
using WPFGrowlNotification;

namespace FishNoty
{
    /// <summary>
    /// Interaction logic for quick.xaml
    /// </summary>
    public partial class quick : Window
    {
        #region Private Fields

        private const double topOffset = 20;
        private const double leftOffset = 380;
        readonly GrowlNotifiactions growlNotifications = new GrowlNotifiactions();

        //readonly QuizletImporter quizletImporter;
        private WordList wordList;

        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private WindowState m_storedWindowState = WindowState.Normal;

        #endregion

        public quick()
        {
            InitializeComponent();


            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            m_notifyIcon.BalloonTipTitle = "The App";
            m_notifyIcon.Text = "The App";
            m_notifyIcon.Icon = new System.Drawing.Icon("TheAppIcon.ico");
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);



            growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;

            //szotar_quizlet_initialization();
        }

        private void szotar_quizlet_initialization()
        {
            Szotar.LocalizationProvider.Default = new Szotar.WindowsForms.LocalizationProvider();

            try
            {
                DataStore.InitializeDatabase();
            }
            catch (Szotar.Sqlite.DatabaseVersionException e)
            {
                Errors.NewerDatabaseVersion(e);
                return;
            }
            catch (System.IO.FileNotFoundException e)
            {
                if (e.FileName.StartsWith("System.Data.SQLite"))
                    Errors.DllNotFound("System.Data.SQLite.dll", e);
                else
                    Errors.FileNotFound(e);
                return;
            }
            catch (System.IO.IOException e)
            {
                Errors.CannotOpenDatabase(e);
                return;
            }
            catch (System.Data.Common.DbException e)
            {
                Errors.CannotOpenDatabase(e);
                return;
            }


            QuizletImporter quizletImporter = new QuizletImporter();
            quizletImporter.Completed += quizletImporter_Completed;
            quizletImporter.Set = 1883437; // some hungarian word list as sample
            quizletImporter.BeginImport();
        }

        void quizletImporter_Completed(object sender, Szotar.ImportCompletedEventArgs<Szotar.WordList> e)
        {
            wordList = e.ImportedObject;

            
        }

        private int ix = -1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (wordList == null) return;

            WordListEntry entry = wordList[(++ix) % wordList.Count];

            growlNotifications.AddNotification(new Notification { Title = entry.Phrase, ImageUrl = "pack://application:,,,/Resources/notification-icon.png", Message = entry.Translation });
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

        // show search window
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            search searchWnd = new search();
            searchWnd.Show();
        }

        // show home window
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            home homeWnd = new home();
            homeWnd.Show();
        }

    }
}
