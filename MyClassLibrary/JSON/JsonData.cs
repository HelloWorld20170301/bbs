using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace MyClassLibrary.JSON
{
    /// <summary>
    /// JSON对象
    /// </summary>
    public class JsonData : IJsonWrapper, IEquatable<JsonData>
    {
        #region 私有变量
        /// <summary>
        /// 当JsonData为数组时该变量有效
        /// </summary>
        private IList<JsonData> inst_array;

        /// <summary>
        /// 当JsonData为布尔型时该变量有效
        /// </summary>
        private bool inst_boolean;

        /// <summary>
        /// 当Jsondata为浮点型时该变量有效
        /// </summary>
        private double inst_double;

        /// <summary>
        /// 当JsonData为整形时该变量有效
        /// </summary>
        private int inst_int;

        /// <summary>
        /// 当JsonData为长整型时该变量有效
        /// </summary>
        private long inst_long;

        /// <summary>
        /// 当JsonData为字符串时该变量有效
        /// </summary>
        private string inst_string;

        /// <summary>
        /// 当JsonData为对象时该变量有效,采用字典保存Json对象的属性
        /// </summary>
        private IDictionary<string, JsonData> inst_object;

        /// <summary>
        /// 保存该JsonData的json字符串
        /// </summary>
        private string json;

        /// <summary>
        /// 该JsonData的类型
        /// </summary>
        private JsonType type;

        // Used to implement the IOrderedDictionary interface
        private IList<KeyValuePair<string, JsonData>> object_list;
        #endregion


        #region 公有属性计算JsonData对象属性的数目
        /// <summary>
        /// 计算JSON属性的数目,当JsonData为JArray或JObject类型并且JsonData已经被初始化该属性有效，否则抛出异常“JsonData未被初始化”
        /// </summary>
        public int Count
        {
            get { return EnsureCollection().Count; }
        }
        #endregion

        #region 公有属性判断JsonData对象的类型
        /// <summary>
        /// 判断是否是Array
        /// </summary>
        public bool IsArray
        {
            get { return type == JsonType.Array; }
        }
        /// <summary>
        /// 判断是否是Bool
        /// </summary>
        public bool IsBoolean
        {
            get { return type == JsonType.Boolean; }
        }
        /// <summary>
        /// 判断是否是Double
        /// </summary>
        public bool IsDouble
        {
            get { return type == JsonType.Double; }
        }
        /// <summary>
        /// 判断是否是Int
        /// </summary>
        public bool IsInt
        {
            get { return type == JsonType.Int; }
        }
        /// <summary>
        /// 判断是否是Long
        /// </summary>
        public bool IsLong
        {
            get { return type == JsonType.Long; }
        }
        /// <summary>
        /// 判断是否是Object
        /// </summary>
        public bool IsObject
        {
            get { return type == JsonType.Object; }
        }
        /// <summary>
        /// 判断是否是String
        /// </summary>
        public bool IsString
        {
            get { return type == JsonType.String; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 空构造函数
        /// </summary>
        public JsonData()
        {
        }
        /// <summary>
        /// 构造布尔类型的JsonData
        /// </summary>
        /// <param name="boolean"></param>
        public JsonData(bool boolean)
        {
            type = JsonType.Boolean;
            inst_boolean = boolean;
        }

        /// <summary>
        /// 构造双精度浮点型型JsonData
        /// </summary>
        /// <param name="number"></param>
        public JsonData(double number)
        {
            type = JsonType.Double;
            inst_double = number;
        }
        /// <summary>
        /// 构造整型JsonData
        /// </summary>
        /// <param name="number"></param>
        public JsonData(int number)
        {
            type = JsonType.Int;
            inst_int = number;
        }
        /// <summary>
        /// 构造长整型JsonData
        /// </summary>
        /// <param name="number"></param>
        public JsonData(long number)
        {
            type = JsonType.Long;
            inst_long = number;
        }

        /// <summary>
        /// 构造字符串型JsonData
        /// </summary>
        /// <param name="str"></param>
        public JsonData(string str)
        {
            type = JsonType.String;
            inst_string = str;
        }

        /// <summary>
        /// 构造JsonData,依据对象类型构造不同类型的JsonData
        /// </summary>
        /// <param name="obj"></param>
        public JsonData(object obj)
        {
            if (obj is Boolean)
            {
                type = JsonType.Boolean;
                inst_boolean = (bool)obj;
            }
            else if (obj is Decimal)
            {
                type = JsonType.Double;
                inst_double = Convert.ToDouble(obj);
            }
            else if (obj is Double)
            {
                type = JsonType.Double;
                inst_double = (double)obj;
            }
            else if (obj is Int16)
            {
                type = JsonType.Int;
                inst_int = (short)obj;
            }
            else if (obj is Int32)
            {
                type = JsonType.Int;
                inst_int = (int)obj;
            }

            else if (obj is Int64)
            {
                type = JsonType.Long;
                inst_long = (long)obj;
            }

            else if (obj is String)
            {
                type = JsonType.String;
                inst_string = (string)obj;
            }
            else
            {
                type = JsonType.String;
                inst_string = obj.ToString();
            }
        }
        #endregion

        #region 通过索引或名称获取或设置值
        /// <summary>
        /// 通过属性名获取值
        /// </summary>
        /// <param name="name_">属性名称</param>
        /// <returns></returns>
        public JsonData this[string name_]
        {
            get
            {
                EnsureDictionary();
                return inst_object[name_];
            }

            set
            {
                EnsureDictionary();

                KeyValuePair<string, JsonData> entry = new KeyValuePair<string, JsonData>(name_, value);

                if (inst_object.ContainsKey(name_))
                {
                    for (int i = 0; i < object_list.Count; i++)
                    {
                        if (object_list[i].Key == name_)
                        {
                            object_list[i] = entry;
                            break;
                        }
                    }
                }
                else
                {
                    object_list.Add(entry);
                }
                inst_object[name_] = value;

                json = null;
            }
        }

        /// <summary>
        /// 通过索引位置获取属性的值
        /// </summary>
        /// <param name="index_"></param>
        /// <returns></returns>
        public JsonData this[int index_]
        {
            get
            {
                EnsureCollection();

                if (type == JsonType.Array)
                    return inst_array[index_];

                return object_list[index_].Value;
            }

            set
            {
                EnsureCollection();

                if (type == JsonType.Array)
                    inst_array[index_] = value;
                else
                {
                    KeyValuePair<string, JsonData> entry = object_list[index_];
                    KeyValuePair<string, JsonData> new_entry =
                        new KeyValuePair<string, JsonData>(entry.Key, value);

                    object_list[index_] = new_entry;
                    inst_object[entry.Key] = value;
                }

                json = null;
            }
        }
        #endregion

        #region ICollection Properties
        int ICollection.Count
        {
            get
            {
                return Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return EnsureCollection().IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return EnsureCollection().SyncRoot;
            }
        }
        #endregion


        #region IDictionary Properties
        bool IDictionary.IsFixedSize
        {
            get
            {
                return EnsureDictionary().IsFixedSize;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return EnsureDictionary().IsReadOnly;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                EnsureDictionary();
                IList<string> keys = new List<string>();

                foreach (KeyValuePair<string, JsonData> entry in
                         object_list)
                {
                    keys.Add(entry.Key);
                }

                return (ICollection)keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                EnsureDictionary();
                IList<JsonData> values = new List<JsonData>();

                foreach (KeyValuePair<string, JsonData> entry in
                         object_list)
                {
                    values.Add(entry.Value);
                }

                return (ICollection)values;
            }
        }
        #endregion



        #region IJsonWrapper Properties
        bool IJsonWrapper.IsArray
        {
            get { return IsArray; }
        }

        bool IJsonWrapper.IsBoolean
        {
            get { return IsBoolean; }
        }

        bool IJsonWrapper.IsDouble
        {
            get { return IsDouble; }
        }

        bool IJsonWrapper.IsInt
        {
            get { return IsInt; }
        }

        bool IJsonWrapper.IsLong
        {
            get { return IsLong; }
        }

        bool IJsonWrapper.IsObject
        {
            get { return IsObject; }
        }

        bool IJsonWrapper.IsString
        {
            get { return IsString; }
        }
        #endregion


        #region IList Properties
        bool IList.IsFixedSize
        {
            get
            {
                return EnsureList().IsFixedSize;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return EnsureList().IsReadOnly;
            }
        }
        #endregion


        #region IDictionary Indexer
        object IDictionary.this[object key]
        {
            get
            {
                return EnsureDictionary()[key];
            }

            set
            {
                if (!(key is String))
                    throw new ArgumentException(
                        "The key has to be a string");

                JsonData data = ToJsonData(value);

                this[(string)key] = data;
            }
        }
        #endregion


        #region IOrderedDictionary Indexer
        object IOrderedDictionary.this[int idx]
        {
            get
            {
                EnsureDictionary();
                return object_list[idx].Value;
            }

            set
            {
                EnsureDictionary();
                JsonData data = ToJsonData(value);

                KeyValuePair<string, JsonData> old_entry = object_list[idx];

                inst_object[old_entry.Key] = data;

                KeyValuePair<string, JsonData> entry =
                    new KeyValuePair<string, JsonData>(old_entry.Key, data);

                object_list[idx] = entry;
            }
        }
        #endregion


        #region IList Indexer
        object IList.this[int index]
        {
            get
            {
                return EnsureList()[index];
            }

            set
            {
                EnsureList();
                JsonData data = ToJsonData(value);

                this[index] = data;
            }
        }
        #endregion


        #region 隐式转换
        /// <summary>
        /// 布尔型数据到JsonData的隐式转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData(Boolean data)
        {
            return new JsonData(data);
        }

        /// <summary>
        /// 浮点型型数据到JsonData的隐式转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData(Double data)
        {
            return new JsonData(data);
        }

        /// <summary>
        /// 整型数据到JsonData的隐式转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData(Int32 data)
        {
            return new JsonData(data);
        }

        /// <summary>
        /// 长整型数据到JsonData的隐式转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData(Int64 data)
        {
            return new JsonData(data);
        }

        /// <summary>
        /// 字符串数据到JsonData的隐式转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator JsonData(String data)
        {
            return new JsonData(data);
        }
        #endregion


        #region 显示转换
        /// <summary>
        /// JsonData到布尔型的显示转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator Boolean(JsonData data)
        {
            if (data.type != JsonType.Boolean)
            {
                throw new InvalidCastException("该JsonData不是布尔型数据,无法完成到布尔型的显示转换.");
            }
            return data.inst_boolean;
        }

        /// <summary>
        /// JsonData到浮点型的显示转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator Double(JsonData data)
        {
            if (data.type != JsonType.Double)
            {
                throw new InvalidCastException("该JsonData不是浮点型数据,无法完成到浮点型的显示转换.");
            }
            return data.inst_double;
        }

        /// <summary>
        /// JsonData到整型的显示转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator Int32(JsonData data)
        {
            if (data.type != JsonType.Int)
            {
                throw new InvalidCastException("该JsonData不是整型数据,无法完成到整型的显示转换.");
            }
            return data.inst_int;
        }

        /// <summary>
        /// JsonData到长整型的显示转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator Int64(JsonData data)
        {
            if (data.type != JsonType.Long)
            {
                throw new InvalidCastException("该JsonData不是长整型数据,无法完成到长整型的显示转换.");
            }
            return data.inst_long;
        }

        /// <summary>
        /// JsonData到字符串型的显示转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static explicit operator String(JsonData data)
        {
            if (data.type != JsonType.String)
            {
                throw new InvalidCastException("该JsonData不是字符串型数据,无法完成到字符串型的显示转换.");
            }
            return data.inst_string;
        }
        #endregion


        #region ICollection Methods
        void ICollection.CopyTo(Array array, int index)
        {
            EnsureCollection().CopyTo(array, index);
        }
        #endregion


        #region IDictionary Methods
        void IDictionary.Add(object key, object value)
        {
            JsonData data = ToJsonData(value);

            EnsureDictionary().Add(key, data);

            KeyValuePair<string, JsonData> entry =
                new KeyValuePair<string, JsonData>((string)key, data);
            object_list.Add(entry);

            json = null;
        }

        void IDictionary.Clear()
        {
            EnsureDictionary().Clear();
            object_list.Clear();
            json = null;
        }

        bool IDictionary.Contains(object key)
        {
            return EnsureDictionary().Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IOrderedDictionary)this).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            EnsureDictionary().Remove(key);

            for (int i = 0; i < object_list.Count; i++)
            {
                if (object_list[i].Key == (string)key)
                {
                    object_list.RemoveAt(i);
                    break;
                }
            }

            json = null;
        }
        #endregion


        #region IEnumerable Methods
        IEnumerator IEnumerable.GetEnumerator()
        {
            return EnsureCollection().GetEnumerator();
        }
        #endregion


        #region IJsonWrapper Methods
        bool IJsonWrapper.GetBoolean()
        {
            if (type != JsonType.Boolean)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a boolean");

            return inst_boolean;
        }

        double IJsonWrapper.GetDouble()
        {
            if (type != JsonType.Double)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a double");

            return inst_double;
        }

        int IJsonWrapper.GetInt()
        {
            if (type != JsonType.Int)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold an int");

            return inst_int;
        }

        long IJsonWrapper.GetLong()
        {
            if (type != JsonType.Long)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a long");

            return inst_long;
        }

        string IJsonWrapper.GetString()
        {
            if (type != JsonType.String)
                throw new InvalidOperationException(
                    "JsonData instance doesn't hold a string");

            return inst_string;
        }

        void IJsonWrapper.SetBoolean(bool val)
        {
            type = JsonType.Boolean;
            inst_boolean = val;
            json = null;
        }

        void IJsonWrapper.SetDouble(double val)
        {
            type = JsonType.Double;
            inst_double = val;
            json = null;
        }

        void IJsonWrapper.SetInt(int val)
        {
            type = JsonType.Int;
            inst_int = val;
            json = null;
        }

        void IJsonWrapper.SetLong(long val)
        {
            type = JsonType.Long;
            inst_long = val;
            json = null;
        }

        void IJsonWrapper.SetString(string val)
        {
            type = JsonType.String;
            inst_string = val;
            json = null;
        }

        string IJsonWrapper.ToJson()
        {
            return ToJson();
        }

        void IJsonWrapper.ToJson(JsonWriter writer)
        {
            ToJson(writer);
        }
        #endregion


        #region IList Methods
        int IList.Add(object value)
        {
            return Add(value);
        }

        void IList.Clear()
        {
            EnsureList().Clear();
            json = null;
        }

        bool IList.Contains(object value)
        {
            return EnsureList().Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return EnsureList().IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            EnsureList().Insert(index, value);
            json = null;
        }

        void IList.Remove(object value)
        {
            EnsureList().Remove(value);
            json = null;
        }

        void IList.RemoveAt(int index)
        {
            EnsureList().RemoveAt(index);
            json = null;
        }
        #endregion


        #region IOrderedDictionary Methods
        IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
        {
            EnsureDictionary();

            return new OrderedDictionaryEnumerator(
                object_list.GetEnumerator());
        }

        void IOrderedDictionary.Insert(int idx, object key, object value)
        {
            string property = (string)key;
            JsonData data = ToJsonData(value);

            this[property] = data;

            KeyValuePair<string, JsonData> entry =
                new KeyValuePair<string, JsonData>(property, data);

            object_list.Insert(idx, entry);
        }

        void IOrderedDictionary.RemoveAt(int idx)
        {
            EnsureDictionary();

            inst_object.Remove(object_list[idx].Key);
            object_list.RemoveAt(idx);
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 确保JsonData为非泛型集合类对象
        /// </summary>
        /// <returns></returns>
        private ICollection EnsureCollection()
        {
            if (type == JsonType.Array)
            {
                return (ICollection)inst_array;
            }
            if (type == JsonType.Object)
            {
                return (ICollection)inst_object;
            }
            throw new InvalidOperationException("JsonData未被初始化");
        }

        /// <summary>
        /// 确保JsonData为键/值对的非通用集合
        /// </summary>
        /// <returns></returns>
        private IDictionary EnsureDictionary()
        {
            if (type == JsonType.Object)
            {
                return (IDictionary)inst_object;
            }
            if (type == JsonType.None)
            {
                type = JsonType.Object;
                inst_object = new Dictionary<string, JsonData>();
                object_list = new List<KeyValuePair<string, JsonData>>();
                return (IDictionary)inst_object;
            }
            else
            {
                throw new InvalidOperationException("该JsonData实例不是一个Dictionary实例");
            }
        }

        /// <summary>
        /// 确保JsonData为可按照索引单独访问的对象的非泛型集合
        /// </summary>
        /// <returns></returns>
        private IList EnsureList()
        {
            if (type == JsonType.Array)
            {
                return (IList)inst_array;
            }
            if (type == JsonType.None)
            {
                type = JsonType.Array;
                inst_array = new List<JsonData>();
                return (IList)inst_array;
            }
            else
            {
                throw new InvalidOperationException("该JsonData实例不是一个List实例");
            }
        }

        /// <summary>
        /// 转化为JsonData
        /// </summary>
        /// <param name="obj">值</param>
        /// <returns></returns>
        private JsonData ToJsonData(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is JsonData)
            {
                return (JsonData)obj;
            }
            else
            {
                return new JsonData(obj);
            }
        }

        private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
        {
            if (obj.IsString)
            {
                writer.Write(obj.GetString());
                return;
            }

            if (obj.IsBoolean)
            {
                writer.Write(obj.GetBoolean());
                return;
            }

            if (obj.IsDouble)
            {
                writer.Write(obj.GetDouble());
                return;
            }

            if (obj.IsInt)
            {
                writer.Write(obj.GetInt());
                return;
            }

            if (obj.IsLong)
            {
                writer.Write(obj.GetLong());
                return;
            }

            if (obj.IsArray)
            {
                writer.WriteArrayStart();
                foreach (object elem in (IList)obj)
                    WriteJson((JsonData)elem, writer);
                writer.WriteArrayEnd();

                return;
            }

            if (obj.IsObject)
            {
                writer.WriteObjectStart();

                foreach (DictionaryEntry entry in ((IDictionary)obj))
                {
                    writer.WritePropertyName((string)entry.Key);
                    WriteJson((JsonData)entry.Value, writer);
                }
                writer.WriteObjectEnd();

                return;
            }
        }
        #endregion


        public int Add(object value)
        {
            JsonData data = ToJsonData(value);

            json = null;

            return EnsureList().Add(data);
        }

        public void Clear()
        {
            if (IsObject)
            {
                ((IDictionary)this).Clear();
                return;
            }

            if (IsArray)
            {
                ((IList)this).Clear();
                return;
            }
        }

        public bool Equals(JsonData x)
        {
            if (x == null)
                return false;

            if (x.type != this.type)
                return false;

            switch (this.type)
            {
                case JsonType.None:
                    return true;

                case JsonType.Object:
                    return this.inst_object.Equals(x.inst_object);

                case JsonType.Array:
                    return this.inst_array.Equals(x.inst_array);

                case JsonType.String:
                    return this.inst_string.Equals(x.inst_string);

                case JsonType.Int:
                    return this.inst_int.Equals(x.inst_int);

                case JsonType.Long:
                    return this.inst_long.Equals(x.inst_long);

                case JsonType.Double:
                    return this.inst_double.Equals(x.inst_double);

                case JsonType.Boolean:
                    return this.inst_boolean.Equals(x.inst_boolean);
            }

            return false;
        }

        /// <summary>
        /// 获取该JsonData实例的类型
        /// </summary>
        /// <returns></returns>
        public JsonType GetJsonType()
        {
            return type;
        }

        /// <summary>
        /// 设置JsonData实例的类型
        /// </summary>
        /// <param name="type"></param>
        public void SetJsonType(JsonType type)
        {
            if (this.type == type)
                return;

            switch (type)
            {
                case JsonType.None:
                    break;

                case JsonType.Object:
                    inst_object = new Dictionary<string, JsonData>();
                    object_list = new List<KeyValuePair<string, JsonData>>();
                    break;

                case JsonType.Array:
                    inst_array = new List<JsonData>();
                    break;

                case JsonType.String:
                    inst_string = default(String);
                    break;

                case JsonType.Int:
                    inst_int = default(Int32);
                    break;

                case JsonType.Long:
                    inst_long = default(Int64);
                    break;

                case JsonType.Double:
                    inst_double = default(Double);
                    break;

                case JsonType.Boolean:
                    inst_boolean = default(Boolean);
                    break;
            }

            this.type = type;
        }

        /// <summary>
        /// 将当前JSON对象转化为JSON字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            if (json != null)
                return json;

            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonWriter(sw);
            writer.Validate = false;

            WriteJson(this, writer);
            json = sw.ToString();

            return json;
        }

        public void ToJson(JsonWriter writer)
        {
            bool old_validate = writer.Validate;

            writer.Validate = false;

            WriteJson(this, writer);

            writer.Validate = old_validate;
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, ref JsonData value)
        {
            value = null;
            try
            {
                value = this[key];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            switch (type)
            {
                case JsonType.Array:
                    return "JsonData Array";

                case JsonType.Boolean:
                    return inst_boolean.ToString();

                case JsonType.Double:
                    return inst_double.ToString();

                case JsonType.Int:
                    return inst_int.ToString();

                case JsonType.Long:
                    return inst_long.ToString();

                case JsonType.Object:
                    return "JsonData Object";

                case JsonType.String:
                    return inst_string;
            }

            return "JsonData未初始化";
        }
    }


    internal class OrderedDictionaryEnumerator : IDictionaryEnumerator
    {
        IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;


        public object Current
        {
            get { return Entry; }
        }

        public DictionaryEntry Entry
        {
            get
            {
                KeyValuePair<string, JsonData> curr = list_enumerator.Current;
                return new DictionaryEntry(curr.Key, curr.Value);
            }
        }

        public object Key
        {
            get { return list_enumerator.Current.Key; }
        }

        public object Value
        {
            get { return list_enumerator.Current.Value; }
        }


        public OrderedDictionaryEnumerator(
            IEnumerator<KeyValuePair<string, JsonData>> enumerator)
        {
            list_enumerator = enumerator;
        }

        public bool MoveNext()
        {
            return list_enumerator.MoveNext();
        }

        public void Reset()
        {
            list_enumerator.Reset();
        }
    }
}
