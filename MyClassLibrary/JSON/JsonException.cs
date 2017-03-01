using System;

namespace MyClassLibrary.JSON
{
    /// <summary>
    /// Json异常
    /// </summary>
    public class JsonException : ApplicationException
    {
        /// <summary>
        /// 空构造函数
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
        /// 构造函数
        /// </summary>
        /// <param name="message">消息</param>
        public JsonException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="inner_exception">内部异常</param>
        public JsonException(string message, Exception inner_exception) :
            base(message, inner_exception)
        {
        }
    }
}
