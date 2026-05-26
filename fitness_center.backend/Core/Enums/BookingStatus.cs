using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum BookingStatus
    {
        Active, // записался, место занято
        Cancelled, // отменил
        NotCome, // не пришёл
        Completed, // пришёл
    }
}
