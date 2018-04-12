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

        /// <summary>
        /// 输出类型的标签
        /// </summary>
        private const string m_useOutTypeTag = "&";

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

                    var tempReturn = new CustomMethodReturn();
                    tempReturn.Exception = returnValue.Exception;
                    tempReturn.ReturnValue = returnValue.ReturnValue;
                    tempReturn.InvocationContext = returnValue.InvocationContext;

                    List<ParameterInfo> lstParameterInfo = new List<ParameterInfo>();
                    List<object> lstOutPutValue = new List<object>();

                    PrepareOutPutParameter(input, lstParameterInfo, lstOutPutValue);

                    tempReturn.Outputs = new ParameterCollection(lstOutPutValue.ToArray(), lstParameterInfo.ToArray(), k => true);

                    //若输出参数个数>0
                    if (lstParameterInfo.Count > 0)
                    {
                        returnValue = tempReturn;
                    }
                   
                }
                else
                {
                    returnValue.ReturnValue = true;
                }
            }


            return returnValue;
        }

        /// <summary>
        /// 准备输出参数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lstParameterInfo"></param>
        /// <param name="lstOutPutValue"></param>
        private void PrepareOutPutParameter(IMethodInvocation input, List<ParameterInfo> lstParameterInfo, List<object> lstOutPutValue)
        {
            int tempIndex = 0;
            foreach (var oneParameter in input.MethodBase.GetParameters())
            {
                if (oneParameter.ParameterType.FullName.Contains(m_useOutTypeTag))
                {
                    lstParameterInfo.Add(oneParameter);
                    lstOutPutValue.Add(input.Arguments[tempIndex]);
                }

                tempIndex++;
            }

   
        }

        /// <summary>
        /// 特定返回封装
        /// </summary>
        private class CustomMethodReturn : IMethodReturn
        {

            public Exception Exception
            {
                get;set;
            }

            public IDictionary<string, object> InvocationContext
            {
                get;internal set;
            }

            public IParameterCollection Outputs
            {
                get;internal set;
            }

            public object ReturnValue
            {
                get; set;
            }
        }
    }
}
