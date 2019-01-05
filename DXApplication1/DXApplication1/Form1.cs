using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace DXApplication1
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public delegate void ThreadCallBack();
        public Form1()
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            simpleButton_start.Text = "选择";
        }

        private void simpleButton_start_Click(object sender, EventArgs e)
        {
            if(simpleButton_start.Text=="选择")
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if(ofd.ShowDialog()==DialogResult.OK)
                {
                    textEdit_dir.Text = ofd.FileName;
                    simpleButton_start.Text = "开始";
                }
            }
            else if(simpleButton_start.Text=="开始")
            {
                simpleButton_start.Enabled = false;
                PcapAnalysis analysis = new PcapAnalysis(this);
                ThreadCallBack tcb = new ThreadCallBack(analysis.initAnalysis);
                IAsyncResult result = tcb.BeginInvoke(new AsyncCallback(CallBackFun),null);
                
            }
        }

        public void CallBackFun(IAsyncResult result)
        {
            ThreadCallBack tcb = (ThreadCallBack)((AsyncResult)result).AsyncDelegate;
            simpleButton_start.Enabled = true;
        }
    }
}
