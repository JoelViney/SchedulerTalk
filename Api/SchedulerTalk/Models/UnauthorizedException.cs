using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerTalk.Models
{
    /// <summary>
    /// This exception is thrown when the user attempts to perform an action that they don't have permission to do.
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {

        }
    }
}
