using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Providers
{
    public interface ILoggerManager
    {
        void LogInformation(string message);

        void LogError(string message);
    }
}
