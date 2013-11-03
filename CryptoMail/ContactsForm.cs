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
using Microsoft.VisualBasic;


namespace CryptoMail
{
    public partial class ContactsForm : Form
    {
        public String pass;
        public ContactsForm(String Pass)
        {
            InitializeComponent();
            pass = Pass; ListContact();
            
        }
        public void ListContact(){
            listBox1.Items.Clear();
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
                    listBox1.DisplayMember = "Mail"; 
                    listBox1.Items.Add(CC);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String contact = Microsoft.VisualBasic.Interaction.InputBox("Input Contact", "Contact", "", this.Location.X + 100, this.Location.Y + 100);
            if (contact.Trim() == "") { return; }
            DataTable tbl = new DataTable();
            SQLLiteconnect sql = new SQLLiteconnect("db.db3");
            sql.SQLTransact("INSERT INTO MyContacts (name, publickey) VALUES('" + DesClass.Encrypt(contact.ToLower().Trim(), pass) + "','')");
            ListContact();
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (listBox1.SelectedIndex == -1) { return; }
            if (e.ClickedItem == contextMenuStrip1.Items[0])
            {
                //edit
                ContactClass CC = (ContactClass)listBox1.Items[listBox1.SelectedIndex];
                String contact = Microsoft.VisualBasic.Interaction.InputBox("Input Contact", "Contact", CC.Mail, this.Location.X + 100, this.Location.Y + 100);
                if (contact.Trim() == "") { return; }
                DataTable tbl = new DataTable();
                SQLLiteconnect sql = new SQLLiteconnect("db.db3");
                sql.SQLTransact("UPDATE MyContacts SET name='" + DesClass.Encrypt(contact.ToLower().Trim(), pass) + "' where name='" + DesClass.Encrypt(CC.Mail.ToLower().Trim(), pass) + "'");

            }
            if (e.ClickedItem == contextMenuStrip1.Items[1])
            {
                //del                
                if (MessageBox.Show("Delete the selected contact?", "Delete contact", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ContactClass CC = (ContactClass)listBox1.Items[listBox1.SelectedIndex];
                    DataTable tbl = new DataTable();
                    SQLLiteconnect sql = new SQLLiteconnect("db.db3");
                    sql.SQLTransact("Delete from MyContacts where name='" + DesClass.Encrypt(CC.Mail.ToLower().Trim(), pass) + "'");
                }
                
            }
            if (e.ClickedItem == contextMenuStrip1.Items[4])
            {
                //sync
                ContactClass CC = (ContactClass)listBox1.Items[listBox1.SelectedIndex];

                String result = DesClass.Decode(ServerAPIClass.ReadPubKey(DesClass.Encode(CC.Mail.ToLower().Trim())));
                if (result.Trim() == "")
                {
                    MessageBox.Show("ERROR");
                }
                else
                {                    
                
                String pubkey = "<RSAKeyValue><Modulus>" + result + "</Exponent></RSAKeyValue>";
                
                //MessageBox.Show(pubkey);
                DataTable tbl = new DataTable();
                SQLLiteconnect sql = new SQLLiteconnect("db.db3");
                sql.SQLTransact("UPDATE MyContacts SET publickey='" + DesClass.Encrypt(pubkey, pass) + "' where name='" + DesClass.Encrypt(CC.Mail.ToLower().Trim(), pass) + "'");
                MessageBox.Show("OK");
                }
            }

            if (e.ClickedItem == contextMenuStrip1.Items[2])
            {
                //create cryptomail
                ContactClass CC = (ContactClass)listBox1.Items[listBox1.SelectedIndex];
                SendMailForm SMF = new SendMailForm(pass, CC.Mail.Trim(), "");
                SMF.ShowDialog();
            }

            if (e.ClickedItem == contextMenuStrip1.Items[3])
            {
                //show publickey
                ContactClass CC = (ContactClass)listBox1.Items[listBox1.SelectedIndex];
                if (CC.Publickey.Trim() == "") { MessageBox.Show("No publickey"); }
                else
                {
                    MessageBox.Show("<RSAKeyValue><Modulus>" + CC.Publickey.Trim() + "</Exponent></RSAKeyValue>");
                }
            }

            ListContact();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            //create cryptomail
            ContactClass CC = (ContactClass)listBox1.Items[listBox1.SelectedIndex];
            SendMailForm SMF = new SendMailForm(pass, CC.Mail.Trim(), "");
            SMF.ShowDialog();
        }
    }
}
