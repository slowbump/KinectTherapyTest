using System.Collections.Generic;
using System.Data;
using System.Xml;
using NUnit.Framework;

using SWENG.Service;

namespace KinectTherapyUnitTesting
{
    [TestFixture]
    public class CatalogExerciseXmlTestFixture
    {
        private CatalogManager _catalogManager;
        private string CatalogDirectory = System.AppDomain.CurrentDomain.BaseDirectory + "/GitHub/KinectTherapyTest/KinectTherapyContent/Exercises/";

        [Test(Description = "UC-1.2: The application loads the “Catalog” screen")]
        public void CatalogXmlLoaded()
        {
            _catalogManager = new CatalogManager();

            _catalogManager.CatalogXmlLinqData();
            Assert.Pass("CatalogManager.CatalogXmlLinqData() passed.");
        }

        [Test(Description = "UC-1.3: The physical therapist selects a category")]
        public void AreExerciseListsEquivalent()
        {
            var testListComparer = new List<CatalogItem> {
               new CatalogItem("EXELAE","Left Arm Extensions","Move Left arm from upright to fully extended"),
               new CatalogItem("EXERAE","Reft Arm Extensions","Move Right arm from upright to fully extended")
            };

            var dataTable = new DataTable("ExerciseCatalog");
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("Category");

            var newRowOne = dataTable.NewRow();
            newRowOne["Id"] = "EXERAE";
            newRowOne["Category"] = "Arms";
            dataTable.Rows.Add(newRowOne);

            var newRowTwo = dataTable.NewRow();
            newRowTwo["Id"] = "EXELAE";
            newRowTwo["Category"] = "Arms";
            dataTable.Rows.Add(newRowTwo);

            var newRowThree = dataTable.NewRow();
            newRowThree["Id"] = "EXELUNGE";
            newRowThree["Category"] = "Thighs";
            dataTable.Rows.Add(newRowThree);

            _catalogManager = new CatalogManager();

            var testListResults = _catalogManager.GetExercisesByType("Arms");
            
            CollectionAssert.AreEquivalent(testListComparer, testListResults);
            Assert.Pass("Exercise Lists are equivalent.");
        }

    }
}
