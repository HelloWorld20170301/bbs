using System;

namespace MyClassLibrary.JSON
{
    /// <summary>
    /// Json�쳣
    /// </summary>
    public class JsonException : ApplicationException
    {
        /// <summary>
        /// �չ��캯��
        /// </summary>
        public JsonException()
            : base()
        {
        }

        internal JsonException(ParserToken token) :
            base(String.Format("Invalid token '{0}' in input string", token))
        {
        }

        internal JsonException(ParserToken token, Exception inner_exception) :
            base(String.Format("Invalid token '{0}' in input string", token), inner_exception)
        {
        }

        internal JsonException(int c) :
            base(String.Format("Invalid character '{0}' in input string", (char)c))
        {
        }

        internal JsonException(int c, Exception inner_exception) :
            base(String.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public JsonException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="message">��Ϣ</param>
        /// <param name="inner_exception">�ڲ��쳣</param>
        public JsonException(string message, Exception inner_exception) :
            base(message, inner_exception)
        {
        }
    }
}
