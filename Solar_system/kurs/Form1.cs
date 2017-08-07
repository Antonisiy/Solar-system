using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using OpenTK.Platform.Windows;


namespace One.Space
{
    public partial class Form1 : Form
    {

        double lastMouseX,lastAngleZ,lastMouseY,lastPosX, lastPosY;

        bool pressed = false;
        double AngleY = 0, AngleZ = 0, Z = 0;

        Galaxy milky_way = new Galaxy();

        Timer timer;

        double angle = 0;
        double angle_os = 0;

        const double angle_c = 0.03;
        const double Step = 0.2;
        const double AngleDl = 5;
        Galaxy.PointFloat Pos;


        public Form1()
        {
            InitializeComponent();

            milky_way.Pictures[0] = new Bitmap("tex/Space.bmp");
            milky_way.Pictures[1] = new Bitmap("tex/sun.bmp");
            milky_way.Pictures[2] = new Bitmap("tex/merkur.png");
            milky_way.Pictures[3] = new Bitmap("tex/venus.png");
            milky_way.Pictures[4] = new Bitmap("tex/Earth.png");
            milky_way.Pictures[5] = new Bitmap("tex/MarsNew3.bmp");
            milky_way.Pictures[6] = new Bitmap("tex/jupiter.png");
            milky_way.Pictures[7] = new Bitmap("tex/uran.png");
            milky_way.Pictures[8] = new Bitmap("tex/Neptun.png");
            milky_way.Pictures[9] = new Bitmap("tex/pluto.png");
        }

      

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {          
            // очистка буферов цвета и глубины
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // поворот изображения
            GL.LoadIdentity();
            GL.Rotate(AngleZ, 0.0, 1.0, 0.0);
            GL.Translate(Pos.X, Pos.Y, Z);


            milky_way.DrawSpace(milky_way.Pictures[0], angle_os);

            milky_way.DrawSun(milky_way.Pictures[1], angle_os);

            milky_way.DrawPlanet(milky_way.Pictures[2], angle/2, 2.7 , 0.114);

            milky_way.DrawPlanet(milky_way.Pictures[3], angle/3, 3.5, 0.295);

            milky_way.DrawPlanet(milky_way.Pictures[4], angle/4, 4.5, 0.3);

            milky_way.DrawPlanet(milky_way.Pictures[5], angle / 5, 5.5, 0.174);

            milky_way.DrawPlanet(milky_way.Pictures[6], angle / 6, 8, 1);

            milky_way.DrawPlanet(milky_way.Pictures[7], angle / 7, 10, 0.8);

            milky_way.DrawPlanet(milky_way.Pictures[8], angle / 8, 12, 0.7);

            milky_way.DrawPlanet(milky_way.Pictures[9], angle / 9, 15, 0.05);

            GL.Flush();
            GL.Finish();

            glControl1.SwapBuffers();

        }

        private void glControl1_Load(object sender, EventArgs e)
        {

            GL.Enable(EnableCap.DepthTest);
             float[] light2_diffuse = { 0.9f, 0.9f, 1.0f };
            float[] light2_position = { 0.0f, 0.0f, 1.0f, 1.0f };
            GL.Enable(EnableCap.Light2);

            GL.Light(LightName.Light2, LightParameter.Diffuse, light2_diffuse);

            GL.Light(LightName.Light2, LightParameter.Position, light2_position);

            GL.Light(LightName.Light2, LightParameter.ConstantAttenuation, 0.01f);

            GL.Light(LightName.Light2, LightParameter.LinearAttenuation, 0.000009f);
 
            GL.Light(LightName.Light2, LightParameter.QuadraticAttenuation, 0.00009f);
  
         
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


            Z = -7.6;
            Pos.Y = -1.4;

            timer = new Timer();
            timer.Interval = 35;
            timer.Tick += moveOb;
            timer.Enabled = true;

           

        }

        private void moveOb(object sender, EventArgs e)
        {
            angle += angle_c / 5;
            angle_os += angle_c / 150;
            glControl1.Invalidate();
        }


      
        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)        {
                case Keys.A:
                    AngleZ += -AngleDl;
                    break;
                case Keys.D:
                    AngleZ -= -AngleDl;
                    break;
                case Keys.W:
                    Pos.X = Pos.X - Step * Math.Sin(AngleZ * Math.PI / 180);
                    Z = Z + Step * Math.Cos(AngleZ * Math.PI / 180);
                    break;
                case Keys.S:
                    Pos.X = Pos.X + Step * Math.Sin(AngleZ * Math.PI / 180);
                    Z = Z - Step * Math.Cos(AngleZ * Math.PI / 180);
                    break;
                case Keys.I:
                    Pos.Y = Pos.Y - Step;
                    //* Math.Sin(AngleZ * Math.PI / 180);
                    //Z = Z + Step * Math.Cos(AngleZ * Math.PI / 180);
                    break;
                case Keys.K:
                    Pos.Y = Pos.Y + Step;
                    //* Math.Sin(AngleZ * Math.PI / 180);
                    //Z = Z + Step * Math.Cos(AngleZ * Math.PI / 180);
                    break;

            }

            if (AngleY >= 360)
                AngleY -= 360;

            glControl1.Invalidate(); 

        }

        private void glControl1_Resize(object sender, EventArgs e)
        {

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-0.5, 0.5, -0.5, 0.5, 0.5, 50);
            GL.MatrixMode(MatrixMode.Modelview);
            glControl1.Invalidate();

        }

        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pressed = true;
            lastMouseX = e.X;
            lastMouseY = e.Y;
            lastAngleZ = AngleZ;
            lastPosX = Pos.X;
            lastPosY = Pos.Y;
        }

        private void glControl1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pressed)
            {
                AngleZ = lastAngleZ - (e.X - lastMouseX) / 5;
                Pos.X = lastPosX + (e.Y - lastMouseY) / 50 * Math.Sin(AngleZ * Math.PI / 180);
                Pos.Y = lastPosY - (e.Y - lastMouseY) / 50 * Math.Cos(AngleZ * Math.PI / 180);
                glControl1.Invalidate();
            }
        }

        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pressed = false;
        }
    }

    class Galaxy
    {
        const double hdl = 50;

        private Bitmap[] pictures = new Bitmap[10];
        public struct Wall
        {
            public int L;
            public int R;
        }
        public struct PointFloat
        {
            public double X;
            public double Y;
        }

        public PointFloat[] coners = new[]{
            new PointFloat{X = -33, Y = -33},
            new PointFloat{X = -33, Y = 33},
            new PointFloat{X =  33, Y = 33},
            new PointFloat{X =  33, Y = -33},
            new PointFloat{X =  0, Y = 33},
            new PointFloat{X =  33, Y = 0}
        };

        public Wall[] plan = new[]
        {
            new Wall{L = 0, R = 1},
            new Wall{L = 1, R = 2},
            new Wall{L = 2, R = 3},
            new Wall{L = 3, R = 0},


        };

        public Bitmap[] Pictures
        {
            get
            {
                return pictures;
            }

            set
            {
                pictures = value;
            }
        }

        public void DrawSun(Bitmap pic, double angle_os) {

            GLTexture.LoadTexture(pic);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);
            GL.PushMatrix();
            GL.Rotate(angle_os, 1.0, 1.0, 1.0);
            Sphere(2.5, 20, 20, 0, 0, 0, angle_os, true);
            GL.PopMatrix();
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);

        }

        public void DrawPlanet(Bitmap pic, double angle, double k =1.5, double radius = 0.2, double param = 225, int nx = 20, int ny = 20)
        {
          
            float a = 0f;
            GLTexture.LoadTexture(pic);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);
            GL.PushMatrix();
            GL.Rotate(param, 1.0, 1.0, 1.0);
          
            Sphere(radius, nx, ny, Math.Sin(angle) * k, Math.Cos(angle) * k, 0, angle, true);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex2(0.0f, 0.0f);
            for (int i = 0; i <= 50; i++)
            {
                a = (float)i / 50.0f * 3.1415f * 2.0f;
                GL.Vertex2(Math.Sin(a) * k, Math.Cos(a) * k);
            }
            GL.End();
            GL.PopMatrix();
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);

            

        }


        public void DrawSpace(Bitmap pic, double angle_os)
        {
            GLTexture.LoadTexture(pic);
            GL.Enable(EnableCap.Texture2D);
            Sphere(30, 200, 200, 0, 0, 0, angle_os, true);
            GL.Disable(EnableCap.Texture2D);


        }
        public static void Sphere(double r, int nx, int ny, double sx, double sy, double sz, double angle, bool rotate_texture = false)
        {
            int ix, iy;
            double x, y, z, tex_x, tex_y;


            for (iy = 0; iy < ny; ++iy)
            {
                tex_y = (double)iy / (double)ny;

                GL.Begin(PrimitiveType.QuadStrip);
                for (ix = 0; ix <= nx; ++ix)
                {
                    tex_x = (double)ix / (double)nx + (rotate_texture ? angle - Math.Floor(angle) : 0);

                    x = r * Math.Sin(iy * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx) + sx;
                    y = r * Math.Sin(iy * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx) + sy;
                    z = r * Math.Cos(iy * Math.PI / ny) + sz;
                    GL.Normal3(x, y, z);//нормаль направлена от центра
                    GL.TexCoord2(tex_x, tex_y);
                    GL.Vertex3(x, y, z);

                    x = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx) + sx;
                    y = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx) + sy;
                    z = r * Math.Cos((iy + 1) * Math.PI / ny) + sz;
                    GL.Normal3(x, y, z);
                    GL.TexCoord2(tex_x, tex_y + 1.0 / (double)ny);
                    GL.Vertex3(x, y, z);
                }
                GL.End();
            }
        }
    }
}
