using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace UnityUtility
{
    /// <summary>
    /// EF事务操作
    /// </summary>
    class EFTransactionAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return Singleton.UseSingleTonHandler;
        }

        /// <summary>
        /// 单例模式懒加载
        /// </summary>
        private class Singleton
        {
            internal readonly static ICallHandler UseSingleTonHandler = new EFTransactionHandler();
        }
    }
}
