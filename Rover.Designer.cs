namespace RoverGUI
{
    partial class Rover
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rover));
            this.serialPortBox = new System.Windows.Forms.GroupBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.sendTextBox = new System.Windows.Forms.TextBox();
            this.sendLabel = new System.Windows.Forms.Label();
            this.keyboard = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.spinButton = new System.Windows.Forms.Button();
            this.changeGearButton = new System.Windows.Forms.Button();
            this.rightButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.ibOriginal = new Emgu.CV.UI.ImageBox();
            this.btnPauseOrResume = new System.Windows.Forms.Button();
            this.serialPortBox.SuspendLayout();
            this.keyboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ibOriginal)).BeginInit();
            this.SuspendLayout();
            // 
            // serialPortBox
            // 
            this.serialPortBox.Controls.Add(this.sendButton);
            this.serialPortBox.Controls.Add(this.sendTextBox);
            this.serialPortBox.Controls.Add(this.sendLabel);
            this.serialPortBox.Controls.Add(this.keyboard);
            this.serialPortBox.Controls.Add(this.connectButton);
            this.serialPortBox.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serialPortBox.Location = new System.Drawing.Point(12, 12);
            this.serialPortBox.Name = "serialPortBox";
            this.serialPortBox.Size = new System.Drawing.Size(381, 209);
            this.serialPortBox.TabIndex = 14;
            this.serialPortBox.TabStop = false;
            this.serialPortBox.Text = "Rover Connection";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(45, 165);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(90, 26);
            this.sendButton.TabIndex = 12;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendMouseClick);
            // 
            // sendTextBox
            // 
            this.sendTextBox.Location = new System.Drawing.Point(6, 128);
            this.sendTextBox.Name = "sendTextBox";
            this.sendTextBox.Size = new System.Drawing.Size(166, 26);
            this.sendTextBox.TabIndex = 4;
            // 
            // sendLabel
            // 
            this.sendLabel.AutoSize = true;
            this.sendLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendLabel.Location = new System.Drawing.Point(70, 106);
            this.sendLabel.Name = "sendLabel";
            this.sendLabel.Size = new System.Drawing.Size(40, 19);
            this.sendLabel.TabIndex = 9;
            this.sendLabel.Text = "Sent";
            // 
            // keyboard
            // 
            this.keyboard.Controls.Add(this.button1);
            this.keyboard.Controls.Add(this.spinButton);
            this.keyboard.Controls.Add(this.changeGearButton);
            this.keyboard.Controls.Add(this.rightButton);
            this.keyboard.Controls.Add(this.leftButton);
            this.keyboard.Controls.Add(this.backButton);
            this.keyboard.Controls.Add(this.upButton);
            this.keyboard.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyboard.Location = new System.Drawing.Point(178, 19);
            this.keyboard.Name = "keyboard";
            this.keyboard.Size = new System.Drawing.Size(179, 184);
            this.keyboard.TabIndex = 6;
            this.keyboard.TabStop = false;
            this.keyboard.Text = "Keyboard Controls";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 26);
            this.button1.TabIndex = 6;
            this.button1.Text = "H - Shift Gear Down";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // spinButton
            // 
            this.spinButton.Location = new System.Drawing.Point(6, 91);
            this.spinButton.Name = "spinButton";
            this.spinButton.Size = new System.Drawing.Size(84, 27);
            this.spinButton.TabIndex = 5;
            this.spinButton.Text = "C - Spin";
            this.spinButton.UseVisualStyleBackColor = true;
            // 
            // changeGearButton
            // 
            this.changeGearButton.Location = new System.Drawing.Point(6, 124);
            this.changeGearButton.Name = "changeGearButton";
            this.changeGearButton.Size = new System.Drawing.Size(163, 26);
            this.changeGearButton.TabIndex = 4;
            this.changeGearButton.Text = "G - Shift Gear Up";
            this.changeGearButton.UseVisualStyleBackColor = true;
            // 
            // rightButton
            // 
            this.rightButton.Location = new System.Drawing.Point(96, 56);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(73, 29);
            this.rightButton.TabIndex = 3;
            this.rightButton.Text = "D - Right";
            this.rightButton.UseVisualStyleBackColor = true;
            // 
            // leftButton
            // 
            this.leftButton.Location = new System.Drawing.Point(6, 56);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(84, 29);
            this.leftButton.TabIndex = 2;
            this.leftButton.Text = "A - Left";
            this.leftButton.UseVisualStyleBackColor = true;
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(96, 19);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 31);
            this.backButton.TabIndex = 1;
            this.backButton.Text = "S - Back";
            this.backButton.UseVisualStyleBackColor = true;
            // 
            // upButton
            // 
            this.upButton.Location = new System.Drawing.Point(6, 19);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(84, 31);
            this.upButton.TabIndex = 0;
            this.upButton.Text = "W - Front";
            this.upButton.UseVisualStyleBackColor = true;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(45, 35);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(90, 26);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectMouseClick);
            // 
            // ibOriginal
            // 
            this.ibOriginal.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ibOriginal.Location = new System.Drawing.Point(431, 12);
            this.ibOriginal.Name = "ibOriginal";
            this.ibOriginal.Size = new System.Drawing.Size(491, 428);
            this.ibOriginal.TabIndex = 2;
            this.ibOriginal.TabStop = false;
            // 
            // btnPauseOrResume
            // 
            this.btnPauseOrResume.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPauseOrResume.Location = new System.Drawing.Point(643, 446);
            this.btnPauseOrResume.Name = "btnPauseOrResume";
            this.btnPauseOrResume.Size = new System.Drawing.Size(89, 37);
            this.btnPauseOrResume.TabIndex = 16;
            this.btnPauseOrResume.Text = "Pause";
            this.btnPauseOrResume.UseVisualStyleBackColor = true;
            this.btnPauseOrResume.Click += new System.EventHandler(this.btnPauseOrResume_Click);
            // 
            // Rover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(955, 495);
            this.Controls.Add(this.btnPauseOrResume);
            this.Controls.Add(this.ibOriginal);
            this.Controls.Add(this.serialPortBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Rover";
            this.Text = "Rover";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_Closed);
            this.serialPortBox.ResumeLayout(false);
            this.serialPortBox.PerformLayout();
            this.keyboard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ibOriginal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox serialPortBox;
        private System.Windows.Forms.TextBox sendTextBox;
        private System.Windows.Forms.Label sendLabel;
        private System.Windows.Forms.GroupBox keyboard;
        private System.Windows.Forms.Button changeGearButton;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button spinButton;
        private Emgu.CV.UI.ImageBox ibOriginal;
        private System.Windows.Forms.Button btnPauseOrResume;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button button1;
    }
}

