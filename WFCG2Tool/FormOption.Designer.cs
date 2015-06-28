namespace WFCG2Tool
{
    partial class FormOption
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtLoopMax = new System.Windows.Forms.TextBox();
            this.cboStrategy = new System.Windows.Forms.ComboBox();
            this.chkQuitTeam = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(31, 284);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "確認";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(156, 284);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "時間(秒)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "步數";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "策略";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(99, 21);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(107, 22);
            this.txtTime.TabIndex = 2;
            // 
            // txtLoopMax
            // 
            this.txtLoopMax.Location = new System.Drawing.Point(99, 54);
            this.txtLoopMax.Name = "txtLoopMax";
            this.txtLoopMax.Size = new System.Drawing.Size(107, 22);
            this.txtLoopMax.TabIndex = 3;
            // 
            // cboStrategy
            // 
            this.cboStrategy.FormattingEnabled = true;
            this.cboStrategy.Location = new System.Drawing.Point(99, 84);
            this.cboStrategy.Name = "cboStrategy";
            this.cboStrategy.Size = new System.Drawing.Size(121, 20);
            this.cboStrategy.TabIndex = 4;
            // 
            // chkQuitTeam
            // 
            this.chkQuitTeam.AutoSize = true;
            this.chkQuitTeam.Location = new System.Drawing.Point(12, 124);
            this.chkQuitTeam.Name = "chkQuitTeam";
            this.chkQuitTeam.Size = new System.Drawing.Size(144, 16);
            this.chkQuitTeam.TabIndex = 6;
            this.chkQuitTeam.Text = "時間結束，解取團隊。";
            this.chkQuitTeam.UseVisualStyleBackColor = true;
            // 
            // FormOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 319);
            this.Controls.Add(this.chkQuitTeam);
            this.Controls.Add(this.cboStrategy);
            this.Controls.Add(this.txtLoopMax);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormOption";
            this.Text = "選項";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtLoopMax;
        private System.Windows.Forms.ComboBox cboStrategy;
        private System.Windows.Forms.CheckBox chkQuitTeam;
    }
}