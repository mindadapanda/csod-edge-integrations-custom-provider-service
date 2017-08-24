using System;
using System.Collections.Generic;
using System.Text;

namespace AonHewitt.Lib
{
    public interface IAonLogger
    {
        void Trace(string format, params object[] args);
        void Error(Exception ex, string format, params object[] args);
    }
}
