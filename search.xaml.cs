using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FishNoty
{
    /// <summary>
    /// Interaction logic for search.xaml
    /// </summary>
    public partial class search : Window
    {
        long? selectedSet;
        readonly QuizletImporter importer;
        CancellationTokenSource cts;
        //readonly DisposableComponent disposableComponent;
        bool searching;

        public search()
        {
            InitializeComponent();

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
            //importButton.Enabled = false;
            //searchResults.SelectedIndexChanged += SearchResultsSelectedIndexChanged;
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
			AbortRequest();

            new QuizletApi().SearchSets(searchTextBox.Text.Trim(), cts.Token).ContinueWith(t => {
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
			searchBtn.Content = Properties.Resources.Abort;

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
            searchBtn.Content = Properties.Resources.Search;
        }

        private void SearchError(Exception e)
        {
            var ae = e as AggregateException;
            if (ae != null && ae.InnerExceptions.Count == 1)
                e = ae.InnerExceptions[0];

            searching = false;
            // ?
            searchBtn.Content = Properties.Resources.Search;
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
            searchBtn.Content = Properties.Resources.Search;
            //progressBar.Visible = false;

            resultsGrid.ItemsSource = results;

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
