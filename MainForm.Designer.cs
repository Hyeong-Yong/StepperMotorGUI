﻿
using System.Windows.Forms;

namespace MotorGUI
{
    partial class MotorGUI
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
            this.components = new System.ComponentModel.Container();
            this.gbConnect = new System.Windows.Forms.GroupBox();
            this.txtBaudrate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPortname = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtRead = new System.Windows.Forms.RichTextBox();
            this.txtWrite = new System.Windows.Forms.TextBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.serialPortIn = new System.IO.Ports.SerialPort(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMotorGUISet = new System.Windows.Forms.Button();
            this.btnMotorGUIAct = new System.Windows.Forms.Button();
            this.rbtnAntiClockwise = new System.Windows.Forms.RadioButton();
            this.rbtnClockwise = new System.Windows.Forms.RadioButton();
            this.txtMotorGUIAngle = new System.Windows.Forms.TextBox();
            this.txtMotorGUISpeed = new System.Windows.Forms.TextBox();
            this.domainUpDown1 = new System.Windows.Forms.DomainUpDown();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.gbConnect.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConnect
            // 
            this.gbConnect.Controls.Add(this.txtBaudrate);
            this.gbConnect.Controls.Add(this.label2);
            this.gbConnect.Controls.Add(this.txtPortname);
            this.gbConnect.Controls.Add(this.label1);
            this.gbConnect.Location = new System.Drawing.Point(12, 21);
            this.gbConnect.Name = "gbConnect";
            this.gbConnect.Size = new System.Drawing.Size(280, 141);
            this.gbConnect.TabIndex = 0;
            this.gbConnect.TabStop = false;
            this.gbConnect.Text = "Connection";
            // 
            // txtBaudrate
            // 
            this.txtBaudrate.FormattingEnabled = true;
            this.txtBaudrate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "14400",
            "57600",
            "115200"});
            this.txtBaudrate.Location = new System.Drawing.Point(118, 97);
            this.txtBaudrate.Name = "txtBaudrate";
            this.txtBaudrate.Size = new System.Drawing.Size(121, 23);
            this.txtBaudrate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud Rate :";
            // 
            // txtPortname
            // 
            this.txtPortname.FormattingEnabled = true;
            this.txtPortname.Location = new System.Drawing.Point(118, 51);
            this.txtPortname.Name = "txtPortname";
            this.txtPortname.Size = new System.Drawing.Size(121, 23);
            this.txtPortname.TabIndex = 1;
            this.txtPortname.DropDown += new System.EventHandler(this.txtPortname_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port Name :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSend);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.txtRead);
            this.groupBox1.Controls.Add(this.txtWrite);
            this.groupBox1.Location = new System.Drawing.Point(298, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 349);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Log";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(284, 42);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(284, 300);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtRead
            // 
            this.txtRead.Location = new System.Drawing.Point(16, 84);
            this.txtRead.Name = "txtRead";
            this.txtRead.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtRead.Size = new System.Drawing.Size(343, 210);
            this.txtRead.TabIndex = 1;
            this.txtRead.Text = "";
            // 
            // txtWrite
            // 
            this.txtWrite.Location = new System.Drawing.Point(16, 43);
            this.txtWrite.Name = "txtWrite";
            this.txtWrite.Size = new System.Drawing.Size(262, 25);
            this.txtWrite.TabIndex = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(98, 172);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(94, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(198, 172);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(98, 212);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(94, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(198, 212);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(94, 23);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "<Direction>";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 229);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "<MotorGUI Speed>";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "<MotorGUI Angle>";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMotorGUISet);
            this.groupBox2.Controls.Add(this.btnMotorGUIAct);
            this.groupBox2.Controls.Add(this.rbtnAntiClockwise);
            this.groupBox2.Controls.Add(this.rbtnClockwise);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtMotorGUIAngle);
            this.groupBox2.Controls.Add(this.txtMotorGUISpeed);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(693, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 342);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MotorGUI controller";
            // 
            // btnMotorGUISet
            // 
            this.btnMotorGUISet.Location = new System.Drawing.Point(133, 254);
            this.btnMotorGUISet.Name = "btnMotorGUISet";
            this.btnMotorGUISet.Size = new System.Drawing.Size(75, 23);
            this.btnMotorGUISet.TabIndex = 8;
            this.btnMotorGUISet.Text = "Send";
            this.btnMotorGUISet.UseVisualStyleBackColor = true;
            this.btnMotorGUISet.Click += new System.EventHandler(this.btnMotorGUISet_Click);
            // 
            // btnMotorGUIAct
            // 
            this.btnMotorGUIAct.Location = new System.Drawing.Point(133, 116);
            this.btnMotorGUIAct.Name = "btnMotorGUIAct";
            this.btnMotorGUIAct.Size = new System.Drawing.Size(75, 23);
            this.btnMotorGUIAct.TabIndex = 7;
            this.btnMotorGUIAct.Text = "Send";
            this.btnMotorGUIAct.UseVisualStyleBackColor = true;
            this.btnMotorGUIAct.Click += new System.EventHandler(this.btnMotorGUIAct_Click);
            // 
            // rbtnAntiClockwise
            // 
            this.rbtnAntiClockwise.AutoSize = true;
            this.rbtnAntiClockwise.Location = new System.Drawing.Point(121, 57);
            this.rbtnAntiClockwise.Name = "rbtnAntiClockwise";
            this.rbtnAntiClockwise.Size = new System.Drawing.Size(116, 19);
            this.rbtnAntiClockwise.TabIndex = 6;
            this.rbtnAntiClockwise.TabStop = true;
            this.rbtnAntiClockwise.Text = "Anticlockwise";
            this.rbtnAntiClockwise.UseVisualStyleBackColor = true;
            // 
            // rbtnClockwise
            // 
            this.rbtnClockwise.AutoSize = true;
            this.rbtnClockwise.Location = new System.Drawing.Point(21, 57);
            this.rbtnClockwise.Name = "rbtnClockwise";
            this.rbtnClockwise.Size = new System.Drawing.Size(94, 19);
            this.rbtnClockwise.TabIndex = 6;
            this.rbtnClockwise.TabStop = true;
            this.rbtnClockwise.Text = "Clockwise";
            this.rbtnClockwise.UseVisualStyleBackColor = true;
            // 
            // txtMotorGUIAngle
            // 
            this.txtMotorGUIAngle.Location = new System.Drawing.Point(21, 117);
            this.txtMotorGUIAngle.Name = "txtMotorGUIAngle";
            this.txtMotorGUIAngle.Size = new System.Drawing.Size(100, 25);
            this.txtMotorGUIAngle.TabIndex = 4;
            this.txtMotorGUIAngle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMotorGUIAngle_KeyPress);
            // 
            // txtMotorGUISpeed
            // 
            this.txtMotorGUISpeed.Location = new System.Drawing.Point(21, 253);
            this.txtMotorGUISpeed.Name = "txtMotorGUISpeed";
            this.txtMotorGUISpeed.Size = new System.Drawing.Size(100, 25);
            this.txtMotorGUISpeed.TabIndex = 4;
            this.txtMotorGUISpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMotorGUISpeed_KeyPress);
            // 
            // domainUpDown1
            // 
            this.domainUpDown1.Location = new System.Drawing.Point(39, 303);
            this.domainUpDown1.Name = "domainUpDown1";
            this.domainUpDown1.Size = new System.Drawing.Size(120, 25);
            this.domainUpDown1.TabIndex = 4;
            this.domainUpDown1.Text = "domainUpDown1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(39, 334);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(56, 257);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(80, 21);
            this.hScrollBar1.TabIndex = 6;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(151, 257);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(71, 15);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // MotorGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 445);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.domainUpDown1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbConnect);
            this.Controls.Add(this.btnEdit);
            this.Name = "MotorGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MotorGUI_Load);
            this.gbConnect.ResumeLayout(false);
            this.gbConnect.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.ComboBox txtBaudrate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox txtPortname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtRead;
        private System.Windows.Forms.TextBox txtWrite;
        private System.IO.Ports.SerialPort serialPortIn;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnAntiClockwise;
        private System.Windows.Forms.RadioButton rbtnClockwise;
        private System.Windows.Forms.TextBox txtMotorGUIAngle;
        private System.Windows.Forms.TextBox txtMotorGUISpeed;
        private System.Windows.Forms.Button btnMotorGUISet;
        private System.Windows.Forms.Button btnMotorGUIAct;
        private DomainUpDown domainUpDown1;
        private FlowLayoutPanel flowLayoutPanel1;
        private HScrollBar hScrollBar1;
        private LinkLabel linkLabel1;
    }
}

