using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterHtml
{
    public class Constants
    {
        public static class ConstantsFile
        {
            //Ira ler o arquivo
            public const int ReadFileList = 1;
            public const int WriteFile = 2;
            public const int WriteFileGRU = 3;
            public const int ReadWriteFile = 4;
            public const int NewFile = 5;
            
        }
        //Constantes para colocar observacao no item
        public static class ConstantsObservacao {
            public const string ErroPreco = "Preco esta errado";
            public const string NaoCadastrado = "Nao cadastrado";
            public const string OK = "OK";
            public const string CodigoErrado = "Cod. Amazon cadastrado:";
        }
    }
}
