using System;
using System.Diagnostics;

namespace DemoPick.Services
{
    internal static class SmokeStepRunner
    {
        internal static SmokeTestStepResult RunStep(string name, Func<string> action)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                string details = action?.Invoke();
                sw.Stop();
                return new SmokeTestStepResult { Name = name, Success = true, Duration = sw.Elapsed, Details = details };
            }
            catch (Exception ex)
            {
                sw.Stop();
                return new SmokeTestStepResult { Name = name, Success = false, Duration = sw.Elapsed, Details = ex.Message, Exception = ex };
            }
        }
    }
}
