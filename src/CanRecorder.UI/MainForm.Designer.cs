namespace CanRecorder.UI
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
            TabMenu = new TabControl();
            RecordPage = new TabPage();
            FramesDataGrid = new DataGridView();
            TimeColumn = new DataGridViewTextBoxColumn();
            IdColumn = new DataGridViewTextBoxColumn();
            DataColumn = new DataGridViewTextBoxColumn();
            StopButton = new Button();
            StartButton = new Button();
            ChannelLabel = new Label();
            ChannelSelection = new ComboBox();
            StatusBar = new StatusStrip();
            StatusMessage = new ToolStripStatusLabel();
            TabMenu.SuspendLayout();
            RecordPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)FramesDataGrid).BeginInit();
            StatusBar.SuspendLayout();
            SuspendLayout();
            // 
            // TabMenu
            // 
            TabMenu.Controls.Add(RecordPage);
            TabMenu.Location = new Point(12, 12);
            TabMenu.Name = "TabMenu";
            TabMenu.SelectedIndex = 0;
            TabMenu.Size = new Size(776, 406);
            TabMenu.TabIndex = 0;
            // 
            // RecordPage
            // 
            RecordPage.Controls.Add(FramesDataGrid);
            RecordPage.Controls.Add(StopButton);
            RecordPage.Controls.Add(StartButton);
            RecordPage.Controls.Add(ChannelLabel);
            RecordPage.Controls.Add(ChannelSelection);
            RecordPage.Location = new Point(4, 24);
            RecordPage.Name = "RecordPage";
            RecordPage.Padding = new Padding(3);
            RecordPage.Size = new Size(768, 378);
            RecordPage.TabIndex = 0;
            RecordPage.Text = "Record";
            RecordPage.UseVisualStyleBackColor = true;
            // 
            // FramesDataGrid
            // 
            FramesDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            FramesDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            FramesDataGrid.Columns.AddRange(new DataGridViewColumn[] { TimeColumn, IdColumn, DataColumn });
            FramesDataGrid.Location = new Point(15, 76);
            FramesDataGrid.Name = "FramesDataGrid";
            FramesDataGrid.ReadOnly = true;
            FramesDataGrid.Size = new Size(742, 316);
            FramesDataGrid.TabIndex = 4;
            // 
            // TimeColumn
            // 
            TimeColumn.HeaderText = "Time";
            TimeColumn.Name = "TimeColumn";
            TimeColumn.ReadOnly = true;
            // 
            // IdColumn
            // 
            IdColumn.HeaderText = "Id";
            IdColumn.Name = "IdColumn";
            IdColumn.ReadOnly = true;
            // 
            // DataColumn
            // 
            DataColumn.HeaderText = "Data";
            DataColumn.Name = "DataColumn";
            DataColumn.ReadOnly = true;
            // 
            // StopButton
            // 
            StopButton.Enabled = false;
            StopButton.Location = new Point(682, 7);
            StopButton.Name = "StopButton";
            StopButton.Size = new Size(75, 23);
            StopButton.TabIndex = 3;
            StopButton.Text = "Stop";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += StopButton_Click;
            // 
            // StartButton
            // 
            StartButton.Location = new Point(601, 7);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(75, 23);
            StartButton.TabIndex = 2;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // ChannelLabel
            // 
            ChannelLabel.AutoSize = true;
            ChannelLabel.Location = new Point(15, 10);
            ChannelLabel.Name = "ChannelLabel";
            ChannelLabel.Size = new Size(57, 15);
            ChannelLabel.TabIndex = 1;
            ChannelLabel.Text = "Channel :";
            // 
            // ChannelSelection
            // 
            ChannelSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            ChannelSelection.Location = new Point(78, 7);
            ChannelSelection.Name = "ChannelSelection";
            ChannelSelection.Size = new Size(517, 23);
            ChannelSelection.TabIndex = 0;
            // 
            // StatusBar
            // 
            StatusBar.Items.AddRange(new ToolStripItem[] { StatusMessage });
            StatusBar.Location = new Point(0, 428);
            StatusBar.Name = "StatusBar";
            StatusBar.Size = new Size(800, 22);
            StatusBar.TabIndex = 1;
            // 
            // StatusMessage
            // 
            StatusMessage.Name = "StatusMessage";
            StatusMessage.Size = new Size(48, 17);
            StatusMessage.Text = "Starting";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(StatusBar);
            Controls.Add(TabMenu);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CanRecorder";
            Load += MainForm_Load;
            TabMenu.ResumeLayout(false);
            RecordPage.ResumeLayout(false);
            RecordPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)FramesDataGrid).EndInit();
            StatusBar.ResumeLayout(false);
            StatusBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl TabMenu;
        private TabPage RecordPage;
        private Label ChannelLabel;
        private ComboBox ChannelSelection;
        private Button StopButton;
        private Button StartButton;
        private DataGridView FramesDataGrid;
        private DataGridViewTextBoxColumn TimeColumn;
        private DataGridViewTextBoxColumn IdColumn;
        private DataGridViewTextBoxColumn DataColumn;
        private StatusStrip StatusBar;
        private ToolStripStatusLabel StatusMessage;
    }
}