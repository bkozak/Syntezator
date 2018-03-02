using Syntezator.Klasy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Syntezator.Skrypty;

namespace Syntezator
{
    public partial class Default : System.Web.UI.Page
    {

        
        protected void Page_Load(object sender, EventArgs e)
        {
            //----------------------------------------Skrypt wrzucający dane do bazy danych----------------------------------------------------------------------
            //WaveToBinary dodaj = new WaveToBinary();
            //dodaj.DoBazy();

            
        }        

        protected void usunDuplikaty_btn_Click(object sender, EventArgs e)
        {
            UsuwanieDuplikatow usuwanie = new UsuwanieDuplikatow();
            string s = textbox.Text;
            duplikatybox.Text = usuwanie.UsunDuplikaty(s);            
        }

        protected void wyczysc_btn_Click(object sender, EventArgs e)
        {
            textbox.Text = "";
            duplikatybox.Text = "";
        }

        protected void czytaj_Click(object sender, EventArgs e)
        {
            try
            {

                PodzialNaWyrazy Podziel = new PodzialNaWyrazy();
                PlikWave PlikWave = new PlikWave();
                Podziel.Podzial(textbox.Text);

                PlikWave.czytaj();
            }
            catch(Exception ex)
            {
                Bledy.Text = ex.Message.ToString();
            }

            
            
        }

    }

}