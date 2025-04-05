using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vibely_App.API
{
    public interface IBusiness<T>
    {
        List<T> GetAll();
        void Add(T t);
        T? Find(int id);
        void Update(int id, T t);
        void Remove(int id);
    }
}
