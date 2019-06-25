using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using weCare.Core.Dac;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace print
{
    public class dbBiz
    {
        #region

        internal List<EntityPre> GetPreInfo()
        {
            string Sql = string.Empty;
            SqlHelper svc = null;
            List<EntityPre> data = new List<EntityPre>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select a.putmeddetailid_chr,
                               ord.orderid_chr,
                               e.lastname_vchr,
                               d.inpatientid_chr,
                               c.code_vchr,
                               c.deptname_vchr,
                               a.medid_chr,
                               a.pubdate_dat,
                               b.assistcode_chr,
                               b.medicinename_vchr,
                               b.medspec_vchr,
                               a.unitprice_mny,
                               a.unit_vchr,
                               a.get_dec,
                               nvl(a.get_dec / 2, 0) as premedamount 
                          from t_bih_opr_putmeddetail   a,
                               t_bse_medicine           b,
                               t_bse_deptdesc           c,
                               t_opr_bih_register       d,
                               t_opr_bih_registerdetail e,
                               t_bse_patientcard        card,
                               t_opr_bih_order          ord
                         where (a.areaid_chr = c.deptid_chr)
                           and (a.medid_chr = b.medicineid_chr)
                           and (a.registerid_chr = d.registerid_chr)
                           and (d.registerid_chr = e.registerid_chr)
                           and (a.paientid_chr = card.patientid_chr)
                           and (a.orderid_chr = ord.orderid_chr(+))
                           and a.status_int = 0
                           and a.pretestdays > 0  order by c.deptname_vchr ,b.medicinename_vchr ";

                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string deptNmae = string.Empty;
                    string assistCode = string.Empty;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            deptNmae = dr["deptname_vchr"].ToString();
                            assistCode = dr["assistcode_chr"].ToString();

                            if (data.Any(t => t.deptName == deptNmae && t.assistCode == assistCode))
                            {
                                #region 累计
                                EntityPre voClone = data.FirstOrDefault(t => t.deptName == deptNmae && t.assistCode == assistCode);
                                voClone.premedamount += Function.Dec(dr["premedamount"]);
                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityPre vo = new EntityPre();
                                vo.deptName = deptNmae;
                                vo.assistCode = assistCode;
                                vo.medicineName = dr["medicinename_vchr"].ToString();
                                vo.medSpec = dr["medspec_vchr"].ToString();
                                vo.premedamount = Function.Dec(dr["premedamount"]);

                                #endregion
                                data.Add(vo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }

            return data;
        }
        #endregion

        #region
        internal List<EntityLxh> GetLxhInfo(string dteStart, string dteEnd)
        {
            string Sql = string.Empty;
            SqlHelper svc = null;
            List<EntityLxh> data = new List<EntityLxh>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select 
               b.patient_inhospitalno_chr,
               a.patient_name_vchr,
               a.sex_chr,
               a.age_chr,
               d.homephone_vchr,
               e.result_vchr,
               e.check_item_name_vchr
          from t_opr_lis_application  a,
               t_opr_lis_sample       b,
               t_bse_patientcard      c,
               t_bse_patient          d,
               t_opr_lis_check_result e
         where a.application_id_chr = b.application_id_chr
           and a.patientid_chr = c.patientcardid_chr
           and c.patientid_chr = d.patientid_chr
           and b.sample_id_chr = e.sample_id_chr
           and e.check_item_id_chr in ('000412',
                                       '001146',
                                       '001152',
                                       '001168',
                                       '001170',
                                       '000413',
                                       '001142',
                                       '001148',
                                       '001167',
                                       '001169',
                                       '000408',
                                       '001172',
                                       '000409',
                                       '001171',
                                       '000422',
                                       '001125',
                                       '000421',
                                       '001126',
                                       '001420',
                                       '000406',
                                       '001122',
                                       '001526',
                                       '000066',
                                       '000443',
                                       '001221',
                                       '001156',
                                       '001109',
                                       '000064',
                                       '000734',
                                       '001158',
                                       '001223',
                                       '000063',
                                       '001488',
                                       '001617',
                                       '000620',
                                       '001508',
                                       '002027',
                                       '002002')
           and a.patient_type_id_chr = 3
           and a.pstatus_int = 2
           and b.status_int = 6
           and a.application_dat between
               to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
               to_date(?, 'yyyy-mm-dd hh24:mi:ss') ";

                IDataParameter[] parm = svc.CreateParm(2);
                parm[0].Value = dteStart + " 00:00:00";
                parm[1].Value = dteEnd + " 23:59:59"; ;

                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string inhospitalno = string.Empty;
                    string name = string.Empty;
                    string itemname = string.Empty;
                    string result = string.Empty;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            inhospitalno = dr["patient_inhospitalno_chr"].ToString();
                            name = dr["patient_name_vchr"].ToString();
                            itemname = dr["check_item_name_vchr"].ToString();

                            if (data.Any(t => t.inhospitalno == inhospitalno && t.name == name))
                            {
                                #region
                                EntityLxh voClone = data.FirstOrDefault(t => t.inhospitalno == inhospitalno && t.name == name);
                                if (itemname.Contains("总胆固醇"))
                                {
                                    voClone.F1 = result.Trim();
                                }
                                if (itemname.Contains("甘油"))
                                {
                                    voClone.F2 = result.Trim();
                                }
                                if (itemname.Contains("高密度脂蛋白胆固醇"))
                                {
                                    voClone.F3 = result.Trim();
                                }
                                if (itemname.Contains("低密度脂蛋白胆固醇"))
                                {
                                    voClone.F4 = result.Trim();
                                }
                                if (itemname.Contains("抗链球菌O溶血素"))
                                {
                                    voClone.F5 = result.Trim();
                                }
                                if (itemname.Contains("类风湿因子"))
                                {
                                    voClone.F6 = result.Trim();
                                }
                                if (itemname.Contains("C反应蛋白"))
                                {
                                    voClone.F7 = result.Trim();
                                }
                                if (itemname.Contains("肌酐"))
                                {
                                    voClone.F8 = result.Trim();
                                }
                                if (itemname.Contains("尿酸"))
                                {
                                    voClone.F9 = result.Trim();
                                }
                                if (itemname.Contains("血糖"))
                                {
                                    voClone.F10 = result.Trim();
                                }
                                if (itemname.Contains("25羟维生素D测定"))
                                {
                                    voClone.F11 = result.Trim();
                                }
                                if (itemname.Contains("抗环瓜氨酸肽抗体测定"))
                                {
                                    voClone.F12 = result.Trim();
                                }
                                if (itemname.Contains("血清铁蛋白"))
                                {
                                    voClone.F13 = result.Trim();
                                }
                                if (itemname.Contains("抗核抗体"))
                                {
                                    voClone.F14 = result.Trim();
                                }
                                if (itemname.Contains("尿素"))
                                {
                                    voClone.F15 = result.Trim();
                                }
                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityLxh vo = new EntityLxh();
                                vo.inhospitalno = inhospitalno;
                                vo.name = name;
                                vo.sex = dr["sex_chr"].ToString();
                                vo.age = dr["sex_chr"].ToString();
                                vo.homephone = dr["homephone_vchr"].ToString();
                                if (itemname.Contains("总胆固醇"))
                                {
                                    vo.F1 = result.Trim();
                                }
                                if (itemname.Contains("甘油"))
                                {
                                    vo.F2 = result.Trim();
                                }
                                if (itemname.Contains("高密度脂蛋白胆固醇"))
                                {
                                    vo.F3 = result.Trim();
                                }
                                if (itemname.Contains("低密度脂蛋白胆固醇"))
                                {
                                    vo.F4 = result.Trim();
                                }
                                if (itemname.Contains("抗链球菌O溶血素"))
                                {
                                    vo.F5 = result.Trim();
                                }
                                if (itemname.Contains("类风湿因子"))
                                {
                                    vo.F6 = result.Trim();
                                }
                                if (itemname.Contains("C反应蛋白"))
                                {
                                    vo.F7 = result.Trim();
                                }
                                if (itemname.Contains("肌酐"))
                                {
                                    vo.F8 = result.Trim();
                                }
                                if (itemname.Contains("尿酸"))
                                {
                                    vo.F9 = result.Trim();
                                }
                                if (itemname.Contains("血糖"))
                                {
                                    vo.F10 = result.Trim();
                                }
                                if (itemname.Contains("25羟维生素D测定"))
                                {
                                    vo.F11 = result.Trim();
                                }
                                if (itemname.Contains("抗环瓜氨酸肽抗体测定"))
                                {
                                    vo.F12 = result.Trim();
                                }
                                if (itemname.Contains("血清铁蛋白"))
                                {
                                    vo.F13 = result.Trim();
                                }
                                if (itemname.Contains("抗核抗体"))
                                {
                                    vo.F14 = result.Trim();
                                }
                                if (itemname.Contains("尿素"))
                                {
                                    vo.F15 = result.Trim();
                                }

                                #endregion
                                data.Add(vo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }

            return data;
        }
        #endregion

        #region
        internal List<EntityBihRegSate> GetBihRegState(string dteStart, string dteEnd, int flg)
        {
            string Sql = string.Empty;
            SqlHelper svc = null;
            List<EntityBihRegSate> data = new List<EntityBihRegSate>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select a.registerid_chr,b.lastname_vchr, a.inpatientid_chr,a.modify_dat from t_opr_bih_register a
                                left join t_bse_patient b
                                on a.patientid_chr = b.patientid_chr
                                                             where a.status_int=1
                                                             and a.modify_dat between
                                                                   to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                                                   to_date(?, 'yyyy-mm-dd hh24:mi:ss')";

                IDataParameter[] parm = svc.CreateParm(2);
                parm[0].Value = dteStart + " 00:00:00";
                parm[1].Value = dteEnd + " 23:59:59"; ;

                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int n = 0;

                    TimeSpan tStart1 = DateTime.Parse("08:00:00").TimeOfDay;
                    TimeSpan tEnd1 = DateTime.Parse("16:00:00").TimeOfDay;

                    TimeSpan tStart2 = DateTime.Parse("16:00:01").TimeOfDay;
                    TimeSpan tEnd2 = DateTime.Parse("23:59:59").TimeOfDay;

                    TimeSpan tStart3 = DateTime.Parse("00:00:00").TimeOfDay;
                    TimeSpan tEnd3 = DateTime.Parse("08:00:01").TimeOfDay;

                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityBihRegSate vo = new EntityBihRegSate();
                        vo.registerid = dr["registerid_chr"].ToString();
                        vo.inpatientid = dr["inpatientid_chr"].ToString();
                        vo.modifyDate = dr["modify_dat"].ToString();
                        vo.name = dr["lastname_vchr"].ToString();

                        string modity = Function.Datetime(dr["modify_dat"].ToString()).ToString("HH:mm:ss");
                        TimeSpan tModity = DateTime.Parse(modity).TimeOfDay;
                        if (flg == 1)
                        {
                            #region //白班

                            if (tModity > tStart1 && tModity < tEnd1)
                            {
                                //vo.F1++;
                                vo.n = ++n;
                            }
                            else
                                continue;

                            #endregion


                        }
                        else if (flg == 2)
                        {
                            #region //夜班

                            if ((tModity > tStart2 && tModity < tEnd2) || (tModity > tStart3 && tModity < tEnd3))
                            {
                                //vo.F2++;
                                vo.n = ++n;
                            }
                            else
                                continue;

                            #endregion
                        }

                        data.Add(vo);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }

            return data;
        }
        #endregion

        #region
        internal List<EntityBxgy> GetBxgy(string dteStart, string dteEnd, int flg)
        {
            string Sql = string.Empty;
            SqlHelper svc = null;
            List<EntityBxgy> data = new List<EntityBxgy>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct a.patient_name_vchr AS HZXM, --患者姓名
                                            a.sex_chr,
                                            a.patient_inhospitalno_chr,
                                            a.patientcardid_chr,
                                            a.appl_deptid_chr,
                                            e.deptname_vchr AS deptName, -- 送检科室
                                            e1.lastname_vchr AS applyDoct, -- 申请医生
                                            e2.lastname_vchr  AS checker,  --检验者
                                            r1.check_item_id_chr ,
                                            r1.check_item_name_vchr,
                                            t2.check_category_desc_vchr,
                                            d.sampletype_vchr AS BBLX, --标本类型
                                            to_char(d.sampling_date_dat, 'yyyy-mm-dd hh24:mi:ss') AS CYSJ, --采样时间
                                            to_char(r.report_dat, 'yyyy-mm-dd hh24:mi:ss') AS BGSJ, --报告时间
                                            r1.refrange_vchr,
                                            r1.max_val_dec,
                                            r1.min_val_dec,
                                            r1.result_vchr
                              from t_opr_lis_application a
                              left join t_opr_lis_sample d
                                on a.application_id_chr = d.application_id_chr
                             left join t_opr_lis_app_apply_unit t
                                on a.application_id_chr  = t.application_id_chr
                              left join t_aid_lis_apply_unit t1
                                on t.apply_unit_id_chr = t1.apply_unit_id_chr
                              left join t_bse_lis_check_category t2
                                    on t1.check_category_id_chr = t2.check_category_id_chr
                              left join t_opr_lis_app_report r
                                on a.application_id_chr = r.application_id_chr
                              left join t_bse_deptdesc e
                                on a.appl_deptid_chr = e.deptid_chr
                              left join t_bse_employee e1
                                on a.appl_empid_chr = e1.empid_chr
                              left join t_opr_lis_check_result r1
                                on d.sample_id_chr = r1.sample_id_chr
                              left join t_bse_employee e2
                                on r.reportor_id_chr = e2.empid_chr
                             where d.status_int > 5
                               and a.pstatus_int = 2
                               and r.report_dat between
                                   to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                   to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                               and r.report_dat is not null
                               and r.status_int > 1
                               and r1.result_vchr <> '\'
                               and r1.status_int = 1   
 and r1.check_item_id_chr = '001782' order by e.deptname_vchr";

                IDataParameter[] parm = svc.CreateParm(2);
                parm[0].Value = dteStart + " 00:00:00";
                parm[1].Value = dteEnd + " 23:59:59"; ;

                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int n = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityBxgy vo = new EntityBxgy();
                        vo.HZXM = dr["HZXM"].ToString();
                        if (!string.IsNullOrEmpty(dr["patient_inhospitalno_chr"].ToString().Trim()))
                            vo.KH = dr["patient_inhospitalno_chr"].ToString();
                        else
                            vo.KH = dr["patientcardid_chr"].ToString();
                        vo.BGSJ = dr["BGSJ"].ToString();
                        if (!string.IsNullOrEmpty(dr["result_vchr"].ToString().Trim()))
                            vo.JG = Function.Int(dr["result_vchr"]);

                        if (vo.JG < 20)
                            continue;
                        vo.n = ++n;

                        data.Add(vo);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }

            return data;
        }
        #endregion

        #region
        internal List<EntityLisSample> GetLisSambleAccept(string dteStart, string dteEnd, int flg)
        {
            string Sql = string.Empty;
            SqlHelper svc = null;
            List<EntityLisSample> data = new List<EntityLisSample>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct 
                        t.barcode_vchr as barcode,--条码号
                        t.patientcardid_chr as patcardno,--门诊号
                        t.patient_inhospitalno_chr as peno,--住院号/体检号
                        e.dictname_vchr as pattype, --病人类型
                        t.patient_name_vchr as patname, --病人姓名
                        t.sampletype_vchr as sampletype,--标本类型
                        --f.itemname as checkcontent,   --检验内容
                        j.check_content_vchr as checkcontent,
                        c.lastname_vchr as applyername,--申请人
                        d.DEPTNAME_VCHR as deptname,--申请科室
                        a.recorderid,--打包人id
                        a.recorddate , 
                        a.packdate as packtime, --打包时间
                        g.lastname_vchr as packname,--打包人
                        t.accept_dat as checktime,  --核收时间
                        h.lastname_vchr as checkname,--核收人
                        k.feedback_date_date as rechecktime,--拒收时间
                        k.sample_back_reason_vchr as recheckreason,--拒收原因
                        t.appl_dat
                        from   t_opr_lis_sample  t
                        left join t_samplepack a
                        on  t.barcode_vchr = a.barcode 
                        left join t_samplepack_detail f
                        on t.barcode_vchr = f.barcode
                        left join (select d.dictid_chr, d.dictname_vchr
                                  from t_aid_dict d
                                 where trim(d.dictid_chr) <> 0
                                   and dictkind_chr = '61') e
                        on t.patient_type_chr = e.dictid_chr
                        left join t_bse_employee c
                        on t.appl_empid_chr = c.empid_chr
                        left join t_bse_deptdesc d
                        on t.appl_deptid_chr = d.DEPTID_CHR
                        left join t_bse_employee g
                        on a.recorderid = g.empid_chr
                        left join t_bse_employee h
                        on t.acceptor_id_chr =h.empid_chr
                        left join t_opr_lis_application j
                        on t.application_id_chr = j.application_id_chr
                        left join t_opr_lis_sample_feedback k
                        on t.sample_id_chr = k.sample_id_chr
                        where t.appl_deptid_chr is not null 
                        and (t.accept_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss')) 
                        and t.status_int >= 2 and t.status_int < 7 order by  t.patientcardid_chr, d.DEPTNAME_VCHR  ";

                IDataParameter[] parm = svc.CreateParm(2);
                parm[0].Value = dteStart + " 00:00:00";
                parm[1].Value = dteEnd + " 23:59:59"; ;

                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int n = 0;

                    TimeSpan tStart1 = DateTime.Parse("17:30:00").TimeOfDay;
                    TimeSpan tEnd1 = DateTime.Parse("23:59:59").TimeOfDay;

                    TimeSpan tStart2 = DateTime.Parse("00:00:00").TimeOfDay;
                    TimeSpan tEnd2 = DateTime.Parse("05:00:01").TimeOfDay;

                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityLisSample vo = new EntityLisSample();

                        vo.barcode = dr["barcode"].ToString();
                        vo.patcardno = dr["patcardno"].ToString();
                        vo.inpatno = dr["peno"].ToString();
                        vo.pattype = dr["pattype"].ToString();
                        vo.patname = dr["patname"].ToString();
                        vo.sampletype = dr["sampletype"].ToString();
                        vo.checkcontent = dr["checkcontent"].ToString();
                        vo.applyername = dr["applyername"].ToString();
                        vo.deptname = dr["deptname"].ToString();
                        vo.packtime = dr["packtime"].ToString();
                        vo.packname = dr["packname"].ToString();
                        vo.checktime = dr["checktime"].ToString();
                        vo.checkname = dr["checkname"].ToString();
                        vo.rechecktime = dr["rechecktime"].ToString();
                        vo.recheckreason = dr["recheckreason"].ToString();

                        //string modity = Function.Datetime(dr["checktime"].ToString()).ToString("HH:mm:ss");
                        //TimeSpan tModity = DateTime.Parse(modity).TimeOfDay;
                        vo.n = ++n;

                        //if (tModity >= tStart1 && tModity <= tEnd1)
                        //{
                        //    vo.n = ++n;
                        //}
                        //else if (tModity >= tStart2 && tModity <= tEnd2)
                        //{
                        //    vo.n = ++n;
                        //}
                        //else
                        //    continue;

                        data.Add(vo);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }

            return data;
        }
        #endregion
    }
}
