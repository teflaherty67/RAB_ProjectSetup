#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Forms = System.Windows.Forms;

#endregion

namespace RAB_ProjectSetup
{
    [Transaction(TransactionMode.Manual)]
    public class cmdProjectSetup : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // declare variables

            string levelPath = "";
            string sheetPath = "";

            Forms.OpenFileDialog levelFile = new Forms.OpenFileDialog();
            levelFile.InitialDirectory = @"S:\Personal Folders\Training Material\ArchSmarter\Revit Add-in Bootcamp\Session 03";
            levelFile.Multiselect = false;
            levelFile.Filter = "CSV Files | *.csv; | All files | *.*";            

            Forms.OpenFileDialog sheetFile = new Forms.OpenFileDialog();
            sheetFile.InitialDirectory = @"S:\Personal Folders\Training Material\ArchSmarter\Revit Add-in Bootcamp\Session 03";
            sheetFile.Multiselect = false;
            sheetFile.Filter = "CSV Files | *.csv; | All files | *.*";

            if (levelFile.ShowDialog() != Forms.DialogResult.OK)
            {
                levelPath = levelFile.FileName;
            }

            if (sheetFile.ShowDialog() != Forms.DialogResult.OK)
            {
                sheetPath = sheetFile.FileName;
            }

            // read text file data

            List<LevelData> levelDataList = new List<LevelData>();
            List<SheetData> sheetDataList = new List<SheetData>();

            string[] levelArray = File.ReadAllLines(levelPath);
            string[] sheetArray = File.ReadAllLines(sheetPath);

            // loop through file data and put into lists

            foreach (string levelString in levelArray)
            {
                string[] cellData = levelString.Split(',');

                LevelData curLevelData = new LevelData();
                curLevelData.LevelName = cellData[0];
                curLevelData.LevelElevation = ConvertStringToDouble(cellData[1]);

                levelDataList.Add(curLevelData);
            }

            foreach (string sheetString in sheetArray)
            {
                string[] cellData = sheetString.Split(',');

                SheetData curSheetData = new SheetData();
                curSheetData.SheetNumber = cellData[0];
                curSheetData.SheetName = cellData[1];

                sheetDataList.Add(curSheetData);
            }

            // remove header rows

            levelDataList.RemoveAt(0);
            sheetDataList.RemoveAt(0);

            // create levels

            Transaction t1 = new Transaction(doc);
            t1.Start("Create Levels");

            foreach (string[] curLevelData in levelData)
            {
                string stringHeight = curLevelData[1];
                double levelHeight = 0;
                bool convertFeet = double.TryParse(stringHeight, out levelHeight);

                if (convertFeet == false)
                {
                    TaskDialog.Show("Error", "Could not convert value. Defaulting to 0");
                }

                Level curLevel = Level.Create(doc, levelHeight);
                curLevel.Name = curLevelData[0];
            }

            t1.Commit();
            t1.Dispose();

            // get titleblock element ID

            FilteredElementCollector colTBlocks = new FilteredElementCollector(doc);
            colTBlocks.OfCategory(BuiltInCategory.OST_TitleBlocks);
            ElementId tblockID = colTBlocks.FirstElementId();

            // create sheets

            Transaction t2 = new Transaction(doc);
            t2.Start("Create Sheets");

            foreach (string[] curSheetData in sheetData)
            {
                ViewSheet curSheet = ViewSheet.Create(doc, tblockID);
                curSheet.SheetNumber = curSheetData[0];
                curSheet.Name = curSheetData[1];
            }

            t2.Commit();
            t2.Dispose();

            return Result.Succeeded;
        }

        private double ConvertStringToDouble(string numberString)
        {
            double levelHeight = 0;
            bool convertString = double.TryParse(numberString, out levelHeight);

            return levelHeight;
        }

        public struct LevelData
        {
            public string LevelName;
            public double LevelElevation;
        }
         public struct SheetData
        {
            public string SheetNumber;
            public string SheetName;
        }

    }
}