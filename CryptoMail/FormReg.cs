/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CryptoMail
{
    public partial class FormReg : Form
    {
        public FormReg()
        {
            InitializeComponent();
            string site = "http://localhost:8080";
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select name from PubKeySite");
            if (tbl.Rows[0][0].ToString().Trim().Length > 0)
            {
                site = tbl.Rows[0][0].ToString().Trim();
            }
            webBrowser1.Navigate(site);
        }
    }
}
