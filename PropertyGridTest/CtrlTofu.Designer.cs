namespace PropertyGridEx {
	partial class CtrlTofu {
		/// <summary> 
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent( )
		{
			this.rdo01 = new System.Windows.Forms.RadioButton();
			this.rdo02 = new System.Windows.Forms.RadioButton();
			this.rdo03 = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// rdo01
			// 
			this.rdo01.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rdo01.Location = new System.Drawing.Point(3, 3);
			this.rdo01.Name = "rdo01";
			this.rdo01.Size = new System.Drawing.Size(85, 19);
			this.rdo01.TabIndex = 0;
			this.rdo01.Text = "絹ごし";
			this.rdo01.UseVisualStyleBackColor = true;
			// 
			// rdo02
			// 
			this.rdo02.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rdo02.Location = new System.Drawing.Point(3, 28);
			this.rdo02.Name = "rdo02";
			this.rdo02.Size = new System.Drawing.Size(85, 19);
			this.rdo02.TabIndex = 1;
			this.rdo02.Text = "木綿";
			this.rdo02.UseVisualStyleBackColor = true;
			// 
			// rdo03
			// 
			this.rdo03.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rdo03.Location = new System.Drawing.Point(3, 53);
			this.rdo03.Name = "rdo03";
			this.rdo03.Size = new System.Drawing.Size(85, 19);
			this.rdo03.TabIndex = 2;
			this.rdo03.Text = "揚げ豆腐";
			this.rdo03.UseVisualStyleBackColor = true;
			// 
			// UserControlTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.rdo03);
			this.Controls.Add(this.rdo02);
			this.Controls.Add(this.rdo01);
			this.Name = "UserControlTest";
			this.Size = new System.Drawing.Size(91, 78);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RadioButton rdo01;
		private System.Windows.Forms.RadioButton rdo02;
		private System.Windows.Forms.RadioButton rdo03;
	}
}
