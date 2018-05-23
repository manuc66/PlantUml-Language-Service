namespace Disassembler.Forms
{
    partial class PlsReverseFrm
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
            this.tboxInput = new System.Windows.Forms.TextBox();
            this.tboxOutput = new System.Windows.Forms.TextBox();
            this.tboxRoot = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBrowseForDll = new System.Windows.Forms.Button();
            this.btnBrowseForOutputDir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tboxInput
            // 
            this.tboxInput.Location = new System.Drawing.Point(194, 30);
            this.tboxInput.Name = "tboxInput";
            this.tboxInput.Size = new System.Drawing.Size(254, 22);
            this.tboxInput.TabIndex = 0;
            this.tboxInput.TextChanged += new System.EventHandler(this.tboxInput_TextChanged);
            // 
            // tboxOutput
            // 
            this.tboxOutput.Location = new System.Drawing.Point(194, 71);
            this.tboxOutput.Name = "tboxOutput";
            this.tboxOutput.Size = new System.Drawing.Size(254, 22);
            this.tboxOutput.TabIndex = 1;
            this.tboxOutput.TextChanged += new System.EventHandler(this.tboxOutput_TextChanged);
            // 
            // tboxRoot
            // 
            this.tboxRoot.Location = new System.Drawing.Point(194, 112);
            this.tboxRoot.Name = "tboxRoot";
            this.tboxRoot.Size = new System.Drawing.Size(254, 22);
            this.tboxRoot.TabIndex = 2;
            this.tboxRoot.TextChanged += new System.EventHandler(this.tboxRoot_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input Assembly (full path)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output Directory";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Relative Path (root)";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(12, 165);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(233, 23);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(251, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(235, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBrowseForDll
            // 
            this.btnBrowseForDll.Location = new System.Drawing.Point(455, 30);
            this.btnBrowseForDll.Name = "btnBrowseForDll";
            this.btnBrowseForDll.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseForDll.TabIndex = 8;
            this.btnBrowseForDll.Text = "...";
            this.btnBrowseForDll.UseVisualStyleBackColor = true;
            // 
            // btnBrowseForOutputDir
            // 
            this.btnBrowseForOutputDir.Location = new System.Drawing.Point(455, 71);
            this.btnBrowseForOutputDir.Name = "btnBrowseForOutputDir";
            this.btnBrowseForOutputDir.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseForOutputDir.TabIndex = 9;
            this.btnBrowseForOutputDir.Text = "...";
            this.btnBrowseForOutputDir.UseVisualStyleBackColor = true;
            // 
            // PlsReverseFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 208);
            this.ControlBox = false;
            this.Controls.Add(this.btnBrowseForOutputDir);
            this.Controls.Add(this.btnBrowseForDll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tboxRoot);
            this.Controls.Add(this.tboxOutput);
            this.Controls.Add(this.tboxInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PlsReverseFrm";
            this.Text = "Reverse DLL to PlantUml";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tboxInput;
        private System.Windows.Forms.TextBox tboxOutput;
        private System.Windows.Forms.TextBox tboxRoot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBrowseForDll;
        private System.Windows.Forms.Button btnBrowseForOutputDir;
    }
}