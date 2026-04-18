using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DemoPick.Services
{
    internal static class SmokeReportWriter
    {
        internal static string WriteMarkdownReport(string root, DateTime startedAt, TimeSpan total, List<SmokeTestStepResult> steps, string identifier, string credentialSource)
        {
            string docsDir = Path.Combine(root, "Docs");
            Directory.CreateDirectory(docsDir);

            string path = Path.Combine(docsDir, "SMOKE_RUN.md");

            var sb = new StringBuilder();
            sb.AppendLine("# DemoPick Smoke Test Report");
            sb.AppendLine();
            sb.AppendLine("- Started: " + startedAt.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine("- Duration: " + total.TotalSeconds.ToString("F2") + "s");
            sb.AppendLine("- Machine: " + Environment.MachineName);
            sb.AppendLine("- User: " + Environment.UserName);
            sb.AppendLine("- App: " + AppDomain.CurrentDomain.FriendlyName);
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(identifier))
            {
                sb.AppendLine("## Login Used");
                sb.AppendLine();
                sb.AppendLine("- Identifier: " + identifier);
                if (!string.IsNullOrWhiteSpace(credentialSource)) sb.AppendLine("- Source: " + credentialSource);
                sb.AppendLine();
            }

            sb.AppendLine("## Steps");
            sb.AppendLine();
            sb.AppendLine("| Step | Result | Duration | Details |");
            sb.AppendLine("|---|---|---:|---|");

            foreach (var s in steps)
            {
                string res = s.Success ? "SUCCESS" : "FAIL";
                string dur = s.Duration.TotalMilliseconds.ToString("0") + "ms";
                string details = (s.Details ?? string.Empty).Replace("\r", " ").Replace("\n", " ");
                if (details.Length > 200) details = details.Substring(0, 200) + "…";
                sb.AppendLine("| " + EscapePipe(s.Name) + " | " + res + " | " + dur + " | " + EscapePipe(details) + " |");
            }

            sb.AppendLine();
            sb.AppendLine("## Failures");
            sb.AppendLine();
            bool anyFail = false;
            foreach (var s in steps)
            {
                if (s.Success) continue;
                anyFail = true;
                sb.AppendLine("### " + s.Name);
                sb.AppendLine();
                sb.AppendLine("```text");
                sb.AppendLine((s.Exception ?? new Exception(s.Details ?? "Unknown error")).ToString());
                sb.AppendLine("```");
                sb.AppendLine();
            }
            if (!anyFail)
            {
                sb.AppendLine("No failures.");
                sb.AppendLine();
            }

            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            return path;
        }

        private static string EscapePipe(string s)
        {
            return (s ?? string.Empty).Replace("|", "\\|");
        }
    }
}
