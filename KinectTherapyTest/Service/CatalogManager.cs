using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Xna.Framework;
using System.IO;

using SWENG.Criteria;

namespace SWENG.Service
{
    public enum CatalogManagerStatus
    {
        Complete,
        Start,
        Cancel
    }

    public delegate void CatalogSelectionStatusChanged(object sender, CatalogSelectionChangedEventArg e);

    public class CatalogSelectionChangedEventArg : EventArgs
    {
        public string ExerciseId;

        public CatalogSelectionChangedEventArg(string exerciseId)
        {
            ExerciseId = exerciseId;
        }
    }

    public class CatalogManager : IGameComponent
    {
        public DataTable DataTable { get; set; }

        #region event stuff
        public event CatalogSelectionStatusChanged CatalogSelectionStatusChanged;

        // Invoke the Changed event; called whenever the catalog status changes
        protected virtual void OnRecordingStatusChanged(CatalogSelectionChangedEventArg e)
        {
            if (CatalogSelectionStatusChanged != null)
                CatalogSelectionStatusChanged(this, e);
        }

        #endregion

        public CatalogManagerStatus Status { get; internal set; }

        public bool IsWorkoutClicked { get; set; }
        public bool IsExerciseClicked { get; set; }

        private List<string> _workoutList;

        // Initialize catalog variables
        private const string CatalogDirectory = @"C:\Project Backups\KinectTherapySolution\KinectTherapy\KinectTherapyContent\Exercises\";
        private const string XmlHeader = @"<?xml version=""1.0"" encoding=""utf-8"" ?>";


        public void Initialize()
        {
        }

        public void SelectionStart()
        {
            Status = CatalogManagerStatus.Start;
        }

        public void SelectionStop(object sender, EventArgs e)
        {
            Status = CatalogManagerStatus.Complete;
        }

        public DataTable CatalogXmlLinqData()
        {
            var catalogList =
                from c in XDocument.Load(CatalogDirectory + @"MasterCatalog.xml").Descendants("Exercise")
                select new
                    {
                        xmlFile = c.Attribute("Id").Value
                    };

            var xmlFileList = catalogList.ToList();
            var xmlFileName = new List<string>(xmlFileList.Count);
            xmlFileName.AddRange(from t in xmlFileList where !t.xmlFile.Equals("MasterCatalog") select t.xmlFile);

            DataTable = new DataTable("ExerciseCatalog");

            DataTable.Columns.Add("Id");
            DataTable.Columns.Add("Catagory");
            DataTable.Columns.Add("Name");
            DataTable.Columns.Add("Description");

            var catalogArray = new Array[4];

            foreach (var fileName in xmlFileName)
            {
                var newFileName = CatalogDirectory + @"\" + fileName + ".xml";

                var query =
                    from s in XDocument.Load(CatalogDirectory + @"\MasterCatalog.xml").Descendants("Exercise")
                    join e in XDocument.Load(newFileName).Descendants("Exercise") on s.Attribute("Id").Value equals
                        e.Attribute("Id").Value

                    select new
                        {
                            Id = s.Attribute("Id").Value,
                            Category = e.Attribute("Category").Value,
                            Name = s.Attribute("Name").Value,
                            Description = e.Attribute("Description").Value
                        };

                foreach (var s in query)
                {
                    DataTable.Rows.Add(s.Id, s.Category, s.Name, s.Description);
                }
            }

            return DataTable;
        }

        public List<string> ListWorkoutExercises(string exerciseGroup, DataTable dataTable)
        {
            _workoutList = new List<string>();

            try
            {
                var dataRow = dataTable.Select("Category = '" + exerciseGroup + "'");

                foreach (var row in dataRow)
                {
                    _workoutList.Add(row["Id"].ToString());
                }

            }
            catch (Exception)
            {
                _workoutList = null;
                throw;
            }

            return _workoutList;

        }

        public XmlReader ListWorkoutExerciseObjects(List<string> exerciseList)
        {
            var fileContents = String.Empty;
            var startPos = 0;

            foreach (var sr in exerciseList.Select(item => new FileStream(CatalogDirectory + item + @".xml", FileMode.Open, FileAccess.Read)).Select(fs => new StreamReader(fs)))
            {
                fileContents = fileContents.Insert(startPos, sr.ReadToEnd());

                startPos = (int)(fileContents.Length);
            }

            // Remove all occurences of Xml Header that was inserted due to processing multiple files
            fileContents = fileContents.Replace(XmlHeader, "");

            // Put header back in for proper xml formatting
            fileContents = fileContents.Insert(0, XmlHeader);

            // Add root node
            fileContents = fileContents.Insert(XmlHeader.Length, "<Exercises>");
            fileContents = fileContents + "</Exercises>";

            var xmlReader = XmlReader.Create(fileContents);

            return xmlReader;
        }
    }
}
