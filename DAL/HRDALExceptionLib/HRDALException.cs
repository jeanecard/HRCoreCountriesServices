using System;

namespace HRDALExceptionLib
{
    public class HRDALException : Exception
    {
        private String _code = String.Empty;
        public HRDALException(String message) : base(message)
        {

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
