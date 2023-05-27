using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeachPlaceApp
{
    public static class WriteDataProfile
    {
        public static void updateDataProfile(string value, TableUsers table, TextBox tbName, TextBox tbSurname, TextBox tbSecondName, 
            TextBox tbPhone, TextBox tbEmail, TextBox tbBrief, TextBox tbFull, TextBox tbPrice, ComboBox cbExperience, CheckedListBox clBox ,PictureBox pictureAvatar) 
        {
            foreach (RegisteredUser item in table.selectOnlyRegistered())
            {
                if (item.Login == value)
                {
                    tbName.Text = item.Name;
                    tbSurname.Text = item.Surname;
                    tbSecondName.Text = item.SecondName;
                    tbPhone.Text = item.PhoneNumber;
                    tbEmail.Text = item.Email;
                    tbBrief.Text = item.BriefInfo;
                    tbFull.Text = item.Fullinfo;
                    tbPrice.Text = Convert.ToString(item.Cost);
                    cbExperience.SelectedIndex = ((int)item.Experience);

                    if (item.Subjects != null)
                    {
                        foreach (var subj in item.Subjects)
                        {
                            switch (subj)
                            {
                                case ESubject.Mathematics:
                                    clBox.SetItemChecked(1, true);
                                    break;
                                case ESubject.English:
                                    clBox.SetItemChecked(0, true);
                                    break;
                                case ESubject.Coding:
                                    clBox.SetItemChecked(2, true);
                                    break;
                            }
                        }
                    }

                    if (item.PhotoPath != null)
                    {
                        string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                        string folderName = "Images";
                        string fileName = $"{item.PhotoPath}";
                        string filePath = Path.Combine(projectPath, folderName, fileName);

                        pictureAvatar.Image = Image.FromFile(filePath);
                    }
                }
            }
        }
    }
}
