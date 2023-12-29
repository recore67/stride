// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using Stride.Core.Annotations;

namespace Stride.Core
{
    /// <summary>
    /// Folders used for the running platform.
    /// </summary>
    public class PlatformFolders
    {
        // TODO: This class should not try to initialize directories...etc. Try to find another way to do this

        /// <summary>
        /// The system temporary directory.
        /// </summary>
        public static readonly string TemporaryDirectory = GetTemporaryDirectory();

        /// <summary>
        /// The Application temporary directory.
        /// </summary>
        public static readonly string ApplicationTemporaryDirectory = GetApplicationTemporaryDirectory();

        /// <summary>
        /// The application local directory, where user can write local data (included in backup).
        /// </summary>
        public static readonly string ApplicationLocalDirectory = GetApplicationLocalDirectory();

        /// <summary>
        /// The application roaming directory, where user can write roaming data (included in backup).
        /// </summary>
        public static readonly string ApplicationRoamingDirectory = GetApplicationRoamingDirectory();

        /// <summary>
        /// The application cache directory, where user can write data that won't be backup.
        /// </summary>
        public static readonly string ApplicationCacheDirectory = GetApplicationCacheDirectory();

        /// <summary>
        /// The application data directory, where data is deployed.
        /// It could be read-only on some platforms.
        /// </summary>
        public static readonly string ApplicationDataDirectory = GetApplicationDataDirectory();

        /// <summary>
        /// The (optional) application data subdirectory. If not null or empty, /data will be mounted on <see cref="ApplicationDataDirectory"/>/<see cref="ApplicationDataSubDirectory"/>
        /// </summary>
        /// <remarks>This property should not be written after the VirtualFileSystem static initialization. If so, an InvalidOperationExeception will be thrown.</remarks>
        public static string ApplicationDataSubDirectory
        {
            get { return applicationDataSubDirectory; }

            set
            {
                if (virtualFileSystemInitialized)
                    throw new InvalidOperationException("ApplicationDataSubDirectory cannot be modified after the VirtualFileSystem has been initialized.");

                applicationDataSubDirectory = value;
            }
        }

        /// <summary>
        /// The application directory, where assemblies are deployed.
        /// It could be read-only on some platforms.
        /// </summary>
        public static readonly string ApplicationBinaryDirectory = GetApplicationBinaryDirectory();

        /// <summary>
        /// Get the path to the application executable.
        /// </summary>
        /// <remarks>Might be null if start executable is unknown.</remarks>
        public static readonly string ApplicationExecutablePath = GetApplicationExecutablePath();

        private static string applicationDataSubDirectory = string.Empty;

        private static bool virtualFileSystemInitialized;

        public static bool IsVirtualFileSystemInitialized
        {
            get
            {
                return virtualFileSystemInitialized;
            }
            internal set
            {
                virtualFileSystemInitialized = value;
            }
        }

        [NotNull]
        private static string GetApplicationLocalDirectory()
        {
            // TODO: Should we add "local" ?
            var directory = Path.Combine(GetApplicationBinaryDirectory(), "local");
            Directory.CreateDirectory(directory);
            return directory;
        }

        [NotNull]
        private static string GetApplicationRoamingDirectory()
        {
            // TODO: Should we add "local" ?
            var directory = Path.Combine(GetApplicationBinaryDirectory(), "roaming");
            Directory.CreateDirectory(directory);
            return directory;
        }

        [NotNull]
        private static string GetApplicationCacheDirectory()
        {
            // TODO: Should we add "local" ?
            var directory = Path.Combine(GetApplicationBinaryDirectory(), "cache");
            Directory.CreateDirectory(directory);
            return directory;
        }

        private static string GetApplicationExecutablePath()
        {
            return null;
        }

        [NotNull]
        private static string GetTemporaryDirectory()
        {
            return GetApplicationTemporaryDirectory();
        }

        [NotNull]
        private static string GetApplicationTemporaryDirectory()
        {
            return Path.GetTempPath();
        }

        [NotNull]
        private static string GetApplicationBinaryDirectory()
        {
            return Path.GetDirectoryName(typeof(PlatformFolders).Assembly.Location);
        }

        private static string GetApplicationExecutableDirectory()
        {
#if STRIDE_PLATFORM_DESKTOP || STRIDE_PLATFORM_MONO_MOBILE
            var executableName = GetApplicationExecutablePath();
            if (!string.IsNullOrEmpty(executableName))
            {
                return Path.GetDirectoryName(executableName);
            }
    #if STRIDE_RUNTIME_CORECLR
            return AppContext.BaseDirectory;
    #else
            return AppDomain.CurrentDomain.BaseDirectory;
    #endif
#elif STRIDE_PLATFORM_UWP
            return Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
#else
            throw new NotImplementedException();
#endif
        }

        [NotNull]
        private static string GetApplicationDataDirectory()
        {
            return Path.Combine(GetApplicationBinaryDirectory(), "data");
        }
    }
}
