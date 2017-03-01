using System.Collections;
using System.Collections.Specialized;

namespace MyClassLibrary.JSON
{
    /// <summary>
    /// Json对象的包装接口,包括泛型集合和键值对接口以及新定义的判断Json的类型,针对不同的Json类型获取或设置Json数据
    /// </summary>
    public interface IJsonWrapper : IList, IOrderedDictionary
    {
        /// <summary>
        /// 获取JsonData是否是数组
        /// </summary>
        bool IsArray
        {
            get;
        }
        /// <summary>
        /// 获取JsonData是否是布尔型
        /// </summary>
        bool IsBoolean
        {
            get;
        }
        /// <summary>
        /// 获取JsonData是否是浮点型
        /// </summary>
        bool IsDouble
        {
            get;
        }
        /// <summary>
        /// 获取JsonData是否是整型
        /// </summary>
        bool IsInt
        {
            get;
        }
        /// <summary>
        /// 获取JsonData是否是长整型
        /// </summary>
        bool IsLong
        {
            get;
        }
        /// <summary>
        /// 获取JsonData是否是对象
        /// </summary>
        bool IsObject
        {
            get;
        }
        /// <summary>
        /// 获取JsonData是否是字符串
        /// </summary>
        bool IsString
        {
            get;
        }
        /// <summary>
        /// 获得布尔型值
        /// </summary>
        /// <returns></returns>
        bool GetBoolean();
        /// <summary>
        /// 获得浮点型值
        /// </summary>
        /// <returns></returns>
        double GetDouble();
        /// <summary>
        /// 获得整型值
        /// </summary>
        /// <returns></returns>
        int GetInt();
        /// <summary>
        /// 获得长整型值
        /// </summary>
        /// <returns></returns>
        long GetLong();
        /// <summary>
        /// 获得字符串值
        /// </summary>
        /// <returns></returns>
        string GetString();
        /// <summary>
        /// 设置布尔型值
        /// </summary>
        /// <param name="val">布尔型值</param>
        void SetBoolean(bool val);
        /// <summary>
        /// 设置浮点型值
        /// </summary>
        /// <param name="val">浮点型值</param>
        void SetDouble(double val);
        /// <summary>
        /// 设置整型值
        /// </summary>
        /// <param name="val">整型值</param>
        void SetInt(int val);
        /// <summary>
        /// 设置长整型值
        /// </summary>
        /// <param name="val">长整型值</param>
        void SetLong(long val);
        /// <summary>
        /// 设置字符串值
        /// </summary>
        /// <param name="val">字符串值</param>
        void SetString(string val);

        /// <summary>
        /// 获取JsonData的类型
        /// </summary>
        /// <returns></returns>
        JsonType GetJsonType();
        /// <summary>
        /// 设置JsonData的类型
        /// </summary>
        /// <param name="type">类型</param>
        void SetJsonType(JsonType type);

        /// <summary>
        /// 获取JsonData的json字符串
        /// </summary>
        /// <returns></returns>
        string ToJson();

        /// <summary>
        /// 将JsonData的json字符串写入JsonWriter对象中
        /// </summary>
        /// <param name="writer"></param>
        void ToJson(JsonWriter writer);
    }
}
