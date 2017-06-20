using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;
//C# NETSEAL THEME
//ORIGINAL : AEONHACK!
//CONVERTED FULLY BY: Trontor!

abstract class ThemeContainer154 : ContainerControl
{

    #region " Initialization "

    protected Graphics G;

    protected Bitmap B;
    public ThemeContainer154()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        if (DoneCreation)
            InitializeMessages();

        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;
        if (!_ControlMode)
            base.Dock = DockStyle.Fill;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    private bool DoneCreation;
    protected override sealed void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);

        if (Parent == null)
            return;
        _IsParentForm = Parent is Form;

        if (!_ControlMode)
        {
            InitializeMessages();

            if (_IsParentForm)
            {
                ParentForm.FormBorderStyle = _BorderStyle;
                ParentForm.TransparencyKey = _TransparencyKey;

                if (!DesignMode)
                {
                    ParentForm.Shown += FormShown;
                }
            }

            Parent.BackColor = BackColor;
        }

        OnCreation();
        DoneCreation = true;
        InvalidateTimer();
    }

    #endregion

    private void DoAnimation(bool i)
    {
        OnAnimation();
        if (i)
            Invalidate();
    }

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent && _ControlMode)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        ThemeShare.RemoveAnimationCallback(DoAnimation);
        base.OnHandleDestroyed(e);
    }

    private bool HasShown;
    private void FormShown(object sender, EventArgs e)
    {
        if (_ControlMode || HasShown)
            return;

        if (_StartPosition == FormStartPosition.CenterParent || _StartPosition == FormStartPosition.CenterScreen)
        {
            Rectangle SB = Screen.PrimaryScreen.Bounds;
            Rectangle CB = ParentForm.Bounds;
            //ParentForm.Location = new Point(SB.Width / 2 - CB.Width / 2, SB.Height / 2 - CB.Width / 2);
        }

        HasShown = true;
    }


    #region " Size Handling "

    private Rectangle Frame;
    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Movable && !_ControlMode)
        {
            Frame = new Rectangle(7, 7, Width - 14, _Header - 7);
        }

        InvalidateBitmap();
        Invalidate();

        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    protected MouseState State;
    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
        {
            if (_Sizable && !_ControlMode)
                InvalidateMouse();
        }

        base.OnMouseMove(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        SetState(MouseState.None);

        if (GetChildAtPoint(PointToClient(MousePosition)) != null)
        {
            if (_Sizable && !_ControlMode)
            {
                Cursor = Cursors.Default;
                Previous = 0;
            }
        }

        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
            SetState(MouseState.Down);

        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
        {
            if (_Movable && Frame.Contains(e.Location))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[0]);
            }
            else if (_Sizable && !(Previous == 0))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[Previous]);
            }
        }

        base.OnMouseDown(e);
    }

    private bool WM_LMBUTTONDOWN;
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (WM_LMBUTTONDOWN && m.Msg == 513)
        {
            WM_LMBUTTONDOWN = false;

            SetState(MouseState.Over);
            if (!_SmartBounds)
                return;

            if (IsParentMdi)
            {
                CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
            }
            else
            {
                CorrectBounds(Screen.FromControl(Parent).WorkingArea);
            }
        }
    }

    private Point GetIndexPoint;
    private bool B1;
    private bool B2;
    private bool B3;
    private bool B4;
    private int GetIndex()
    {
        GetIndexPoint = PointToClient(MousePosition);
        B1 = GetIndexPoint.X < 7;
        B2 = GetIndexPoint.X > Width - 7;
        B3 = GetIndexPoint.Y < 7;
        B4 = GetIndexPoint.Y > Height - 7;

        if (B1 && B3)
            return 4;
        if (B1 && B4)
            return 7;
        if (B2 && B3)
            return 5;
        if (B2 && B4)
            return 8;
        if (B1)
            return 1;
        if (B2)
            return 2;
        if (B3)
            return 3;
        if (B4)
            return 6;
        return 0;
    }

    private int Current;
    private int Previous;
    private void InvalidateMouse()
    {
        Current = GetIndex();
        if (Current == Previous)
            return;

        Previous = Current;
        switch (Previous)
        {
            case 0:
                Cursor = Cursors.Default;
                break;
            case 1:
            case 2:
                Cursor = Cursors.SizeWE;
                break;
            case 3:
            case 6:
                Cursor = Cursors.SizeNS;
                break;
            case 4:
            case 8:
                Cursor = Cursors.SizeNWSE;
                break;
            case 5:
            case 7:
                Cursor = Cursors.SizeNESW;
                break;
        }
    }

    private Message[] Messages = new Message[9];
    private void InitializeMessages()
    {
        Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
        for (int I = 1; I <= 8; I++)
        {
            Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
        }
    }

    private void CorrectBounds(Rectangle bounds)
    {
        if (Parent.Width > bounds.Width)
            Parent.Width = bounds.Width;
        if (Parent.Height > bounds.Height)
            Parent.Height = bounds.Height;

        int X = Parent.Location.X;
        int Y = Parent.Location.Y;

        if (X < bounds.X)
            X = bounds.X;
        if (Y < bounds.Y)
            Y = bounds.Y;

        int Width = bounds.X + bounds.Width;
        int Height = bounds.Y + bounds.Height;

        if (X + Parent.Width > Width)
            X = Width - Parent.Width;
        if (Y + Parent.Height > Height)
            Y = Height - Parent.Height;

        Parent.Location = new Point(X, Y);
    }

    #endregion


    #region " Base Properties "

    public override DockStyle Dock
    {
        get { return base.Dock; }
        set
        {
            if (!_ControlMode)
                return;
            base.Dock = value;
        }
    }

    private bool _BackColor;
    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (value == base.BackColor)
                return;

            if (!IsHandleCreated && _ControlMode && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
            {
                if (!_ControlMode)
                    Parent.BackColor = value;
                ColorHook();
            }
        }
    }

    public override Size MinimumSize
    {
        get { return base.MinimumSize; }
        set
        {
            base.MinimumSize = value;
            if (Parent != null)
                Parent.MinimumSize = value;
        }
    }

    public override Size MaximumSize
    {
        get { return base.MaximumSize; }
        set
        {
            base.MaximumSize = value;
            if (Parent != null)
                Parent.MaximumSize = value;
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    #endregion

    #region " Public Properties "

    private bool _SmartBounds = true;
    public bool SmartBounds
    {
        get { return _SmartBounds; }
        set { _SmartBounds = value; }
    }

    private bool _Movable = true;
    public bool Movable
    {
        get { return _Movable; }
        set { _Movable = value; }
    }

    private bool _Sizable = true;
    public bool Sizable
    {
        get { return _Sizable; }
        set { _Sizable = value; }
    }

    private Color _TransparencyKey;
    public Color TransparencyKey
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.TransparencyKey;
            else
                return _TransparencyKey;
        }
        set
        {
            if (value == _TransparencyKey)
                return;
            _TransparencyKey = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.TransparencyKey = value;
                ColorHook();
            }
        }
    }

    private FormBorderStyle _BorderStyle;
    public FormBorderStyle BorderStyle
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.FormBorderStyle;
            else
                return _BorderStyle;
        }
        set
        {
            _BorderStyle = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.FormBorderStyle = value;

                if (!(value == FormBorderStyle.None))
                {
                    Movable = false;
                    Sizable = false;
                }
            }
        }
    }

    private FormStartPosition _StartPosition;
    public FormStartPosition StartPosition
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.StartPosition;
            else
                return _StartPosition;
        }
        set
        {
            _StartPosition = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.StartPosition = value;
            }
        }
    }

    private bool _NoRounding;
    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    private Image _Image;
    public Image Image
    {
        get { return _Image; }
        set
        {
            if (value == null)
                _ImageSize = Size.Empty;
            else
                _ImageSize = value.Size;

            _Image = value;
            Invalidate();
        }
    }

    private Dictionary<string, Color> Items = new Dictionary<string, Color>();
    public Bloom[] Colors
    {
        get
        {
            List<Bloom> T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    private string _Customization;
    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    private bool _Transparent;
    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!(IsHandleCreated || _ControlMode))
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            InvalidateBitmap();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private Size _ImageSize;
    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    private bool _IsParentForm;
    protected bool IsParentForm
    {
        get { return _IsParentForm; }
    }

    protected bool IsParentMdi
    {
        get
        {
            if (Parent == null)
                return false;
            return Parent.Parent != null;
        }
    }

    private int _LockWidth;
    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    private int _LockHeight;
    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    private int _Header = 24;
    protected int Header
    {
        get { return _Header; }
        set
        {
            _Header = value;

            if (!_ControlMode)
            {
                Frame = new Rectangle(7, 7, Width - 14, value - 7);
                Invalidate();
            }
        }
    }

    private bool _ControlMode;
    protected bool ControlMode
    {
        get { return _ControlMode; }
        set
        {
            _ControlMode = value;

            Transparent = _Transparent;
            if (_Transparent && _BackColor)
                BackColor = Color.Transparent;

            InvalidateBitmap();
            Invalidate();
        }
    }

    private bool _IsAnimated;
    protected bool IsAnimated
    {
        get { return _IsAnimated; }
        set
        {
            _IsAnimated = value;
            InvalidateTimer();
        }
    }

    #endregion


    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }
    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }
    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }
    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }
    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (_Transparent && _ControlMode)
        {
            if (Width == 0 || Height == 0)
                return;
            B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            G = Graphics.FromImage(B);
        }
        else
        {
            G = null;
            B = null;
        }
    }

    private void InvalidateCustimization()
    {
        MemoryStream M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    private void InvalidateTimer()
    {
        if (DesignMode || !DoneCreation)
            return;

        if (_IsAnimated)
        {
            ThemeShare.AddAnimationCallback(DoAnimation);
        }
        else
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
        }
    }

    #endregion


    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    protected virtual void OnAnimation()
    {
    }

    #endregion


    #region " Offset "

    private Rectangle OffsetReturnRectangle;
    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    private Size OffsetReturnSize;
    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    private Point OffsetReturnPoint;
    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "


    private Point CenterReturn;
    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }
    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private Bitmap MeasureBitmap;

    private Graphics MeasureGraphics;
    protected Size Measure()
    {
        lock (MeasureGraphics)
        {
            return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
        }
    }
    protected Size Measure(string text)
    {
        lock (MeasureGraphics)
        {
            return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
        }
    }

    #endregion


    #region " DrawPixel "


    private SolidBrush DrawPixelBrush;
    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "


    private SolidBrush DrawCornersBrush;
    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }
    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }
    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }
    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }
    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;

    private Size DrawTextSize;
    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }
    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = new Point(Width / 2 - DrawTextSize.Width / 2, Header / 2 - DrawTextSize.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }
    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "


    private Point DrawImagePoint;
    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }
    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = new Point(Width / 2 - image.Width / 2, Header / 2 - image.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }
    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }
    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;

    private Rectangle DrawGradientRectangle;
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }
    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;

    private Rectangle DrawRadialRectangle;
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle);
    }
    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawGradientBrush, r);
    }

    #endregion

    #region " CreateRound "

    private GraphicsPath CreateRoundPath;

    private Rectangle CreateRoundRectangle;
    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion

}

abstract class ThemeControl154 : Control
{
    #region " Initialization "

    protected Graphics G;

    protected Bitmap B;
    public ThemeControl154()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
        //Remove?
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    private bool DoneCreation;
    protected override sealed void OnParentChanged(EventArgs e)
    {
        if (Parent != null)
        {
            OnCreation();
            DoneCreation = true;
            InvalidateTimer();
        }

        base.OnParentChanged(e);
    }

    #endregion

    private void DoAnimation(bool i)
    {
        OnAnimation();
        if (i)
            Invalidate();
    }

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        ThemeShare.RemoveAnimationCallback(DoAnimation);
        base.OnHandleDestroyed(e);
    }

    #region " Size Handling "

    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Transparent)
        {
            InvalidateBitmap();
        }

        Invalidate();
        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    private bool InPosition;
    protected override void OnMouseEnter(EventArgs e)
    {
        InPosition = true;
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (InPosition)
            SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
            SetState(MouseState.Down);
        base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        InPosition = false;
        SetState(MouseState.None);
        base.OnMouseLeave(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    protected MouseState State;
    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    #endregion


    #region " Base Properties "

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    private bool _BackColor;
    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (!IsHandleCreated && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
                ColorHook();
        }
    }

    #endregion

    #region " Public Properties "

    private bool _NoRounding;
    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    private Image _Image;
    public Image Image
    {
        get { return _Image; }
        set
        {
            if (value == null)
            {
                _ImageSize = Size.Empty;
            }
            else
            {
                _ImageSize = value.Size;
            }

            _Image = value;
            Invalidate();
        }
    }

    private bool _Transparent;
    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!IsHandleCreated)
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            if (value)
                InvalidateBitmap();
            else
                B = null;
            Invalidate();
        }
    }

    private Dictionary<string, Color> Items = new Dictionary<string, Color>();
    public Bloom[] Colors
    {
        get
        {
            List<Bloom> T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    private string _Customization;
    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private Size _ImageSize;
    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    private int _LockWidth;
    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    private int _LockHeight;
    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    private bool _IsAnimated;
    protected bool IsAnimated
    {
        get { return _IsAnimated; }
        set
        {
            _IsAnimated = value;
            InvalidateTimer();
        }
    }

    #endregion


    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }
    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }
    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }
    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }
    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (Width == 0 || Height == 0)
            return;
        B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
        G = Graphics.FromImage(B);
    }

    private void InvalidateCustimization()
    {
        MemoryStream M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    private void InvalidateTimer()
    {
        if (DesignMode || !DoneCreation)
            return;

        if (_IsAnimated)
        {
            ThemeShare.AddAnimationCallback(DoAnimation);
        }
        else
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
        }
    }
    #endregion


    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    protected virtual void OnAnimation()
    {
    }

    #endregion


    #region " Offset "

    private Rectangle OffsetReturnRectangle;
    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    private Size OffsetReturnSize;
    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    private Point OffsetReturnPoint;
    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "


    private Point CenterReturn;
    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }
    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }
    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private Bitmap MeasureBitmap;
    //TODO: Potential issues during multi-threading.
    private Graphics MeasureGraphics;

    protected Size Measure()
    {
        return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
    }
    protected Size Measure(string text)
    {
        return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
    }

    #endregion


    #region " DrawPixel "


    private SolidBrush DrawPixelBrush;
    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "


    private SolidBrush DrawCornersBrush;
    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }
    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }
    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }
    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }
    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }
    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }
    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;

    private Size DrawTextSize;
    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }
    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = Center(DrawTextSize);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }
    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "


    private Point DrawImagePoint;
    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }
    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = Center(image.Size);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }
    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }
    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;

    private Rectangle DrawGradientRectangle;
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }
    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }
    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }
    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;

    private Rectangle DrawRadialRectangle;
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }
    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle);
    }
    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillEllipse(DrawRadialBrush2, r);
    }
    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawRadialBrush2, r);
    }

    #endregion

    #region " CreateRound "

    private GraphicsPath CreateRoundPath;

    private Rectangle CreateRoundRectangle;
    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion

}

static class ThemeShare
{

    #region " Animation "

    private static int Frames;
    private static bool Invalidate;

    public static PrecisionTimer ThemeTimer = new PrecisionTimer();
    //1000 / 50 = 20 FPS
    private const int FPS = 50;

    private const int Rate = 10;
    public delegate void AnimationDelegate(bool invalidate);


    private static List<AnimationDelegate> Callbacks = new List<AnimationDelegate>();
    private static void HandleCallbacks(IntPtr state, bool reserve)
    {
        Invalidate = (Frames >= FPS);
        if (Invalidate)
            Frames = 0;

        lock (Callbacks)
        {
            for (int I = 0; I <= Callbacks.Count - 1; I++)
            {
                Callbacks[I].Invoke(Invalidate);
            }
        }

        Frames += Rate;
    }

    private static void InvalidateThemeTimer()
    {
        if (Callbacks.Count == 0)
        {
            ThemeTimer.Delete();
        }
        else
        {
            ThemeTimer.Create(0, Rate, HandleCallbacks);
        }
    }

    public static void AddAnimationCallback(AnimationDelegate callback)
    {
        lock (Callbacks)
        {
            if (Callbacks.Contains(callback))
                return;

            Callbacks.Add(callback);
            InvalidateThemeTimer();
        }
    }

    public static void RemoveAnimationCallback(AnimationDelegate callback)
    {
        lock (Callbacks)
        {
            if (!Callbacks.Contains(callback))
                return;

            Callbacks.Remove(callback);
            InvalidateThemeTimer();
        }
    }

    #endregion

}

enum MouseState : byte
{
    None = 0,
    Over = 1,
    Down = 2,
    Block = 3
}

struct Bloom
{

    public string _Name;
    public string Name
    {
        get { return _Name; }
    }

    private Color _Value;
    public Color Value
    {
        get { return _Value; }
        set { _Value = value; }
    }

    public string ValueHex
    {
        get { return string.Concat("#", _Value.R.ToString("X2", null), _Value.G.ToString("X2", null), _Value.B.ToString("X2", null)); }
        set
        {
            try
            {
                _Value = ColorTranslator.FromHtml(value);
            }
            catch
            {
                return;
            }
        }
    }


    public Bloom(string name, Color value)
    {
        _Name = name;
        _Value = value;
    }
}

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 11/30/2011
//Changed: 11/30/2011
//Version: 1.0.0
//------------------
class PrecisionTimer : IDisposable
{

    private bool _Enabled;
    public bool Enabled
    {
        get { return _Enabled; }
    }

    private IntPtr Handle;

    private TimerDelegate TimerCallback;
    [DllImport("kernel32.dll", EntryPoint = "CreateTimerQueueTimer")]
    private static extern bool CreateTimerQueueTimer(ref IntPtr handle, IntPtr queue, TimerDelegate callback, IntPtr state, uint dueTime, uint period, uint flags);

    [DllImport("kernel32.dll", EntryPoint = "DeleteTimerQueueTimer")]
    private static extern bool DeleteTimerQueueTimer(IntPtr queue, IntPtr handle, IntPtr callback);

    public delegate void TimerDelegate(IntPtr r1, bool r2);

    public void Create(uint dueTime, uint period, TimerDelegate callback)
    {
        if (_Enabled)
            return;

        TimerCallback = callback;
        bool Success = CreateTimerQueueTimer(ref Handle, IntPtr.Zero, TimerCallback, IntPtr.Zero, dueTime, period, 0);

        if (!Success)
            ThrowNewException("CreateTimerQueueTimer");
        _Enabled = Success;
    }

    public void Delete()
    {
        if (!_Enabled)
            return;
        bool Success = DeleteTimerQueueTimer(IntPtr.Zero, Handle, IntPtr.Zero);

        if (!Success && !(Marshal.GetLastWin32Error() == 997))
        {
            ThrowNewException("DeleteTimerQueueTimer");
        }

        _Enabled = !Success;
    }

    private void ThrowNewException(string name)
    {
        throw new Exception(string.Format("{0} failed. Win32Error: {1}", name, Marshal.GetLastWin32Error()));
    }

    public void Dispose()
    {
        Delete();
    }
}

public class ThemeModule
{
    static internal Graphics G;

    public ThemeModule()
    {
        TextBitmap = new Bitmap(1, 1);
        TextGraphics = Graphics.FromImage(TextBitmap);
    }

    private static Bitmap TextBitmap;

    private static Graphics TextGraphics;
    static internal SizeF MeasureString(string text, Font font)
    {
        return TextGraphics.MeasureString(text, font);
    }

    static internal SizeF MeasureString(string text, Font font, int width)
    {
        return TextGraphics.MeasureString(text, font, width, StringFormat.GenericTypographic);
    }

    private static GraphicsPath CreateRoundPath;

    private static Rectangle CreateRoundRectangle;
    static internal GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    static internal GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

}

class NSTheme : ThemeContainer154
{

    private int _AccentOffset = 42;
    public int AccentOffset
    {
        get { return _AccentOffset; }
        set
        {
            _AccentOffset = value;
            Invalidate();
        }
    }

    public NSTheme()
    {
        Header = 30;
        BackColor = Color.FromArgb(50, 50, 50);

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(60, 60, 60));

        B1 = new SolidBrush(Color.FromArgb(50, 50, 50));
    }


    protected override void ColorHook()
    {
    }


    private Rectangle R1;
    private Pen P1;
    private Pen P2;

    private SolidBrush B1;

    private int Pad;
    protected override void PaintHook()
    {
        G.Clear(BackColor);
        DrawBorders(P2, 1);

        G.DrawLine(P1, 0, 26, Width, 26);
        G.DrawLine(P2, 0, 25, Width, 25);

        Pad = Math.Max(Measure().Width + 20, 80);
        R1 = new Rectangle(Pad, 17, Width - (Pad * 2) + _AccentOffset, 8);

        G.DrawRectangle(P2, R1);
        G.DrawRectangle(P1, R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height);

        G.DrawLine(P1, 0, 29, Width, 29);
        G.DrawLine(P2, 0, 30, Width, 30);

        DrawText(Brushes.Black, HorizontalAlignment.Left, 8, 1);
        DrawText(Brushes.White, HorizontalAlignment.Left, 7, 0);

        G.FillRectangle(B1, 0, 27, Width, 2);
        DrawBorders(Pens.Black);
    }

}

class NSButton : Control
{

    public NSButton()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
    }

    private bool IsMouseDown;
    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;

    private Pen P2;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        if (IsMouseDown)
        {
            PB1 = new PathGradientBrush(GP1);
            PB1.CenterColor = Color.FromArgb(60, 60, 60);
            PB1.SurroundColors = new Color[] { Color.FromArgb(55, 55, 55) };
            PB1.FocusScales = new PointF(0.8f, 0.5f);

            G.FillPath(PB1, GP1);
        }
        else
        {
            GB1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
            G.FillPath(GB1, GP1);
        }

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(5, Height / 2 - SZ1.Height / 2);

        if (IsMouseDown)
        {
            PT1.X += 1f;
            PT1.Y += 1f;
        }
        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        IsMouseDown = true;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        IsMouseDown = false;
        Invalidate();
    }

}

class NSProgressBar : ProgressBar
{
    private int _Minimum;


    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;
            Invalidate();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;
            Invalidate();
        }
    }

    private int _Value;
    public int Value
    {
        get { return _Value; }
        set
        {
            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            Invalidate();
        }
    }

    private void Increment(int amount)
    {
        Value += amount;
    }

    public NSProgressBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
        
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;

    private GraphicsPath GP3;
    private Rectangle R1;

    private Rectangle R2;
    private Pen P1;
    private Pen P2;
    private SolidBrush B1;
    private LinearGradientBrush GB1;

    private LinearGradientBrush GB2;

    private int I1;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Rectangle rec = e.ClipRectangle;
        

        rec.Width = (int)(rec.Width * ((double)(Value<=0?1:Value) / Maximum)) - 4;

        e.Graphics.Clear(BackColor);

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        e.Graphics.DrawPath(P2, GP1);
        e.Graphics.DrawPath(P1, GP2);

        if (ProgressBarRenderer.IsSupported)
        {
            GB1 = new LinearGradientBrush(rec, Color.FromArgb(45, 45, 45), Color.FromArgb(50, 50, 50), 90f);
            e.Graphics.FillRectangle(GB1, rec);
        }
            //ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
        rec.Height = rec.Height - 4;


        SolidBrush bb = new SolidBrush(Color.FromArgb(205, 150, 0));
        e.Graphics.FillRectangle(bb, 2, 2, rec.Width, rec.Height);
        /*Graphics G = e.Graphics;

        //G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        R1 = new Rectangle(0, 2, Width - 1, Height - 1);
        GB1 = new LinearGradientBrush(R1, Color.FromArgb(45, 45, 45), Color.FromArgb(50, 50, 50), 90f);

        G.SetClip(GP1);
        G.FillRectangle(GB1, R1);

        I1 = Convert.ToInt32((Value - Minimum) / (Maximum - Minimum) * (Width - 3));

        if (I1 > 1)
        {
            GP3 = ThemeModule.CreateRound(1, 1, I1, Height - 3, 7);

            R2 = new Rectangle(1, 1, I1, Height - 3);
            GB2 = new LinearGradientBrush(R2, Color.FromArgb(205, 150, 0), Color.FromArgb(150, 110, 0), 90f);

            G.FillPath(GB2, GP3);
            G.DrawPath(P1, GP3);

            G.SetClip(GP3);
            G.SmoothingMode = SmoothingMode.None;

            G.FillRectangle(B1, R2.X, R2.Y + 1, R2.Width, R2.Height / 2);

            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.ResetClip();
        }

        G.DrawPath(P2, GP1);
        G.DrawPath(P1, GP2);*/
    }

}

class NSLabel : Control
{

    public NSLabel()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Font = new Font("Segoe UI", 11.25f, FontStyle.Bold);

        B1 = new SolidBrush(Color.FromArgb(205, 150, 0));
    }

    private string _Value1 = "NET";
    public string Value1
    {
        get { return _Value1; }
        set
        {
            _Value1 = value;
            Invalidate();
        }
    }

    private string _Value2 = "SEAL";
    public string Value2
    {
        get { return _Value2; }
        set
        {
            _Value2 = value;
            Invalidate();
        }
    }


    private SolidBrush B1;
    private PointF PT1;
    private PointF PT2;
    private SizeF SZ1;

    private SizeF SZ2;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);

        SZ1 = G.MeasureString(Value1, Font, Width, StringFormat.GenericTypographic);
        SZ2 = G.MeasureString(Value2, Font, Width, StringFormat.GenericTypographic);

        PT1 = new PointF(0, Height / 2 - SZ1.Height / 2);
        PT2 = new PointF(SZ1.Width + 1, Height / 2 - SZ1.Height / 2);

        G.DrawString(Value1, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Value1, Font, Brushes.White, PT1);

        G.DrawString(Value2, Font, Brushes.Black, PT2.X + 1, PT2.Y + 1);
        G.DrawString(Value2, Font, B1, PT2);
    }

}

[DefaultEvent("TextChanged")]
class NSTextBox : Control
{

    private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
    public HorizontalAlignment TextAlign
    {
        get { return _TextAlign; }
        set
        {
            _TextAlign = value;
            if (Base != null)
            {
                Base.TextAlign = value;
            }
        }
    }

    private int _MaxLength = 32767;
    public int MaxLength
    {
        get { return _MaxLength; }
        set
        {
            _MaxLength = value;
            if (Base != null)
            {
                Base.MaxLength = value;
            }
        }
    }

    private bool _ReadOnly;
    public bool ReadOnly
    {
        get { return _ReadOnly; }
        set
        {
            _ReadOnly = value;
            if (Base != null)
            {
                Base.ReadOnly = value;
            }
        }
    }

    private bool _UseSystemPasswordChar;
    public bool UseSystemPasswordChar
    {
        get { return _UseSystemPasswordChar; }
        set
        {
            _UseSystemPasswordChar = value;
            if (Base != null)
            {
                Base.UseSystemPasswordChar = value;
            }
        }
    }

    private bool _Multiline;
    public bool Multiline
    {
        get { return _Multiline; }
        set
        {
            _Multiline = value;
            if (Base != null)
            {
                Base.Multiline = value;

                if (value)
                {
                    Base.Height = Height - 11;
                }
                else
                {
                    Height = Base.Height + 11;
                }
            }
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            if (Base != null)
            {
                Base.Text = value;
            }
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            if (Base != null)
            {
                Base.Font = value;
                Base.Location = new Point(5, 5);
                Base.Width = Width - 8;

                if (!_Multiline)
                {
                    Height = Base.Height + 11;
                }
            }
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        if (!Controls.Contains(Base))
        {
            Controls.Add(Base);
        }

        base.OnHandleCreated(e);
    }

    private TextBox Base;
    public NSTextBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, true);

        Cursor = Cursors.IBeam;

        Base = new TextBox();
        Base.Font = Font;
        Base.Text = Text;
        Base.MaxLength = _MaxLength;
        Base.Multiline = _Multiline;
        Base.ReadOnly = _ReadOnly;
        Base.UseSystemPasswordChar = _UseSystemPasswordChar;

        Base.ForeColor = Color.White;
        Base.BackColor = Color.FromArgb(50, 50, 50);

        Base.BorderStyle = BorderStyle.None;

        Base.Location = new Point(5, 5);
        Base.Width = Width - 14;

        if (_Multiline)
        {
            Base.Height = Height - 11;
        }
        else
        {
            Height = Base.Height + 11;
        }

        Base.TextChanged += OnBaseTextChanged;
        Base.KeyDown += OnBaseKeyDown;

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private Pen P1;
    private Pen P2;

    private PathGradientBrush PB1;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Graphics G = e.Graphics;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.9f, 0.5f);

        G.FillPath(PB1, GP1);

        G.DrawPath(P2, GP1);
        G.DrawPath(P1, GP2);
    }

    private void OnBaseTextChanged(object s, EventArgs e)
    {
        Text = Base.Text;
    }

    private void OnBaseKeyDown(object s, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.A)
        {
            Base.SelectAll();
            e.SuppressKeyPress = true;
        }
    }

    protected override void OnResize(EventArgs e)
    {
        Base.Location = new Point(5, 5);

        Base.Width = Width - 10;
        Base.Height = Height - 11;

        base.OnResize(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Base.Focus();
        base.OnMouseDown(e);
    }

    protected override void OnEnter(EventArgs e)
    {
        Base.Focus();
        Invalidate();
        base.OnEnter(e);
    }

    protected override void OnLeave(EventArgs e)
    {
        Invalidate();
        base.OnLeave(e);
    }

}

[DefaultEvent("CheckedChanged")]
class NSCheckBox : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public NSCheckBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P11 = new Pen(Color.FromArgb(55, 55, 55));
        P22 = new Pen(Color.FromArgb(35, 35, 35));
        P3 = new Pen(Color.Black, 2f);
        P4 = new Pen(Color.White, 2f);
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }

            Invalidate();
        }
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P11;
    private Pen P22;
    private Pen P3;

    private Pen P4;

    private PathGradientBrush PB1;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 2, Height - 5, Height - 5, 5);
        GP2 = ThemeModule.CreateRound(1, 3, Height - 7, Height - 7, 5);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.3f, 0.3f);

        G.FillPath(PB1, GP1);
        G.DrawPath(P11, GP1);
        G.DrawPath(P22, GP2);

        if (_Checked)
        {
            G.DrawLine(P3, 5, Height - 9, 8, Height - 7);
            G.DrawLine(P3, 7, Height - 7, Height - 8, 7);

            G.DrawLine(P4, 4, Height - 10, 7, Height - 8);
            G.DrawLine(P4, 6, Height - 8, Height - 9, 6);
        }

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(Height - 3, Height / 2 - SZ1.Height / 2);

        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = !Checked;
    }

}

[DefaultEvent("CheckedChanged")]
class NSRadioButton : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public NSRadioButton()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;

            if (_Checked)
            {
                InvalidateParent();
            }

            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }
            Invalidate();
        }
    }

    private void InvalidateParent()
    {
        if (Parent == null)
            return;

        foreach (Control C in Parent.Controls)
        {
            if ((!object.ReferenceEquals(C, this)) && (C is NSRadioButton))
            {
                ((NSRadioButton)C).Checked = false;
            }
        }
    }


    private GraphicsPath GP1;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;

    private Pen P2;

    private PathGradientBrush PB1;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = new GraphicsPath();
        GP1.AddEllipse(0, 2, Height - 5, Height - 5);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.3f, 0.3f);

        G.FillPath(PB1, GP1);

        G.DrawEllipse(P1, 0, 2, Height - 5, Height - 5);
        G.DrawEllipse(P2, 1, 3, Height - 7, Height - 7);

        if (_Checked)
        {
            G.FillEllipse(Brushes.Black, 6, 8, Height - 15, Height - 15);
            G.FillEllipse(Brushes.White, 5, 7, Height - 15, Height - 15);
        }

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(Height - 3, Height / 2 - SZ1.Height / 2);

        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = true;
        base.OnMouseDown(e);
    }

}

class NSComboBox : ComboBox
{

    public NSComboBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        DropDownStyle = ComboBoxStyle.DropDownList;

        BackColor = Color.FromArgb(50, 50, 50);
        ForeColor = Color.White;

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.White, 2f);
        P3 = new Pen(Brushes.Black, 2f);
        P4 = new Pen(Color.FromArgb(65, 65, 65));

        B1 = new SolidBrush(Color.FromArgb(65, 65, 65));
        B2 = new SolidBrush(Color.FromArgb(55, 55, 55));
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    private LinearGradientBrush GB1;
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        GB1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
        G.SetClip(GP1);
        G.FillRectangle(GB1, ClientRectangle);
        G.ResetClip();

        G.DrawPath(P1, GP1);
        G.DrawPath(P4, GP2);

        SZ1 = G.MeasureString(Text, Font);
        PT1 = new PointF(5, Height / 2 - SZ1.Height / 2);

        G.DrawString(Text, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(Text, Font, Brushes.White, PT1);

        G.DrawLine(P3, Width - 15, 10, Width - 11, 13);
        G.DrawLine(P3, Width - 7, 10, Width - 11, 13);
        G.DrawLine(Pens.Black, Width - 11, 13, Width - 11, 14);

        G.DrawLine(P2, Width - 16, 9, Width - 12, 12);
        G.DrawLine(P2, Width - 8, 9, Width - 12, 12);
        G.DrawLine(Pens.White, Width - 12, 12, Width - 12, 13);

        G.DrawLine(P1, Width - 22, 0, Width - 22, Height);
        G.DrawLine(P4, Width - 23, 1, Width - 23, Height - 2);
        G.DrawLine(P4, Width - 21, 1, Width - 21, Height - 2);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        {
            e.Graphics.FillRectangle(B1, e.Bounds);
        }
        else
        {
            e.Graphics.FillRectangle(B2, e.Bounds);
        }

        if (!(e.Index == -1))
        {
            ObjectCollection items = Items;
            e.Graphics.DrawString(GetItemText(items[e.Index]), e.Font, Brushes.White, e.Bounds);
        }
    }

}

class NSTabControl : TabControl
{

    public NSTabControl()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        SizeMode = TabSizeMode.Fixed;
        Alignment = TabAlignment.Left;
        ItemSize = new Size(28, 115);

        DrawMode = TabDrawMode.OwnerDrawFixed;

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));
        P3 = new Pen(Color.FromArgb(45, 45, 45), 2);

        B1 = new SolidBrush(Color.FromArgb(50, 50, 50));
        B2 = new SolidBrush(Color.FromArgb(35, 35, 35));
        B3 = new SolidBrush(Color.FromArgb(205, 150, 0));
        B4 = new SolidBrush(Color.FromArgb(65, 65, 65));

        SF1 = new StringFormat();
        SF1.LineAlignment = StringAlignment.Center;
    }

    protected override void OnControlAdded(ControlEventArgs e)
    {
        if (e.Control is TabPage)
        {
            e.Control.BackColor = Color.FromArgb(50, 50, 50);
        }

        base.OnControlAdded(e);
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Rectangle R1;

    private Rectangle R2;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;

    private SolidBrush B4;

    private PathGradientBrush PB1;
    private TabPage TP1;

    private StringFormat SF1;
    private int Offset;

    private int ItemHeight;
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(Color.FromArgb(50, 50, 50));
        G.SmoothingMode = SmoothingMode.AntiAlias;

        ItemHeight = ItemSize.Height + 2;

        GP1 = ThemeModule.CreateRound(0, 0, ItemHeight + 3, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, ItemHeight + 3, Height - 3, 7);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.8f, 0.95f);

        G.FillPath(PB1, GP1);

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        for (int I = 0; I <= TabCount - 1; I++)
        {
            R1 = GetTabRect(I);
            R1.Y += 2;
            R1.Height -= 3;
            R1.Width += 1;
            R1.X -= 1;

            TP1 = TabPages[I];
            Offset = 0;

            if (SelectedIndex == I)
            {
                G.FillRectangle(B1, R1);

                for (int J = 0; J <= 1; J++)
                {
                    G.FillRectangle(B2, R1.X + 5 + (J * 5), R1.Y + 6, 2, R1.Height - 9);

                    G.SmoothingMode = SmoothingMode.None;
                    G.FillRectangle(B3, R1.X + 5 + (J * 5), R1.Y + 5, 2, R1.Height - 9);
                    G.SmoothingMode = SmoothingMode.AntiAlias;

                    Offset += 5;
                }

                G.DrawRectangle(P3, R1.X + 1, R1.Y - 1, R1.Width, R1.Height + 2);
                G.DrawRectangle(P1, R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height - 2);
                G.DrawRectangle(P2, R1);
            }
            else
            {
                for (int J = 0; J <= 1; J++)
                {
                    G.FillRectangle(B2, R1.X + 5 + (J * 5), R1.Y + 6, 2, R1.Height - 9);

                    G.SmoothingMode = SmoothingMode.None;
                    G.FillRectangle(B4, R1.X + 5 + (J * 5), R1.Y + 5, 2, R1.Height - 9);
                    G.SmoothingMode = SmoothingMode.AntiAlias;

                    Offset += 5;
                }
            }

            R1.X += 5 + Offset;

            R2 = R1;
            R2.Y += 1;
            R2.X += 1;

            G.DrawString(TP1.Text, Font, Brushes.Black, R2, SF1);
            G.DrawString(TP1.Text, Font, Brushes.White, R1, SF1);
        }

        GP3 = ThemeModule.CreateRound(ItemHeight, 0, Width - ItemHeight - 1, Height - 1, 7);
        GP4 = ThemeModule.CreateRound(ItemHeight + 1, 1, Width - ItemHeight - 3, Height - 3, 7);

        G.DrawPath(P2, GP3);
        G.DrawPath(P1, GP4);
    }

}

[DefaultEvent("CheckedChanged")]
class NSOnOffBox : Control
{

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);

    public NSOnOffBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));
        P3 = new Pen(Color.FromArgb(65, 65, 65));

        B1 = new SolidBrush(Color.FromArgb(35, 35, 35));
        B2 = new SolidBrush(Color.FromArgb(85, 85, 85));
        B3 = new SolidBrush(Color.FromArgb(65, 65, 65));
        B4 = new SolidBrush(Color.FromArgb(205, 150, 0));
        B5 = new SolidBrush(Color.FromArgb(40, 40, 40));

        SF1 = new StringFormat();
        SF1.LineAlignment = StringAlignment.Center;
        SF1.Alignment = StringAlignment.Near;

        SF2 = new StringFormat();
        SF2.LineAlignment = StringAlignment.Center;
        SF2.Alignment = StringAlignment.Far;

        Size = new Size(56, 24);
        MinimumSize = Size;
        MaximumSize = Size;
    }

    private bool _Checked;
    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            if (CheckedChanged != null)
            {
                CheckedChanged(this);
            }

            Invalidate();
        }
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;
    private SolidBrush B2;
    private SolidBrush B3;
    private SolidBrush B4;

    private SolidBrush B5;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;
    private Rectangle R1;
    private Rectangle R2;
    private Rectangle R3;
    private StringFormat SF1;

    private StringFormat SF2;

    private int Offset;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.3f, 0.3f);

        G.FillPath(PB1, GP1);
        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        R1 = new Rectangle(5, 0, Width - 10, Height + 2);
        R2 = new Rectangle(6, 1, Width - 10, Height + 2);

        R3 = new Rectangle(1, 1, (Width / 2) - 1, Height - 3);

        if (_Checked)
        {
            G.DrawString("On", Font, Brushes.Black, R2, SF1);
            G.DrawString("On", Font, Brushes.White, R1, SF1);

            R3.X += (Width / 2) - 1;
        }
        else
        {
            G.DrawString("Off", Font, B1, R2, SF2);
            G.DrawString("Off", Font, B2, R1, SF2);
        }

        GP3 = ThemeModule.CreateRound(R3, 7);
        GP4 = ThemeModule.CreateRound(R3.X + 1, R3.Y + 1, R3.Width - 2, R3.Height - 2, 7);

        GB1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);

        G.FillPath(GB1, GP3);
        G.DrawPath(P2, GP3);
        G.DrawPath(P3, GP4);

        Offset = R3.X + (R3.Width / 2) - 3;

        for (int I = 0; I <= 1; I++)
        {
            if (_Checked)
            {
                G.FillRectangle(B1, Offset + (I * 5), 7, 2, Height - 14);
            }
            else
            {
                G.FillRectangle(B3, Offset + (I * 5), 7, 2, Height - 14);
            }

            G.SmoothingMode = SmoothingMode.None;

            if (_Checked)
            {
                G.FillRectangle(B4, Offset + (I * 5), 7, 2, Height - 14);
            }
            else
            {
                G.FillRectangle(B5, Offset + (I * 5), 7, 2, Height - 14);
            }

            G.SmoothingMode = SmoothingMode.AntiAlias;
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        Checked = !Checked;
        base.OnMouseDown(e);
    }

}

class NSControlButton : Control
{

    public enum Button : byte
    {
        None = 0,
        Minimize = 1,
        MaximizeRestore = 2,
        Close = 3
    }

    private Button _ControlButton = Button.Close;
    public Button ControlButton
    {
        get { return _ControlButton; }
        set
        {
            _ControlButton = value;
            Invalidate();
        }
    }

    public NSControlButton()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Anchor = AnchorStyles.Top | AnchorStyles.Right;

        Width = 18;
        Height = 20;

        MinimumSize = Size;
        MaximumSize = Size;

        Margin = new Padding(0);
    }

    Graphics G;
    protected override void OnPaint(PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        switch (_ControlButton)
        {
            case Button.Minimize:
                DrawMinimize(3, 10);
                break;
            case Button.MaximizeRestore:
                if (FindForm().WindowState == FormWindowState.Normal)
                {
                    DrawMaximize(3, 5);
                }
                else
                {
                    DrawRestore(3, 4);
                }
                break;
            case Button.Close:
                DrawClose(4, 5);
                break;
        }
    }

    private void DrawMinimize(int x, int y)
    {
        G.FillRectangle(Brushes.White, x, y, 12, 5);
        G.DrawRectangle(Pens.Black, x, y, 11, 4);
    }

    private void DrawMaximize(int x, int y)
    {
        G.DrawRectangle(new Pen(Color.White, 2), x + 2, y + 2, 8, 6);
        G.DrawRectangle(Pens.Black, x, y, 11, 9);
        G.DrawRectangle(Pens.Black, x + 3, y + 3, 5, 3);
    }

    private void DrawRestore(int x, int y)
    {
        G.FillRectangle(Brushes.White, x + 3, y + 1, 8, 4);
        G.FillRectangle(Brushes.White, x + 7, y + 5, 4, 4);
        G.DrawRectangle(Pens.Black, x + 2, y + 0, 9, 9);

        G.FillRectangle(Brushes.White, x + 1, y + 3, 2, 6);
        G.FillRectangle(Brushes.White, x + 1, y + 9, 8, 2);
        G.DrawRectangle(Pens.Black, x, y + 2, 9, 9);
        G.DrawRectangle(Pens.Black, x + 3, y + 5, 3, 3);
    }

    private GraphicsPath ClosePath;
    private void DrawClose(int x, int y)
    {
        if (ClosePath == null)
        {
            ClosePath = new GraphicsPath();
            ClosePath.AddLine(x + 1, y, x + 3, y);
            ClosePath.AddLine(x + 5, y + 2, x + 7, y);
            ClosePath.AddLine(x + 9, y, x + 10, y + 1);
            ClosePath.AddLine(x + 7, y + 4, x + 7, y + 5);
            ClosePath.AddLine(x + 10, y + 8, x + 9, y + 9);
            ClosePath.AddLine(x + 7, y + 9, x + 5, y + 7);
            ClosePath.AddLine(x + 3, y + 9, x + 1, y + 9);
            ClosePath.AddLine(x + 0, y + 8, x + 3, y + 5);
            ClosePath.AddLine(x + 3, y + 4, x + 0, y + 1);
        }

        G.FillPath(Brushes.White, ClosePath);
        G.DrawPath(Pens.Black, ClosePath);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {

        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            Form F = FindForm();

            switch (_ControlButton)
            {
                case Button.Minimize:
                    F.WindowState = FormWindowState.Minimized;
                    break;
                case Button.MaximizeRestore:
                    if (F.WindowState == FormWindowState.Normal)
                    {
                        F.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        F.WindowState = FormWindowState.Normal;
                    }
                    break;
                case Button.Close:
                    F.Close();
                    break;
            }

        }

        Invalidate();
        base.OnMouseClick(e);
    }

}

class NSGroupBox : ContainerControl
{

    private bool _DrawSeperator;
    public bool DrawSeperator
    {
        get { return _DrawSeperator; }
        set
        {
            _DrawSeperator = value;
            Invalidate();
        }
    }

    private string _Title = "GroupBox";
    public string Title
    {
        get { return _Title; }
        set
        {
            _Title = value;
            Invalidate();
        }
    }

    private string _SubTitle = "Details";
    public string SubTitle
    {
        get { return _SubTitle; }
        set
        {
            _SubTitle = value;
            Invalidate();
        }
    }

    private Font _TitleFont;

    private Font _SubTitleFont;
    public NSGroupBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        _TitleFont = new Font("Verdana", 10f);
        _SubTitleFont = new Font("Verdana", 6.5f);

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));

        B1 = new SolidBrush(Color.FromArgb(205, 150, 0));
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private PointF PT1;
    private SizeF SZ1;

    private SizeF SZ2;
    private Pen P1;
    private Pen P2;

    private SolidBrush B1;
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        SZ1 = G.MeasureString(_Title, _TitleFont, Width, StringFormat.GenericTypographic);
        SZ2 = G.MeasureString(_SubTitle, _SubTitleFont, Width, StringFormat.GenericTypographic);

        G.DrawString(_Title, _TitleFont, Brushes.Black, 6, 6);
        G.DrawString(_Title, _TitleFont, B1, 5, 5);

        PT1 = new PointF(6f, SZ1.Height + 4f);

        G.DrawString(_SubTitle, _SubTitleFont, Brushes.Black, PT1.X + 1, PT1.Y + 1);
        G.DrawString(_SubTitle, _SubTitleFont, Brushes.White, PT1.X, PT1.Y);

        if (_DrawSeperator)
        {
            int Y = Convert.ToInt32(PT1.Y + SZ2.Height) + 8;

            G.DrawLine(P1, 4, Y, Width - 5, Y);
            G.DrawLine(P2, 4, Y + 1, Width - 5, Y + 1);
        }
    }

}

class NSSeperator : Control
{

    public NSSeperator()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Height = 10;

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
    }

    private Pen P1;

    private Pen P2;
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;
        G.Clear(BackColor);

        G.DrawLine(P1, 0, 5, Width, 5);
        G.DrawLine(P2, 0, 6, Width, 6);
    }

}

[DefaultEvent("Scroll")]
class NSTrackBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;
            Invalidate();
        }
    }

    private int _Maximum = 10;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;
            Invalidate();
        }
    }

    private int _Value;
    public int Value
    {
        get { return _Value; }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            Invalidate();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    public NSTrackBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Height = 17;

        P1 = new Pen(Color.FromArgb(150, 110, 0), 2);
        P2 = new Pen(Color.FromArgb(55, 55, 55));
        P3 = new Pen(Color.FromArgb(35, 35, 35));
        P4 = new Pen(Color.FromArgb(65, 65, 65));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Rectangle R1;
    private Rectangle R2;
    private Rectangle R3;

    private int I1;
    private Pen P1;
    private Pen P2;
    private Pen P3;

    private Pen P4;
    private LinearGradientBrush GB1;
    private LinearGradientBrush GB2;

    private LinearGradientBrush GB3;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        Graphics G = e.Graphics;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 5, Width - 1, 10, 5);
        GP2 = ThemeModule.CreateRound(1, 6, Width - 3, 8, 5);

        R1 = new Rectangle(0, 7, Width - 1, 5);
        GB1 = new LinearGradientBrush(R1, Color.FromArgb(45, 45, 45), Color.FromArgb(50, 50, 50), 90f);

        I1 = Convert.ToInt32((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 11));
        R2 = new Rectangle(I1, 0, 10, 20);

        G.SetClip(GP2);
        G.FillRectangle(GB1, R1);

        R3 = new Rectangle(1, 7, R2.X + R2.Width - 2, 8);
        GB2 = new LinearGradientBrush(R3, Color.FromArgb(205, 150, 0), Color.FromArgb(150, 110, 0), 90f);

        G.SmoothingMode = SmoothingMode.None;
        G.FillRectangle(GB2, R3);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        for (int I = 0; I <= R3.Width - 15; I += 5)
        {
            G.DrawLine(P1, I, 0, I + 15, Height);
        }

        G.ResetClip();

        G.DrawPath(P2, GP1);
        G.DrawPath(P3, GP2);

        GP3 = ThemeModule.CreateRound(R2, 5);
        GP4 = ThemeModule.CreateRound(R2.X + 1, R2.Y + 1, R2.Width - 2, R2.Height - 2, 5);
        GB3 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);

        G.FillPath(GB3, GP3);
        G.DrawPath(P3, GP3);
        G.DrawPath(P4, GP4);
    }

    private bool TrackDown;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            I1 = Convert.ToInt32((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 11));
            R2 = new Rectangle(I1, 0, 10, 20);

            TrackDown = R2.Contains(e.Location);
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (TrackDown && e.X > -1 && e.X < (Width + 1))
        {
            Value = _Minimum + Convert.ToInt32((_Maximum - _Minimum) * (e.X / Width));
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        TrackDown = false;
        base.OnMouseUp(e);
    }

}

[DefaultEvent("ValueChanged")]
class NSRandomPool : Control
{

    public event ValueChangedEventHandler ValueChanged;
    public delegate void ValueChangedEventHandler(object sender);

    private StringBuilder _Value = new StringBuilder();
    public string Value
    {
        get { return _Value.ToString(); }
    }

    private string _FullValue;
    public string FullValue
    {
        get { return BitConverter.ToString(Table).Replace("-", ""); }
    }


    private Random RNG = new Random();
    private int ItemSize = 9;

    private int DrawSize = 8;

    private Rectangle WA;
    private int RowSize;

    private int ColumnSize;
    public NSRandomPool()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        P1 = new Pen(Color.FromArgb(55, 55, 55));
        P2 = new Pen(Color.FromArgb(35, 35, 35));

        B1 = new SolidBrush(Color.FromArgb(30, 30, 30));
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        UpdateTable();
        base.OnHandleCreated(e);
    }

    private byte[] Table;
    private void UpdateTable()
    {
        WA = new Rectangle(5, 5, Width - 10, Height - 10);

        RowSize = WA.Width / ItemSize;
        ColumnSize = WA.Height / ItemSize;

        WA.Width = RowSize * ItemSize;
        WA.Height = ColumnSize * ItemSize;

        WA.X = (Width / 2) - (WA.Width / 2);
        WA.Y = (Height / 2) - (WA.Height / 2);

        Table = new byte[(RowSize * ColumnSize)];

        for (int I = 0; I <= Table.Length - 1; I++)
        {
            Table[I] = Convert.ToByte(RNG.Next(100));
        }

        Invalidate();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        UpdateTable();
    }

    private int Index1 = -1;

    private int Index2;

    private bool InvertColors;
    protected override void OnMouseMove(MouseEventArgs e)
    {
        HandleDraw(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        HandleDraw(e);
        base.OnMouseDown(e);
    }

    private void HandleDraw(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right)
        {

            if (!WA.Contains(e.Location))
                return;

            InvertColors = (e.Button == System.Windows.Forms.MouseButtons.Right);

            Index1 = GetIndex(e.X, e.Y);
            if (Index1 == Index2)
                return;

            bool L = !(Index1 % RowSize == 0);
            bool R = !(Index1 % RowSize == (RowSize - 1));

            Randomize(Index1 - RowSize);

            if (L)
                Randomize(Index1 - 1);
            Randomize(Index1);
            if (R)
                Randomize(Index1 + 1);
            Randomize(Index1 + RowSize);

            _Value.Append(Table[Index1].ToString("X"));
            if (_Value.Length > 32)
                _Value.Remove(0, 2);

            if (ValueChanged != null)
            {
                ValueChanged(this);
            }

            Index2 = Index1;
            Invalidate();
        }
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;
    private Pen P1;
    private Pen P2;
    private SolidBrush B1;

    private SolidBrush B2;

    private PathGradientBrush PB1;
    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics G = e.Graphics;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        GP1 = ThemeModule.CreateRound(0, 0, Width - 1, Height - 1, 7);
        GP2 = ThemeModule.CreateRound(1, 1, Width - 3, Height - 3, 7);

        PB1 = new PathGradientBrush(GP1);
        PB1.CenterColor = Color.FromArgb(50, 50, 50);
        PB1.SurroundColors = new Color[] { Color.FromArgb(45, 45, 45) };
        PB1.FocusScales = new PointF(0.9f, 0.5f);

        G.FillPath(PB1, GP1);

        G.DrawPath(P1, GP1);
        G.DrawPath(P2, GP2);

        G.SmoothingMode = SmoothingMode.None;

        for (int I = 0; I <= Table.Length - 1; I++)
        {
            int C = Math.Max((int)Table[I], (int)75);

            int X = ((I % RowSize) * ItemSize) + WA.X;
            int Y = ((I / RowSize) * ItemSize) + WA.Y;

            B2 = new SolidBrush(Color.FromArgb(C, C, C));

            G.FillRectangle(B1, X + 1, Y + 1, DrawSize, DrawSize);
            G.FillRectangle(B2, X, Y, DrawSize, DrawSize);

            B2.Dispose();
        }

    }

    private int GetIndex(int x, int y)
    {
        return (((y - WA.Y) / ItemSize) * RowSize) + ((x - WA.X) / ItemSize);
    }

    private void Randomize(int index)
    {
        if (index > -1 && index < Table.Length)
        {
            if (InvertColors)
            {
                Table[index] = Convert.ToByte(RNG.Next(100));
            }
            else
            {
                Table[index] = Convert.ToByte(RNG.Next(100, 256));
            }
        }
    }
}

class NSKeyboard : Control
{

    private Bitmap TextBitmap;

    private Graphics TextGraphics;
    const string LowerKeys = "1234567890-=qwertyuiop[]asdfghjkl\\;'zxcvbnm,./`";

    const string UpperKeys = "!@#$%^&*()_+QWERTYUIOP{}ASDFGHJKL|:\"ZXCVBNM<>?~";
    public NSKeyboard()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Font = new Font("Verdana", 8.25f);

        TextBitmap = new Bitmap(1, 1);
        TextGraphics = Graphics.FromImage(TextBitmap);

        MinimumSize = new Size(386, 162);
        MaximumSize = new Size(386, 162);

        Lower = LowerKeys.ToCharArray();
        Upper = UpperKeys.ToCharArray();

        PrepareCache();

        P1 = new Pen(Color.FromArgb(45, 45, 45));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
        P3 = new Pen(Color.FromArgb(35, 35, 35));

        B1 = new SolidBrush(Color.FromArgb(100, 100, 100));
    }

    private Control _Target;
    public Control Target
    {
        get { return _Target; }
        set { _Target = value; }
    }


    private bool Shift;
    private int Pressed = -1;

    private Rectangle[] Buttons;
    private char[] Lower;
    private char[] Upper;
    private string[] Other = {
        "Shift",
        "Space",
        "Back"

    };
    private PointF[] UpperCache;

    private PointF[] LowerCache;
    private void PrepareCache()
    {
        Buttons = new Rectangle[51];
        UpperCache = new PointF[Upper.Length];
        LowerCache = new PointF[Lower.Length];

        int I = 0;

        SizeF S = default(SizeF);
        Rectangle R = default(Rectangle);

        for (int Y = 0; Y <= 3; Y++)
        {
            for (int X = 0; X <= 11; X++)
            {
                I = (Y * 12) + X;
                R = new Rectangle(X * 32, Y * 32, 32, 32);

                Buttons[I] = R;

                if (!(I == 47) && !char.IsLetter(Upper[I]))
                {
                    S = TextGraphics.MeasureString(Upper[I].ToString(), Font);
                    UpperCache[I] = new PointF(R.X + (R.Width / 2 - S.Width / 2), R.Y + R.Height - S.Height - 2);

                    S = TextGraphics.MeasureString(Lower[I].ToString(), Font);
                    LowerCache[I] = new PointF(R.X + (R.Width / 2 - S.Width / 2), R.Y + R.Height - S.Height - 2);
                }
            }
        }

        Buttons[48] = new Rectangle(0, 4 * 32, 2 * 32, 32);
        Buttons[49] = new Rectangle(Buttons[48].Right, 4 * 32, 8 * 32, 32);
        Buttons[50] = new Rectangle(Buttons[49].Right, 4 * 32, 2 * 32, 32);
    }


    private GraphicsPath GP1;
    private SizeF SZ1;

    private PointF PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;

    private SolidBrush B1;
    private PathGradientBrush PB1;

    private LinearGradientBrush GB1;

    Graphics G;

    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);

        Rectangle R = default(Rectangle);

        int Offset = 0;
        G.DrawRectangle(P1, 0, 0, (12 * 32) + 1, (5 * 32) + 1);

        for (int I = 0; I <= Buttons.Length - 1; I++)
        {
            R = Buttons[I];

            Offset = 0;
            if (I == Pressed)
            {
                Offset = 1;

                GP1 = new GraphicsPath();
                GP1.AddRectangle(R);

                PB1 = new PathGradientBrush(GP1);
                PB1.CenterColor = Color.FromArgb(60, 60, 60);
                PB1.SurroundColors = new Color[] { Color.FromArgb(55, 55, 55) };
                PB1.FocusScales = new PointF(0.8f, 0.5f);

                G.FillPath(PB1, GP1);
            }
            else
            {
                GB1 = new LinearGradientBrush(R, Color.FromArgb(60, 60, 60), Color.FromArgb(55, 55, 55), 90f);
                G.FillRectangle(GB1, R);
            }

            switch (I)
            {
                case 48:
                case 49:
                case 50:
                    SZ1 = G.MeasureString(Other[I - 48], Font);
                    G.DrawString(Other[I - 48], Font, Brushes.Black, R.X + (R.Width / 2 - SZ1.Width / 2) + Offset + 1, R.Y + (R.Height / 2 - SZ1.Height / 2) + Offset + 1);
                    G.DrawString(Other[I - 48], Font, Brushes.White, R.X + (R.Width / 2 - SZ1.Width / 2) + Offset, R.Y + (R.Height / 2 - SZ1.Height / 2) + Offset);
                    break;
                case 47:
                    DrawArrow(Color.Black, R.X + Offset + 1, R.Y + Offset + 1);
                    DrawArrow(Color.White, R.X + Offset, R.Y + Offset);
                    break;
                default:
                    if (Shift)
                    {
                        G.DrawString(Upper[I].ToString(), Font, Brushes.Black, R.X + 3 + Offset + 1, R.Y + 2 + Offset + 1);
                        G.DrawString(Upper[I].ToString(), Font, Brushes.White, R.X + 3 + Offset, R.Y + 2 + Offset);

                        if (!char.IsLetter(Lower[I]))
                        {
                            PT1 = LowerCache[I];
                            G.DrawString(Lower[I].ToString(), Font, B1, PT1.X + Offset, PT1.Y + Offset);
                        }
                    }
                    else
                    {
                        G.DrawString(Lower[I].ToString(), Font, Brushes.Black, R.X + 3 + Offset + 1, R.Y + 2 + Offset + 1);
                        G.DrawString(Lower[I].ToString(), Font, Brushes.White, R.X + 3 + Offset, R.Y + 2 + Offset);

                        if (!char.IsLetter(Upper[I]))
                        {
                            PT1 = UpperCache[I];
                            G.DrawString(Upper[I].ToString(), Font, B1, PT1.X + Offset, PT1.Y + Offset);
                        }
                    }
                    break;
            }

            G.DrawRectangle(P2, R.X + 1 + Offset, R.Y + 1 + Offset, R.Width - 2, R.Height - 2);
            G.DrawRectangle(P3, R.X + Offset, R.Y + Offset, R.Width, R.Height);

            if (I == Pressed)
            {
                G.DrawLine(P1, R.X, R.Y, R.Right, R.Y);
                G.DrawLine(P1, R.X, R.Y, R.X, R.Bottom);
            }
        }
    }

    private void DrawArrow(Color color, int rx, int ry)
    {
        Rectangle R = new Rectangle(rx + 8, ry + 8, 16, 16);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        Pen P = new Pen(color, 1);
        AdjustableArrowCap C = new AdjustableArrowCap(3, 2);
        P.CustomEndCap = C;

        G.DrawArc(P, R, 0f, 290f);

        P.Dispose();
        C.Dispose();
        G.SmoothingMode = SmoothingMode.None;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        int Index = ((e.Y / 32) * 12) + (e.X / 32);

        if (Index > 47)
        {
            for (int I = 48; I <= Buttons.Length - 1; I++)
            {
                if (Buttons[I].Contains(e.X, e.Y))
                {
                    Pressed = I;
                    break;
                }
            }
        }
        else
        {
            Pressed = Index;
        }

        HandleKey();
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        Pressed = -1;
        Invalidate();
    }

    private void HandleKey()
    {
        if (_Target == null)
            return;
        if (Pressed == -1)
            return;

        switch (Pressed)
        {
            case 47:
                _Target.Text = string.Empty;
                break;
            case 48:
                Shift = !Shift;
                break;
            case 49:
                _Target.Text += " ";
                break;
            case 50:
                if (!(_Target.Text.Length == 0))
                {
                    _Target.Text = _Target.Text.Remove(_Target.Text.Length - 1);
                }
                break;
            default:
                if (Shift)
                {
                    _Target.Text += Upper[Pressed];
                }
                else
                {
                    _Target.Text += Lower[Pressed];
                }
                break;
        }
    }

}

[DefaultEvent("SelectedIndexChanged")]
class NSPaginator : Control
{

    public event SelectedIndexChangedEventHandler SelectedIndexChanged;
    public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);

    private Bitmap TextBitmap;

    private Graphics TextGraphics;
    public NSPaginator()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Size = new Size(202, 26);

        TextBitmap = new Bitmap(1, 1);
        TextGraphics = Graphics.FromImage(TextBitmap);

        InvalidateItems();

        B1 = new SolidBrush(Color.FromArgb(50, 50, 50));
        B2 = new SolidBrush(Color.FromArgb(55, 55, 55));

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(55, 55, 55));
        P3 = new Pen(Color.FromArgb(65, 65, 65));
    }

    private int _SelectedIndex;
    public int SelectedIndex
    {
        get { return _SelectedIndex; }
        set
        {
            _SelectedIndex = Math.Max(Math.Min(value, MaximumIndex), 0);
            Invalidate();
        }
    }

    private int _NumberOfPages;
    public int NumberOfPages
    {
        get { return _NumberOfPages; }
        set
        {
            _NumberOfPages = value;
            _SelectedIndex = Math.Max(Math.Min(_SelectedIndex, MaximumIndex), 0);
            Invalidate();
        }
    }

    public int MaximumIndex
    {
        get { return NumberOfPages - 1; }
    }


    private int ItemWidth;
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;

            InvalidateItems();
            Invalidate();
        }
    }

    private void InvalidateItems()
    {
        Size S = TextGraphics.MeasureString("000 ..", Font).ToSize();
        ItemWidth = S.Width + 10;
    }

    private GraphicsPath GP1;

    private GraphicsPath GP2;

    private Rectangle R1;
    private Size SZ1;

    private Point PT1;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private SolidBrush B1;

    private SolidBrush B2;
    Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.AntiAlias;

        bool LeftEllipse = false;
        bool RightEllipse = false;

        if (_SelectedIndex < 4)
        {
            for (int I = 0; I <= Math.Min(MaximumIndex, 4); I++)
            {
                RightEllipse = (I == 4) && (MaximumIndex > 4);
                DrawBox(I * ItemWidth, I, false, RightEllipse);
            }
        }
        else if (_SelectedIndex > 3 && _SelectedIndex < (MaximumIndex - 3))
        {
            for (int I = 0; I <= 4; I++)
            {
                LeftEllipse = (I == 0);
                RightEllipse = (I == 4);
                DrawBox(I * ItemWidth, _SelectedIndex + I - 2, LeftEllipse, RightEllipse);
            }
        }
        else
        {
            for (int I = 0; I <= 4; I++)
            {
                LeftEllipse = (I == 0) && (MaximumIndex > 4);
                DrawBox(I * ItemWidth, MaximumIndex - (4 - I), LeftEllipse, false);
            }
        }
    }

    private void DrawBox(int x, int index, bool leftEllipse, bool rightEllipse)
    {
        R1 = new Rectangle(x, 0, ItemWidth - 4, Height - 1);

        GP1 = ThemeModule.CreateRound(R1, 7);
        GP2 = ThemeModule.CreateRound(R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height - 2, 7);

        string T = Convert.ToString(index + 1);

        if (leftEllipse)
            T = ".. " + T;
        if (rightEllipse)
            T = T + " ..";

        SZ1 = G.MeasureString(T, Font).ToSize();
        PT1 = new Point(R1.X + (R1.Width / 2 - SZ1.Width / 2), R1.Y + (R1.Height / 2 - SZ1.Height / 2));

        if (index == _SelectedIndex)
        {
            G.FillPath(B1, GP1);

            Font F = new Font(Font, FontStyle.Underline);
            G.DrawString(T, F, Brushes.Black, PT1.X + 1, PT1.Y + 1);
            G.DrawString(T, F, Brushes.White, PT1);
            F.Dispose();

            G.DrawPath(P1, GP2);
            G.DrawPath(P2, GP1);
        }
        else
        {
            G.FillPath(B2, GP1);

            G.DrawString(T, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
            G.DrawString(T, Font, Brushes.White, PT1);

            G.DrawPath(P3, GP2);
            G.DrawPath(P1, GP1);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            int NewIndex = 0;
            int OldIndex = _SelectedIndex;

            if (_SelectedIndex < 4)
            {
                NewIndex = (e.X / ItemWidth);
            }
            else if (_SelectedIndex > 3 && _SelectedIndex < (MaximumIndex - 3))
            {
                NewIndex = (e.X / ItemWidth);

                switch (NewIndex)
                {
                    case 2:
                        NewIndex = OldIndex;
                        break;
                    default:
                        if (NewIndex < 2)
                            NewIndex = OldIndex - (2 - NewIndex);
                        if (NewIndex > 2)
                            NewIndex = OldIndex + (NewIndex - 2);
                        break;
                }
            }
            else
            {
                NewIndex = MaximumIndex - (4 - (e.X / ItemWidth));
            }

            if ((NewIndex < _NumberOfPages) && (!(NewIndex == OldIndex)))
            {
                SelectedIndex = NewIndex;
                if (SelectedIndexChanged != null)
                {
                    SelectedIndexChanged(this, null);
                }
            }
        }

        base.OnMouseDown(e);
    }

}

[DefaultEvent("Scroll")]
class NSVScrollBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;

            InvalidateLayout();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;

            InvalidateLayout();
        }
    }

    private int _Value;
    public int Value
    {
        get
        {
            if (!ShowThumb)
                return _Minimum;
            return _Value;
        }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            InvalidatePosition();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    private int _SmallChange = 1;
    public int SmallChange
    {
        get { return _SmallChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _SmallChange = value;
        }
    }

    private int _LargeChange = 10;
    public int LargeChange
    {
        get { return _LargeChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _LargeChange = value;
        }
    }

    private int ButtonSize = 16;
    // 14 minimum
    private int ThumbSize = 24;

    private Rectangle TSA;
    private Rectangle BSA;
    private Rectangle Shaft;

    private Rectangle Thumb;
    private bool ShowThumb;

    private bool ThumbDown;
    public NSVScrollBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Width = 18;

        B1 = new SolidBrush(Color.FromArgb(55, 55, 55));
        B2 = new SolidBrush(Color.FromArgb(35, 35, 35));

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
        P3 = new Pen(Color.FromArgb(55, 55, 55));
        P4 = new Pen(Color.FromArgb(40, 40, 40));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    int I1;
    Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        GP1 = DrawArrow(4, 6, false);
        GP2 = DrawArrow(5, 7, false);

        G.FillPath(B1, GP2);
        G.FillPath(B2, GP1);

        GP3 = DrawArrow(4, Height - 11, true);
        GP4 = DrawArrow(5, Height - 10, true);

        G.FillPath(B1, GP4);
        G.FillPath(B2, GP3);

        if (ShowThumb)
        {
            G.FillRectangle(B1, Thumb);
            G.DrawRectangle(P1, Thumb);
            G.DrawRectangle(P2, Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2);

            int Y = 0;
            int LY = Thumb.Y + (Thumb.Height / 2) - 3;

            for (int I = 0; I <= 2; I++)
            {
                Y = LY + (I * 3);

                G.DrawLine(P1, Thumb.X + 5, Y, Thumb.Right - 5, Y);
                G.DrawLine(P2, Thumb.X + 5, Y + 1, Thumb.Right - 5, Y + 1);
            }
        }

        G.DrawRectangle(P3, 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(P4, 1, 1, Width - 3, Height - 3);
    }

    private GraphicsPath DrawArrow(int x, int y, bool flip)
    {
        GraphicsPath GP = new GraphicsPath();

        int W = 9;
        int H = 5;

        if (flip)
        {
            GP.AddLine(x + 1, y, x + W + 1, y);
            GP.AddLine(x + W, y, x + H, y + H - 1);
        }
        else
        {
            GP.AddLine(x, y + H, x + W, y + H);
            GP.AddLine(x + W, y + H, x + H, y);
        }

        GP.CloseFigure();
        return GP;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        InvalidateLayout();
    }

    private void InvalidateLayout()
    {
        TSA = new Rectangle(0, 0, Width, ButtonSize);
        BSA = new Rectangle(0, Height - ButtonSize, Width, ButtonSize);
        Shaft = new Rectangle(0, TSA.Bottom + 1, Width, Height - (ButtonSize * 2) - 1);

        ShowThumb = ((_Maximum - _Minimum) > Shaft.Height);

        if (ShowThumb)
        {
            //ThumbSize = Math.Max(0, 14) 'TODO: Implement this.
            Thumb = new Rectangle(1, 0, Width - 3, ThumbSize);
        }

        if (Scroll != null)
        {
            Scroll(this);
        }
        InvalidatePosition();
    }

    private void InvalidatePosition()
    {
        Thumb.Y = Convert.ToInt32(GetProgress() * (Shaft.Height - ThumbSize)) + TSA.Height;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left && ShowThumb)
        {
            if (TSA.Contains(e.Location))
            {
                I1 = _Value - _SmallChange;
            }
            else if (BSA.Contains(e.Location))
            {
                I1 = _Value + _SmallChange;
            }
            else
            {
                if (Thumb.Contains(e.Location))
                {
                    ThumbDown = true;
                    return;
                }
                else
                {
                    if (e.Y < Thumb.Y)
                    {
                        I1 = _Value - _LargeChange;
                    }
                    else
                    {
                        I1 = _Value + _LargeChange;
                    }
                }
            }

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (ThumbDown && ShowThumb)
        {
            int ThumbPosition = e.Y - TSA.Height - (ThumbSize / 2);
            int ThumbBounds = Shaft.Height - ThumbSize;

            I1 = Convert.ToInt32((ThumbPosition / ThumbBounds) * (_Maximum - _Minimum)) + _Minimum;

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        ThumbDown = false;
    }

    private double GetProgress()
    {
        return (_Value - _Minimum) / (_Maximum - _Minimum);
    }

}

[DefaultEvent("Scroll")]
class NSHScrollBar : Control
{

    public event ScrollEventHandler Scroll;
    public delegate void ScrollEventHandler(object sender);

    private int _Minimum;
    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Minimum = value;
            if (value > _Value)
                _Value = value;
            if (value > _Maximum)
                _Maximum = value;

            InvalidateLayout();
        }
    }

    private int _Maximum = 100;
    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }

            _Maximum = value;
            if (value < _Value)
                _Value = value;
            if (value < _Minimum)
                _Minimum = value;

            InvalidateLayout();
        }
    }

    private int _Value;
    public int Value
    {
        get
        {
            if (!ShowThumb)
                return _Minimum;
            return _Value;
        }
        set
        {
            if (value == _Value)
                return;

            if (value > _Maximum || value < _Minimum)
            {
                throw new Exception("Property value is not valid.");
            }

            _Value = value;
            InvalidatePosition();

            if (Scroll != null)
            {
                Scroll(this);
            }
        }
    }

    private int _SmallChange = 1;
    public int SmallChange
    {
        get { return _SmallChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _SmallChange = value;
        }
    }

    private int _LargeChange = 10;
    public int LargeChange
    {
        get { return _LargeChange; }
        set
        {
            if (value < 1)
            {
                throw new Exception("Property value is not valid.");
            }

            _LargeChange = value;
        }
    }

    private int ButtonSize = 16;
    // 14 minimum
    private int ThumbSize = 24;

    private Rectangle LSA;
    private Rectangle RSA;
    private Rectangle Shaft;

    private Rectangle Thumb;
    private bool ShowThumb;

    private bool ThumbDown;
    public NSHScrollBar()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);

        Height = 18;

        B1 = new SolidBrush(Color.FromArgb(55, 55, 55));
        B2 = new SolidBrush(Color.FromArgb(35, 35, 35));

        P1 = new Pen(Color.FromArgb(35, 35, 35));
        P2 = new Pen(Color.FromArgb(65, 65, 65));
        P3 = new Pen(Color.FromArgb(55, 55, 55));
        P4 = new Pen(Color.FromArgb(40, 40, 40));
    }

    private GraphicsPath GP1;
    private GraphicsPath GP2;
    private GraphicsPath GP3;

    private GraphicsPath GP4;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Pen P4;
    private SolidBrush B1;

    private SolidBrush B2;

    int I1;
    Graphics G;
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        G = e.Graphics;
        G.Clear(BackColor);

        GP1 = DrawArrow(6, 4, false);
        GP2 = DrawArrow(7, 5, false);

        G.FillPath(B1, GP2);
        G.FillPath(B2, GP1);

        GP3 = DrawArrow(Width - 11, 4, true);
        GP4 = DrawArrow(Width - 10, 5, true);

        G.FillPath(B1, GP4);
        G.FillPath(B2, GP3);

        if (ShowThumb)
        {
            G.FillRectangle(B1, Thumb);
            G.DrawRectangle(P1, Thumb);
            G.DrawRectangle(P2, Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2);

            int X = 0;
            int LX = Thumb.X + (Thumb.Width / 2) - 3;

            for (int I = 0; I <= 2; I++)
            {
                X = LX + (I * 3);

                G.DrawLine(P1, X, Thumb.Y + 5, X, Thumb.Bottom - 5);
                G.DrawLine(P2, X + 1, Thumb.Y + 5, X + 1, Thumb.Bottom - 5);
            }
        }

        G.DrawRectangle(P3, 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(P4, 1, 1, Width - 3, Height - 3);
    }

    private GraphicsPath DrawArrow(int x, int y, bool flip)
    {
        GraphicsPath GP = new GraphicsPath();

        int W = 5;
        int H = 9;

        if (flip)
        {
            GP.AddLine(x, y + 1, x, y + H + 1);
            GP.AddLine(x, y + H, x + W - 1, y + W);
        }
        else
        {
            GP.AddLine(x + W, y, x + W, y + H);
            GP.AddLine(x + W, y + H, x + 1, y + W);
        }

        GP.CloseFigure();
        return GP;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        InvalidateLayout();
    }

    private void InvalidateLayout()
    {
        LSA = new Rectangle(0, 0, ButtonSize, Height);
        RSA = new Rectangle(Width - ButtonSize, 0, ButtonSize, Height);
        Shaft = new Rectangle(LSA.Right + 1, 0, Width - (ButtonSize * 2) - 1, Height);

        ShowThumb = ((_Maximum - _Minimum) > Shaft.Width);

        if (ShowThumb)
        {
            //ThumbSize = Math.Max(0, 14) 'TODO: Implement this.
            Thumb = new Rectangle(0, 1, ThumbSize, Height - 3);
        }

        if (Scroll != null)
        {
            Scroll(this);
        }
        InvalidatePosition();
    }

    private void InvalidatePosition()
    {
        Thumb.X = Convert.ToInt32(GetProgress() * (Shaft.Width - ThumbSize)) + LSA.Width;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left && ShowThumb)
        {
            if (LSA.Contains(e.Location))
            {
                I1 = _Value - _SmallChange;
            }
            else if (RSA.Contains(e.Location))
            {
                I1 = _Value + _SmallChange;
            }
            else
            {
                if (Thumb.Contains(e.Location))
                {
                    ThumbDown = true;
                    return;
                }
                else
                {
                    if (e.X < Thumb.X)
                    {
                        I1 = _Value - _LargeChange;
                    }
                    else
                    {
                        I1 = _Value + _LargeChange;
                    }
                }
            }

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (ThumbDown && ShowThumb)
        {
            int ThumbPosition = e.X - LSA.Width - (ThumbSize / 2);
            int ThumbBounds = Shaft.Width - ThumbSize;

            I1 = Convert.ToInt32((ThumbPosition / ThumbBounds) * (_Maximum - _Minimum)) + _Minimum;

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum);
            InvalidatePosition();
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        ThumbDown = false;
    }

    private double GetProgress()
    {
        return (_Value - _Minimum) / (_Maximum - _Minimum);
    }

}

class NSContextMenu : ContextMenuStrip
{

    public NSContextMenu()
    {
        Renderer = new ToolStripProfessionalRenderer(new NSColorTable());
        ForeColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        base.OnPaint(e);
    }

}

class NSColorTable : ProfessionalColorTable
{


    private Color BackColor = Color.FromArgb(55, 55, 55);
    public override Color ButtonSelectedBorder
    {
        get { return BackColor; }
    }

    public override Color CheckBackground
    {
        get { return BackColor; }
    }

    public override Color CheckPressedBackground
    {
        get { return BackColor; }
    }

    public override Color CheckSelectedBackground
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientBegin
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientEnd
    {
        get { return BackColor; }
    }

    public override Color ImageMarginGradientMiddle
    {
        get { return BackColor; }
    }

    public override Color MenuBorder
    {
        get { return Color.FromArgb(25, 25, 25); }
    }

    public override Color MenuItemBorder
    {
        get { return BackColor; }
    }

    public override Color MenuItemSelected
    {
        get { return Color.FromArgb(65, 65, 65); }
    }

    public override Color SeparatorDark
    {
        get { return Color.FromArgb(35, 35, 35); }
    }

    public override Color ToolStripDropDownBackground
    {
        get { return BackColor; }
    }



    abstract class ThemeContainer154 : ContainerControl
    {

        #region " Initialization "

        protected Graphics G;

        protected Bitmap B;
        public ThemeContainer154()
        {
            SetStyle((ControlStyles)139270, true);

            _ImageSize = Size.Empty;
            Font = new Font("Verdana", 8);

            MeasureBitmap = new Bitmap(1, 1);
            MeasureGraphics = Graphics.FromImage(MeasureBitmap);

            DrawRadialPath = new GraphicsPath();

            InvalidateCustimization();
        }

        protected override sealed void OnHandleCreated(EventArgs e)
        {
            if (DoneCreation)
                InitializeMessages();

            InvalidateCustimization();
            ColorHook();

            if (!(_LockWidth == 0))
                Width = _LockWidth;
            if (!(_LockHeight == 0))
                Height = _LockHeight;
            if (!_ControlMode)
                base.Dock = DockStyle.Fill;

            Transparent = _Transparent;
            if (_Transparent && _BackColor)
                BackColor = Color.Transparent;

            base.OnHandleCreated(e);
        }

        private bool DoneCreation;
        protected override sealed void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (Parent == null)
                return;
            _IsParentForm = Parent is Form;

            if (!_ControlMode)
            {
                InitializeMessages();

                if (_IsParentForm)
                {
                    ParentForm.FormBorderStyle = _BorderStyle;
                    ParentForm.TransparencyKey = _TransparencyKey;

                    if (!DesignMode)
                    {
                        ParentForm.Shown += FormShown;
                    }
                }

                Parent.BackColor = BackColor;
            }

            OnCreation();
            DoneCreation = true;
            InvalidateTimer();
        }

        #endregion

        private void DoAnimation(bool i)
        {
            OnAnimation();
            if (i)
                Invalidate();
        }

        protected override sealed void OnPaint(PaintEventArgs e)
        {
            if (Width == 0 || Height == 0)
                return;

            if (_Transparent && _ControlMode)
            {
                PaintHook();
                e.Graphics.DrawImage(B, 0, 0);
            }
            else
            {
                G = e.Graphics;
                PaintHook();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
            base.OnHandleDestroyed(e);
        }

        private bool HasShown;
        private void FormShown(object sender, EventArgs e)
        {
            if (_ControlMode || HasShown)
                return;

            if (_StartPosition == FormStartPosition.CenterParent || _StartPosition == FormStartPosition.CenterScreen)
            {
                Rectangle SB = Screen.PrimaryScreen.Bounds;
                Rectangle CB = ParentForm.Bounds;
                ParentForm.Location = new Point(SB.Width / 2 - CB.Width / 2, SB.Height / 2 - CB.Width / 2);
            }

            HasShown = true;
        }


        #region " Size Handling "

        private Rectangle Frame;
        protected override sealed void OnSizeChanged(EventArgs e)
        {
            if (_Movable && !_ControlMode)
            {
                Frame = new Rectangle(7, 7, Width - 14, _Header - 7);
            }

            InvalidateBitmap();
            Invalidate();

            base.OnSizeChanged(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (!(_LockWidth == 0))
                width = _LockWidth;
            if (!(_LockHeight == 0))
                height = _LockHeight;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        #endregion

        #region " State Handling "

        protected MouseState State;
        private void SetState(MouseState current)
        {
            State = current;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
            {
                if (_Sizable && !_ControlMode)
                    InvalidateMouse();
            }

            base.OnMouseMove(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                SetState(MouseState.None);
            else
                SetState(MouseState.Block);
            base.OnEnabledChanged(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            SetState(MouseState.Over);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            SetState(MouseState.Over);
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetState(MouseState.None);

            if (GetChildAtPoint(PointToClient(MousePosition)) != null)
            {
                if (_Sizable && !_ControlMode)
                {
                    Cursor = Cursors.Default;
                    Previous = 0;
                }
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                SetState(MouseState.Down);

            if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
            {
                if (_Movable && Frame.Contains(e.Location))
                {
                    Capture = false;
                    WM_LMBUTTONDOWN = true;
                    DefWndProc(ref Messages[0]);
                }
                else if (_Sizable && !(Previous == 0))
                {
                    Capture = false;
                    WM_LMBUTTONDOWN = true;
                    DefWndProc(ref Messages[Previous]);
                }
            }

            base.OnMouseDown(e);
        }

        private bool WM_LMBUTTONDOWN;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (WM_LMBUTTONDOWN && m.Msg == 513)
            {
                WM_LMBUTTONDOWN = false;

                SetState(MouseState.Over);
                if (!_SmartBounds)
                    return;

                if (IsParentMdi)
                {
                    CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
                }
                else
                {
                    CorrectBounds(Screen.FromControl(Parent).WorkingArea);
                }
            }
        }

        private Point GetIndexPoint;
        private bool B1;
        private bool B2;
        private bool B3;
        private bool B4;
        private int GetIndex()
        {
            GetIndexPoint = PointToClient(MousePosition);
            B1 = GetIndexPoint.X < 7;
            B2 = GetIndexPoint.X > Width - 7;
            B3 = GetIndexPoint.Y < 7;
            B4 = GetIndexPoint.Y > Height - 7;

            if (B1 && B3)
                return 4;
            if (B1 && B4)
                return 7;
            if (B2 && B3)
                return 5;
            if (B2 && B4)
                return 8;
            if (B1)
                return 1;
            if (B2)
                return 2;
            if (B3)
                return 3;
            if (B4)
                return 6;
            return 0;
        }

        private int Current;
        private int Previous;
        private void InvalidateMouse()
        {
            Current = GetIndex();
            if (Current == Previous)
                return;

            Previous = Current;
            switch (Previous)
            {
                case 0:
                    Cursor = Cursors.Default;
                    break;
                case 1:
                case 2:
                    Cursor = Cursors.SizeWE;
                    break;
                case 3:
                case 6:
                    Cursor = Cursors.SizeNS;
                    break;
                case 4:
                case 8:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case 5:
                case 7:
                    Cursor = Cursors.SizeNESW;
                    break;
            }
        }

        private Message[] Messages = new Message[9];
        private void InitializeMessages()
        {
            Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
            for (int I = 1; I <= 8; I++)
            {
                Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
            }
        }

        private void CorrectBounds(Rectangle bounds)
        {
            if (Parent.Width > bounds.Width)
                Parent.Width = bounds.Width;
            if (Parent.Height > bounds.Height)
                Parent.Height = bounds.Height;

            int X = Parent.Location.X;
            int Y = Parent.Location.Y;

            if (X < bounds.X)
                X = bounds.X;
            if (Y < bounds.Y)
                Y = bounds.Y;

            int Width = bounds.X + bounds.Width;
            int Height = bounds.Y + bounds.Height;

            if (X + Parent.Width > Width)
                X = Width - Parent.Width;
            if (Y + Parent.Height > Height)
                Y = Height - Parent.Height;

            Parent.Location = new Point(X, Y);
        }

        #endregion


        #region " Base Properties "

        public override DockStyle Dock
        {
            get { return base.Dock; }
            set
            {
                if (!_ControlMode)
                    return;
                base.Dock = value;
            }
        }

        private bool _BackColor;
        [Category("Misc")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (value == base.BackColor)
                    return;

                if (!IsHandleCreated && _ControlMode && value == Color.Transparent)
                {
                    _BackColor = true;
                    return;
                }

                base.BackColor = value;
                if (Parent != null)
                {
                    if (!_ControlMode)
                        Parent.BackColor = value;
                    ColorHook();
                }
            }
        }

        public override Size MinimumSize
        {
            get { return base.MinimumSize; }
            set
            {
                base.MinimumSize = value;
                if (Parent != null)
                    Parent.MinimumSize = value;
            }
        }

        public override Size MaximumSize
        {
            get { return base.MaximumSize; }
            set
            {
                base.MaximumSize = value;
                if (Parent != null)
                    Parent.MaximumSize = value;
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                Invalidate();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor
        {
            get { return Color.Empty; }
            set { }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage
        {
            get { return null; }
            set { }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return ImageLayout.None; }
            set { }
        }

        #endregion

        #region " Public Properties "

        private bool _SmartBounds = true;
        public bool SmartBounds
        {
            get { return _SmartBounds; }
            set { _SmartBounds = value; }
        }

        private bool _Movable = true;
        public bool Movable
        {
            get { return _Movable; }
            set { _Movable = value; }
        }

        private bool _Sizable = true;
        public bool Sizable
        {
            get { return _Sizable; }
            set { _Sizable = value; }
        }

        private Color _TransparencyKey;
        public Color TransparencyKey
        {
            get
            {
                if (_IsParentForm && !_ControlMode)
                    return ParentForm.TransparencyKey;
                else
                    return _TransparencyKey;
            }
            set
            {
                if (value == _TransparencyKey)
                    return;
                _TransparencyKey = value;

                if (_IsParentForm && !_ControlMode)
                {
                    ParentForm.TransparencyKey = value;
                    ColorHook();
                }
            }
        }

        private FormBorderStyle _BorderStyle;
        public FormBorderStyle BorderStyle
        {
            get
            {
                if (_IsParentForm && !_ControlMode)
                    return ParentForm.FormBorderStyle;
                else
                    return _BorderStyle;
            }
            set
            {
                _BorderStyle = value;

                if (_IsParentForm && !_ControlMode)
                {
                    ParentForm.FormBorderStyle = value;

                    if (!(value == FormBorderStyle.None))
                    {
                        Movable = false;
                        Sizable = false;
                    }
                }
            }
        }

        private FormStartPosition _StartPosition;
        public FormStartPosition StartPosition
        {
            get
            {
                if (_IsParentForm && !_ControlMode)
                    return ParentForm.StartPosition;
                else
                    return _StartPosition;
            }
            set
            {
                _StartPosition = value;

                if (_IsParentForm && !_ControlMode)
                {
                    ParentForm.StartPosition = value;
                }
            }
        }

        private bool _NoRounding;
        public bool NoRounding
        {
            get { return _NoRounding; }
            set
            {
                _NoRounding = value;
                Invalidate();
            }
        }

        private Image _Image;
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (value == null)
                    _ImageSize = Size.Empty;
                else
                    _ImageSize = value.Size;

                _Image = value;
                Invalidate();
            }
        }

        private Dictionary<string, Color> Items = new Dictionary<string, Color>();
        public Bloom[] Colors
        {
            get
            {
                List<Bloom> T = new List<Bloom>();
                Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

                while (E.MoveNext())
                {
                    T.Add(new Bloom(E.Current.Key, E.Current.Value));
                }

                return T.ToArray();
            }
            set
            {
                foreach (Bloom B in value)
                {
                    if (Items.ContainsKey(B.Name))
                        Items[B.Name] = B.Value;
                }

                InvalidateCustimization();
                ColorHook();
                Invalidate();
            }
        }

        private string _Customization;
        public string Customization
        {
            get { return _Customization; }
            set
            {
                if (value == _Customization)
                    return;

                byte[] Data = null;
                Bloom[] Items = Colors;

                try
                {
                    Data = Convert.FromBase64String(value);
                    for (int I = 0; I <= Items.Length - 1; I++)
                    {
                        Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                    }
                }
                catch
                {
                    return;
                }

                _Customization = value;

                Colors = Items;
                ColorHook();
                Invalidate();
            }
        }

        private bool _Transparent;
        public bool Transparent
        {
            get { return _Transparent; }
            set
            {
                _Transparent = value;
                if (!(IsHandleCreated || _ControlMode))
                    return;

                if (!value && !(BackColor.A == 255))
                {
                    throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
                }

                SetStyle(ControlStyles.Opaque, !value);
                SetStyle(ControlStyles.SupportsTransparentBackColor, value);

                InvalidateBitmap();
                Invalidate();
            }
        }

        #endregion

        #region " Private Properties "

        private Size _ImageSize;
        protected Size ImageSize
        {
            get { return _ImageSize; }
        }

        private bool _IsParentForm;
        protected bool IsParentForm
        {
            get { return _IsParentForm; }
        }

        protected bool IsParentMdi
        {
            get
            {
                if (Parent == null)
                    return false;
                return Parent.Parent != null;
            }
        }

        private int _LockWidth;
        protected int LockWidth
        {
            get { return _LockWidth; }
            set
            {
                _LockWidth = value;
                if (!(LockWidth == 0) && IsHandleCreated)
                    Width = LockWidth;
            }
        }

        private int _LockHeight;
        protected int LockHeight
        {
            get { return _LockHeight; }
            set
            {
                _LockHeight = value;
                if (!(LockHeight == 0) && IsHandleCreated)
                    Height = LockHeight;
            }
        }

        private int _Header = 24;
        protected int Header
        {
            get { return _Header; }
            set
            {
                _Header = value;

                if (!_ControlMode)
                {
                    Frame = new Rectangle(7, 7, Width - 14, value - 7);
                    Invalidate();
                }
            }
        }

        private bool _ControlMode;
        protected bool ControlMode
        {
            get { return _ControlMode; }
            set
            {
                _ControlMode = value;

                Transparent = _Transparent;
                if (_Transparent && _BackColor)
                    BackColor = Color.Transparent;

                InvalidateBitmap();
                Invalidate();
            }
        }

        private bool _IsAnimated;
        protected bool IsAnimated
        {
            get { return _IsAnimated; }
            set
            {
                _IsAnimated = value;
                InvalidateTimer();
            }
        }

        #endregion


        #region " Property Helpers "

        protected Pen GetPen(string name)
        {
            return new Pen(Items[name]);
        }
        protected Pen GetPen(string name, float width)
        {
            return new Pen(Items[name], width);
        }

        protected SolidBrush GetBrush(string name)
        {
            return new SolidBrush(Items[name]);
        }

        protected Color GetColor(string name)
        {
            return Items[name];
        }

        protected void SetColor(string name, Color value)
        {
            if (Items.ContainsKey(name))
                Items[name] = value;
            else
                Items.Add(name, value);
        }
        protected void SetColor(string name, byte r, byte g, byte b)
        {
            SetColor(name, Color.FromArgb(r, g, b));
        }
        protected void SetColor(string name, byte a, byte r, byte g, byte b)
        {
            SetColor(name, Color.FromArgb(a, r, g, b));
        }
        protected void SetColor(string name, byte a, Color value)
        {
            SetColor(name, Color.FromArgb(a, value));
        }

        private void InvalidateBitmap()
        {
            if (_Transparent && _ControlMode)
            {
                if (Width == 0 || Height == 0)
                    return;
                B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
                G = Graphics.FromImage(B);
            }
            else
            {
                G = null;
                B = null;
            }
        }

        private void InvalidateCustimization()
        {
            MemoryStream M = new MemoryStream(Items.Count * 4);

            foreach (Bloom B in Colors)
            {
                M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
            }

            M.Close();
            _Customization = Convert.ToBase64String(M.ToArray());
        }

        private void InvalidateTimer()
        {
            if (DesignMode || !DoneCreation)
                return;

            if (_IsAnimated)
            {
                ThemeShare.AddAnimationCallback(DoAnimation);
            }
            else
            {
                ThemeShare.RemoveAnimationCallback(DoAnimation);
            }
        }

        #endregion


        #region " User Hooks "

        protected abstract void ColorHook();
        protected abstract void PaintHook();

        protected virtual void OnCreation()
        {
        }

        protected virtual void OnAnimation()
        {
        }

        #endregion


        #region " Offset "

        private Rectangle OffsetReturnRectangle;
        protected Rectangle Offset(Rectangle r, int amount)
        {
            OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
            return OffsetReturnRectangle;
        }

        private Size OffsetReturnSize;
        protected Size Offset(Size s, int amount)
        {
            OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
            return OffsetReturnSize;
        }

        private Point OffsetReturnPoint;
        protected Point Offset(Point p, int amount)
        {
            OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
            return OffsetReturnPoint;
        }

        #endregion

        #region " Center "


        private Point CenterReturn;
        protected Point Center(Rectangle p, Rectangle c)
        {
            CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
            return CenterReturn;
        }
        protected Point Center(Rectangle p, Size c)
        {
            CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
            return CenterReturn;
        }

        protected Point Center(Rectangle child)
        {
            return Center(Width, Height, child.Width, child.Height);
        }
        protected Point Center(Size child)
        {
            return Center(Width, Height, child.Width, child.Height);
        }
        protected Point Center(int childWidth, int childHeight)
        {
            return Center(Width, Height, childWidth, childHeight);
        }

        protected Point Center(Size p, Size c)
        {
            return Center(p.Width, p.Height, c.Width, c.Height);
        }

        protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
        {
            CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
            return CenterReturn;
        }

        #endregion

        #region " Measure "

        private Bitmap MeasureBitmap;

        private Graphics MeasureGraphics;
        protected Size Measure()
        {
            lock (MeasureGraphics)
            {
                return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
            }
        }
        protected Size Measure(string text)
        {
            lock (MeasureGraphics)
            {
                return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
            }
        }

        #endregion


        #region " DrawPixel "


        private SolidBrush DrawPixelBrush;
        protected void DrawPixel(Color c1, int x, int y)
        {
            if (_Transparent)
            {
                B.SetPixel(x, y, c1);
            }
            else
            {
                DrawPixelBrush = new SolidBrush(c1);
                G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
            }
        }

        #endregion

        #region " DrawCorners "


        private SolidBrush DrawCornersBrush;
        protected void DrawCorners(Color c1, int offset)
        {
            DrawCorners(c1, 0, 0, Width, Height, offset);
        }
        protected void DrawCorners(Color c1, Rectangle r1, int offset)
        {
            DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
        }
        protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
        {
            DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
        }

        protected void DrawCorners(Color c1)
        {
            DrawCorners(c1, 0, 0, Width, Height);
        }
        protected void DrawCorners(Color c1, Rectangle r1)
        {
            DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
        }
        protected void DrawCorners(Color c1, int x, int y, int width, int height)
        {
            if (_NoRounding)
                return;

            if (_Transparent)
            {
                B.SetPixel(x, y, c1);
                B.SetPixel(x + (width - 1), y, c1);
                B.SetPixel(x, y + (height - 1), c1);
                B.SetPixel(x + (width - 1), y + (height - 1), c1);
            }
            else
            {
                DrawCornersBrush = new SolidBrush(c1);
                G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
                G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
                G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
                G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
            }
        }

        #endregion

        #region " DrawBorders "

        protected void DrawBorders(Pen p1, int offset)
        {
            DrawBorders(p1, 0, 0, Width, Height, offset);
        }
        protected void DrawBorders(Pen p1, Rectangle r, int offset)
        {
            DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
        {
            DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
        }

        protected void DrawBorders(Pen p1)
        {
            DrawBorders(p1, 0, 0, Width, Height);
        }
        protected void DrawBorders(Pen p1, Rectangle r)
        {
            DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height)
        {
            G.DrawRectangle(p1, x, y, width - 1, height - 1);
        }

        #endregion

        #region " DrawText "

        private Point DrawTextPoint;

        private Size DrawTextSize;
        protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
        {
            DrawText(b1, Text, a, x, y);
        }
        protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
        {
            if (text.Length == 0)
                return;

            DrawTextSize = Measure(text);
            DrawTextPoint = new Point(Width / 2 - DrawTextSize.Width / 2, Header / 2 - DrawTextSize.Height / 2);

            switch (a)
            {
                case HorizontalAlignment.Left:
                    G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                    break;
                case HorizontalAlignment.Center:
                    G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                    break;
                case HorizontalAlignment.Right:
                    G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                    break;
            }
        }

        protected void DrawText(Brush b1, Point p1)
        {
            if (Text.Length == 0)
                return;
            G.DrawString(Text, Font, b1, p1);
        }
        protected void DrawText(Brush b1, int x, int y)
        {
            if (Text.Length == 0)
                return;
            G.DrawString(Text, Font, b1, x, y);
        }

        #endregion

        #region " DrawImage "


        private Point DrawImagePoint;
        protected void DrawImage(HorizontalAlignment a, int x, int y)
        {
            DrawImage(_Image, a, x, y);
        }
        protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
        {
            if (image == null)
                return;
            DrawImagePoint = new Point(Width / 2 - image.Width / 2, Header / 2 - image.Height / 2);

            switch (a)
            {
                case HorizontalAlignment.Left:
                    G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                    break;
                case HorizontalAlignment.Center:
                    G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                    break;
                case HorizontalAlignment.Right:
                    G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                    break;
            }
        }

        protected void DrawImage(Point p1)
        {
            DrawImage(_Image, p1.X, p1.Y);
        }
        protected void DrawImage(int x, int y)
        {
            DrawImage(_Image, x, y);
        }

        protected void DrawImage(Image image, Point p1)
        {
            DrawImage(image, p1.X, p1.Y);
        }
        protected void DrawImage(Image image, int x, int y)
        {
            if (image == null)
                return;
            G.DrawImage(image, x, y, image.Width, image.Height);
        }

        #endregion

        #region " DrawGradient "

        private LinearGradientBrush DrawGradientBrush;

        private Rectangle DrawGradientRectangle;
        protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
        {
            DrawGradientRectangle = new Rectangle(x, y, width, height);
            DrawGradient(blend, DrawGradientRectangle);
        }
        protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
        {
            DrawGradientRectangle = new Rectangle(x, y, width, height);
            DrawGradient(blend, DrawGradientRectangle, angle);
        }

        protected void DrawGradient(ColorBlend blend, Rectangle r)
        {
            DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
            DrawGradientBrush.InterpolationColors = blend;
            G.FillRectangle(DrawGradientBrush, r);
        }
        protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
        {
            DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
            DrawGradientBrush.InterpolationColors = blend;
            G.FillRectangle(DrawGradientBrush, r);
        }


        protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
        {
            DrawGradientRectangle = new Rectangle(x, y, width, height);
            DrawGradient(c1, c2, DrawGradientRectangle);
        }
        protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
        {
            DrawGradientRectangle = new Rectangle(x, y, width, height);
            DrawGradient(c1, c2, DrawGradientRectangle, angle);
        }

        protected void DrawGradient(Color c1, Color c2, Rectangle r)
        {
            DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
            G.FillRectangle(DrawGradientBrush, r);
        }
        protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
        {
            DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
            G.FillRectangle(DrawGradientBrush, r);
        }

        #endregion

        #region " DrawRadial "

        private GraphicsPath DrawRadialPath;
        private PathGradientBrush DrawRadialBrush1;
        private LinearGradientBrush DrawRadialBrush2;

        private Rectangle DrawRadialRectangle;
        public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
        }
        public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
        }
        public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(blend, DrawRadialRectangle, cx, cy);
        }

        public void DrawRadial(ColorBlend blend, Rectangle r)
        {
            DrawRadial(blend, r, r.Width / 2, r.Height / 2);
        }
        public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
        {
            DrawRadial(blend, r, center.X, center.Y);
        }
        public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
        {
            DrawRadialPath.Reset();
            DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

            DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
            DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
            DrawRadialBrush1.InterpolationColors = blend;

            if (G.SmoothingMode == SmoothingMode.AntiAlias)
            {
                G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
            }
            else
            {
                G.FillEllipse(DrawRadialBrush1, r);
            }
        }


        protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(c1, c2, DrawGradientRectangle);
        }
        protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(c1, c2, DrawGradientRectangle, angle);
        }

        protected void DrawRadial(Color c1, Color c2, Rectangle r)
        {
            DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
            G.FillRectangle(DrawGradientBrush, r);
        }
        protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
        {
            DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
            G.FillEllipse(DrawGradientBrush, r);
        }

        #endregion

        #region " CreateRound "

        private GraphicsPath CreateRoundPath;

        private Rectangle CreateRoundRectangle;
        public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
        {
            CreateRoundRectangle = new Rectangle(x, y, width, height);
            return CreateRound(CreateRoundRectangle, slope);
        }

        public GraphicsPath CreateRound(Rectangle r, int slope)
        {
            CreateRoundPath = new GraphicsPath(FillMode.Winding);
            CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
            CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
            CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
            CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
            CreateRoundPath.CloseFigure();
            return CreateRoundPath;
        }

        #endregion

    }

    abstract class ThemeControl154 : Control
    {


        #region " Initialization "

        protected Graphics G;

        protected Bitmap B;
        public ThemeControl154()
        {
            SetStyle((ControlStyles)139270, true);

            _ImageSize = Size.Empty;
            Font = new Font("Verdana", 8);

            MeasureBitmap = new Bitmap(1, 1);
            MeasureGraphics = Graphics.FromImage(MeasureBitmap);

            DrawRadialPath = new GraphicsPath();

            InvalidateCustimization();
            //Remove?
        }

        protected override sealed void OnHandleCreated(EventArgs e)
        {
            InvalidateCustimization();
            ColorHook();

            if (!(_LockWidth == 0))
                Width = _LockWidth;
            if (!(_LockHeight == 0))
                Height = _LockHeight;

            Transparent = _Transparent;
            if (_Transparent && _BackColor)
                BackColor = Color.Transparent;

            base.OnHandleCreated(e);
        }

        private bool DoneCreation;
        protected override sealed void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
            {
                OnCreation();
                DoneCreation = true;
                InvalidateTimer();
            }

            base.OnParentChanged(e);
        }

        #endregion

        private void DoAnimation(bool i)
        {
            OnAnimation();
            if (i)
                Invalidate();
        }

        protected override sealed void OnPaint(PaintEventArgs e)
        {
            if (Width == 0 || Height == 0)
                return;

            if (_Transparent)
            {
                PaintHook();
                e.Graphics.DrawImage(B, 0, 0);
            }
            else
            {
                G = e.Graphics;
                PaintHook();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
            base.OnHandleDestroyed(e);
        }

        #region " Size Handling "

        protected override sealed void OnSizeChanged(EventArgs e)
        {
            if (_Transparent)
            {
                InvalidateBitmap();
            }

            Invalidate();
            base.OnSizeChanged(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (!(_LockWidth == 0))
                width = _LockWidth;
            if (!(_LockHeight == 0))
                height = _LockHeight;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        #endregion

        #region " State Handling "

        private bool InPosition;
        protected override void OnMouseEnter(EventArgs e)
        {
            InPosition = true;
            SetState(MouseState.Over);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (InPosition)
                SetState(MouseState.Over);
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                SetState(MouseState.Down);
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            InPosition = false;
            SetState(MouseState.None);
            base.OnMouseLeave(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                SetState(MouseState.None);
            else
                SetState(MouseState.Block);
            base.OnEnabledChanged(e);
        }

        protected MouseState State;
        private void SetState(MouseState current)
        {
            State = current;
            Invalidate();
        }

        #endregion


        #region " Base Properties "

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color ForeColor
        {
            get { return Color.Empty; }
            set { }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage
        {
            get { return null; }
            set { }
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return ImageLayout.None; }
            set { }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                Invalidate();
            }
        }

        private bool _BackColor;
        [Category("Misc")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (!IsHandleCreated && value == Color.Transparent)
                {
                    _BackColor = true;
                    return;
                }

                base.BackColor = value;
                if (Parent != null)
                    ColorHook();
            }
        }

        #endregion

        #region " Public Properties "

        private bool _NoRounding;
        public bool NoRounding
        {
            get { return _NoRounding; }
            set
            {
                _NoRounding = value;
                Invalidate();
            }
        }

        private Image _Image;
        public Image Image
        {
            get { return _Image; }
            set
            {
                if (value == null)
                {
                    _ImageSize = Size.Empty;
                }
                else
                {
                    _ImageSize = value.Size;
                }

                _Image = value;
                Invalidate();
            }
        }

        private bool _Transparent;
        public bool Transparent
        {
            get { return _Transparent; }
            set
            {
                _Transparent = value;
                if (!IsHandleCreated)
                    return;

                if (!value && !(BackColor.A == 255))
                {
                    throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
                }

                SetStyle(ControlStyles.Opaque, !value);
                SetStyle(ControlStyles.SupportsTransparentBackColor, value);

                if (value)
                    InvalidateBitmap();
                else
                    B = null;
                Invalidate();
            }
        }

        private Dictionary<string, Color> Items = new Dictionary<string, Color>();
        public Bloom[] Colors
        {
            get
            {
                List<Bloom> T = new List<Bloom>();
                Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

                while (E.MoveNext())
                {
                    T.Add(new Bloom(E.Current.Key, E.Current.Value));
                }

                return T.ToArray();
            }
            set
            {
                foreach (Bloom B in value)
                {
                    if (Items.ContainsKey(B.Name))
                        Items[B.Name] = B.Value;
                }

                InvalidateCustimization();
                ColorHook();
                Invalidate();
            }
        }

        private string _Customization;
        public string Customization
        {
            get { return _Customization; }
            set
            {
                if (value == _Customization)
                    return;

                byte[] Data = null;
                Bloom[] Items = Colors;

                try
                {
                    Data = Convert.FromBase64String(value);
                    for (int I = 0; I <= Items.Length - 1; I++)
                    {
                        Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                    }
                }
                catch
                {
                    return;
                }

                _Customization = value;

                Colors = Items;
                ColorHook();
                Invalidate();
            }
        }

        #endregion

        #region " Private Properties "

        private Size _ImageSize;
        protected Size ImageSize
        {
            get { return _ImageSize; }
        }

        private int _LockWidth;
        protected int LockWidth
        {
            get { return _LockWidth; }
            set
            {
                _LockWidth = value;
                if (!(LockWidth == 0) && IsHandleCreated)
                    Width = LockWidth;
            }
        }

        private int _LockHeight;
        protected int LockHeight
        {
            get { return _LockHeight; }
            set
            {
                _LockHeight = value;
                if (!(LockHeight == 0) && IsHandleCreated)
                    Height = LockHeight;
            }
        }

        private bool _IsAnimated;
        protected bool IsAnimated
        {
            get { return _IsAnimated; }
            set
            {
                _IsAnimated = value;
                InvalidateTimer();
            }
        }

        #endregion


        #region " Property Helpers "

        protected Pen GetPen(string name)
        {
            return new Pen(Items[name]);
        }
        protected Pen GetPen(string name, float width)
        {
            return new Pen(Items[name], width);
        }

        protected SolidBrush GetBrush(string name)
        {
            return new SolidBrush(Items[name]);
        }

        protected Color GetColor(string name)
        {
            return Items[name];
        }

        protected void SetColor(string name, Color value)
        {
            if (Items.ContainsKey(name))
                Items[name] = value;
            else
                Items.Add(name, value);
        }
        protected void SetColor(string name, byte r, byte g, byte b)
        {
            SetColor(name, Color.FromArgb(r, g, b));
        }
        protected void SetColor(string name, byte a, byte r, byte g, byte b)
        {
            SetColor(name, Color.FromArgb(a, r, g, b));
        }
        protected void SetColor(string name, byte a, Color value)
        {
            SetColor(name, Color.FromArgb(a, value));
        }

        private void InvalidateBitmap()
        {
            if (Width == 0 || Height == 0)
                return;
            B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            G = Graphics.FromImage(B);
        }

        private void InvalidateCustimization()
        {
            MemoryStream M = new MemoryStream(Items.Count * 4);

            foreach (Bloom B in Colors)
            {
                M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
            }

            M.Close();
            _Customization = Convert.ToBase64String(M.ToArray());
        }

        private void InvalidateTimer()
        {
            if (DesignMode || !DoneCreation)
                return;

            if (_IsAnimated)
            {
                ThemeShare.AddAnimationCallback(DoAnimation);
            }
            else
            {
                ThemeShare.RemoveAnimationCallback(DoAnimation);
            }
        }
        #endregion


        #region " User Hooks "

        protected abstract void ColorHook();
        protected abstract void PaintHook();

        protected virtual void OnCreation()
        {
        }

        protected virtual void OnAnimation()
        {
        }

        #endregion


        #region " Offset "

        private Rectangle OffsetReturnRectangle;
        protected Rectangle Offset(Rectangle r, int amount)
        {
            OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
            return OffsetReturnRectangle;
        }

        private Size OffsetReturnSize;
        protected Size Offset(Size s, int amount)
        {
            OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
            return OffsetReturnSize;
        }

        private Point OffsetReturnPoint;
        protected Point Offset(Point p, int amount)
        {
            OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
            return OffsetReturnPoint;
        }

        #endregion

        #region " Center "


        private Point CenterReturn;
        protected Point Center(Rectangle p, Rectangle c)
        {
            CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
            return CenterReturn;
        }
        protected Point Center(Rectangle p, Size c)
        {
            CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
            return CenterReturn;
        }

        protected Point Center(Rectangle child)
        {
            return Center(Width, Height, child.Width, child.Height);
        }
        protected Point Center(Size child)
        {
            return Center(Width, Height, child.Width, child.Height);
        }
        protected Point Center(int childWidth, int childHeight)
        {
            return Center(Width, Height, childWidth, childHeight);
        }

        protected Point Center(Size p, Size c)
        {
            return Center(p.Width, p.Height, c.Width, c.Height);
        }

        protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
        {
            CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
            return CenterReturn;
        }

        #endregion

        #region " Measure "

        private Bitmap MeasureBitmap;
        //TODO: Potential issues during multi-threading.
        private Graphics MeasureGraphics;

        protected Size Measure()
        {
            return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
        }
        protected Size Measure(string text)
        {
            return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
        }

        #endregion


        #region " DrawPixel "


        private SolidBrush DrawPixelBrush;
        protected void DrawPixel(Color c1, int x, int y)
        {
            if (_Transparent)
            {
                B.SetPixel(x, y, c1);
            }
            else
            {
                DrawPixelBrush = new SolidBrush(c1);
                G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
            }
        }

        #endregion

        #region " DrawCorners "


        private SolidBrush DrawCornersBrush;
        protected void DrawCorners(Color c1, int offset)
        {
            DrawCorners(c1, 0, 0, Width, Height, offset);
        }
        protected void DrawCorners(Color c1, Rectangle r1, int offset)
        {
            DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
        }
        protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
        {
            DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
        }

        protected void DrawCorners(Color c1)
        {
            DrawCorners(c1, 0, 0, Width, Height);
        }
        protected void DrawCorners(Color c1, Rectangle r1)
        {
            DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
        }
        protected void DrawCorners(Color c1, int x, int y, int width, int height)
        {
            if (_NoRounding)
                return;

            if (_Transparent)
            {
                B.SetPixel(x, y, c1);
                B.SetPixel(x + (width - 1), y, c1);
                B.SetPixel(x, y + (height - 1), c1);
                B.SetPixel(x + (width - 1), y + (height - 1), c1);
            }
            else
            {
                DrawCornersBrush = new SolidBrush(c1);
                G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
                G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
                G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
                G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
            }
        }

        #endregion

        #region " DrawBorders "

        protected void DrawBorders(Pen p1, int offset)
        {
            DrawBorders(p1, 0, 0, Width, Height, offset);
        }
        protected void DrawBorders(Pen p1, Rectangle r, int offset)
        {
            DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
        {
            DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
        }

        protected void DrawBorders(Pen p1)
        {
            DrawBorders(p1, 0, 0, Width, Height);
        }
        protected void DrawBorders(Pen p1, Rectangle r)
        {
            DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
        }
        protected void DrawBorders(Pen p1, int x, int y, int width, int height)
        {
            G.DrawRectangle(p1, x, y, width - 1, height - 1);
        }

        #endregion

        #region " DrawText "

        private Point DrawTextPoint;

        private Size DrawTextSize;
        protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
        {
            DrawText(b1, Text, a, x, y);
        }
        protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
        {
            if (text.Length == 0)
                return;

            DrawTextSize = Measure(text);
            DrawTextPoint = Center(DrawTextSize);

            switch (a)
            {
                case HorizontalAlignment.Left:
                    G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                    break;
                case HorizontalAlignment.Center:
                    G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                    break;
                case HorizontalAlignment.Right:
                    G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                    break;
            }
        }

        protected void DrawText(Brush b1, Point p1)
        {
            if (Text.Length == 0)
                return;
            G.DrawString(Text, Font, b1, p1);
        }
        protected void DrawText(Brush b1, int x, int y)
        {
            if (Text.Length == 0)
                return;
            G.DrawString(Text, Font, b1, x, y);
        }

        #endregion

        #region " DrawImage "


        private Point DrawImagePoint;
        protected void DrawImage(HorizontalAlignment a, int x, int y)
        {
            DrawImage(_Image, a, x, y);
        }
        protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
        {
            if (image == null)
                return;
            DrawImagePoint = Center(image.Size);

            switch (a)
            {
                case HorizontalAlignment.Left:
                    G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                    break;
                case HorizontalAlignment.Center:
                    G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                    break;
                case HorizontalAlignment.Right:
                    G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                    break;
            }
        }

        protected void DrawImage(Point p1)
        {
            DrawImage(_Image, p1.X, p1.Y);
        }
        protected void DrawImage(int x, int y)
        {
            DrawImage(_Image, x, y);
        }

        protected void DrawImage(Image image, Point p1)
        {
            DrawImage(image, p1.X, p1.Y);
        }
        protected void DrawImage(Image image, int x, int y)
        {
            if (image == null)
                return;
            G.DrawImage(image, x, y, image.Width, image.Height);
        }

        #endregion

        #region " DrawGradient "

        private LinearGradientBrush DrawGradientBrush;

        private Rectangle DrawGradientRectangle;
        protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
        {
            DrawGradientRectangle = new Rectangle(x, y, width, height);
            DrawGradient(blend, DrawGradientRectangle);
        }
        protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
        {
            DrawGradientRectangle = new Rectangle(x, y, width, height);
            DrawGradient(blend, DrawGradientRectangle, angle);
        }

        protected void DrawGradient(ColorBlend blend, Rectangle r)
        {
            DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
            DrawGradientBrush.InterpolationColors = blend;
            G.FillRectangle(DrawGradientBrush, r);
        }
        protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
        {
            DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
            DrawGradientBrush.InterpolationColors = blend;
            G.FillRectangle(DrawGradientBrush, r);
        }


        protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
        {
            DrawGradientRectangle = new Rectangle(x, y, width, height);
            DrawGradient(c1, c2, DrawGradientRectangle);
        }
        protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
        {
            DrawGradientRectangle = new Rectangle(x, y, width, height);
            DrawGradient(c1, c2, DrawGradientRectangle, angle);
        }

        protected void DrawGradient(Color c1, Color c2, Rectangle r)
        {
            DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
            G.FillRectangle(DrawGradientBrush, r);
        }
        protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
        {
            DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
            G.FillRectangle(DrawGradientBrush, r);
        }

        #endregion

        #region " DrawRadial "

        private GraphicsPath DrawRadialPath;
        private PathGradientBrush DrawRadialBrush1;
        private LinearGradientBrush DrawRadialBrush2;

        private Rectangle DrawRadialRectangle;
        public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
        }
        public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
        }
        public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(blend, DrawRadialRectangle, cx, cy);
        }

        public void DrawRadial(ColorBlend blend, Rectangle r)
        {
            DrawRadial(blend, r, r.Width / 2, r.Height / 2);
        }
        public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
        {
            DrawRadial(blend, r, center.X, center.Y);
        }
        public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
        {
            DrawRadialPath.Reset();
            DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

            DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
            DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
            DrawRadialBrush1.InterpolationColors = blend;

            if (G.SmoothingMode == SmoothingMode.AntiAlias)
            {
                G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
            }
            else
            {
                G.FillEllipse(DrawRadialBrush1, r);
            }
        }


        protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(c1, c2, DrawRadialRectangle);
        }
        protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
        {
            DrawRadialRectangle = new Rectangle(x, y, width, height);
            DrawRadial(c1, c2, DrawRadialRectangle, angle);
        }

        protected void DrawRadial(Color c1, Color c2, Rectangle r)
        {
            DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
            G.FillEllipse(DrawRadialBrush2, r);
        }
        protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
        {
            DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
            G.FillEllipse(DrawRadialBrush2, r);
        }

        #endregion

        #region " CreateRound "

        private GraphicsPath CreateRoundPath;

        private Rectangle CreateRoundRectangle;
        public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
        {
            CreateRoundRectangle = new Rectangle(x, y, width, height);
            return CreateRound(CreateRoundRectangle, slope);
        }

        public GraphicsPath CreateRound(Rectangle r, int slope)
        {
            CreateRoundPath = new GraphicsPath(FillMode.Winding);
            CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
            CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
            CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
            CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
            CreateRoundPath.CloseFigure();
            return CreateRoundPath;
        }

        #endregion

    }

    static class ThemeShare
    {

        #region " Animation "

        private static int Frames;
        private static bool Invalidate;

        public static PrecisionTimer ThemeTimer = new PrecisionTimer();
        //1000 / 50 = 20 FPS
        private const int FPS = 50;

        private const int Rate = 10;
        public delegate void AnimationDelegate(bool invalidate);


        private static List<AnimationDelegate> Callbacks = new List<AnimationDelegate>();
        private static void HandleCallbacks(IntPtr state, bool reserve)
        {
            Invalidate = (Frames >= FPS);
            if (Invalidate)
                Frames = 0;

            lock (Callbacks)
            {
                for (int I = 0; I <= Callbacks.Count - 1; I++)
                {
                    Callbacks[I].Invoke(Invalidate);
                }
            }

            Frames += Rate;
        }

        private static void InvalidateThemeTimer()
        {
            if (Callbacks.Count == 0)
            {
                ThemeTimer.Delete();
            }
            else
            {
                ThemeTimer.Create(0, Rate, HandleCallbacks);
            }
        }

        public static void AddAnimationCallback(AnimationDelegate callback)
        {
            lock (Callbacks)
            {
                if (Callbacks.Contains(callback))
                    return;

                Callbacks.Add(callback);
                InvalidateThemeTimer();
            }
        }

        public static void RemoveAnimationCallback(AnimationDelegate callback)
        {
            lock (Callbacks)
            {
                if (!Callbacks.Contains(callback))
                    return;

                Callbacks.Remove(callback);
                InvalidateThemeTimer();
            }
        }

        #endregion

    }

    enum MouseState : byte
    {
        None = 0,
        Over = 1,
        Down = 2,
        Block = 3
    }

    struct Bloom
    {

        public string _Name;
        public string Name
        {
            get { return _Name; }
        }

        private Color _Value;
        public Color Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public string ValueHex
        {
            get { return string.Concat("#", _Value.R.ToString("X2", null), _Value.G.ToString("X2", null), _Value.B.ToString("X2", null)); }
            set
            {
                try
                {
                    _Value = ColorTranslator.FromHtml(value);
                }
                catch
                {
                    return;
                }
            }
        }


        public Bloom(string name, Color value)
        {
            _Name = name;
            _Value = value;
        }
    }

    //------------------
    //Creator: aeonhack
    //Site: elitevs.net
    //Created: 11/30/2011
    //Changed: 11/30/2011
    //Version: 1.0.0
    //------------------
    class PrecisionTimer : IDisposable
    {

        private bool _Enabled;
        public bool Enabled
        {
            get { return _Enabled; }
        }

        private IntPtr Handle;

        private TimerDelegate TimerCallback;
        [DllImport("kernel32.dll", EntryPoint = "CreateTimerQueueTimer")]
        private static extern bool CreateTimerQueueTimer(ref IntPtr handle, IntPtr queue, TimerDelegate callback, IntPtr state, uint dueTime, uint period, uint flags);

        [DllImport("kernel32.dll", EntryPoint = "DeleteTimerQueueTimer")]
        private static extern bool DeleteTimerQueueTimer(IntPtr queue, IntPtr handle, IntPtr callback);

        public delegate void TimerDelegate(IntPtr r1, bool r2);

        public void Create(uint dueTime, uint period, TimerDelegate callback)
        {
            if (_Enabled)
                return;

            TimerCallback = callback;
            bool Success = CreateTimerQueueTimer(ref Handle, IntPtr.Zero, TimerCallback, IntPtr.Zero, dueTime, period, 0);

            if (!Success)
                ThrowNewException("CreateTimerQueueTimer");
            _Enabled = Success;
        }

        public void Delete()
        {
            if (!_Enabled)
                return;
            bool Success = DeleteTimerQueueTimer(IntPtr.Zero, Handle, IntPtr.Zero);

            if (!Success && !(Marshal.GetLastWin32Error() == 997))
            {
                ThrowNewException("DeleteTimerQueueTimer");
            }

            _Enabled = !Success;
        }

        private void ThrowNewException(string name)
        {
            throw new Exception(string.Format("{0} failed. Win32Error: {1}", name, Marshal.GetLastWin32Error()));
        }

        public void Dispose()
        {
            Delete();
        }
    }
}