using System;
using System.Drawing;

namespace LiteDraw
{
	static class Program
	{
		private static void Main(string[] Args)
		{
			DrawingTest();
		}

		private static void DrawingTest()
		{
			LiteDraw w = new LiteDraw();

			w.Color = Color.Red;
			w.MouseClick += (litedraw, args) =>
			{
				litedraw.FilledRectangle(args.X, args.Y, 10, 10);
				litedraw.Show();
			};
		}

		private static void ImageSaveTest()
		{
			LiteDraw w = new LiteDraw();

			w.Color = Color.BlueViolet;

			w.DrawArc(200, 200, 50, 50, 0, 90);
			w.FilledArc(200, 400, 50, 50, 0, 270);

			w.DrawArc(400, 200, 50, 50, 0, Math.PI / 2);
			w.FilledArc(400, 400, 50, 50, 0, Math.PI * 3 / 2);

			w.Color = Color.Orange;
			w.DrawRectangle(150, 150, 100, 100);

			w.Color = Color.Red;
			w.FilledCircle(200, 200, 10);

			w.AsImage().Save("C:\\Users\\Dewernh\\Desktop\\saved.png");

			w.Show();
		}

		private static void FontTest()
		{
			LiteDraw w = new LiteDraw();

			w.Font = new Font(new FontFamily("Arial"), 20, FontStyle.Bold);

			w.DrawText(200, 200, "MY BOLD TEXT!");

			w.Show();
		}

		private static void ImageScaleTest()
		{
			LiteDraw w = new LiteDraw();

			w.DrawImage(100, 100, 200, 200, Image.FromFile("C:\\Users\\Dewernh\\Desktop\\test.jpg"));

			w.Show();
		}

		private static void ImageTest()
		{
			LiteDraw w = new LiteDraw();

			w.DrawImage(10, 10, Image.FromFile("C:\\Users\\Dewernh\\Desktop\\test.jpg"));

			w.Show();
		}

		private static void PolygonTest()
		{
			LiteDraw w = new LiteDraw();
			w.FilledPolygon((10, 40), (200, 200), (100, 30));
			w.Show();
		}

		private static void BezierTest()
		{
			LiteDraw w = new LiteDraw();
			w.DrawBezier((100, 100), (300, 200), (200, 300), (400, 400));
			w.Show();
		}

		private static void ArcTest()
		{
			LiteDraw w = new LiteDraw();

			w.Color = Color.BlueViolet;

			w.DrawArc(200, 200, 50, 50, 0, 90);
			w.FilledArc(200, 400, 50, 50, 0, 270);

			w.DrawArc(400, 200, 50, 50, 0, Math.PI / 2);
			w.FilledArc(400, 400, 50, 50, 0, Math.PI * 3 / 2);

			w.Color = Color.Orange;
			w.DrawRectangle(150, 150, 100, 100);

			w.Color = Color.Red;
			w.FilledCircle(200, 200, 10);

			w.Show();
		}

		private static void AnimationTest2()
		{
			LiteDraw w = new LiteDraw();

			int steps = 5;
			int end = 80;
			int offset = 100;
			int pause = 10;

			while (true)
			{
				for (int i = 0; i < end; i++)
				{
					w.Reset();
					w.DrawText(offset + i * steps, offset, "I'm animated!");
					w.Show(pause);
				}

				for (int i = 0; i < end; i++)
				{
					w.Reset();
					w.DrawText(offset + steps * end - i * steps, offset, "I'm animated!");
					w.Show(pause);
				}
			}
		}

		private static void TwoWindowTest()
		{
			LiteDraw w1 = new LiteDraw();
			LiteDraw w2 = new LiteDraw();

			w1.DrawCircle(100, 100, 50);
			w2.DrawCircle(400, 200, 100);

			w1.Show();
			w2.Show();
		}

		private static void CornerTest()
		{
			LiteDraw draw = new LiteDraw();

			int size = 1;

			draw.Color = Color.Red;
			draw.FilledRectangle(0, 0, size, size);
			draw.FilledRectangle(0, draw.Height - size, size, size);
			draw.FilledRectangle(draw.Width - size, 0, size, size);
			draw.FilledRectangle(draw.Width - size, draw.Height - size, size, size);
			draw.Show();
		}

		private static void AnimationTest()
		{
			LiteDraw draw = new LiteDraw();

			for (int i = 0; i < 30; i++)
			{
				draw.Reset();

				draw.Color = Color.Black;
				draw.DrawText(100, 400, "Hello World!");
				draw.FilledRectangle(100 + i * 10, 100 + i, 100, 100);
				draw.Color = Color.Orange;
				draw.FilledEllipse(20, 40, 20, 40);
				draw.Show(30);
			}
		}

		private static void FirstTest()
		{
			LiteDraw l = new LiteDraw();

			l.Color = Color.Red;
			l.FilledRectangle(20, 20, 100, 100);

			l.Title = "Hello World";

			l.Color = Color.Blue;
			l.FilledCircle(50, 50, 50);

			l.Color = Color.LightBlue;
			l.PenSize = 5;
			l.DrawRectangle(30, 30, 200, 200);

			l.Show();
		}
	}
}
