using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Syntezator.Skrypty;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace Syntezator.Klasy
{
    public class PlikWave
    {
        public List<PlikWave> ListaPlikow;
        public List<char> ListaFonow;
        public List<PlikWave> ListaSlow;
        public ArrayList ListaGlosek;
        public ArrayList ListaGlosekBajty;
        public int Id { get; set; }
        public string Wartosc { get; set; }
        public byte[] Binary { get; set; }
        public int IdP { get; set; }
        private SqlConnection _conn;        
        public string slowo;
        public byte[] razem;
        public string[] Podzielone;
        public static byte[][] tablicaBajtow;
        public byte[][] DoSprawdzeniaDwuznakow;
        

        public void pobierStringa()
        {
            this.Podzielone = PodzialNaWyrazy.podzielone;
        }//koniec metody pobierzStringa

        public bool Sprawdzenie()
        {
            int[] ID = new int[ListaPlikow.Count];
            ArrayList IDList = new ArrayList();
            int i = new int();
            i = 0;
            
            foreach(PlikWave plik in ListaPlikow)
            {

                ID[i] = plik.Id;
                IDList.Add(plik.Id);
                i++;
            }

            IDList.Sort();
            bool sequential = ID.Zip(ID.Skip(1), (a, b) => (a + 1) == b).All(x => x);
            return sequential;
        }//koniec metody Sprawdzenie

        public byte[] Noweslowo(string slowo)
        {
            _conn = new SqlConnection(@"Data Source=(localDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BazaSlow.mdf;Initial Catalog=Bazaslow;Integrated Security=True");
            char[] znaki = new char[slowo.Length];
                       
            znaki = slowo.ToCharArray();            

            ListaGlosek = new ArrayList();
            ListaGlosekBajty = new ArrayList();

            int i = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT BLOB FROM Fony WHERE Fon = @FON", _conn))
                {
                    _conn.Open();

                    foreach (char znak in znaki)
                    {                                                
                        command.Parameters.AddWithValue("@FON", znak);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ListaGlosek.Add(znak.ToString());
                                ListaGlosekBajty.Add((byte[])reader["BLOB"]);
                            }
                        }

                        command.Parameters.Clear();
                        reader.Close();
                        i++;
                    }

                   
                }

            }catch(Exception ex)
            {
               if(ex != null)
               { throw ex; }
               else
                throw new Exception("Błąd podczas pobierania danych(Fony) z bazy");
            }finally
            {
                _conn.Close();
            }                

                WaveIOFony polaczenie = new WaveIOFony();
                DoSprawdzeniaDwuznakow = SprawdzenieDwuznakow();
                
                Normalizacja normalizuj = new Normalizacja(polaczenie.Merge(DoSprawdzeniaDwuznakow));
                normalizuj.HeadOut();
                razem = normalizuj.Zrob();                

                return razem;
        }//koniec metory Noweslowo

        public byte[][] SprawdzenieDwuznakow()
        {
            for (int i = 0; i < ListaGlosek.Count; i++)
            {

                if (ListaGlosek.IndexOf("c") >= 0 &
                    ListaGlosek.IndexOf("z") >= 0)
                {

                    if (ListaGlosek.IndexOf("c", i) + 1 == ListaGlosek.IndexOf("z", i) &
                        ListaGlosek.IndexOf("z", i) - 1 == ListaGlosek.IndexOf("c", i))
                    {

                        int indexC = ListaGlosek.IndexOf("z", i) - 1;
                        int indexZ = ListaGlosek.IndexOf("c", i) + 1;

                        ListaGlosekBajty.RemoveRange(indexC, 2);
                        ZamienZnak("cz", indexC);
                        ListaGlosek.RemoveRange(indexC, 2);
                        ListaGlosek.Insert(indexC, "cz");

                    }//koniec if
                }//koniec if

                if (ListaGlosek.IndexOf("s") >= 0 &
                   ListaGlosek.IndexOf("z") >= 0)
                {

                    if (ListaGlosek.IndexOf("s", i) + 1 == ListaGlosek.IndexOf("z", i) &
                        ListaGlosek.IndexOf("z", i) - 1 == ListaGlosek.IndexOf("s", i))
                    {

                        int indexC = ListaGlosek.IndexOf("z", i) - 1;
                        int indexZ = ListaGlosek.IndexOf("s", i) + 1;
                        
                        ListaGlosekBajty.RemoveRange(indexC, 2);
                        ZamienZnak("sz", indexC);
                        ListaGlosek.RemoveRange(indexC, 2);
                        ListaGlosek.Insert(indexC, "sz");                        

                    }//koniec if
                }//koniec if

                if (ListaGlosek.IndexOf("r") >= 0 &
                   ListaGlosek.IndexOf("z") >= 0)
                {

                    if (ListaGlosek.IndexOf("r", i) + 1 == ListaGlosek.IndexOf("z", i) &
                        ListaGlosek.IndexOf("z", i) - 1 == ListaGlosek.IndexOf("r", i))
                    {

                        int indexR = ListaGlosek.IndexOf("z", i) - 1;
                        int indexZ = ListaGlosek.IndexOf("r", i) + 1;

                        ListaGlosekBajty.RemoveRange(indexR, 2);
                        ZamienZnak("ż", indexR);
                        ListaGlosek.RemoveRange(indexR, 2);
                        ListaGlosek.Insert(indexR, "ż");

                    }//koniec if
                }//koniec if

                if (ListaGlosek.IndexOf("c") >= 0 &
                   ListaGlosek.IndexOf("h") >= 0)
                {

                    if (ListaGlosek.IndexOf("c", i) + 1 == ListaGlosek.IndexOf("h", i) &
                        ListaGlosek.IndexOf("h", i) - 1 == ListaGlosek.IndexOf("c", i))
                    {

                        int indexR = ListaGlosek.IndexOf("h", i) - 1;
                        int indexZ = ListaGlosek.IndexOf("c", i) + 1;

                        ListaGlosekBajty.RemoveRange(indexR, 2);
                        ZamienZnak("h", indexR);
                        ListaGlosek.RemoveRange(indexR, 2);
                        ListaGlosek.Insert(indexR, "h");

                    }//koniec if
                }//koniec if
            }  
                return (byte[][])ListaGlosekBajty.ToArray(typeof(byte[]));
        }//koniec metody SprawdzenieDwuznakow

        public void ZamienZnak(string znak, int Index)
        {
            _conn = new SqlConnection(@"Data Source=(localDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BazaSlow.mdf;Initial Catalog=Bazaslow;Integrated Security=True");

            using (SqlCommand command = new SqlCommand("SELECT BLOB FROM Fony WHERE Fon = @FON", _conn))
            {
                _conn.Open();

                command.Parameters.AddWithValue("@FON", znak);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                        
                        ListaGlosekBajty.Insert(Index, (byte[])reader["BLOB"]);
                    }//koniec while
                }//koniec if

                command.Parameters.Clear();
                reader.Close();
                
            }//koniec using
        }//koniec metody ZamienZnak    

        public void PobierzDaneSlowa()
        {           
 
            ListaPlikow = new List<PlikWave>();
            _conn = new SqlConnection(@"Data Source=(localDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BazaSlow.mdf;Initial Catalog=Bazaslow;Integrated Security=True");

            try
            {
                using (SqlCommand command = new SqlCommand("SELECT ID, Slowo, BLOB, IdP FROM Slowa WHERE Slowo = @Slowo", _conn))
                {
                    _conn.Open();
                    for (int i = 0; i < Podzielone.Length; i++)
                    {

                        slowo = Podzielone[i];
                        command.Parameters.AddWithValue("@Slowo", slowo);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                PlikWave Wav = new PlikWave();

                                Wav.Id = (int)reader["ID"];
                                Wav.Wartosc = (string)reader["Slowo"];
                                Wav.Binary = (byte[])reader["BLOB"];

                                ListaPlikow.Add(Wav);                          
                            }
                        }
                        else
                        {
                            PlikWave Wav = new PlikWave();

                            Wav.Id = 0;
                            Wav.Wartosc = slowo;
                            Wav.Binary = null;
                            Wav.IdP = 0;

                            ListaPlikow.Add(Wav);
                        }

                        command.Parameters.Clear();
                        reader.Close();
                    }
                }
            }catch(Exception)
            {
                throw new Exception("Błąd podczas pobierania danych(Słowa) z bazy");
            }finally
            {
                _conn.Close();
            }

        }//koniec metody PobierzDaneSlowa
          
        public void ZamienNaTablice()
        {

            try
            {

                int i = 0;

                tablicaBajtow = new byte[ListaSlow.Count][];


                foreach (PlikWave plik in ListaSlow)
                {
                    
                    if (plik.Binary != null)
                    {
                        tablicaBajtow[i] = plik.Binary;

                    }//koniec if
                    else
                    {                        
                         tablicaBajtow[i] = Noweslowo(plik.Wartosc);

                    }//koniec else
                    i++;
                }//koniec foreach

            }//koniec try

            catch (Exception ex)
            {
                if(ex != null)
                { throw ex; }
                else
                {
                    throw new Exception("Błąd podczas tworzenia tablicy tablic bajtów");
                }
                
            }//koniec catch
        }//koniec metody ZamienNaTablice

        public void czytaj()
        {
            
            pobierStringa();
            PobierzDaneSlowa();
            SprawdzKolejnosc();
            ZamienNaTablice();
            
            WaveIOSlowa wa = new WaveIOSlowa();
            byte[] DoPrzeczytania = wa.Merge(tablicaBajtow);            

            try
            {
               
                    MediaPlayer mPlayer = new MediaPlayer(DoPrzeczytania);
                    mPlayer.Play();
               
                
                
            }catch(Exception)
            {
                throw new Exception("Błąd podczas odtwarzania");
            }
            
        }//koniec metody czytaj

        public List<PlikWave> SprawdzKolejnosc()
        {
            ListaSlow = new List<PlikWave>();
            for(int i = 0; i< Podzielone.Length; i++)
            {
                for(int j = 0; j <ListaPlikow.Count; j++)
                {
                    if(Podzielone[i] == ListaPlikow[j].Wartosc)
                    {
                        for(int k = 0; k < ListaPlikow.Count; k++)
                        {
                            if (i == Podzielone.Length - 1)
                            {
                                if (Podzielone[i] == ListaPlikow[k].Wartosc)
                                {
                                    ListaSlow.Add(ListaPlikow[k]);
                                    return ListaSlow;
                                }                                
                            }
                            else if (Podzielone[i + 1] == ListaPlikow[k].Wartosc)
                            {
                                if(ListaSlow.Count == 0)
                                {
                                    ListaSlow.Add(ListaPlikow[j]);
                                }
                                else if (ListaPlikow[j].Id == ListaPlikow[k].Id - 1 &
                                    ListaSlow[ListaSlow.Count - 1].Wartosc != ListaPlikow[j].Wartosc)
                                {
                                    ListaSlow.Add(ListaPlikow[j]);
                                }
                            }
                        }
                        
                        if(ListaPlikow[j+1].Wartosc == Podzielone[i] &
                           ListaSlow.Count == 0)
                        {
                            ListaSlow.Add(ListaPlikow[j]);
                        }
                        else if(ListaPlikow[j+1].Wartosc != Podzielone[i] &
                            ListaSlow[ListaSlow.Count - 1].Wartosc != ListaPlikow[j].Wartosc)
                        {
                            ListaSlow.Add(ListaPlikow[j]);
                        }
                    }
                }
            }
            return ListaSlow;
        }
       
    }//koniec klasy
}