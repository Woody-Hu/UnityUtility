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
        /// bean标记特性
        /// </summary>
        private static Type m_useBeanType = typeof(BeanAttribute);

        /// <summary>
        /// bean扫描类特性
        /// </summary>
        private static Type m_useBeanScanType = typeof(BeanScanAttribute);

        /// <summary>
        /// void类型
        /// </summary>
        private static Type m_useVoidType = typeof(void);

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

                //若是需要Bean扫描的类型
                if (oneType.IsClass && oneType.IsPublic && null != oneType.GetCustomAttribute(m_useBeanScanType))
                {
                    RegisitOneTypeByBeanScan(oneType);
                }
                else
                {
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
            }

            //注册AOP切面
            foreach (var oneType in lstAopInterfaceType)
            {
                m_useTypeService.RefistOneAOPInterface(oneType);
            }
        }

        /// <summary>
        /// 基于类型扫描形式的注册
        /// </summary>
        /// <param name="oneType"></param>
        private void RegisitOneTypeByBeanScan(Type oneType)
        {

            foreach (var oneMethod in oneType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                BeanAttribute tempAttribute = oneMethod.GetCustomAttribute(m_useBeanType) as BeanAttribute;

                //若没有特性或存在参数或没有返回值
                if (null == tempAttribute || oneMethod.GetParameters().Count() > 0 || oneMethod.ReturnType == m_useVoidType)
                {
                    continue;
                }

                object tempObject;

                //创建对象
                try
                {
                    tempObject = oneMethod.Invoke(null, new Object[] { });
                }
                catch (Exception)
                {
                    continue;
                }

                ObjectIOCTypeInfo tempTypeInfo;
                if (!tempAttribute.RegistByClass)
                {
                    //已接口形式注入
                    foreach (var oneInterfaceType in tempObject.GetType().GetInterfaces())
                    {
                        tempTypeInfo = new ObjectIOCTypeInfo(oneInterfaceType, tempObject, tempAttribute.Name);
                        m_useTypeService.RegistOneObjectInfo(tempTypeInfo);
                    }
                }
                else
                {
                    tempTypeInfo = new ObjectIOCTypeInfo(tempObject.GetType(), tempObject, tempAttribute.Name);
                    m_useTypeService.RegistOneObjectInfo(tempTypeInfo);
                }
            }
        }
    }
}
