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
using System.Drawing;
using System.Windows.Forms;

namespace ThatOtherAppPlus.Controls
{
    // Adapted from https://dotnetrix.co.uk/button.htm#tip3

    internal sealed class ImageButton : Control, IButtonControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components;
        private Image image;
        private SolidBrush backgroundBrush;
        private SolidBrush hotTrackBrush;
        private ButtonDrawState drawState;
        private bool keyPressed;

        public ImageButton()
        {
            SetStyle(ControlStyles.Selectable |
                     ControlStyles.StandardClick |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint,
                     true);


            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.drawState = ButtonDrawState.Normal;
            this.backgroundBrush = new SolidBrush(this.BackColor);
            this.hotTrackBrush = new SolidBrush(Color.FromArgb(128, SystemColors.HotTrack));
        }

        public ImageButton(Bitmap image, object tag) : this()
        {
            if (image is null)
            {
                ExceptionUtil.ThrowArgumentNullException(nameof(image));
            }


            this.Size = GetButtonSizeForImage(image);
            this.image = image;
            this.Tag = tag;
        }

        private enum ButtonDrawState
        {
            Normal = 0,
            Hot,
            Pressed,
            Disabled,
            Focused
        }

        public DialogResult DialogResult { get; set; }

        public bool IsDefault { get; private set; }

        public Image Image
        {
            get
            {
                if (this.IsDisposed)
                {
                    ExceptionUtil.ThrowObjectDisposedException(nameof(ImageButton));
                }

                return this.image;
            }
            set
            {
                this.image?.Dispose();

                if (value != null)
                {
                    this.Size = GetButtonSizeForImage(value);
                }

                this.image = value;
            }

        }


        private static Size GetButtonSizeForImage(Image image)
        {
            return new Size(image.Width + 5, image.Height + 5);
        }

        public void NotifyDefault(bool value)
        {
            this.IsDefault = value;
        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.components?.Dispose();
                this.image?.Dispose();
                this.backgroundBrush?.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this.DoubleBuffered = true;
        }
        #endregion

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            this.backgroundBrush.Color = this.BackColor;

            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            Invalidate();
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);

            this.backgroundBrush.Color = this.BackColor;

            Invalidate();
        }

        protected override void OnParentForeColorChanged(EventArgs e)
        {
            base.OnParentForeColorChanged(e);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawBackground(e.Graphics);

            Point point = new Point
            {
                X = (this.Width - this.Image.Width) / 2,
                Y = (this.Height - this.Image.Height) / 2
            };

            if (this.Enabled)
            {
                if (this.drawState == ButtonDrawState.Pressed)
                {
                    point.Offset(1, 1);
                }

                if (this.image != null)
                {
                    e.Graphics.DrawImage(this.image, point);
                }

                if (this.drawState == ButtonDrawState.Focused)
                {
                    DrawFocusRectangle(e.Graphics);
                }
            }
            else
            {
                if (this.image != null)
                {
                    ControlPaint.DrawImageDisabled(e.Graphics, this.image, point.X, point.Y, this.BackColor);
                }
            }

            base.OnPaint(e);
        }

        private void DrawBackground(Graphics graphics)
        {
            if (this.drawState == ButtonDrawState.Hot && this.Enabled)
            {
                graphics.FillRectangle(this.hotTrackBrush, this.ClientRectangle);
            }
            else
            {
                graphics.FillRectangle(this.backgroundBrush, this.ClientRectangle);
            }
        }

        private void DrawFocusRectangle(Graphics g)
        {
            if (!this.ShowFocusCues || !this.Focused)
            {
                return;
            }

            Rectangle focusRect = this.ClientRectangle;
            focusRect.Inflate(-3, -3);
            ControlPaint.DrawFocusRectangle(g, focusRect);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!this.Enabled)
            {
                return;
            }

            if (e.KeyCode == Keys.Space)
            {
                this.keyPressed = true;
                this.drawState = ButtonDrawState.Pressed;
                Invalidate();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Space)
            {
                this.keyPressed = false;
                this.drawState = ButtonDrawState.Focused;
                Invalidate();
                OnClick(EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                OnClick(EventArgs.Empty);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (this.Enabled && !this.keyPressed)
            {
                this.drawState = ButtonDrawState.Hot;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.Enabled && !this.keyPressed)
            {
                if (this.IsDefault)
                {
                    this.drawState = ButtonDrawState.Focused;
                }
                else
                {
                    this.drawState = ButtonDrawState.Normal;
                }
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!this.Enabled)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                this.drawState = ButtonDrawState.Pressed;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.Enabled)
            {
                if (this.drawState == ButtonDrawState.Pressed)
                {
                    OnClick(EventArgs.Empty);
                }

                this.drawState = ButtonDrawState.Focused;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!this.Enabled)
            {
                return;
            }

            if (e.Button == MouseButtons.Left && this.Bounds.Contains(e.X, e.Y))
            {
                this.drawState = ButtonDrawState.Pressed;
            }
            else
            {
                if (this.keyPressed)
                {
                    return;
                }

                this.drawState = ButtonDrawState.Hot;
            }
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (this.Enabled)
            {
                this.drawState = ButtonDrawState.Focused;
                Invalidate();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (this.Enabled)
            {
                this.drawState = ButtonDrawState.Normal;
                Invalidate();
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this.Enabled)
            {
                this.drawState = ButtonDrawState.Normal;
            }
            else
            {
                this.drawState = ButtonDrawState.Disabled;
            }
            Invalidate();

        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            base.OnLostFocus(EventArgs.Empty);
        }


        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            base.OnLostFocus(EventArgs.Empty);
        }

        protected override void WndProc(ref Message m)
        {
            const int BM_CLICK = 0x00F5;

            if (m.Msg == BM_CLICK)
            {
                OnClick(EventArgs.Empty);
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
