using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;
using OpenTK.Input;

namespace Session3
{
        class Program : GameWindow
        {
        public static string TITLE = "Catch Ball Game";
        public static int WIDTH = 500;
        public static int HEIGHT = 500;
        public int textureBallId = 0;
        public int textureSkyId = 0;
        public int textureBlockId = 0;
        public int textureBasketId = 0;
        public int texturePauseId = 0;
        int entered = 1;
        public Brush brush;
        public Font font;
        public OpenTK.Graphics.TextPrinter text;
        public float[] speed;
        public float blockSpeed = 0.1f;
        public float speedNum = 0;
        public float[] left;
        public float[] right;
        public float dy = 0;
        public float dx = 0;
        public int score = 0;
        public float sx = 1f;
        public float sy = 1f;
        public int threshold = 0;
        public bool paused = false;
        public bool ConditionEntered = false;
        public int missedBalls = 0;
        public List<float> dxBalls;
        public List<float> dyBalls;
        public List<bool> checkBalls;
        public List<float> dxBlocks;
        public List<float> dyBlocks;
        public List<bool> checkBlocks;
        public int blockDrop = 1;
        public bool gameOver = false;
        public string losingMsg = "";
        Random r;

        public Program() : base(WIDTH, HEIGHT, GraphicsMode.Default, TITLE) {
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            text = new OpenTK.Graphics.TextPrinter();
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            brush = Brushes.Black;
            font = new Font("Times New Roman", 12.0f);
            textureBallId = Utilities.LoadTexture(@"images\ball.png");
            textureBasketId = Utilities.LoadTexture(@"images\basket.png");
            textureSkyId = Utilities.LoadTexture(@"images\sky.jpg");
            textureBlockId = Utilities.LoadTexture(@"images\block.png");
            texturePauseId = Utilities.LoadTexture(@"images\Pause.png");
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha,BlendingFactorDest.OneMinusSrcAlpha);
            dxBalls = new List<float>();
            dyBalls = new List<float>();
            checkBalls = new List<bool>();
            dxBlocks = new List<float>();
            dyBlocks = new List<float>();
            threshold = 30;
            left = new float[3];
            right = new float[3];
            speed = new float[3];
            left[0] = -0.04f;
            right[0] = 0.04f;
            left[1] = -0.07f;
            right[1] = 0.08f;
            left[2] = -0.07f;
            right[2] = 0.08f;
            speed[0] = 0.08f;
            speed[1] = 0.12f;
            speed[2] = 0.16f;
            dxBalls.Add(0);
            dyBalls.Add(0);
            r = new Random();
            checkBalls.Add(false);
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            entered = 1;
            if ((Mouse[MouseButton.Left] || Mouse[MouseButton.Right]) && !paused)
            {
                if ((Mouse.X < 50 && Mouse.X > 10) && (Mouse.Y > 10 && Mouse.Y < 50))
                {
                    paused = true;
                }
            }
            if (Keyboard[Key.Space] && !paused)
            {
                paused = true;
                entered = 2;
            }
            if ((Mouse[MouseButton.Left] || Mouse[MouseButton.Right]) && paused)
            {
                if ((Mouse.X <= 320 && Mouse.X >= 185) && (Mouse.Y >= 275 && Mouse.Y <= 312))
                {
                    paused = false;
                }
            }
            if (Keyboard[Key.Space] && paused && entered == 1)
            {
                paused = false;
            }
            if ((Keyboard[Key.Space] && gameOver) || (((Mouse[MouseButton.Left] || Mouse[MouseButton.Right]) && gameOver && (Mouse.X <= 300 && Mouse.X >= 200) && (Mouse.Y >= 275 && Mouse.Y <= 315))))
            {
                dy = 0;
                dx = 0;
                score = 0;
                sx = 1f;
                sy = 1f;
                paused = false;
                ConditionEntered = false;
                missedBalls = 0;
                blockDrop = 1;
                dxBalls = new List<float>();
                dyBalls = new List<float>();
                checkBalls = new List<bool>();
                dxBlocks = new List<float>();
                dyBlocks = new List<float>();
                dxBalls.Add(0);
                dyBalls.Add(0);
                checkBalls.Add(false);
                gameOver = false;
                blockSpeed = 0.1f;
                speedNum = 0;
                losingMsg = "";
            }
            if (!paused && !gameOver)
            {
                for (int i = 0; i < dxBlocks.Count; i++)
                {
                    if (dyBlocks[i] + 0.9 * sy < -0.95f)
                    {
                        dxBlocks.RemoveAt(i);
                        dyBlocks.RemoveAt(i);
                        continue;
                    }
                    if ((0.9 * sy + dyBlocks[i] <= dy - 0.7f) && (1 * sy + dyBlocks[i] >= dy - 1f) && ((0.05 * sx + dxBlocks[i] >= dx - 0.25) && (-0.05 * sx + dxBlocks[i]  <= dx + 0.25)))
                    {
                        gameOver = true;
                        losingMsg = "Your basket touched a block!";
                        break;
                    }
                    dyBlocks[i] -= blockSpeed;
                }
                if (score >= 30)
                {
                    if (score >= 50)
                    {
                        speedNum = 2;
                    }
                    else
                    {
                    sx = 1.5f;
                    sy = 1.5f;
                    blockSpeed = 0.12f;
                    }
                }
                else if (score >= 10)
                {
                    speedNum = 1;
                }

                if (Keyboard[Key.Left])
                {
                    if (dx - 0.25f > -1f)
                    {
                        dx -= 0.1f;
                    }
                }
                if (Keyboard[Key.Right])
                {
                    if (dx + 0.25f < 1f)
                    {
                        dx += 0.1f;
                    }
                }
                if (Keyboard[Key.Up])
                {
                    if (dy - 0.7f < -0.25f)
                    {
                        dy += 0.1f;
                    }
                }
                if (Keyboard[Key.Down])
                {
                    if (dy - 1f > -0.95f)
                    {
                        dy -= 0.1f;
                    }
                }

                for (int i = 0; i < dxBalls.Count; i++)
                {
                    if (dyBalls[i] + 0.9 < -0.7 + dy)
                    {
                        if (checkBalls[i])
                        {
                            score++;
                        }
                        else
                        {
                            missedBalls++;
                        }
                        dxBalls.RemoveAt(i);
                        dyBalls.RemoveAt(i);
                        checkBalls.RemoveAt(i);
                        continue;
                    }
                    if (dyBalls[i] + 1 > left[(int)speedNum] && (dyBalls[i] + 1) < right[(int)speedNum])
                    {
                        dxBalls.Add((float)r.NextDouble() * (2 * 0.95f) - 0.95f);
                        dyBalls.Add(0);
                        checkBalls.Add(false);
                        blockDrop++;
                        r.NextDouble();
                    }
                    if (dyBalls[i] + 0.9 >= -0.7 + dy)
                    {
                        if (dxBalls[i] - 0.05f >= dx - 0.275f && dxBalls[i] + 0.05f <= dx + 0.275f)
                        {
                            checkBalls[i] = true;
                        }
                        else
                        {
                            checkBalls[i] = false;
                        }
                    }
                    dyBalls[i] -= speed[(int)speedNum];
                }
                if (blockDrop % 5 == 0)
                {
                    r.NextDouble();
                    dxBlocks.Add((float)r.NextDouble() * (2 * 0.6f) - 0.6f);
                    dyBlocks.Add(0);
                    blockDrop++;
                }
                
                if (missedBalls >= threshold)
                {
                    gameOver = true;
                    losingMsg = "Your missed balls number has reached "+ threshold+" balls!";
                }
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            if (gameOver)
            {
                GL.Color3(Color.LightGray);
                GL.Begin(BeginMode.Polygon);
                GL.Vertex2(-0.7f, -0.3f);
                GL.Vertex2(0.7f, -0.3f);
                GL.Vertex2(0.7f, 0.3f);
                GL.Vertex2(-0.7f, 0.3f);
                GL.End();
                GL.Color3(Color.SkyBlue);
                GL.Begin(BeginMode.Polygon);
                GL.Vertex2(-0.2f, -0.25f);
                GL.Vertex2(0.2f, -0.25f);
                GL.Vertex2(0.2f, -0.1f);
                GL.Vertex2(-0.2f, -0.1f);
                GL.End();
                text.Begin();
                text.Print("Your score is " + score, font, Color.Red, new RectangleF(150, 220, 200, 200), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                text.Print(losingMsg , font, Color.Red, new RectangleF(80, 250, 350,200), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                text.Print("Play Again", font, Color.Black, new RectangleF(150, 280, 200, 200), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                text.End();
            }
            if (paused && !gameOver)
            {
                GL.Color3(Color.LightGray);
                GL.Begin(BeginMode.Polygon);
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-0.7f, -0.3f);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(0.7f, -0.3f);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(0.7f, 0.3f);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-0.7f, 0.3f);
                GL.End();
                GL.Color3(Color.SkyBlue);
                GL.Begin(BeginMode.Polygon);
                GL.Vertex2(-0.25f, -0.25f);
                GL.Vertex2(0.25f, -0.25f);
                GL.Vertex2(0.25f, -0.1f);
                GL.Vertex2(-0.25f, -0.1f);
                GL.End();
                text.Begin();
                text.Print("Your score is "+score, font, Color.Red, new RectangleF(150, 220, 200, 200), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                text.Print("Continue Playing", font, Color.Black, new RectangleF(150,280,200,200), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                text.End();
            }
            if (!paused && !gameOver)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.Color3(Color.White);
                GL.LoadIdentity();
                GL.BindTexture(TextureTarget.Texture2D, textureSkyId);
                GL.Begin(BeginMode.Polygon);
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1.0f, -1.0f);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.0f, 1.0f);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1.0f, 1.0f);
                GL.End();
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.BindTexture(TextureTarget.Texture2D, texturePauseId);
                GL.Begin(BeginMode.Polygon);
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-0.95f, 0.95f);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(-0.8f, 0.95f);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(-0.8f, 0.8f);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-0.95f, 0.8f);
                GL.End();
                GL.BindTexture(TextureTarget.Texture2D, 0);
                for (int i = 0; i < dxBlocks.Count; i++)
                {
                    GL.BindTexture(TextureTarget.Texture2D, textureBlockId);
                    GL.Begin(BeginMode.Polygon);
                    GL.TexCoord2(new Vector2(0f, 0f));
                    GL.Vertex2(-0.05 * sx + dxBlocks[i], 1f * sy + dyBlocks[i]);
                    GL.TexCoord2(new Vector2(1f, 0f));
                    GL.Vertex2(0.05 * sx + dxBlocks[i], 1f * sy + dyBlocks[i]);
                    GL.TexCoord2(new Vector2(1f, 1f));
                    GL.Vertex2(0.05 * sx + dxBlocks[i], 0.9f * sy + dyBlocks[i]);
                    GL.TexCoord2(new Vector2(0f, 1f));
                    GL.Vertex2(-0.05 * sx + dxBlocks[i], 0.9f * sy + dyBlocks[i]);
                    GL.End();
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
                GL.LoadIdentity();
                for (int i = 0; i < dxBalls.Count; i++)
                {
                    GL.BindTexture(TextureTarget.Texture2D, textureBallId);
                    GL.Begin(BeginMode.Polygon);
                    GL.TexCoord2(new Vector2(0f, 0f));
                    GL.Vertex2(-0.05 + dxBalls[i], 1 + dyBalls[i]);
                    GL.TexCoord2(new Vector2(1f, 0f));
                    GL.Vertex2(0.05 + dxBalls[i], 1 + dyBalls[i]);
                    GL.TexCoord2(new Vector2(1f, 1f));
                    GL.Vertex2(0.05 + dxBalls[i], 0.9 + dyBalls[i]);
                    GL.TexCoord2(new Vector2(0f, 1f));
                    GL.Vertex2(-0.05 + dxBalls[i], 0.9 + dyBalls[i]);
                    GL.End();
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
                GL.BindTexture(TextureTarget.Texture2D, textureBasketId);
                GL.Begin(BeginMode.Polygon);
                GL.TexCoord2(new Vector2(0f, 0f));
                GL.Vertex2(-0.25f + dx, -0.7f + dy);
                GL.TexCoord2(new Vector2(1f, 0f));
                GL.Vertex2(0.25f + dx, -0.7f + dy);
                GL.TexCoord2(new Vector2(1f, 1f));
                GL.Vertex2(0.25f + dx, -1f + dy);
                GL.TexCoord2(new Vector2(0f, 1f));
                GL.Vertex2(-0.25f + dx, -1f + dy);
                GL.End();
                GL.BindTexture(TextureTarget.Texture2D, 0);
                text.Begin();
                text.Print("Score " + score, font, Color.Black, new RectangleF(150, 25, 200, 200), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                text.Print("Missed Balls " + missedBalls, font, Color.Black, new RectangleF(330, 25, 200, 200), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
                text.End();
            }
            
            SwapBuffers();
        }


        static void Main(string[] args)
        {
            Program myGameWin = new Program();
            myGameWin.Run(10);

        }
    }
}
