using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.BSC.Report;
using System.Data;

namespace Hanbell.GRpt.ServiceCB.MFCB.ToMonth
{
    /// <summary>
    /// 服务免费
    /// </summary>
    public class ServiceHelp : BscReport
    {
        public DateTime RptDatetime { get; set; }
        System.Data.IDbConnection connSybaseC = null;
        Hanbell.DBUtility.IDbHelper dbhC = null;
        public override System.Data.DataSet GetResult(string indexSn, DateTime dateNow)
        {
            //throw new NotImplementedException();
            DataSet ds = new DataSet();
            ds.Tables.Add(base.GetBscData(indexSn));
            return ds;
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


        #region 得到R_外
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultOut()
        {
            DataSet ds = new DataSet();
            DataTable dt_r_mb = this.SetR_MB_OUT();
            DataTable dt_r_sj = this.SetR_SJ_OUT();
            DataTable dt_r_kzl = SetR_KZL_OUT(dt_r_mb, dt_r_sj);
            ds.Tables.Add(dt_r_mb);
            ds.Tables.Add(dt_r_sj);
            ds.Tables.Add(dt_r_kzl);
            

            return ds;
        }
        /// <summary>
        /// R目标外帮助
        /// </summary>
        /// <param name="rptname">指标枚举</param>
        /// <param name="rowname">新行名称</param>
        /// <param name="dtR_MB_OUT">操作表</param>
        private void R_MB_OUT_HELP(ReportName rptname, string rowname, DataTable dtR_MB_OUT)
        {
            string sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, rptname);
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception(rptname + "  " + RptDatetime.Year + "指标未设定！");
            }
            DataTable dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos != null)
            {
                if (dtbos.Rows.Count > 0)
                {
                    DataRow newrow = dtR_MB_OUT.NewRow();
                    double total = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        newrow[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));
                        total += Convert.ToDouble(newrow[i]);
                    }
                    newrow[12] = total;
                    newrow[13] = rowname;
                    dtR_MB_OUT.Rows.Add(newrow);
                }
            }
        }

        /// <summary>
        /// 根据参数num中的值来对table作相加产生新行，并添加到table末尾
        /// </summary>
        /// <param name="table">要操作table【0-12列为数字，13列为行名称】</param>
        /// <param name="num">要相加的行位置</param>
        /// <param name="rowname">新行名称</param>
        private void SetDBAddByNum(DataTable table, int[] num, string rowname)
        {
            DataRow newrow = table.NewRow();//新行
            int count = table.Columns.Count;//列数
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < num.Length; j++)
                {
                    if ((i + 1) == num[j])//如果行的位置==数组中的数
                    {
                        for (int k = 0; k < 12; k++)
                        {
                            newrow[k] = Convert.ToDouble(newrow[k] == DBNull.Value ? 0 : newrow[k]) + Convert.ToDouble(table.Rows[i][k] == DBNull.Value ? 0 : table.Rows[i][k]);
                        }
                    }
                }
            }
            newrow[13] = rowname;
            table.Rows.Add(newrow);
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
        /// 
        /// </summary>
        /// <param name="protype"></param>
        /// <param name="areatype"></param>
        /// <param name="year"></param>
        /// <param name="dtR_SJ_OUT"></param>
        /// <param name="dtbsc"></param>
        /// <param name="rowname"></param>
        private void SetDB_R_SJ_OUT(string protype, string areatype, int year, DataTable dtR_SJ_OUT, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ_OUT.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal rhdfwll = 0;
            decimal rhdtravelcost = 0;
            decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = rhdfwll + rhdtravelcost + rhdfare;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ_OUT.Rows.Add(newrow);
        }
       
        /// <summary>
        /// 得到R目标外
        /// </summary>
        /// <returns></returns>
        private DataTable SetR_MB_OUT()
        {
            DataTable dtR_MB_OUT = BuildTB("R_MB");//目标表外
            this.R_MB_OUT_HELP(ReportName.Fw_R_HD, "MB_RHD", dtR_MB_OUT);//华东
            this.R_MB_OUT_HELP(ReportName.Fw_R_NJ, "MB_RNJ", dtR_MB_OUT);//南京
            this.R_MB_OUT_HELP(ReportName.Fw_R_HB, "MB_RHB", dtR_MB_OUT);//华北
            this.R_MB_OUT_HELP(ReportName.Fw_R_HN, "MB_RHN", dtR_MB_OUT);//华南
            this.R_MB_OUT_HELP(ReportName.Fw_R_CQ, "MB_RCQ", dtR_MB_OUT);//重庆
            SetDBAddByNum(dtR_MB_OUT, new int[5] { 1, 2, 3, 4, 5 }, "MB_DY");//当月
            SetDBLJByNum(dtR_MB_OUT, 6, "MB_LJ");//累计
            return dtR_MB_OUT;
        }

        /// <summary>
        /// 得到R实际外
        /// </summary>
        /// <returns></returns>
        private DataTable SetR_SJ_OUT()
        {
            string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataTable dtR_SJ_OUT = BuildTB("R_SJ");//目标表外
            this.SetDB_R_SJ_OUT("R", "HD", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHD");
            this.SetDB_R_SJ_OUT("R", "NJ", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RNJ");
            this.SetDB_R_SJ_OUT("R", "HB", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHB");
            this.SetDB_R_SJ_OUT("R", "HN", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHN");
            this.SetDB_R_SJ_OUT("R", "CQ", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RCQ");
            SetDBAddByNum(dtR_SJ_OUT, new int[5] { 1, 2, 3, 4, 5 }, "SJ_DY");//当月
            SetDBLJByNum(dtR_SJ_OUT, 6, "SJ_LJ");//累计
            return dtR_SJ_OUT;
        }

        /// <summary>
        /// 得到R控制率厂外
        /// <param name="R_MB_OUT">目标</param>
        /// <param name="R_SJ_OUT">实际</param>
        /// </summary>
        private DataTable SetR_KZL_OUT(DataTable R_MB_OUT,DataTable R_SJ_OUT)
        {
            DataTable dtR_KZL_OUT = BuildTB("R_KZL");//目标表外

            #region DY_KZL 当月控制率
            DataRow newrow_dy = dtR_KZL_OUT.NewRow();//当月控制率
            DataRow[] mb_dy = R_MB_OUT.Select("type='MB_DY'");//目标当月
            DataRow[] sj_dy = R_SJ_OUT.Select("type='SJ_DY'");//实际当月
            decimal a = 0;
            decimal b = 0;
            if (mb_dy.Length > 0 && sj_dy.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_dy[0][i] == DBNull.Value ? 0 : mb_dy[0][i]);
                    b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]);
                    newrow_dy[i] = decimal.Round(a / (b == 0 ? 1 : b) * 100, 2);
                }
                newrow_dy[13] = "DY_KZL";
            }
            #endregion

            #region LJ_KZL 累计控制率
            DataRow newrow_lj = dtR_KZL_OUT.NewRow();//累计控制率
            DataRow[] mb_lj = R_MB_OUT.Select("type='MB_LJ'");//目标累计
            DataRow[] sj_lj = R_SJ_OUT.Select("type='SJ_LJ'");//实际累计
            if (mb_lj.Length > 0 && sj_lj.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_lj[0][i] == DBNull.Value ? 0 : mb_lj[0][i]);
                    b = Convert.ToDecimal(sj_lj[0][i] == DBNull.Value ? 0 : sj_lj[0][i]);
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? 1 : b) * 100, 2);
                }
                newrow_lj[13] = "LJ_KZL";
            }
            #endregion

            dtR_KZL_OUT.Rows.Add(newrow_dy);
            dtR_KZL_OUT.Rows.Add(newrow_lj);
            return dtR_KZL_OUT;
        }

        #endregion


        //------------------------------------------------------------------------------------------


        #region 得到R_内

        /// <summary>
        /// R目标内帮助
        /// </summary>
        /// <param name="rptname">指标枚举</param>
        /// <param name="rowname">新行名称</param>
        /// <param name="dtR_MB_IN">操作表</param>
        private void R_MB_IN_HELP(ReportName rptname, string rowname, DataTable dtR_MB_IN)
        {
            string sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, rptname);
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception(rptname + "  " + RptDatetime.Year + "指标未设定！");
            }
            DataTable dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos != null)
            {
                if (dtbos.Rows.Count > 0)
                {
                    DataRow newrow = dtR_MB_IN.NewRow();
                    double total = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        newrow[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));
                        total += Convert.ToDouble(newrow[i]);
                    }
                    newrow[12] = total;
                    newrow[13] = rowname;
                    dtR_MB_IN.Rows.Add(newrow);
                }
            }
        }

        private DataTable SetR_MB_IN()
        {
            DataTable dtR_MB_IN = BuildTB("R_MB");//目标
            this.R_MB_IN_HELP(ReportName.Fw_R_HD, "MB_RHD", dtR_MB_IN);//华东
            this.R_MB_IN_HELP(ReportName.Fw_R_NJ, "MB_RNJ", dtR_MB_IN);//南京
            this.R_MB_IN_HELP(ReportName.Fw_R_HB, "MB_RHB", dtR_MB_IN);//华北
            this.R_MB_IN_HELP(ReportName.Fw_R_HN, "MB_RHN", dtR_MB_IN);//华南
            this.R_MB_IN_HELP(ReportName.Fw_R_CQ, "MB_RCQ", dtR_MB_IN);//重庆
            SetDBAddByNum(dtR_MB_IN, new int[5] { 1, 2, 3, 4, 5 }, "MB_DY");//当月
            SetDBLJByNum(dtR_MB_IN, 6, "MB_LJ");//累计
            return dtR_MB_IN;
        }

        private void SetDB_R_SJ_IN(string protype, string areatype, int year, DataTable dtR_SJ_IN, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ_IN.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal mancost = 0;
            decimal wxll = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                in1 = mancost + wxll;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ_IN.Rows.Add(newrow);
        }

        private DataTable SetR_SJ_IN()
        {
            string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataTable dtR_SJ_IN = BuildTB("R_SJ");
            this.SetDB_R_SJ_IN("R", "HD", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHD");
            this.SetDB_R_SJ_IN("R", "NJ", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RNJ");
            this.SetDB_R_SJ_IN("R", "HB", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHB");
            this.SetDB_R_SJ_IN("R", "HN", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHN");
            this.SetDB_R_SJ_IN("R", "CQ", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RCQ");
            SetDBAddByNum(dtR_SJ_IN, new int[5] { 1, 2, 3, 4, 5 }, "SJ_DY");//当月
            SetDBLJByNum(dtR_SJ_IN, 6, "SJ_LJ");//累计
            return dtR_SJ_IN;
        }

        private DataTable SetR_KZL_IN(DataTable R_MB_IN, DataTable R_SJ_IN)
        {
            DataTable dtR_KZL_IN = BuildTB("R_KZL");//目标表外

            #region DY_KZL 当月控制率
            DataRow newrow_dy = dtR_KZL_IN.NewRow();//当月控制率
            DataRow[] mb_dy = R_MB_IN.Select("type='MB_DY'");//目标当月
            DataRow[] sj_dy = R_SJ_IN.Select("type='SJ_DY'");//实际当月
            decimal a = 0;
            decimal b = 0;
            if (mb_dy.Length > 0 && sj_dy.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_dy[0][i] == DBNull.Value ? 0 : mb_dy[0][i]);
                    b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]);
                    newrow_dy[i] = decimal.Round(a / (b == 0 ? 1 : b) * 100, 2);
                }
                newrow_dy[13] = "DY_KZL";
            }
            #endregion

            #region LJ_KZL 累计控制率
            DataRow newrow_lj = dtR_KZL_IN.NewRow();//累计控制率
            DataRow[] mb_lj = R_MB_IN.Select("type='MB_LJ'");//目标累计
            DataRow[] sj_lj = R_SJ_IN.Select("type='SJ_LJ'");//实际累计
            if (mb_lj.Length > 0 && sj_lj.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_lj[0][i] == DBNull.Value ? 0 : mb_lj[0][i]);
                    b = Convert.ToDecimal(sj_lj[0][i] == DBNull.Value ? 0 : sj_lj[0][i]);
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? 1 : b) * 100, 2);
                }
                newrow_lj[13] = "LJ_KZL";
            }
            #endregion

            dtR_KZL_IN.Rows.Add(newrow_dy);
            dtR_KZL_IN.Rows.Add(newrow_lj);
            return dtR_KZL_IN;
        }

        public DataSet GetResultIn()
        {
            DataSet ds = new DataSet();
            DataTable dt_r_mb = this.SetR_MB_IN();
            DataTable dt_r_sj = this.SetR_SJ_IN();
            DataTable dt_r_kzl = SetR_KZL_IN(dt_r_mb, dt_r_sj);
            ds.Tables.Add(dt_r_mb);
            ds.Tables.Add(dt_r_sj);
            ds.Tables.Add(dt_r_kzl);


            return ds;
        }
        #endregion


        //------------------------------------------------------------------------------------------

        #region 得到R汇总

        private void MB_HELP(ReportName rptname,string rowname,DataTable dtR_MB)
        {
            string sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, rptname);
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception(rptname + "  " + RptDatetime.Year + "指标未设定！");
            }
            DataTable dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos != null)
            {
                if (dtbos.Rows.Count > 0)
                {
                    DataRow newrow = dtR_MB.NewRow();
                    double a = 0;
                    double b = 0;
                    double total = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        a = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));
                        b = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));
                        newrow[i] = a + b;
                        total += (a + b);
                    }
                    newrow[12] = total;
                    newrow[13] = rowname;
                    dtR_MB.Rows.Add(newrow);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable SetR_MB()
        {
            DataTable dtR_MB = BuildTB("R_MB");//目标
            this.MB_HELP(ReportName.Fw_R_HD, "MB_RHD", dtR_MB);//华东
            this.MB_HELP(ReportName.Fw_R_NJ, "MB_RNJ", dtR_MB);//南京
            this.MB_HELP(ReportName.Fw_R_HB, "MB_RHB", dtR_MB);//华北
            this.MB_HELP(ReportName.Fw_R_HN, "MB_RHN", dtR_MB);//华南
            this.MB_HELP(ReportName.Fw_R_CQ, "MB_RCQ", dtR_MB);//重庆
            SetDBAddByNum(dtR_MB, new int[5] { 1, 2, 3, 4, 5 }, "MB_DY");//当月
            SetDBLJByNum(dtR_MB, 6, "MB_LJ");//累计
            return dtR_MB;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="protype"></param>
        /// <param name="areatype"></param>
        /// <param name="year"></param>
        /// <param name="dtR_SJ"></param>
        /// <param name="dtbsc"></param>
        /// <param name="rowname"></param>
        private void SetDB_R_SJ(string protype, string areatype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal mancost = 0;
            decimal wxll = 0;
            decimal rhdfwll = 0;
            decimal rhdtravelcost = 0;
            decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost + wxll + rhdfwll + rhdtravelcost + rhdfare;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ.Rows.Add(newrow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable SetR_SJ() 
        {
            string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataTable dtR_SJ = BuildTB("R_SJ");
            this.SetDB_R_SJ("R", "HD", RptDatetime.Year, dtR_SJ, dt, "SJ_RHD");
            this.SetDB_R_SJ("R", "NJ", RptDatetime.Year, dtR_SJ, dt, "SJ_RNJ");
            this.SetDB_R_SJ("R", "HB", RptDatetime.Year, dtR_SJ, dt, "SJ_RHB");
            this.SetDB_R_SJ("R", "HN", RptDatetime.Year, dtR_SJ, dt, "SJ_RHN");
            this.SetDB_R_SJ("R", "CQ", RptDatetime.Year, dtR_SJ, dt, "SJ_RCQ");
            SetDBAddByNum(dtR_SJ, new int[5] { 1, 2, 3, 4, 5 }, "SJ_DY");//当月
            SetDBLJByNum(dtR_SJ, 6, "SJ_LJ");//累计
            return dtR_SJ;
        }

        private DataTable SetR_KZL(DataTable R_MB,DataTable R_SJ)
        {
            DataTable dtR_KZL = BuildTB("R_KZL");//目标表外

            #region DY_KZL 当月控制率
            DataRow newrow_dy = dtR_KZL.NewRow();//当月控制率
            DataRow[] mb_dy = R_MB.Select("type='MB_DY'");//目标当月
            DataRow[] sj_dy = R_SJ.Select("type='SJ_DY'");//实际当月
            decimal a = 0;
            decimal b = 0;
            if (mb_dy.Length > 0 && sj_dy.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_dy[0][i] == DBNull.Value ? 0 : mb_dy[0][i]);
                    b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]);
                    newrow_dy[i] = decimal.Round(a / (b == 0 ? 1 : b) * 100, 2);
                }
                newrow_dy[13] = "DY_KZL";
            }
            #endregion

            #region LJ_KZL 累计控制率
            DataRow newrow_lj = dtR_KZL.NewRow();//累计控制率
            DataRow[] mb_lj = R_MB.Select("type='MB_LJ'");//目标累计
            DataRow[] sj_lj = R_SJ.Select("type='SJ_LJ'");//实际累计
            if (mb_lj.Length > 0 && sj_lj.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_lj[0][i] == DBNull.Value ? 0 : mb_lj[0][i]);
                    b = Convert.ToDecimal(sj_lj[0][i] == DBNull.Value ? 0 : sj_lj[0][i]);
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? 1 : b) * 100, 2);
                }
                newrow_lj[13] = "LJ_KZL";
            }
            #endregion

            dtR_KZL.Rows.Add(newrow_dy);
            dtR_KZL.Rows.Add(newrow_lj);
            return dtR_KZL;
        }

        public DataSet GetResultALL()
        {
            DataSet ds = new DataSet();
            DataTable dt_r_mb = this.SetR_MB();
            DataTable dt_r_sj = this.SetR_SJ();
            DataTable dt_r_kzl = SetR_KZL(dt_r_mb, dt_r_sj);
            ds.Tables.Add(dt_r_mb);
            ds.Tables.Add(dt_r_sj);
            ds.Tables.Add(dt_r_kzl);
            return ds;
        }

        #endregion


        //-------------------------------------------------------------------------------------------

        #region 得到非R系其他产品别相关
        private void SetDB_SJ(string protype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal mancost = 0;
            decimal wxll = 0;
            decimal rhdfwll = 0;
            decimal rhdtravelcost = 0;
            decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                rhdfwll = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                rhdtravelcost = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                rhdfare = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost + wxll + rhdfwll + rhdtravelcost + rhdfare;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ.Rows.Add(newrow);
        }

        public DataTable SetKZL(string tbname,DataTable MB, DataTable SJ)
        {
            DataTable dtKZL = BuildTB(tbname);

            #region DY_KZL 当月控制率
            DataRow newrow_dy = dtKZL.NewRow();//当月控制率
            DataRow[] mb_dy = MB.Select("type='MB_DY'");//目标当月
            DataRow[] sj_dy = SJ.Select("type='SJ_DY'");//实际当月
            decimal a = 0;
            decimal b = 0;
            if (mb_dy.Length > 0 && sj_dy.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_dy[0][i] == DBNull.Value ? 0 : mb_dy[0][i]);
                    b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]);
                    newrow_dy[i] = decimal.Round(a / (b == 0 ? 1 : b) * 100, 2);
                }
                newrow_dy[13] = "DY_KZL";
            }
            #endregion

            #region LJ_KZL 累计控制率
            DataRow newrow_lj = dtKZL.NewRow();//累计控制率
            DataRow[] mb_lj = MB.Select("type='MB_LJ'");//目标累计
            DataRow[] sj_lj = SJ.Select("type='SJ_LJ'");//实际累计
            if (mb_lj.Length > 0 && sj_lj.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_lj[0][i] == DBNull.Value ? 0 : mb_lj[0][i]);
                    b = Convert.ToDecimal(sj_lj[0][i] == DBNull.Value ? 0 : sj_lj[0][i]);
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? 1 : b) * 100, 2);
                }
                newrow_lj[13] = "LJ_KZL";
            }
            #endregion

            dtKZL.Rows.Add(newrow_dy);
            dtKZL.Rows.Add(newrow_lj);
            return dtKZL;
        }


        #region AA
        private DataTable SetAA_MB()
        {
            DataTable dtAA_MB = BuildTB("AA_MB");
            MB_HELP(ReportName.Fw_AA, "MB_DY", dtAA_MB);
            SetDBLJByNum(dtAA_MB, 1, "MB_LJ");
            return dtAA_MB;
        }
        private DataTable SetAA_SJ(DataTable dtbsc)
        {
            DataTable dtAA_SJ = BuildTB("AA_SJ");
            SetDB_SJ("AA",RptDatetime.Year, dtAA_SJ, dtbsc, "SJ_DY");
            SetDBLJByNum(dtAA_SJ, 1, "SJ_LJ");
            return dtAA_SJ;
        }
        private DataTable SetAA_KZL(DataTable AA_MB,DataTable AA_SJ)
        {
            DataTable dtKZL = this.SetKZL("AA_KZL", AA_MB, AA_SJ);
            return dtKZL;
        }
        #endregion

        #region AH
        private DataTable SetAH_MB()
        {
            DataTable dtAH_MB = BuildTB("AH_MB");
            MB_HELP(ReportName.Fw_AA, "MB_DY", dtAH_MB);
            SetDBLJByNum(dtAH_MB, 1, "MB_LJ");
            return dtAH_MB;
        }
        private DataTable SetAH_SJ(DataTable dtbsc)
        {
            DataTable dtAH_SJ = BuildTB("AH_SJ");
            SetDB_SJ("AH", RptDatetime.Year, dtAH_SJ, dtbsc, "SJ_DY");
            SetDBLJByNum(dtAH_SJ, 1, "SJ_LJ");
            return dtAH_SJ;
        }
        private DataTable SetAH_KZL(DataTable AH_MB, DataTable AH_SJ)
        {
            DataTable dtKZL = this.SetKZL("AH_KZL", AH_MB, AH_SJ);
            return dtKZL;
        }
        #endregion

        #region AP
        private DataTable SetP_MB()
        {
            DataTable dtP_MB = BuildTB("P_MB");
            MB_HELP(ReportName.Fw_AA, "MB_DY", dtP_MB);
            SetDBLJByNum(dtP_MB, 1, "MB_LJ");
            return dtP_MB;
        }
        private DataTable SetP_SJ(DataTable dtbsc)
        {
            DataTable dtP_SJ = BuildTB("P_SJ");
            SetDB_SJ("AH", RptDatetime.Year, dtP_SJ, dtbsc, "SJ_DY");
            SetDBLJByNum(dtP_SJ, 1, "SJ_LJ");
            return dtP_SJ;
        }
        private DataTable SetP_KZL(DataTable P_MB, DataTable P_SJ)
        {
            DataTable dtKZL = this.SetKZL("P_KZL", P_MB, P_SJ);
            return dtKZL;
        }
        #endregion

        #region CM
        private DataTable SetCM_MB()
        {
            DataTable dtCM_MB = BuildTB("CM_MB");
            MB_HELP(ReportName.Fw_AA, "MB_DY", dtCM_MB);
            SetDBLJByNum(dtCM_MB, 1, "MB_LJ");
            return dtCM_MB;
        }
        private DataTable SetCM_SJ(DataTable dtbsc)
        {
            DataTable dtCM_SJ = BuildTB("CM_SJ");
            SetDB_SJ("AH", RptDatetime.Year, dtCM_SJ, dtbsc, "SJ_DY");
            SetDBLJByNum(dtCM_SJ, 1, "SJ_LJ");
            return dtCM_SJ;
        }
        private DataTable SetCM_KZL(DataTable CM_MB, DataTable CM_SJ)
        {
            DataTable dtKZL = this.SetKZL("CM_KZL", CM_MB, CM_SJ);
            return dtKZL;
        }
        #endregion

        /// <summary>
        /// 得到非R系列其他产品所有
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultOther()
        {
            string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataSet ds = new DataSet();

            DataTable dt_aa_mb = SetAA_MB();
            DataTable dt_aa_sj = SetAA_SJ(dt);
            DataTable dt_aa_kzl = SetAA_KZL(dt_aa_mb, dt_aa_sj);
            DataTable dt_ah_mb = SetAH_MB();
            DataTable dt_ah_sj = SetAH_SJ(dt);
            DataTable dt_ah_kzl = SetAH_KZL(dt_ah_mb, dt_ah_sj);
            DataTable dt_p_mb = SetP_MB();
            DataTable dt_p_sj = SetP_SJ(dt);
            DataTable dt_p_kzl = SetP_KZL(dt_p_mb, dt_p_sj);
            DataTable dt_cm_mb = SetCM_MB();
            DataTable dt_cm_sj = SetCM_SJ(dt);
            DataTable dt_cm_kzl = SetCM_KZL(dt_cm_mb, dt_cm_sj);
            ds.Tables.Add(dt_aa_mb);
            ds.Tables.Add(dt_aa_sj);
            ds.Tables.Add(dt_aa_kzl);
            ds.Tables.Add(dt_ah_mb);
            ds.Tables.Add(dt_ah_sj);
            ds.Tables.Add(dt_ah_kzl);
            ds.Tables.Add(dt_p_mb);
            ds.Tables.Add(dt_p_sj);
            ds.Tables.Add(dt_p_kzl);
            ds.Tables.Add(dt_cm_mb);
            ds.Tables.Add(dt_cm_sj);
            ds.Tables.Add(dt_cm_kzl);

            return ds;
        }




        #endregion


        //---------------------------------------------------------------------------------------------
        

    }
}
