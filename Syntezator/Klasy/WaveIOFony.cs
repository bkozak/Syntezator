using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Collections;

namespace Syntezator.Klasy
{
    public class WaveIOFony
    {
        private int length;
        private short channels;
        private int samplerate;
        private int DataLength;
        private short BitsPerSample;
        public byte[] koniec;
        public byte[][] files;

        private void WaveHeaderIN(byte[] spath)
        {
            Stream fs = new MemoryStream(spath);

            BinaryReader br = new BinaryReader(fs);
            length = (int)fs.Length - 8;
            fs.Position = 22;
            channels = br.ReadInt16();
            fs.Position = 24;
            samplerate = br.ReadInt32();
            fs.Position = 34;

            BitsPerSample = br.ReadInt16();
            DataLength = (int)fs.Length - 44;
            br.Close();
            fs.Close();

        }

        public byte[] Merge(byte[][] tablicaWejsciowa)
        {
            try
            {
                files = tablicaWejsciowa;

                WaveIOFony wa_IN = new WaveIOFony();
                WaveIOFony wa_out = new WaveIOFony();

                wa_out.DataLength = 0;
                wa_out.length = 0;

                //Pobranie danych o "głowie"
                foreach (byte[] path in files)
                {
                    wa_IN.WaveHeaderIN(@path);
                    wa_out.DataLength += wa_IN.DataLength;
                    wa_out.length += wa_IN.length;

                }

                //Rekonstrukcja nowej "głowy"
                wa_out.BitsPerSample = wa_IN.BitsPerSample;
                wa_out.channels = wa_IN.channels;
                wa_out.samplerate = wa_IN.samplerate;

                using (MemoryStream fs = new MemoryStream())
                {

                    BinaryWriter bw = new BinaryWriter(fs);
                    fs.Position = 0;
                    bw.Write(new char[4] { 'R', 'I', 'F', 'F' });

                    bw.Write(wa_out.length);

                    bw.Write(new char[8] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });

                    bw.Write((int)16);

                    bw.Write((short)1);
                    bw.Write(wa_out.channels);

                    bw.Write(wa_out.samplerate);

                    bw.Write((int)(wa_out.samplerate * ((wa_out.BitsPerSample * wa_out.channels) / 8)));

                    bw.Write((short)((wa_out.BitsPerSample * wa_out.channels) / 8));

                    bw.Write(wa_out.BitsPerSample);

                    bw.Write(new char[4] { 'd', 'a', 't', 'a' });
                    bw.Write(wa_out.DataLength);


                    for (int i = 0; i < files.Length; i++ )
                    {
                        if(i != 0)
                        {
                            using (MemoryStream ms = new MemoryStream(files[i]))
                            {
                                fs.Position -= 22816;
                                byte[] razem = new byte[ms.Length - 44];
                                ms.Position = 4410;

                                ms.Read(razem, 0, razem.Length - 4410);                                
                                bw.Write(razem);                                

                            }
                        }else
                        {
                            using (MemoryStream ms = new MemoryStream(files[i]))
                            {

                                byte[] razem = new byte[ms.Length - 44];
                                ms.Position = 4410;

                                ms.Read(razem, 0, razem.Length - 4410);

                                bw.Write(razem);

                            }
                        }                       
                    }

                    koniec = new byte[fs.Length];
                    fs.Position = 0;
                    fs.Read(koniec, 0, koniec.Length);

                    bw.Close();
                    fs.Close();
                    
                }

                return koniec;

            }
            catch (Exception)
            {
                throw new Exception("Błąd podczas łączenia dźwięku(Fony).");
            }
        }
        
    }
}