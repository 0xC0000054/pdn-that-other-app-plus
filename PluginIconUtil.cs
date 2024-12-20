////////////////////////////////////////////////////////////////////////
//
// This file is part of pdn-that-other-app-plus, a Effect plugin for
// Paint.NET that exports the current layer to other image editors.
//
// Copyright (c) 2020, 2024 Nicholas Hayes
//
// This file is licensed under the MIT License.
// See LICENSE.txt for complete licensing and attribution information.
//
////////////////////////////////////////////////////////////////////////

using System;

namespace ThatOtherAppPlus
{
    internal static class PluginIconUtil
    {
        private static readonly Tuple<int, string>[] availableIcons = new Tuple<int, string>[]
        {
            new Tuple<int, string>(96, "Resources.Icons.icon-96.png"),
            new Tuple<int, string>(120, "Resources.Icons.icon-120.png"),
            new Tuple<int, string>(144, "Resources.Icons.icon-144.png"),
            new Tuple<int, string>(192, "Resources.Icons.icon-192.png"),
            new Tuple<int, string>(384, "Resources.Icons.icon-384.png"),
        };

        internal static string GetIconResourceNameForDpi(int dpi)
        {
            for (int i = 0; i < availableIcons.Length; i++)
            {
                Tuple<int, string> icon = availableIcons[i];
                if (icon.Item1 >= dpi)
                {
                    return icon.Item2;
                }
            }

            return "Resources.Icons.icon-384.png";
        }
    }
}
