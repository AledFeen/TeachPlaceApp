using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TeachPlaceApp
{
    public class TableUsers : List<User>, ISortable<User>, IFilterable<TableUsers>
    {

        public TableUsers selectOnlyRegistered()
        {
            TableUsers table = new TableUsers();
            foreach(var item in this) 
            {
                if (item is RegisteredUser) 
                {
                    table.Add(item);
                }
            }
            return table;
        }

        public TableUsers selectOnlyAdmins()
        {
            TableUsers table = new TableUsers();
            foreach (var item in this)
            {
                if (item is Administrator)
                {
                    table.Add(item);
                }
            }
            return table;
        }

        public List<User> sort()
        {
            User[] people = new User[Count];

            for (int i = 0; i < people.Length; i++)
            {
                people[i] = this[i];
            }

            Array.Sort(people);

            for (int i = 0; i < people.Length; i++)
            {
                this[i] = people[i];
            }
            return this;
        }

        public List<User> reverse()
        {
            User[] people = new User[Count];

            for (int i = 0; i < people.Length; i++)
            {
                people[i] = this[i];
            }

            Array.Reverse(people);

            for (int i = 0; i < people.Length; i++)
            {

                this[i] = people[i];
            }
            return this;
        }

        public TableUsers filterUsers(string named)
        {
            TableUsers tabl = new TableUsers();
            foreach (RegisteredUser user in this)
            {
                if (user.Name == named)
                {
                    tabl.Add(user);
                }
            }
            return tabl;
        }

        public TableUsers filterUsers(int price, int comparison)
        {
            // comapison 0 - until price
            // comparison 1 - from price 
            TableUsers tabl = new TableUsers();
            if(comparison == 0) 
            {
                foreach (RegisteredUser user in this)
                {
                    if (user.Cost <= price)
                    {
                        tabl.Add(user);
                    }
                }
            } else if (comparison == 1) 
            {
                foreach (RegisteredUser user in this)
                {
                    if (user.Cost >= price)
                    {
                        tabl.Add(user);
                    }
                }
            } else { throw new Exception("Uncurrect argument {Comparison}"); }
            
            return tabl;
        }

        public TableUsers filterUsers(ESubject subject) 
        {
            TableUsers tabl = new TableUsers();
            foreach(RegisteredUser user in this) 
            {
                if(user.Subjects.Count != 0) 
                {
                    foreach(var item in user.Subjects) 
                    {
                        if (item.Equals(subject)) 
                        {
                            tabl.Add(user);
                        }
                    }
                }
            }
            return tabl;
        }

        public bool checkIfUserExists(string login)
        {
            int counter = 0;
            foreach (User user in this)
            {
                if (login == user.Login)
                {
                    counter++;
                }
            }
            if (counter > 0) { return true; }
            else { return false; }
        }

        public static void SaveToFileJson(TableUsers table, string path)
        {
            try
            {
                string jsonstring = "";
                foreach (var item in table)
                {
                    if (item is RegisteredUser) { jsonstring += JsonSerializer.Serialize((RegisteredUser)item); jsonstring += "\n"; }
                    else if (item is Administrator) { jsonstring += JsonSerializer.Serialize((Administrator)item); jsonstring += "\n"; }
                }
                File.WriteAllText(path, jsonstring);
            }
            catch (Exception)
            {
                throw new Exception("Ошибка добавления");
            }
        }

        public static TableUsers ReadFromFileJson(string path)
        {
            TableUsers users = new TableUsers();

            List<string> lines = new List<string>();
            lines = File.ReadAllLines(path).ToList();
            foreach (var item in lines)
            {
                try
                {
                    if (item.Contains("Cost"))
                    {
                        RegisteredUser reg = JsonSerializer.Deserialize<RegisteredUser>(item);
                        users.Add(reg);
                    }
                    else
                    {
                        Administrator adm = JsonSerializer.Deserialize<Administrator>(item);
                        users.Add(adm);
                    }
                }
                catch (IOException ex)
                {
                    throw new Exception($"Reading JSON file error: {ex.Message}");
                }
                
            }
            return users;
        }
    }   
}