using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using JPMorrow.Revit.Views;
using JPMorrow.RevitElementSearch;
using JPMorrow.Tools.Diagnostics;
using JPMorrow.UI.Views;
using MoreLinq;

namespace JPMorrow.UI.ViewModels
{
	public partial class ParentViewModel
    {
        /// <summary>
        /// prompt for save and exit
        /// </summary>
        public void MasterClose(Window window)
        {
            try {
                window.Close();
            }
            catch(Exception ex) {
                debugger.show(err:ex.ToString());
            }
        }

        public void Search(Window window) {
            
            try {
                var bic = BICItems[SelBIC];
                var search_term = SearchTerm;
                var parameter_name = SearchParameter;
                if(String.IsNullOrWhiteSpace(search_term) || String.IsNullOrWhiteSpace(parameter_name)) return;
                var qs = ElementSearch.QueryElements(Info, bic, parameter_name, search_term);

                SearchQueryItems.Clear();
                qs.ForEach(x => SearchQueryItems.Add(new ElementQueryPresenter(x, Info)));
                Update("SearchQueryItems");
            }
            catch(Exception ex) {
                debugger.show(header:"Search", err:ex.ToString());
            }
        }

        public void SearchQuerySelectionChanged(Window window) {
            
            try {
                var selected = SearchQueryItems.Where(x => x.IsSelected).ToList();
                if(!selected.Any()) return;

                var query = selected.First().Value;
                var views = ViewFinder.GetViewsThatContainElement(Info, query.CollectedElement);

                ViewItems.Clear();
                views.ForEach(x => ViewItems.Add(new ViewPresenter(x, Info)));
                Update("ViewItems");
            }
            catch(Exception ex) {
                debugger.show(header:"Search Selection Changed", err:ex.ToString());
            }
            
        }

        public void ViewSelectionChanged(Window window) {
            
            try {
                var selected = ViewItems.Where(x => x.IsSelected).ToList();
                if(!selected.Any()) return;

                var sel_q = SearchQueryItems.Where(x => x.IsSelected).ToList();
                if(!sel_q.Any()) return;

                var query = sel_q.First().Value;
                var view = selected.First().Value;

                Info.UIDOC.ActiveView = view;
                Info.UIDOC.ShowElements(new[] { query.CollectedElement });
                Info.SEL.SetElementIds(new[] { query.CollectedElement });
            }
            catch(Exception ex) {
                debugger.show(header:"View Selection Changed", err:ex.ToString());
            }
        }
	}
}