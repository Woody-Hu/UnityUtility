using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

namespace UnityUtility
{
    /// <summary>
    /// 类型核心服务实现
    /// </summary>
    internal class CoreTypeServiceImp : ICoreTypeService
    {
        /// <summary>
        /// 使用的核心容器
        /// </summary>
        private UnityContainer m_useContainer;

        internal CoreTypeServiceImp()
        {
            //初始化
            m_useContainer = new UnityContainer();
            //标记扩展
            m_useContainer.AddNewExtension<Interception>();
        }

        /// <summary>
        /// 获取容器
        /// </summary>
        public UnityContainer UseContainer
        {
            get
            {
                return m_useContainer;
            }
        }

        public void RegistOneTypeByClass(Type oneType, CompentAttribute useAttribute)
        {
            //若是单例
            if (useAttribute.Singleton)
            {
                //若赋名称
                if (!string.IsNullOrWhiteSpace(useAttribute.Name))
                {
                    m_useContainer.RegisterSingleton(oneType, useAttribute.Name);
                }
                else
                {
                    m_useContainer.RegisterSingleton(oneType);
                }
            }
            //若不是单例
            else
            {
                //若赋名称
                if (!string.IsNullOrWhiteSpace(useAttribute.Name))
                {
                    m_useContainer.RegisterType(oneType, useAttribute.Name);
                }
                else
                {
                    m_useContainer.RegisterType(oneType);
                }
            }
        }

        public void RegistOneTypeByInterface(Type oneType, CompentAttribute useAttribute)
        {
            foreach (var oneInterfaceType in oneType.GetInterfaces())
            {
                //若是单例
                if (useAttribute.Singleton)
                {
                    //若赋名称
                    if (!string.IsNullOrWhiteSpace(useAttribute.Name))
                    {
                        m_useContainer.RegisterSingleton(oneInterfaceType, oneType, useAttribute.Name);
                    }
                    else
                    {
                        m_useContainer.RegisterSingleton(oneInterfaceType, oneType);
                    }
                }
                //若不是单例
                else
                {
                    //若赋名称
                    if (!string.IsNullOrWhiteSpace(useAttribute.Name))
                    {
                        m_useContainer.RegisterType(oneInterfaceType, oneType, useAttribute.Name);
                    }
                    else
                    {
                        m_useContainer.RegisterType(oneInterfaceType, oneType);
                    }
                }
            }
        }

        /// <summary>
        /// 注册一个AOP切面接口
        /// </summary>
        /// <param name="inputType"></param>
        public void RefistOneAOPInterface(Type inputType)
        {
            m_useContainer.Configure<Interception>().SetInterceptorFor(inputType, new InterfaceInterceptor());

        }
    }
}
