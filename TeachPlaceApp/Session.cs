using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace TeachPlaceApp
{
    public class Session
    {
        public bool IsLoged { get; set; }
        public string Login { get; set; }
        
        public void SaveToFileJson(string path)
        {
            try
            {
                string jsonstring = "";
                jsonstring += JsonSerializer.Serialize(this); jsonstring += "\n"; 
                File.WriteAllText(path, jsonstring);
            }
            catch (Exception)
            {
                throw new Exception("Ошибка добавления");
            }
        }

        public static Session ReadFromFileJson(string path)
        {
            Session ses = new Session();
            string jsonString = File.ReadAllText(path);
            ses = JsonSerializer.Deserialize<Session>(jsonString);
            return ses;
        }
    }
}
