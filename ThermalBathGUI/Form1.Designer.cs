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
            unitsText = new Label();
            userMailText = new Label();
            email = new TextBox();
            startTestBtn = new Button();
            Ie1List = new ListBox();
            Ie2List = new ListBox();
            Ie3List = new ListBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
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
            tdauPort = new ComboBox();
            textBox1 = new TextBox();
            connectTDAU = new Button();
            bathPort = new ComboBox();
            connectBath = new Button();
            tabControl1 = new TabControl();
            rangeTab = new TabPage();
            manualTab = new TabPage();
            temperatureCleanBtn = new Button();
            manTemperature = new TextBox();
            temperatureList = new ListBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            vccGroup.SuspendLayout();
            tabControl1.SuspendLayout();
            rangeTab.SuspendLayout();
            manualTab.SuspendLayout();
            SuspendLayout();
            // 
            // projName
            // 
            projName.BackColor = Color.White;
            projName.BorderStyle = BorderStyle.FixedSingle;
            projName.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            projName.Location = new Point(168, 38);
            projName.Name = "projName";
            projName.Size = new Size(62, 27);
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
            vcc.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            vcc.Location = new Point(23, 15);
            vcc.Name = "vcc";
            vcc.Size = new Size(113, 32);
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
            vccEnDis.Location = new Point(5, 22);
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
            Ie1.Size = new Size(107, 25);
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
            Ie2.Size = new Size(108, 25);
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
            Ie3.Size = new Size(109, 25);
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
            LowText.Location = new Point(6, 2);
            LowText.Name = "LowText";
            LowText.Size = new Size(50, 25);
            LowText.TabIndex = 20;
            LowText.Text = "Low:";
            // 
            // lowTempText
            // 
            lowTempText.AutoSize = true;
            lowTempText.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lowTempText.Location = new Point(121, 6);
            lowTempText.Name = "lowTempText";
            lowTempText.Size = new Size(0, 20);
            lowTempText.TabIndex = 21;
            // 
            // lowTemp
            // 
            lowTemp.BackColor = Color.White;
            lowTemp.BorderStyle = BorderStyle.FixedSingle;
            lowTemp.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            lowTemp.Location = new Point(60, 2);
            lowTemp.Name = "lowTemp";
            lowTemp.Size = new Size(50, 27);
            lowTemp.TabIndex = 7;
            lowTemp.Text = "°C";
            lowTemp.Enter += lowTemp_Enter;
            lowTemp.KeyPress += lowTemp_KeyPress;
            lowTemp.Leave += lowTemp_Leave;
            // 
            // highTemp
            // 
            highTemp.BackColor = Color.White;
            highTemp.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            highTemp.Location = new Point(60, 31);
            highTemp.Name = "highTemp";
            highTemp.Size = new Size(50, 27);
            highTemp.TabIndex = 8;
            highTemp.Text = "°C";
            highTemp.Enter += highTemp_Enter;
            highTemp.KeyPress += highTemp_KeyPress;
            highTemp.Leave += highTemp_Leave;
            // 
            // highTempText
            // 
            highTempText.AutoSize = true;
            highTempText.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            highTempText.Location = new Point(121, 36);
            highTempText.Name = "highTempText";
            highTempText.Size = new Size(0, 20);
            highTempText.TabIndex = 24;
            // 
            // HighText
            // 
            HighText.AutoSize = true;
            HighText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            HighText.ImageAlign = ContentAlignment.BottomCenter;
            HighText.Location = new Point(4, 31);
            HighText.Name = "HighText";
            HighText.Size = new Size(56, 25);
            HighText.TabIndex = 23;
            HighText.Text = "High:";
            // 
            // stepTemp
            // 
            stepTemp.BackColor = Color.White;
            stepTemp.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            stepTemp.Location = new Point(60, 62);
            stepTemp.Name = "stepTemp";
            stepTemp.Size = new Size(50, 27);
            stepTemp.TabIndex = 9;
            stepTemp.KeyPress += stepTemp_KeyPress;
            // 
            // tempStepText
            // 
            tempStepText.AutoSize = true;
            tempStepText.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            tempStepText.Location = new Point(121, 67);
            tempStepText.Name = "tempStepText";
            tempStepText.Size = new Size(0, 20);
            tempStepText.TabIndex = 27;
            // 
            // StepText
            // 
            StepText.AutoSize = true;
            StepText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            StepText.ImageAlign = ContentAlignment.BottomCenter;
            StepText.Location = new Point(6, 62);
            StepText.Name = "StepText";
            StepText.Size = new Size(52, 25);
            StepText.TabIndex = 26;
            StepText.Text = "Step:";
            // 
            // unitsText
            // 
            unitsText.AutoSize = true;
            unitsText.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            unitsText.Location = new Point(12, 495);
            unitsText.Name = "unitsText";
            unitsText.Size = new Size(80, 32);
            unitsText.TabIndex = 35;
            unitsText.Text = "TDAU";
            // 
            // userMailText
            // 
            userMailText.AutoSize = true;
            userMailText.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            userMailText.Location = new Point(27, 636);
            userMailText.Name = "userMailText";
            userMailText.Size = new Size(109, 25);
            userMailText.TabIndex = 44;
            userMailText.Text = "User Email:";
            // 
            // email
            // 
            email.BackColor = Color.White;
            email.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            email.Location = new Point(141, 632);
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
            startTestBtn.Location = new Point(27, 670);
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
            groupBox1.Controls.Add(Ie3CleanBtn);
            groupBox1.Controls.Add(Ie3List);
            groupBox1.Controls.Add(Ie3);
            groupBox1.Location = new Point(279, 182);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(121, 146);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(Ie2CleanBtn);
            groupBox2.Controls.Add(Ie2List);
            groupBox2.Controls.Add(Ie2);
            groupBox2.Location = new Point(153, 182);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(120, 146);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(Ie1CleanBtn);
            groupBox3.Controls.Add(Ie1List);
            groupBox3.Controls.Add(Ie1);
            groupBox3.Location = new Point(27, 182);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(120, 146);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
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
            vccGroup.Size = new Size(373, 47);
            vccGroup.TabIndex = 59;
            vccGroup.TabStop = false;
            // 
            // vccText
            // 
            vccText.AutoSize = true;
            vccText.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            vccText.Location = new Point(141, 17);
            vccText.Name = "vccText";
            vccText.Size = new Size(0, 25);
            vccText.TabIndex = 22;
            // 
            // EmailText
            // 
            EmailText.AutoSize = true;
            EmailText.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
            EmailText.Location = new Point(12, 598);
            EmailText.Name = "EmailText";
            EmailText.Size = new Size(200, 32);
            EmailText.TabIndex = 61;
            EmailText.Text = "Email Recipients";
            // 
            // labelTemporary
            // 
            labelTemporary.AutoSize = true;
            labelTemporary.BackColor = Color.LemonChiffon;
            labelTemporary.Location = new Point(299, 40);
            labelTemporary.Name = "labelTemporary";
            labelTemporary.Size = new Size(0, 15);
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
            listView1.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            listView1.Items.AddRange(new ListViewItem[] { listViewItem1 });
            listView1.Location = new Point(240, 530);
            listView1.Margin = new Padding(2);
            listView1.Name = "listView1";
            listView1.Size = new Size(154, 77);
            listView1.TabIndex = 63;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // TDAUCard
            // 
            TDAUCard.Text = "TDAU";
            TDAUCard.Width = 45;
            // 
            // COMPort
            // 
            COMPort.Text = "COM";
            COMPort.Width = 40;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "SN";
            // 
            // projStep
            // 
            projStep.BackColor = Color.White;
            projStep.BorderStyle = BorderStyle.FixedSingle;
            projStep.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            projStep.Location = new Point(240, 38);
            projStep.Name = "projStep";
            projStep.Size = new Size(62, 27);
            projStep.TabIndex = 2;
            projStep.Text = "Step";
            projStep.Enter += projStep_Enter;
            projStep.KeyPress += projStep_KeyPress;
            projStep.Leave += projStep_Leave;
            // 
            // tdauPort
            // 
            tdauPort.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            tdauPort.FormattingEnabled = true;
            tdauPort.ItemHeight = 20;
            tdauPort.Location = new Point(175, 530);
            tdauPort.Margin = new Padding(2);
            tdauPort.Name = "tdauPort";
            tdauPort.Size = new Size(40, 28);
            tdauPort.TabIndex = 64;
            tdauPort.MouseClick += tdauPort_MouseClick;
            // 
            // textBox1
            // 
            textBox1.AccessibleName = "";
            textBox1.BackColor = SystemColors.Window;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Enabled = false;
            textBox1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(52, 119);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(113, 32);
            textBox1.TabIndex = 3;
            textBox1.Text = "Vcc";
            textBox1.Enter += vcc_Enter;
            textBox1.KeyPress += vcc_KeyPress;
            textBox1.Leave += vcc_Leave;
            // 
            // connectTDAU
            // 
            connectTDAU.BackColor = Color.RoyalBlue;
            connectTDAU.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            connectTDAU.ForeColor = Color.White;
            connectTDAU.Location = new Point(27, 558);
            connectTDAU.Name = "connectTDAU";
            connectTDAU.Size = new Size(119, 28);
            connectTDAU.TabIndex = 65;
            connectTDAU.Text = "Connect TDAU";
            connectTDAU.UseVisualStyleBackColor = false;
            connectTDAU.Click += connectTDAU_Click;
            // 
            // bathPort
            // 
            bathPort.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            bathPort.FormattingEnabled = true;
            bathPort.ItemHeight = 20;
            bathPort.Location = new Point(240, 398);
            bathPort.Margin = new Padding(2);
            bathPort.Name = "bathPort";
            bathPort.Size = new Size(40, 28);
            bathPort.TabIndex = 67;
            bathPort.MouseClick += bathPort_MouseClick;
            // 
            // connectBath
            // 
            connectBath.BackColor = Color.RoyalBlue;
            connectBath.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            connectBath.ForeColor = Color.White;
            connectBath.Location = new Point(293, 398);
            connectBath.Name = "connectBath";
            connectBath.Size = new Size(107, 28);
            connectBath.TabIndex = 66;
            connectBath.Text = "Connect bath";
            connectBath.UseVisualStyleBackColor = false;
            connectBath.Click += connectBath_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(rangeTab);
            tabControl1.Controls.Add(manualTab);
            tabControl1.Location = new Point(27, 374);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(200, 119);
            tabControl1.TabIndex = 69;
            // 
            // rangeTab
            // 
            rangeTab.Controls.Add(LowText);
            rangeTab.Controls.Add(lowTempText);
            rangeTab.Controls.Add(lowTemp);
            rangeTab.Controls.Add(HighText);
            rangeTab.Controls.Add(highTempText);
            rangeTab.Controls.Add(highTemp);
            rangeTab.Controls.Add(StepText);
            rangeTab.Controls.Add(tempStepText);
            rangeTab.Controls.Add(stepTemp);
            rangeTab.Location = new Point(4, 24);
            rangeTab.Name = "rangeTab";
            rangeTab.Padding = new Padding(3);
            rangeTab.Size = new Size(192, 91);
            rangeTab.TabIndex = 0;
            rangeTab.Text = "Range Input";
            rangeTab.UseVisualStyleBackColor = true;
            // 
            // manualTab
            // 
            manualTab.Controls.Add(temperatureCleanBtn);
            manualTab.Controls.Add(manTemperature);
            manualTab.Controls.Add(temperatureList);
            manualTab.Location = new Point(4, 24);
            manualTab.Name = "manualTab";
            manualTab.Padding = new Padding(3);
            manualTab.Size = new Size(192, 91);
            manualTab.TabIndex = 1;
            manualTab.Text = "Manual Input";
            manualTab.UseVisualStyleBackColor = true;
            // 
            // temperatureCleanBtn
            // 
            temperatureCleanBtn.BackColor = Color.RoyalBlue;
            temperatureCleanBtn.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            temperatureCleanBtn.ForeColor = Color.White;
            temperatureCleanBtn.Location = new Point(6, 55);
            temperatureCleanBtn.Name = "temperatureCleanBtn";
            temperatureCleanBtn.Size = new Size(70, 30);
            temperatureCleanBtn.TabIndex = 72;
            temperatureCleanBtn.Text = "Clean";
            temperatureCleanBtn.UseVisualStyleBackColor = false;
            temperatureCleanBtn.Click += temperatureCleanBtn_Click;
            // 
            // manTemperature
            // 
            manTemperature.Location = new Point(6, 6);
            manTemperature.Name = "manTemperature";
            manTemperature.Size = new Size(70, 23);
            manTemperature.TabIndex = 71;
            manTemperature.Text = "°C";
            manTemperature.Enter += manTemperature_Enter;
            manTemperature.KeyPress += manTemperature_KeyPress;
            manTemperature.Leave += manTemperature_Leave;
            // 
            // temperatureList
            // 
            temperatureList.FormattingEnabled = true;
            temperatureList.ItemHeight = 15;
            temperatureList.Location = new Point(82, 6);
            temperatureList.Name = "temperatureList";
            temperatureList.Size = new Size(104, 79);
            temperatureList.TabIndex = 70;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(424, 773);
            Controls.Add(tabControl1);
            Controls.Add(bathPort);
            Controls.Add(connectBath);
            Controls.Add(connectTDAU);
            Controls.Add(tdauPort);
            Controls.Add(textBox1);
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
            Controls.Add(temperature);
            Controls.Add(currents);
            Controls.Add(ProNameText);
            Controls.Add(projName);
            HelpButton = true;
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
            tabControl1.ResumeLayout(false);
            rangeTab.ResumeLayout(false);
            rangeTab.PerformLayout();
            manualTab.ResumeLayout(false);
            manualTab.PerformLayout();
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
        private Label EmailText;
        private Label labelTemporary;
        public System.Windows.Forms.Timer timerTemporary;
        private ListView listView1;
        private ColumnHeader TDAUCard;
        private ColumnHeader COMPort;
        private ColumnHeader columnHeader1;
        private TextBox projStep;
        private ComboBox tdauPort;
        private TextBox textBox1;
        private Button connectTDAU;
        private ComboBox bathPort;
        private Button connectBath;
        private TabControl tabControl1;
        private TabPage rangeTab;
        private TabPage manualTab;
        private ListBox temperatureList;
        private Button temperatureCleanBtn;
        private TextBox manTemperature;
    }
}