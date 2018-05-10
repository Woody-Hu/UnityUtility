using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityUtility
{
    /// <summary>
    /// Bean特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BeanAttribute: Attribute
    {
        /// <summary>
        /// 使用的名称
        /// </summary>
        private string m_Name;

        /// <summary>
        /// 是否以类型进行注册(是 此类型/否 接口）
        /// </summary>
        private bool m_RegistByClass;

        /// <summary>
        /// 使用的名称
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// 是否以类型进行注册(是 此类型/否 接口）
        /// </summary>
        public bool RegistByClass
        {
            get
            {
                return m_RegistByClass;
            }

            set
            {
                m_RegistByClass = value;
            }
        }
    }
}
