
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


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(透视变换器));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LocationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.PanelControls = new System.Windows.Forms.Panel();
            this.tranRadioButton = new System.Windows.Forms.RadioButton();
            this.drawRadioButton = new System.Windows.Forms.RadioButton();
            this.BShow = new System.Windows.Forms.Button();
            this.BStore = new System.Windows.Forms.Button();
            this.BSave = new System.Windows.Forms.Button();
            this.BClear = new System.Windows.Forms.Button();
            this.BTran = new System.Windows.Forms.Button();
            this.BOpen = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PanelOrigin = new PerspectiveTransformation.MyPanel();
            this.PanelShown = new PerspectiveTransformation.MyPanel();
            this.statusStrip1.SuspendLayout();
            this.PanelControls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.SteelBlue;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LocationLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 689);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1373, 40);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LocationLabel
            // 
            this.LocationLabel.BackColor = System.Drawing.Color.SteelBlue;
            this.LocationLabel.Font = new System.Drawing.Font("方正经黑简体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LocationLabel.ForeColor = System.Drawing.Color.MidnightBlue;
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new System.Drawing.Size(125, 30);
            this.LocationLabel.Text = "坐标位置";
            // 
            // PanelControls
            // 
            this.PanelControls.BackColor = System.Drawing.Color.LightBlue;
            this.PanelControls.Controls.Add(this.tranRadioButton);
            this.PanelControls.Controls.Add(this.drawRadioButton);
            this.PanelControls.Controls.Add(this.BShow);
            this.PanelControls.Controls.Add(this.BStore);
            this.PanelControls.Controls.Add(this.BSave);
            this.PanelControls.Controls.Add(this.BClear);
            this.PanelControls.Controls.Add(this.BTran);
            this.PanelControls.Controls.Add(this.BOpen);
            this.PanelControls.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelControls.Location = new System.Drawing.Point(0, 0);
            this.PanelControls.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PanelControls.Name = "PanelControls";
            this.PanelControls.Size = new System.Drawing.Size(140, 689);
            this.PanelControls.TabIndex = 3;
            // 
            // tranRadioButton
            // 
            this.tranRadioButton.AutoSize = true;
            this.tranRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.tranRadioButton.Enabled = false;
            this.tranRadioButton.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tranRadioButton.Location = new System.Drawing.Point(0, 424);
            this.tranRadioButton.Name = "tranRadioButton";
            this.tranRadioButton.Size = new System.Drawing.Size(140, 47);
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
            this.drawRadioButton.Location = new System.Drawing.Point(0, 377);
            this.drawRadioButton.Name = "drawRadioButton";
            this.drawRadioButton.Size = new System.Drawing.Size(140, 47);
            this.drawRadioButton.TabIndex = 12;
            this.drawRadioButton.TabStop = true;
            this.drawRadioButton.Text = "版面";
            this.drawRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.drawRadioButton.UseVisualStyleBackColor = true;
            // 
            // BShow
            // 
            this.BShow.BackColor = System.Drawing.Color.SteelBlue;
            this.BShow.Dock = System.Windows.Forms.DockStyle.Top;
            this.BShow.Enabled = false;
            this.BShow.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BShow.Location = new System.Drawing.Point(0, 302);
            this.BShow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BShow.Name = "BShow";
            this.BShow.Size = new System.Drawing.Size(140, 75);
            this.BShow.TabIndex = 9;
            this.BShow.Text = "刷新";
            this.BShow.UseVisualStyleBackColor = false;
            this.BShow.Click += new System.EventHandler(this.BShow_Click);
            // 
            // BStore
            // 
            this.BStore.BackColor = System.Drawing.Color.SteelBlue;
            this.BStore.Dock = System.Windows.Forms.DockStyle.Top;
            this.BStore.Enabled = false;
            this.BStore.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BStore.Location = new System.Drawing.Point(0, 225);
            this.BStore.Name = "BStore";
            this.BStore.Size = new System.Drawing.Size(140, 77);
            this.BStore.TabIndex = 8;
            this.BStore.Text = "存储";
            this.BStore.UseVisualStyleBackColor = false;
            this.BStore.Click += new System.EventHandler(this.BStore_Click);
            // 
            // BSave
            // 
            this.BSave.BackColor = System.Drawing.Color.SteelBlue;
            this.BSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BSave.Enabled = false;
            this.BSave.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BSave.Location = new System.Drawing.Point(0, 614);
            this.BSave.Name = "BSave";
            this.BSave.Size = new System.Drawing.Size(140, 75);
            this.BSave.TabIndex = 8;
            this.BSave.Text = "保存";
            this.BSave.UseVisualStyleBackColor = false;
            this.BSave.Click += new System.EventHandler(this.BSave_Click);
            // 
            // BClear
            // 
            this.BClear.BackColor = System.Drawing.Color.SteelBlue;
            this.BClear.Dock = System.Windows.Forms.DockStyle.Top;
            this.BClear.Enabled = false;
            this.BClear.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BClear.Location = new System.Drawing.Point(0, 150);
            this.BClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BClear.Name = "BClear";
            this.BClear.Size = new System.Drawing.Size(140, 75);
            this.BClear.TabIndex = 6;
            this.BClear.Text = "清除";
            this.BClear.UseVisualStyleBackColor = false;
            this.BClear.Click += new System.EventHandler(this.BClear_Click);
            // 
            // BTran
            // 
            this.BTran.BackColor = System.Drawing.Color.SteelBlue;
            this.BTran.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTran.Enabled = false;
            this.BTran.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BTran.Location = new System.Drawing.Point(0, 75);
            this.BTran.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BTran.Name = "BTran";
            this.BTran.Size = new System.Drawing.Size(140, 75);
            this.BTran.TabIndex = 4;
            this.BTran.Text = "透视";
            this.BTran.UseVisualStyleBackColor = false;
            this.BTran.Click += new System.EventHandler(this.BtTran_Click);
            // 
            // BOpen
            // 
            this.BOpen.BackColor = System.Drawing.Color.SteelBlue;
            this.BOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.BOpen.Font = new System.Drawing.Font("方正经黑简体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BOpen.Location = new System.Drawing.Point(0, 0);
            this.BOpen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BOpen.Name = "BOpen";
            this.BOpen.Size = new System.Drawing.Size(140, 75);
            this.BOpen.TabIndex = 0;
            this.BOpen.Text = "打开";
            this.BOpen.UseVisualStyleBackColor = false;
            this.BOpen.Click += new System.EventHandler(this.BOpen_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.Controls.Add(this.PanelOrigin);
            this.panel1.Controls.Add(this.PanelShown);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1373, 729);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // PanelOrigin
            // 
            this.PanelOrigin.BackColor = System.Drawing.Color.SteelBlue;
            this.PanelOrigin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PanelOrigin.ForeColor = System.Drawing.Color.SteelBlue;
            this.PanelOrigin.Location = new System.Drawing.Point(278, 101);
            this.PanelOrigin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PanelOrigin.Name = "PanelOrigin";
            this.PanelOrigin.Size = new System.Drawing.Size(407, 491);
            this.PanelOrigin.TabIndex = 0;
            this.PanelOrigin.Text = "mypanel1";
            this.PanelOrigin.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelOrigin_Paint);
            this.PanelOrigin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelOrigin_MouseDown);
            this.PanelOrigin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelOrigin_MouseMove);
            this.PanelOrigin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelOrigin_MouseUp);
            // 
            // PanelShown
            // 
            this.PanelShown.BackColor = System.Drawing.Color.SteelBlue;
            this.PanelShown.ForeColor = System.Drawing.Color.SteelBlue;
            this.PanelShown.Location = new System.Drawing.Point(824, 101);
            this.PanelShown.Name = "PanelShown";
            this.PanelShown.Size = new System.Drawing.Size(407, 491);
            this.PanelShown.TabIndex = 2;
            this.PanelShown.Text = "PanelShown";
            this.PanelShown.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelShown_Paint);
            this.PanelShown.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelShown_MouseMove);
            // 
            // 透视变换器
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1373, 729);
            this.Controls.Add(this.PanelControls);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
        private System.Windows.Forms.Button BShow;
        private System.Windows.Forms.Button BStore;
        private MyPanel PanelShown;
    }
}

