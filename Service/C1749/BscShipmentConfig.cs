using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{
    class BscShipmentConfig : NotificationConfig
    {
        public BscShipmentConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new BscShipmentDS();
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {
            StringBuilder sb = new StringBuilder();
            //取當日的數據
            sb.Append(" select protype,shpnum,shpamts,ordnum,ordamts from bsc_groupshipment where datediff(dd,soday,getdate())=30 ORDER BY  shpnum,protype ");
            Fill(sb.ToString(), ds, "jtdaytlb");
        }

        public override void ConfigData()
        {

            DataTable result = new DataTable();
            result.Columns.Add("SEQ", typeof(String));//添加列
            result.Columns.Add("R", typeof(String));
            result.Columns.Add("AH", typeof(String));
            result.Columns.Add("AA", typeof(String));
            result.Columns.Add("SDS", typeof(String));
            result.Columns.Add("P", typeof(String));
            result.Columns.Add("K", typeof(String));
            result.Columns.Add("FW", typeof(String));
            result.Columns.Add("HS", typeof(String));
            result.Columns.Add("VH", typeof(String));

            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow dr = result.NewRow();
                dr["aa"] = "aaa";
                dr[0] = "aa";
                result.Rows.Add(dr);

            }
             
            List<string> shpnumList = new List<string>(6);
            List<string> shpamtsList = new List<string>(6);
            List<string> ordnumList = new List<string>(6);
            List<string> ordamtsList = new List<string>(6);
            String shpnumR = "";
            String shpnumAH = "";
            String shpnumAA = "";
            String shpnumP = "";
            String shpnumS = "";
            String shpnumK = "";
            String shpamt = "";
            int sumshpnumK = 0;
            int sumshpamtsK = 0;
            int sumordnumK = 0;
            int sumordamtsK = 0;
            String ordnum = "";
            String ordamts = "";
            //DataTable result;
            DataTable dtl;
            result = new DataTable();
            result = ds.Tables["testtlb"].Copy();
            //result.Columns.Add("SEQ", typeof(String));//添加列
            //result.Columns.Add("R", typeof(String));
            //result.Columns.Add("AH", typeof(String));
            //result.Columns.Add("AA", typeof(String));
            //result.Columns.Add("SDS", typeof(String));
            //result.Columns.Add("P", typeof(String));
            //result.Columns.Add("K", typeof(String));
            //result.Columns.Add("FW", typeof(String));
            //result.Columns.Add("HS", typeof(String));
            //result.Columns.Add("VH", typeof(String));
            dtl = ds.Tables["jtdaytlb"].Copy();
            if(dtl.Rows.Count>0){
                foreach (DataRow item in dtl.Rows)
                {
                    if (item["protype"].ToString().Contains("R"))
                    {
                        shpnumList[0] = item["shpnum"].ToString();
                        shpamtsList[0] = item["shpamts"].ToString();
                        ordnumList[0] = item["ordnum"].ToString();
                        ordamtsList[0] = item["ordamts"].ToString();
                    }
                    else 
                    {
                        shpnumList[0] = "";
                        shpamtsList[0] = "0.0";
                        ordnumList[0] = "0.0";
                        ordamtsList[0] = "0.0";
                    }
                    
                    if (item["protype"].ToString().Contains("A机体"))
                    {
                        shpnumList[1] = item["shpnum"].ToString();
                        shpamtsList[1] = item["shpamts"].ToString();
                        ordnumList[1] = item["ordnum"].ToString();
                        ordamtsList[1] = item["ordamts"].ToString();
                    }
                    else
                    {
                        shpnumList[1] = "0.0";
                        shpamtsList[1] = "0.0";
                        ordnumList[1] = "0.0";
                        ordamtsList[1] = "0.0";
                    }
                    if (item["protype"].ToString().Contains("A机组"))
                    {
                        shpnumList[2] = item["shpnum"].ToString();
                        shpamtsList[2] = item["shpamts"].ToString();
                        ordnumList[2] = item["ordnum"].ToString();
                        ordamtsList[2] = item["ordamts"].ToString();
                    }
                    else
                    {
                        shpnumList[2] = "0";
                        shpamtsList[2] = "0";
                        ordnumList[2] = "0";
                        ordamtsList[2] = "0";
                    }
                    if (item["protype"].ToString().Contains("真空"))
                    {
                        shpnumList[3] = item["shpnum"].ToString();
                        shpamtsList[3] = item["shpamts"].ToString();
                        ordnumList[3] = item["ordnum"].ToString();
                        ordamtsList[3] = item["ordamts"].ToString();
                    }
                    else
                    {
                        shpnumList[3] = "0";
                        shpamtsList[3] = "0";
                        ordnumList[3] = "0";
                        ordamtsList[3] = "0";
                    }
                    if (item["protype"].ToString().Contains("无油机组"))
                    {
                        shpnumList[4] = item["shpnum"].ToString();
                        shpamtsList[4] = item["shpamts"].ToString();
                        ordnumList[4] = item["ordnum"].ToString();
                        ordamtsList[4] = item["ordamts"].ToString();
                    }
                    else
                    {
                        shpnumList[4] = "0";
                        shpamtsList[4] = "0";
                        ordnumList[4] = "0";
                        ordamtsList[4] = "0";
                    }
                    if (item["protype"].ToString().Contains("低环温热泵"))
                    {
                        sumshpnumK += Int32.Parse(item["shpnum"].ToString());
                        sumshpamtsK += Int32.Parse(item["shpamts"].ToString());
                        sumordnumK += Int32.Parse(item["ordnum"].ToString());
                        sumordamtsK += Int32.Parse(item["ordamts"].ToString());
                        shpnumList[5] = sumshpnumK.ToString();
                        shpamtsList[5] = sumshpamtsK.ToString();
                        ordnumList[5] = sumordnumK.ToString();
                        ordamtsList[5] = sumordamtsK.ToString();
                    }
                    if (item["protype"].ToString().Contains("离心机体"))
                    {
                        sumshpnumK += Int32.Parse(item["shpnum"].ToString());
                        sumshpamtsK += Int32.Parse(item["shpamts"].ToString());
                        sumordnumK += Int32.Parse(item["ordnum"].ToString());
                        sumordamtsK += Int32.Parse(item["ordamts"].ToString());
                        shpnumList[5] = sumshpnumK.ToString();
                        shpamtsList[5] = sumshpamtsK.ToString();
                        ordnumList[5] = sumordnumK.ToString();
                        ordamtsList[5] = sumordamtsK.ToString();
                    }
                    else 
                    {
                        shpnumList[5] = "0";
                        shpamtsList[5] = "0";
                        ordnumList[5] = "0";
                        ordamtsList[5] = "0";
                    }

                }
            }
            

            if (shpnumList.Count > 0)//判断出货台数不为0
            {
                for (int i = 0; i < shpnumList.Count; i++)
                {
                    result.Rows.Add(new object[] { "SHB出貨台數", shpnumList[0], shpnumList[1], shpnumList[2], shpnumList[3], shpnumList[4], shpnumList[5], "8", "9", "10" });//添加行
                    result.Rows.Add(new object[] { "提供集團報表出貨台數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                    result.Rows.Add(new object[] { "出貨台數差異數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                }
            }
           
            if (shpamtsList.Count > 0)//判断出货金额不为0
            {

                result.Rows.Add(new object[] { "SHB出貨金額", shpamtsList[0].ToString(), "3", "4", "5", "6", "7", "8", "9", "10" });
                result.Rows.Add(new object[] { "提供集團報表出貨台數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                result.Rows.Add(new object[] { "出貨金額差異數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });

            }
           
            if (ordnumList.Count > 0)//判断订单台数不为0
            {
                for (int i = 0; i < ordnumList.Count; i++)
                {
                    result.Rows.Add(new object[] { "SHB 訂單台數", ordnumList[0].ToString(), "3", "4", "5", "6", "7", "8", "9", "10" });
                    result.Rows.Add(new object[] { "提供集團報表訂單台數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                    result.Rows.Add(new object[] { "訂單台數差異數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                }
            }
           
            if (ordamtsList.Count > 0)//判断订单金额不为0
            {
                for (int i = 0; i < ordamtsList.Count; i++)
                {
                    result.Rows.Add(new object[] { "SHB訂單金額", ordamtsList[0].ToString(), "3", "4", "5", "6", "7", "8", "9", "10" });
                    result.Rows.Add(new object[] { "提供集團報表訂單台數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                    result.Rows.Add(new object[] { "訂單金額差異數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                }
            }
            if (ordamtsList.Count > 0)
            {
                for (int i = 0; i < ordamtsList.Count; i++)
                {
                    result.Rows.Add(new object[] { "SHB訂單金額", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                    result.Rows.Add(new object[] { "提供集團報表訂單台數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                    result.Rows.Add(new object[] { "訂單金額差異數", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
                }
            }
           
            result.AcceptChanges();
            ds.AcceptChanges();
        }

    }
}
