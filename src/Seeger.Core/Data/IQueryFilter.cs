using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data
{
    public interface IQueryFilter<T>
    {
        IQueryable<T> ApplyTo(IQueryable<T> input);
    }
}
