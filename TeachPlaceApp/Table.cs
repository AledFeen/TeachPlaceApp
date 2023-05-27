using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachPlaceApp
{
    public static class Table
    {
        public static TableUsers table { get; set; }
        public static TableUsers filterTable { get; set; }
        public static string tempLogin { get; set; } = "";
    }
}
