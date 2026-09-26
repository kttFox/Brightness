namespace Brightness
{
    partial class BrightnessForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.button0 = new System.Windows.Forms.Button();
			this.button25 = new System.Windows.Forms.Button();
			this.button50 = new System.Windows.Forms.Button();
			this.button75 = new System.Windows.Forms.Button();
			this.button100 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button0
			// 
			this.button0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button0.Location = new System.Drawing.Point( 15, 12 );
			this.button0.Name = "button0";
			this.button0.Size = new System.Drawing.Size( 62, 32 );
			this.button0.TabIndex = 0;
			this.button0.Tag = "0";
			this.button0.Text = "0%";
			this.button0.Click +=  this.presetButton_Click ;
			// 
			// button25
			// 
			this.button25.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button25.Location = new System.Drawing.Point( 83, 12 );
			this.button25.Name = "button25";
			this.button25.Size = new System.Drawing.Size( 62, 32 );
			this.button25.TabIndex = 1;
			this.button25.Tag = "25";
			this.button25.Text = "25%";
			this.button25.Click +=  this.presetButton_Click ;
			// 
			// button50
			// 
			this.button50.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button50.Location = new System.Drawing.Point( 151, 12 );
			this.button50.Name = "button50";
			this.button50.Size = new System.Drawing.Size( 62, 32 );
			this.button50.TabIndex = 2;
			this.button50.Tag = "50";
			this.button50.Text = "50%";
			this.button50.Click +=  this.presetButton_Click ;
			// 
			// button75
			// 
			this.button75.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button75.Location = new System.Drawing.Point( 219, 12 );
			this.button75.Name = "button75";
			this.button75.Size = new System.Drawing.Size( 62, 32 );
			this.button75.TabIndex = 3;
			this.button75.Tag = "75";
			this.button75.Text = "75%";
			this.button75.Click +=  this.presetButton_Click ;
			// 
			// button100
			// 
			this.button100.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button100.Location = new System.Drawing.Point( 287, 12 );
			this.button100.Name = "button100";
			this.button100.Size = new System.Drawing.Size( 62, 32 );
			this.button100.TabIndex = 4;
			this.button100.Tag = "100";
			this.button100.Text = "100%";
			this.button100.Click +=  this.presetButton_Click ;
			// 
			// BrightnessForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 96F, 96F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size( 365, 56 );
			this.Controls.Add( this.button100 );
			this.Controls.Add( this.button75 );
			this.Controls.Add( this.button50 );
			this.Controls.Add( this.button25 );
			this.Controls.Add( this.button0 );
			this.Font = new System.Drawing.Font( "Yu Gothic UI", 10F );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "BrightnessForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "画面輝度";
			this.TopMost = true;
			this.ResumeLayout( false );
		}

		#endregion

		private System.Windows.Forms.Button button0;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button button50;
        private System.Windows.Forms.Button button75;
        private System.Windows.Forms.Button button100;
    }
}
