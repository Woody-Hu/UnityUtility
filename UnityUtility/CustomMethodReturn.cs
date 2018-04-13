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
    /// 特定返回封装
    /// </summary>
    internal class CustomMethodReturn : IMethodReturn
    {
        /// <summary>
        /// 输出类型的标签
        /// </summary>
        private const string m_useOutTypeTag = "&";

        public Exception Exception
        {
            get; set;
        }

        public IDictionary<string, object> InvocationContext
        {
            get; internal set;
        }

        public IParameterCollection Outputs
        {
            get; internal set;
        }

        public object ReturnValue
        {
            get; set;
        }

        /// <summary>
        /// 自定义返回值当异常时
        /// </summary>
        /// <param name="input"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        internal static IMethodReturn PrepareCustomReturn(IMethodInvocation input, IMethodReturn returnValue)
        {
            //若有异常
            if (returnValue.Exception != null)
            {
                //清空异常
                returnValue.Exception = null;

                var tempReturn = new CustomMethodReturn();
                tempReturn.Exception = returnValue.Exception;
                tempReturn.ReturnValue = returnValue.ReturnValue;
                tempReturn.InvocationContext = returnValue.InvocationContext;

                List<ParameterInfo> lstParameterInfo = new List<ParameterInfo>();
                List<object> lstOutPutValue = new List<object>();

                PrepareOutPutParameter(input, lstParameterInfo, lstOutPutValue);

                //设置输出值
                tempReturn.Outputs = new ParameterCollection(lstOutPutValue.ToArray(), lstParameterInfo.ToArray(), k => true);

                //若输出参数个数>0
                if (lstParameterInfo.Count > 0)
                {
                    returnValue = tempReturn;
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
        private static void PrepareOutPutParameter(IMethodInvocation input, List<ParameterInfo> lstParameterInfo, List<object> lstOutPutValue)
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
    }
}
