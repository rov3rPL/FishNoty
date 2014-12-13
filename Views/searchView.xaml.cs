using FishNoty.ViewModels;
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

namespace FishNoty.Views
{
    /// <summary>
    /// Interaction logic for searchView.xaml
    /// </summary>
    public partial class searchView : UserControl
    {
        long? selectedSet;
        
        public searchView()
        {
            InitializeComponent();

            this.DataContext = new searchViewModel();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {

            searchViewModel viewModel = (searchViewModel)this.DataContext;
            viewModel.OnSearch(searchTextBox.Text.Trim());

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // do what you want with selected SetPresentation - e.g. display terms, detail info etc.
            
            // Try is needed here too in case the import fails synchronously.
            try
            {
                SetModel set = (e.Source as DataGridRow).Item as SetModel;

                searchViewModel viewModel = (searchViewModel)this.DataContext;
                viewModel.OnImport(set);
            }
            catch (ImportException ex)
            {
                //ImportFailed(ex);
            }

        }

    }
}
