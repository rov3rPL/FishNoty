using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiSoft.VSMine.ViewModels;
using KoiSoft.VSMine.Common;
using System.Windows.Input;
using FishNoty.Exceptions;
using System.Windows.Data;
using System.ComponentModel;
using Szotar;
using System.Globalization;
using Szotar.Quizlet;
using System.Threading;
using Szotar.WindowsForms;

namespace FishNoty.ViewModels
{
    class searchViewModel : ViewModelBase
    {
        public FastObservableCollection<SetModel> SearchResults { get; set; }

        public ICollectionView SearchResultsView { get; set; }

        private bool _isLoading;
        /// <summary>
        /// Is loading sth in background
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        private bool _isInitialised;


        public async void OnSearch(string searchStr)
        {
            try
            {
                if (!_isInitialised)
                {
                    await SearchSets(searchStr);
                    //await LoadProjects();
                    _isInitialised = true;
                }
                //Refresh();
            }
            catch (NotInitializedException notInitializedException)
            {

            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(exp.ToString());
            }
        }

        public void OnImport(SetModel set)
        {
            importer.Set = set.SetID;
            importer.BeginImport();
        }

        #region Commands

        private System.Windows.Input.ICommand _refreshCommand;
        /// <summary>
        /// Refresh viewed data
        /// </summary>
        public System.Windows.Input.ICommand RefreshCommand
        {
            get
            {
                //if (_refreshCommand == null)
                //{
                //    _refreshCommand = new RelayCommand(p => Refresh(), p => this.SelectedProject != null);
                //}
                return _refreshCommand;
            }
        }

        #endregion

        readonly QuizletImporter importer;
        CancellationTokenSource cts;
        //readonly DisposableComponent disposableComponent;
        bool searching;

        public searchViewModel()
        {
            SearchResults = new FastObservableCollection<SetModel>();
            SearchResultsView = CollectionViewSource.GetDefaultView(SearchResults);

            //Errors = new FastObservableCollection<string>();


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



            importer = new QuizletImporter();

            //if (components == null)
            //    components = new System.ComponentModel.Container();

            cts = new CancellationTokenSource();
            //components.Add(disposableComponent = new DisposableComponent(cts));

            searching = false;
        }

        private async Task SearchSets(string searchStr)
        {
            //IList<SetPresentation> _sets = new List<SetPresentation>();


            AbortRequest();

            new QuizletApi().SearchSets(searchStr, cts.Token).ContinueWith(t =>
            {
                if (t.Exception != null)
                    SearchError(t.Exception);
                else if (t.IsCanceled)
                    SearchError(new OperationCanceledException());
                else
                    SetResults(t.Result);
            }, TaskScheduler.FromCurrentSynchronizationContext());

            searching = true;
            //progressBar.Visible = true;
            // ?
            //searchBtn.Content = Properties.Resources.Abort;
        }


        // Aborts the current web request, if any.
        private void AbortRequest()
        {
            if (!searching)
                return;

            cts.Cancel();
            cts.Dispose();
            //disposableComponent.Thing = cts = new CancellationTokenSource();
            searching = false;
            //progressBar.Visible = false;
            // ?
            //searchBtn.Content = Properties.Resources.Search;
        }

        private void SearchError(Exception e)
        {
            var ae = e as AggregateException;
            if (ae != null && ae.InnerExceptions.Count == 1)
                e = ae.InnerExceptions[0];

            searching = false;
            // ?
            //searchBtn.Content = Properties.Resources.Search;
            //progressBar.Visible = false;

            //searchResults.Items.Clear();
            //searchResults.Items.Add("Error: " + (e != null ? e.Message : "<Unknown>"));
            //searchResults.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            //searchResults.Enabled = false;
        }

        private void SetResults(IEnumerable<SetModel> results)
        {
            searching = false;
            // ?
            //searchBtn.Content = Properties.Resources.Search;
            //progressBar.Visible = false;

            //resultsGrid.ItemsSource = results;
            SearchResults.Clear();
            SearchResults.AddRange(results);

            //searchResults.Enabled = true;
            //searchResults.BeginUpdate();
            //searchResults.Items.Clear();
            //foreach (var set in results)
            //{
            //    var item = new ListViewItem(new[] { 
            //        set.Title, 
            //        set.Author, 
            //        set.Created.ToString("d", CultureInfo.CurrentUICulture), 
            //        set.TermCount.ToString(CultureInfo.CurrentUICulture), 
            //        set.SetID.ToString(CultureInfo.CurrentUICulture) }) { Tag = set.SetID };
            //    searchResults.Items.Add(item);
            //}
            //searchResults.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            //searchResults.EndUpdate();

            //UpdateImportButton();
        }

    }
}
