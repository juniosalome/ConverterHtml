using System;
using System.IO;
using System.Text.RegularExpressions;
using static ConverterHtml.Entity;

namespace ConverterHtml
{
    public class GenericMethod
    {
        //Remover as tags
        public static string RemoveTag(string Input)
        {
            string srt;
            srt = Regex.Replace(Input, "<.*?>", String.Empty);
            srt = RemoveTab(srt);
            return srt;
        }
        public static string ReplaceAllSpaces(string str)
        {
            return Regex.Replace(str, @"\s+", "");
        }

        //Remover as tabulacoes
        public static string RemoveTab(string Input)
        {
            string str;
            const string reduceMultiSpace = @"[ ]{2,}";
            str = Regex.Replace(Input.Replace("\t", ""), reduceMultiSpace, "");
            return str;
        }

        //Trocar ponto por virgula e retirar a virgula
        public static string FormatValue(string Input)
        {
            string str;
            str = Input.Replace(",", "");
            str = str.Replace(".", ",");
            return str;
        }
        public static bool ValidadeDate(string Input)
        {
            Regex date = new Regex(@"(\d{4}\-\d{2}\-\d{2})");
            return date.Match(Input).Success;
        }
        //Conferir arquivo de entrada
        public static StreamReader OpenFileRead(string Path)
        {
            if ((Path is null) || String.Compare(Path,"") == 0)
            {
                Console.WriteLine($"Caminho do arquivo em branco.\n");
                return null;
            }
            try
            {
                return new System.IO.StreamReader(Path);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'.\n");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"The directory was not found: '{e}'.\n");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'.\n");
            }
            return null;
        }
        //Conferir arquivo de saida
        public static StreamWriter OpenFileWriter(string Path)
        {
            if ((Path is null) || Path == "")
            {
                Console.WriteLine($"Caminho do arquivo em branco.\n");
                return null;
            }
            try
            {
                return new System.IO.StreamWriter(Path);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file or directory cannot be found: '{e}'.\n");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"The file or directory cannot be found: '{e}'.\n");
            }
            catch (DriveNotFoundException e)
            {
                Console.WriteLine($"The drive specified in 'Path' is invalid: '{e}'.\n");
            }
            catch (PathTooLongException e)
            {
                Console.WriteLine($"'Path' exceeds the maxium supported path length: '{e}'.\n");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"You do not have permission to create this file: '{e}'.\n");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                Console.WriteLine("There is a sharing violation.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                Console.WriteLine("The file already exists.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
            }
            return null;
        }//Fim OpenFileWriter
        public static void CloseFile(EstruturaArquivoStream InputFile) {
            if (InputFile.Read01  != null) InputFile.Read01.Close();
            if (InputFile.Read02  != null) InputFile.Read02.Close();
            if (InputFile.Write01 != null) InputFile.Write01.Close();
            if (InputFile.Write02 != null) InputFile.Write02.Close();
        }//Fim CloseFile
    }
}

