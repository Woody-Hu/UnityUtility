using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace UnityUtility
{
    /// <summary>
    /// 服务代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ServiceProxy<T> : RealProxy
        where T:class
    {
        /// <summary>
        /// 核心对象
        /// </summary>
        T m_coreInstance = null;

        internal ServiceProxy(T inputCoreInstance):base(typeof(T))
        {
            m_coreInstance = inputCoreInstance;
        }

        /// <summary>
        /// 获取AOP结果对象
        /// </summary>
        /// <returns></returns>
        internal T GetAOPResutl()
        {
            return (T)this.GetTransparentProxy();
        }

        /// <summary>
        /// AOP切面
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callmessage = (IMethodCallMessage)msg;
            try
            {
        
                //调用真实方法  
                object returnValue = callmessage.MethodBase.Invoke(this.m_coreInstance, callmessage.Args);

                //制作返回值
                ReturnMessage returnmessage = new ReturnMessage(returnValue, new object[0], 0, null, callmessage);

                return returnmessage;
            }
            catch (Exception)
            {
                return new ReturnMessage(null, new object[0], 0, null, callmessage);
            }
        }
    }
}
