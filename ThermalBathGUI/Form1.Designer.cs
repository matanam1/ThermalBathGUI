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
            ListViewItem listViewItem1 = new ListViewItem("");
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
            TDAU1ConnectBtn = new Button();
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
            listView1 = new ListView();
            TDAUCard = new ColumnHeader();
            COMPort = new ColumnHeader();
            columnHeader1 = new ColumnHeader();
            projStep = new TextBox();
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
            projName.Location = new Point(288, 72);
            projName.Margin = new Padding(5, 6, 5, 6);
            projName.Name = "projName";
            projName.Size = new Size(105, 51);
            projName.TabIndex = 1;
            projName.Text = "Name";
            projName.Enter += projName_Enter;
            projName.KeyPress += projName_KeyPress;
            projName.Leave += projName_Leave;
            // 
            // ProNameText
            // 
            ProNameText.AutoSize = true;
            ProNameText.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            ProNameText.Location = new Point(46, 74);
            ProNameText.Margin = new Padding(5, 0, 5, 0);
            ProNameText.Name = "ProNameText";
            ProNameText.Size = new Size(233, 45);
            ProNameText.TabIndex = 2;
            ProNameText.Text = "Project name:";
            // 
            // vcc
            // 
            vcc.AccessibleName = "";
            vcc.BackColor = SystemColors.Window;
            vcc.BorderStyle = BorderStyle.FixedSingle;
            vcc.Enabled = false;
            vcc.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            vcc.Location = new Point(40, 30);
            vcc.Margin = new Padding(5, 6, 5, 6);
            vcc.Name = "vcc";
            vcc.Size = new Size(192, 51);
            vcc.TabIndex = 3;
            vcc.Text = "Vcc";
            vcc.Enter += vcc_Enter;
            vcc.KeyPress += vcc_KeyPress;
            vcc.Leave += vcc_Leave;
            // 
            // vccEnDis
            // 
            vccEnDis.AutoSize = true;
            vccEnDis.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            vccEnDis.ForeColor = Color.Black;
            vccEnDis.Location = new Point(8, 45);
            vccEnDis.Margin = new Padding(5, 6, 5, 6);
            vccEnDis.Name = "vccEnDis";
            vccEnDis.RightToLeft = RightToLeft.Yes;
            vccEnDis.Size = new Size(22, 21);
            vccEnDis.TabIndex = 2;
            vccEnDis.UseVisualStyleBackColor = true;
            vccEnDis.CheckedChanged += vccEnDis_CheckedChanged;
            // 
            // Ie1
            // 
            Ie1.BackColor = Color.White;
            Ie1.BorderStyle = BorderStyle.FixedSingle;
            Ie1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Ie1.Location = new Point(12, 26);
            Ie1.Margin = new Padding(5, 6, 5, 6);
            Ie1.Name = "Ie1";
            Ie1.Size = new Size(76, 39);
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
            Ie1CleanBtn.Location = new Point(12, 228);
            Ie1CleanBtn.Margin = new Padding(5, 6, 5, 6);
            Ie1CleanBtn.Name = "Ie1CleanBtn";
            Ie1CleanBtn.Size = new Size(183, 52);
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
            Ie2CleanBtn.Location = new Point(10, 228);
            Ie2CleanBtn.Margin = new Padding(5, 6, 5, 6);
            Ie2CleanBtn.Name = "Ie2CleanBtn";
            Ie2CleanBtn.Size = new Size(183, 52);
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
            Ie2.Location = new Point(10, 26);
            Ie2.Margin = new Padding(5, 6, 5, 6);
            Ie2.Name = "Ie2";
            Ie2.Size = new Size(76, 39);
            Ie2.TabIndex = 5;
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
            Ie3CleanBtn.Location = new Point(10, 228);
            Ie3CleanBtn.Margin = new Padding(5, 6, 5, 6);
            Ie3CleanBtn.Name = "Ie3CleanBtn";
            Ie3CleanBtn.Size = new Size(187, 52);
            Ie3CleanBtn.TabIndex = 9;
            Ie3CleanBtn.Text = "Clean";
            Ie3CleanBtn.UseVisualStyleBackColor = false;
            Ie3CleanBtn.Click += Ie3CleanBtn_Click;
            // 
            // Ie3
            // 
            Ie3.BackColor = Color.White;
            Ie3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Ie3.Location = new Point(10, 26);
            Ie3.Margin = new Padding(5, 6, 5, 6);
            Ie3.Name = "Ie3";
            Ie3.Size = new Size(78, 39);
            Ie3.TabIndex = 6;
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
            currents.Location = new Point(21, 306);
            currents.Margin = new Padding(5, 0, 5, 0);
            currents.Name = "currents";
            currents.Size = new Size(203, 57);
            currents.TabIndex = 18;
            currents.Text = "Currents:";
            // 
            // temperature
            // 
            temperature.AutoSize = true;
            temperature.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            temperature.Location = new Point(21, 678);
            temperature.Margin = new Padding(5, 0, 5, 0);
            temperature.Name = "temperature";
            temperature.Size = new Size(288, 57);
            temperature.TabIndex = 19;
            temperature.Text = "Temperature:";
            // 
            // LowText
            // 
            LowText.AutoSize = true;
            LowText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            LowText.ImageAlign = ContentAlignment.BottomCenter;
            LowText.Location = new Point(46, 742);
            LowText.Margin = new Padding(5, 0, 5, 0);
            LowText.Name = "LowText";
            LowText.Size = new Size(86, 45);
            LowText.TabIndex = 20;
            LowText.Text = "Low:";
            // 
            // lowTempText
            // 
            lowTempText.AutoSize = true;
            lowTempText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lowTempText.Location = new Point(348, 744);
            lowTempText.Margin = new Padding(5, 0, 5, 0);
            lowTempText.Name = "lowTempText";
            lowTempText.Size = new Size(0, 45);
            lowTempText.TabIndex = 21;
            // 
            // lowTemp
            // 
            lowTemp.BackColor = Color.White;
            lowTemp.BorderStyle = BorderStyle.FixedSingle;
            lowTemp.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lowTemp.Location = new Point(151, 736);
            lowTemp.Margin = new Padding(5, 6, 5, 6);
            lowTemp.Name = "lowTemp";
            lowTemp.Size = new Size(170, 51);
            lowTemp.TabIndex = 7;
            lowTemp.Text = "°C";
            lowTemp.Enter += lowTemp_Enter;
            lowTemp.KeyPress += lowTemp_KeyPress;
            lowTemp.Leave += lowTemp_Leave;
            // 
            // highTemp
            // 
            highTemp.BackColor = Color.White;
            highTemp.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            highTemp.Location = new Point(151, 796);
            highTemp.Margin = new Padding(5, 6, 5, 6);
            highTemp.Name = "highTemp";
            highTemp.Size = new Size(169, 51);
            highTemp.TabIndex = 8;
            highTemp.Text = "°C";
            highTemp.Enter += highTemp_Enter;
            highTemp.KeyPress += highTemp_KeyPress;
            highTemp.Leave += highTemp_Leave;
            // 
            // highTempText
            // 
            highTempText.AutoSize = true;
            highTempText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            highTempText.Location = new Point(348, 806);
            highTempText.Margin = new Padding(5, 0, 5, 0);
            highTempText.Name = "highTempText";
            highTempText.Size = new Size(0, 45);
            highTempText.TabIndex = 24;
            // 
            // HighText
            // 
            HighText.AutoSize = true;
            HighText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            HighText.ImageAlign = ContentAlignment.BottomCenter;
            HighText.Location = new Point(46, 802);
            HighText.Margin = new Padding(5, 0, 5, 0);
            HighText.Name = "HighText";
            HighText.Size = new Size(96, 45);
            HighText.TabIndex = 23;
            HighText.Text = "High:";
            // 
            // stepTemp
            // 
            stepTemp.BackColor = Color.White;
            stepTemp.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            stepTemp.Location = new Point(151, 858);
            stepTemp.Margin = new Padding(5, 6, 5, 6);
            stepTemp.Name = "stepTemp";
            stepTemp.Size = new Size(169, 51);
            stepTemp.TabIndex = 9;
            stepTemp.KeyPress += stepTemp_KeyPress;
            // 
            // tempStepText
            // 
            tempStepText.AutoSize = true;
            tempStepText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            tempStepText.Location = new Point(348, 868);
            tempStepText.Margin = new Padding(5, 0, 5, 0);
            tempStepText.Name = "tempStepText";
            tempStepText.Size = new Size(0, 45);
            tempStepText.TabIndex = 27;
            // 
            // StepText
            // 
            StepText.AutoSize = true;
            StepText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            StepText.ImageAlign = ContentAlignment.BottomCenter;
            StepText.Location = new Point(46, 864);
            StepText.Margin = new Padding(5, 0, 5, 0);
            StepText.Name = "StepText";
            StepText.Size = new Size(91, 45);
            StepText.TabIndex = 26;
            StepText.Text = "Step:";
            // 
            // TDAU1ConnectBtn
            // 
            TDAU1ConnectBtn.BackColor = Color.RoyalBlue;
            TDAU1ConnectBtn.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            TDAU1ConnectBtn.ForeColor = Color.White;
            TDAU1ConnectBtn.Location = new Point(46, 1021);
            TDAU1ConnectBtn.Margin = new Padding(5, 6, 5, 6);
            TDAU1ConnectBtn.Name = "TDAU1ConnectBtn";
            TDAU1ConnectBtn.Size = new Size(204, 52);
            TDAU1ConnectBtn.TabIndex = 14;
            TDAU1ConnectBtn.Text = "Add TDAU";
            TDAU1ConnectBtn.UseVisualStyleBackColor = false;
            TDAU1ConnectBtn.Click += TDAU1ConnectBtn_Click;
            // 
            // unitsText
            // 
            unitsText.AutoSize = true;
            unitsText.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            unitsText.Location = new Point(21, 946);
            unitsText.Margin = new Padding(5, 0, 5, 0);
            unitsText.Name = "unitsText";
            unitsText.Size = new Size(139, 57);
            unitsText.TabIndex = 35;
            unitsText.Text = "TDAU";
            // 
            // userMailText
            // 
            userMailText.AutoSize = true;
            userMailText.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            userMailText.Location = new Point(46, 1228);
            userMailText.Margin = new Padding(5, 0, 5, 0);
            userMailText.Name = "userMailText";
            userMailText.Size = new Size(192, 45);
            userMailText.TabIndex = 44;
            userMailText.Text = "User Email:";
            // 
            // email
            // 
            email.BackColor = Color.White;
            email.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            email.Location = new Point(242, 1220);
            email.Margin = new Padding(5, 6, 5, 6);
            email.Name = "email";
            email.Size = new Size(441, 51);
            email.TabIndex = 17;
            email.KeyPress += email_KeyPress;
            // 
            // startTestBtn
            // 
            startTestBtn.BackColor = Color.RoyalBlue;
            startTestBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            startTestBtn.ForeColor = Color.White;
            startTestBtn.Location = new Point(46, 1296);
            startTestBtn.Margin = new Padding(5, 6, 5, 6);
            startTestBtn.Name = "startTestBtn";
            startTestBtn.Size = new Size(639, 106);
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
            Ie1List.ItemHeight = 30;
            Ie1List.Location = new Point(12, 88);
            Ie1List.Margin = new Padding(5, 6, 5, 6);
            Ie1List.Name = "Ie1List";
            Ie1List.Size = new Size(183, 120);
            Ie1List.TabIndex = 48;
            // 
            // Ie2List
            // 
            Ie2List.BackColor = Color.White;
            Ie2List.BorderStyle = BorderStyle.None;
            Ie2List.FormattingEnabled = true;
            Ie2List.ItemHeight = 30;
            Ie2List.Location = new Point(10, 88);
            Ie2List.Margin = new Padding(5, 6, 5, 6);
            Ie2List.Name = "Ie2List";
            Ie2List.Size = new Size(183, 120);
            Ie2List.TabIndex = 49;
            // 
            // Ie3List
            // 
            Ie3List.BackColor = Color.White;
            Ie3List.BorderStyle = BorderStyle.None;
            Ie3List.FormattingEnabled = true;
            Ie3List.ItemHeight = 30;
            Ie3List.Location = new Point(10, 88);
            Ie3List.Margin = new Padding(5, 6, 5, 6);
            Ie3List.Name = "Ie3List";
            Ie3List.Size = new Size(187, 120);
            Ie3List.TabIndex = 50;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(Ie3CleanBtn);
            groupBox1.Controls.Add(Ie3List);
            groupBox1.Controls.Add(Ie3);
            groupBox1.Location = new Point(478, 362);
            groupBox1.Margin = new Padding(5, 6, 5, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(5, 6, 5, 6);
            groupBox1.Size = new Size(207, 292);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            // 
            // button1
            // 
            button1.BackColor = Color.RoyalBlue;
            button1.ForeColor = Color.White;
            button1.Location = new Point(98, 26);
            button1.Margin = new Padding(5, 6, 5, 6);
            button1.Name = "button1";
            button1.Size = new Size(99, 50);
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
            groupBox2.Location = new Point(262, 362);
            groupBox2.Margin = new Padding(5, 6, 5, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(5, 6, 5, 6);
            groupBox2.Size = new Size(206, 292);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            // 
            // Ie2AddBTN
            // 
            Ie2AddBTN.BackColor = Color.RoyalBlue;
            Ie2AddBTN.ForeColor = Color.White;
            Ie2AddBTN.Location = new Point(98, 24);
            Ie2AddBTN.Margin = new Padding(5, 6, 5, 6);
            Ie2AddBTN.Name = "Ie2AddBTN";
            Ie2AddBTN.Size = new Size(96, 54);
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
            groupBox3.Location = new Point(46, 362);
            groupBox3.Margin = new Padding(5, 6, 5, 6);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(5, 6, 5, 6);
            groupBox3.Size = new Size(206, 292);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            // 
            // Ie1AddBTN
            // 
            Ie1AddBTN.BackColor = Color.RoyalBlue;
            Ie1AddBTN.ForeColor = Color.White;
            Ie1AddBTN.Location = new Point(99, 24);
            Ie1AddBTN.Margin = new Padding(5, 6, 5, 6);
            Ie1AddBTN.Name = "Ie1AddBTN";
            Ie1AddBTN.Size = new Size(96, 52);
            Ie1AddBTN.TabIndex = 49;
            Ie1AddBTN.Text = "Add";
            Ie1AddBTN.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            label1.Location = new Point(21, 8);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(215, 57);
            label1.TabIndex = 57;
            label1.Text = "Metadata";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            label2.Location = new Point(21, 138);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(176, 57);
            label2.TabIndex = 58;
            label2.Text = "Voltage";
            // 
            // vccGroup
            // 
            vccGroup.Controls.Add(vccText);
            vccGroup.Controls.Add(vcc);
            vccGroup.Controls.Add(vccEnDis);
            vccGroup.FlatStyle = FlatStyle.Flat;
            vccGroup.Location = new Point(46, 208);
            vccGroup.Margin = new Padding(3, 4, 3, 4);
            vccGroup.Name = "vccGroup";
            vccGroup.Padding = new Padding(3, 4, 3, 4);
            vccGroup.Size = new Size(639, 94);
            vccGroup.TabIndex = 59;
            vccGroup.TabStop = false;
            // 
            // vccText
            // 
            vccText.AutoSize = true;
            vccText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            vccText.Location = new Point(242, 34);
            vccText.Margin = new Padding(5, 0, 5, 0);
            vccText.Name = "vccText";
            vccText.Size = new Size(0, 45);
            vccText.TabIndex = 22;
            // 
            // EmailText
            // 
            EmailText.AutoSize = true;
            EmailText.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            EmailText.Location = new Point(21, 1152);
            EmailText.Margin = new Padding(5, 0, 5, 0);
            EmailText.Name = "EmailText";
            EmailText.Size = new Size(345, 57);
            EmailText.TabIndex = 61;
            EmailText.Text = "Email Recipients";
            // 
            // labelTemporary
            // 
            labelTemporary.AutoSize = true;
            labelTemporary.BackColor = Color.LemonChiffon;
            labelTemporary.Location = new Point(513, 74);
            labelTemporary.Margin = new Padding(5, 0, 5, 0);
            labelTemporary.Name = "labelTemporary";
            labelTemporary.Size = new Size(0, 30);
            labelTemporary.TabIndex = 62;
            // 
            // timerTemporary
            // 
            timerTemporary.Enabled = true;
            timerTemporary.Tick += timerTemporary_Tick;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { TDAUCard, COMPort, columnHeader1 });
            listView1.Items.AddRange(new ListViewItem[] { listViewItem1 });
            listView1.Location = new Point(327, 979);
            listView1.Name = "listView1";
            listView1.Size = new Size(356, 170);
            listView1.TabIndex = 63;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // TDAUCard
            // 
            TDAUCard.Text = "TDAU";
            TDAUCard.Width = 70;
            // 
            // COMPort
            // 
            COMPort.Text = "COM";
            COMPort.Width = 80;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "SN";
            columnHeader1.Width = 100;
            // 
            // projStep
            // 
            projStep.BackColor = Color.White;
            projStep.BorderStyle = BorderStyle.FixedSingle;
            projStep.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            projStep.Location = new Point(411, 72);
            projStep.Margin = new Padding(5, 6, 5, 6);
            projStep.Name = "projStep";
            projStep.Size = new Size(105, 51);
            projStep.TabIndex = 2;
            projStep.Text = "Step";
            projStep.Enter += projStep_Enter;
            projStep.KeyPress += projStep_KeyPress;
            projStep.Leave += projStep_Leave;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(726, 1436);
            Controls.Add(projStep);
            Controls.Add(listView1);
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
            Controls.Add(TDAU1ConnectBtn);
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
            Margin = new Padding(5, 6, 5, 6);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
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
        private Button TDAU1ConnectBtn;
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
        private ListView listView1;
        private ColumnHeader TDAUCard;
        private ColumnHeader COMPort;
        private ColumnHeader columnHeader1;
        private TextBox projStep;
    }
}