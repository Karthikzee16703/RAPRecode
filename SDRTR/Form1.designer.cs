using System;

namespace RPARecode
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
        private void InitializeComponent(string ConsoleName)
        {

            Console.WriteLine("InitializeComponent Method Started-{0}********", DateTime.Now);
            this.rtbData = new System.Windows.Forms.RichTextBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rtbData
            // 
            this.rtbData.BackColor = System.Drawing.Color.Silver;
            this.rtbData.CausesValidation = false;
            this.rtbData.EnableAutoDragDrop = true;
            this.rtbData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbData.ForeColor = System.Drawing.Color.Black;
            this.rtbData.Location = new System.Drawing.Point(12, 38);
            this.rtbData.Name = "rtbData";
            this.rtbData.ReadOnly = true;
            this.rtbData.Size = new System.Drawing.Size(510, 387);
            this.rtbData.TabIndex = 0;
            this.rtbData.Text = "";
            this.rtbData.WordWrap = false;
            // 
            // lblCount
            // 
            this.lblCount.BackColor = System.Drawing.Color.Silver;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.Location = new System.Drawing.Point(12, 9);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(510, 23);
            this.lblCount.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 437);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.rtbData);
            this.Name = ConsoleName;
            this.Text = ConsoleName;
            this.ResumeLayout(false);

            Console.WriteLine("InitializeComponent Method Started-{0}********", DateTime.Now);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbData;
        private System.Windows.Forms.Label lblCount;
    }
}

