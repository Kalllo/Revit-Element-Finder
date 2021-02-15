using System.Windows.Input;
using System.Windows;
using JPMorrow.Revit.Documents;
using System.Linq;
using JPMorrow.Views.RelayCmd;
using om = System.Collections.ObjectModel;
using JPMorrow.RevitElementSearch;
using JPMorrow.Revit.BICs;

namespace JPMorrow.UI.ViewModels
{
    // observable collection aliases
    using ObsStr = om.ObservableCollection<string>;
    using ObsSearchQuery = om.ObservableCollection<ParentViewModel.ElementQueryPresenter>;
    using ObsView = om.ObservableCollection<ParentViewModel.ViewPresenter>;
    using ObsParam = om.ObservableCollection<ParentViewModel.ParameterPresenter>;

    public partial class ParentViewModel : Presenter
    {
        private static ModelInfo Info { get; set; }

        // observable collections
        public ObsSearchQuery SearchQueryItems { get; set; } = new ObsSearchQuery();
        public ObsParam ParameterItems { get; set; } = new ObsParam();
        public ObsView ViewItems { get; set; } = new ObsView();
        public ObsStr BICItems { get; set; } = new ObsStr();

        public int SelBIC { get; set; }
        
        public string SearchTerm { get; set; }
        public string SearchParameter { get; set; }

        public ICommand MasterCloseCmd => new RelayCommand<Window>(MasterClose);
        public ICommand SearchCmd => new RelayCommand<Window>(Search);
        public ICommand SearchQuerySelectionChangedCmd => new RelayCommand<Window>(SearchQuerySelectionChanged);
        public ICommand ViewSelectionChangedCmd => new RelayCommand<Window>(ViewSelectionChanged);

        
        public ParentViewModel(ModelInfo info)
        {
            //revit documents and pre converted elements
            Info = info;
            BICItems = new ObsStr(BICutil.GetCategoryNames(ElementSearch.SearchCategories));
            SearchTerm = "";
            SearchParameter = "";
        }
    }
}
