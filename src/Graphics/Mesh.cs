using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Sharp.Core;

namespace Sharp.Graphics
{
    public class Mesh
    {
        [Serializable]
        private struct Vertex
        {
            public Vector3 pos;
            public Vector3 normal;
        }

        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private BoundingBox bbox = new BoundingBox();
        private PrimitiveType primType = PrimitiveType.TriangleList;
        private int vertexCount = 0;
        private int primCount = 0;
        private int vertexStride = 0;
        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer = null;
        private VertexDeclaration vertexDeclaration = null;
        private Material material = null;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/

        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public void Load(System.IO.Stream stream)
        {
            GraphicsDevice device = GraphicsMgr.Instance.Device;
            if (device == null)
                throw new Exception("Invalid graphic device");

            BinaryReader reader = new BinaryReader(stream);

            //read vertex format
            uint vertexFormat = reader.ReadUInt32();
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
                new VertexElement(0, 12, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0)
            };

            this.vertexDeclaration = new VertexDeclaration(device, elements);

            this.primType = PrimitiveType.TriangleList;
            this.vertexStride = 24;

            //read vertex buffer
            this.vertexCount = (int)reader.ReadUInt32();

            //create our vertex buffer
            Vertex[] vertices = new Vertex[vertexCount];
            for (int i = 0; i < this.vertexCount; i++)
            {
                //read position
                vertices[i].pos.X = reader.ReadSingle();
                vertices[i].pos.Y = reader.ReadSingle();
                vertices[i].pos.Z = reader.ReadSingle();

                vertices[i].normal.X = reader.ReadSingle();
                vertices[i].normal.Y = reader.ReadSingle();
                vertices[i].normal.Z = reader.ReadSingle();
            }

            this.vertexBuffer = new VertexBuffer(device, typeof(Vertex), vertexCount, ResourceUsage.WriteOnly, ResourceManagementMode.Automatic);
            this.vertexBuffer.SetData<Vertex>(vertices);

            //read index buffer
            int indexCount = (int)reader.ReadUInt32();
            ushort[] indices = new ushort[indexCount];
            for (int i = 0; i < indexCount; i++)
                indices[i] = reader.ReadUInt16();

            this.primCount = indexCount / 3;

            this.indexBuffer = new IndexBuffer(device, typeof(ushort), indexCount, ResourceUsage.WriteOnly, ResourceManagementMode.Automatic);
            this.indexBuffer.SetData<ushort>(indices);            

            //base.Load(stream);
        }

        public void Render(GraphicsDevice device)
        {
            device.VertexDeclaration = this.vertexDeclaration;
            device.Vertices[0].SetSource(this.vertexBuffer, 0, this.vertexStride);
            device.Indices = this.indexBuffer;
            device.DrawIndexedPrimitives(this.primType, 0, 0, this.vertexCount, 0, this.primCount);
        }
        #endregion
    }
}
