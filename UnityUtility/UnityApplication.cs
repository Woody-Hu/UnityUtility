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
    /// Unity管理器
    /// </summary>
    public class UnityApplication
    {
        /// <summary>
        /// 使用的单例模式标示
        /// </summary>
        private static UnityApplication m_singleTag = null;

        /// <summary>
        /// 使用的程序集服务
        /// </summary>
        private ICoreAssemblyService m_useAssemblyService = null;

        /// <summary>
        /// 使用的Unity容器
        /// </summary>
        public UnityContainer CoreUnityContainer
        {
            get
            {
                return m_useAssemblyService.UseContainer;
            }

        }

        /// <summary>
        /// 私有构造方法
        /// </summary>
        private UnityApplication()
        {
            m_useAssemblyService = ServiceProxyFactory.CreatProxy<ICoreAssemblyService>(new CoreAssemblyServiceImp());
            
            //获取所有程序集
            var aLLAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var oneAssemblies in aLLAssemblies)
            {
                m_useAssemblyService.RegistOneAssembly(oneAssemblies);
            }

        }

        /// <summary>
        /// 获得应用程序
        /// </summary>
        /// <returns></returns>
        public static UnityApplication GetApplication()
        {
            //利用上不会并发，不考虑双重锁
            if (null == m_singleTag)
            {
                m_singleTag = new UnityApplication();
            }

            return m_singleTag;
        }



    }
}
