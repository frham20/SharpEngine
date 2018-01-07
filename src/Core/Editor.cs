using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Sharp.Entities;

namespace Sharp.Core
{
    public class Editor : Microsoft.Xna.Framework.GameComponent
    {
         #region Variables
        /************************************************************************/
        /* Variables                                                            */
        /************************************************************************/
        private static Editor sInstance = null;
        private EditorCamera camera = null;
        private Camera oldCamera = null;
        private Form mainForm = new Form();
        private TabControl tabCtrl = new TabControl();
        private PropertyGrid propGrid = new PropertyGrid();
        private TreeView treeView = new TreeView();
        private SplitContainer splitContainer = new SplitContainer();
        private bool editorKeyPressed = false;
        #endregion

        #region Properties
        /************************************************************************/
        /* Properties                                                           */
        /************************************************************************/
        public static Editor Instance
        {
            get { return sInstance; }
        }
        #endregion

        #region Methods
        /************************************************************************/
        /* Methods                                                              */
        /************************************************************************/
        public Editor(Game game)
            : base(game)
        {
        }

        public static Editor Create(Game game)
        {
            sInstance = new Editor(game);
            return sInstance;
        }

        public override void Initialize()
        {
            base.Initialize();
         
            //init editor form
            this.mainForm.Text = "Sharp Editor v0.1";
            this.mainForm.Size = new System.Drawing.Size(400,600);
            this.mainForm.MaximizeBox = false;
            this.mainForm.MinimizeBox = false;
            this.mainForm.Activated += new EventHandler(OnActivated);
            this.mainForm.Deactivate += new EventHandler(OnDeactivate);

            this.mainForm.Menu = new MainMenu();
            this.mainForm.Menu.MenuItems.Add("File");
            this.mainForm.Menu.MenuItems[0].MenuItems.Add("Close", new EventHandler(OnMenuFileClose));
            this.mainForm.Menu.MenuItems.Add("View");
            this.mainForm.Menu.MenuItems.Add("Help");
            this.mainForm.Menu.MenuItems[2].MenuItems.Add("About", new EventHandler(OnMenuHelpAbout));

            this.tabCtrl.Dock = DockStyle.Fill;
            this.tabCtrl.TabPages.Add("Entities");
            this.tabCtrl.TabPages.Add("Models");
            this.tabCtrl.TabPages.Add("Textures");
            this.tabCtrl.TabPages.Add("Sounds");
            this.mainForm.Controls.Add(this.tabCtrl);

            //split container
            this.splitContainer.Dock = DockStyle.Fill;
            this.tabCtrl.TabPages[0].Controls.Add(this.splitContainer);

            //tree view
            this.treeView.HideSelection = false;
            this.treeView.Dock = DockStyle.Fill;
            this.treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(OnTreeViewNodeMouseClick);
            this.splitContainer.Panel1.Controls.Add(this.treeView);

            //property grid
            this.propGrid.Dock = DockStyle.Fill;
            this.splitContainer.Panel2.Controls.Add(this.propGrid);

            this.splitContainer.SplitterDistance = (this.splitContainer.Size.Width / 2) - 40;

            //create our camera
            this.camera = new EditorCamera();
            this.camera.Name = "EditorCamera";
            this.camera.ID = 0xf0000000;
            this.camera.Active = false;
            EntityMgr.Instance.AddEntity(this.camera);
        }

        public void OnActivated(object sender, EventArgs e)
        {
            InputMgr.Instance.CaptureMouse = false;
            this.Game.IsMouseVisible = true;
        }

        public void OnDeactivate(object sender, EventArgs e)
        {
            //InputMgr.Instance.CaptureMouse = true;
            //this.Game.IsMouseVisible = false;
        }

        public void OnMenuFileClose(object sender, EventArgs args)
        {
            this.mainForm.Hide();
            this.editorKeyPressed = false;
        }

        public void OnMenuHelpAbout(object sender, EventArgs args)
        {

        }

        public void FillTreeView()
        {
            //fill list ctrl with all the entities there are
            List<Type> entTypes = new List<Type>(10);
            this.treeView.Nodes.Clear();
            EntityMgr entityMgr = EntityMgr.Instance;
            foreach (KeyValuePair<uint, Entity> pair in entityMgr.entityMap)
            {
                Entity entity = pair.Value;

                //find all the types
                entTypes.Clear();
                Type type = entity.GetType();
                while (type != typeof(Entity))
                {
                    entTypes.Add(type);
                    type = type.BaseType;
                }

                //find class nodes
                TreeNodeCollection curNodes = this.treeView.Nodes;
                for (int i = entTypes.Count - 1; i >= 0; i--)
                {
                    TreeNode[] nodes = curNodes.Find(entTypes[i].Name, false);
                    if (nodes.Length == 0)
                    {
                        //node doesn't exists so create it
                        TreeNode treeNode = new TreeNode(entTypes[i].Name);
                        treeNode.Name = entTypes[i].Name;                        
                        curNodes.Add(treeNode);
                        curNodes = treeNode.Nodes;
                    }
                    else
                    {
                        curNodes = nodes[0].Nodes;
                    }                    
                }

                TreeNode entityNode = new TreeNode(entity.Name);
                entityNode.Tag = entity;
                curNodes.Add(entityNode);                
            }
        }

        public void OnTreeViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs args)
        {
            this.propGrid.SelectedObject = args.Node.Tag;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                this.editorKeyPressed = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                if (this.editorKeyPressed)
                {
                    this.editorKeyPressed = false;

                    if (this.mainForm.Visible)
                    {
                        this.mainForm.Hide();
                        InputMgr.Instance.CaptureMouse = true;

                        GraphicsMgr gm = GraphicsMgr.Instance;
                        gm.Device.RenderState.FillMode = Microsoft.Xna.Framework.Graphics.FillMode.Solid;

                        //restore previous camera
                        gm.Camera = this.oldCamera;
                    }
                    else
                    {
                        GraphicsMgr gm = GraphicsMgr.Instance;
                        //retrieve current camera
                        this.oldCamera = gm.Camera;

                        //set editor camera to position and orientation of current camera
                        this.camera.xform = this.oldCamera.worldXForm;

                        gm.Camera = this.camera;
                        gm.Device.RenderState.FillMode = Microsoft.Xna.Framework.Graphics.FillMode.WireFrame;

                        FillTreeView();
                        this.mainForm.Show();
                        InputMgr.Instance.CaptureMouse = false;
                    }
                }                
            }


            base.Update(gameTime);
        }
        #endregion
    }
}
