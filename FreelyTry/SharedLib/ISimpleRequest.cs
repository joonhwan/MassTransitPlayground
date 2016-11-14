using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    public interface ISimpleRequest
    {
        DateTime TimeStamp { get; }
        string CustomerId { get; }
    }
}
