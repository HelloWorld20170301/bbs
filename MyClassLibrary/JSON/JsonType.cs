using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyClassLibrary.JSON
{
    /// <summary>
    /// Json类型的枚举
    /// </summary>
    public enum JsonType
    {
        /// <summary>
        /// 其他未知类型
        /// </summary>
        None,
        /// <summary>
        /// 对象
        /// </summary>
        Object,
        /// <summary>
        /// 数组
        /// </summary>
        Array,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 整型
        /// </summary>
        Int,
        /// <summary>
        /// 长整型
        /// </summary>
        Long,
        /// <summary>
        /// 浮点型
        /// </summary>
        Double,
        /// <summary>
        /// 布尔型
        /// </summary>
        Boolean
    }
}
