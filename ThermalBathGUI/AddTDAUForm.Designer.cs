namespace ThermalBathGUI
{
    partial class AddTDAUForm
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
            label1 = new Label();
            portComboBox = new ComboBox();
            tdauNumber = new TextBox();
            label2 = new Label();
            ConnectBtn = new Button();
            cancelBtn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.8571434F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(50, 102);
            label1.Name = "label1";
            label1.Size = new Size(203, 50);
            label1.TabIndex = 1;
            label1.Text = "COM Port:";
            // 
            // portComboBox
            // 
            portComboBox.FormattingEnabled = true;
            portComboBox.Location = new Point(259, 115);
            portComboBox.MaxDropDownItems = 20;
            portComboBox.Name = "portComboBox";
            portComboBox.Size = new Size(108, 38);
            portComboBox.TabIndex = 2;
            portComboBox.MouseClick += mouseClickPort;
            // 
            // tdauNumber
            // 
            tdauNumber.Location = new Point(259, 42);
            tdauNumber.Name = "tdauNumber";
            tdauNumber.Size = new Size(78, 35);
            tdauNumber.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.8571434F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(50, 29);
            label2.Name = "label2";
            label2.Size = new Size(132, 50);
            label2.TabIndex = 4;
            label2.Text = "TDAU:";
            // 
            // ConnectBtn
            // 
            ConnectBtn.Location = new Point(50, 184);
            ConnectBtn.Name = "ConnectBtn";
            ConnectBtn.Size = new Size(131, 40);
            ConnectBtn.TabIndex = 5;
            ConnectBtn.Text = "Connect";
            ConnectBtn.UseVisualStyleBackColor = true;
            ConnectBtn.Click += ConnectBtn_Click;
            // 
            // cancelBtn
            // 
            cancelBtn.Location = new Point(206, 184);
            cancelBtn.Name = "cancelBtn";
            cancelBtn.Size = new Size(131, 40);
            cancelBtn.TabIndex = 6;
            cancelBtn.Text = "Cancel";
            cancelBtn.UseVisualStyleBackColor = true;
            // 
            // AddTDAUForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(397, 256);
            Controls.Add(cancelBtn);
            Controls.Add(ConnectBtn);
            Controls.Add(label2);
            Controls.Add(tdauNumber);
            Controls.Add(portComboBox);
            Controls.Add(label1);
            Name = "AddTDAUForm";
            Text = "AddTDAUForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private ComboBox portComboBox;
        private TextBox tdauNumber;
        private Label label2;
        private Button ConnectBtn;
        private Button cancelBtn;
    }
}