// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
#pragma warning disable SA1300 // Element should begin with upper-case letter
using System;
using Stride.Core;

namespace Stride.Data
{
    [Flags]
    public enum ConfigPlatforms
    {
        None = 0,
        Windows = 1 << PlatformType.Windows,
        Linux = 1 << PlatformType.Linux,
    }
}
