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

using System.Windows.Forms;

namespace ThatOtherAppPlus.Controls
{
    internal sealed class DoubleBufferedListView : ListView
    {
        public DoubleBufferedListView() : base()
        {
            this.DoubleBuffered = true;
        }
    }
}
