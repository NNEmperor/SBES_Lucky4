using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract(Name = "EServerState")]
    public enum EServerState
    {
        [EnumMemberAttribute]
        Unkonwn,
        [EnumMemberAttribute]
        Primary,
        [EnumMemberAttribute]
        Secondary

    }
}
