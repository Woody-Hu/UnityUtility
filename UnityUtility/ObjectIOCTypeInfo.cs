using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityUtility
{
    /// <summary>
    /// 单例IOC注册对象
    /// </summary>
    public sealed class ObjectIOCTypeInfo
    {
        #region 私有字段
        /// <summary>
        /// 使用的类型
        /// </summary>
        private Type m_useType;

        /// <summary>
        /// 使用的单例对象
        /// </summary>
        private object m_useObject;

        /// <summary>
        /// 使用的名称
        /// </summary>
        private string m_useName;
        #endregion


        /// <summary>
        /// 使用的类型
        /// </summary>
        public Type UseType
        {
            get
            {
                return m_useType;
            }

            private set
            {
                m_useType = value;
            }
        }

        /// <summary>
        /// 使用的单例对象
        /// </summary>
        public object UseObject
        {
            get
            {
                return m_useObject;
            }

            private set
            {
                m_useObject = value;
            }
        }

        /// <summary>
        /// 使用的名称
        /// </summary>
        public string UseName
        {
            get
            {
                return m_useName;
            }

            private set
            {
                m_useName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="inputObject"></param>
        /// <param name="inputName"></param>
        /// <exception cref="NullReferenceException">type 或 object 为null</exception>
        public ObjectIOCTypeInfo(Type inputType,object inputObject,string inputName = null)
        {
            if (null == inputType || null == inputObject)
            {
                throw new NullReferenceException();
            }
            UseType = inputType;
            UseObject = inputObject;

            UseName = inputName;
        }

        public override int GetHashCode()
        {
            if (!string.IsNullOrWhiteSpace(UseName))
            {
                return UseType.GetHashCode() + UseName.GetHashCode();
            }
            else
            {
                return UseType.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ObjectIOCTypeInfo)
            {
                ObjectIOCTypeInfo inputInfo = obj as ObjectIOCTypeInfo;

                return inputInfo.UseType == this.UseType && inputInfo.UseName == this.UseName;
            }
            else
            {
                return base.Equals(obj);
            }
            
        }
    }
}
