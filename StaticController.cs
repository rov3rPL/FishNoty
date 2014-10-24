using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Szotar;
using WPFGrowlNotification;

namespace FishNoty
{
    public static class StaticController
    {
        private const double topOffset = 20;
        private const double leftOffset = 340;
        private static GrowlNotifiactions growlNotifications;
        //private static readonly GrowlNotifiactions growlNotifications = new GrowlNotifiactions()
        //{
        //    Top = 0, //SystemParameters.WorkArea.Top + topOffset,
        //    Left = 0 //SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset
        //};
        //growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
        //growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;

        private static System.Timers.Timer notificationsTimer;
        private static SynchronizationContext uiContext;

        private static int i = 0;
        private static WordList wordList;

        public static void AddNotificationsSource(WordList list)
        {
            // REDO: for multiple sources of notifications
            wordList = list;

            if (notificationsTimer == null)
            {
                notificationsTimer = new System.Timers.Timer();
                notificationsTimer.Interval = 1200; //1000*60;
                notificationsTimer.AutoReset = false;
                notificationsTimer.Elapsed += notificationsTimer_Elapsed;
                notificationsTimer.Start();

                uiContext = SynchronizationContext.Current;

                uiContext.Send(ShowNotification, "test");
            }
            
        }

        static void notificationsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            uiContext.Send(ShowNotification, "test");
            //ShowNotification();

            notificationsTimer.Start();
        }

        private static void ShowNotification(object state)
        {
            if (growlNotifications == null)
            {
                growlNotifications = new GrowlNotifiactions()
                {
                    Top = SystemParameters.WorkArea.Top + topOffset,
                    Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset
                };
            }

            WordListEntry entry = wordList[i % wordList.Count];
            growlNotifications.AddNotification(new Notification
            {
                Title = entry.Phrase
                ,
                //ImageUrl = "pack://application:,,,/Resources/notification-icon.png"
                ImageUrl = "pack://application:,,,/Resources/quizlet.png"
                ,
                Message = entry.Translation
            });

            ++i;
        }

        
    }
}
