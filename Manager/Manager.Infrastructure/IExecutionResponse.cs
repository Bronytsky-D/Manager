using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Infrastructure
{
    public interface IExecutionResponse
    {
        bool Success { get; }
        IEnumerable<string> Errors { get; }
        object Result { get; }
    }
}
