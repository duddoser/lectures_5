using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Microsoft.VisualC;

namespace Directx_Light
{
    public partial class Form1 : Form
    {
        private Device device = null;
        private VertexBuffer vb = null;
        private float angle = 0f;
        private CustomVertex.PositionNormalColored[] vertices;
        private IndexBuffer ib = null;
        private int[] indices;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            InitializeDevice();
            VertexDeclaration();
            CameraPositioning();
        }

        public void InitializeDevice()
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;

            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);

            //включаем режим освещения и обработку невидимых граней
            device.RenderState.CullMode = Cull.CounterClockwise;
        }

        public void CameraPositioning()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, (float)this.Width / this.Height, 1f, 50f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(20, -10, 20),
                                        new Vector3(0, 0, 0),
                                        new Vector3(0, 1, 0));
            device.RenderState.Lighting =true;
            device.RenderState.CullMode = Cull.None;

            device.Lights[0].Enabled = true;
            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.Green;
            device.Lights[0].Direction = nev Vector3(-1f, -1f, -1f);

        public void VertexDeclaration()
        {
            vb = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), 5, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalColored.Format, Pool.Default);

            //вершины содержат координаты, нормаль и цвет

            vb = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), 5, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalColored.Format, Pool.Default);
            vertices = new CustomVertex.PositionNormalColored[5];
            vertices[0] = new CustomVertex.PositionNormalColored(0f, 0f, 0f, -1f, 0f, 1f, Color.Cyan.ToArgb());
            vertices[1] = new CustomVertex.PositionNormalColored(0f, 5f, 0f, -1f, 1f, 1f, Color.Cyan.ToArgb());
            vertices[2] = new CustomVertex.PositionNormalColored(5f, 5f, 0f, 1f, 0f, 1f, Color.Blue.ToArgb());
            vertices[3] = new CustomVertex.PositionNormalColored(5f, 0f, 0f, 1f, -1f, 1f, Color.Red.ToArgb());
            vertices[4] = new CustomVertex.PositionNormalColored(3f, 3f, 3f, 0f, 0f, 1f, Color.Green.ToArgb());

            vb.SetData(vertices, 0, LockFlags.None);

            //индексный буфер показывает, как вершины объединить в треугольники

            ib = new IndexBuffer(typeof(int), 18, device, Usage.WriteOnly, Pool.Default);
            indices = new int[]
            {
                // bottom
                0, 1, 3,
                3, 1, 2,
                // front
                2, 4, 3,
                2, 1, 4,
                // zadnie bokovye
                0, 3, 4
                0, 4, 1

            };

            //заполнить массив индексов так, чтобы вершины выстроились в треугольники 

            ib.SetData(indices, 0, LockFlags.None);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target, Color.DarkSlateBlue, 1.0f, 0);

            device.BeginScene();
            device.VertexFormat = CustomVertex.PositionNormalColored.Format;

            //установка вершин и индексов, показывающих как из них построить поверхность
            device.SetStreamSource(0, vb, 0);
            device.Indices = ib;

            //материал показываем, как свет взаимодействует с объектом
            //Material M = new Material();
            //M.Diffuse = Color.Red;
            //M.Emissive = Color.Red;
            //доработать свойства материала
            //device.Material = M;

            device.Transform.World = Matrix.RotationX(angle) * Matrix.RotationY(2 * angle) * Matrix.RotationZ(3 * angle);
        
            device.Indices = ib;

            device.SetStreamSource
            //отрисовка индексированных фигур
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 5, 0, 6);
            
            device.EndScene();

            device.Present();

            this.Invalidate();
            angle += 0.02f;
        }
    }
}
