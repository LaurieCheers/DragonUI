namespace DragonUIEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemNew = new System.Windows.Forms.MenuItem();
            this.menuItemOpen = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cursorTool = new System.Windows.Forms.ToolStripButton();
            this.zoomTool = new System.Windows.Forms.ToolStripButton();
            this.handTool = new System.Windows.Forms.ToolStripButton();
            this.textTool = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomInButton = new System.Windows.Forms.ToolStripButton();
            this.zoomOutButton = new System.Windows.Forms.ToolStripButton();
            this.zoomPageButton = new System.Windows.Forms.ToolStripButton();
            this.editorPanel = new DragonUI.DragonUIEditorPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNew,
            this.menuItemOpen,
            this.menuItemSave,
            this.menuItemSaveAs,
            this.menuItemExit});
            this.menuItem1.Text = "File";
            // 
            // menuItemNew
            // 
            this.menuItemNew.Index = 0;
            this.menuItemNew.Text = "New";
            // 
            // menuItemOpen
            // 
            this.menuItemOpen.Index = 1;
            this.menuItemOpen.Text = "Open...";
            // 
            // menuItemSave
            // 
            this.menuItemSave.Index = 2;
            this.menuItemSave.Text = "Save";
            // 
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.Index = 3;
            this.menuItemSaveAs.Text = "Save As...";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 4;
            this.menuItemExit.Text = "Exit";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem6,
            this.menuItem7,
            this.menuItem8,
            this.menuItem9,
            this.menuItem10});
            this.menuItem2.Text = "Edit";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 0;
            this.menuItem6.Text = "Cut";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 1;
            this.menuItem7.Text = "Copy";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 2;
            this.menuItem8.Text = "Paste";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 3;
            this.menuItem9.Text = "Undo";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 4;
            this.menuItem10.Text = "Redo";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "View";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(491, 26);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(277, 430);
            this.propertyGrid1.TabIndex = 4;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cursorTool,
            this.zoomTool,
            this.handTool,
            this.textTool,
            this.toolStripSeparator1,
            this.zoomInButton,
            this.zoomOutButton,
            this.zoomPageButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(770, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cursorTool
            // 
            this.cursorTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cursorTool.Image = ((System.Drawing.Image)(resources.GetObject("cursorTool.Image")));
            this.cursorTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cursorTool.Name = "cursorTool";
            this.cursorTool.Size = new System.Drawing.Size(23, 22);
            this.cursorTool.Text = "Select";
            this.cursorTool.Click += new System.EventHandler(this.cursorTool_Click);
            // 
            // zoomTool
            // 
            this.zoomTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomTool.Image = ((System.Drawing.Image)(resources.GetObject("zoomTool.Image")));
            this.zoomTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomTool.Name = "zoomTool";
            this.zoomTool.Size = new System.Drawing.Size(23, 22);
            this.zoomTool.Text = "Zoom";
            this.zoomTool.Click += new System.EventHandler(this.zoomTool_Click);
            // 
            // handTool
            // 
            this.handTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.handTool.Image = ((System.Drawing.Image)(resources.GetObject("handTool.Image")));
            this.handTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.handTool.Name = "handTool";
            this.handTool.Size = new System.Drawing.Size(23, 22);
            this.handTool.Text = "Move View";
            this.handTool.Click += new System.EventHandler(this.handTool_Click);
            // 
            // textTool
            // 
            this.textTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.textTool.Image = ((System.Drawing.Image)(resources.GetObject("textTool.Image")));
            this.textTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.textTool.Name = "textTool";
            this.textTool.Size = new System.Drawing.Size(23, 22);
            this.textTool.Text = "Text";
            this.textTool.Click += new System.EventHandler(this.textTool_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // zoomInButton
            // 
            this.zoomInButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomInButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomInButton.Image")));
            this.zoomInButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(23, 22);
            this.zoomInButton.Text = "toolStripButton1";
            this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomOutButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomOutButton.Image")));
            this.zoomOutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(23, 22);
            this.zoomOutButton.Text = "toolStripButton2";
            this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
            // 
            // zoomPageButton
            // 
            this.zoomPageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomPageButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomPageButton.Image")));
            this.zoomPageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomPageButton.Name = "zoomPageButton";
            this.zoomPageButton.Size = new System.Drawing.Size(23, 22);
            this.zoomPageButton.Text = "Zoom To Fit Page";
            this.zoomPageButton.Click += new System.EventHandler(this.zoomPageButton_Click);
            // 
            // editorPanel
            // 
            this.editorPanel.AllowDrop = true;
            this.editorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPanel.Location = new System.Drawing.Point(-1, 26);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(486, 430);
            this.editorPanel.TabIndex = 5;
            this.editorPanel.Text = "dragonUIEditorPage1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 456);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.editorPanel);
            this.Controls.Add(this.propertyGrid1);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "4";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemSave;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.MenuItem menuItemNew;
        private DragonUI.DragonUIEditorPanel editorPanel;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.Windows.Forms.MenuItem menuItemSaveAs;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton cursorTool;
        private System.Windows.Forms.ToolStripButton handTool;
        private System.Windows.Forms.ToolStripButton textTool;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton zoomInButton;
        private System.Windows.Forms.ToolStripButton zoomOutButton;
        private System.Windows.Forms.ToolStripButton zoomTool;
        private System.Windows.Forms.ToolStripButton zoomPageButton;
    }
}

