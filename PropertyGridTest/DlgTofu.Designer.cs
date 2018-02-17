namespace PropertyGridTest
{
	partial class DlgTofu
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
			this.rdoKinu = new System.Windows.Forms.RadioButton();
			this.rdoMomen = new System.Windows.Forms.RadioButton();
			this.rdoAge = new System.Windows.Forms.RadioButton();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// rdoKinu
			// 
			this.rdoKinu.AutoSize = true;
			this.rdoKinu.Location = new System.Drawing.Point(12, 11);
			this.rdoKinu.Name = "rdoKinu";
			this.rdoKinu.Size = new System.Drawing.Size(66, 19);
			this.rdoKinu.TabIndex = 0;
			this.rdoKinu.TabStop = true;
			this.rdoKinu.Text = "絹ごし";
			this.rdoKinu.UseVisualStyleBackColor = true;
			// 
			// rdoMomen
			// 
			this.rdoMomen.AutoSize = true;
			this.rdoMomen.Location = new System.Drawing.Point(12, 36);
			this.rdoMomen.Name = "rdoMomen";
			this.rdoMomen.Size = new System.Drawing.Size(58, 19);
			this.rdoMomen.TabIndex = 0;
			this.rdoMomen.TabStop = true;
			this.rdoMomen.Text = "木綿";
			this.rdoMomen.UseVisualStyleBackColor = true;
			// 
			// rdoAge
			// 
			this.rdoAge.AutoSize = true;
			this.rdoAge.Location = new System.Drawing.Point(12, 61);
			this.rdoAge.Name = "rdoAge";
			this.rdoAge.Size = new System.Drawing.Size(85, 19);
			this.rdoAge.TabIndex = 0;
			this.rdoAge.TabStop = true;
			this.rdoAge.Text = "揚げ豆腐";
			this.rdoAge.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(79, 95);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.AutoSize = true;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(160, 95);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 25);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// DlgTofu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(246, 125);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.rdoAge);
			this.Controls.Add(this.rdoMomen);
			this.Controls.Add(this.rdoKinu);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "DlgTofu";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton rdoKinu;
		private System.Windows.Forms.RadioButton rdoMomen;
		private System.Windows.Forms.RadioButton rdoAge;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}