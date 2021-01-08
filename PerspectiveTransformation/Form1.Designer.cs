
namespace PerspectiveTransformation
{
    partial class 透视变换器
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(透视变换器));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LocationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.PanelControls = new System.Windows.Forms.Panel();
            this.BSave = new System.Windows.Forms.Button();
            this.tranRadioButton = new System.Windows.Forms.RadioButton();
            this.drawRadioButton = new System.Windows.Forms.RadioButton();
            this.BClear = new System.Windows.Forms.Button();
            this.BTran = new System.Windows.Forms.Button();
            this.BOpen = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PanelShow = new System.Windows.Forms.Panel();
            this.PanelOrigin = new PerspectiveTransformation.MyPanel();
            this.statusStrip1.SuspendLayout();
            this.PanelControls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LocationLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 516);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1030, 31);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LocationLabel
            // 
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new System.Drawing.Size(82, 24);
            this.LocationLabel.Text = "坐标位置";
            // 
            // PanelControls
            // 
            this.PanelControls.Controls.Add(this.BSave);
            this.PanelControls.Controls.Add(this.tranRadioButton);
            this.PanelControls.Controls.Add(this.drawRadioButton);
            this.PanelControls.Controls.Add(this.BClear);
            this.PanelControls.Controls.Add(this.BTran);
            this.PanelControls.Controls.Add(this.BOpen);
            this.PanelControls.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelControls.Location = new System.Drawing.Point(0, 0);
            this.PanelControls.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelControls.Name = "PanelControls";
            this.PanelControls.Size = new System.Drawing.Size(105, 516);
            this.PanelControls.TabIndex = 3;
            // 
            // BSave
            // 
            this.BSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BSave.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BSave.Location = new System.Drawing.Point(0, 460);
            this.BSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BSave.Name = "BSave";
            this.BSave.Size = new System.Drawing.Size(105, 56);
            this.BSave.TabIndex = 8;
            this.BSave.Text = "保存";
            this.BSave.UseVisualStyleBackColor = true;
            this.BSave.Click += new System.EventHandler(this.BSave_Click);
            // 
            // tranRadioButton
            // 
            this.tranRadioButton.AutoSize = true;
            this.tranRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.tranRadioButton.Enabled = false;
            this.tranRadioButton.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tranRadioButton.Location = new System.Drawing.Point(0, 146);
            this.tranRadioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tranRadioButton.Name = "tranRadioButton";
            this.tranRadioButton.Size = new System.Drawing.Size(79, 27);
            this.tranRadioButton.TabIndex = 10;
            this.tranRadioButton.TabStop = true;
            this.tranRadioButton.Text = "变形";
            this.tranRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tranRadioButton.UseVisualStyleBackColor = true;
            // 
            // drawRadioButton
            // 
            this.drawRadioButton.AutoSize = true;
            this.drawRadioButton.Checked = true;
            this.drawRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.drawRadioButton.Enabled = false;
            this.drawRadioButton.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.drawRadioButton.Location = new System.Drawing.Point(0, 168);
            this.drawRadioButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.drawRadioButton.Name = "drawRadioButton";
            this.drawRadioButton.Size = new System.Drawing.Size(79, 27);
            this.drawRadioButton.TabIndex = 9;
            this.drawRadioButton.TabStop = true;
            this.drawRadioButton.Text = "版面";
            this.drawRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.drawRadioButton.UseVisualStyleBackColor = true;
            // 
            // BClear
            // 
            this.BClear.Dock = System.Windows.Forms.DockStyle.Top;
            this.BClear.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BClear.Location = new System.Drawing.Point(0, 112);
            this.BClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BClear.Name = "BClear";
            this.BClear.Size = new System.Drawing.Size(105, 56);
            this.BClear.TabIndex = 6;
            this.BClear.Text = "清除";
            this.BClear.UseVisualStyleBackColor = true;
            this.BClear.Click += new System.EventHandler(this.BClear_Click);
            // 
            // BTran
            // 
            this.BTran.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTran.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BTran.Location = new System.Drawing.Point(0, 56);
            this.BTran.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BTran.Name = "BTran";
            this.BTran.Size = new System.Drawing.Size(105, 56);
            this.BTran.TabIndex = 4;
            this.BTran.Text = "透视";
            this.BTran.UseVisualStyleBackColor = true;
            this.BTran.Click += new System.EventHandler(this.BtTran_Click);
            // 
            // BOpen
            // 
            this.BOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.BOpen.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BOpen.Location = new System.Drawing.Point(0, 0);
            this.BOpen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BOpen.Name = "BOpen";
            this.BOpen.Size = new System.Drawing.Size(105, 56);
            this.BOpen.TabIndex = 0;
            this.BOpen.Text = "打开";
            this.BOpen.UseVisualStyleBackColor = true;
            this.BOpen.Click += new System.EventHandler(this.BOpen_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PanelShow);
            this.panel1.Controls.Add(this.PanelOrigin);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1030, 547);
            this.panel1.TabIndex = 4;
            // 
            // PanelShow
            // 
            this.PanelShow.BackColor = System.Drawing.Color.Black;
            this.PanelShow.Location = new System.Drawing.Point(566, 76);
            this.PanelShow.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PanelShow.Name = "PanelShow";
            this.PanelShow.Size = new System.Drawing.Size(267, 363);
            this.PanelShow.TabIndex = 1;
            // 
            // PanelOrigin
            // 
            this.PanelOrigin.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PanelOrigin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PanelOrigin.Location = new System.Drawing.Point(134, 71);
            this.PanelOrigin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelOrigin.Name = "PanelOrigin";
            this.PanelOrigin.Size = new System.Drawing.Size(305, 368);
            this.PanelOrigin.TabIndex = 0;
            this.PanelOrigin.Text = "mypanel1";
            this.PanelOrigin.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelOrigin_Paint);
            this.PanelOrigin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelOrigin_MouseDown);
            this.PanelOrigin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel1_MouseMove);
            this.PanelOrigin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelOrigin_MouseUp);
            // 
            // 透视变换器
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 547);
            this.Controls.Add(this.PanelControls);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "透视变换器";
            this.Text = "透视变换器";
            this.SizeChanged += new System.EventHandler(this.Form1_Size);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.PanelControls.ResumeLayout(false);
            this.PanelControls.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LocationLabel;
        private System.Windows.Forms.Panel PanelControls;
        private System.Windows.Forms.Button BOpen;
        private MyPanel PanelOrigin;
        private System.Windows.Forms.Button BTran;
        private System.Windows.Forms.Button BClear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton tranRadioButton;
        private System.Windows.Forms.RadioButton drawRadioButton;
        private System.Windows.Forms.Button BSave;
        private System.Windows.Forms.Panel PanelShow;
    }
}

