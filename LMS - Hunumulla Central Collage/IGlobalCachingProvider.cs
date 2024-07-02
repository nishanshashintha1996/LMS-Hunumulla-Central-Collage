using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS___Hunumulla_Central_Collage
{
    public interface IGlobalCachingProvider
    {
        void AddItem(string key, object value);

        object GetItem(string key);
        object GetItem(string key, bool remove);
    }
}
