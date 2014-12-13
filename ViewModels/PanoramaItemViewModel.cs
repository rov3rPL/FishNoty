using System;
//using Publisher.Wpf.Services;

namespace FishNoty.ViewModels
{
    public class PanoramaItemViewModel : INPCBase
    {
        //private IWebSocketInvoker webSocketInvoker;
        private string imageUrl = "";


        public PanoramaItemViewModel(
            //IWebSocketInvoker webSocketInvoker,
            string imageUrl)
        {
            //this.webSocketInvoker = webSocketInvoker;
            this.ImageUrl = imageUrl;
            this.TileClickedCommand = new SimpleCommand<object, object>(TileClickedCommandExecuted);

        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                base.NotifyChanged("ImageUrl");
            }
        }

        public SimpleCommand<object, object> TileClickedCommand { get; private set; }

        public void TileClickedCommandExecuted(object dummy)
        {
            try
            {
                string imageName = ImageUrl.Substring(ImageUrl.LastIndexOf(@"/") + 1);
                //webSocketInvoker.SendNewMessage(imageName);
            }
            catch (Exception)
            {
                //Oh no : In real code we would do something about this
            }
        }
    }

}
