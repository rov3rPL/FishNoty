using FishNoty.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using Szotar.Sqlite;

namespace FishNoty.Views
{
    //public class SetPresentation
    //{
    //    public string Name { get; set; }
    //    public string Date { get; set; }
    //    public string TermCount { get; set; }
    //    public object Tag { get; set; }
    //    public string Group { get; set; }
    //}

    //public class SetPresentation : INotifyPropertyChanged
    //{
    //    protected long? _setID;
    //    public long? SetID { get { return _setID; } set { _setID = value; RaisePropertyChanged("SetID"); } }

    //    protected string _name;
    //    public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }

    //    protected string _date;
    //    public string Date { get { return _date; } set { _date = value; RaisePropertyChanged("Date"); } }

    //    protected string _termCount;
    //    public string TermCount { get { return _termCount; } set { _termCount = value; RaisePropertyChanged("TermCount"); } }

    //    protected object _tag;
    //    public object Tag { get { return _tag; } set { _tag = value; RaisePropertyChanged("Tag"); } }

    //    protected string _group;
    //    public string Group { get { return _group; } set { _group = value; RaisePropertyChanged("Group"); } }

    //    protected bool _turnedOn;
    //    public bool TurnedOn { get { return _turnedOn; } set { _turnedOn = value; RaisePropertyChanged("TurnedOn");
    //        if(value)
    //        {
    //            if (!this.SetID.HasValue)
    //                return;
    //            WordList list = DataStore.Database.GetWordList(this.SetID.Value);
    //            if (list == null)
    //                return;
    //            StaticController.AddNotificationsSource(list);
    //        }
    //    } }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public void RaisePropertyChanged(String propertyName)
    //    {
    //        PropertyChangedEventHandler temp = PropertyChanged;
    //        if (temp != null)
    //        {
    //            temp(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }
    //}

    /// <summary>
    /// Interaction logic for homeView.xaml
    /// </summary>
    public partial class homeView : UserControl
    {
        public homeView()
        {
            InitializeComponent();

            this.DataContext = new homeViewModel();            
        }

        private void homeView_Loaded(object sender, RoutedEventArgs e)
        {
            homeViewModel viewModel = (homeViewModel)this.DataContext;
            viewModel.OnLoaded();
        }

        #region ToBeRemoved
        //todo: implement MVVM everywhere
        //List<SetPresentation> SetList = new List<SetPresentation>();
        // make it 'FastObservableCollection' like in VSMine project
        //FastObservableCollection<SetPresentation> Sets = new FastObservableCollection<SetPresentation>();

        private bool ShowTags = true;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // do what you want with selected SetPresentation - e.g. display terms, detail info etc.
            SetPresentation selectedSet = (e.Source as DataGridRow).Item as SetPresentation;

            if (!selectedSet.SetID.HasValue)
                return;
            WordList list = DataStore.Database.GetWordList(selectedSet.SetID.Value);
            if (list == null)
                return;
            StaticController.AddNotificationsSource(list);
        }

        //private bool PopulateResults(int? maxCount)
        //{
        //    //results.Groups.Clear();

        //    string searchTerm = ""; // searchBox.Text.Trim();
        //    bool noSearch = string.IsNullOrEmpty(searchTerm);
        //    string tagSearch = searchTerm.StartsWith("tag:") ? searchTerm.Substring(4) : null;
        //    searchTerm = searchTerm.ToLower();

        //    //if (ShowTags && !noSearch)
        //    //{
        //    //    var tagsGroup = new ListViewGroup(Properties.Resources.Tags) { Header = Properties.Resources.Tags };
        //    //    results.Groups.Add(tagsGroup);

        //    //    foreach (var tag in DataStore.Database.GetTags())
        //    //    {
        //    //        if (!tag.Key.Contains(searchTerm))
        //    //            continue;

        //    //        var item = new ListViewItem(new[] { tag.Key, string.Format(Properties.Resources.NLists, tag.Value, string.Empty) }, "Tag") { Tag = tag, Group = tagsGroup };
        //    //        results.Items.Add(item);
        //    //    }
        //    //}

        //    //if (ShowDictionaries && tagSearch == null)
        //    //{
        //    //    var dictsGroup = new ListViewGroup(Properties.Resources.Dictionaries) { Name = "Dictionaries" };
        //    //    results.Groups.Add(dictsGroup);

        //    //    Action<DictionaryInfo> addItem = dict =>
        //    //    {
        //    //        int entries = dict.SectionSizes == null ? 0 : dict.SectionSizes.Sum();

        //    //        var item = new ListViewItem(new[] { 
        //    //            dict.Name, 
        //    //            entries > 0 ? string.Format(Properties.Resources.NEntries, entries) : string.Empty,
        //    //            dict.Author ?? string.Empty
        //    //        }, "Dictionary");
        //    //        item.Group = dictsGroup;
        //    //        item.Tag = dict;
        //    //        results.Items.Add(item);
        //    //    };

        //    //    if (searchTerm.Length == 0)
        //    //    {
        //    //        dictsGroup.Header = Properties.Resources.RecentDictionaries;
        //    //        if (GuiConfiguration.RecentDictionaries != null)
        //    //        {
        //    //            foreach (var dict in GuiConfiguration.RecentDictionaries.Entries)
        //    //                if (File.Exists(dict.Path))
        //    //                    addItem(dict);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        foreach (var dict in dictionaries)
        //    //            if (dict.Name.ToLower().Contains(searchTerm))
        //    //                addItem(dict);
        //    //    }
        //    //}

        //    var recentLists = DataStore.Database.GetRecentSets(Configuration.RecentListsSize).ToList();

        //    Szotar.Action<string, IEnumerable<ListInfo>> addItems = delegate(string headerText, IEnumerable<ListInfo> lists)
        //    {
        //        //var group = new ListViewGroup(headerText);
        //        //results.Groups.Add(group);
        //        foreach (ListInfo list in lists)
        //        {
        //            if (!list.ID.HasValue || !DataStore.Database.WordListExists(list.ID.Value))
        //                continue;

        //            if (string.IsNullOrEmpty(list.Name))
        //                list.Name = Properties.Resources.DefaultListName;

        //            // If it's a tag search, the items have already been filtered - no need to do so now.
        //            if (tagSearch == null && !noSearch && list.Name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) == -1)
        //                continue;

        //            if (tagSearch == null && lists != recentLists && recentLists.Count(a => a.ID == list.ID) > 0)
        //                continue;

        //            //var item = new ListViewItem(
        //            //    new[] { list.Name, 
        //            //            list.Date.HasValue ? list.Date.Value.ToString(CultureInfo.CurrentUICulture) : string.Empty, 
        //            //            list.TermCount.HasValue ? string.Format(Properties.Resources.NTerms, list.TermCount.Value) : string.Empty },

        //            //    "List");
        //            //item.Tag = list;
        //            //item.Group = group;

        //            //results.Items.Add(item);

        //            var item = new SetPresentation()
        //            {
        //                SetID = list.ID,
        //                Name = list.Name,
        //                Date = list.Date.HasValue ? list.Date.Value.ToString(CultureInfo.CurrentUICulture) : string.Empty,
        //                TermCount = list.TermCount.HasValue ? string.Format(Properties.Resources.NTerms, list.TermCount.Value) : string.Empty,

        //                Tag = list,
        //                Group = headerText // group
        //            };

        //            SetList.Add(item);
                    
        //        }
        //    };

        //    if (tagSearch != null)
        //    {
        //        addItems(tagSearch, DataStore.Database.SearchByTag(tagSearch));
        //    }
        //    else
        //    {
        //        addItems(Properties.Resources.RecentListStoreName, recentLists);
        //        addItems(Properties.Resources.DefaultListStoreName, DataStore.Database.GetAllSets());
        //    }

        //    //if (ShowListItems && tagSearch == null && !string.IsNullOrEmpty(searchBox.Text) && !(results.Items.Count > maxCount))
        //    //{
        //    //    var group = new ListViewGroup(Properties.Resources.SearchResults);
        //    //    group.Name = Properties.Resources.SearchResults;
        //    //    results.Groups.Add(group);
        //    //    foreach (var wsr in DataStore.Database.SearchAllEntries(searchBox.Text))
        //    //    {
        //    //        if (results.Items.Count > maxCount)
        //    //            return false;

        //    //        var item = new ListViewItem(
        //    //            new[] { wsr.Phrase, 
        //    //                        wsr.Translation, 
        //    //                        wsr.SetName });
        //    //        item.Tag = wsr;
        //    //        item.Group = group;
        //    //        results.Items.Add(item);
        //    //    }
        //    //}

        //    return true;
        //}
        #endregion        

        

    }
}
