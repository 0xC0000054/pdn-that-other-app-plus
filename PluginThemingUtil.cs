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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ThatOtherAppPlus
{
    internal static class PluginThemingUtil
    {
        public static void UpdateControlBackColor(Control root)
        {
            Color backColor = root.BackColor;

            Stack<Control> stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Control parent = stack.Pop();

                Control.ControlCollection controls = parent.Controls;

                for (int i = 0; i < controls.Count; i++)
                {
                    Control control = controls[i];

                    // Update the BackColor for all child controls as some controls
                    // do not change the BackColor when the parent control does.

                    control.BackColor = backColor;

                    if (control.HasChildren)
                    {
                        stack.Push(control);
                    }
                }
            }
        }

        public static void UpdateControlForeColor(Control root)
        {
            Color foreColor = root.ForeColor;

            Stack<Control> stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Control parent = stack.Pop();

                Control.ControlCollection controls = parent.Controls;

                for (int i = 0; i < controls.Count; i++)
                {
                    Control control = controls[i];

                    if (control is LinkLabel link)
                    {
                        if (foreColor != Control.DefaultForeColor)
                        {
                            link.LinkColor = foreColor;
                        }
                        else
                        {
                            // If the control is using the default foreground color set the link color
                            // to Color.Empty so the LinkLabel will use its default colors.
                            link.LinkColor = Color.Empty;
                        }
                    }
                    else
                    {
                        // Update the ForeColor for all child controls as some controls
                        // do not change the ForeColor when the parent control does.

                        control.ForeColor = foreColor;

                        if (control.HasChildren)
                        {
                            stack.Push(control);
                        }
                    }
                }
            }
        }
    }
}
