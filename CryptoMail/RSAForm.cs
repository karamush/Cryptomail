/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CryptoMail
{
    public partial class RSAForm : Form
    {
        public String pass;
        public RSAForm(String Pass)
        {
            InitializeComponent(); pass = Pass;
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select pubkey, privkey from MyTable");

            if (tbl.Rows[0][0].ToString().Trim().Length > 0)
            {
                tbK1.Text = DesClass.Decrypt(tbl.Rows[0][0].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][1].ToString().Trim().Length > 0)
            {
                tbK2.Text = DesClass.Decrypt(tbl.Rows[0][1].ToString().Trim(), pass);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //gen
            RSACryptoServiceProvider RsaKey = new RSACryptoServiceProvider();
            string publickey = RsaKey.ToXmlString(false);
            string privatekey = RsaKey.ToXmlString(true);

            tbK1.Text = publickey;
            tbK2.Text = privatekey;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //send to server
            if (tbK1.Text.Trim() == "") { return; }
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select L1 from MyMail");
            String mail = "";
            if (tbl.Rows[0][0].ToString().Trim().Length > 0)
            {
                mail = DesClass.Decrypt(tbl.Rows[0][0].ToString().Trim(), pass);
            }
            if (mail == "") { return; }
            String pubkey = tbK1.Text.Trim().Replace("<RSAKeyValue><Modulus>", "");
            pubkey = pubkey.Trim().Replace("</Exponent></RSAKeyValue>", "");
            String result = ServerAPIClass.SendPubKey(DesClass.Encode(mail), DesClass.Encode(pubkey));
          //  tbK2.Text = "<RSAKeyValue><Modulus>" + DesClass.Decode(ServerAPIClass.ReadPubKey(DesClass.Encode(mail), "http://localhost:8080")) + "</Exponent></RSAKeyValue>";
            if (result.Trim() == "OK")
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show("ERROR");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //save to base
            if (tbK1.Text.Trim() == "") { return; }
            if (tbK2.Text.Trim() == "") { return; }

            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            sql.SQLTransact("UPDATE MyTable SET pubkey='" + DesClass.Encrypt(tbK1.Text.Trim(), pass) + "', privkey='" + DesClass.Encrypt(tbK2.Text.Trim(), pass) + "'");
            MessageBox.Show("OK");

        }

      
    }
}
