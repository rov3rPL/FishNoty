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

namespace FishNoty.ViewModels
{
    class homeViewModel : ViewModelBase
    {
        public FastObservableCollection<SetPresentation> Sets { get; set; }

        public ICollectionView SetsView { get; set; }

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


        public async void OnLoaded()
        {
            try
            {
                if (!_isInitialised)
                {
                    await LoadSets();
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

        public homeViewModel()
        {
            Sets = new FastObservableCollection<SetPresentation>();
            SetsView = CollectionViewSource.GetDefaultView(Sets);

            //Errors = new FastObservableCollection<string>();

            //SettingsService = new KoiSoft.VSMine.Services.Providers.SettingsService();
            //RedmineService = new RestSharpRedmineProvider();

            //if (!String.IsNullOrEmpty(VSMinePackage.Options.RedmineBaseURL))
            //{
            //    RedmineService.Init(VSMinePackage.Options.RedmineBaseURL,
            //                        VSMinePackage.Options.RedmineUsername,
            //                        VSMinePackage.Options.RedminePassword);
            //}
        }

        private async Task LoadSets()
        {
            IList<SetPresentation> _sets = new List<SetPresentation>();

            string searchTerm = ""; // searchBox.Text.Trim();
            bool noSearch = string.IsNullOrEmpty(searchTerm);
            string tagSearch = searchTerm.StartsWith("tag:") ? searchTerm.Substring(4) : null;
            searchTerm = searchTerm.ToLower();

            var recentLists = DataStore.Database.GetRecentSets(Configuration.RecentListsSize).ToList();

            Szotar.Action<string, IEnumerable<ListInfo>> addItems = delegate(string headerText, IEnumerable<ListInfo> lists)
            {
                //var group = new ListViewGroup(headerText);
                //results.Groups.Add(group);
                foreach (ListInfo list in lists)
                {
                    if (!list.ID.HasValue || !DataStore.Database.WordListExists(list.ID.Value))
                        continue;

                    if (string.IsNullOrEmpty(list.Name))
                        list.Name = Properties.Resources.DefaultListName;

                    // If it's a tag search, the items have already been filtered - no need to do so now.
                    if (tagSearch == null && !noSearch && list.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) == -1)
                        continue;

                    if (tagSearch == null && lists != recentLists && recentLists.Count(a => a.ID == list.ID) > 0)
                        continue;

                    //var item = new ListViewItem(
                    //    new[] { list.Name, 
                    //            list.Date.HasValue ? list.Date.Value.ToString(CultureInfo.CurrentUICulture) : string.Empty, 
                    //            list.TermCount.HasValue ? string.Format(Properties.Resources.NTerms, list.TermCount.Value) : string.Empty },

                    //    "List");
                    //item.Tag = list;
                    //item.Group = group;

                    //results.Items.Add(item);

                    var item = new SetPresentation()
                    {
                        SetID = list.ID,
                        Name = list.Name,
                        Date = list.Date.HasValue ? list.Date.Value.ToString(CultureInfo.CurrentUICulture) : string.Empty,
                        TermCount = list.TermCount.HasValue ? string.Format(Properties.Resources.NTerms, list.TermCount.Value) : string.Empty,

                        Tag = list,
                        Group = headerText // group
                    };

                    //SetList.Add(item);
                    _sets.Add(item);
                }
            };

            if (tagSearch != null)
            {
                addItems(tagSearch, DataStore.Database.SearchByTag(tagSearch));
            }
            else
            {
                addItems(Properties.Resources.RecentListStoreName, recentLists);
                addItems(Properties.Resources.DefaultListStoreName, DataStore.Database.GetAllSets());
            }


            Sets.Clear();
            Sets.AddRange(_sets);

        }


        private bool PopulateResults(int? maxCount)
        {
            //results.Groups.Clear();

            string searchTerm = ""; // searchBox.Text.Trim();
            bool noSearch = string.IsNullOrEmpty(searchTerm);
            string tagSearch = searchTerm.StartsWith("tag:") ? searchTerm.Substring(4) : null;
            searchTerm = searchTerm.ToLower();

            //if (ShowTags && !noSearch)
            //{
            //    var tagsGroup = new ListViewGroup(Properties.Resources.Tags) { Header = Properties.Resources.Tags };
            //    results.Groups.Add(tagsGroup);

            //    foreach (var tag in DataStore.Database.GetTags())
            //    {
            //        if (!tag.Key.Contains(searchTerm))
            //            continue;

            //        var item = new ListViewItem(new[] { tag.Key, string.Format(Properties.Resources.NLists, tag.Value, string.Empty) }, "Tag") { Tag = tag, Group = tagsGroup };
            //        results.Items.Add(item);
            //    }
            //}

            //if (ShowDictionaries && tagSearch == null)
            //{
            //    var dictsGroup = new ListViewGroup(Properties.Resources.Dictionaries) { Name = "Dictionaries" };
            //    results.Groups.Add(dictsGroup);

            //    Action<DictionaryInfo> addItem = dict =>
            //    {
            //        int entries = dict.SectionSizes == null ? 0 : dict.SectionSizes.Sum();

            //        var item = new ListViewItem(new[] { 
            //            dict.Name, 
            //            entries > 0 ? string.Format(Properties.Resources.NEntries, entries) : string.Empty,
            //            dict.Author ?? string.Empty
            //        }, "Dictionary");
            //        item.Group = dictsGroup;
            //        item.Tag = dict;
            //        results.Items.Add(item);
            //    };

            //    if (searchTerm.Length == 0)
            //    {
            //        dictsGroup.Header = Properties.Resources.RecentDictionaries;
            //        if (GuiConfiguration.RecentDictionaries != null)
            //        {
            //            foreach (var dict in GuiConfiguration.RecentDictionaries.Entries)
            //                if (File.Exists(dict.Path))
            //                    addItem(dict);
            //        }
            //    }
            //    else
            //    {
            //        foreach (var dict in dictionaries)
            //            if (dict.Name.ToLower().Contains(searchTerm))
            //                addItem(dict);
            //    }
            //}

            var recentLists = DataStore.Database.GetRecentSets(Configuration.RecentListsSize).ToList();

            Szotar.Action<string, IEnumerable<ListInfo>> addItems = delegate(string headerText, IEnumerable<ListInfo> lists)
            {
                //var group = new ListViewGroup(headerText);
                //results.Groups.Add(group);
                foreach (ListInfo list in lists)
                {
                    if (!list.ID.HasValue || !DataStore.Database.WordListExists(list.ID.Value))
                        continue;

                    if (string.IsNullOrEmpty(list.Name))
                        list.Name = Properties.Resources.DefaultListName;

                    // If it's a tag search, the items have already been filtered - no need to do so now.
                    if (tagSearch == null && !noSearch && list.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) == -1)
                        continue;

                    if (tagSearch == null && lists != recentLists && recentLists.Count(a => a.ID == list.ID) > 0)
                        continue;

                    //var item = new ListViewItem(
                    //    new[] { list.Name, 
                    //            list.Date.HasValue ? list.Date.Value.ToString(CultureInfo.CurrentUICulture) : string.Empty, 
                    //            list.TermCount.HasValue ? string.Format(Properties.Resources.NTerms, list.TermCount.Value) : string.Empty },

                    //    "List");
                    //item.Tag = list;
                    //item.Group = group;

                    //results.Items.Add(item);

                    var item = new SetPresentation()
                    {
                        SetID = list.ID,
                        Name = list.Name,
                        Date = list.Date.HasValue ? list.Date.Value.ToString(CultureInfo.CurrentUICulture) : string.Empty,
                        TermCount = list.TermCount.HasValue ? string.Format(Properties.Resources.NTerms, list.TermCount.Value) : string.Empty,

                        Tag = list,
                        Group = headerText // group
                    };

                    //SetList.Add(item);
                    Sets.Add(item);
                }
            };

            if (tagSearch != null)
            {
                addItems(tagSearch, DataStore.Database.SearchByTag(tagSearch));
            }
            else
            {
                addItems(Properties.Resources.RecentListStoreName, recentLists);
                addItems(Properties.Resources.DefaultListStoreName, DataStore.Database.GetAllSets());
            }

            //if (ShowListItems && tagSearch == null && !string.IsNullOrEmpty(searchBox.Text) && !(results.Items.Count > maxCount))
            //{
            //    var group = new ListViewGroup(Properties.Resources.SearchResults);
            //    group.Name = Properties.Resources.SearchResults;
            //    results.Groups.Add(group);
            //    foreach (var wsr in DataStore.Database.SearchAllEntries(searchBox.Text))
            //    {
            //        if (results.Items.Count > maxCount)
            //            return false;

            //        var item = new ListViewItem(
            //            new[] { wsr.Phrase, 
            //                        wsr.Translation, 
            //                        wsr.SetName });
            //        item.Tag = wsr;
            //        item.Group = group;
            //        results.Items.Add(item);
            //    }
            //}

            return true;
        }



    }
}
