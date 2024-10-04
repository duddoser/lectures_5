using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Directx_Textures
{
    public partial class Form1 : Form
    {
        private Device device = null;
        private VertexBuffer vb = null;
        private float angle = 0f;
        private CustomVertex.PositionNormalTextured[] vertices;
        private IndexBuffer ib = null;
        private int[] indices;
        Bitmap b = null;
        Texture tex1 = null;

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

            device.RenderState.Lighting = true;
 

            device.RenderState.CullMode = Cull.CounterClockwise;

            b = (Bitmap)Image.FromFile("Logo.bmp");
            tex1 = new Texture(device, b, 0, Pool.Managed);
        }

        public void CameraPositioning()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1f, 50f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(30f, 12f, 30f),
                                        new Vector3(0, 0, 0),
                                        new Vector3(0, 1, 0));

            //настройка освещения
            device.Lights[0].Enabled = true;
            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.Aqua;
            device.Lights[0].Direction = new Vector3(-1, -1, -1);

            device.Lights[1].Enabled = true;
            device.Lights[1].Type = LightType.Directional;
            device.Lights[1].Ambient = Color.White;
            device.Lights[1].Direction = new Vector3(0, 1, 0);
        }

        public void VertexDeclaration()
        {

            vertices = new CustomVertex.PositionNormalTextured[48];

            vb = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 48, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

            //нижние вершины
            vertices[0] = new CustomVertex.PositionNormalTextured(0f, 0f, 0f, 0f, -1f, 0f, 0f, 1f);
            vertices[1] = new CustomVertex.PositionNormalTextured(5f, 0f, 0f, 0f, -1f, 0f, 1f, 1f);
            vertices[2] = new CustomVertex.PositionNormalTextured(5f, 0f, 5f, f, -1f, 0f, 1f, 0f);
            vertices[3] = new CustomVertex.PositionNormalTextured(0f, 0f, 5f, 0f, -1f, 0f, 0f, 0f);

            //верхние
            vertices[4] = new CustomVertex.PositionNormalTextured(0f, 5f, 0f, 0f, 1f, 0f, 0f, 0f);
            vertices[5] = new CustomVertex.PositionNormalTextured(5f, 5f, 0f, 0f, 1f, 0f, 1f, 0f);
            vertices[6] = new CustomVertex.PositionNormalTextured(5f, 5f, 5f, 0f, 1f, 0f, 1f, 1f);
            vertices[7] = new CustomVertex.PositionNormalTextured(0f, 5f, 5f, 0f, 1f, 0f, 0f, 1f);

            //левые
            vertices[8] = new CustomVertex.PositionNormalTextured(0f, 5f, 5f, -1f, 0f, 0f, 1f, 0f);
            vertices[9] = new CustomVertex.PositionNormalTextured(0f, 5f, 0f, -1f, 0f, 0f, 0f, 0f);
            vertices[10] = new CustomVertex.PositionNormalTextured(0f, 0f, 0f, -1f, 0f, 0f, 0f, 1f);
            vertices[11] = new CustomVertex.PositionNormalTextured(0f, 0f, 5f, -1f, 0f, 0f, 1f, 1f);

            //правые
            vertices[12] = new CustomVertex.PositionNormalTextured(5f, 5f, 5f, 1f, 0f, 0f, 0f, 0f);
            vertices[13] = new CustomVertex.PositionNormalTextured(5f, 5f, 5f, 1f, 0f, 0f, 1f, 0f);
            vertices[14] = new CustomVertex.PositionNormalTextured(5f, 0f, 0f, 1f, 0f, 0f, 1f, 1f);
            vertices[15] = new CustomVertex.PositionNormalTextured(5f, 0f, 5f, 1f, 0f, 0f, 0f, 1f);

            //передние
            vertices[16] = new CustomVertex.PositionNormalTextured(0f, 5f, 5f, 0f, 0f, 1f, 0f, 0f);
            vertices[17] = new CustomVertex.PositionNormalTextured(0f, 0f, 5f, 0f, 0f, 1f, 1f, 0f);
            vertices[18] = new CustomVertex.PositionNormalTextured(5f, 0f, 5f, 0f, 0f, 1f, 1f, 1f);
            vertices[19] = new CustomVertex.PositionNormalTextured(5f, 5f, 5f, 0f, 0f, 1f, 0f, 1f);

            //задние
            vertices[20] = new CustomVertex.PositionNormalTextured(0f, 5f, 0f, 0f, 0f, -1f, 1f, 0f);
            vertices[21] = new CustomVertex.PositionNormalTextured(5f, 5f, 0f, 0f, 0f, -1f, 0f, 0f);
            vertices[22] = new CustomVertex.PositionNormalTextured(5f, 0f, 0f, 0f, 0f, -1f, 0f, 1f);
            vertices[23] = new CustomVertex.PositionNormalTextured(0f, 0f, 0f, 0f, 0f, -1f, 1f, 1f);

            indices = new int[]{
            //описать массив индексов
            0,1,3,
            3,1,2,
            5,4,6,
            4,7,6,
            8,9,10,
            8,10,11,
            12,13,15,
            13,14,15,
            19,17,18,
            19,16,17,
            20,23,21,
            23,22,21
            };

            for (int i = 24; i < 48; ++i) {
                vertices[i] = new CustomVertex.PositionNormalTextured(vartices[i - 24].X + 2, vertices[i - 24].Y + 2, vertices[i - 24].Z + 2, 
                vertices[i - 24].Nx, vertices[i - 24].Ny, vertices[i - 24].Nz, vertices[i - 24].Tu, vertices[i - 24].Tv)
            }



            ib = new IndexBuffer(typeof(int), indices.Length, device,
                     Usage.WriteOnly, Pool.Default);

            //сброс данных о вершинах и индексах в графическое устройство
            ib.SetData(indices, 0, LockFlags.None);

            vb.SetData(vertices, 0, LockFlags.None);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target, Color.DarkSlateBlue, 1.0f, 0);

            device.BeginScene();
            device.VertexFormat = CustomVertex.PositionNormalTextured.Format;

            //сохраняем информацию о вершинах, текстуре и индексах
            device.SetStreamSource(0, vb, 0);
            device.SetTexture(0, tex1);
            device.Indices = ib;

            Material M = new Material();
            M.Diffuse = Color.Aqua;
            M.Ambient = Color.Beige;
            device.Material = M;

            device.Transform.World = Matrix.RotationX(angle) * Matrix.RotationY(2 * angle) * Matrix.RotationZ(3 * angle);
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 24, 0, 12);

            device.EndScene();

            device.Present();

            this.Invalidate();
            angle += 0.005f;
        }
    }
}
