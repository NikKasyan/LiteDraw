using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace LiteDraw
{
	public class LiteDraw : IDisposable
	{
		public LiteDraw() : this(600, 600) { }

		public LiteDraw(int width, int height)
		{
			Width = width;
			Height = height;

			form = GuiRunner.CreateGui(new Size(width, Height));
			buffer = form.CreateBufferedGraphics();

			UpdateBrushes();
			Reset();
			Show();
		}

		private CanvasForm form;
		private BufferedGraphics buffer;
		private Graphics G => buffer.Graphics;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public int DesktopPositionX => form.DesktopLocation.X;

		public int DesktopPositionY => form.DesktopLocation.Y;

		public string Title
		{
			get => form.Title;
			set => form.Title = value;
		}

		public Font Font { get; set; } = new Font(FontFamily.GenericSansSerif, 12);

		private Color color = Color.Black;
		public Color Color
		{
			get => color;
			set { color = value; UpdateBrushes(); }
		}

		private double pensize = 1;
		public double PenSize
		{
			get => pensize;
			set { pensize = value; UpdateBrushes(); }
		}

		private SolidBrush brush = new SolidBrush(Color.Black);
		private Pen pen = new Pen(Color.Black, 1);
		private void UpdateBrushes()
		{
			brush.Dispose();
			brush = new SolidBrush(color);
			pen.Dispose();
			pen = new Pen(color, (float)pensize);
		}

		public delegate void EventHandler<T>(LiteDraw litedraw, T args);

		public event EventHandler<MouseEventArgs> MouseClick
		{
			add => form.MouseClick += MapEvent(value);
			remove => form.MouseClick -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseMove
		{
			add => form.MouseMove += MapEvent(value);
			remove => form.MouseMove -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseDown
		{
			add => form.MouseDown += MapEvent(value);
			remove => form.MouseDown -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseUp
		{
			add => form.MouseUp += MapEvent(value);
			remove => form.MouseUp -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseWheel
		{
			add => form.MouseWheel += MapEvent(value);
			remove => form.MouseWheel -= MapEvent(value);
		}
		public event EventHandler<MouseEventArgs> MouseDoubleClick
		{
			add => form.MouseDoubleClick += MapEvent(value);
			remove => form.MouseDoubleClick -= MapEvent(value);
		}
		public event EventHandler<EventArgs> MouseLeave
		{
			add => form.MouseLeave += MapEvent(value);
			remove => form.MouseLeave -= MapEvent(value);
		}

		public event EventHandler<KeyEventArgs> KeyDown
		{
			add => form.KeyDown += MapEvent(value);
			remove => form.KeyDown -= MapEvent(value);
		}
		public event EventHandler<KeyEventArgs> KeyUp
		{
			add => form.KeyUp += MapEvent(value);
			remove => form.KeyUp -= MapEvent(value);
		}
		public event EventHandler<KeyPressEventArgs> KeyPress
		{
			add => form.KeyPress += MapEvent(value);
			remove => form.KeyPress -= MapEvent(value);
		}

		public event EventHandler<EventArgs> WindowMove
		{
			add => form.WindowMove += MapEvent(value);
			remove => form.WindowMove -= MapEvent(value);
		}

		private EventHandler MapEvent(EventHandler<EventArgs> mouseEvent) {
			return (sender, args) => mouseEvent(this, args);
		}
		private MouseEventHandler MapEvent(EventHandler<MouseEventArgs> mouseEvent)
		{
			return (sender, args) => mouseEvent(this, args);
		}
		private KeyEventHandler MapEvent(EventHandler<KeyEventArgs> mouseEvent)
		{
			return (sender, args) => mouseEvent(this, args);
		}
		private KeyPressEventHandler MapEvent(EventHandler<KeyPressEventArgs> mouseEvent)
		{
			return (sender, args) => mouseEvent(this, args);
		}

		public void DrawLine((int, int) start, (int, int) end)
		{
			G.DrawLine(pen, start.Item1, start.Item2, end.Item1, end.Item2);
		}

		public void DrawBezier((int, int) start, (int, int) control1, (int, int) control2, (int, int) end)
		{
			G.DrawBezier(pen, MapToPoint(start), MapToPoint(control1), MapToPoint(control2), MapToPoint(end));
		}

		public void DrawText(int x, int y, string text)
		{
			G.DrawString(text, Font, brush, x, y);
		}

		public void DrawText(int x, int y, string text, StringFormat format)
		{
			G.DrawString(text, Font, brush, new PointF(x, y), format);
		}

		public void DrawPoint(int x, int y)
		{
			DrawRectangle(x, y, 1, 1);
		}

		public void DrawSquare(int x, int y, int size)
		{
			DrawRectangle(x, y, size, size);
		}

		public void FilledSquare(int x, int y, int size)
		{
			FilledRectangle(x, y, size, size);
		}

		public void DrawRectangle(int x, int y, int width, int height)
		{
			G.DrawRectangle(pen, x, y, width, height);
		}

		public void FilledRectangle(int x, int y, int width, int height)
		{
			G.FillRectangle(brush, x, y, width, height);
		}

		public void DrawCircle(int x, int y, int radius)
		{
			DrawEllipse(x, y, radius, radius);
		}

		public void FilledCircle(int x, int y, int radius)
		{
			FilledEllipse(x, y, radius, radius);
		}

		public void DrawEllipse(int x, int y, int hradius, int vradius)
		{
			G.DrawEllipse(pen, x - hradius, y - vradius, 2 * hradius, 2 * vradius);
		}

		public void FilledEllipse(int x, int y, int hradius, int vradius)
		{
			G.FillEllipse(brush, x - hradius, y - vradius, 2 * hradius, 2 * vradius);
		}

		public void DrawArc(int x, int y, int hradius, int vradius, int startDegrees, int sweepDegrees)
		{
			G.DrawArc(pen, new Rectangle(x - hradius, y - vradius, 2 * hradius, 2 * vradius), startDegrees, sweepDegrees);
		}

		public void DrawArc(int x, int y, int hradius, int vradius, double startRadians, double sweepRadians)
		{
			G.DrawArc(pen, new Rectangle(x - hradius, y - vradius, 2 * hradius, 2 * vradius), (float)ToDegrees(startRadians), (float)ToDegrees(sweepRadians));
		}

		public void FilledArc(int x, int y, int hradius, int vradius, int startDegrees, int sweepDegrees)
		{
			G.FillPie(brush, x - hradius, y - vradius, 2 * hradius, 2 * vradius, startDegrees, sweepDegrees);
		}

		public void FilledArc(int x, int y, int hradius, int vradius, double startRadians, double sweepRadians)
		{
			G.FillPie(brush, new Rectangle(x - hradius, y - vradius, 2 * hradius, 2 * vradius), (float)ToDegrees(startRadians), (float)ToDegrees(sweepRadians));
		}

		public void DrawPolygon(params (int, int)[] points)
		{
			G.DrawPolygon(pen, MapToPoints(points));
		}

		public void FilledPolygon(params (int, int)[] points)
		{
			G.FillPolygon(brush, MapToPoints(points));
		}

		public void DrawImage(int x, int y, Image image)
		{
			G.DrawImage(image, x, y);
		}

		public void DrawImage(int x, int y, int width, int height, Image image)
		{
			G.DrawImage(image, x, y, width, height);
		}

		public Image AsImage()
		{
			Bitmap bitmap = new Bitmap(Width, Height);
			using Graphics graphics = Graphics.FromImage(bitmap);
			buffer.Render(graphics);
			return bitmap;
		}

		public void Transform(Matrix matrix)
		{
			G.MultiplyTransform(matrix);
		}

		public void Reset()
		{
			Reset(Color.White);
		}

		public void Reset(Color color)
		{
			Color c = Color;
			Color = color;
			FilledRectangle(0, 0, Width, Height);
			Color = c;
		}

		public void Show()
		{
			form.Render(buffer);
		}

		public void Show(int waitMilliseconds)
		{
			form.Render(buffer, waitMilliseconds);
		}

		public void Dispose()
		{
			brush.Dispose();
			pen.Dispose();
			Font.Dispose();
			form.CloseAndDispose();
		}

		private static double ToDegrees(double Radians)
		{
			return Radians * (180 / Math.PI);
		}

		private static Point[] MapToPoints((int, int)[] tuples)
		{
			Point[] result = new Point[tuples.Length];

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = MapToPoint(tuples[i]);
			}

			return result;
		}

		private static Point MapToPoint((int, int) tuple)
		{
			return new Point(tuple.Item1, tuple.Item2);
		}

		private class CanvasForm : IDisposable
		{
			public CanvasForm(Size size)
			{
				this.size = size;
				this.canvas = new Bitmap(this.size.Width, size.Height);
				this.form = new DoubleBufferedForm();

				form.SuspendLayout();

				form.AutoScaleDimensions = new SizeF(7F, 15F);
				form.AutoScaleMode = AutoScaleMode.Font;
				form.ClientSize = size;
				form.Name = "LiteDrawForm";
				form.ShowIcon = false;
				form.Text = "LiteDraw";
				form.MaximizeBox = false;
				form.FormBorderStyle = FormBorderStyle.FixedSingle;
				form.KeyDown += OnCtrlCPasteDisplayImageToClipboard;

				form.ResumeLayout(false);
			}

			private Form form;
			private Size size;
			private Bitmap canvas;
			private BufferedGraphicsContext context = new BufferedGraphicsContext();

			public string Title
			{
				get => form.Text;
				set => InvokeAsync(() => form.Text = value);
			}

			public Point DesktopLocation => form.DesktopLocation;

			public event MouseEventHandler MouseClick
			{
				add => form.MouseClick += value;
				remove => form.MouseClick -= value;
			}
			public event MouseEventHandler MouseMove
			{
				add => form.MouseMove += value;
				remove => form.MouseMove -= value;
			}
			public event MouseEventHandler MouseDown
			{
				add => form.MouseDown += value;
				remove => form.MouseDown -= value;
			}
			public event MouseEventHandler MouseUp
			{
				add => form.MouseUp += value;
				remove => form.MouseUp -= value;
			}
			public event MouseEventHandler MouseWheel
			{
				add => form.MouseWheel += value;
				remove => form.MouseWheel -= value;
			}
			public event MouseEventHandler MouseDoubleClick
			{
				add => form.MouseDoubleClick += value;
				remove => form.MouseDoubleClick -= value;
			}
			public event EventHandler MouseLeave
			{
				add => form.MouseLeave += value;
				remove => form.MouseLeave -= value;
			}

			public event KeyEventHandler KeyDown
			{
				add => form.KeyDown += value;
				remove => form.KeyDown -= value;
			}
			public event KeyEventHandler KeyUp
			{
				add => form.KeyUp += value;
				remove => form.KeyUp -= value;
			}
			public event KeyPressEventHandler KeyPress
			{
				add => form.KeyPress += value;
				remove => form.KeyPress -= value;
			}

			public event EventHandler WindowMove
			{
				add => form.Move += value;
				remove => form.Move -= value;
			}

			public BufferedGraphics CreateBufferedGraphics()
			{
				return context.Allocate(form.CreateGraphics(), form.DisplayRectangle);
			}

			public void Render(BufferedGraphics buffer)
			{
				CopyBufferToCanvas(buffer);
				InvokeSync(() => CopyBufferToForm(buffer));
			}

			public void Render(BufferedGraphics buffer, int waitMilliseconds)
			{
				CopyBufferToCanvas(buffer);

				using Semaphore s = new Semaphore(0, 100);
				InvokeAsync(() =>
				{
					CopyBufferToForm(buffer);
					s.Release();
				});
				Thread.Sleep(waitMilliseconds);
				s.WaitOne();
			}

			public void WaitForInvocability()
			{
				while (!form.IsHandleCreated) Thread.Sleep(10);
				InvokeSync(() => { });
			}

			private void OnCtrlCPasteDisplayImageToClipboard(object? sender, KeyEventArgs args)
			{
				if (args.Control && args.KeyCode == Keys.C)
				{
					Clipboard.SetImage(canvas);
				}
			}

			private void CopyBufferToCanvas(BufferedGraphics buffer)
			{
				using Graphics g1 = Graphics.FromImage(canvas);
				buffer.Render(g1);
			}

			private void CopyBufferToForm(BufferedGraphics buffer)
			{
				using Graphics g2 = form.CreateGraphics();
				buffer.Render(g2);
			}

			private void InvokeAsync(Action action)
			{
				action = IgnoreObjectDisposedException(action);

				if (form.InvokeRequired)
				{
					form.Invoke(action);
				}
				else
				{
					action();
				}
			}

			private void InvokeSync(Action action)
			{
				action = IgnoreObjectDisposedException(action);
				using Semaphore s = new Semaphore(0, 100);

				InvokeAsync(() =>
				{
					action();
					s.Release();
				});

				s.WaitOne();
			}

			private static Action IgnoreObjectDisposedException(Action action)
			{
				return () =>
				{
					try
					{
						action();
					}
					catch (ObjectDisposedException) { }
				};
			}

			public void CloseAndDispose()
			{
				InvokeSync(() => form.Close());
				Dispose();
			}

			public void Dispose()
			{
				canvas.Dispose();
				form.Dispose();
				context.Dispose();
			}

			public static void Configure()
			{
				Application.SetHighDpiMode(HighDpiMode.SystemAware);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
			}

			public static void Run(CanvasForm form)
			{
				Application.Run(form.form);
			}

			private class DoubleBufferedForm : Form
			{
				public DoubleBufferedForm()
				{
					DoubleBuffered = true;
				}
			}
		}

		private class GuiRunner : IDisposable
		{
			public static CanvasForm CreateGui(Size size)
			{
				using GuiRunner runner = new GuiRunner(size);
				return runner.Start();
			}

			private GuiRunner(Size size)
			{
				this.size = size;
				thread = new Thread(Run);
			}

			private CanvasForm Start()
			{
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				canvasIsSet.WaitOne();
				if (canvas == null) throw new Exception("LiteDraw is in an invalid state, although gui has been instanciated the gui is still null.");
				canvas.WaitForInvocability();
				return canvas;
			}

			private Size size;
			private CanvasForm? canvas;
			private Semaphore canvasIsSet = new Semaphore(0, 100);
			private Thread thread;

			[STAThread]
			private void Run()
			{
				CanvasForm.Configure();

				try
				{
					IncrementCanvasCount();
					canvas = new CanvasForm(size);
					canvasIsSet.Release();
					CanvasForm.Run(canvas);
				}
				finally
				{
					DecrementCanvasCount();
				}
			}

			public void Dispose()
			{
				canvasIsSet.Dispose();
			}

			private static Semaphore canvasCountRegion = new Semaphore(1, int.MaxValue);
			private static int canvasCount = 0;

			private static void IncrementCanvasCount()
			{
				canvasCountRegion.WaitOne();
				canvasCount++;
				canvasCountRegion.Release();
			}

			private static void DecrementCanvasCount()
			{
				canvasCountRegion.WaitOne();
				canvasCount--;
				if (canvasCount == 0) Environment.Exit(0);
				canvasCountRegion.Release();
			}
		}
	}
}
