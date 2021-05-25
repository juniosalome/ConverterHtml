using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Conexao
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Getting Connection ...");
            //your server
            string datasource = "localhost";
            //your database name
            string database = "PlannerAmazon";
            //username of server to connect
            string username = "sa";
            //password
            string password = "econguiloo";
            //your connection string 
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            string str = "\n    aaa a a\n a a a a a                 kkk \n \t lllll lllll               aaaa    ";
            Console.WriteLine("--------------");
            Console.WriteLine($"{str}");
            Console.WriteLine("--------------");
            Console.WriteLine($"{ReplaceAllSpaces(str)}");

            //create instanace of database connection
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    Console.WriteLine("Openning Connection ...");

                    //open connection
                    conn.Open();

                    Console.WriteLine("Connection successful!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

                Console.Read();
                conn.Close();
            }
        }

        public static string ReplaceAllSpaces(string str)
        {
            return Regex.Replace(str, @"\s+", "");
        }


    }
}


