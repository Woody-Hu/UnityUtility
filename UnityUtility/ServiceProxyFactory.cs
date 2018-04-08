using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityUtility
{
    /// <summary>
    /// 代理工厂
    /// </summary>
    internal class ServiceProxyFactory
    {
        /// <summary>
        /// 建造代理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputObject"></param>
        /// <returns></returns>
        internal static T CreatProxy<T>(T inputObject)
            where T:class
        {
            if (null == inputObject)
            {
                return null;
            }
            else
            {
                var tempProxy = new ServiceProxy<T>(inputObject);
                return tempProxy.GetAOPResutl();
            }
        }
    }
}
