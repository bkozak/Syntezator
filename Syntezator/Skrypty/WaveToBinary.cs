using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;

namespace Syntezator.Skrypty
{
    public class WaveToBinary
    {
       private SqlConnection _conn;
       private string NazwaPliku;
       private string sciezka;
     

       public void DoBazy()
       {
           _conn = new SqlConnection(@"Data Source=(localDb)\v11.0;AttachDbFilename=|DataDirectory|\BazaSlow.mdf;Initial Catalog=Bazaslow;Integrated Security=True");
           string[] tablica = new string[34];
           tablica = Directory.GetFiles("c:/test/Głoski/nowe/ok4/");
           sciezka = "c:/test/Głoski/nowe/ok4/";
           try
           {
               _conn.Open();
               for (int i = 0; i < tablica.Length; i++)
               {
                   byte[] bytes = File.ReadAllBytes(sciezka + Path.GetFileName(tablica[i]));
                   NazwaPliku = Path.GetFileNameWithoutExtension(tablica[i]);

                   SqlCommand addEmp = new SqlCommand(
                      "INSERT INTO Fony (" +
                      "Fon, BLOB) " +
                      "VALUES(@Fon,@BLOB)", _conn);

                   addEmp.Parameters.Add("@Fon", SqlDbType.NVarChar, 20).Value = NazwaPliku;
                   addEmp.Parameters.Add("@BLOB", SqlDbType.VarBinary).Value = bytes;


                   addEmp.ExecuteNonQuery();
               }
           }
           catch(Exception)
           {
               throw new Exception("Błąd podczas eksportu plików do bazy."); ;
              
           }
           finally
           {
               _conn.Close();
              
           }
           
           
       }
       
        
    }
}