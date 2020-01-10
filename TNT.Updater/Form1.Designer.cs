namespace TNT.Updater
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
			this.label1 = new System.Windows.Forms.Label();
			this.labelInstalledVersion = new System.Windows.Forms.Label();
			this.labelCurrentVersion = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonInstall = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.labelReleaseDate = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Installed version:";
			// 
			// labelInstalledVersion
			// 
			this.labelInstalledVersion.AutoSize = true;
			this.labelInstalledVersion.Location = new System.Drawing.Point(104, 9);
			this.labelInstalledVersion.Name = "labelInstalledVersion";
			this.labelInstalledVersion.Size = new System.Drawing.Size(40, 13);
			this.labelInstalledVersion.TabIndex = 1;
			this.labelInstalledVersion.Text = "9.9.9.9";
			// 
			// labelCurrentVersion
			// 
			this.labelCurrentVersion.AutoSize = true;
			this.labelCurrentVersion.Location = new System.Drawing.Point(104, 30);
			this.labelCurrentVersion.Name = "labelCurrentVersion";
			this.labelCurrentVersion.Size = new System.Drawing.Size(22, 13);
			this.labelCurrentVersion.TabIndex = 3;
			this.labelCurrentVersion.Text = "NA";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(81, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Current version:";
			// 
			// buttonInstall
			// 
			this.buttonInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonInstall.Enabled = false;
			this.buttonInstall.Location = new System.Drawing.Point(149, 77);
			this.buttonInstall.Name = "buttonInstall";
			this.buttonInstall.Size = new System.Drawing.Size(75, 23);
			this.buttonInstall.TabIndex = 4;
			this.buttonInstall.Text = "Install";
			this.buttonInstall.UseVisualStyleBackColor = true;
			this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(25, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Release date:";
			// 
			// labelReleaseDate
			// 
			this.labelReleaseDate.AutoSize = true;
			this.labelReleaseDate.Location = new System.Drawing.Point(104, 51);
			this.labelReleaseDate.Name = "labelReleaseDate";
			this.labelReleaseDate.Size = new System.Drawing.Size(22, 13);
			this.labelReleaseDate.TabIndex = 6;
			this.labelReleaseDate.Text = "NA";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(236, 112);
			this.Controls.Add(this.labelReleaseDate);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.buttonInstall);
			this.Controls.Add(this.labelCurrentVersion);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelInstalledVersion);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelInstalledVersion;
		private System.Windows.Forms.Label labelCurrentVersion;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonInstall;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelReleaseDate;
	}
}

