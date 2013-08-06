namespace JhMessageBoxTestApp_WindowsForms
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
            this.btnNotifyUser = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNotifyUser
            // 
            this.btnNotifyUser.Location = new System.Drawing.Point(307, 142);
            this.btnNotifyUser.Name = "btnNotifyUser";
            this.btnNotifyUser.Size = new System.Drawing.Size(109, 23);
            this.btnNotifyUser.TabIndex = 0;
            this.btnNotifyUser.Text = "NotifyUser";
            this.btnNotifyUser.UseVisualStyleBackColor = true;
            this.btnNotifyUser.Click += new System.EventHandler(this.OnClick_btnNotifyUser);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 348);
            this.Controls.Add(this.btnNotifyUser);
            this.Name = "Form1";
            this.Text = "JhMessageBox Test Application - Windows Forms";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNotifyUser;
    }
}

