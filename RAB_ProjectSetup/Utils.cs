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

        public static ViewFamilyType GetViewFamilyTypeByName(Document doc, string vftName, ViewFamily vf)
        {
            FilteredElementCollector colVFT = new FilteredElementCollector(doc);
            colVFT.OfClass(typeof(ViewFamilyType));

            foreach(ViewFamilyType curVFT in colVFT)
            {
                if(curVFT.Name == vftName && curVFT.ViewFamily == vf)
                {
                    return curVFT;
                }
            }

            return null;
        }

        public static View GetViewByName(Document doc, string name)
        {
            FilteredElementCollector colViewName = new FilteredElementCollector(doc);
            colViewName.OfCategory(BuiltInCategory.OST_Views);
            {
                foreach(View curView in colViewName)
                {
                    if(curView.Name == name)
                    {
                        return curView;
                    }
                }

                return null;
            }
        }
        
        public static XYZ GetSheetCenterPoint(ViewSheet curSheet)
        {
            BoundingBoxUV outline = curSheet.Outline;

            double x = (outline.Max.U + outline.Min.U) / 2;
            double y = (outline.Max.V + outline.Min.V) / 2;

            XYZ returnPoint = new XYZ(x, y, 0);

            return returnPoint;
        }
    }
}
