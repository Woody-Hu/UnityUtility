using System;
using System.Collections.Generic;
using System.Linq;
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

        private StringBuilder m_useStringBuilder = new StringBuilder();

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
            try
            {
                m_useStringBuilder.AppendLine(input.MethodBase.Name);
                returnValue = getNext()(input, getNext);
                if (returnValue.Exception != null)
                {
                    m_useStringBuilder.AppendLine(returnValue.Exception.Message);
                }
                else
                {
                    m_useStringBuilder.AppendLine(input.MethodBase.Name + "Finish");
                }
            }
            catch (Exception)
            {
;
            }
           

            return returnValue;
        }
    }
}
