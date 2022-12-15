using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RAB_ProjectSetup
{
    internal static class Utils
    {
        public static Element GetTitleBlockByName(Document doc, string name)
        {
            FilteredElementCollector colTBlocks = new FilteredElementCollector(doc);
            colTBlocks.OfCategory(BuiltInCategory.OST_TitleBlocks);

            foreach (Element curTBlock in colTBlocks)
            {
                if (curTBlock.Name == name)
                    return curTBlock;
            }

            return null;
        }
    }
}
