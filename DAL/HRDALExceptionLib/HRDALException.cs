using System;

namespace HRDALExceptionLib
{
    /// <summary>
    /// Class to manage specific Error.
    /// </summary>
    public class HRDALException : Exception
    {
        private String _code = String.Empty;
        public HRDALException(String message) : base(message)
        {

        }
        /// <summary>
        /// Static factory.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public static void ThrowException(String message, String code)
        {
            HRDALException exception = new HRDALException(message);
            exception.Code = code;
            throw exception;
        }

        private HRDALException()
        {
        }
        /// <summary>
        /// Internal HR Code
        /// </summary>
        public String Code
        {
            get { return _code; }
            set
            {
                _code = value;
            }
        }

    }
}
