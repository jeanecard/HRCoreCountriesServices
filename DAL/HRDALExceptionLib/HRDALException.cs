using System;

namespace HRDALExceptionLib
{
    public class HRDALException : Exception
    {
        private String _code = String.Empty;
        public HRDALException(String message) : base(message)
        {

        }
        public static void ThrowException(String message, String code)
        {
            HRDALException exception = new HRDALException(message);
            exception.Code = code;
            throw exception;
        }

        private HRDALException()
        {
        }
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
