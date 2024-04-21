namespace ThermalBathGUI
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
            components = new System.ComponentModel.Container();
            projName = new TextBox();
            ProNameText = new Label();
            vcc = new TextBox();
            vccEnDis = new CheckBox();
            Ie1 = new TextBox();
            Ie1CleanBtn = new Button();
            Ie2CleanBtn = new Button();
            Ie2 = new TextBox();
            Ie3CleanBtn = new Button();
            Ie3 = new TextBox();
            currents = new Label();
            temperature = new Label();
            LowText = new Label();
            lowTempText = new Label();
            lowTemp = new TextBox();
            highTemp = new TextBox();
            highTempText = new Label();
            HighText = new Label();
            stepTemp = new TextBox();
            tempStepText = new Label();
            StepText = new Label();
            TDAUCard1Text = new Label();
            TDAUCard2Text = new Label();
            TDAU1 = new ComboBox();
            TDAU2 = new ComboBox();
            TDAU1ConnectBtn = new Button();
            TDAU2ConnectBtn = new Button();
            unitsText = new Label();
            userMailText = new Label();
            email = new TextBox();
            startTestBtn = new Button();
            Ie1List = new ListBox();
            Ie2List = new ListBox();
            Ie3List = new ListBox();
            groupBox1 = new GroupBox();
            button1 = new Button();
            groupBox2 = new GroupBox();
            Ie2AddBTN = new Button();
            groupBox3 = new GroupBox();
            Ie1AddBTN = new Button();
            label1 = new Label();
            label2 = new Label();
            vccGroup = new GroupBox();
            vccText = new Label();
            EmailText = new Label();
            labelTemporary = new Label();
            timerTemporary = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            vccGroup.SuspendLayout();
            SuspendLayout();
            // 
            // projName
            // 
            projName.BackColor = Color.White;
            projName.BorderStyle = BorderStyle.FixedSingle;
            projName.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            projName.Location = new Point(163, 36);
            projName.Name = "projName";
            projName.Size = new Size(62, 32);
            projName.TabIndex = 1;
            projName.KeyPress += projName_KeyPress;
            // 
            // ProNameText
            // 
            ProNameText.AutoSize = true;
            ProNameText.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            ProNameText.Location = new Point(27, 37);
            ProNameText.Name = "ProNameText";
            ProNameText.Size = new Size(134, 25);
            ProNameText.TabIndex = 2;
            ProNameText.Text = "Project name:";
            // 
            // vcc
            // 
            vcc.AccessibleName = "";
            vcc.BackColor = SystemColors.Window;
            vcc.BorderStyle = BorderStyle.FixedSingle;
            vcc.Enabled = false;
            vcc.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            vcc.Location = new Point(26, 11);
            vcc.Name = "vcc";
            vcc.Size = new Size(113, 25);
            vcc.TabIndex = 3;
            vcc.Text = "Vcc";
            vcc.Enter += vcc_Enter;
            vcc.Leave += vcc_Leave;
            // 
            // vccEnDis
            // 
            vccEnDis.AutoSize = true;
            vccEnDis.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            vccEnDis.ForeColor = Color.Black;
            vccEnDis.Location = new Point(6, 15);
            vccEnDis.Name = "vccEnDis";
            vccEnDis.RightToLeft = RightToLeft.Yes;
            vccEnDis.Size = new Size(15, 14);
            vccEnDis.TabIndex = 2;
            vccEnDis.UseVisualStyleBackColor = true;
            vccEnDis.CheckedChanged += vccEnDis_CheckedChanged;
            // 
            // Ie1
            // 
            Ie1.BackColor = Color.White;
            Ie1.BorderStyle = BorderStyle.FixedSingle;
            Ie1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Ie1.Location = new Point(7, 13);
            Ie1.Name = "Ie1";
            Ie1.Size = new Size(45, 25);
            Ie1.TabIndex = 4;
            Ie1.Text = "Ie1";
            Ie1.TextAlign = HorizontalAlignment.Center;
            Ie1.Enter += Ie1_Enter;
            Ie1.KeyPress += Ie1_KeyPress;
            Ie1.Leave += Ie1_Leave;
            // 
            // Ie1CleanBtn
            // 
            Ie1CleanBtn.BackColor = Color.RoyalBlue;
            Ie1CleanBtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Ie1CleanBtn.ForeColor = Color.White;
            Ie1CleanBtn.Location = new Point(7, 114);
            Ie1CleanBtn.Name = "Ie1CleanBtn";
            Ie1CleanBtn.Size = new Size(107, 26);
            Ie1CleanBtn.TabIndex = 4;
            Ie1CleanBtn.Text = "Clean";
            Ie1CleanBtn.UseVisualStyleBackColor = false;
            Ie1CleanBtn.Click += Ie1CleanBtn_Click;
            // 
            // Ie2CleanBtn
            // 
            Ie2CleanBtn.BackColor = Color.RoyalBlue;
            Ie2CleanBtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Ie2CleanBtn.ForeColor = Color.White;
            Ie2CleanBtn.Location = new Point(6, 114);
            Ie2CleanBtn.Name = "Ie2CleanBtn";
            Ie2CleanBtn.Size = new Size(107, 26);
            Ie2CleanBtn.TabIndex = 7;
            Ie2CleanBtn.Text = "Clean";
            Ie2CleanBtn.UseVisualStyleBackColor = false;
            Ie2CleanBtn.Click += Ie2CleanBtn_Click;
            // 
            // Ie2
            // 
            Ie2.BackColor = Color.White;
            Ie2.BorderStyle = BorderStyle.FixedSingle;
            Ie2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Ie2.Location = new Point(6, 13);
            Ie2.Name = "Ie2";
            Ie2.Size = new Size(45, 25);
            Ie2.TabIndex = 6;
            Ie2.Text = "Ie2";
            Ie2.TextAlign = HorizontalAlignment.Center;
            Ie2.Enter += Ie2_Enter;
            Ie2.KeyPress += Ie2_KeyPress;
            Ie2.Leave += Ie2_Leave;
            // 
            // Ie3CleanBtn
            // 
            Ie3CleanBtn.BackColor = Color.RoyalBlue;
            Ie3CleanBtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Ie3CleanBtn.ForeColor = Color.White;
            Ie3CleanBtn.Location = new Point(6, 114);
            Ie3CleanBtn.Name = "Ie3CleanBtn";
            Ie3CleanBtn.Size = new Size(109, 26);
            Ie3CleanBtn.TabIndex = 9;
            Ie3CleanBtn.Text = "Clean";
            Ie3CleanBtn.UseVisualStyleBackColor = false;
            Ie3CleanBtn.Click += Ie3CleanBtn_Click;
            // 
            // Ie3
            // 
            Ie3.BackColor = Color.White;
            Ie3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Ie3.Location = new Point(6, 13);
            Ie3.Name = "Ie3";
            Ie3.Size = new Size(47, 25);
            Ie3.TabIndex = 8;
            Ie3.Text = "Ie3";
            Ie3.TextAlign = HorizontalAlignment.Center;
            Ie3.Enter += Ie3_Enter;
            Ie3.KeyPress += Ie3_KeyPress;
            Ie3.Leave += Ie3_Leave;
            // 
            // currents
            // 
            currents.AutoSize = true;
            currents.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            currents.Location = new Point(12, 153);
            currents.Name = "currents";
            currents.Size = new Size(119, 32);
            currents.TabIndex = 18;
            currents.Text = "Currents:";
            // 
            // temperature
            // 
            temperature.AutoSize = true;
            temperature.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            temperature.Location = new Point(12, 339);
            temperature.Name = "temperature";
            temperature.Size = new Size(166, 32);
            temperature.TabIndex = 19;
            temperature.Text = "Temperature:";
            // 
            // LowText
            // 
            LowText.AutoSize = true;
            LowText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            LowText.ImageAlign = ContentAlignment.BottomCenter;
            LowText.Location = new Point(27, 371);
            LowText.Name = "LowText";
            LowText.Size = new Size(50, 25);
            LowText.TabIndex = 20;
            LowText.Text = "Low:";
            // 
            // lowTempText
            // 
            lowTempText.AutoSize = true;
            lowTempText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lowTempText.Location = new Point(203, 372);
            lowTempText.Name = "lowTempText";
            lowTempText.Size = new Size(0, 25);
            lowTempText.TabIndex = 21;
            // 
            // lowTemp
            // 
            lowTemp.BackColor = Color.White;
            lowTemp.BorderStyle = BorderStyle.FixedSingle;
            lowTemp.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lowTemp.Location = new Point(88, 368);
            lowTemp.Name = "lowTemp";
            lowTemp.Size = new Size(100, 32);
            lowTemp.TabIndex = 10;
            lowTemp.Text = "°C";
            lowTemp.Enter += lowTemp_Enter;
            lowTemp.KeyPress += lowTemp_KeyPress;
            lowTemp.Leave += lowTemp_Leave;
            // 
            // highTemp
            // 
            highTemp.BackColor = Color.White;
            highTemp.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            highTemp.Location = new Point(88, 398);
            highTemp.Name = "highTemp";
            highTemp.Size = new Size(100, 32);
            highTemp.TabIndex = 11;
            highTemp.Text = "°C";
            highTemp.Enter += highTemp_Enter;
            highTemp.KeyPress += highTemp_KeyPress;
            highTemp.Leave += highTemp_Leave;
            // 
            // highTempText
            // 
            highTempText.AutoSize = true;
            highTempText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            highTempText.Location = new Point(203, 403);
            highTempText.Name = "highTempText";
            highTempText.Size = new Size(0, 25);
            highTempText.TabIndex = 24;
            // 
            // HighText
            // 
            HighText.AutoSize = true;
            HighText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            HighText.ImageAlign = ContentAlignment.BottomCenter;
            HighText.Location = new Point(27, 401);
            HighText.Name = "HighText";
            HighText.Size = new Size(56, 25);
            HighText.TabIndex = 23;
            HighText.Text = "High:";
            // 
            // stepTemp
            // 
            stepTemp.BackColor = Color.White;
            stepTemp.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            stepTemp.Location = new Point(88, 429);
            stepTemp.Name = "stepTemp";
            stepTemp.Size = new Size(100, 32);
            stepTemp.TabIndex = 12;
            stepTemp.KeyPress += stepTemp_KeyPress;
            // 
            // tempStepText
            // 
            tempStepText.AutoSize = true;
            tempStepText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            tempStepText.Location = new Point(203, 434);
            tempStepText.Name = "tempStepText";
            tempStepText.Size = new Size(0, 25);
            tempStepText.TabIndex = 27;
            // 
            // StepText
            // 
            StepText.AutoSize = true;
            StepText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            StepText.ImageAlign = ContentAlignment.BottomCenter;
            StepText.Location = new Point(27, 432);
            StepText.Name = "StepText";
            StepText.Size = new Size(52, 25);
            StepText.TabIndex = 26;
            StepText.Text = "Step:";
            // 
            // TDAUCard1Text
            // 
            TDAUCard1Text.AutoSize = true;
            TDAUCard1Text.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            TDAUCard1Text.Location = new Point(32, 504);
            TDAUCard1Text.Name = "TDAUCard1Text";
            TDAUCard1Text.Size = new Size(121, 25);
            TDAUCard1Text.TabIndex = 29;
            TDAUCard1Text.Text = "TDAU card 1:";
            // 
            // TDAUCard2Text
            // 
            TDAUCard2Text.AutoSize = true;
            TDAUCard2Text.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            TDAUCard2Text.Location = new Point(32, 538);
            TDAUCard2Text.Name = "TDAUCard2Text";
            TDAUCard2Text.Size = new Size(121, 25);
            TDAUCard2Text.TabIndex = 30;
            TDAUCard2Text.Text = "TDAU card 2:";
            // 
            // TDAU1
            // 
            TDAU1.BackColor = Color.White;
            TDAU1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            TDAU1.FormattingEnabled = true;
            TDAU1.Location = new Point(159, 501);
            TDAU1.Name = "TDAU1";
            TDAU1.Size = new Size(121, 33);
            TDAU1.TabIndex = 13;
            TDAU1.MouseClick += TDAU1_MouseClick;
            // 
            // TDAU2
            // 
            TDAU2.BackColor = Color.White;
            TDAU2.Enabled = false;
            TDAU2.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            TDAU2.FormattingEnabled = true;
            TDAU2.Location = new Point(159, 534);
            TDAU2.Name = "TDAU2";
            TDAU2.Size = new Size(121, 33);
            TDAU2.TabIndex = 15;
            TDAU2.MouseClick += TDAU2_MouseClick;
            // 
            // TDAU1ConnectBtn
            // 
            TDAU1ConnectBtn.BackColor = Color.RoyalBlue;
            TDAU1ConnectBtn.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            TDAU1ConnectBtn.ForeColor = Color.White;
            TDAU1ConnectBtn.Location = new Point(286, 501);
            TDAU1ConnectBtn.Name = "TDAU1ConnectBtn";
            TDAU1ConnectBtn.Size = new Size(119, 26);
            TDAU1ConnectBtn.TabIndex = 14;
            TDAU1ConnectBtn.Text = "Connect";
            TDAU1ConnectBtn.UseVisualStyleBackColor = false;
            TDAU1ConnectBtn.Click += TDAU1ConnectBtn_Click;
            // 
            // TDAU2ConnectBtn
            // 
            TDAU2ConnectBtn.BackColor = Color.RoyalBlue;
            TDAU2ConnectBtn.Enabled = false;
            TDAU2ConnectBtn.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            TDAU2ConnectBtn.ForeColor = Color.White;
            TDAU2ConnectBtn.Location = new Point(286, 534);
            TDAU2ConnectBtn.Name = "TDAU2ConnectBtn";
            TDAU2ConnectBtn.Size = new Size(119, 26);
            TDAU2ConnectBtn.TabIndex = 16;
            TDAU2ConnectBtn.Text = "Connect";
            TDAU2ConnectBtn.UseVisualStyleBackColor = false;
            TDAU2ConnectBtn.Click += TDAU2ConnectBtn_Click;
            // 
            // unitsText
            // 
            unitsText.AutoSize = true;
            unitsText.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            unitsText.Location = new Point(12, 473);
            unitsText.Name = "unitsText";
            unitsText.Size = new Size(73, 32);
            unitsText.TabIndex = 35;
            unitsText.Text = "Units";
            // 
            // userMailText
            // 
            userMailText.AutoSize = true;
            userMailText.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            userMailText.Location = new Point(27, 614);
            userMailText.Name = "userMailText";
            userMailText.Size = new Size(109, 25);
            userMailText.TabIndex = 44;
            userMailText.Text = "User Email:";
            // 
            // email
            // 
            email.BackColor = Color.White;
            email.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            email.Location = new Point(141, 610);
            email.Name = "email";
            email.Size = new Size(259, 32);
            email.TabIndex = 17;
            email.KeyPress += email_KeyPress;
            // 
            // startTestBtn
            // 
            startTestBtn.BackColor = Color.RoyalBlue;
            startTestBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            startTestBtn.ForeColor = Color.White;
            startTestBtn.Location = new Point(27, 648);
            startTestBtn.Name = "startTestBtn";
            startTestBtn.Size = new Size(373, 53);
            startTestBtn.TabIndex = 18;
            startTestBtn.Text = "Start test!";
            startTestBtn.UseVisualStyleBackColor = false;
            startTestBtn.Click += startTestBtn_Click;
            // 
            // Ie1List
            // 
            Ie1List.BackColor = Color.White;
            Ie1List.BorderStyle = BorderStyle.None;
            Ie1List.FormattingEnabled = true;
            Ie1List.ItemHeight = 15;
            Ie1List.Location = new Point(7, 44);
            Ie1List.Name = "Ie1List";
            Ie1List.Size = new Size(107, 60);
            Ie1List.TabIndex = 48;
            // 
            // Ie2List
            // 
            Ie2List.BackColor = Color.White;
            Ie2List.BorderStyle = BorderStyle.None;
            Ie2List.FormattingEnabled = true;
            Ie2List.ItemHeight = 15;
            Ie2List.Location = new Point(6, 44);
            Ie2List.Name = "Ie2List";
            Ie2List.Size = new Size(107, 60);
            Ie2List.TabIndex = 49;
            // 
            // Ie3List
            // 
            Ie3List.BackColor = Color.White;
            Ie3List.BorderStyle = BorderStyle.None;
            Ie3List.FormattingEnabled = true;
            Ie3List.ItemHeight = 15;
            Ie3List.Location = new Point(6, 44);
            Ie3List.Name = "Ie3List";
            Ie3List.Size = new Size(109, 60);
            Ie3List.TabIndex = 50;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(Ie3CleanBtn);
            groupBox1.Controls.Add(Ie3List);
            groupBox1.Controls.Add(Ie3);
            groupBox1.Location = new Point(279, 181);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(121, 146);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            // 
            // button1
            // 
            button1.BackColor = Color.RoyalBlue;
            button1.ForeColor = Color.White;
            button1.Location = new Point(57, 13);
            button1.Name = "button1";
            button1.Size = new Size(58, 25);
            button1.TabIndex = 51;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(Ie2AddBTN);
            groupBox2.Controls.Add(Ie2CleanBtn);
            groupBox2.Controls.Add(Ie2List);
            groupBox2.Controls.Add(Ie2);
            groupBox2.Location = new Point(153, 181);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(120, 146);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            // 
            // Ie2AddBTN
            // 
            Ie2AddBTN.BackColor = Color.RoyalBlue;
            Ie2AddBTN.ForeColor = Color.White;
            Ie2AddBTN.Location = new Point(57, 12);
            Ie2AddBTN.Name = "Ie2AddBTN";
            Ie2AddBTN.Size = new Size(56, 27);
            Ie2AddBTN.TabIndex = 50;
            Ie2AddBTN.Text = "Add";
            Ie2AddBTN.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(Ie1AddBTN);
            groupBox3.Controls.Add(Ie1CleanBtn);
            groupBox3.Controls.Add(Ie1List);
            groupBox3.Controls.Add(Ie1);
            groupBox3.Location = new Point(27, 181);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(120, 146);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            // 
            // Ie1AddBTN
            // 
            Ie1AddBTN.BackColor = Color.RoyalBlue;
            Ie1AddBTN.ForeColor = Color.White;
            Ie1AddBTN.Location = new Point(58, 12);
            Ie1AddBTN.Name = "Ie1AddBTN";
            Ie1AddBTN.Size = new Size(56, 26);
            Ie1AddBTN.TabIndex = 49;
            Ie1AddBTN.Text = "Add";
            Ie1AddBTN.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            label1.Location = new Point(12, 4);
            label1.Name = "label1";
            label1.Size = new Size(122, 32);
            label1.TabIndex = 57;
            label1.Text = "Metadata";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            label2.Location = new Point(12, 69);
            label2.Name = "label2";
            label2.Size = new Size(100, 32);
            label2.TabIndex = 58;
            label2.Text = "Voltage";
            // 
            // vccGroup
            // 
            vccGroup.Controls.Add(vccText);
            vccGroup.Controls.Add(vcc);
            vccGroup.Controls.Add(vccEnDis);
            vccGroup.FlatStyle = FlatStyle.Flat;
            vccGroup.Location = new Point(27, 104);
            vccGroup.Margin = new Padding(2);
            vccGroup.Name = "vccGroup";
            vccGroup.Padding = new Padding(2);
            vccGroup.Size = new Size(373, 39);
            vccGroup.TabIndex = 59;
            vccGroup.TabStop = false;
            // 
            // vccText
            // 
            vccText.AutoSize = true;
            vccText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            vccText.Location = new Point(142, 15);
            vccText.Name = "vccText";
            vccText.Size = new Size(0, 25);
            vccText.TabIndex = 22;
            // 
            // EmailText
            // 
            EmailText.AutoSize = true;
            EmailText.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            EmailText.Location = new Point(12, 576);
            EmailText.Name = "EmailText";
            EmailText.Size = new Size(200, 32);
            EmailText.TabIndex = 61;
            EmailText.Text = "Email Recipients";
            // 
            // labelTemporary
            // 
            labelTemporary.AutoSize = true;
            labelTemporary.BackColor = Color.LemonChiffon;
            labelTemporary.Location = new Point(294, 37);
            labelTemporary.Name = "labelTemporary";
            labelTemporary.Size = new Size(0, 15);
            labelTemporary.TabIndex = 62;
            // 
            // timerTemporary
            // 
            timerTemporary.Enabled = true;
            timerTemporary.Tick += timerTemporary_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(423, 713);
            Controls.Add(labelTemporary);
            Controls.Add(EmailText);
            Controls.Add(vccGroup);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(startTestBtn);
            Controls.Add(email);
            Controls.Add(userMailText);
            Controls.Add(unitsText);
            Controls.Add(TDAU2ConnectBtn);
            Controls.Add(TDAU1ConnectBtn);
            Controls.Add(TDAU2);
            Controls.Add(TDAU1);
            Controls.Add(TDAUCard2Text);
            Controls.Add(TDAUCard1Text);
            Controls.Add(stepTemp);
            Controls.Add(tempStepText);
            Controls.Add(StepText);
            Controls.Add(highTemp);
            Controls.Add(highTempText);
            Controls.Add(HighText);
            Controls.Add(lowTemp);
            Controls.Add(lowTempText);
            Controls.Add(LowText);
            Controls.Add(temperature);
            Controls.Add(currents);
            Controls.Add(ProNameText);
            Controls.Add(projName);
            HelpButton = true;
            Name = "Form1";
            Text = "Thermal project";
            TransparencyKey = Color.Turquoise;
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            vccGroup.ResumeLayout(false);
            vccGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void U1_CheckedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
        private TextBox projName;
        private Label ProNameText;
        private TextBox vcc;
        private CheckBox vccEnDis;
        private TextBox Ie1;
        private Button Ie1CleanBtn;
        private Button Ie2CleanBtn;
        private TextBox Ie2;
        private Button Ie3CleanBtn;
        private TextBox Ie3;
        private Label currents;
        private Label temperature;
        private Label LowText;
        private Label lowTempText;
        private TextBox lowTemp;
        private TextBox highTemp;
        private Label highTempText;
        private Label HighText;
        private TextBox stepTemp;
        private Label tempStepText;
        private Label StepText;
        private Label TDAUCard1Text;
        private Label TDAUCard2Text;
        private ComboBox TDAU1;
        private ComboBox TDAU2;
        private Button TDAU1ConnectBtn;
        private Button TDAU2ConnectBtn;
        private Label unitsText;
        private Label userMailText;
        private TextBox email;
        private Button startTestBtn;
        private ListBox Ie1List;
        private ListBox Ie2List;
        private ListBox Ie3List;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private GroupBox vccGroup;
        private Label vccText;
        private Button button1;
        private Button Ie2AddBTN;
        private Button Ie1AddBTN;
        private Label EmailText;
        private Label labelTemporary;
        public System.Windows.Forms.Timer timerTemporary;
    }
}