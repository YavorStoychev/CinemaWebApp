using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.GCommon.Exceptions
{
    public class EntityKeyNullOrEmptyException : Exception
    {
        public EntityKeyNullOrEmptyException()
        {

        }

        public EntityKeyNullOrEmptyException(string message)
            : base(message)
        {

        }
    }
}
