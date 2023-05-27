using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TeachPlaceApp
{
    public class Administrator : User
    {
        public List<Messages> Messages { get; set; }
        public Administrator(string login, string password) : base(login, password) { }

        [JsonConstructor]
        public Administrator(List<Messages> messages, string login, string password) : base(login, password)
        {
            Messages = messages;
        }
    }
}
 