using LeagueMatrix.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace MatrixTest
{
    [TestClass]
    public class TestEcho
    {
        [TestMethod]
        public void ReturnsEcho()
        {
            var controller = new MatrixController();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"matrix.csv");
            using (var csv = File.OpenRead(path))
            {
                IFormFile file = new FormFile(csv, 0, csv.Length, "id_from_form", "matrix.csv");

                var result = controller.Echo(file);

                var expected = "1,2,3\n4,5,6\n7,8,9";
                Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public void NoFile()
        {
            var controller = new MatrixController();

            var result = controller.Echo(null);

            var expected = "file is missing";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void NonInt()
        {
            var controller = new MatrixController();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"nonints.csv");
            using (var csv = File.OpenRead(path))
            {
                IFormFile file = new FormFile(csv, 0, csv.Length, "id_from_form", "nonints.csv");

                var result = controller.Echo(file);

                var expected = "matrix must contain comma separated integers";
                Assert.AreEqual(expected, result);
            }
        }
    }
}
