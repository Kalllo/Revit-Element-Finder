using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using JPMorrow.Revit.Documents;

namespace JPMorrow.Revit.Views {
    
    public static class ViewFinder {

        public static IEnumerable<View> GetViewsThatContainElement(ModelInfo info, ElementId id) {

            var ret_views = new List<View>();
            var el = info.DOC.GetElement(id);

            var coll = new FilteredElementCollector(info.DOC);
            var views = coll
                .OfCategory(BuiltInCategory.OST_Views)
                .Where(x => !(x as View).IsTemplate)
                .Select(x => x as View).ToList();
            
            foreach(var v in views) {
                var el_coll = new FilteredElementCollector(info.DOC, v.Id);
                var has_el = el_coll.ToElementIds().Any(x => x.IntegerValue == id.IntegerValue);
                if(has_el) ret_views.Add(v);
            }

            return ret_views;
        }
    }
}