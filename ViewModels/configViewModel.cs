using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
//using WPFDatagridCustomization.Model;

namespace FishNoty.ViewModels
{
    public class Temperatures
    {
        public Temperatures(string state, string city, double max, double min)
        {
            State = state;
            City = city;
            MaxTemperature = max;
            MinTemperature = min;
        }
        public string State { get; set; }
        public string City { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public string Description { get; set; }
    }

    public class configViewModel : INotifyPropertyChanged
    {
        public configViewModel()
        {
            TemperatureCollection = new ObservableCollection<Temperatures>();
            BindData();
        }

        private ObservableCollection<Temperatures> temperatureCollection;
        public ObservableCollection<Temperatures> TemperatureCollection
        {
            get { return temperatureCollection; }
            set
            {
                temperatureCollection = value;
                RaisePropertyChanged("TemperatureCollection");
            }
        }


        private void BindData()
        {
            TemperatureCollection.Add(new Temperatures("Gujarat", "Ahmedabad", 42.32, 25.36) { Description = "This city has a warm dry climate, except for rainy seasons. The place time to visit the place is from October to March.\n October to March months are cool with pleasant atmosphere and perfect for outings and participations in celebrations. June to September are usually typically avoided by visitors, due to uncertainty in heavy rains " });
            TemperatureCollection.Add(new Temperatures("Gujarat", "Surat", 39.45, 22.30) { Description = "This city has a warm dry climate, except for rainy seasons. The place time to visit the place is from October to March.\n October to March months are cool with pleasant atmosphere and perfect for outings and participations in celebrations. June to September are usually typically avoided by visitors, due to uncertainty in heavy rains " });
            TemperatureCollection.Add(new Temperatures("Gujarat", "Vadodara", 40.13, 26.75) { Description = "This city has a warm dry climate, except for rainy seasons. The place time to visit the place is from October to March.\n October to March months are cool with pleasant atmosphere and perfect for outings and participations in celebrations. June to September are usually typically avoided by visitors, due to uncertainty in heavy rains " });
            TemperatureCollection.Add(new Temperatures("Maharashtra", "Mumbai", 40.40, 23.23) { Description = "This city has a warm dry climate, except for rainy seasons. The place time to visit the place is from October to March.\n October to March months are cool with pleasant atmosphere and perfect for outings and participations in celebrations. June to September are usually typically avoided by visitors, due to uncertainty in heavy rains " });
            TemperatureCollection.Add(new Temperatures("Maharashtra", "Pune", 40.69, 23.10) { Description = "This city has a warm dry climate, except for rainy seasons. The place time to visit the place is from October to March.\n October to March months are cool with pleasant atmosphere and perfect for outings and participations in celebrations. June to September are usually typically avoided by visitors, due to uncertainty in heavy rains " });
            TemperatureCollection.Add(new Temperatures("Karnataka", "Banglore", 37.15, 20.06) { Description = "This city has a warm dry climate, except for rainy seasons. The place time to visit the place is from October to March.\n October to March months are cool with pleasant atmosphere and perfect for outings and participations in celebrations. June to September are usually typically avoided by visitors, due to uncertainty in heavy rains " });
            TemperatureCollection.Add(new Temperatures("Andhra Pradesh", "Hyderabad", 43.05, 28.08) { Description = "This city has a warm dry climate, except for rainy seasons. The place time to visit the place is from October to March.\n October to March months are cool with pleasant atmosphere and perfect for outings and participations in celebrations. June to September are usually typically avoided by visitors, due to uncertainty in heavy rains " });
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
