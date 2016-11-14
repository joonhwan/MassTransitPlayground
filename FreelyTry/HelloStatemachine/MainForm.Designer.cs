namespace HelloStatemachine
{
    partial class MainForm
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
            this.helloButton = new System.Windows.Forms.Button();
            this.pissOffButton = new System.Windows.Forms.Button();
            this.introduceButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.personsComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // helloButton
            // 
            this.helloButton.Location = new System.Drawing.Point(182, 42);
            this.helloButton.Name = "helloButton";
            this.helloButton.Size = new System.Drawing.Size(75, 23);
            this.helloButton.TabIndex = 0;
            this.helloButton.Text = "Hello";
            this.helloButton.UseVisualStyleBackColor = true;
            // 
            // pissOffButton
            // 
            this.pissOffButton.Location = new System.Drawing.Point(94, 42);
            this.pissOffButton.Name = "pissOffButton";
            this.pissOffButton.Size = new System.Drawing.Size(82, 23);
            this.pissOffButton.TabIndex = 0;
            this.pissOffButton.Text = "Piss Off!";
            this.pissOffButton.UseVisualStyleBackColor = true;
            // 
            // introduceButton
            // 
            this.introduceButton.Location = new System.Drawing.Point(13, 42);
            this.introduceButton.Name = "introduceButton";
            this.introduceButton.Size = new System.Drawing.Size(75, 23);
            this.introduceButton.TabIndex = 0;
            this.introduceButton.Text = "Introduce";
            this.introduceButton.UseVisualStyleBackColor = true;
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(13, 71);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(396, 278);
            this.logTextBox.TabIndex = 1;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(13, 13);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(163, 23);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "Add Person";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // personsComboBox
            // 
            this.personsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.personsComboBox.FormattingEnabled = true;
            this.personsComboBox.Location = new System.Drawing.Point(182, 15);
            this.personsComboBox.Name = "personsComboBox";
            this.personsComboBox.Size = new System.Drawing.Size(121, 20);
            this.personsComboBox.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 361);
            this.Controls.Add(this.personsComboBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.introduceButton);
            this.Controls.Add(this.pissOffButton);
            this.Controls.Add(this.helloButton);
            this.Name = "MainForm";
            this.Text = "Hello StateMachine Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button helloButton;
        private System.Windows.Forms.Button pissOffButton;
        private System.Windows.Forms.Button introduceButton;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.ComboBox personsComboBox;
    }
}