using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace DemoPick.Services
{
    internal static class SmokePerformancePersistence
    {
        internal static SmokePerfBaseline LoadOrCreatePerfBaseline(string root, string machine, double priceOps, double pendingOps, bool persistIfMissing)
        {
            string docsDir = Path.Combine(root, "Docs");
            Directory.CreateDirectory(docsDir);

            string path = Path.Combine(docsDir, "PERF_BASELINES.csv");

            var map = new Dictionary<string, SmokePerfBaseline>(StringComparer.OrdinalIgnoreCase);
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (line.Length == 0) continue;
                    var parts = line.Split(',');
                    if (parts.Length < 4) continue;

                    if (!double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var p1)) continue;
                    if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var p2)) continue;
                    DateTime.TryParse(parts[3], CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var updated);

                    map[parts[0]] = new SmokePerfBaseline
                    {
                        Machine = parts[0],
                        PriceCalcMinOps = p1,
                        PendingOrdersMinOps = p2,
                        UpdatedAt = updated
                    };
                }
            }

            if (!map.TryGetValue(machine, out var baseline))
            {
                baseline = new SmokePerfBaseline
                {
                    Machine = machine,
                    PriceCalcMinOps = Math.Max(1d, priceOps * 0.80d),
                    PendingOrdersMinOps = Math.Max(1d, pendingOps * 0.80d),
                    UpdatedAt = DateTime.Now
                };
                map[machine] = baseline;
                if (persistIfMissing)
                {
                    SavePerfBaselines(path, map);
                }
            }

            return baseline;
        }

        internal static string WritePerformanceModuleReport(string root, List<SmokeModulePerfResult> results, decimal checksum)
        {
            string perfDir = Path.Combine(root, "Docs", "Perf");
            Directory.CreateDirectory(perfDir);

            string path = Path.Combine(perfDir, "PERF_LAST_RUN.md");
            var sb = new StringBuilder();
            sb.AppendLine("# DemoPick Performance Report");
            sb.AppendLine();
            sb.AppendLine($"- Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"- Machine: {Environment.MachineName}");
            sb.AppendLine($"- Checksum: {checksum:N0}");
            sb.AppendLine();
            sb.AppendLine("| Module | Iterations | Elapsed (ms) | Ops/s | Baseline Min Ops/s | Result | Mode |");
            sb.AppendLine("|---|---:|---:|---:|---:|---|---|");

            foreach (var r in results)
            {
                sb.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "| {0} | {1} | {2} | {3:F0} | {4:F0} | {5} | {6} |",
                    r.Module,
                    r.Iterations,
                    r.ElapsedMs,
                    r.OpsPerSec,
                    r.ThresholdOpsPerSec,
                    r.Passed ? "PASS" : "FAIL",
                    r.Mode
                ));
            }

            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(false));
            return path;
        }

        internal static void AppendPerformanceHistory(string root, List<SmokeModulePerfResult> results)
        {
            string perfDir = Path.Combine(root, "Docs", "Perf");
            Directory.CreateDirectory(perfDir);

            string history = Path.Combine(perfDir, "PERF_MODULES_HISTORY.csv");
            bool exists = File.Exists(history);

            var sb = new StringBuilder();
            if (!exists)
            {
                sb.AppendLine("Timestamp,Machine,Module,Iterations,ElapsedMs,OpsPerSec,BaselineMinOpsPerSec,Passed,Mode");
            }

            string ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            foreach (var r in results)
            {
                sb.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0},{1},{2},{3},{4},{5:F2},{6:F2},{7},{8}",
                    ts,
                    Environment.MachineName,
                    r.Module,
                    r.Iterations,
                    r.ElapsedMs,
                    r.OpsPerSec,
                    r.ThresholdOpsPerSec,
                    r.Passed ? "1" : "0",
                    r.Mode
                ));
            }

            File.AppendAllText(history, sb.ToString(), new UTF8Encoding(false));
        }

        private static void SavePerfBaselines(string path, Dictionary<string, SmokePerfBaseline> map)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Machine,PriceCalcMinOps,PendingOrdersMinOps,UpdatedAt");
            foreach (var kv in map)
            {
                var b = kv.Value;
                sb.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0},{1:F2},{2:F2},{3:yyyy-MM-dd HH:mm:ss}",
                    b.Machine,
                    b.PriceCalcMinOps,
                    b.PendingOrdersMinOps,
                    b.UpdatedAt
                ));
            }
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(false));
        }
    }
}
