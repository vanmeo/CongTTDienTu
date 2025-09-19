using System;
using System.Collections.Generic;
using System.Text;

namespace Xim.Library.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException()
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public override string ToString()
        {
            return this.Message;
        }
    }
}
