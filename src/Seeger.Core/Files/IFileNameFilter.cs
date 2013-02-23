using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public interface IFileNameFilter
    {
        string Filter(string fileName);
    }
}
