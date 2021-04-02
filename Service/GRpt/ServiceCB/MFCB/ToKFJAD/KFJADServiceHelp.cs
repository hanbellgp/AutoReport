using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Hanbell.GRpt.ServiceCB.MFCB.ToKFJAD
{
    public class KFJADServiceHelp : Hanbell.BSC.Report.BscReport
    {


        public DateTime RptDatetime { get; set; }
        System.Data.IDbConnection connSybaseC = null;
        Hanbell.DBUtility.IDbHelper dbhC = null;
        public override System.Data.DataSet GetResult(string indexSn, DateTime dateNow)
        {
            //throw new NotImplementedException();
            return null;
        }


        public DataSet GetResult()
        {
            string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservicejad where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);
            DataSet ds = new DataSet();
            DataTable dtr = this.GetR(dt);
            DataTable dtaa = this.GetAA(dt);
            DataTable dtah = this.GetAH(dt);
            DataTable dtp = this.GetP(dt);
            DataTable dtcm = this.GetCM(dt);
            DataTable dtall = this.GetALL(dt);
            ds.Tables.Add(dtr);
            ds.Tables.Add(dtaa);
            ds.Tables.Add(dtah);
            ds.Tables.Add(dtp);
            ds.Tables.Add(dtcm);
            ds.Tables.Add(dtall);
            return ds;
        }







        /// <summary>
        /// 横向累加table中根据num对应的行并添加到table末尾
        /// </summary>
        /// <param name="table">要操作table【0-12列为数字，13列为行名称】</param>
        /// <param name="num">要累加哪一行</param>
        /// <param name="rowname">新行名称</param>
        private void SetDBLJByNum(DataTable table, int num, string rowname)
        {
            DataRow newrow = table.NewRow();
            DataRow row = table.Rows[num - 1];
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    newrow[i] = Convert.ToDouble(newrow[i] == DBNull.Value ? 0 : newrow[i]) + Convert.ToDouble(row[j] == DBNull.Value ? 0 : row[j]);
                }
            }
            newrow[13] = rowname;
            table.Rows.Add(newrow);
        }

        /// <summary>
        /// 检索帮助
        /// </summary>
        /// <param name="protype"></param>
        /// <param name="areatype"></param>
        /// <param name="dtall"></param>
        /// <returns></returns>
        private DataRow[] GetHelp(string protype, string areatype, DataTable dtall)
        {
            DataRow[] rows = dtall.Select("protype='" + protype + "' and areatype='" + areatype + "' and year=" + this.RptDatetime.Year);
            if (rows != null)
            {
                if (rows.Length > 0)
                {
                    return rows;
                }

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtall"></param>
        /// <param name="protype"></param>
        /// <returns></returns>
        private DataRow[] GetHelp(DataTable dtall, string protype)
        {
            DataRow[] rows = dtall.Select("protype='" + protype + "'");
            if (rows != null)
            {
                if (rows.Length > 0)
                {
                    return rows;
                }

            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt">操作表</param>
        /// <param name="dtall">总表</param>
        /// <param name="protype">产品别</param>
        /// <param name="areatype">区域别</param>
        /// <param name="rowname">行名</param>
        private void SetTB(DataTable dt, DataTable dtall, string protype, string areatype, string rowname)
        {
            DataRow newrow = dt.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = 0;
            }
            decimal total = 0;
            decimal mancost = 0;
            decimal wxll = 0;
            decimal fwll = 0;
            decimal travelcost = 0;
            decimal fare = 0;
            decimal in1 = 0;
            //protype='" + protype + "' and areatype='" + areatype + "' and year=" + this.RptDatetime.Year
            for (int i = 0; i < 12; i++)
            {
                mancost = dtall.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtall.Select("protype='" + protype + "' and areatype='" + areatype + "'  and month=" + (i + 1)).Sum(p => p.Field<decimal>("wxll"));//差旅
                fwll = dtall.Select("protype='" + protype + "' and areatype='" + areatype + "'  and month=" + (i + 1)).Sum(p => p.Field<decimal>("fwll"));//服务领料
                travelcost = dtall.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("travelcost"));//差旅
                fare = dtall.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost + wxll + fwll + travelcost + fare;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;//合计
            newrow[13] = rowname;//行名
            dt.Rows.Add(newrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtall"></param>
        /// <param name="protype"></param>
        /// <param name="rowname"></param>
        private void SetTB(DataTable dt, DataTable dtall, string protype, string rowname)
        {
            DataRow newrow = dt.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = 0;
            }
            decimal total = 0;
            decimal mancost = 0;
            decimal wxll = 0;
            decimal fwll = 0;
            decimal travelcost = 0;
            decimal fare = 0;
            decimal in1 = 0;
            //protype='" + protype + "' and areatype='" + areatype + "' and year=" + this.RptDatetime.Year
            for (int i = 0; i < 12; i++)
            {
                mancost = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("wxll"));//差旅
                fwll = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("fwll"));//服务领料
                travelcost = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("travelcost"));//差旅
                fare = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost + wxll + fwll + travelcost + fare;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;//合计
            newrow[13] = rowname;//行名
            dt.Rows.Add(newrow);
        }

        /// <summary>
        /// 得到当月
        /// </summary>
        /// <param name="dt">当月要插入的表</param>
        /// <param name="dtall">总表</param>
        /// <param name="protype">机型别</param>
        private void SetTOT(DataTable dt, DataTable dtall, string protype, string rowname)
        {
            DataRow newrow = dt.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = 0;
            }
            decimal total = 0;
            decimal mancost = 0;
            decimal wxll = 0;
            decimal fwll = 0;
            decimal travelcost = 0;
            decimal fare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("wxll"));//差旅
                fwll = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("fwll"));//服务领料
                travelcost = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("travelcost"));//差旅
                fare = dtall.Select("protype='" + protype + "' and month=" + (i + 1)).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost + wxll + fwll + travelcost + fare;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dt.Rows.Add(newrow);
        }






        /// <summary>
        /// 得到R
        /// </summary>
        /// <returns></returns>
        private DataTable GetR(DataTable dtall)
        {
            DataTable dtR = this.BuildTB("R");//构建R结构表
            this.SetTB(dtR, dtall, "R", "HD", "R_HD");
            this.SetTB(dtR, dtall, "R", "NJ", "R_NJ");
            this.SetTB(dtR, dtall, "R", "HB", "R_HB");
            this.SetTB(dtR, dtall, "R", "HN", "R_HN");
            this.SetTB(dtR, dtall, "R", "CQ", "R_CQ");
            this.SetTOT(dtR, dtall, "R", "TOT_DY");

            SetDBLJByNum(dtR, 6, "R_LJ");

            return dtR;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetAA(DataTable dtall)
        {
            DataTable dtAA = this.BuildTB("AA");
            this.SetTB(dtAA, dtall, "AA", "AA_DY");
            SetDBLJByNum(dtAA, 1, "AA_LJ");
            return dtAA;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtall"></param>
        /// <returns></returns>
        private DataTable GetAH(DataTable dtall)
        {
            DataTable dtAH = this.BuildTB("AH");
            this.SetTB(dtAH, dtall, "AH", "AH_DY");
            SetDBLJByNum(dtAH, 1, "AH_LJ");
            return dtAH;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtall"></param>
        /// <returns></returns>
        private DataTable GetP(DataTable dtall)
        {
            DataTable dtP = this.BuildTB("P");
            this.SetTB(dtP, dtall, "P", "P_DY");
            SetDBLJByNum(dtP, 1, "P_LJ");
            return dtP;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtall"></param>
        /// <returns></returns>
        private DataTable GetCM(DataTable dtall)
        {
            DataTable dtCM = this.BuildTB("CM");
            this.SetTB(dtCM, dtall, "CM", "CM_DY");
            SetDBLJByNum(dtCM, 1, "CM_LJ");
            return dtCM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtall"></param>
        /// <returns></returns>
        private DataTable GetALL(DataTable dtall)
        {
            DataTable dtTOT = this.BuildTB("ALL");
            DataRow newrow = dtTOT.NewRow();
            decimal total = 0;
            decimal mancost = 0;
            decimal wxll = 0;
            decimal fwll = 0;
            decimal travelcost = 0;
            decimal fare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtall.Select("month=" + (i + 1)).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtall.Select("month=" + (i + 1)).Sum(p => p.Field<decimal>("wxll"));//差旅
                fwll = dtall.Select("month=" + (i + 1)).Sum(p => p.Field<decimal>("fwll"));//服务领料
                travelcost = dtall.Select("month=" + (i + 1)).Sum(p => p.Field<decimal>("travelcost"));//差旅
                fare = dtall.Select("month=" + (i + 1)).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost + wxll + fwll + travelcost + fare;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = "TOT_DY";
            dtTOT.Rows.Add(newrow);
            //累计
            this.SetDBLJByNum(dtTOT, 1, "TOT_LJ");
            return dtTOT;
        }

        /// <summary>
        /// 表结构建立
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataTable BuildTB(string tablename)
        {
            DataTable dtbsc = new DataTable(tablename);
            dtbsc.Columns.Add("1", typeof(decimal));
            dtbsc.Columns.Add("2", typeof(decimal));
            dtbsc.Columns.Add("3", typeof(decimal));
            dtbsc.Columns.Add("4", typeof(decimal));
            dtbsc.Columns.Add("5", typeof(decimal));
            dtbsc.Columns.Add("6", typeof(decimal));
            dtbsc.Columns.Add("7", typeof(decimal));
            dtbsc.Columns.Add("8", typeof(decimal));
            dtbsc.Columns.Add("9", typeof(decimal));
            dtbsc.Columns.Add("10", typeof(decimal));
            dtbsc.Columns.Add("11", typeof(decimal));
            dtbsc.Columns.Add("12", typeof(decimal));
            dtbsc.Columns.Add("13", typeof(decimal));
            dtbsc.Columns.Add("type", typeof(string));

            return dtbsc;
        }









    }
}
