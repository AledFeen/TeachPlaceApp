using System;

namespace TeachPlaceApp
{
    public abstract class User
    {
        private string login;
        private string password;
        public string Login 
        {
            get => login;
            set 
            {
                if(value.Length <= 4) 
                {
                    throw new Exception("Value of login must be more then 6 symbols");
                }
                else { login = value; }
            }
        }
        public string Password 
        {
            get => password; 
            set 
            {
                if(value.Length <= 6)
                {
                    throw new Exception("Value of password must be more then 6 symbols");
                } else if (value.Equals(value.ToLower())) 
                {
                    throw new Exception("Password must contains uppercase symbols");
                } else if ((value.Contains("0") || value.Contains("1") || value.Contains("2") || value.Contains("3") 
                    || value.Contains("4") || value.Contains("5") || value.Contains("6") || value.Contains("7") 
                    || value.Contains("8") || value.Contains("9")) == false) 
                {
                    throw new Exception("Password must contains number symbols");
                } else { password = value; }
            }
        }
        public User(string login, string password) 
        {
            Login = login;
            Password = password;
        }
    }
}
