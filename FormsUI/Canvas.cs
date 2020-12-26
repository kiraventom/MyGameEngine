using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FormsUI
{
	public partial class Canvas : UserControl
	{
		public Canvas()
		{
			InitializeComponent();
			this.BackColor = Color.White;
			this.ForeColor = Color.Black;
		}

		public void Draw(Action<Graphics> action)
		{
			this.Action = action;
			this.Invalidate();
		}

		private Action<Graphics> Action { get; set; }

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.Clear(this.BackColor);
			this.Action?.Invoke(e.Graphics);
		}
	}
}
