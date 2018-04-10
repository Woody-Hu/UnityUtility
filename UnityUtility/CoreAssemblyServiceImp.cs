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
        /// 接口AOP标记特性
        /// </summary>
        private static Type m_useAopInterfaceType = typeof(InterfaceAOPAttribute);

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

        /// <summary>
        /// 获取使用的类型服务
        /// </summary>
        public ICoreTypeService UseTypeServie
        {
            get
            {
                return m_useTypeService;
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
            //待AOP的interface列表
            List<Type> lstAopInterfaceType = new List<Type>();

            //全类型检查
            foreach (var oneType in inputAssembly.GetTypes())
            {
                //无法继承跳过
                if (oneType.IsSealed)
                {
                    continue;
                }

                //若是标记的待使用的Interface
                //需要Public接口
                if (oneType.IsInterface && oneType.IsPublic && null != oneType.GetCustomAttribute(m_useAopInterfaceType))
                {
                    lstAopInterfaceType.Add(oneType);
                }

                //接口 抽象跳过
                if (oneType.IsAbstract)
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

            //注册AOP切面
            foreach (var oneType in lstAopInterfaceType)
            {
                m_useTypeService.RefistOneAOPInterface(oneType);
            }
        }
    }
}
