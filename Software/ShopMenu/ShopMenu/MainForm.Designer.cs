namespace ShopMenu
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.shopMenuType = new System.Windows.Forms.ComboBox();
            this.shopType = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // shopMenuType
            // 
            this.shopMenuType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shopMenuType.Enabled = false;
            this.shopMenuType.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.shopMenuType.FormattingEnabled = true;
            this.shopMenuType.IntegralHeight = false;
            this.shopMenuType.ItemHeight = 20;
            this.shopMenuType.Location = new System.Drawing.Point(413, 32);
            this.shopMenuType.Name = "shopMenuType";
            this.shopMenuType.Size = new System.Drawing.Size(93, 28);
            this.shopMenuType.TabIndex = 0;
            this.shopMenuType.SelectedIndexChanged += new System.EventHandler(this.shopMenuType_SelectedIndexChanged);
            // 
            // shopType
            // 
            this.shopType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shopType.Enabled = false;
            this.shopType.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.shopType.FormattingEnabled = true;
            this.shopType.Location = new System.Drawing.Point(524, 31);
            this.shopType.Name = "shopType";
            this.shopType.Size = new System.Drawing.Size(93, 28);
            this.shopType.TabIndex = 1;
            this.shopType.SelectedIndexChanged += new System.EventHandler(this.shopType_SelectedIndexChanged);
            // 
            // btnGo
            // 
            this.btnGo.Enabled = false;
            this.btnGo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGo.Location = new System.Drawing.Point(640, 31);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(122, 28);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "写入数据库";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPath.Location = new System.Drawing.Point(88, 32);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(181, 26);
            this.txtPath.TabIndex = 3;
            // 
            // btnReadFile
            // 
            this.btnReadFile.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadFile.Location = new System.Drawing.Point(275, 32);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(122, 28);
            this.btnReadFile.TabIndex = 4;
            this.btnReadFile.Text = "读取文件";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "文件路径：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(703, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "使用说明";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "重置程序";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 103);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReadFile);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.shopType);
            this.Controls.Add(this.shopMenuType);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量插入超市数据";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox shopMenuType;
        private System.Windows.Forms.ComboBox shopType;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

