using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Interception.PolicyInjection.Pipeline;

namespace UnityUtility
{
    /// <summary>
    /// 异常AOP机制
    /// </summary>
    internal class ExceptionHandler : ICallHandler
    {
        private int m_useOrder;

        public int Order
        {
            get
            {
                return m_useOrder;
            }

            set
            {
                m_useOrder = value;
            }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn returnValue = null;

            //顺序执行
            returnValue = getNext()(input, getNext);

            //若符合要求
            if (input.MethodBase is MethodInfo && (input.MethodBase as MethodInfo).ReturnType == typeof(bool))
            {
                //若有异常
                if (returnValue.Exception != null)
                {
                    //清空异常
                    returnValue.Exception = null;
                    //设置返回值
                    returnValue.ReturnValue = false;
                }
                else
                {
                    returnValue.ReturnValue = false;
                }
            }

           
           

            return returnValue;
        }
    }
}
