using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WinFormsGraphicsDevice;
using System.IO;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using NSVectangle;
using System.ComponentModel;

namespace DragonUI
{
    enum HandleMode
    {
        None,
        Location,
        Size,
    }

    struct HandleRect
    {
        public Vectangle vectangle;
        public HandleMode xMode;
        public HandleMode yMode;

        public HandleRect(Vectangle aVectangle, HandleMode aXMode, HandleMode aYMode)
        {
            vectangle = aVectangle;
            xMode = aXMode;
            yMode = aYMode;
        }
    }

    class DragonUIFrame
    {
        [CategoryAttribute("Page Size")]
        public int Width { get; set; }

        [CategoryAttribute("Page Size")]
        public int Height { get; set; }

        public Texture2D texture;

        public DragonUIFrame(int aWidth, int aHeight, Texture2D aTexture)
        {
            Width = aWidth;
            Height = aHeight;
            texture = aTexture;
        }

        public DragonUIFrame(JSONArray template, Texture2D aTexture)
        {
            Width = template.getInt(0);
            Height = template.getInt(1);
            texture = aTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(0, 0, Width, Height), Color.White);
        }

        public Point GetWindowOffset(int windowWidth, int windowHeight)
        {
            return new Point((windowWidth - Width) / 2, (windowHeight - Height) / 2);
        }
    }

    class DragonUIViewport
    {
        // Where to draw the DragonUI system on screen - scale, and XY coordinate of the origin in screenspace
        public int X;
        public int Y;
        public float scale;

        public DragonUIViewport()
        {
            Reset();
        }

        public void Reset()
        {
            X = 0;
            Y = 0;
            scale = 1;
        }

        public Matrix getViewMatrix()
        {
            return new Matrix(
                scale, 0, 0, 0,
                0, scale, 0, 0,
                0, 0, scale, 0,
                X, Y, 0, 1);
        }

        public Vector2 ClientSpaceToDruiSpace(Point clientSpace)
        {
            return new Vector2(
                (clientSpace.X - X) / scale,
                (clientSpace.Y - Y) / scale
            );
        }

        public Point DruiSpaceToClientSpace(Vector2 druiSpace)
        {
            return new Point((int)(druiSpace.X*scale + X), (int)(druiSpace.Y*scale + Y));
        }

        public void Zoom(float factor, Vector2 druiSpaceFocus)
        {
            Point screenFocusPoint = DruiSpaceToClientSpace(druiSpaceFocus);
            
            X = (int)(screenFocusPoint.X + (X - screenFocusPoint.X) * factor);
            Y = (int)(screenFocusPoint.Y + (Y - screenFocusPoint.Y) * factor);
            scale *= factor;
        }
    }

    public class SelectionEventArgs: EventArgs
    {
        public object selected;

        public SelectionEventArgs(object aSelected)
        {
            selected = aSelected;
        }
    }

    public delegate void SelectionChangedHandler(object sender, SelectionEventArgs e);

    //==================================================================================================
    //DragonUIEditorPanel
    //==================================================================================================
    class DragonUIEditorPanel : GraphicsDeviceControl
    {
        EditorState_Tool _currentTool;
        public EditorState_Tool currentTool { get { return _currentTool; } set { _currentTool = value; ToolSelected(this, new EventArgs()); } }
        public readonly EditorState_HandTool TOOL_hand;
        public readonly EditorState_CursorTool TOOL_cursor;
        public readonly EditorState_ZoomTool TOOL_zoom;

        public DragonUISystem druiSystem { get; private set; }
        DragonUIFrame druiFrame;
        public readonly DragonUIViewport druiViewport = new DragonUIViewport();
        SpriteBatch spriteBatch;
        public List<DragonUIComponent> selectedComponents { get; private set; }
        public Texture2D whiteTexture { get; private set; }
        public Texture2D placeholderTexture { get; private set; }
        public event SelectionChangedHandler SelectionChanged;
        public event EventHandler ToolSelected;

        public DragonUIEditorPanel()
        {
            TOOL_hand = new EditorState_HandTool(this);
            TOOL_cursor = new EditorState_CursorTool(this);
            TOOL_zoom = new EditorState_ZoomTool(this);
        }
        
        protected override void Initialize()
        {
            // Hook the idle event to constantly redraw our animation.
            //Application.Idle += delegate { Invalidate(); };

            spriteBatch = new SpriteBatch(GraphicsDevice);

            try
            {
                Stream whiteFile = File.Open("Assets/white.png", FileMode.Open);
                whiteTexture = Texture2D.FromStream(GraphicsDevice, whiteFile);
                whiteFile.Close();

                Stream placeholderFile = File.Open("Assets/placeholder.png", FileMode.Open);
                placeholderTexture = Texture2D.FromStream(GraphicsDevice, placeholderFile);
                placeholderFile.Close();
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

            druiSystem = new DragonUISystem(GraphicsDevice, placeholderTexture);
            druiFrame = new DragonUIFrame(640, 480, whiteTexture);
            currentTool = TOOL_cursor;

            MouseDown += new MouseEventHandler(handler_MouseDown);
            MouseMove += new MouseEventHandler(handler_MouseMove);
            MouseUp += new MouseEventHandler(handler_MouseUp);

            DragEnter += new DragEventHandler(handler_DragEnter);
            DragOver += new DragEventHandler(handler_DragOver);
            DragLeave += new EventHandler(handler_DragLeave);
            DragDrop += new DragEventHandler(handler_DragDrop);
        }

        public Vectangle[] EdgeRects(Vectangle rect)
        {
            int edgeThickness = (int)Math.Ceiling(1 / druiViewport.scale);
            return new Vectangle[]
            {
                new Vectangle(rect.X-edgeThickness, rect.Y, edgeThickness, rect.Height),
                new Vectangle(rect.X+rect.Width, rect.Y, edgeThickness, rect.Height),
                new Vectangle(rect.X-edgeThickness, rect.Y-edgeThickness, rect.Width+edgeThickness*2, 1),
                new Vectangle(rect.X-edgeThickness, rect.Y+rect.Height, rect.Width+edgeThickness*2, 1),
            };
        }

        public HandleRect[] GetHandles(Rectangle rect)
        {
            float handleSize = 7/druiViewport.scale;
            float handleMargin = 2/druiViewport.scale;
            float lhs = rect.X - (handleSize + handleMargin);
            float top = rect.Y - (handleSize + handleMargin);
            float midX = rect.X + rect.Width/2 - handleSize/2;
            float midY = rect.Y + rect.Height/2 - handleSize/2;
            float rhs = rect.X + rect.Width + handleMargin;
            float bot = rect.Y + rect.Height + handleMargin;
            return new HandleRect[]
            {
                new HandleRect(new Vectangle(lhs,top, handleSize, handleSize), HandleMode.Location, HandleMode.Location),
                new HandleRect(new Vectangle(midX,top, handleSize, handleSize), HandleMode.None, HandleMode.Location),
                new HandleRect(new Vectangle(rhs,top, handleSize, handleSize), HandleMode.Size, HandleMode.Location),
                new HandleRect(new Vectangle(lhs,midY, handleSize, handleSize), HandleMode.Location, HandleMode.None),
                new HandleRect(new Vectangle(rhs,midY, handleSize, handleSize), HandleMode.Size, HandleMode.None),
                new HandleRect(new Vectangle(lhs,bot, handleSize, handleSize), HandleMode.Location, HandleMode.Size),
                new HandleRect(new Vectangle(midX,bot, handleSize, handleSize), HandleMode.None, HandleMode.Size),
                new HandleRect(new Vectangle(rhs,bot, handleSize, handleSize), HandleMode.Size, HandleMode.Size),
            };
        }

        public Vector2 ScreenSpaceToDruiSpace(Point screenSpace)
        {
            return ClientSpaceToDruiSpace(PointToClient(screenSpace));
        }

        public Vector2 ClientSpaceToDruiSpace(Point clientSpace)
        {
            return druiViewport.ClientSpaceToDruiSpace(clientSpace);
        }

        public Point PointToClient(int X, int Y)
        {
            System.Drawing.Point result = PointToClient(new System.Drawing.Point(X, Y));
            return new Point(result.X, result.Y);
        }

        public Point PointToClient(Point p)
        {
            System.Drawing.Point result = PointToClient(new System.Drawing.Point(p.X, p.Y));
            return new Point(result.X, result.Y);
        }

        public List<DragonUIComponent> PickComponent(Vector2 p)
        {
            // iterate in reverse order - we prefer to pick topmost components
            for(int Idx = druiSystem.druiComponents.Count-1; Idx >= 0; --Idx)
            {
                DragonUIComponent c = druiSystem.druiComponents[Idx];
                if( c.rect.Contains(p) )
                {
                    return new List<DragonUIComponent>(){ c };
                }
            }

            return null;
        }

        public List<DragonUIComponent> AllComponentsInBandbox(Vectangle bandbox)
        {
            List<DragonUIComponent> result = new List<DragonUIComponent>();
            foreach (DragonUIComponent c in druiSystem.druiComponents)
            {
                if (c.rect.Intersects(bandbox))
                {
                    result.Add(c);
                }
            }

            return result;
        }

        private void handler_MouseDown(object sender, MouseEventArgs e)
        {
            Vector2 mousePos = ClientSpaceToDruiSpace(new Point(e.X, e.Y));
            currentTool.MouseDown(mousePos);
        }

        private void handler_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 mousePos = ClientSpaceToDruiSpace(new Point(e.X, e.Y));

            if (currentTool.MouseMoved(mousePos) == ShouldRedraw.Yes)
            {
                Invalidate();
            }
        }

        private void handler_MouseUp(object sender, MouseEventArgs e)
        {
            Vector2 mousePos = ClientSpaceToDruiSpace(new Point(e.X, e.Y));
            if (currentTool.MouseUp(mousePos) == ShouldRedraw.Yes )
            {
                Invalidate();
            }
        }

        private void handler_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1)
                {
                    try
                    {
                        DragonUIComponent newComponent = new DragonUIImage(files[0]);
                        druiSystem.Add(newComponent);

                        Vector2 mousePos = ScreenSpaceToDruiSpace(new Point(e.X, e.Y));

                        currentTool = TOOL_cursor;
                        TOOL_cursor.BeginDragFile(newComponent, mousePos);

                        e.Effect = DragDropEffects.Link;
                    }
                    catch (Exception)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
            Invalidate();
        }

        private void handler_DragLeave(object sender, System.EventArgs e)
        {
            TOOL_cursor.CancelDragFile();
            Invalidate();
        }

        private void handler_DragOver(object sender, DragEventArgs e)
        {
            Vector2 mousePos = ScreenSpaceToDruiSpace(new Point(e.X, e.Y));
            if (currentTool.MouseMoved(mousePos) == ShouldRedraw.Yes)
            {
                Invalidate();
            }
        }

        private void handler_DragDrop(object sender, DragEventArgs e)
        {
            Vector2 mousePos = ScreenSpaceToDruiSpace(new Point(e.X, e.Y));
            if (currentTool.MouseUp(mousePos) == ShouldRedraw.Yes)
            {
                Invalidate();
            }
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.SlateGray);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, druiViewport.getViewMatrix());

            druiFrame.Draw(spriteBatch);
            druiSystem.Draw(spriteBatch);
            if (selectedComponents != null)
            {
                if (selectedComponents.Count == 1)
                {
                    foreach (HandleRect handle in GetHandles(selectedComponents[0].rect))
                    {
                        spriteBatch.Draw(whiteTexture, handle.vectangle, Color.Black);
                    }
                }

                foreach (DragonUIComponent component in selectedComponents)
                {
                    foreach (Vectangle sideRect in EdgeRects(component.rect.Vectangle()))
                    {
                        spriteBatch.Draw(whiteTexture, sideRect, Color.SlateGray);
                    }
                }
            }

            currentTool.Draw(spriteBatch);
            //spriteBatch.Draw(testTexture, new Rectangle(localMousePos.X, localMousePos.Y, 100, 100), Color.White);
            spriteBatch.End();
        }

        public void SaveTo(StreamWriter writer)
        {
            writer.Write("{\n\"frame\":["+druiFrame.Width+","+druiFrame.Height+"],\n\"components\":\n[\n");
            foreach (DragonUIComponent component in druiSystem.druiComponents)
            {
                writer.WriteLine("{");
                component.SaveMembersTo(writer);
                writer.WriteLine("},");
            }

            writer.Write("\n]\n}");
        }

        public void OpenFile(string filename)
        {
            Open(JSONTable.parseFile(filename));
        }

        public void Open(JSONTable template)
        {
            if (template == null)
                return;

            druiFrame = new DragonUIFrame(template.getArray("frame"), whiteTexture);
            druiSystem = new DragonUISystem(GraphicsDevice, placeholderTexture);
            druiViewport.Reset();

            foreach(JSONTable entry in template.getArray("components").asJSONTables())
            {
                druiSystem.Add( DragonUIComponent.newFromTemplate(entry) );
            }

            SetSelected(null);
            currentTool = TOOL_cursor;
            TOOL_cursor.Reset();
        }

        public void SetSelected(List<DragonUIComponent> newSelectedComponents)
        {
            selectedComponents = newSelectedComponents;
            if (selectedComponents == null || selectedComponents.Count == 0)
            {
                SelectionChanged(this, new SelectionEventArgs(druiFrame));
            }
            else if( selectedComponents.Count == 1 )
            {
                SelectionChanged(this, new SelectionEventArgs(selectedComponents[0]));
            }
            else
            {
                SelectionChanged(this, new SelectionEventArgs(null));
            }
        }
    }
}
