using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usergroup.Sugnl.Search
{
    public interface ICache<T>
    {
        bool ContainsKey(string key);
        T GetEntity(string key);
        void AddEntity(string key, T entity);
    }
}
