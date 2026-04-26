using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public interface IContactInfo
    {
        string GetContact();
        string Type { get; } 
    }
    
}
