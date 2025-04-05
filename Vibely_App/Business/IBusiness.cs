using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vibely_App.Business
{
    public interface IBusiness<T>
    {
        void Add(T t);
        T? Find(int id);
        T? Find(T t);
        void Update(int id, T t);
        void Remove(int id);
    }
}
