using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Sharp.Core;

namespace Sharp.Graphics
{
    public class EnvRender : Sharp.Core.Resource
    {
        private class EnvMesh
        {
            public uint vertexBufferOffset;
            public uint indexOffset;
            public uint vertexFormat;
            public uint vertexCount;
            public uint primCount;
            public byte primType;
            public byte vertexStride;
            public byte pad0;
            public byte pad1;
            public VertexDeclaration vertexDeclaration = null;
        }

        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private EnvMesh[] meshList = null;
        private PixelShader pixelShader = null;
        private VertexShader vertexShader = null;
        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer = null;
        private Texture2D texture = null;
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
        public override void Load(System.IO.Stream stream)
        {
            GraphicsDevice device = GraphicsMgr.Instance.Device;
            if (device == null)
                throw new Exception("Invalid graphic device");

            BinaryReader reader = new BinaryReader(stream);

            //read magic number
            uint magicNumber = reader.ReadUInt32();
            
            //read vertex buffer
            uint vbSize = reader.ReadUInt32();
            byte[] vbData = reader.ReadBytes((int)vbSize);
            this.vertexBuffer = new VertexBuffer(device, (int)vbSize, ResourceUsage.WriteOnly, ResourceManagementMode.Automatic);
            this.vertexBuffer.SetData<byte>(vbData);

            //read index buffer
            uint ibSize = reader.ReadUInt32();
            byte[] ibData = new byte[ibSize];
            reader.Read(ibData, 0, (int)ibSize);
            this.indexBuffer = new IndexBuffer(device, (int)ibSize, ResourceUsage.WriteOnly, ResourceManagementMode.Automatic, IndexElementSize.SixteenBits);
            this.indexBuffer.SetData<byte>(ibData);

            //read meshes
            uint meshCount = reader.ReadUInt32();
            this.meshList = new EnvMesh[meshCount];
            for (int i = 0; i < this.meshList.Length; i++)
            {
                EnvMesh mesh = new EnvMesh();

                mesh.vertexBufferOffset = reader.ReadUInt32();
                mesh.indexOffset        = reader.ReadUInt32();
                mesh.vertexFormat       = reader.ReadUInt32();
                mesh.vertexCount        = reader.ReadUInt32();
                mesh.primCount          = reader.ReadUInt32();
                mesh.primType           = reader.ReadByte();
                mesh.vertexStride       = reader.ReadByte();
                mesh.pad0               = reader.ReadByte();
                mesh.pad1               = reader.ReadByte();

                //get vertex declaration for vertex format
                mesh.vertexDeclaration = GraphicsMgr.Instance.GetVertexDeclaration(mesh.vertexFormat);

                //add mesh to list
                this.meshList[i] = mesh;
            }
        }

        public void Initialize(GraphicsDevice device)
        {
            FileStream fileStream = new FileStream("Data\\Envs\\TestLevel.senv", FileMode.Open);
            Load(fileStream);

            CompiledShader compiledShader = ShaderCompiler.CompileFromFile("Data\\Shaders\\Simple.vsh", null, null, CompilerOptions.None, "VertexShaderMain", ShaderProfile.VS_3_0, TargetPlatform.Windows);
            this.vertexShader = new VertexShader(device, compiledShader.GetShaderCode());

            compiledShader = ShaderCompiler.CompileFromFile("Data\\Shaders\\Simple.psh", null, null, CompilerOptions.None, "PixelShaderMain", ShaderProfile.PS_3_0, TargetPlatform.Windows);
            this.pixelShader = new PixelShader(device, compiledShader.GetShaderCode());

            this.texture = (Texture2D)ResourceMgr.Instance.GetResource<Texture>("UV1024");
        }

        public void Render(GraphicsDevice device)
        {
            device.PixelShader = this.pixelShader;
            device.VertexShader = this.vertexShader;

            device.SetVertexShaderConstant((int)GraphicsMgr.VSConstant.WorldMatrix, Matrix.Identity);
            device.SetVertexShaderConstant((int)GraphicsMgr.VSConstant.WorldInvTransMatrix, Matrix.Identity);

            Vector4 lightDir = new Vector4(-0.5f, -0.5f, 0.0f, 1.0f);
            lightDir.Normalize();
            lightDir = -lightDir;
            device.SetPixelShaderConstant(1, lightDir);

            Vector4 ambient = new Vector4(0.0f, 0.0f, 0.0f, 0.2f);
            device.SetPixelShaderConstant(0, ambient);

            Vector4 dirLightColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            device.SetPixelShaderConstant(2, dirLightColor);

            device.Textures[0] = this.texture;
            device.SamplerStates[0].MinFilter = TextureFilter.Linear;
            device.SamplerStates[0].MagFilter = TextureFilter.Linear;
            device.SamplerStates[0].MipFilter = TextureFilter.Linear;

            device.RenderState.DepthBufferEnable = true;
            device.RenderState.CullMode = CullMode.CullClockwiseFace;
            device.RenderState.DepthBufferWriteEnable = true;
            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.SourceBlend = Blend.One;
            device.RenderState.DestinationBlend = Blend.Zero;
            device.RenderState.ColorWriteChannels = ColorWriteChannels.All;
            device.RenderState.AlphaTestEnable = false;

            //draw all the meshes
            device.Indices = this.indexBuffer;
            for (int i = 0; i < this.meshList.Length; i++)
            {
                EnvMesh mesh = this.meshList[i];
                device.Vertices[0].SetSource(this.vertexBuffer, (int)mesh.vertexBufferOffset, (int)mesh.vertexStride);
                device.VertexDeclaration = mesh.vertexDeclaration;
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (int)mesh.vertexCount, (int)mesh.indexOffset, (int)mesh.primCount);
            }
        }
        #endregion
    }
}
