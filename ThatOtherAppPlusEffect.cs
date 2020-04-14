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
using PaintDotNet.Effects;
using System.Drawing;

namespace ThatOtherAppPlus
{
    [PluginSupportInfo(typeof(PluginSupportInfo))]
    public sealed class ThatOtherAppPlusEffect : Effect
    {
        private bool repeatEffect;

        internal static string StaticName
        {
            get
            {
                return "That other app+";
            }
        }

        internal static Bitmap StaticIcon
        {
            get
            {
                return new Bitmap(typeof(ThatOtherAppPlusEffect), PluginIconUtil.GetIconResourceNameForDpi(UIScaleFactor.Current.Dpi));
            }
        }

        public ThatOtherAppPlusEffect()
            : base(StaticName, StaticIcon, "Tools", new EffectOptions { Flags = EffectFlags.Configurable })
        {
            this.repeatEffect = true;
        }

        public override EffectConfigDialog CreateConfigDialog()
        {
            this.repeatEffect = false;
            return new ThatOtherAppPlusConfigDialog();
        }

        protected override void OnSetRenderInfo(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            if (this.repeatEffect)
            {
                // This plugin does not support being run as a repeat effect.

                ThatOtherAppPlusConfigToken token = (ThatOtherAppPlusConfigToken)parameters;

                if (token.Surface != null)
                {
                    token.Surface.Dispose();
                    token.Surface = null;
                }
            }

            base.OnSetRenderInfo(parameters, dstArgs, srcArgs);
        }

        public override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            if (length == 0)
            {
                return;
            }

            ThatOtherAppPlusConfigToken token = (ThatOtherAppPlusConfigToken)parameters;

            if (token.Surface != null)
            {
                dstArgs.Surface.CopySurface(token.Surface, rois, startIndex, length);
            }
            else
            {
                dstArgs.Surface.CopySurface(srcArgs.Surface, rois, startIndex, length);
            }
        }
    }
}
