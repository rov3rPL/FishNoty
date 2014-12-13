using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using PanoramaControl;
//using Publisher.Wpf.Services;


namespace FishNoty.ViewModels
{
    public interface IViewLoadedAware
    {
        void CreateItems();
    }


    public class MainWindowViewModel : INPCBase, IViewLoadedAware
    {
        private ObservableCollection<PanoramaItemViewModel> items = new ObservableCollection<PanoramaItemViewModel>();
        private IEnumerable<PanoramaGroup> panoramaItems;
        //private IMessageBoxService messageBoxService;
        //private IWebSocketInvoker webSocketInvoker;
        private Random rand = new Random(156577);

        public MainWindowViewModel
            (
                //IMessageBoxService messageBoxService,
                //IWebSocketInvoker webSocketInvoker
            )
        {
            //this.webSocketInvoker = webSocketInvoker;
            //this.messageBoxService = messageBoxService;
           
        }


        public void CreateItems()
        {
            List<string> images = new List<string>();
            //for (int i = 0; i < 60; i++)
            //{
            //    int imageNum = rand.Next(1, 16);
            //    images.Add(string.Format("/images/demo{0}.jpg", imageNum));
            //}

            //List<PanoramaGroup> data = new List<PanoramaGroup>();
            //data.Add(new PanoramaGroup("",
            // CollectionViewSource.GetDefaultView(
            //     images.Select(x => new PanoramaItemViewModel(webSocketInvoker, x)))));

            //PanoramaItems = data;
            //messageBoxService.ShowInformation("Click an image to send it to the web site");
        }




        public IEnumerable<PanoramaGroup> PanoramaItems
        {
            get { return this.panoramaItems; }

            set
            {
                if (value != this.panoramaItems)
                {
                    this.panoramaItems = value;
                    base.NotifyChanged("PanoramaItems");
                }
            }
        }


    }
}
