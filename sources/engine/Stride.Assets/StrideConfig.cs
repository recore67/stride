// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stride.Core.Assets;
using Stride.Core;
using Stride.Core.VisualStudio;
using System.Runtime.InteropServices;

namespace Stride.Assets
{
    [DataContract("Stride")]
    public sealed class StrideConfig
    {
        public const string PackageName = "Stride";

        public static readonly PackageVersion LatestPackageVersion = new PackageVersion(StrideVersion.NuGetVersion);

        private static readonly string ProgramFilesX86 = Environment.GetEnvironmentVariable(Environment.Is64BitOperatingSystem ? "ProgramFiles(x86)" : "ProgramFiles");

        private static readonly Version VS2015Version = new Version(14, 0);
        private static readonly Version VSAnyVersion = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

        public static PackageDependency GetLatestPackageDependency()
        {
            return new PackageDependency(PackageName, new PackageVersionRange()
                {
                    MinVersion = LatestPackageVersion,
                    IsMinInclusive = true
                });
        }

        /// <summary>
        /// Registers the solution platforms supported by Stride.
        /// </summary>
        internal static void RegisterSolutionPlatforms()
        {
            var solutionPlatforms = new List<SolutionPlatform>();

            // Windows
            var windowsPlatform = new SolutionPlatform()
            {
                Name = PlatformType.Windows.ToString(),
                IsAvailable = true,
                TargetFramework = "net8.0-windows",
                RuntimeIdentifier = "win-x64",
                Type = PlatformType.Windows
            };
            solutionPlatforms.Add(windowsPlatform);

            // Linux
            var linuxPlatform = new SolutionPlatform()
            {
                Name = PlatformType.Linux.ToString(),
                IsAvailable = true,
                TargetFramework = "net8.0",
                RuntimeIdentifier = "linux-x64",
                Type = PlatformType.Linux,
            };
            solutionPlatforms.Add(linuxPlatform);

            AssetRegistry.RegisterSupportedPlatforms(solutionPlatforms);
        }

        /// <summary>
        /// Checks if any of the provided component versions are available on this system
        /// </summary>
        /// <param name="vsVersionToComponent">A dictionary of Visual Studio versions to their respective paths for a given component</param>
        /// <returns>true if any of the components in the dictionary are available, false otherwise</returns>
        internal static bool IsVSComponentAvailableAnyVersion(IDictionary<Version, string> vsVersionToComponent)
        {
            if (vsVersionToComponent == null) { throw new ArgumentNullException("vsVersionToComponent"); }

            foreach (var pair in vsVersionToComponent)
            {
                if (pair.Key == VS2015Version)
                {
                    return IsFileInProgramFilesx86Exist(pair.Value);
                }
                else
                {
                    return VisualStudioVersions.AvailableVisualStudioInstances.Any(
                        ideInfo => ideInfo.PackageVersions.ContainsKey(pair.Value)
                    );
                }
            }
            return false;
        }

        // For VS 2015
        internal static bool IsFileInProgramFilesx86Exist(string path)
        {
            return (ProgramFilesX86 != null && File.Exists(Path.Combine(ProgramFilesX86, path)));
        }
    }
}
