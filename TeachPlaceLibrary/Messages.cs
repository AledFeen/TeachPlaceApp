using System;
namespace TeachPlaceApp
{
    public class Messages
    {
        private string user;
        private string email;
        private string message;
        public string User
        {
            get => user;
            set
            {
                if (value.Length < 3)
                {
                    throw new Exception("Name must have more than 3 symbols");
                }
                else if (((value.Contains("0") || value.Contains("1") || value.Contains("2") || value.Contains("3")
                  || value.Contains("4") || value.Contains("5") || value.Contains("6") || value.Contains("7")
                  || value.Contains("8") || value.Contains("9")) == true))
                {
                    throw new Exception("Name cant contained numbers");
                }
                else { user = value; }
            }
        }
        public string Email
        {
            get => email;
            set
            {
                if (value.Length < 5)
                {
                    throw new Exception("count of symbols must be more than 5");
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
        public string Message
        {
            get => message;
            set
            {
                if (value.Length < 10) { throw new Exception("Message mu have more than 10 symbols"); }
                else { message = value; }
            }
        }

        public Messages () { }
        public Messages(string user, string email, string message)
        {
            User = user;
            Email = email;
            Message = message;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
 