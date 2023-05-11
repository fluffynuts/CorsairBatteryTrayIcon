using System.ComponentModel;

namespace CorsairBatteryTrayIcon
{
    partial class About
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(
            bool disposing
        )
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
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.forkedFromLabel = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.DismissButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                14.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte) (0))
            );
            this.label1.Location = new System.Drawing.Point(
                6,
                13
            );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(
                303,
                31
            );
            this.label1.TabIndex = 0;
            this.label1.Text = "Corsair Battery Tray Icon";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(
                7,
                88
            );
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(
                302,
                20
            );
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://github.com/fluffynuts/CorsairBatteryIcon";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // forkedFromLabel
            // 
            this.forkedFromLabel.Location = new System.Drawing.Point(
                7,
                108
            );
            this.forkedFromLabel.Name = "forkedFromLabel";
            this.forkedFromLabel.Size = new System.Drawing.Size(
                302,
                17
            );
            this.forkedFromLabel.TabIndex = 2;
            this.forkedFromLabel.Text = "Credits: forked from";
            this.forkedFromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel2
            // 
            this.linkLabel2.Location = new System.Drawing.Point(
                7,
                125
            );
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(
                302,
                19
            );
            this.linkLabel2.TabIndex = 3;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "https://github.com/mx0c/Corsair-Headset-Battery-Overlay";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // VersionLabel
            // 
            this.VersionLabel.Location = new System.Drawing.Point(
                12,
                44
            );
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(
                297,
                24
            );
            this.VersionLabel.TabIndex = 4;
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DismissButton
            // 
            this.DismissButton.Location = new System.Drawing.Point(
                241,
                147
            );
            this.DismissButton.Name = "DismissButton";
            this.DismissButton.Size = new System.Drawing.Size(
                61,
                24
            );
            this.DismissButton.TabIndex = 5;
            this.DismissButton.Text = "Ok";
            this.DismissButton.UseVisualStyleBackColor = true;
            this.DismissButton.Click += new System.EventHandler(this.DismissButton_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(
                7,
                66
            );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(
                302,
                23
            );
            this.label2.TabIndex = 6;
            this.label2.Text = "Home page:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(
                6F,
                13F
            );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(
                314,
                212
            );
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DismissButton);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.forkedFromLabel);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Name = "About";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Button DismissButton;

        private System.Windows.Forms.Label VersionLabel;

        private System.Windows.Forms.LinkLabel linkLabel2;

        private System.Windows.Forms.Label forkedFromLabel;

        private System.Windows.Forms.LinkLabel linkLabel1;

        private System.Windows.Forms.Label label1;

        #endregion
    }
}