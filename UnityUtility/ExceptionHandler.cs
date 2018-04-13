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
                //根据异常状态设置返回值
                if (null != returnValue.Exception)
                {
                    returnValue.ReturnValue = true;
                }
                else
                {
                    returnValue.ReturnValue = false;
                }

                //设置返回值类型
                returnValue = CustomMethodReturn.PrepareCustomReturn(input, returnValue);
            }


            return returnValue;
        }      

      
    }
}
