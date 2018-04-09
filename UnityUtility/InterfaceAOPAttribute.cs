using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityUtility
{
    /// <summary>
    /// 使用AOP接口标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface,AllowMultiple = false,Inherited = false)]
    public class InterfaceAOPAttribute:Attribute
    {
    }
}
