using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syntezator.Skrypty
{
    public class PodzialNaWyrazy
    {
        public static string[] podzielone;

        public void Podzial(string tekst)
        {
            try
            {
                char[] znaki = { ' ', ',', '.', ':', '\t', '?', '!', ';', '(', ')' };
                podzielone = tekst.ToLower().Split(znaki, StringSplitOptions.RemoveEmptyEntries);

            }catch(Exception ex)
            {
                throw ex;
            }
        }        
    }
}