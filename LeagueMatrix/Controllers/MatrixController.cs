using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeagueMatrix.Controllers
{
    [Route("")]
    [ApiController]
    public class MatrixController : ControllerBase
    {
        // Post: /echo
        [HttpPost("echo")]
        public string Echo([FromForm(Name = "file")] IFormFile file)
        {
            var err = checkFile(file);
            if (err != String.Empty) return err;
            try
            {
                var matrix = readMatrix(file);
                return outputMatrix(matrix, '\n');
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // Post: /invert
        [HttpPost("invert")]
        public string Invert([FromForm(Name = "file")] IFormFile file)
        {
            var err = checkFile(file);
            if (err != String.Empty) return err;

            try { 
                var matrix = readMatrix(file);

                int[][] inverted = new int[matrix[0].Length][];

                for (int i = 0; i < matrix[0].Length; i++)
                {
                    int[] invertedRow = new int[matrix.Length];
                    for (int j = 0; j < matrix.Length; j++)
                    {
                        invertedRow[j] = matrix[j][i];
                    }
                    inverted[i] = invertedRow;
                }

                return outputMatrix(inverted, '\n');
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // Post: /flatten
        [HttpPost("flatten")]
        public string Flatten([FromForm(Name = "file")] IFormFile file)
        {
            var err = checkFile(file);
            if (err != String.Empty) return err;
            try
            {
                var matrix = readMatrix(file);
                return outputMatrix(matrix, ',');
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // Post: /sum
        [HttpPost("sum")]
        public string Sum([FromForm(Name = "file")] IFormFile file)
        {
            var err = checkFile(file);
            if (err != String.Empty) return err;
            try
            {
                var matrix = readMatrix(file);
                return calculate(matrix, (x, y) => x + y, 0);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // Post: /multiply
        [HttpPost("multiply")]
        public string Multiply([FromForm(Name = "file")] IFormFile file)
        {
            var err = checkFile(file);
            if (err != String.Empty) return err;
            try
            {
                var matrix = readMatrix(file);
                return calculate(matrix, (x, y) => x * y, 1);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string checkFile(IFormFile file)
        {
            if (file == null)
            {
                return "file is missing";
            }
            return String.Empty;
        }

        private int[][] readMatrix(IFormFile file)
        {

            var matrix = new List<int[]>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    try
                    {
                        var vals = line.Split(',').Select(Int32.Parse).ToArray();
                        matrix.Add(vals);
                    }
                    catch (Exception e)
                    {
                        throw new FormatException("matrix must contain comma separated integers");
                    }
                }
            }
            return matrix.ToArray();
        }

        private string outputMatrix(int[][] matrix, char joinChar)
        {
            var rows = new List<string>();
            foreach (var line in matrix)
            {
                rows.Add(String.Join(',', line));
            }
            return String.Join(joinChar, rows);
        }

        private string calculate(int[][] matrix, Func<long, int, long> op, int startVal)
        {
            long val = startVal;
            foreach (var line in matrix)
            {
                foreach (var num in line)
                {
                    val = op(val, num);
                }
            }
            return val.ToString();
        }
    }
}
