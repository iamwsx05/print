using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace print
{
    public class EntityPre
    {
        public string deptName { get; set; }
        public string assistCode { get; set; }
        public string medicineName { get; set; }
        public string medSpec { get; set; }
        public decimal premedamount { get; set; }
    }


    public class EntityLxh
    {
        public string inhospitalno { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string age { get; set; }
        public string homephone { get; set; }
        //public string itemname { get; set; }

        public string F1 { get; set; }

        public string F2 { get; set; }

        public string F3 { get; set; }

        public string F4 { get; set; }

        public string F5 { get; set; }

        public string F6 { get; set; }

        public string F7 { get; set; }

        public string F8 { get; set; }

        public string F9 { get; set; }

        public string F10 { get; set; }

        public string F11 { get; set; }

        public string F12 { get; set; }

        public string F13 { get; set; }

        public string F14 { get; set; }

        public string F15 { get; set; }


    }



    public class EntityBihRegSate
    {
        public int n { get; set; }
        public string registerid { get; set; }
        public string name {get;set;}
        public string inpatientid{get;set;}
        public string modifyDate {get;set;}
    }

    public class EntityBxgy
    {
        public int n { get; set; }
        public string XM { get; set; }
        public string HZXM { get; set; }
        public string KH { get; set; }
        public string BGSJ { get; set; }
        public int JG { get; set; }
    }


    public class EntityLisSample
    {
        public int n { get; set; }
        public string barcode { get; set; }
        public string patcardno { get; set; }
        public string inpatno { get; set; }
        public string pattype { get; set; }
        public string patname { get; set; }
        public string sampletype { get; set; }
        public string checkcontent { get; set; }
        public string applyername { get; set; }
        public string deptname { get; set; }
        public string packtime { get; set; }
        public string packname { get; set; }
        public string checktime { get; set; }
        public string checkname { get; set; }
        public string rechecktime { get; set; }
        public string recheckreason { get; set; }
    }

}
