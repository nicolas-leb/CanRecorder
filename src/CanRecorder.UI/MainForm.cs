using CanRecorder.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanRecorder.UI
{
    public partial class MainForm : Form
    {
        private CanVectorService _canVectorService;
        public MainForm()
        {
            _canVectorService = new();
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var channels = _canVectorService.GetChannelsName();
            ChannelSelection.Items.Clear();
            foreach (var channel in channels)
            {
                ChannelSelection.Items.Add(channel);
            }
            if (ChannelSelection.Items.Count > 0)
            {
                ChannelSelection.SelectedIndex = 0;
            }

            _canVectorService.FrameReceived += _canVectorService_FrameReceived;

            StatusMessage.Text = "Ready";
        }

        private void _canVectorService_FrameReceived(object? sender, Models.Frame e)
        {
            FramesDataGrid.Invoke(new Action(() =>
            {
                FramesDataGrid.Rows.Add(e.timestamp.ToString(), e.id.ToString(), "test");
            }));
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if(_canVectorService.Start(ChannelSelection.Text))
            {
                StatusMessage.Text = "Started";
                StartButton.Enabled = false;
                StopButton.Enabled = true;
            }
            else
            {
                StatusMessage.Text = "Impossible to start !";
            }
            
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if(_canVectorService.Stop())
            {
                StatusMessage.Text = "Ready";
                StartButton.Enabled = true;
                StopButton.Enabled = false;
            }
            else
            {
                StatusMessage.Text = "Impossible to stop !";
            }
        }
    }
}
