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
    public partial class FormOptions : Form
    {
        public String pass;
        public FormOptions(String Pass)
        {
            InitializeComponent(); 
            pass = Pass;
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select S1,S2,W1,W2,L1,L2,P1,P2 from MyMail");

            if (tbl.Rows[0][0].ToString().Trim().Length>0)
            {
                tbS1.Text = DesClass.Decrypt(tbl.Rows[0][0].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][1].ToString().Trim().Length > 0)
            {
                tbS2.Text = DesClass.Decrypt(tbl.Rows[0][1].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][2].ToString().Trim().Length > 0)
            {
                tbW1.Text = DesClass.Decrypt(tbl.Rows[0][2].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][3].ToString().Trim().Length > 0)
            {
                tbW2.Text = DesClass.Decrypt(tbl.Rows[0][3].ToString().Trim(), pass);
            }


            if (tbl.Rows[0][4].ToString().Trim().Length > 0)
            {
                tbL1.Text = DesClass.Decrypt(tbl.Rows[0][4].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][5].ToString().Trim().Length > 0)
            {
                tbL2.Text = DesClass.Decrypt(tbl.Rows[0][5].ToString().Trim(), pass);
            }             

            tbP1.Text = tbl.Rows[0][6].ToString();
            tbP2.Text = tbl.Rows[0][7].ToString();

            tbl = sql.SelectTable("select name from PubKeySite");
            if (tbl.Rows[0][0].ToString().Trim().Length > 0)
            {
                tbSite.Text = tbl.Rows[0][0].ToString().Trim();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //save
            if (tbS1.Text.Trim() == "") { return; }
            if (tbP1.Text.Trim() == "") { return; }
            if (tbL1.Text.Trim() == "") { return; }
            if (tbW1.Text.Trim() == "") { return; }
            if (tbS2.Text.Trim() == "") { return; }
            if (tbP2.Text.Trim() == "") { return; }
            if (tbL2.Text.Trim() == "") { return; }
            if (tbW2.Text.Trim() == "") { return; } 
            if (tbSite.Text.Trim() == "") { return; }

            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            sql.SQLTransact("Delete from MyMail");
            List<string> ListParam = new List<string>();
            ListParam.Add(DesClass.Encrypt(tbS1.Text.Trim(), pass));
            ListParam.Add(DesClass.Encrypt(tbS2.Text.Trim(), pass));
            ListParam.Add(DesClass.Encrypt(tbW1.Text.Trim(), pass));
            ListParam.Add(DesClass.Encrypt(tbW2.Text.Trim(), pass));
            ListParam.Add(DesClass.Encrypt(tbL1.Text.ToLower().Trim(), pass));
            ListParam.Add(DesClass.Encrypt(tbL2.Text.Trim(), pass));
            ListParam.Add(tbP1.Text.Trim());
            ListParam.Add(tbP2.Text.Trim());
            sql.SQLTransactParam("INSERT INTO MyMail (S1,S2,W1,W2,L1,L2,P1,P2,id) VALUES(?,?,?,?,?,?,?,?,0)", ListParam);

            sql.SQLTransact("UPDATE PubKeySite SET name='" + tbSite.Text.Trim() + "'");
            MessageBox.Show("OK");
            this.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tbW2.Text = tbW1.Text;
            tbL2.Text = tbL1.Text;
        }

        static string sha256(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tbW3.Text.Trim().Length < 6) { MessageBox.Show("Error. Min length = 6"); return; } 
            if (MessageBox.Show("Change your password?", "Change your password", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (tbS1.Text.Trim() == "") { return; }
                if (tbP1.Text.Trim() == "") { return; }
                if (tbL1.Text.Trim() == "") { return; }
                if (tbW1.Text.Trim() == "") { return; }
                if (tbS2.Text.Trim() == "") { return; }
                if (tbP2.Text.Trim() == "") { return; }
                if (tbL2.Text.Trim() == "") { return; }
                if (tbW2.Text.Trim() == "") { return; }
                if (tbSite.Text.Trim() == "") { return; }                
                
                SQLLiteconnect sql = new SQLLiteconnect("db.db3");
                DataTable tbl = new DataTable(); 
                List<string> ListParam = new List<string>();
           
                tbl = sql.SelectTable("select name, publickey from MyContacts");
                if (tbl.Rows.Count != 0)
                {
                    sql.SQLTransact("Delete from MyContacts"); 
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        
                       String pubkeycontact= DesClass.Decrypt(tbl.Rows[i][1].ToString().Trim(), pass);
                       String namecontact =DesClass.Decrypt(tbl.Rows[i][0].ToString().Trim(), pass);
                       sql.SQLTransact("INSERT INTO MyContacts (name, publickey) VALUES('" + DesClass.Encrypt(namecontact, tbW3.Text.Trim()) + "','" + DesClass.Encrypt(pubkeycontact, tbW3.Text.Trim()) + "')");
                    }
                }  

                String def = sha256(tbW3.Text.Trim());
                
                tbl = sql.SelectTable("select pubkey,privkey from MyTable");
                String pubkey = DesClass.Decrypt(tbl.Rows[0][0].ToString().Trim(),pass);
                String privkey = DesClass.Decrypt(tbl.Rows[0][1].ToString().Trim(), pass);
                sql.SQLTransact("Delete from MyTable");

                sql.SQLTransactParam("INSERT INTO MyTable (pass,pubkey,privkey) VALUES('" + def + "','" + DesClass.Encrypt(pubkey, tbW3.Text.Trim()) + "','" + DesClass.Encrypt(privkey, tbW3.Text.Trim()) + "')", ListParam);

                pass = tbW3.Text.Trim();
                ListParam.Clear();
                sql.SQLTransact("Delete from MyMail");
                
                ListParam.Add(DesClass.Encrypt(tbS1.Text.Trim(), pass));
                ListParam.Add(DesClass.Encrypt(tbS2.Text.Trim(), pass));
                ListParam.Add(DesClass.Encrypt(tbW1.Text.Trim(), pass));
                ListParam.Add(DesClass.Encrypt(tbW2.Text.Trim(), pass));
                ListParam.Add(DesClass.Encrypt(tbL1.Text.ToLower().Trim(), pass));
                ListParam.Add(DesClass.Encrypt(tbL2.Text.Trim(), pass));
                ListParam.Add(tbP1.Text.Trim());
                ListParam.Add(tbP2.Text.Trim());
                sql.SQLTransactParam("INSERT INTO MyMail (S1,S2,W1,W2,L1,L2,P1,P2,id) VALUES(?,?,?,?,?,?,?,?,0)", ListParam);               
                
                
                MessageBox.Show("Reopen application"); 
                Application.Exit();
            }
        }
    }
}
