using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TeachPlaceApp
{
    public class RegisteredUser : User, IComparable
    {
        private string email;
        private string phoneNumber;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }
        public int Cost { get; set; }
        public string BriefInfo { get; set; }
        public string Fullinfo { get; set; }
        public string PhotoPath { get; set; }
        public EnumExperience Experience { get; set; }
        public List<ESubject> Subjects { get; set; } = new List<ESubject>();
        public List<Request> Requests { get; set; } = new List<Request>();

        public bool IsPublicate { get; set; } = false;

        public string Email
        {
            get => email;
            set
            {
                if (value.Length < 8)
                {
                    throw new Exception("count of symbols must be more than 8");
                }
                else if (value.EndsWith(".com") == false)
                {
                    throw new Exception("You have not write .com in the end");
                }
                else if (value.Contains("@") == false)
                {
                    throw new Exception("Value must have @");
                }
                else
                {
                    email = value;
                }
            }
        }
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (value.Length < 11 || value.Length > 13)
                {
                    throw new Exception("Value must be between 11 and 13 symbols");
                }
                else if (value.StartsWith("+") == false)
                {
                    throw new Exception("Value have not plus at the start");
                } else { phoneNumber = value; }
            }
        }

        public RegisteredUser(string login, string password, string phoneNumber, string email) : base(login, password)
        {
            Login = login;
            Password = password;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        [JsonConstructor]
        public RegisteredUser(string name, string surname, string secondName, string email, string phoneNumber, int cost, string briefInfo, string fullInfo, string photoPath, EnumExperience experience, List<ESubject> subjects, List<Request> requests, string login, string password, bool isPublicate) : base(login, password)
        {
            Name = name;
            Surname = surname;
            SecondName = secondName;
            Email = email;
            PhoneNumber = phoneNumber;
            Cost = cost;
            BriefInfo = briefInfo;
            Fullinfo = fullInfo;
            PhotoPath = photoPath;
            Experience = experience;
            Subjects = subjects;
            Requests = requests;
            IsPublicate = isPublicate;
            Login = login;
            Password = password;
        }

        public int CompareTo(object o)
        {
            if (o is RegisteredUser user) return Cost.CompareTo(user.Cost);
            else throw new ArgumentException("Некорректное значение параметра");
        }
    }
}
