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
    /// 核心程序集加载服务
    /// </summary>
    internal class CoreAssemblyServiceImp : ICoreAssemblyService
    {
        /// <summary>
        /// 使用的核心服务
        /// </summary>
        private ICoreTypeService m_useTypeService = null;

        /// <summary>
        /// 使用的特性属性名称
        /// </summary>
        private static Type m_compentType = typeof(CompentAttribute);

        /// <summary>
        /// 使用的容器
        /// </summary>
        public UnityContainer UseContainer
        {
            get
            {
                return m_useTypeService.UseContainer;
            }
        }

        internal CoreAssemblyServiceImp()
        {
            //初始化
            m_useTypeService = ServiceProxyFactory.CreatProxy<ICoreTypeService>(new CoreTypeServiceImp());
        }

        /// <summary>
        /// 注册一个程序集
        /// </summary>
        /// <param name="inputAssembly"></param>
        public void RegistOneAssembly(Assembly inputAssembly)
        {
            //全类型检查
            foreach (var oneType in inputAssembly.GetTypes())
            {
                //接口,抽象，无法继承跳过
                if (oneType.IsInterface || oneType.IsAbstract || oneType.IsSealed)
                {
                    continue;
                }

                //获取必须属性
                var useAttribute = (CompentAttribute)oneType.GetCustomAttribute(m_compentType);

                //没有特性跳过
                if (null == useAttribute)
                {
                    continue;
                }

                //以类的形式注册
                if (useAttribute.RegistByClass)
                {
                    m_useTypeService.RegistOneTypeByClass(oneType, useAttribute);
                }
                else
                {
                    m_useTypeService.RegistOneTypeByInterface(oneType, useAttribute);
                }

            }
        }
    }
}
