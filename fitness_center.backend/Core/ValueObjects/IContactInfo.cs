using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public abstract record ContactInfo
    {
        public abstract string Type { get; }
        public abstract string GetContact();
    }

}
