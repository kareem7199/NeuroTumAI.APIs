using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Entities.Contact_Us
{
    public enum MessageStatus
    {
        [EnumMember(Value = "Pending")]
        Pending = 0,
        [EnumMember(Value = "Closed")]
        Closed = 1,
        
    }
}
