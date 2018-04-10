using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Unity.Interception.PolicyInjection.Pipeline;

namespace UnityUtility
{
    /// <summary>
    /// EF事务处理AOP
    /// </summary>
    internal class EFTransactionHandler : ICallHandler
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
            using (TransactionScope useScope = new TransactionScope())
            {
                returnValue = getNext()(input, getNext);
                //若没有异常则提交事务
                if (null == returnValue.Exception)
                {
                    useScope.Complete();
                }
                else
                {
                    //清空异常
                    returnValue.Exception = null;
                }
            }
            return returnValue;
        }
    }
}
