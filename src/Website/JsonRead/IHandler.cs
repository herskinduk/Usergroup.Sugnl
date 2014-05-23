using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usergroup.Sugnl.JsonReadSpike
{
    public interface IHandler<T>
    {
        void Process(T item);
    }
}
