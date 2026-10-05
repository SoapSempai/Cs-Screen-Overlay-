using System.Drawing.Drawing2D;
using TEST_1.Data;

namespace TEST_1;

internal sealed class OverlayForm : Form
{
    private CharacterMatch? _match;

    public OverlayForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        TopMost = true;
        StartPosition = FormStartPosition.Manual;
        BackColor = Color.Lime;
        TransparencyKey = Color.Lime;
        Opacity = 0.95;
        Bounds = Screen.PrimaryScreen?.Bounds ?? new Rectangle(0, 0, 800, 600);
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x20 | 0x80000 | 0x8000000 | 0x80;
            return cp;
        }
    }

    public void SetCharacter(CharacterMatch match)
    {
        _match = match;
        Bounds = Screen.PrimaryScreen?.Bounds ?? Bounds;
        if (!Visible) Show();
        Invalidate();
    }

    public void ClearBoxes()
    {
        _match = null;
        Invalidate();
    }

    protected override void OnPaintBackground(PaintEventArgs e) => e.Graphics.Clear(TransparencyKey);

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_match is null) return;

        using var boxPen = new Pen(Color.Red, 3f);
        using var skeletonPen = new Pen(Color.DeepSkyBlue, 3f) {
            StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round
        };
        using var jointBrush = new SolidBrush(Color.White);
        using var headPen = new Pen(Color.Gold, 2.5f);

        var r = _match.Bounds;
        e.Graphics.DrawRectangle(boxPen, r);

        var k = _match.Keypoints;
        DrawSkeleton(e.Graphics, skeletonPen, jointBrush, k);

        if (k.Length > 0 && k[0].Score >= 0.30f)
        {
            const int radius = 9;
            e.Graphics.DrawEllipse(headPen,
                k[0].Point.X - radius, k[0].Point.Y - radius,
                radius * 2, radius * 2);
        }

        using var textBrush = new SolidBrush(Color.White);
        using var font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
        e.Graphics.DrawString($"Person {_match.Confidence:P0}", font, textBrush,
            r.Left, Math.Max(0, r.Top - 22));
    }

    private static void DrawSkeleton(Graphics g, Pen pen, Brush brush, PoseKeypoint[] k)
    {
        int[][] links = {
            new[] {0,1}, new[] {0,2}, new[] {1,3}, new[] {2,4},
            new[] {5,6}, new[] {5,7}, new[] {7,9}, new[] {6,8}, new[] {8,10},
            new[] {5,11}, new[] {6,12}, new[] {11,12},
            new[] {11,13}, new[] {13,15}, new[] {12,14}, new[] {14,16}
        };

        foreach (var link in links)
        {
            if (k[link[0]].Score >= 0.30f && k[link[1]].Score >= 0.30f)
                g.DrawLine(pen, k[link[0]].Point, k[link[1]].Point);
        }

        foreach (var point in k)
        {
            if (point.Score < 0.30f) continue;
            const int radius = 4;
            g.FillEllipse(brush, point.Point.X - radius, point.Point.Y - radius,
                radius * 2, radius * 2);
        }
    }
}
