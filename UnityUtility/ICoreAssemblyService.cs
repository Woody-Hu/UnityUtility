using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace UnityUtility
{
    /// <summary>
    /// 程序集服务
    /// </summary>
    interface ICoreAssemblyService
    {
        /// <summary>
        /// 使用的容器
        /// </summary>
        UnityContainer UseContainer { get; }

        /// <summary>
        /// 使用的类型服务
        /// </summary>
        ICoreTypeService UseTypeServie { get; }

        /// <summary>
        /// 注册一个程序集
        /// </summary>
        /// <param name="inputAssembly"></param>
        /// <param name="inputTypeService"></param>
        void RegistOneAssembly(Assembly inputAssembly);
    }
}
