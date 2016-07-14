namespace Paint.RAGE
{
    // System
    using System;
    using System.Linq;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Collections.Generic;

    // RPH
    using Rage;
    using Rage.Native;

    // AdvancedUI
    using AdvancedUI;

    internal static class EntryPoint
    { 
        static Dictionary<int, Circle> CirclesByIndex = new Dictionary<int, Circle>();

        static Vector2 MousePosition = new Vector2();

        static float BrushRadius = 50.0f;
        static int BrushColorIndex = 1;
        static Color BrushColor => BrushColors[BrushColorIndex];

        static Color[] BrushColors =
        {
            Color.Transparent, // add extra one to get the indexes easier
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.White,
            Color.Black,
            Color.SandyBrown,
            Color.Pink,
            Color.Orange,
            Color.Yellow,
        };

        static RectangleF[] PreviewColorsRectangles =
        {
            new RectangleF(), // add extra one to get the indexes easier
            new RectangleF(16 + 64 * 0, 16, 32, 32),
            new RectangleF(16 + 64 * 1, 16, 32, 32),
            new RectangleF(16 + 64 * 2, 16, 32, 32),
            new RectangleF(16 + 64 * 3, 16, 32, 32),
            new RectangleF(16 + 64 * 4, 16, 32, 32),
            new RectangleF(16 + 64 * 5, 16, 32, 32),
            new RectangleF(16 + 64 * 6, 16, 32, 32),
            new RectangleF(16 + 64 * 7, 16, 32, 32),
            new RectangleF(16 + 64 * 8, 16, 32, 32),
        };

        static Texture PencilCursorTexture = Game.CreateTextureFromFile("pencil.png");

        public static void Main()
        {
            while (Game.IsLoading)
                GameFiber.Yield();

            Game.RawFrameRender += OnRawFrameRender;
            while (true)
            {
                GameFiber.Yield();
                MainUpdate();
            }
        }


        static void MainUpdate()
        {
            UICommon.DisableAllGameControls();

            MouseState mouse = GetMouseState();
            MousePosition = GetMousePosition(mouse);

            if (Game.IsKeyDownRightNow(Keys.LButton) && !CirclesByIndex.Any(x => Vector2.DistanceSquared(x.Value.Position, MousePosition) < 0.3225f))
            {
                CirclesByIndex.Add(CirclesByIndex.Count, new Circle(MousePosition, BrushRadius, BrushColor));
            }

            if (Game.IsKeyDown(Keys.D1))
                BrushColorIndex = 1;
            else if(Game.IsKeyDown(Keys.D2))
                BrushColorIndex = 2;
            else if (Game.IsKeyDown(Keys.D3))
                BrushColorIndex = 3;
            else if (Game.IsKeyDown(Keys.D4))
                BrushColorIndex = 4;
            else if (Game.IsKeyDown(Keys.D5))
                BrushColorIndex = 5;
            else if (Game.IsKeyDown(Keys.D6))
                BrushColorIndex = 6;
            else if (Game.IsKeyDown(Keys.D7))
                BrushColorIndex = 7;
            else if (Game.IsKeyDown(Keys.D8))
                BrushColorIndex = 8;
            else if (Game.IsKeyDown(Keys.D9))
                BrushColorIndex = 9;

            BrushRadius = MathHelper.Clamp(BrushRadius + mouse.MouseWheelDelta * 1.8345f, 5.0f, 360.0f);

            if(Game.IsControlKeyDownRightNow && Game.IsKeyDownRightNow(Keys.Z) && CirclesByIndex.Count > 0)
            {
                CirclesByIndex.Remove(CirclesByIndex.Count - 1);
            }
        }


        static void OnRawFrameRender(object sender, GraphicsEventArgs e)
        {
            for (int i = 0; i < CirclesByIndex.Count; i++)
            {
                Circle c = CirclesByIndex[i];
                e.Graphics.DrawFilledCircle(c.Position, c.Radius, c.Color);
            }


            e.Graphics.DrawCircle(MousePosition, BrushRadius, Color.Black);
            e.Graphics.DrawTexture(PencilCursorTexture, new RectangleF(MousePosition.X, MousePosition.Y - 128, 128, 128));

            e.Graphics.DrawRectangle(RectangleF.Inflate(PreviewColorsRectangles[BrushColorIndex], 5, 5), Color.White);

            for (int i = 1; i < PreviewColorsRectangles.Length; i++)
            {
                e.Graphics.DrawRectangle(RectangleF.Inflate(PreviewColorsRectangles[i], 2, 2), Color.Black);
                e.Graphics.DrawRectangle(PreviewColorsRectangles[i], BrushColors[i]);
            }

        }


        static MouseState GetMouseState()
        {
            return Game.GetMouseState();
        }

        static Vector2 GetMousePosition(MouseState mouse)
        {
            return new Vector2(mouse.X, mouse.Y);
        }
    }
}
