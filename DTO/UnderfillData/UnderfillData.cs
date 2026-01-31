using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UnderfillData
    {
        public string Trans_Time { get; set; }
        public string EM_Pcb_ID { get; set; }
        public string EM_Start_SUB1 { get; set; }
        public string EM_End_SUB1 { get; set; }
        public string EM_TimeUF_SUB1 { get; set; }
        public int EM_Location_SUB1 { get; set; }
        public string EM_Gm_SUB1 { get; set; }
        public string EM_Result_Loc_SUB1 { get; set; }
        public string EM_Result_Gm_SUB1 { get; set; }
        public string EM_Start_SUB2 { get; set; }
        public string EM_End_SUB2 { get; set; }
        public string EM_TimeUF_SUB2 { get; set; }
        public int EM_Location_SUB2 { get; set; }
        public string EM_Gm_SUB2 { get; set; }
        public string EM_Result_Loc_SUB2 { get; set; }
        public string EM_Result_Gm_SUB2 { get; set; }
        public string EM_Start_SUB3 { get; set; }
        public string EM_End_SUB3 { get; set; }
        public string EM_TimeUF_SUB3 { get; set; }
        public int EM_Location_SUB3 { get; set; }
        public string EM_Gm_SUB3 { get; set; }
        public string EM_Result_Loc_SUB3 { get; set; }
        public string EM_Result_Gm_SUB3 { get; set; }
        public string TOTAL_RESULT { get; set; }
        public int CurrentLocationSUB1 { get; set; }
        public int CurrentLocationSUB2 { get; set; }
        public int CurrentLocationSUB3 { get; set; }
        public UnderfillData()
        {
            this.Trans_Time = "";
            this.EM_Pcb_ID = "";
            this.EM_Start_SUB1 = "";
            this.EM_End_SUB1 = "";
            this.EM_TimeUF_SUB1 = "";
            this.EM_Location_SUB1 = 0;
            this.EM_Gm_SUB1 = "";
            this.EM_Result_Loc_SUB1 = "NG";
            this.EM_Result_Gm_SUB1 = "NG";

            this.EM_Start_SUB2 = "";
            this.EM_End_SUB2 = "";
            this.EM_TimeUF_SUB2 = "";
            this.EM_Location_SUB2 = 0;
            this.EM_Gm_SUB2 = "";
            this.EM_Result_Loc_SUB2 = "NG";
            this.EM_Result_Gm_SUB2 = "NG";

            this.EM_Start_SUB3 = "";
            this.EM_End_SUB3 = "";
            this.EM_TimeUF_SUB3 = "";
            this.EM_Location_SUB3 = 0;
            this.EM_Gm_SUB3 = "";
            this.EM_Result_Loc_SUB3 = "NG";
            this.EM_Result_Gm_SUB3 = "NG";

            this.TOTAL_RESULT = "";

            this.CurrentLocationSUB1 = 0;
            this.CurrentLocationSUB2 = 0;
            this.CurrentLocationSUB3 = 0;
        }
    }
}
