﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuKeeper.NuGet.Api;

namespace NuKeeper.Engine
{
    public static class CommitReport
    { 
        public static string MakeCommitMessage(List<PackageUpdate> updates)
        {
            return $"Automatic update of {updates[0].PackageId} to {updates[0].NewVersion}";
        }

        public static string MakeCommitDetails(List<PackageUpdate> updates)
        {
            var oldVersions = updates
                .Select(u => CodeQuote(u.OldVersion.ToString()))
                .Distinct()
                .ToList();

            var oldVersionsString = string.Join(",", oldVersions);
            var newVersion = CodeQuote(updates[0].NewVersion.ToString());
            var packageId = CodeQuote(updates[0].PackageId);

            var builder = new StringBuilder();


            if (oldVersions.Count == 1)
            {
                builder.AppendLine($"NuKeeper has generated an update of {packageId} to {newVersion} from {oldVersionsString}");
            }
            else
            {
                builder.AppendLine($"NuKeeper has generated an update of {packageId} to {newVersion}");
                builder.AppendLine($"{oldVersions.Count} versions of {packageId} were found in use: {oldVersionsString}");
            }

            if (updates.Count == 1)
            {
                builder.AppendLine("1 project update:");
            }
            else
            {
                builder.AppendLine($"{updates.Count} project updates:");
            }

            foreach (var update in updates)
            {
                var line = $"Updated {CodeQuote(update.CurrentPackage.Path.RelativePath)} to {packageId} {CodeQuote(update.NewVersion.ToString())} from {CodeQuote(update.OldVersion.ToString())}";

                builder.AppendLine(line);
            }

            builder.AppendLine("This is an automated update. Merge only if it passes tests");
            builder.AppendLine("");
            builder.AppendLine("**NuKeeper**: https://github.com/NuKeeperDotNet/NuKeeper");
            return builder.ToString();
        }

        private static string CodeQuote(string value)
        {
            return "`" + value + "`";
        }
    }
}