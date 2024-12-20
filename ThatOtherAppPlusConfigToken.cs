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

using PaintDotNet;
using PaintDotNet.Effects;

namespace ThatOtherAppPlus
{
    public class ThatOtherAppPlusConfigToken : EffectConfigToken
    {
        internal ThatOtherAppPlusConfigToken()
        {
            this.Surface = null;
        }

        private ThatOtherAppPlusConfigToken(ThatOtherAppPlusConfigToken copyMe)
        {
            this.Surface = copyMe.Surface;
        }

        public Surface Surface { get; set; }

        public override object Clone()
        {
            return new ThatOtherAppPlusConfigToken(this);
        }
    }
}
