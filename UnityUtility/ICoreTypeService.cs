using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace UnityUtility
{
    internal interface ICoreTypeService
    {
        UnityContainer UseContainer { get; }

        /// <summary>
        /// 注册一个外部对象
        /// </summary>
        /// <param name="inputInfo"></param>
        void RegistOneObjectInfo(ObjectIOCTypeInfo inputInfo);

        /// <summary>
        /// 利用类型注册一个类型
        /// </summary>
        /// <param name="oneType"></param>
        /// <param name="useAttribute"></param>
        void RegistOneTypeByClass(Type oneType, CompentAttribute useAttribute);

        /// <summary>
        /// 利用接口注册一个类型
        /// </summary>
        /// <param name="oneType"></param>
        /// <param name="useAttribute"></param>
        void RegistOneTypeByInterface(Type oneType, CompentAttribute useAttribute);

        /// <summary>
        /// 注册一个AOP用接口
        /// </summary>
        /// <param name="inputType"></param>
        void RefistOneAOPInterface(Type inputType);
    }
}
