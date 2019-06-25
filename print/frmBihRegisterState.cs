using Common.Controls;
using Hisitf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using weCare.Core.Utils;

namespace print
{
    public partial class frmBihRegisterState : frmBase
    {
        public frmBihRegisterState()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            QueryBihReg();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }


        #region
        void QueryBihReg()
        {
            string beginDate = string.Empty;
            string endDate = string.Empty;
            beginDate = dteStart.Text.Trim();
            endDate = dteEnd.Text.Trim();

            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }
            }

            dbBiz biz = new dbBiz();
            if(this.radioGroup1.SelectedIndex == 0)
                this.gcData.DataSource = biz.GetBihRegState(beginDate, endDate,1);
            else
                this.gcData.DataSource = biz.GetBihRegState(beginDate, endDate, 2);
        }

        #endregion

        private void frmBihRegisterState_Load(object sender, EventArgs e)
        {

        }
    }
}