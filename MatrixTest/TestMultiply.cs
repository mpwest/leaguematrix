using LeagueMatrix.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace MatrixTest
{
    [TestClass]
    public class TestMultiply
    {
        [TestMethod]
        public void ReturnsProduct()
        {
            var controller = new MatrixController();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"matrix.csv");
            using (var csv = File.OpenRead(path))
            {
                IFormFile file = new FormFile(csv, 0, csv.Length, "id_from_form", "matrix.csv");

                var result = controller.Multiply(file);


                var expected = "362880";
                Assert.AreEqual(expected, result);
            }
        }
    }
}
