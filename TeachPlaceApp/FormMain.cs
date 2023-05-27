using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;


namespace TeachPlaceApp
{   
    public partial class FormMain : Form
    {
        TabControlHelper tabControl;
        Session session = new Session();
        public FormMain()
        {
            InitializeComponent();
            Table.table = new TableUsers();
            getFromFiles();
            tabControl1.BackColor = Color.Brown;
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);

            tabControl = new TabControlHelper(tabControl1);
            textBoxPasswordAuth.PasswordChar = '*';
            textBoxPasswordReg.PasswordChar = '*';
            textBoxSecPasswordReg.PasswordChar = '*';
            configureTabs();
            
            FormClosing += OnFormClosing;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            tabControl2.SelectedIndexChanged += TabControl2_SelectedIndexChanged;
            tabControl3.SelectedIndexChanged += TabControl3_SelectedIndexChanged;
            writeDataMainPage(Table.table);
            writeDataMessagePage();
            writeDataRequestPage();
            writeDataUserList();
        }

        private void tabControl1_DrawItem(Object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            TabPage _tabPage = tabControl1.TabPages[e.Index];

            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {  
                _textBrush = new SolidBrush(ColorTranslator.FromHtml("#F8F9FB"));
                g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#5061C5")), e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#C9D5FD")), e.Bounds);
            }

          

            Font _tabFont = new Font("Arial", 10.0f, FontStyle.Bold, GraphicsUnit.Pixel);
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else 
            {
                updateImages();
                saveToFiles();
            }
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTab = tabControl.SelectedTab;

            if (selectedTab.Name == "tabPageAccount")
            {
                flowLayoutPanelTeachers.Controls.Clear();
                writeDataProfile();
            } 
            else if (selectedTab.Name == "tabPageMain") 
            {
                writeDataMainPage(Table.table);
            } 
            else if (selectedTab.Name == "tabPageLog") 
            {

                foreach(Control control in Controls) 
                {
                    if(control is TextBox) { control.Text = ""; }
                }
            } 
            else if (selectedTab.Name == "tabPageRequests") 
            {
                writeDataRequestPage();
            } 
            else if (selectedTab.Name == "tabPageAdmin") 
            {
                writeDataUserList();
            }
        }

        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTab = tabControl.SelectedTab;

            if (selectedTab.Name == "tabPageAuth")
            {
                textBoxLoginAuth.Text = "";
                textBoxPasswordAuth.Text = "";
            }
            else if (selectedTab.Name == "tabPageReg")
            {
                textBoxPasswordReg.Text = "";
                textBoxPhoneReg.Text = "";
                textBoxSecPasswordReg.Text = "";
                textBoxLoginReg.Text = "";
                textBoxEmailReg.Text = "";
            }
        }

        private void TabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            TabPage selectedTab = tabControl.SelectedTab;

            if (selectedTab.Name == "tabPageMessages")
            {
                writeDataMessagePage();
            }
            else if (selectedTab.Name == "tabPageUserList")
            {
                writeDataUserList();
            }
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            bool isTrue = false;
            foreach (var items in Table.table) 
            {
                if (textBoxLoginAuth.Text == items.Login && textBoxPasswordAuth.Text == items.Password)
                {
                    session.IsLoged = true;
                    session.Login = textBoxLoginAuth.Text;
                    session.SaveToFileJson("session.json");
                    configureTabs();
                    panelContentProfile.VerticalScroll.Value = 0;
                    isTrue = true;
                }   
            }
            if (isTrue == false)
            {
                MessageBox.Show("Невірний логін чи пароль");
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            try 
            {
                if (Table.table.checkIfUserExists(textBoxLoginReg.Text) == false)
                {
                    if (textBoxPasswordReg.Text == textBoxSecPasswordReg.Text)
                    {
                        RegisteredUser reg = new RegisteredUser(textBoxLoginReg.Text, textBoxPasswordReg.Text, textBoxPhoneReg.Text, textBoxEmailReg.Text);
                        Table.table.Add(reg);
                        MessageBox.Show("Успішна реєстарція");
                    }
                    else
                        throw new Exception("Difference passwords");

                }
                else
                {
                    throw new Exception("Existing login");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"Помилка. + {ex.Message}");
            }
        }

        private void btnSendContact_Click(object sender, EventArgs e)
        {
            try 
            {
                foreach(Administrator item in Table.table.selectOnlyAdmins()) 
                {
                    Messages mes = new Messages(textBoxNameContact.Text, textBoxEmailContact.Text, textBoxMessageContact.Text);
                    item.Messages.Add(mes);
                    MessageBox.Show("Успішно відправлено.");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExitAdminProfile_Click(object sender, EventArgs e)
        {
            session.IsLoged = false;
            session.Login = "";
            configureTabs();
        }

        private void btnExitProfile_Click(object sender, EventArgs e)
        {
            session.IsLoged = false;
            session.Login = "";
            configureTabs();
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try 
            {
                bool isCorrect = checkDataProfile();
                if(isCorrect == true) 
                {
                    setDataProfile();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка. {ex.Message}");
            }
        }

        private void btnAddAvatar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if(ofd.ShowDialog() == DialogResult.OK) 
            {
                try
                {
                    pictureBoxAvatar.Image = new Bitmap(ofd.FileName);
                }
                catch 
                {
                    MessageBox.Show("Неможливо відкрити обраний файл", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void comboBoxSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                ESubject subject = ESubject.English;
                switch(comboBoxSubject.SelectedIndex) 
                {
                    case 0:
                        subject = ESubject.English;
                        break;
                    case 2:
                        subject = ESubject.Mathematics;
                        break;
                    case 1:
                        subject = ESubject.Coding;
                        break;
                }

                Table.filterTable = Table.filterTable.filterUsers(subject);
                writeDataMainPage(Table.filterTable);
            
        }

        private void comboBoxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSort.SelectedIndex != -1) 
            {
                switch (comboBoxSort.SelectedIndex)
                {
                    case 0:
                        Table.filterTable = (TableUsers)Table.filterTable.sort();
                        break;
                    case 1:
                        Table.filterTable = (TableUsers)Table.filterTable.reverse();
                        break;
                }
                writeDataMainPage(Table.filterTable);
            }
        }

        private void comboBoxFromPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFromPrice.SelectedIndex != -1) 
            {
                Table.filterTable = Table.filterTable.filterUsers(Convert.ToInt32(comboBoxFromPrice.Text), 1);
                writeDataMainPage(Table.filterTable);
            }
        }

        private void comboBoxUntilPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFromPrice.SelectedIndex != -1)
            {
                Table.filterTable = Table.filterTable.filterUsers(Convert.ToInt32(comboBoxFromPrice.Text), 0);
                writeDataMainPage(Table.filterTable);
            }
        }


        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            comboBoxSubject.SelectedIndex = -1;
            comboBoxFromPrice.SelectedIndex = -1;
            comboBoxSort.SelectedIndex = -1;
            comboBoxUntilPrice.SelectedIndex = -1;
            Table.filterTable = Table.table.selectOnlyRegistered();
            writeDataMainPage(Table.filterTable);
        }

        private void btnDeleteRequest_Click(object sender, EventArgs e)
        {
            try 
            {
                foreach (RegisteredUser item in Table.table.selectOnlyRegistered())
                {
                    if (item.Login == session.Login)
                    {
                        item.Requests.RemoveAt(Convert.ToInt32(textBoxNumberRequest.Text));
                    }
                }
                writeDataRequestPage();
                textBoxNumberRequest.Text = "";
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Помиилка." + ex.Message);
            }
        }

        private void btnFingLogin_Click(object sender, EventArgs e)
        {
            flowLayoutUserList.Controls.Clear();
            if (textBoxFindlogin.Text != "") 
            {
                foreach (RegisteredUser item in Table.table.selectOnlyRegistered())
                {
                    if (item.Login == textBoxFindlogin.Text)
                    {
                       
                        createPanelUser(item);
                    }
                }
            }
            else { writeDataUserList(); }
        }

        private void configureTabs()
        {
            tabControl.HidePage(tabPageContact);
            tabControl.HidePage(tabPageLog);
            tabControl.HidePage(tabPageAccount);
            tabControl.HidePage(tabPageAdmin);
            tabControl.HidePage(tabPageRequests);

            bool type = isAdmin();
            if (session.IsLoged == false)
            {
                tabControl.ShowPage(tabPageContact);
                tabControl.ShowPage(tabPageLog);
            }
            else
            {
                if (type == false)
                {
                    tabControl.ShowPage(tabPageContact);
                    tabControl.ShowPage(tabPageAccount);
                    tabControl.ShowPage(tabPageRequests);
                }
                else
                {
                   //tabControl.ShowPage(tabPageMain);
                    tabControl.ShowPage(tabPageAdmin);
                }
            }
        }

        private bool isAdmin() 
        {
            bool isTrue = false;
            foreach(var item in Table.table) 
            {
                if(item.Login == session.Login) 
                {
                    if(item is Administrator) 
                    {
                        isTrue = true;
                    } 
                }
            }
            return isTrue;
        }

        private void saveToFiles()
        {
            TableUsers.SaveToFileJson(Table.table, "users.json");
            session.SaveToFileJson("session.json");
        }
        private void getFromFiles()
        {
            var table1 = TableUsers.ReadFromFileJson("users.json");
            foreach (var item in table1)
            {
                Table.table.Add(item);
            }
            session = Session.ReadFromFileJson("session.json");
            Table.filterTable = Table.table.selectOnlyRegistered();
        }

        private bool checkDataProfile()
        {
            bool isAllCorrect = true;

            //check Name
            if (textBoxName.Text != "")
            {
                if (textBoxName.Text.Length < 2)
                {
                    isAllCorrect = false;
                    throw new Exception("Ім'я повинно містити быльше двох символів");
                }
                else if (textBoxName.Text.Substring(0, 1).ToUpper() != (textBoxName.Text.Substring(0, 1)))
                {
                    isAllCorrect = false;
                    throw new Exception("Ім'я повинно починатися з великої літери");
                }
            }

            //check Surname
            if (textBoxSurname.Text != "")
            {
                if (textBoxSurname.Text.Length < 2)
                {
                    isAllCorrect = false;
                    throw new Exception("Прізвище повинно містити быльше двох символів");
                }
                else if (textBoxSurname.Text.Substring(0, 1).ToUpper() != (textBoxSurname.Text.Substring(0, 1)))
                {
                    isAllCorrect = false;
                    throw new Exception("Прізвище повинно починатися з великої літери");
                }
            }

            //check Second Name
            if (textBoxSecName.Text != "")
            {
                if (textBoxSecName.Text.Length < 2)
                {
                    isAllCorrect = false;
                    throw new Exception("По-батькові повинно містити быльше двох символів");
                }
                else if (textBoxSecName.Text.Substring(0, 1).ToUpper() != (textBoxSecName.Text.Substring(0, 1)))
                {
                    isAllCorrect = false;
                    throw new Exception("По-батькові повинно починатися з великої літери");
                }
            }

            //check Phone 
            if (textBoxPhone.Text.Length < 11 || textBoxPhone.Text.Length > 13)
            {
                throw new Exception("Значення номеру телефону повинно містити від 11 до 13 символів");
            }
            else if (textBoxPhone.Text.StartsWith("+") == false)
            {
                throw new Exception("Ви не вказали + на початку номеру телефону");
            }

            //string res = textBoxPhone.Text.Remove(0, 1);
            //if ( int.TryParse(res, out int v) == false ) 
            //{
            //    throw new Exception("Значення номеру телефону не повинно містити ніяких символів окрім + на початку та цифр");
            //}

            //check Email
            if (textBoxEmail.Text.Length < 8)
            {
                throw new Exception("Кількість символів у пошті повинно бути більше 8");
            }
            else if (textBoxEmail.Text.EndsWith(".com") == false)
            {
                throw new Exception("У пошті відсутнє .com");
            }
            else if (textBoxEmail.Text.Contains("@") == false)
            {
                throw new Exception("У значенні пошти відсутній символ @");
            }

            //check brief description
            if (textBoxBriefDesc.Text != "")
            {
                if (textBoxBriefDesc.Text.Length < 50)
                {
                    throw new Exception("Коротка інформація має містити не менше 50 символів");
                }
            }

            //check full sescription
            if (textBoxFullDesc.Text != "")
            {
                if (textBoxFullDesc.Text.Length < 150)
                {
                    throw new Exception("Повна інформація має містити не менше 150 символів");
                }
            }

            //check price
            if (textBoxPrice.Text != "")
            {
                if (int.TryParse(textBoxPrice.Text, out int v) == false)
                {
                    throw new Exception("Значення ставки не повинно містити ніяких символів окрім цифр");
                }
                else if (int.Parse(textBoxPrice.Text) < 0)
                {
                    throw new Exception("Ставка не може бути менше 0");
                }
            }

            return isAllCorrect;
        }

        private void setDataProfile()
        {
            foreach (RegisteredUser item in Table.table.selectOnlyRegistered())
            {
                if (item.Login == session.Login)
                {
                    item.Name = textBoxName.Text;
                    item.Surname = textBoxSurname.Text;
                    item.SecondName = textBoxSecName.Text;
                    item.PhoneNumber = textBoxPhone.Text;
                    item.Email = textBoxEmail.Text;
                    item.BriefInfo = textBoxBriefDesc.Text;
                    item.Fullinfo = textBoxFullDesc.Text;
                    item.Cost = Convert.ToInt32(textBoxPrice.Text);

                    switch (comboBoxExperience.SelectedIndex)
                    {
                        case 0:
                            item.Experience = EnumExperience.less1;
                            break;
                        case 1:
                            item.Experience = EnumExperience.from1to2;
                            break;
                        case 2:
                            item.Experience = EnumExperience.from2to5;
                            break;
                        case 3:
                            item.Experience = EnumExperience.from5to10;
                            break;
                        case 4:
                            item.Experience = EnumExperience.more10;
                            break;
                    }

                    List<ESubject> subj = new List<ESubject>();
                    for (int i = 0; i < checkedListBoxSubjects.Items.Count; ++i)
                    {
                        if (checkedListBoxSubjects.GetItemChecked(i))
                        {
                            subj.Add((ESubject)Enum.ToObject(typeof(ESubject), i));
                        }
                    }
                    item.Subjects = subj;

                    if (pictureBoxAvatar.Image != null)
                    {
                        string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                        string folderName = "Images";
                        string fileName = $"{session.Login}.{pictureBoxAvatar.Image.RawFormat}";
                        string filePath = Path.Combine(projectPath, folderName, fileName);


                        if (filePath == item.PhotoPath)
                        {
                            fileName = $"{session.Login}-1.{pictureBoxAvatar.Image.RawFormat}";
                            filePath = Path.Combine(projectPath, folderName, fileName);
                            pictureBoxAvatar.Image.Save(filePath);
                            item.PhotoPath = filePath;
                        }
                        else
                        {
                            pictureBoxAvatar.Image.Save(filePath);
                            item.PhotoPath = filePath;
                        }
                    }
                    item.IsPublicate = true;
                }
            }
        }

        private void updateImages()
        {
            foreach (RegisteredUser item in Table.table.selectOnlyRegistered())
            {
                string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string folderName = "Images";
                string fileName = $"{item.Login}.Jpeg";
                string filePath = Path.Combine(projectPath, folderName, fileName);
                string fileName1 = $"{item.Login}-1.Jpeg";
                string filePath1 = Path.Combine(projectPath, folderName, fileName1);

                try
                {
                    if (item.PhotoPath == filePath)
                    {
                        if (File.Exists(filePath1))
                        {
                            File.Delete(filePath1);
                        }
                    }
                    else if (item.PhotoPath == filePath1)
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        private void writeDataProfile()
        {
            WriteDataProfile.updateDataProfile(session.Login, Table.table, textBoxName, textBoxSurname, textBoxSecName,
                textBoxPhone, textBoxEmail, textBoxBriefDesc, textBoxFullDesc, textBoxPrice, comboBoxExperience, checkedListBoxSubjects, pictureBoxAvatar);
        }

        private void writeDataMessagePage() 
        {
            flowLayoutMessages.Controls.Clear();
            foreach (Administrator item in Table.table.selectOnlyAdmins())
            {
                if (item.Login == session.Login)
                {
                    for (int i = 0; i < item.Messages.Count; i++)
                    {
                        Messages mes = item.Messages[i];

                        Label labelNameMessage = new Label();
                        labelNameMessage.Location = new System.Drawing.Point(82, 45);
                        labelNameMessage.Name = "labelNameMessage";
                        labelNameMessage.Size = new System.Drawing.Size(158, 31);
                        labelNameMessage.TabIndex = 2;
                        labelNameMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelNameMessage.Text = mes.User;

                        Label labelEmailMessage = new Label();
                        labelEmailMessage.Location = new System.Drawing.Point(255, 45);
                        labelEmailMessage.Name = "labelEmailMessage";
                        labelEmailMessage.Size = new System.Drawing.Size(158, 31);
                        labelEmailMessage.TabIndex = 1;
                        labelEmailMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelEmailMessage.Text = mes.Email;

                        Label labelNumberMessage = new Label();
                        labelNumberMessage.Location = new System.Drawing.Point(36, 45);
                        labelNumberMessage.Name = "labelNumberMessage";
                        labelNumberMessage.Size = new System.Drawing.Size(40, 31);
                        labelNumberMessage.TabIndex = 0;
                        labelNumberMessage.TextAlign = ContentAlignment.MiddleCenter;
                        labelNumberMessage.Text = $"{i}";

                        Button btnMessage = new Button();
                        btnMessage.Location = new System.Drawing.Point(428, 38);
                        btnMessage.Name = "btnMessage" + i;
                        btnMessage.Size = new System.Drawing.Size(126, 38);
                        btnMessage.TabIndex = 3;
                        btnMessage.Text = "Детальніше";
                        btnMessage.BackColor = Color.FromArgb(243, 212, 8);
                        btnMessage.Click += new System.EventHandler(btnMessage_Click);

                        Panel panelMessage = new Panel();
                        panelMessage.Location = new System.Drawing.Point(3, 3);
                        panelMessage.Name = "panelMessage";
                        panelMessage.Size = new System.Drawing.Size(573, 112);
                        panelMessage.TabIndex = 0;
                        panelMessage.BackColor = Color.FromArgb(201, 213, 253);
                        panelMessage.Controls.Add(btnMessage);
                        panelMessage.Controls.Add(labelNameMessage);
                        panelMessage.Controls.Add(labelEmailMessage);
                        panelMessage.Controls.Add(labelNumberMessage);

                        flowLayoutMessages.Controls.Add(panelMessage);
                    }
                }
            }
        }

        private void btnMessage_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                foreach (Administrator item in Table.table.selectOnlyAdmins())
                {
                    if (item.Login == session.Login)
                    {
                        for (int i = 0; i < item.Messages.Count; i++)
                        {
                            if (btn.Name == "btnMessage" + i)
                            {
                                Messages mes = item.Messages[i];
                                FormRequest formReq = new FormRequest();
                                TextBox tbMessage = (TextBox)formReq.Controls["textBoxMessage"];
                                Label labelName = (Label)formReq.Controls["labelName"];
                                Label labelEmail = (Label)formReq.Controls["labelEmail"];


                                labelName.Text = mes.User;
                                labelEmail.Text = mes.Email;
                                tbMessage.Text = mes.Message;

                                formReq.ShowDialog();
                            }
                        }
                    }
                }
            }
        }

        private void writeDataRequestPage()
        {
            flowLayoutRequests.Controls.Clear();
            foreach (RegisteredUser item in Table.table.selectOnlyRegistered()) 
            {
                if(item.Login == session.Login) 
                {
                    for (int i = 0; i < item.Requests.Count; i++)
                    {
                        Request req = item.Requests[i];

                        Label labelNameRequest = new Label();
                        labelNameRequest.Location = new System.Drawing.Point(60, 45);
                        labelNameRequest.Name = "labelNameRequest";
                        labelNameRequest.Size = new System.Drawing.Size(165, 31);
                        labelNameRequest.TabIndex = 2;
                        labelNameRequest.TextAlign = ContentAlignment.MiddleCenter;
                        labelNameRequest.Text = req.User;

                        Label labelEmailRequest = new Label();
                        labelEmailRequest.Location = new System.Drawing.Point(230, 45);
                        labelEmailRequest.Name = "labelEmailRequest";
                        labelEmailRequest.Size = new System.Drawing.Size(185, 31);
                        labelEmailRequest.TabIndex = 1;
                        labelEmailRequest.TextAlign = ContentAlignment.MiddleCenter;
                        labelEmailRequest.Text = req.Email;

                        Label labelNumberRequest = new Label(); 
                        labelNumberRequest.Location = new System.Drawing.Point(20, 45);
                        labelNumberRequest.Name = "labelNumberRequest";
                        labelNumberRequest.Size = new System.Drawing.Size(40, 31);
                        labelNumberRequest.TabIndex = 0;
                        labelNumberRequest.TextAlign = ContentAlignment.MiddleCenter;
                        labelNumberRequest.Text = $"{i}";

                        Button btnRequest = new Button();
                        btnRequest.Location = new System.Drawing.Point(442, 41);
                        btnRequest.Name = "btnRequest" + i;
                        btnRequest.Size = new System.Drawing.Size(115, 42);
                        btnRequest.TabIndex = 3;
                        btnRequest.Text = "Детальніше";
                        btnRequest.UseVisualStyleBackColor = true;
                        btnRequest.BackColor = Color.FromArgb(243, 212, 8);
                        btnRequest.Click += new System.EventHandler(btnRequest_Click);

                        Panel panelRequest = new Panel();
                        panelRequest.Location = new System.Drawing.Point(3, 3);
                        panelRequest.Name = "panelRequest";
                        panelRequest.Size = new System.Drawing.Size(573, 112);
                        panelRequest.TabIndex = 0;
                        panelRequest.BackColor = Color.FromArgb(201, 213, 253);
                        panelRequest.Controls.Add(btnRequest);
                        panelRequest.Controls.Add(labelNameRequest);
                        panelRequest.Controls.Add(labelEmailRequest);
                        panelRequest.Controls.Add(labelNumberRequest);

                        flowLayoutRequests.Controls.Add(panelRequest);
                    }
                }
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                foreach (RegisteredUser item in Table.table.selectOnlyRegistered())
                {
                    if (item.Login == session.Login)
                    {
                        for (int i = 0; i < item.Requests.Count; i++)
                        {
                            if (btn.Name == "btnRequest" + i) 
                            {
                                Request req = item.Requests[i];
                                FormRequest formReq = new FormRequest();
                                TextBox tbMessage = (TextBox)formReq.Controls["textBoxMessage"];
                                Label labelName = (Label)formReq.Controls["labelName"];
                                Label labelEmail = (Label)formReq.Controls["labelEmail"];

                                
                                labelName.Text = req.User;
                                labelEmail.Text = req.Email;
                                tbMessage.Text = req.Message;

                                formReq.ShowDialog();
                            }
                        }
                    }
                }
            }
        }

        private void writeDataUserList()
        {
            flowLayoutUserList.Controls.Clear();
            foreach (RegisteredUser item in Table.table.selectOnlyRegistered())
            {
                createPanelUser(item);
            }
        }

        private void createPanelUser(RegisteredUser item) 
        {
            // pictureBoxUser
            PictureBox pictureBoxUser = new PictureBox();
            pictureBoxUser.Location = new System.Drawing.Point(33, 16);
            pictureBoxUser.Name = "pictureBoxUser";
            pictureBoxUser.Size = new System.Drawing.Size(80, 90);
            pictureBoxUser.TabIndex = 0;
            pictureBoxUser.TabStop = false;
            pictureBoxUser.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxUser.ImageLocation = item.PhotoPath;

            // labelLogin
            Label labelLogin = new Label();
            labelLogin.Location = new System.Drawing.Point(158, 16);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new System.Drawing.Size(217, 31);
            labelLogin.TabIndex = 1;
            labelLogin.Text = item.Login;

            // labelName
            Label labelName = new Label();
            labelName.Location = new System.Drawing.Point(158, 45);
            labelName.Name = "labelName";
            labelName.Size = new System.Drawing.Size(217, 31);
            labelName.TabIndex = 2;
            labelName.Text = item.Name;

            // labelSurname
            Label labelSurname = new Label();
            labelSurname.Location = new System.Drawing.Point(158, 76);
            labelSurname.Name = "labelSurname";
            labelSurname.Size = new System.Drawing.Size(217, 31);
            labelSurname.TabIndex = 3;
            labelSurname.Text = item.Surname;

            // btnProfile
            Button btnProfile = new Button();
            btnProfile.Location = new System.Drawing.Point(400, 16);
            btnProfile.Name = "btnProfile" + item.Login;
            btnProfile.Size = new System.Drawing.Size(145, 40);
            btnProfile.TabIndex = 4;
            btnProfile.Text = "Профіль";
            btnProfile.BackColor = Color.FromArgb(39, 27, 128);
            btnProfile.ForeColor = Color.White;

            btnProfile.Click += new System.EventHandler(btnProfile_Click);

            // btnBan
            Button btnBan = new Button();
            btnBan.Location = new System.Drawing.Point(400, 67);
            btnBan.Name = item.Login;
            btnBan.Size = new System.Drawing.Size(145, 40);
            btnBan.TabIndex = 5;
            btnBan.Text = "Заблокувати";
            btnBan.BackColor = Color.FromArgb(243, 212, 8);

            btnBan.Click += new System.EventHandler(btnBan_Click);

            Panel panelUser = new Panel();
            panelUser.Location = new System.Drawing.Point(3, 3);
            panelUser.Name = "panelUser";
            panelUser.Size = new System.Drawing.Size(573, 121);
            panelUser.TabIndex = 0;
            panelUser.BackColor = Color.FromArgb(201, 213, 253);
            panelUser.Controls.Add(btnBan);
            panelUser.Controls.Add(btnProfile);
            panelUser.Controls.Add(labelSurname);
            panelUser.Controls.Add(labelName);
            panelUser.Controls.Add(labelLogin);
            panelUser.Controls.Add(pictureBoxUser);

            flowLayoutUserList.Controls.Add(panelUser);
        }

        private void btnBan_Click(object sender, EventArgs e) 
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                foreach (RegisteredUser item in Table.table.selectOnlyRegistered()) 
                {
                    if (item.Login == btn.Name) 
                    {
                        Table.table.Remove(item);
                    }
                }
            }
            writeDataUserList();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                foreach (RegisteredUser item in Table.table.selectOnlyRegistered())
                {
                    if ("btnProfile" + item.Login == btn.Name)
                    {
                        setFormProfileData(item);
                    }
                }
            }
        }

        private void writeDataMainPage(TableUsers table)
        {
            flowLayoutPanelTeachers.Controls.Clear();
            foreach (RegisteredUser item in table.selectOnlyRegistered())
                if (item.IsPublicate == true) 
                {
                    Label labelExperience = new Label();
                    labelExperience.AutoSize = true;
                    labelExperience.Location = new System.Drawing.Point(314, 141);
                    labelExperience.Name = "labelExperience" + item.Login;
                    labelExperience.Size = new System.Drawing.Size(125, 31);
                    labelExperience.TabIndex = 4;

                    string exp = "";
                    if(item.Experience == EnumExperience.less1) 
                    {
                        exp = "менше року";
                    } else if (item.Experience == EnumExperience.from1to2) 
                    {
                        exp = "від 1 до 2 років";
                    }
                    else if (item.Experience == EnumExperience.from2to5) 
                    {
                        exp = "від 2 до 5 років";
                    }
                    else if (item.Experience == EnumExperience.from5to10) 
                    {
                        exp = "від 5 до 10 років";
                    }
                    else if (item.Experience == EnumExperience.more10) 
                    {
                        exp = "більше 10 років";
                    }

                    labelExperience.Text = "Досвід:" + " " + exp;

                    Label labelPrice = new Label();
                    labelPrice.AutoSize = true;
                    labelPrice.Location = new System.Drawing.Point(314, 83);
                    labelPrice.Name = "labelPrice" + item.Login;
                    labelPrice.Size = new System.Drawing.Size(65, 31);
                    labelPrice.TabIndex = 3;
                    labelPrice.Text = "Ціна: " + item.Cost + " грн";

                    Label labelName = new Label();
                    labelName.AutoEllipsis = true;
                    labelName.Width = 300;
                    labelName.Location = new System.Drawing.Point(314, 22);
                    labelName.Name = "labelSubjects" + item.Login;
                    labelName.Size = new System.Drawing.Size(250, 31);
                    labelName.TabIndex = 2;
                    labelName.Text = item.Name + " " + item.Surname;

                    Label labelSubject = new Label();
                    labelSubject.AutoSize = true;
                    labelSubject.Location = new System.Drawing.Point(46, 186);
                    labelSubject.Name = "labelName" + item.Login;
                    labelSubject.Size = new System.Drawing.Size(71, 31);
                    labelSubject.TabIndex = 1;
                    string strSubjects = "Предмети: ";

                    if (item.Subjects != null)
                    {
                        foreach (var subj in item.Subjects)
                        {
                            switch ((int)subj)
                            {
                                case 0:
                                    strSubjects += " англійська";
                                    break;
                                case 1:
                                    strSubjects += " математика";
                                    break;
                                case 2:
                                    strSubjects += " програмування";
                                    break;
                            }
                        }
                    }

                    labelSubject.Text = strSubjects;

                    Label labelMore = new Label();
                    labelMore.AutoSize = true;
                    labelMore.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
                    labelMore.Location = new System.Drawing.Point(481, 212);
                    labelMore.Name = "labelMore" + item.Login;
                    labelMore.Size = new System.Drawing.Size(123, 28);
                    labelMore.TabIndex = 5;
                    labelMore.Text = "Докладніше";
                    labelMore.Click += new System.EventHandler(labelMore_Click);

                    PictureBox pictureBoxAvatar1 = new PictureBox();
                    pictureBoxAvatar1.Location = new System.Drawing.Point(46, 22);
                    pictureBoxAvatar1.Name = "pictureBoxAvatar" + item.Login;
                    pictureBoxAvatar1.Size = new System.Drawing.Size(150, 150);
                    pictureBoxAvatar1.TabIndex = 0;
                    pictureBoxAvatar1.TabStop = false;
                    pictureBoxAvatar1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBoxAvatar1.ImageLocation = item.PhotoPath;

                    Panel panel = new Panel();
                    panel.Location = new System.Drawing.Point(3, 3);
                    panel.Name = "panel" + item.Login;
                    panel.Size = new System.Drawing.Size(640, 250);
                    panel.BackColor = Color.FromArgb(201, 213, 253);
                    panel.TabIndex = 0;

                    panel.Controls.Add(labelMore);
                    panel.Controls.Add(labelExperience);
                    panel.Controls.Add(labelPrice);
                    panel.Controls.Add(labelSubject);
                    panel.Controls.Add(labelName);
                    panel.Controls.Add(pictureBoxAvatar1);
                    flowLayoutPanelTeachers.Controls.Add(panel);
                }
        }

        private void labelMore_Click(object sender, EventArgs e)
        {
            if (sender is Label)
            {
                Label label = (Label)sender;
                foreach (RegisteredUser item in Table.table.selectOnlyRegistered())
                {
                    if ("labelMore" + item.Login == label.Name)
                    {
                        setFormProfileData(item);
                    }
                }
            }
        }

        private void setFormProfileData(RegisteredUser item) 
        {
            FormProfile formProfile = new FormProfile();

            Panel panel = (Panel)formProfile.Controls["panelContentProfile"];


            TextBox tbName = (TextBox)panel.Controls["textBoxName"];
            TextBox tbSurname = (TextBox)panel.Controls["textBoxSurname"];
            TextBox tbSecName = (TextBox)panel.Controls["textBoxSecName"];
            TextBox tbPhone = (TextBox)panel.Controls["textBoxPhone"];
            TextBox tbEmail = (TextBox)panel.Controls["textBoxEmail"];
            TextBox tbBrief = (TextBox)panel.Controls["textBoxBriefDesc"];
            TextBox tbFull = (TextBox)panel.Controls["textBoxFullDesc"];
            TextBox tbPrice = (TextBox)panel.Controls["textBoxPrice"];   
            ComboBox cbExperience = (ComboBox)panel.Controls["comboBoxExperience"];
            CheckedListBox clbSubjects = (CheckedListBox)panel.Controls["checkedListBoxSubjects"];
            PictureBox pbAvatar = panel.Controls["pictureBoxAvatar"] as PictureBox;
            WriteDataProfile.updateDataProfile(item.Login, Table.table, tbName, tbSurname, tbSecName,
            tbPhone, tbEmail, tbBrief, tbFull, tbPrice, cbExperience, clbSubjects, pbAvatar);

            tbName.ReadOnly = true;
            tbSurname.ReadOnly = true;
            tbSecName.ReadOnly = true;
            
            tbPhone.Hide();
            tbEmail.Hide();
            tbBrief.ReadOnly = true;
            tbFull.ReadOnly = true;
            tbPrice.ReadOnly = true;
            cbExperience.Enabled = false;
            clbSubjects.Enabled = false;

            //temp
            Table.tempLogin = item.Login;
            formProfile.ShowDialog();
        }

        
    }
}
