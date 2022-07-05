using LeagueMatrix.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace MatrixTest
{
    [TestClass]
    public class TestFlatten
    {
        [TestMethod]
        public void ReturnsFlattened()
        {
            var controller = new MatrixController();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"matrix.csv");
            using (var csv = File.OpenRead(path))
            {
                IFormFile file = new FormFile(csv, 0, csv.Length, "id_from_form", "matrix.csv");

                var result = controller.Flatten(file);

                var expected = "1,2,3,4,5,6,7,8,9";
                Assert.AreEqual(expected, result);
            }
        }
    }
}
