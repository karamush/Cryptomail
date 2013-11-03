/*
 * Developer Radik Khaydarov, http://rjump.net
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace CryptoMail
{
    public partial class SendMailForm : Form
    {
        public String pass;
        public SendMailForm(String Pass,String from,String text)
        {
            InitializeComponent(); 
            pass = Pass;
            
            richTextBox1.Text=text;
            LoadContacts(); 
            comboBox1.Text = from;
        }

        private void LoadContacts()//загрузка контактов
        {
            comboBox1.Items.Clear();
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select name, publickey from MyContacts");
            if (tbl.Rows.Count != 0)
            {
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    ContactClass CC = new ContactClass();
                    CC.Mail = DesClass.Decrypt(tbl.Rows[i][0].ToString().Trim(), pass);
                    CC.Publickey = DesClass.Decrypt(tbl.Rows[i][1].ToString().Trim(), pass);
                    comboBox1.DisplayMember = "Mail";
                    comboBox1.Items.Add(CC);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //send 
            if (comboBox1.SelectedIndex == -1) { return; }
            ContactClass CC = (ContactClass)comboBox1.Items[comboBox1.SelectedIndex];
            if (CC.Publickey.Trim() == "") { MessageBox.Show("No publickey"); return; }


            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            tbl = sql.SelectTable("select S1,S2,W1,W2,L1,L2,P1,P2 from MyMail");
           
            String S2 = "";
            String L1 = "";
            String L2 = "";
            String W2 = "";
            String P2 = "";            

            if (tbl.Rows[0][1].ToString().Trim().Length > 0)
            {
                S2 = DesClass.Decrypt(tbl.Rows[0][1].ToString().Trim(), pass);
            }
            
            if (tbl.Rows[0][3].ToString().Trim().Length > 0)
            {
                W2 = DesClass.Decrypt(tbl.Rows[0][3].ToString().Trim(), pass);
            }


            if (tbl.Rows[0][4].ToString().Trim().Length > 0)
            {
                L1 = DesClass.Decrypt(tbl.Rows[0][4].ToString().Trim(), pass);
            }

            if (tbl.Rows[0][5].ToString().Trim().Length > 0)
            {
                L2 = DesClass.Decrypt(tbl.Rows[0][5].ToString().Trim(), pass);
            }
            
            P2 = tbl.Rows[0][7].ToString();

            if (S2 == "") { MessageBox.Show("Options failed"); return; }
            if (W2 == "") { MessageBox.Show("Options failed"); return; }
            if (P2 == "") { MessageBox.Show("Options failed"); return; }
            if (L1 == "") { MessageBox.Show("Options failed"); return; }


            var fromAddress = new MailAddress(L1.Trim(), "");
            string fromPassword = W2.Trim();
            var toAddress = new MailAddress(CC.Mail.Trim(), "");
            
            const string subject = "Cryptomail";

            richTextBox1.Text+="**************************************************************************************";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < richTextBox1.Text.Length; i++)
            {
                sb.Append(DesClass.RSAEnc(richTextBox1.Text.Substring(0, 40), CC.Publickey));
                richTextBox1.Text = richTextBox1.Text.Remove(0, 40);
                if (richTextBox1.Text.Length <= 40)
                {
                    sb.Append(DesClass.RSAEnc(richTextBox1.Text, CC.Publickey));
                    break;
                }
            }
                     
            var smtp = new SmtpClient
            {
                Host = S2.Trim(),
                Port = Convert.ToInt32(P2.Trim()),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = sb.ToString(),
            })

            {
               // message.Headers.Add("X-Message-Flag", "Flagged");
                smtp.Send(message);
            }
            richTextBox1.Text = "";
            MessageBox.Show("OK");                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //sync
            if (comboBox1.SelectedIndex == -1) { return; }
            ContactClass CC = (ContactClass)comboBox1.Items[comboBox1.SelectedIndex];

            if (CC.Publickey.Trim() == "") {

                if (MessageBox.Show("No publickey. Sync pubkey?", "Sync pubkey", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    String result = DesClass.Decode(ServerAPIClass.ReadPubKey(DesClass.Encode(CC.Mail.ToLower().Trim())));
                    if (result.Trim() == "")
                    {
                        MessageBox.Show("ERROR");
                    }
                    else
                    {

                        String pubkey = "<RSAKeyValue><Modulus>" + result + "</Exponent></RSAKeyValue>";
                        DataTable tbl = new DataTable();
                        SQLLiteconnect sql = new SQLLiteconnect("db.db3");
                        sql.SQLTransact("UPDATE MyContacts SET publickey='" + DesClass.Encrypt(pubkey, pass) + "' where name='" + DesClass.Encrypt(CC.Mail.ToLower().Trim(), pass) + "'");
                        MessageBox.Show("OK");
                        LoadContacts();
                    }

                }
            
            }
            else
            {
                MessageBox.Show("<RSAKeyValue><Modulus>" + CC.Publickey.Trim() + "</Exponent></RSAKeyValue>");
            }                        
        }
    }
}
