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
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputName"></param>
        /// <returns></returns>
        public T Reslove<T>(string inputName = null)
        {
            if (string.IsNullOrWhiteSpace(inputName))
            {
                return (T)CoreUnityContainer.Resolve(typeof(T));
            }
            else
            {
                return (T)CoreUnityContainer.Resolve(typeof(T),inputName);
            }    
        }

        /// <summary>
        /// 私有构造方法
        /// </summary>
        private UnityApplication(HashSet<ObjectIOCTypeInfo> lstInputIocInfo = null)
        {
            m_useAssemblyService = ServiceProxyFactory.CreatProxy<ICoreAssemblyService>(new CoreAssemblyServiceImp());

            var useTypeService = m_useAssemblyService.UseTypeServie;

            //获取所有程序集
            var aLLAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (null != lstInputIocInfo)
            {
                foreach (var oneInfo in lstInputIocInfo)
                {
                    useTypeService.RegistOneObjectInfo(oneInfo);
                }
            }

            foreach (var oneAssemblies in aLLAssemblies)
            {
                m_useAssemblyService.RegistOneAssembly(oneAssemblies);
            }

        }

        /// <summary>
        /// 获得应用程序
        /// </summary>
        /// <returns></returns>
        public static UnityApplication GetApplication(HashSet<ObjectIOCTypeInfo> lstInputIocInfo = null)
        {
            //利用上不会并发，不考虑双重锁
            if (null == m_singleTag)
            {
                m_singleTag = new UnityApplication(lstInputIocInfo);
            }

            return m_singleTag;
        }



    }
}
