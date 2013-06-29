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
        private const string CatalogDirectory = @"/GitHub/KinectTherapyTest/KinectTherapyContent/Exercises/";

        [Test]
        public void CatalogXmlLoaded()
        {
            //var dataTable = new DataTable();
            _catalogManager = new CatalogManager();

            var dataTable = _catalogManager.CatalogXmlLinqData();
            Assert.Pass("CatalogManager.CatalogXmlLinqData() passed.");
        }

        [Test]
        public void AreExerciseListsEquivalent()
        {
            var testListComparer = new List<string> { "EXERAE", "EXELAE" };

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

            var testListResults = _catalogManager.ListWorkoutExercises("Arms", dataTable);

            CollectionAssert.AreEqual(testListComparer, testListResults);
        }

        [Test]
        public void AreExerciseXmlEquivalent()
        {
            var exerciseList = new List<string> {"EXELAE", "EXERAE"};

            var expectedXmlDocument = new XmlDocument();
            expectedXmlDocument.Load(CatalogDirectory + @"EXELAE.xml");

            _catalogManager = new CatalogManager();

            var actualXmlDocument = _catalogManager.ListWorkoutExerciseObjects(exerciseList);

           StringAssert.AreEqualIgnoringCase(expectedXmlDocument.ToString(), actualXmlDocument.ToString());
        }

    }
}
