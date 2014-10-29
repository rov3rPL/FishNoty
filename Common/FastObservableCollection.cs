using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace KoiSoft.VSMine.Common
{
    /// <summary>
    /// Kolekcja rozszerzająca ObservableCollection o akcje
    /// AddRange i RemoveAll
    /// </summary>
    /// <typeparam name="T">Typ przetrzymywany w kolekcji</typeparam>
    public class FastObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Flaga określająca, czy informować o zmianach w kolekcji,
        /// czy jeszcze nie
        /// </summary>
        private bool _suspendNotification;

        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public FastObservableCollection()
        {
            _suspendNotification = false;
        }

        /// <summary>
        /// Event pojawia się przy zmianie kolekcji.
        /// </summary>
        /// <param name="e">Argumenty</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suspendNotification)
            {
                base.OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// Pozwala 'wstrzymac' odswiezanie listy
        /// </summary>
        public void SuspendCollectionChangeNotification()
        {
            _suspendNotification = true;
        }

        /// <summary>
        /// Wznawia odświeżanie listy
        /// </summary>
        public void ResumeCollectionChangeNotification()
        {
            _suspendNotification = false;
        }

        /// <summary>
        /// Dodaje do kolekcji zbiór obiektów
        /// </summary>
        /// <param name="items">Zbiór obiektów do wstawienia</param>
        public void AddRange(IEnumerable<T> items)
        {
            this.SuspendCollectionChangeNotification();

            int index = Count;
            try
            {
                foreach (var i in items)
                {
                    InsertItem(index, i);
                }
            }
            finally
            {
                this.ResumeCollectionChangeNotification();
                var arg = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                this.OnCollectionChanged(arg);
            }
        }

        /// <summary>
        /// Usuwa z kolekcji zbiór obiektów
        /// </summary>
        /// <param name="items">Zbiór obiektów do usunięcia</param>
        public void RemoveAll(IEnumerable<T> items)
        {
            this.SuspendCollectionChangeNotification();

            try
            {
                foreach (var i in items)
                {
                    Remove(i);
                }
            }
            finally
            {
                this.ResumeCollectionChangeNotification();
                var arg = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                this.OnCollectionChanged(arg);
            }
        }
    }
}
