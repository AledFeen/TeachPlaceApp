using System.Collections.Generic;

namespace TeachPlaceApp
{
    interface ISortable<T>
    {
        public List<T> sort();
        public List<T> reverse();
    }
}
