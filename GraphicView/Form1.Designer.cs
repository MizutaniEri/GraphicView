namespace GraphicView
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.nextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beforeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.imageListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitScreenSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.widthFitZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.nextToolStripMenuItem,
            this.beforeToolStripMenuItem,
            this.toolStripSeparator2,
            this.imageListToolStripMenuItem,
            this.fitScreenSizeToolStripMenuItem,
            this.widthFitZoomToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 184);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // nextToolStripMenuItem
            // 
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            this.nextToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            this.nextToolStripMenuItem.Text = "Next image";
            this.nextToolStripMenuItem.Click += new System.EventHandler(this.nextToolStripMenuItem_Click);
            // 
            // beforeToolStripMenuItem
            // 
            this.beforeToolStripMenuItem.Name = "beforeToolStripMenuItem";
            this.beforeToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            this.beforeToolStripMenuItem.Text = "Before image";
            this.beforeToolStripMenuItem.Click += new System.EventHandler(this.beforeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(179, 6);
            // 
            // imageListToolStripMenuItem
            // 
            this.imageListToolStripMenuItem.Name = "imageListToolStripMenuItem";
            this.imageListToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            this.imageListToolStripMenuItem.Text = "Image List";
            this.imageListToolStripMenuItem.Click += new System.EventHandler(this.imageListToolStripMenuItem_Click);
            // 
            // fitScreenSizeToolStripMenuItem
            // 
            this.fitScreenSizeToolStripMenuItem.CheckOnClick = true;
            this.fitScreenSizeToolStripMenuItem.Name = "fitScreenSizeToolStripMenuItem";
            this.fitScreenSizeToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            this.fitScreenSizeToolStripMenuItem.Text = "Fit Screen Size";
            this.fitScreenSizeToolStripMenuItem.Click += new System.EventHandler(this.fitScreenSizeToolStripMenuItem_Click);
            // 
            // widthFitZoomToolStripMenuItem
            // 
            this.widthFitZoomToolStripMenuItem.CheckOnClick = true;
            this.widthFitZoomToolStripMenuItem.Name = "widthFitZoomToolStripMenuItem";
            this.widthFitZoomToolStripMenuItem.Size = new System.Drawing.Size(182, 24);
            this.widthFitZoomToolStripMenuItem.Text = "Width Fit Zoom";
            this.widthFitZoomToolStripMenuItem.Click += new System.EventHandler(this.widthFitZoomToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(287, 266);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(396, 341);
            this.Controls.Add(this.pictureBox1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beforeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem imageListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitScreenSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem widthFitZoomToolStripMenuItem;
    }
}

