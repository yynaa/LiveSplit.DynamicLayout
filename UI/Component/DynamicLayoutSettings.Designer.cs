namespace LiveSplit.UI.Components
{
    partial class DynamicLayoutSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.topLevelLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.iplist = new System.Windows.Forms.Label();
            this.topLevelLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topLevelLayoutPanel
            // 
            this.topLevelLayoutPanel.AutoSize = true;
            this.topLevelLayoutPanel.ColumnCount = 2;
            this.topLevelLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.98089F));
            this.topLevelLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.01911F));
            this.topLevelLayoutPanel.Controls.Add(this.iplist, 1, 1);
            this.topLevelLayoutPanel.Controls.Add(this.label2, 0, 1);
            this.topLevelLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.topLevelLayoutPanel.Controls.Add(this.txtPort, 1, 0);
            this.topLevelLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.topLevelLayoutPanel.Name = "topLevelLayoutPanel";
            this.topLevelLayoutPanel.RowCount = 3;
            this.topLevelLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.topLevelLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.topLevelLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.topLevelLayoutPanel.Size = new System.Drawing.Size(368, 294);
            this.topLevelLayoutPanel.TabIndex = 0;
            this.topLevelLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.topLevelLayoutPanel_Paint);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPort
            // 
            this.txtPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPort.Location = new System.Drawing.Point(109, 4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(220, 20);
            this.txtPort.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Local IP(s):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // iplist
            // 
            this.iplist.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.iplist.AutoSize = true;
            this.iplist.Location = new System.Drawing.Point(109, 37);
            this.iplist.Name = "iplist";
            this.iplist.Size = new System.Drawing.Size(58, 13);
            this.iplist.TabIndex = 3;
            this.iplist.Text = "IP Address";
            this.iplist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iplist.Click += new System.EventHandler(this.label3_Click);
            // 
            // DynamicLayoutSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.topLevelLayoutPanel);
            this.Name = "DynamicLayoutSettings";
            this.Size = new System.Drawing.Size(371, 297);
            this.topLevelLayoutPanel.ResumeLayout(false);
            this.topLevelLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.TableLayoutPanel topLevelLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label iplist;
	}
}
