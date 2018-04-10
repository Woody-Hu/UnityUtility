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
            throw new NotImplementedException();
        }
    }
}
