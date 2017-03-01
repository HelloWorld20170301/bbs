using System.Collections;
using System.Collections.Specialized;

namespace MyClassLibrary.JSON
{
    /// <summary>
    /// Json����İ�װ�ӿ�,�������ͼ��Ϻͼ�ֵ�Խӿ��Լ��¶�����ж�Json������,��Բ�ͬ��Json���ͻ�ȡ������Json����
    /// </summary>
    public interface IJsonWrapper : IList, IOrderedDictionary
    {
        /// <summary>
        /// ��ȡJsonData�Ƿ�������
        /// </summary>
        bool IsArray
        {
            get;
        }
        /// <summary>
        /// ��ȡJsonData�Ƿ��ǲ�����
        /// </summary>
        bool IsBoolean
        {
            get;
        }
        /// <summary>
        /// ��ȡJsonData�Ƿ��Ǹ�����
        /// </summary>
        bool IsDouble
        {
            get;
        }
        /// <summary>
        /// ��ȡJsonData�Ƿ�������
        /// </summary>
        bool IsInt
        {
            get;
        }
        /// <summary>
        /// ��ȡJsonData�Ƿ��ǳ�����
        /// </summary>
        bool IsLong
        {
            get;
        }
        /// <summary>
        /// ��ȡJsonData�Ƿ��Ƕ���
        /// </summary>
        bool IsObject
        {
            get;
        }
        /// <summary>
        /// ��ȡJsonData�Ƿ����ַ���
        /// </summary>
        bool IsString
        {
            get;
        }
        /// <summary>
        /// ��ò�����ֵ
        /// </summary>
        /// <returns></returns>
        bool GetBoolean();
        /// <summary>
        /// ��ø�����ֵ
        /// </summary>
        /// <returns></returns>
        double GetDouble();
        /// <summary>
        /// �������ֵ
        /// </summary>
        /// <returns></returns>
        int GetInt();
        /// <summary>
        /// ��ó�����ֵ
        /// </summary>
        /// <returns></returns>
        long GetLong();
        /// <summary>
        /// ����ַ���ֵ
        /// </summary>
        /// <returns></returns>
        string GetString();
        /// <summary>
        /// ���ò�����ֵ
        /// </summary>
        /// <param name="val">������ֵ</param>
        void SetBoolean(bool val);
        /// <summary>
        /// ���ø�����ֵ
        /// </summary>
        /// <param name="val">������ֵ</param>
        void SetDouble(double val);
        /// <summary>
        /// ��������ֵ
        /// </summary>
        /// <param name="val">����ֵ</param>
        void SetInt(int val);
        /// <summary>
        /// ���ó�����ֵ
        /// </summary>
        /// <param name="val">������ֵ</param>
        void SetLong(long val);
        /// <summary>
        /// �����ַ���ֵ
        /// </summary>
        /// <param name="val">�ַ���ֵ</param>
        void SetString(string val);

        /// <summary>
        /// ��ȡJsonData������
        /// </summary>
        /// <returns></returns>
        JsonType GetJsonType();
        /// <summary>
        /// ����JsonData������
        /// </summary>
        /// <param name="type">����</param>
        void SetJsonType(JsonType type);

        /// <summary>
        /// ��ȡJsonData��json�ַ���
        /// </summary>
        /// <returns></returns>
        string ToJson();

        /// <summary>
        /// ��JsonData��json�ַ���д��JsonWriter������
        /// </summary>
        /// <param name="writer"></param>
        void ToJson(JsonWriter writer);
    }
}
