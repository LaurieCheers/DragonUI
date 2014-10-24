using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using NSVectangle;
using System.Diagnostics;

namespace DragonUI
{
    enum ShouldRedraw
    {
        No,
        Yes,
    }

    class EditorState
    {
        public DragonUIEditorPanel editorPanel { get; private set; }

        public EditorState(DragonUIEditorPanel aEditorPanel)
        {
            editorPanel = aEditorPanel;
        }

        public virtual ShouldRedraw MouseDown(Vector2 mousePos)
        {
            return ShouldRedraw.No;
        }

        public virtual ShouldRedraw MouseMoved(Vector2 mousePos)
        {
            return ShouldRedraw.No;
        }

        public virtual ShouldRedraw MouseUp(Vector2 mousePos)
        {
            return ShouldRedraw.No;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }

    class EditorState_Tool: EditorState
    {
        public ToolStripButton button;

        public EditorState_Tool(DragonUIEditorPanel aEditorPanel)
            : base(aEditorPanel)
        {
        }
    }

    class EditorState_ZoomTool : EditorState_Tool
    {
        public EditorState_ZoomTool(DragonUIEditorPanel aEditorPanel) : base(aEditorPanel)
        {
        }

        public override ShouldRedraw MouseUp(Vector2 mousePos)
        {
            editorPanel.druiViewport.Zoom(1.25f, mousePos);
            return ShouldRedraw.Yes;
        }
    }

    class EditorState_HandTool : EditorState_Tool
    {
        public Vector2 handlePos;
        public bool mouseDragged;

        public EditorState_HandTool(DragonUIEditorPanel aEditorPanel)
            : base(aEditorPanel)
        {
        }

        public override ShouldRedraw MouseDown(Vector2 mousePos)
        {
            handlePos = mousePos;
            mouseDragged = false;
            return ShouldRedraw.No;
        }

        public override ShouldRedraw MouseUp(Vector2 mousePos)
        {
            if (!mouseDragged)
            {
                editorPanel.currentTool = editorPanel.TOOL_cursor;
            }
            return ShouldRedraw.No;
        }

        public override ShouldRedraw MouseMoved(Vector2 mousePos)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                mouseDragged = true;
                editorPanel.druiViewport.X += (int)(mousePos.X - handlePos.X);
                editorPanel.druiViewport.Y += (int)(mousePos.Y - handlePos.Y);
                Debug.WriteLine("Hand moved: " + mousePos.X + "," + mousePos.Y + " handle " + handlePos.X + "," + handlePos.Y);
                return ShouldRedraw.Yes;
            }
            return ShouldRedraw.No;
        }
    }

    class EditorState_CursorTool : EditorState_Tool
    {
        EditorState currentState;
        EditorState STATE_normal;
        EditorState_Clicking STATE_clicking;
        EditorState_Dragging STATE_dragging;
        EditorState_DraggingHandle STATE_draggingHandle;
        EditorState_Bandboxing STATE_bandboxing;

        public EditorState_CursorTool(DragonUIEditorPanel aEditorPanel)
            : base(aEditorPanel)
        {
            STATE_normal = new EditorState(editorPanel);
            STATE_clicking = new EditorState_Clicking(editorPanel);
            STATE_dragging = new EditorState_Dragging(editorPanel);
            STATE_draggingHandle = new EditorState_DraggingHandle(editorPanel);
            STATE_bandboxing = new EditorState_Bandboxing(editorPanel);

            currentState = STATE_normal;
        }

        public override ShouldRedraw MouseDown(Vector2 mousePos)
        {
            currentState = STATE_clicking;
            return STATE_clicking.MouseDown(mousePos);
        }

        public override ShouldRedraw MouseMoved(Vector2 mousePos)
        {
            bool forceRedraw = false;
            if (Control.MouseButtons == MouseButtons.Left && currentState == STATE_clicking)
            {
                // it's the first moment of dragging after a click
                forceRedraw = true;
                if (editorPanel.selectedComponents != null && editorPanel.selectedComponents.Count > 0)
                {
                    // if we've dragged within the bounds of a selected component, we're dragging them all
                    bool componentsDragged = false;
                    foreach (DragonUIComponent c in editorPanel.selectedComponents)
                    {
                        if (c.rect.Contains(mousePos))
                        {
                            STATE_dragging.BeginDragging(editorPanel.selectedComponents, mousePos);
                            componentsDragged = true;
                            break;
                        }
                    }

                    if (!componentsDragged && editorPanel.selectedComponents.Count == 1)
                    {
                        // if we've dragged on a selection handle, we're moving it
                        foreach (HandleRect handle in editorPanel.GetHandles(editorPanel.selectedComponents[0].rect))
                        {
                            if (handle.vectangle.Contains(mousePos))
                            {
                                STATE_draggingHandle.BeginDragging(editorPanel.selectedComponents[0], handle.xMode, handle.yMode);
                                currentState = STATE_draggingHandle;
                                return ShouldRedraw.Yes;
                            }
                        }
                    }
                }

                if (STATE_dragging.draggingComponents == null)
                {
                    STATE_dragging.BeginDragging(editorPanel.PickComponent(mousePos), mousePos);
                }

                if (STATE_dragging.draggingComponents != null)
                {
                    currentState = STATE_dragging;
                    editorPanel.SetSelected(STATE_dragging.draggingComponents);
                }
                else
                {
                    currentState = STATE_bandboxing;
                    STATE_bandboxing.MouseDown(mousePos);
                }
            }

            ShouldRedraw result = currentState.MouseMoved(mousePos);
            return (result==ShouldRedraw.Yes || forceRedraw)? ShouldRedraw.Yes: ShouldRedraw.No;
        }

        public override ShouldRedraw MouseUp(Vector2 mousePos)
        {
            ShouldRedraw result = currentState.MouseUp(mousePos);
            currentState = STATE_normal;

            // Hack - this is the best way I can find to update the property grid when the values change.
            editorPanel.SetSelected(editorPanel.selectedComponents);
            return result;
        }

        public void BeginDragFile(DragonUIComponent newComponent, Vector2 mousePos)
        {
            newComponent.rect = new Microsoft.Xna.Framework.Rectangle((int)mousePos.X, (int)mousePos.Y, newComponent.rect.Width, newComponent.rect.Height);
            STATE_dragging.BeginDragging(new List<DragonUIComponent>() { newComponent }, mousePos);
            currentState = STATE_dragging;
        }

        public void CancelDragFile()
        {
            if (currentState == STATE_dragging)
            {
                foreach (DragonUIComponent component in STATE_dragging.draggingComponents)
                {
                    editorPanel.druiSystem.Remove(component);
                }
                STATE_dragging.EndDragging();
                currentState = STATE_normal;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            currentState.Draw(spriteBatch);
        }

        public void Reset()
        {
            currentState = STATE_normal;
        }
    }

    class EditorState_Clicking : EditorState
    {
        public EditorState_Clicking(DragonUIEditorPanel aEditorPanel)
            : base(aEditorPanel)
        {
        }

        public override ShouldRedraw MouseUp(Vector2 mousePos)
        {
            List<DragonUIComponent> newComponents = editorPanel.PickComponent(mousePos);

            if (Control.ModifierKeys == System.Windows.Forms.Keys.Shift)
            {
                if ( editorPanel.selectedComponents.Contains(newComponents[0]))
                {
                    editorPanel.selectedComponents.Remove(newComponents[0]);
                }
                else
                {
                    editorPanel.selectedComponents.AddRange(newComponents);
                }
                editorPanel.SetSelected(editorPanel.selectedComponents);
            }
            else
            {
                editorPanel.SetSelected(newComponents);
            }
            return ShouldRedraw.Yes;
        }
    }

    class EditorState_Bandboxing : EditorState
    {
        public Vector2 bandboxOrigin;
        public Vectangle bandboxRect;

        public EditorState_Bandboxing(DragonUIEditorPanel aEditorPanel)
            : base(aEditorPanel)
        {
        }

        public override ShouldRedraw MouseUp(Vector2 mousePos)
        {
            List<DragonUIComponent> newComponents = editorPanel.AllComponentsInBandbox(bandboxRect);

            if (Control.ModifierKeys == System.Windows.Forms.Keys.Shift)
            {
                editorPanel.selectedComponents.AddRange(newComponents);
                editorPanel.SetSelected(editorPanel.selectedComponents);
            }
            else
            {
                editorPanel.SetSelected(newComponents);
            }
            return ShouldRedraw.Yes;
        }

        public override ShouldRedraw MouseDown(Vector2 mousePos)
        {
            bandboxOrigin = mousePos;
            bandboxRect = new Vectangle(mousePos.X, mousePos.Y, 0,0);
            return ShouldRedraw.Yes;
        }

        public override ShouldRedraw MouseMoved(Vector2 mousePos)
        {
            bandboxRect = new Vectangle(
                Math.Min(mousePos.X, bandboxOrigin.X),
                Math.Min(mousePos.Y, bandboxOrigin.Y),
                Math.Abs(mousePos.X - bandboxOrigin.X),
                Math.Abs(mousePos.Y - bandboxOrigin.Y)
            );
            return ShouldRedraw.Yes;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vectangle edge in editorPanel.EdgeRects(bandboxRect))
            {
                spriteBatch.Draw(editorPanel.whiteTexture, edge, Color.SlateGray);
            }
        }
    }

    class EditorState_Dragging : EditorState
    {
        public List<DragonUIComponent> draggingComponents;
        public Vector2 oldMousePos;

        public EditorState_Dragging(DragonUIEditorPanel aEditorPanel)
            : base(aEditorPanel)
        {
        }

        public void BeginDragging(List<DragonUIComponent> components, Vector2 mousePos)
        {
            if (components == null || components.Count == 0)
                return;

            draggingComponents = components;
            oldMousePos = mousePos;
        }

        public override ShouldRedraw MouseMoved(Vector2 mousePos)
        {
            if (draggingComponents != null)
            {
                float offsetX = mousePos.X - oldMousePos.X;
                float offsetY = mousePos.Y - oldMousePos.Y;
                if (offsetX != 0 || offsetY != 0)
                {
                    foreach (DragonUIComponent component in draggingComponents)
                    {
                        Rectangle oldRect = component.rect;

                        component.rect = new Rectangle((int)(oldRect.X + offsetX), (int)(oldRect.Y + offsetY),
                            component.rect.Width, component.rect.Height);
                    }
                }

                oldMousePos = mousePos;
                return ShouldRedraw.Yes; // yes, redraw
            }
            return ShouldRedraw.No;
        }

        public override ShouldRedraw MouseUp(Vector2 mousePos)
        {
            EndDragging();
            return ShouldRedraw.Yes;
        }

        public void EndDragging()
        {
            draggingComponents = null;
        }
    }

    class EditorState_DraggingHandle : EditorState
    {
        public DragonUIComponent draggingComponent;
        public HandleMode xMode;
        public HandleMode yMode;

        public EditorState_DraggingHandle(DragonUIEditorPanel aEditorPanel)
            : base(aEditorPanel)
        {
        }

        public void BeginDragging(DragonUIComponent component, HandleMode aXMode, HandleMode aYMode)
        {
            if (component != null)
            {
                draggingComponent = component;
                xMode = aXMode;
                yMode = aYMode;
            }
        }

        public override ShouldRedraw MouseMoved(Vector2 mousePos)
        {
            Rectangle oldRect = draggingComponent.rect;
            draggingComponent.rect = new Rectangle
            (
                xMode == HandleMode.Location? (int)mousePos.X:
                    oldRect.X,

                yMode == HandleMode.Location? (int)mousePos.Y:
                    oldRect.Y,

                xMode == HandleMode.Location? (int)(oldRect.Right - mousePos.X):
                    xMode == HandleMode.Size? (int)(mousePos.X - oldRect.X):
                        oldRect.Width,

                yMode == HandleMode.Location? (int)(oldRect.Bottom - mousePos.Y):
                    yMode == HandleMode.Size? (int)(mousePos.Y - oldRect.Y):
                        oldRect.Height
            );

            return ShouldRedraw.Yes; // yes, redraw
        }
    }
}
