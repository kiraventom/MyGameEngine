
namespace FormsUI
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.MainTLP = new System.Windows.Forms.TableLayoutPanel();
			this.InfoLB = new System.Windows.Forms.ListBox();
			this.InventoryLB = new System.Windows.Forms.ListBox();
			this.DescriptionRTB = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// MainTLP
			// 
			this.MainTLP.ColumnCount = 3;
			this.MainTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.MainTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.MainTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.MainTLP.Location = new System.Drawing.Point(314, 12);
			this.MainTLP.Name = "MainTLP";
			this.MainTLP.RowCount = 3;
			this.MainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.976745F));
			this.MainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.51163F));
			this.MainTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.51163F));
			this.MainTLP.Size = new System.Drawing.Size(400, 400);
			this.MainTLP.TabIndex = 1;
			// 
			// InfoLB
			// 
			this.InfoLB.FormattingEnabled = true;
			this.InfoLB.ItemHeight = 15;
			this.InfoLB.Location = new System.Drawing.Point(12, 12);
			this.InfoLB.Name = "InfoLB";
			this.InfoLB.Size = new System.Drawing.Size(206, 124);
			this.InfoLB.TabIndex = 3;
			// 
			// InventoryLB
			// 
			this.InventoryLB.FormattingEnabled = true;
			this.InventoryLB.ItemHeight = 15;
			this.InventoryLB.Location = new System.Drawing.Point(12, 142);
			this.InventoryLB.Name = "InventoryLB";
			this.InventoryLB.Size = new System.Drawing.Size(206, 139);
			this.InventoryLB.TabIndex = 4;
			// 
			// DescriptionRTB
			// 
			this.DescriptionRTB.Location = new System.Drawing.Point(12, 288);
			this.DescriptionRTB.Name = "DescriptionRTB";
			this.DescriptionRTB.ReadOnly = true;
			this.DescriptionRTB.Size = new System.Drawing.Size(206, 115);
			this.DescriptionRTB.TabIndex = 5;
			this.DescriptionRTB.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.DescriptionRTB);
			this.Controls.Add(this.InventoryLB);
			this.Controls.Add(this.InfoLB);
			this.Controls.Add(this.MainTLP);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel MainTLP;
		private System.Windows.Forms.ListBox InfoLB;
		private System.Windows.Forms.ListBox InventoryLB;
		private System.Windows.Forms.RichTextBox DescriptionRTB;
	}
}

