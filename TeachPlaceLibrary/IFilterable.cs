using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachPlaceApp
{
    interface IFilterable<T>
    {
        public T filterUsers(string named);
        public T filterUsers(int price, int comparison);
        public T filterUsers(ESubject subject);
    }
}
