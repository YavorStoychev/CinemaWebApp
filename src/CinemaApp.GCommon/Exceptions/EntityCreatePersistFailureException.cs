using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.GCommon.Exceptions
{
    public class EntityCreatePersistFailureException : Exception
    {
        public EntityCreatePersistFailureException()
        {

        }

        public EntityCreatePersistFailureException(string message)
            : base(message)
        {

        }
    }
}
