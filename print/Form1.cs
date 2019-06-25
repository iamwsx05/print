
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
    public partial class Form1 : frmBase
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 事件



        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void btnQuery_Click(object sender, EventArgs e)
        {
            //this.Query();
            this.QueryLxh();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
             uiHelper.ExportToXls(this.gvData);
        }

        #endregion

        #region 方法


        #region Query
        /// <summary>
        /// Query
        /// </summary>
        void Query()
        {
            dbBiz biz = new dbBiz();
            this.gcData.DataSource = biz.GetPreInfo();
        }
        #endregion

        #region 
        void QueryLxh()
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
            this.gcData.DataSource = biz.GetLxhInfo(beginDate,endDate);
        }

        #endregion

        #endregion
    }
}
