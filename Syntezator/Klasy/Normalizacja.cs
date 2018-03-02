using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syntezator.Klasy
{
    public class Normalizacja
    {
        public byte[] TablicaDoNormalizacji;
        public byte[] TablicaPoNormalizacji;
        public byte[] BezHeda;
        public byte[] HedZNormalizacja;

        public Normalizacja(byte[] TablicaWejsciowa)
        {
            this.TablicaDoNormalizacji = TablicaWejsciowa;
            this.TablicaPoNormalizacji = new byte[TablicaDoNormalizacji.Length];
            this.BezHeda = new byte[TablicaDoNormalizacji.Length - 44];
            this.HedZNormalizacja = TablicaWejsciowa;
        }

        public void HeadOut()
        {
            Buffer.BlockCopy(TablicaDoNormalizacji, 44, BezHeda, 0, BezHeda.Length);
            
        }

        short GetShortFromLittleEndianBytes(byte[] data, int startIndex)
        {
            return (short)((data[startIndex + 1] << 8)
                 | data[startIndex]);
        }

        byte[] GetLittleEndianBytesFromShort(short data)
        {
            byte[] b = new byte[2];
            b[0] = (byte)data;
            b[1] = (byte)(data >> 8 & 0xFF);
            return b;
        }

        public byte[] Zrob()
        {
            byte[] input = BezHeda;
            float biggest = -32768F;
            float sample;
            for (int i = 0; i < input.Length; i += 2)
            {
                sample = (float)GetShortFromLittleEndianBytes(input, i);
                if (sample > biggest) biggest = sample;
            }
            //-------------------------------------------------------------------
            float offset = 32767 - biggest;

            float[] data = new float[input.Length / 2];
            for (int i = 0; i < input.Length; i += 2)
            {
                data[i / 2] = (float)GetShortFromLittleEndianBytes(input, i) + offset;
            }
            //--------------------------------------------------------------------
            byte[] TablicaPoNormalizacji = new byte[input.Length];
            for (int i = 0; i < TablicaPoNormalizacji.Length; i += 2)
            {
                byte[] tmp = GetLittleEndianBytesFromShort(Convert.ToInt16(data[i / 2]));
                TablicaPoNormalizacji[i] = tmp[0];
                TablicaPoNormalizacji[i + 1] = tmp[1];
            }
            Buffer.BlockCopy(TablicaPoNormalizacji, 0, HedZNormalizacja, 44, TablicaPoNormalizacji.Length);
            return HedZNormalizacja;
        }
      
    }
}