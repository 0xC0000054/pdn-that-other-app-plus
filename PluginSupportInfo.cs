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
using System.Reflection;

namespace ThatOtherAppPlus
{
    public sealed class PluginSupportInfo : IPluginSupportInfo
    {
        private readonly Assembly assembly = typeof(PluginSupportInfo).Assembly;

        public string Author => "null54";

        public string Copyright => this.assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;

        public string DisplayName => ThatOtherAppPlusEffect.StaticName;

        public Version Version => this.assembly.GetName().Version;

        public Uri WebsiteUri => new Uri("https://www.getpaint.net/redirect/plugins.html");
    }
}
