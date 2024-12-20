////////////////////////////////////////////////////////////////////////
//
// This file is part of pdn-that-other-app-plus, a Effect plugin for
// Paint.NET that exports the current layer to other image editors.
//
// Copyright (c) 2020 Nicholas Hayes
//
// This file is licensed under the MIT License.
// See LICENSE.txt for complete licensing and attribution information.
//
////////////////////////////////////////////////////////////////////////

using PaintDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ThatOtherAppPlus
{
    [DataContract]
    internal sealed class ApplicationInfo : IEquatable<ApplicationInfo>
    {
        public ApplicationInfo(string path)
        {
            ArgumentNullException.ThrowIfNull(path, nameof(path));

            this.DisplayName = GetDisplayName(path);
            this.Path = path;
        }

        [DataMember]
        public string DisplayName { get; private set; }

        [DataMember]
        public string Path { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is ApplicationInfo other && Equals(other);
        }

        public bool Equals(ApplicationInfo other)
        {
            if (other is null)
            {
                return false;
            }

            return this.DisplayName.Equals(other.DisplayName, StringComparison.Ordinal) &&
                   this.Path.Equals(other.Path, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            int hashCode = 1517007740;

            unchecked
            {
                hashCode = (hashCode * -1521134295) + StringComparer.Ordinal.GetHashCode(this.DisplayName);
                hashCode = (hashCode * -1521134295) + StringComparer.OrdinalIgnoreCase.GetHashCode(this.Path);
            }

            return hashCode;
        }

        private static string GetDisplayName(string path)
        {
            string displayName;

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(path);

            if (!string.IsNullOrEmpty(versionInfo.FileDescription))
            {
                displayName = versionInfo.FileDescription;
            }
            else
            {
                displayName = System.IO.Path.GetFileNameWithoutExtension(path);
            }

            return displayName;
        }

        public static bool operator ==(ApplicationInfo left, ApplicationInfo right)
        {
            return EqualityComparer<ApplicationInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(ApplicationInfo left, ApplicationInfo right)
        {
            return !(left == right);
        }
    }
}
