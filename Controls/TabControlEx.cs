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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ThatOtherAppPlus.Interop;

namespace ThatOtherAppPlus.Controls
{
    // Adapted from http://dotnetrix.co.uk/tabcontrol.htm#tip2

    /// <summary>
    /// A TabControl that supports custom background and foreground colors
    /// </summary>
    internal sealed class TabControlEx : TabControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components;
        private Color backColor;
        private Color foreColor;
        private Color borderColor;
        private Color hotTrackColor;
        private int hotTabIndex;
        private SolidBrush backgroundBrush;
        private Pen borderPen;
        private SolidBrush hotTrackBrush;

        private static readonly Color DefaultBorderColor = SystemColors.ControlDark;
        private static readonly Color DefaultHotTrackColor = Color.FromArgb(128, SystemColors.HotTrack);

        /// <summary>
        /// Initializes a new instance of the <see cref="TabControlEx"/> class.
        /// </summary>
        public TabControlEx()
        {
            // This call is required by the Windows Forms Designer.
            InitializeComponent();
            this.backColor = Color.Empty;
            this.foreColor = Color.Empty;
            this.borderColor = DefaultBorderColor;
            this.hotTrackColor = DefaultHotTrackColor;
            this.hotTabIndex = -1;
        }

        private enum TabState
        {
            Active = 0,
            MouseOver,
            Inactive
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.components?.Dispose();
                this.backgroundBrush?.Dispose();
                this.borderPen?.Dispose();
                this.hotTrackBrush?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
        }
        #endregion

        [Browsable(true), Description("The background color of the control.")]
        public override Color BackColor
        {
            get
            {
                if (this.backColor.IsEmpty)
                {
                    if (this.Parent == null)
                    {
                        return DefaultBackColor;
                    }
                    else
                    {
                        return this.Parent.BackColor;
                    }
                }

                return this.backColor;
            }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    DetermineDrawingMode();
                    // Let the Tabpages know that the backcolor has changed.
                    OnBackColorChanged(EventArgs.Empty);
                }
            }
        }

        [Browsable(true), Description("The foreground color of the control.")]
        public override Color ForeColor
        {
            get
            {
                if (this.foreColor.IsEmpty)
                {
                    if (this.Parent == null)
                    {
                        return DefaultForeColor;
                    }
                    else
                    {
                        return this.Parent.ForeColor;
                    }
                }

                return this.foreColor;
            }
            set
            {
                if (this.foreColor != value)
                {
                    this.foreColor = value;
                    DetermineDrawingMode();

                    // Let the Tabpages know that the forecolor has changed.
                    OnForeColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the border color of the tabs.
        /// </summary>
        /// <value>
        /// The border color of the tabs.
        /// </value>
        [Description("The border color of the tabs.")]
        public Color BorderColor
        {
            get => this.borderColor;
            set
            {
                if (this.borderColor != value)
                {
                    this.borderColor = value;

                    if (this.borderPen != null)
                    {
                        this.borderPen.Dispose();
                        this.borderPen = new Pen(this.borderColor);
                    }

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color displayed when the mouse is over an inactive tab.
        /// </summary>
        /// <value>
        /// The color displayed when the mouse is over an inactive tab.
        /// </value>
        [Description("The color displayed when the mouse is over an inactive tab.")]
        public Color HotTrackColor
        {
            get => this.hotTrackColor;
            set
            {
                if (this.hotTrackColor != value)
                {
                    this.hotTrackColor = value;

                    if (this.hotTrackBrush != null)
                    {
                        this.hotTrackBrush.Dispose();
                        this.hotTrackBrush = new SolidBrush(this.hotTrackColor);
                    }

                    Invalidate();
                }
            }
        }

        private bool HotTrackingEnabled
        {
            get
            {
                if (SystemInformation.IsHotTrackingEnabled)
                {
                    return true;
                }

                return this.HotTrack;
            }
        }

        private bool UseOwnerDraw
        {
            get
            {
                if (SystemInformation.HighContrast)
                {
                    return false;
                }

                return !this.backColor.IsEmpty && this.backColor != DefaultBackColor || !this.foreColor.IsEmpty && this.foreColor != DefaultForeColor;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetBackColor()
        {
            this.BackColor = Color.Empty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetForeColor()
        {
            this.ForeColor = Color.Empty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetBorderColor()
        {
            this.BorderColor = DefaultBorderColor;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ResetHotTrackColor()
        {
            this.HotTrackColor = DefaultHotTrackColor;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBackColor()
        {
            return !this.backColor.IsEmpty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeForeColor()
        {
            return !this.foreColor.IsEmpty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeBorderColor()
        {
            return this.borderColor != DefaultBorderColor;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHotTrackColor()
        {
            return this.hotTrackColor != DefaultHotTrackColor;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (!this.DesignMode)
            {
                if (this.hotTabIndex != -1)
                {
                    this.hotTabIndex = -1;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!this.DesignMode)
            {
                int index = GetTabIndexUnderCursor();

                if (index != this.hotTabIndex)
                {
                    this.hotTabIndex = index;
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (GetStyle(ControlStyles.UserPaint))
            {
                e.Graphics.Clear(this.BackColor);

                if (this.TabCount <= 0)
                {
                    return;
                }
                int activeTabIndex = this.SelectedIndex;

                // Draw the inactive tabs.
                for (int index = 0; index < this.TabCount; index++)
                {
                    if (index != activeTabIndex)
                    {
                        TabState state = TabState.Inactive;
                        if (index == this.hotTabIndex && this.HotTrackingEnabled)
                        {
                            state = TabState.MouseOver;
                        }

                        DrawTab(e.Graphics, this.TabPages[index], GetTabRect(index), state);
                    }
                }

                // Draw the active tab.
                DrawTab(e.Graphics, this.TabPages[activeTabIndex], GetTabRect(activeTabIndex), TabState.Active);
            }
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);

            DetermineDrawingMode();
        }

        private void DetermineDrawingMode()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, this.UseOwnerDraw);
            UpdateTabPageVisualStyleBackColor();
            if (GetStyle(ControlStyles.UserPaint))
            {
                if (this.backgroundBrush == null || this.backgroundBrush.Color != this.BackColor)
                {
                    this.backgroundBrush?.Dispose();
                    this.backgroundBrush = new SolidBrush(this.BackColor);
                }
                if (this.borderPen == null || this.borderPen.Color != this.borderColor)
                {
                    this.borderPen?.Dispose();
                    this.borderPen = new Pen(this.borderColor);
                }
                if (this.hotTrackBrush == null || this.hotTrackBrush.Color != this.hotTrackColor)
                {
                    this.hotTrackBrush?.Dispose();
                    this.hotTrackBrush = new SolidBrush(this.hotTrackColor);
                }
            }

            Invalidate();
        }

        private void DrawTab(Graphics graphics, TabPage page, Rectangle bounds, TabState state)
        {
            if (state == TabState.Active)
            {
                Rectangle activeTabRect = Rectangle.Inflate(bounds, 2, 2);

                graphics.FillRectangle(this.backgroundBrush, activeTabRect);
                DrawTabBorder(graphics, activeTabRect);
            }
            else if (state == TabState.MouseOver)
            {
                graphics.FillRectangle(this.hotTrackBrush, bounds);
                DrawTabBorder(graphics, bounds);
            }
            else
            {
                graphics.FillRectangle(this.backgroundBrush, bounds);
                DrawTabBorder(graphics, bounds);
            }

            DrawTabText(graphics, bounds, page);
        }

        private void DrawTabBorder(Graphics graphics, Rectangle bounds)
        {
            Point[] points = new Point[6];

            switch (this.Alignment)
            {
                case TabAlignment.Top:
                    points[0] = new Point(bounds.Left, bounds.Top);
                    points[1] = new Point(bounds.Left, bounds.Bottom - 1);
                    points[2] = new Point(bounds.Left, bounds.Top);
                    points[3] = new Point(bounds.Right - 1, bounds.Top);
                    points[4] = new Point(bounds.Right - 1, bounds.Top);
                    points[5] = new Point(bounds.Right - 1, bounds.Bottom - 1);
                    break;
                case TabAlignment.Bottom:
                    points[0] = new Point(bounds.Left, bounds.Top);
                    points[1] = new Point(bounds.Left, bounds.Bottom - 1);
                    points[2] = new Point(bounds.Left, bounds.Bottom - 1);
                    points[3] = new Point(bounds.Right - 1, bounds.Bottom - 1);
                    points[4] = new Point(bounds.Right - 1, bounds.Bottom - 1);
                    points[5] = new Point(bounds.Right - 1, bounds.Top);
                    break;
                case TabAlignment.Left:
                    points[0] = new Point(bounds.Left, bounds.Top);
                    points[1] = new Point(bounds.Right - 1, bounds.Top);
                    points[2] = new Point(bounds.Left, bounds.Top);
                    points[3] = new Point(bounds.Left, bounds.Bottom - 1);
                    points[4] = new Point(bounds.Left, bounds.Bottom - 1);
                    points[5] = new Point(bounds.Right, bounds.Bottom - 1);
                    break;
                case TabAlignment.Right:
                    points[0] = new Point(bounds.Left, bounds.Top);
                    points[1] = new Point(bounds.Right - 1, bounds.Top);
                    points[2] = new Point(bounds.Right - 1, bounds.Top);
                    points[3] = new Point(bounds.Right - 1, bounds.Bottom - 1);
                    points[4] = new Point(bounds.Right - 1, bounds.Bottom - 1);
                    points[5] = new Point(bounds.Left, bounds.Bottom - 1);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            graphics.DrawLines(this.borderPen, points);
        }

        private void DrawTabText(Graphics graphics, Rectangle bounds, TabPage page)
        {
            // Set up rotation for left and right aligned tabs.
            if (this.Alignment == TabAlignment.Left || this.Alignment == TabAlignment.Right)
            {
                float rotateAngle = 90;
                if (this.Alignment == TabAlignment.Left)
                {
                    rotateAngle = 270;
                }

                PointF cp = new PointF(bounds.Left + (bounds.Width / 2), bounds.Top + (bounds.Height / 2));
                graphics.TranslateTransform(cp.X, cp.Y);
                graphics.RotateTransform(rotateAngle);

                bounds = new Rectangle(-(bounds.Height / 2), -(bounds.Width / 2), bounds.Height, bounds.Width);
            }

            Rectangle textBounds = Rectangle.Inflate(bounds, -3, -3);
            Color textColor = page.Enabled ? this.ForeColor : SystemColors.GrayText;

            // Draw the Tab text.
            // The background color is set to transparent because the tab background has already been drawn.
            TextRenderer.DrawText(graphics, page.Text, this.Font, textBounds, textColor, Color.Transparent,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            graphics.ResetTransform();
        }

        private int GetTabIndexUnderCursor()
        {
            Point cursor = PointToClient(MousePosition);

            NativeStructs.TCHITTESTINFO hti = new NativeStructs.TCHITTESTINFO
            {
                pt = new NativeStructs.POINT(cursor.X, cursor.Y),
                flags = 0
            };

            int index = SafeNativeMethods.SendMessage(this.Handle, NativeConstants.TCM_HITTEST, IntPtr.Zero, ref hti).ToInt32();

            return index;
        }

        private void UpdateTabPageVisualStyleBackColor()
        {
            bool useVisualStyleBackColor = false;
            if (SystemInformation.HighContrast || this.BackColor == DefaultBackColor)
            {
                useVisualStyleBackColor = true;
            }

            // When the BackColor is changed the TabControl only updates the UseVisualStyleBackColor property for the active tab.
            // Set the property on all tabs so that the correct color is displayed when the user switches tabs.

            for (int i = 0; i < this.TabCount; i++)
            {
                this.TabPages[i].UseVisualStyleBackColor = useVisualStyleBackColor;
            }
        }
    }
}
