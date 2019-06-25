using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common.Controls;
using weCare.Core.Utils;

namespace print
{
    public partial class frmBxgy : frmBase
    {
        public frmBxgy()
        {
            InitializeComponent();
        }

        private void frmBxgy_Load(object sender, EventArgs e)
        {

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.QueryBxgy();
        }

        #region
        void QueryBxgy()
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
            this.gcData.DataSource = biz.GetBxgy(beginDate, endDate, 1);
        }

        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
            uiHelper.ExportToXls(this.gvData);
        }
    }
}