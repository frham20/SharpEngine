using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Sharp.Entities;
using Sharp.Graphics;

namespace Sharp.Core
{
    public class GraphicsMgr : Microsoft.Xna.Framework.GameComponent
    {
        public class VFormat
        {
            public const uint Position = 0x1;
            public const uint Normal   = 0x2;
            public const uint Color    = 0x4;
            public const uint Tangent  = 0x8;
            public const uint TexCoordShift = 24;
            public const uint TexCoordMask = 0xFF000000;
        }

        public enum VSConstant
        {
            ViewProjMatrix = 0,
            WorldMatrix = 4,
            WorldInvTransMatrix = 8,
            ViewInvMatrix = 12
        }

        public enum PSConstant
        {
            None
        }

        #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private static GraphicsMgr sInstance = null;
        private GraphicsDeviceManager deviceMgr = null;
        private GraphicsDevice device = null;
        private Camera camera = null;
        private Matrix projMatrix = new Matrix();
        private Matrix viewMatrix = new Matrix();
        private Matrix viewProjMatrix = new Matrix();
        private Matrix invViewMatrix = new Matrix();
        private Color clearColor = new Color(33, 33, 33, 255);
        private Viewport viewport = new Viewport();
        private EnvRender envRender = new EnvRender();
        private Dictionary<uint, VertexDeclaration> vertexDeclarationMap = new Dictionary<uint, VertexDeclaration>();
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public static GraphicsMgr Instance
        {
            get { return sInstance; }
        }

        public Camera Camera
        {
            get 
            { 
                return this.camera; 
            }

            set 
            {
                if (this.camera != null)
                    this.camera.Active = false;

                this.camera = value;

                if(this.camera != null)
                    this.camera.Active = true;
            }
        }

        public Color ClearColor
        {
            get { return this.clearColor; }
            set { this.clearColor = value; }
        }

        public GraphicsDevice Device
        {
            get { return device; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public GraphicsMgr(Game game)
            : base(game)
        {
        }

        public static GraphicsMgr Create(Game game)
        {
            sInstance = new GraphicsMgr(game);
            return sInstance;
        }

        public override void Initialize()
        {
            this.deviceMgr = ((EngineMgr)this.Game).GraphicsDeviceMgr;
            this.device = this.deviceMgr.GraphicsDevice;

            this.Game.Window.ClientSizeChanged += new EventHandler(OnClientSizeChanged);
            OnClientSizeChanged(this.Game.Window, EventArgs.Empty);

            envRender.Initialize(this.device);

            base.Initialize();
        }

        public void OnClientSizeChanged(object sender, EventArgs args)
        {
            //reset the viewport
            this.viewport.X = 0;
            this.viewport.Y = 0;
            this.viewport.Width = this.Game.Window.ClientBounds.Width;
            this.viewport.Height = this.Game.Window.ClientBounds.Height;
            this.viewport.MinDepth = 0.0f;
            this.viewport.MaxDepth = 1.0f;

            if (this.device != null)
                this.device.Viewport = this.viewport;
        }

        public override void Update(GameTime gameTime)
        {
            //no camera? nothing to do
            if (this.camera == null || this.device == null)
            {
                base.Update(gameTime);
                return;
            }

            Rectangle wndRect = this.Game.Window.ClientBounds;
            float aspectRatio = (float)wndRect.Width / (float)wndRect.Height;

            //setup projection matrix
            this.projMatrix = Matrix.CreatePerspectiveFieldOfView(this.camera.HFOV, 
                                                                    aspectRatio, 
                                                                    this.camera.DistanceNear, 
                                                                    this.camera.DistanceFar);

            //setup view and inv view matrix
            Matrix.Invert(ref this.camera.worldXForm, out this.viewMatrix);

            //calc view proj matrix
            Matrix.Multiply(ref this.viewMatrix, ref this.projMatrix, out this.viewProjMatrix);

            //clear screen
            this.device.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, this.clearColor, 1.0f, 0);

            this.device.SetVertexShaderConstant((int)VSConstant.ViewProjMatrix, this.viewProjMatrix);

            //render environnement
            this.envRender.Render(this.device);

            base.Update(gameTime);

        }

        public VertexDeclaration GetVertexDeclaration(uint vertexFormat)
        {
            VertexDeclaration vd = null;
            if (this.vertexDeclarationMap.TryGetValue(vertexFormat, out vd))
                return vd;

            //not in map so create new vertex declaration
            //find number of elements
            uint elemCount = 0;
            if ((vertexFormat & VFormat.Position) != 0) elemCount++;
            if ((vertexFormat & VFormat.Normal) != 0)   elemCount++;
            if ((vertexFormat & VFormat.Color) != 0)    elemCount++;
            if ((vertexFormat & VFormat.Tangent) != 0)  elemCount++;

            uint texCoordCount = (uint)((int)(vertexFormat & VFormat.TexCoordMask) >> (int)VFormat.TexCoordShift);
            elemCount += texCoordCount;

            if (elemCount == 0)
                throw new Exception("wow no elements in vertex format!");

            VertexElement[] elements = new VertexElement[elemCount];

            uint curElem = 0;
            short curStride = 0;
            if ((vertexFormat & VFormat.Position) != 0)
            {
                elements[curElem++] = new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0);
                curStride += 12;
            }

            if ((vertexFormat & VFormat.Normal) != 0)
            {
                elements[curElem++] = new VertexElement(0, curStride, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0);
                curStride += 12;
            }

            if ((vertexFormat & VFormat.Color) != 0)
            {
                elements[curElem++] = new VertexElement(0, curStride, VertexElementFormat.Rgba32, VertexElementMethod.Default, VertexElementUsage.Color, 0);
                curStride += 4;
            }

            if ((vertexFormat & VFormat.Tangent) != 0)
            {
                elements[curElem++] = new VertexElement(0, curStride, VertexElementFormat.Vector4, VertexElementMethod.Default, VertexElementUsage.Tangent, 0);
                curStride += 16;
            }

            for (int i = 0; i < texCoordCount; i++)
            {
                elements[curElem++] = new VertexElement(0, curStride, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, (byte)i);
                curStride += 8;
            }

            vd = new VertexDeclaration(this.device, elements);

            //add to map
            this.vertexDeclarationMap.Add(vertexFormat, vd);

            return vd;
        }
        #endregion
    }
}
