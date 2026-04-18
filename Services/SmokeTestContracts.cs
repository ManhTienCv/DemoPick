using System;

namespace DemoPick.Services
{
    internal sealed class SmokeTestStepResult
    {
        public string Name;
        public bool Success;
        public TimeSpan Duration;
        public string Details;
        public Exception Exception;
    }

    internal sealed class SmokePerfBaseline
    {
        public string Machine;
        public double PriceCalcMinOps;
        public double PendingOrdersMinOps;
        public DateTime UpdatedAt;
    }

    internal sealed class SmokeModulePerfResult
    {
        public string Module;
        public int Iterations;
        public long ElapsedMs;
        public double OpsPerSec;
        public double ThresholdOpsPerSec;
        public bool Passed;
        public string Mode;
    }
}
