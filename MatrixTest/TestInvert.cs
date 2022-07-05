using LeagueMatrix.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace MatrixTest
{
    [TestClass]
    public class TestInvert
    {
        [TestMethod]
        public void ReturnsInverted()
        {
            var controller = new MatrixController();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"matrix.csv");
            using (var csv = File.OpenRead(path))
            {
                IFormFile file = new FormFile(csv, 0, csv.Length, "id_from_form", "matrix.csv");

                var result = controller.Invert(file);

                var expected = "1,4,7\n2,5,8\n3,6,9";
                Assert.AreEqual(expected, result);
            }
        }
    }
}
