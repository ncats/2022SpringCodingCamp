using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FOTSLibrary.Projects;
using System.Collections.Generic;

namespace CoMaUnitTest.Database
{
    [TestClass]
    public class ProjectTest 
    {
        private const string PROJECT_NAME = "Unit Test";

        [TestMethod]
        public void AssignProjectPropertiesTest()
        {
          
        }

        [TestMethod]
        public void GetAllTest()
        {
            List<Project> projects = Project.GetAll();
            Assert.AreEqual<bool>(projects.Count > 0, true);
        }
               
        [TestMethod]
        public void AddTest()
        {
            decimal? visible = 1;
            string cycle = "cycle";
            string carsUID = "carsUID";
            string carsName = "carsName";
            string assayProvider = "assayProvider";
            string grantNumber = "grantNumber";
            string projectType = "projectType";
            Team team = Team.Get(1);

            Project project = new Project(PROJECT_NAME, visible, cycle, carsUID, carsName, assayProvider, grantNumber, team.Name, projectType);
            project.Add();
            Project project2 = Project.Get(PROJECT_NAME);

            Assert.IsNotNull(project2);
        }

        [TestMethod]
        public void GetOneTest()
        {
            Project project = Project.Get(PROJECT_NAME);
            Assert.AreEqual(project.Name, PROJECT_NAME);
        }

        [TestMethod]
        public void UpdateTest()
        {
            string projectName = "Unit Test2";
            decimal? visible = 2;
            string cycle = "cycle2";
            string carsUID = "carsUID2";
            string carsName = "carsName2";
            string assayProvider = "assayProvider2";
            string grantNumber = "grantNumber2";
            string projectType = "projectType2";
            Team team = Team.Get(2);

            Project project = new Project(projectName, visible, cycle, carsUID, carsName, assayProvider, grantNumber, team.Name, projectType);
            project.Update();
            Project project2 = Project.Get(projectName);

            Assert.IsTrue(project2.Name == projectName);
        }
        
        [TestMethod]
        public void DeleteTest()
        {
           Project project = Project.Get(PROJECT_NAME);

            if (project == null)
                Assert.Fail("Project with " + PROJECT_NAME + " not found");

            project.Delete();

            Project project2 = Project.Get(PROJECT_NAME);

            Assert.IsNull(project2);
        }
    }
}
