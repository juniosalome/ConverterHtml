using System;
using System.Collections.Generic;
using static ConverterHtml.Entity;
using static ConverterHtml.MethodPersistence;
using static ConverterHtml.ExcelMethod;
using static ConverterHtml.GenericMethod;
using static ConverterHtml.Constants;
using static ConverterHtml.CompararTabelas;
using System.Windows.Forms;

namespace ConverterHtml
{
    class ConverterHtml
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
        {

            //comparar();


            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormAnalisarArquivoAmazon());
            
             
        }//End Main
    }//End ConverterHtml
}//End ConverterHtml
