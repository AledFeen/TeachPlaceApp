using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeachPlaceApp
{
    public partial class FormSendRequest : Form
    {
        public FormSendRequest()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            bool isSend = false;
            if(textBoxEmail.Text != "" && textBoxName.Text != "" && textBoxMessage.Text != "") 
            {
                foreach(RegisteredUser item in Table.table.selectOnlyRegistered()) 
                {
                    if(item.Login == Table.tempLogin) 
                    {
                        try 
                        {
                            Request req = new Request(textBoxName.Text, textBoxEmail.Text, textBoxMessage.Text);
                            item.Requests.Add(req);
                            Close();
                            isSend = true;
                        }
                        catch(Exception ex) 
                        {
                            MessageBox.Show("" + ex.Message);
                        }
                    }
                }
                if (isSend == true) 
                {
                    MessageBox.Show("Успішно відправлено");
                }
            }
        }
    }
}
