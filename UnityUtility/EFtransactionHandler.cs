using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            //若符合要求
            if (input.MethodBase is MethodInfo && (input.MethodBase as MethodInfo).ReturnType == typeof(bool))
            {
                //开启事务
                using (TransactionScope useScope = new TransactionScope())
                {
                    returnValue = getNext()(input, getNext);
                    //若没有异常则提交事务
                    if (null == returnValue.Exception)
                    {
                        useScope.Complete();
                        //设置返回值
                        returnValue.ReturnValue = true;
                    }
                    else
                    {
                        //清空异常
                        returnValue.Exception = null;
                        //设置返回值
                        returnValue.ReturnValue = false;
                    }
                }
            }
            //正常执行
            else
            {
                returnValue = getNext()(input, getNext);
            }

           
            return returnValue;
        }
    }
}
