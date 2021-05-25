using ClosedXML.Excel;
using System;
using System.Collections.Generic;

namespace ExecelLeitura
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //string fileName = "C:\\Users\\junio\\workspace\\ConverterHtml\\ExecelLeitura\\Prev.xlsx";
            string fileName = "C:\\Users\\junio\\workspace\\ConverterHtml\\ExecelLeitura\\AmazonRFQ.xlsx";

            using (var excelWorkbook = new XLWorkbook(fileName))
            {
                var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();
                int i, count;
                               
                List<string> nomes = new List<string>();
                string n;
                
                
                    count = excelWorkbook.Worksheets.Count;
                    for (i = 1; i <= count; i++)
                    {
                        n = (excelWorkbook.Worksheets.Worksheet(i)).ToString();
                        nomes.Add(n);
                    }
                

                var a = (excelWorkbook.Worksheets.Worksheet(1)).ToString();
                Console.WriteLine($"{a}");
                foreach (var dataRow in nonEmptyDataRows)

                {
                    if (i > 2)
                    {
                        if (dataRow.Cell(3).Value != "")
                        {
                            var cell1 = dataRow.Cell(3).Value;
                            var cell2 = dataRow.Cell(4).Value;
                            Console.WriteLine(cell1 + "\t" + cell2 + "\t");
                        }
                        else
                        {
                            break;
                        }
                    }
                    i++;
                }
            }
        }
    }
}
