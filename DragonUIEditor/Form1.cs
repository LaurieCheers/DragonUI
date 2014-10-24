using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DragonUI;
using Microsoft.Xna.Framework;
using System.IO;

namespace DragonUIEditor
{
    public partial class Form1 : Form
    {
        EditorPreferences preferences = new EditorPreferences();
        List<ToolStripButton> tools;

        public Form1()
        {
            InitializeComponent();

            editorPanel.SelectionChanged += new SelectionChangedHandler(editorPanel_SelectionChanged);
            propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);

            tools = new List<ToolStripButton>() { cursorTool, zoomTool, handTool, textTool };
            toolStrip1.Renderer = new ToolSelectionRenderer();
            editorPanel.ToolSelected += new EventHandler(handle_ToolSelected);
            
            editorPanel.TOOL_cursor.button = cursorTool;
            editorPanel.TOOL_zoom.button = zoomTool;
            editorPanel.TOOL_hand.button = handTool;
            //editorPanel.TOOL_text.button = textTool;

            cursorTool.Checked = true;
            //propertyGrid1.PropertySort = PropertySort.NoSort;

            preferences.Open();
            this.Load += new EventHandler(onLoad);

            UpdateFilename(preferences.filename);
        }

        private class ToolSelectionRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
            {
                var btn = e.Item as ToolStripButton;
                if (btn != null && btn.Checked)
                {
                    System.Drawing.Rectangle bounds = new System.Drawing.Rectangle( System.Drawing.Point.Empty, e.Item.Size);

                    e.Graphics.DrawImage(btn.Image, new System.Drawing.Rectangle(1, 1, e.Item.Width, e.Item.Height));
                    e.Graphics.DrawImageForceColor(btn.Image, bounds, System.Drawing.Color.Blue);
                }
                else base.OnRenderItemImage(e);
            }
        }

        private void editorPanel_SelectionChanged(object sender, SelectionEventArgs e)
        {
            propertyGrid1.SelectedObject = e.selected;
        }

        public void handle_ToolSelected(object sender, EventArgs e)
        {
            foreach (ToolStripButton tool in tools)
            {
                tool.Checked = false;
            }

            if (editorPanel.currentTool != null && editorPanel.currentTool.button != null)
            {
                editorPanel.currentTool.button.Checked = true;
            }

            toolStrip1.Invalidate();
        }

        private void propertyGrid1_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            editorPanel.Invalidate();
        }

        private void UpdateFilename(string newPath)
        {
            preferences.filename = newPath;
            preferences.Save();
            if (newPath != null)
            {
                string filename = Path.GetFileName(newPath);
                Text = "DragonUI - " + filename;
            }
        }

        private void onLoad(object sender, EventArgs e)
        {
            if (preferences.filename != null)
            {
                editorPanel.OpenFile(preferences.filename);
            }
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {

        }

        private void menuItemSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "drui files (*.drui)|*.drui|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream outStream = saveFileDialog.OpenFile();
                if (outStream != null)
                {
                    // save
                    StreamWriter writer = new StreamWriter(outStream);
                    this.editorPanel.SaveTo(writer);
                    writer.Close();

                    UpdateFilename(saveFileDialog.FileName);
                }
            }
        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            if (preferences.filename != null)
            {
                StreamWriter writer = new StreamWriter(File.Open(preferences.filename, FileMode.Open));
                this.editorPanel.SaveTo(writer);
                writer.Close();
            }
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "drui files (*.drui)|*.drui|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream inStream = openFileDialog.OpenFile();
                if (inStream != null)
                {
                    StreamReader reader = new StreamReader(inStream);
                    JSONTable json = JSONTable.parse(reader.ReadToEnd());
                    reader.Close();
                    editorPanel.Open(json);

                    UpdateFilename(openFileDialog.FileName);
                }
            }
        }

        private void cursorTool_Click(object sender, EventArgs e)
        {
            editorPanel.currentTool = editorPanel.TOOL_cursor;
        }

        private void zoomTool_Click(object sender, EventArgs e)
        {
            editorPanel.currentTool = editorPanel.TOOL_zoom;
        }

        private void textTool_Click(object sender, EventArgs e)
        {
            //editorPanel.currentTool = editorPanel.TOOL_text;
        }

        private void handTool_Click(object sender, EventArgs e)
        {
            editorPanel.currentTool = editorPanel.TOOL_hand;
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            editorPanel.druiViewport.Zoom(1.25f, editorPanel.ClientSpaceToDruiSpace( new Point(editorPanel.Bounds.Width/2, editorPanel.Bounds.Height/2)));
            editorPanel.Invalidate();
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            editorPanel.druiViewport.Zoom(1/1.25f, editorPanel.ClientSpaceToDruiSpace(new Point(editorPanel.Bounds.Width / 2, editorPanel.Bounds.Height / 2)));
            editorPanel.Invalidate();
        }

        private void zoomPageButton_Click(object sender, EventArgs e)
        {
            editorPanel.druiViewport.Reset();
            editorPanel.Invalidate();
        }
    }
}
