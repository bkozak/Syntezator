using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Syntezator.Skrypty
{
    public class UsuwanieDuplikatow
    {
        
        public string UsunDuplikaty(string v)
        {
            try
            {

                var d = new Dictionary<string, bool>();
                StringBuilder b = new StringBuilder();
                string[] a = v.Split(new char[] { ' ', ',', ';', '.' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string current in a)
                {

                    string lower = current.ToLower();

                    if (!d.ContainsKey(lower))
                    {
                        b.Append(current).Append(' ');
                        d.Add(lower, true);
                    }
                }

                return b.ToString().Trim();
            }catch(Exception)
            {
                throw new Exception("Błąd podczas usuwania duplikatów");
            }

        }
    }
}