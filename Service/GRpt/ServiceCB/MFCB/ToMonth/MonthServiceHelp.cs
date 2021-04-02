using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.BSC.Report;
using System.Data;
using Hanbell.DBUtility;

namespace Hanbell.GRpt.ServiceCB.MFCB.ToMonth
{
    /// <summary>
    /// 服务免费
    /// </summary>
    public class MonthServiceHelp : BscReport
    {
        public DateTime RptDatetime { get; set; }
        private IDbConnection connSybaseC { get; set; }
        private IDbHelper dbhC { get; set; }

        public MonthServiceHelp()
        {
            string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);
        }


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
        /// R目标外帮助 dttype='去年同期值' 厂外用
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
                    if ((i) == num[j])//如果行的位置==数组中的数
                    {
                        for (int k = 0; k <= 12; k++)
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
            DataRow row = table.Rows[num];//最后一行
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
        /// 累计值
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="rowindex1">累计行1位置</param>
        /// <param name="rowindex2">累计行2位置</param>
        /// <param name="rowname">表名</param>
        private void SetDBLJByNum2(DataTable table, int rowindex1, int rowindex2, string rowname)
        {
            DataRow newrow = table.NewRow();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    newrow[i] = Convert.ToDouble(newrow[i] == DBNull.Value ? 0 : newrow[i])
                                + Convert.ToDouble(table.Rows[rowindex1][j] == DBNull.Value ? 0 : table.Rows[rowindex1][j])
                                + Convert.ToDouble(table.Rows[rowindex2][j] == DBNull.Value ? 0 : table.Rows[rowindex2][j]);
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
            decimal rhdxszr = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                rhdxszr = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("xszr"));//销售折让
                in1 = rhdfwll + rhdtravelcost + rhdfare + rhdxszr;
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
            SetDBAddByNum(dtR_MB_OUT, new int[5] { 0, 1, 2, 3, 4 }, "MB_DY");//当月
            SetDBLJByNum(dtR_MB_OUT, 5, "MB_LJ");//累计
            return dtR_MB_OUT;
        }

        /// <summary>
        /// 得到R实际外
        /// </summary>
        /// <returns></returns>
        private DataTable SetR_SJ_OUT()
        {

            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataTable dtR_SJ_OUT = BuildTB("R_SJ");//目标表外
            this.SetDB_R_SJ_OUT("R", "HD", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHD");
            this.SetDB_R_SJ_ZLKK("R", "HD", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHD_ZLKK");//SJ_RHD_ZLKK
            this.SetDB_R_SJ_OUT("R", "NJ", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RNJ");
            this.SetDB_R_SJ_ZLKK("R", "NJ", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RNJ_ZLKK");//SJ_RNJ_ZLKK
            this.SetDB_R_SJ_OUT("R", "HB", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHB");
            this.SetDB_R_SJ_ZLKK("R", "HB", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHB_ZLKK");//SJ_RHB_ZLKK
            this.SetDB_R_SJ_OUT("R", "HN", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHN");
            this.SetDB_R_SJ_ZLKK("R", "HN", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHN_ZLKK");//SJ_RHN_ZLKK
            this.SetDB_R_SJ_OUT("R", "CQ", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RCQ");
            this.SetDB_R_SJ_ZLKK("R", "CQ", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RCQ_ZLKK");//SJ_RCQ_ZLKK
            SetDBAddByNum(dtR_SJ_OUT, new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, "SJ_DY");//当月
            //SetDBAddByNum(dtR_SJ_OUT, new int[5] { 1, 3, 5, 7, 9 }, "SJ_DY_ZLKK");//当月质量扣款SJ_DY_ZLKK
            SetDBAddByNum(dtR_SJ_OUT, new int[6] { 1, 3, 5, 7, 9, 10 }, "SJ_DYHJ");//当月累计=当月+质量扣款
            SetDBLJByNum(dtR_SJ_OUT, 10, "SJ_LJ");//累计

            return dtR_SJ_OUT;
        }



        /// <summary>
        /// 得到R控制率厂外
        /// <param name="R_MB_OUT">目标</param>
        /// <param name="R_SJ_OUT">实际</param>
        /// </summary>
        private DataTable SetR_KZL_OUT(DataTable R_MB_OUT, DataTable R_SJ_OUT)
        {
            DataTable dtR_KZL_OUT = BuildTB("R_KZL");//目标表外

            #region DY_KZL 当月控制率
            DataRow newrow_dy = dtR_KZL_OUT.NewRow();//当月控制率
            DataRow[] mb_dy = R_MB_OUT.Select("type='MB_DY'");//目标当月
            DataRow[] sj_dy = R_SJ_OUT.Select("type='SJ_DY'");//实际当月
            //DataRow[] sj_zlkk = R_SJ.Select("type ='SJ_ZLKK'");//质量扣款
            DataRow[] sj_zlkk = R_SJ_OUT.Select("type  like '%ZLKK'").Clone() as DataRow[];//质量扣款
            DataRow nwr = R_SJ_OUT.NewRow();
            nwr[0] = 0; nwr[1] = 0; nwr[2] = 0; nwr[3] = 0; nwr[4] = 0; nwr[5] = 0; nwr[6] = 0; nwr[7] = 0; nwr[8] = 0; nwr[9] = 0; nwr[10] = 0; nwr[11] = 0; nwr[12] = 0;



            //这里把几行的数据加到第一行去
            for (int j = 1; j < sj_zlkk.Length; j++)
            {
                nwr[0] = Convert.ToDecimal(nwr[0]) + Convert.ToDecimal(sj_zlkk[j][0]);
                nwr[1] = Convert.ToDecimal(nwr[1]) + Convert.ToDecimal(sj_zlkk[j][1]);
                nwr[2] = Convert.ToDecimal(nwr[2]) + Convert.ToDecimal(sj_zlkk[j][2]);
                nwr[3] = Convert.ToDecimal(nwr[3]) + Convert.ToDecimal(sj_zlkk[j][3]);
                nwr[4] = Convert.ToDecimal(nwr[4]) + Convert.ToDecimal(sj_zlkk[j][4]);
                nwr[5] = Convert.ToDecimal(nwr[5]) + Convert.ToDecimal(sj_zlkk[j][5]);
                nwr[6] = Convert.ToDecimal(nwr[6]) + Convert.ToDecimal(sj_zlkk[j][6]);
                nwr[7] = Convert.ToDecimal(nwr[7]) + Convert.ToDecimal(sj_zlkk[j][7]);
                nwr[8] = Convert.ToDecimal(nwr[8]) + Convert.ToDecimal(sj_zlkk[j][8]);
                nwr[9] = Convert.ToDecimal(nwr[9]) + Convert.ToDecimal(sj_zlkk[j][9]);
                nwr[10] = Convert.ToDecimal(nwr[10]) + Convert.ToDecimal(sj_zlkk[j][10]);
                nwr[11] = Convert.ToDecimal(nwr[11]) + Convert.ToDecimal(sj_zlkk[j][11]);
                nwr[12] = Convert.ToDecimal(nwr[12]) + Convert.ToDecimal(sj_zlkk[j][12]);
            }
            decimal a = 0;
            decimal b = 0;
            if (mb_dy.Length > 0 && sj_dy.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_dy[0][i] == DBNull.Value ? 0 : mb_dy[0][i]);
                    //b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]);
                    b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]) + Convert.ToDecimal(nwr[i] == DBNull.Value ? 0 : nwr[i]);
                    newrow_dy[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
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
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
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
        /// R目标内帮助 dtype='目标值' 厂内用
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
            SetDBAddByNum(dtR_MB_IN, new int[5] { 0, 1, 2, 3, 4 }, "MB_DY");//当月
            SetDBLJByNum(dtR_MB_IN, 5, "MB_LJ");//累计
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
            //string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            //dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            //connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataTable dtR_SJ_IN = BuildTB("R_SJ");
            this.SetDB_R_SJ_IN("R", "HD", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHD");
            this.SetDB_R_SJ_IN("R", "NJ", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RNJ");
            this.SetDB_R_SJ_IN("R", "HB", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHB");
            this.SetDB_R_SJ_IN("R", "HN", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHN");
            this.SetDB_R_SJ_IN("R", "CQ", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RCQ");
            SetDBAddByNum(dtR_SJ_IN, new int[5] { 0, 1, 2, 3, 4 }, "SJ_DY");//当月
            SetDBLJByNum(dtR_SJ_IN, 5, "SJ_LJ");//累计
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
                    newrow_dy[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
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
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
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

        private void MB_HELP(ReportName rptname, string rowname, DataTable dtR_MB)
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
        private void MB_HELP_FWB(ReportName rptname, string rowname, DataTable dtR_MB)
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
                        //b = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));
                        newrow[i] = a + b;
                        total += (a + b);
                    }
                    newrow[12] = total;
                    newrow[13] = rowname;
                    dtR_MB.Rows.Add(newrow);
                }
            }
        }
        private void MB_HELP_HDBB(ReportName rptname, string rowname, DataTable dtR_MB)
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
                        //a = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));
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
            SetDBAddByNum(dtR_MB, new int[5] { 0, 1, 2, 3, 4 }, "MB_DY");//当月
            SetDBLJByNum(dtR_MB, 5, "MB_LJ");//累计
            return dtR_MB;
        }

        private DataTable SetR_MB2()
        {
            DataTable dtR_MB = BuildTB("R_MB");//目标
            this.MB_HELP_FWB(ReportName.FW_R_FW, "MB_RFWB", dtR_MB);//服务部201150202
            this.MB_HELP_HDBB(ReportName.Fw_R_HD, "MB_RHDBB", dtR_MB);//华东
            //this.MB_HELP(ReportName.FW_R_FW, "", dtR_MB);//服务部201150202
            this.MB_HELP(ReportName.Fw_R_NJ, "MB_RNJ", dtR_MB);//南京
            this.MB_HELP(ReportName.Fw_R_HB, "MB_RHB", dtR_MB);//华北
            this.MB_HELP(ReportName.Fw_R_HN, "MB_RHN", dtR_MB);//华南
            this.MB_HELP(ReportName.Fw_R_CQ, "MB_RCQ", dtR_MB);//重庆
            SetDBAddByNum(dtR_MB, new int[6] { 0, 1, 2, 3, 4, 5 }, "MB_DY");//当月
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
            decimal rhdxszr = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                rhdxszr = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("xszr"));//销售折让
                in1 = mancost + wxll + rhdfwll + rhdtravelcost + rhdfare + rhdxszr;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ.Rows.Add(newrow);
        }
        private void SetDB_R_SJ_FWB(string protype, string areatype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal mancost = 0;
            decimal wxll = 0;
            //decimal rhdfwll = 0;
            //decimal rhdtravelcost = 0;
            //decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                //rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                //rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                //rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost + wxll;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ.Rows.Add(newrow);
        }
        private void SetDB_R_SJ_HDBB(string protype, string areatype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ.NewRow();
            decimal total = 0;
            //R_HD_OUT
            //decimal mancost = 0;
            //decimal wxll = 0;
            decimal rhdfwll = 0;
            decimal rhdtravelcost = 0;
            decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                //mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                //wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                //rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                //rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                //rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                //in1 = mancost + wxll + rhdfwll + rhdtravelcost + rhdfare;
                //total += in1;
                //newrow[i] = in1;

                rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = rhdfwll + rhdtravelcost + rhdfare;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ.Rows.Add(newrow);
        }


        //ADD BY C1368 增加境外的数据
        private void SetDB_R_JW(string protype, string areatype, int year, DataTable dtR_JW, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_JW.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal mancost = 0;
            //decimal wxll = 0;
            //decimal rhdfwll = 0;
            //decimal rhdtravelcost = 0;
            //decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                //wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                //rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                //rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                //rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost ;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_JW.Rows.Add(newrow);
        }
        private void SetDB_A_JW(string protype, string areatype, int year, DataTable dtA_JW, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtA_JW.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal mancost = 0;
            //decimal wxll = 0;
            //decimal rhdfwll = 0;
            //decimal rhdtravelcost = 0;
            //decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                //wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                //rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                //rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                //rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtA_JW.Rows.Add(newrow);
        }
        private void SetDB_P_JW(string protype, string areatype, int year, DataTable dtP_JW, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtP_JW.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal mancost = 0;
            //decimal wxll = 0;
            //decimal rhdfwll = 0;
            //decimal rhdtravelcost = 0;
            //decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                //wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                //rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                //rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                //rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtP_JW.Rows.Add(newrow);
        }


        private void SetDB_CM_JW(string protype, string areatype, int year, DataTable dtCM_JW, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtCM_JW.NewRow();
            decimal total = 0;
            //R_HD_OUT
            decimal mancost = 0;
            //decimal wxll = 0;
            //decimal rhdfwll = 0;
            //decimal rhdtravelcost = 0;
            //decimal rhdfare = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                //wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                //rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                //rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                //rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                in1 = mancost;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtCM_JW.Rows.Add(newrow);
        }
        //ADD END

        /// <summary>
        /// 质量扣款
        /// </summary>
        /// <param name="protype"></param>
        /// <param name="areatype"></param>
        /// <param name="year"></param>
        /// <param name="dtR_SJ"></param>
        /// <param name="dtbsc"></param>
        /// <param name="rowname"></param>
        private void SetDB_R_SJ_ZLKK(string protype, string areatype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ.NewRow();
            decimal total = 0;
            decimal zlkk = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {

                zlkk = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("zlkk"));
                //zlkk = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and areatype='" + areatype + "' and year=" + year).Sum(p => p.Field<decimal>("zlkk"));
                in1 = zlkk;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ.Rows.Add(newrow);
        }

        private void SetDB_HD_SJ_ZLKK(string protype, string areatype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ.NewRow();
            decimal total = 0;
            decimal zlkk = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {

                zlkk = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("zlkk"));
                //zlkk = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and areatype='" + areatype + "' and year=" + year).Sum(p => p.Field<decimal>("zlkk"));
                in1 = zlkk;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dtR_SJ.Rows.Add(newrow);
        }
        /// <summary>
        /// 服务部质量扣款 取outtarget字段
        /// </summary>
        /// <param name="protype"></param>
        /// <param name="areatype"></param>
        /// <param name="year"></param>
        /// <param name="dtR_SJ"></param>
        /// <param name="dtbsc"></param>
        /// <param name="rowname"></param>
        private void SetDB_FW_SJ_ZLKK(string protype, string areatype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ.NewRow();
            decimal total = 0;
            decimal zlkk = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {

                zlkk = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("outtarget"));
                //zlkk = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and areatype='" + areatype + "' and year=" + year).Sum(p => p.Field<decimal>("zlkk"));
                in1 = zlkk;
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
            //string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            //dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            //connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataTable dtR_SJ = BuildTB("R_SJ");
            this.SetDB_R_SJ("R", "HD", RptDatetime.Year, dtR_SJ, dt, "SJ_RHD");
            this.SetDB_R_SJ("R", "NJ", RptDatetime.Year, dtR_SJ, dt, "SJ_RNJ");
            this.SetDB_R_SJ("R", "HB", RptDatetime.Year, dtR_SJ, dt, "SJ_RHB");
            this.SetDB_R_SJ("R", "HN", RptDatetime.Year, dtR_SJ, dt, "SJ_RHN");
            this.SetDB_R_SJ("R", "CQ", RptDatetime.Year, dtR_SJ, dt, "SJ_RCQ");
            SetDBAddByNum(dtR_SJ, new int[5] { 0, 1, 2, 3, 4 }, "SJ_DY");//当月
            SetDBLJByNum(dtR_SJ, 5, "SJ_LJ");//累计
            return dtR_SJ;
        }
        private DataTable SetR_SJ2()
        {
            //string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            //dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            //connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataTable dtR_SJ = BuildTB("R_SJ");
            this.SetDB_R_SJ_FWB("R", "HD", RptDatetime.Year, dtR_SJ, dt, "SJ_RFWB");
            this.SetDB_R_SJ_HDBB("R", "HD", RptDatetime.Year, dtR_SJ, dt, "SJ_RHDBB");
            this.SetDB_R_SJ("R", "NJ", RptDatetime.Year, dtR_SJ, dt, "SJ_RNJ");
            this.SetDB_R_SJ("R", "HB", RptDatetime.Year, dtR_SJ, dt, "SJ_RHB");
            this.SetDB_R_SJ("R", "HN", RptDatetime.Year, dtR_SJ, dt, "SJ_RHN");
            this.SetDB_R_SJ("R", "CQ", RptDatetime.Year, dtR_SJ, dt, "SJ_RCQ");
            SetDBAddByNum(dtR_SJ, new int[6] { 0, 1, 2, 3, 4, 5 }, "SJ_DY");//当月


            this.SetDB_R_SJ_ZLKK("R", null, RptDatetime.Year, dtR_SJ, dt, "SJ_ZLKK");//质量扣款

            SetDBAddByNum(dtR_SJ, new int[2] {6,7} ,"SJ_DYHJ");//当月合计=当月+质量扣款
            

            SetDBLJByNum2(dtR_SJ, 6, 7, "SJ_LJ");//累计
            return dtR_SJ;
        }
        private DataTable SetR_SJ3()
        {
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataTable dtR_SJ_ALL = BuildTB("R_SJ");//目标表外
            this.SetDB_R_SJ_FWB("R", "HD", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RFWB");
            this.SetDB_R_SJ_HDBB("R", "HD", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RHDBB");
            this.SetDB_R_SJ_ZLKK("R", "HD", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RHD_ZLKK");//SJ_RHD_ZLKK
            this.SetDB_R_SJ("R", "NJ", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RNJ");
            this.SetDB_R_SJ_ZLKK("R", "NJ", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RNJ_ZLKK");//SJ_RNJ_ZLKK
            this.SetDB_R_SJ("R", "HB", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RHB");
            this.SetDB_R_SJ_ZLKK("R", "HB", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RHB_ZLKK");//SJ_RHB_ZLKK
            this.SetDB_R_SJ("R", "HN", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RHN");
            this.SetDB_R_SJ_ZLKK("R", "HN", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RHN_ZLKK");//SJ_RHN_ZLKK
            this.SetDB_R_SJ("R", "CQ", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RCQ");
            this.SetDB_R_SJ_ZLKK("R", "CQ", RptDatetime.Year, dtR_SJ_ALL, dt, "SJ_RCQ_ZLKK");//SJ_RCQ_ZLKK
            SetDBAddByNum(dtR_SJ_ALL, new int[11] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "SJ_DY");//当月

            SetDBLJByNum(dtR_SJ_ALL, 11, "SJ_LJ");//累计

            return dtR_SJ_ALL;
        }
        private DataTable SetR_KZL(DataTable R_MB, DataTable R_SJ)
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
                    newrow_dy[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
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
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
                }
                newrow_lj[13] = "LJ_KZL";
            }
            #endregion

            dtR_KZL.Rows.Add(newrow_dy);
            dtR_KZL.Rows.Add(newrow_lj);
            return dtR_KZL;
        }
        private DataTable SetR_KZL2(DataTable R_MB, DataTable R_SJ)
        {
            DataTable dtR_KZL = BuildTB("R_KZL");//目标表外

            #region DY_KZL 当月控制率
            DataRow newrow_dy = dtR_KZL.NewRow();//当月控制率
            DataRow[] mb_dy = R_MB.Select("type='MB_DY'");//目标当月
            DataRow[] sj_dy = R_SJ.Select("type='SJ_DY'");//实际当月
            DataRow[] sj_zlkk = R_SJ.Select("type='SJ_ZLKK'");//质量扣款
            decimal a = 0;
            decimal b = 0;
            if (mb_dy.Length > 0 && sj_dy.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_dy[0][i] == DBNull.Value ? 0 : mb_dy[0][i]);
                    //b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]);
                    b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]) + Convert.ToDecimal(sj_zlkk[0][i] == DBNull.Value ? 0 : sj_zlkk[0][i]);
                    newrow_dy[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
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
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
                }
                newrow_lj[13] = "LJ_KZL";
            }
            #endregion

            dtR_KZL.Rows.Add(newrow_dy);
            dtR_KZL.Rows.Add(newrow_lj);
            return dtR_KZL;
        }
        private DataTable SetR_KZL3(DataTable R_MB, DataTable R_SJ)
        {
            DataTable dtR_KZL = BuildTB("R_KZL");//目标表外

            #region DY_KZL 当月控制率
            DataRow newrow_dy = dtR_KZL.NewRow();//当月控制率
            DataRow[] mb_dy = R_MB.Select("type='MB_DY'");//目标当月
            DataRow[] sj_dy = R_SJ.Select("type='SJ_DY'");//实际当月
            //DataRow[] sj_zlkk = R_SJ.Select("type ='SJ_ZLKK'");//质量扣款

            DataRow[] sj_zlkk = R_SJ.Select("type  like '%ZLKK'");//质量扣款
            DataRow nwr = R_SJ.NewRow();
            nwr[0] = 0; nwr[1] = 0; nwr[2] = 0; nwr[3] = 0; nwr[4] = 0; nwr[5] = 0; nwr[6] = 0; nwr[7] = 0; nwr[8] = 0; nwr[9] = 0; nwr[10] = 0; nwr[11] = 0; nwr[12] = 0;



            //这里把几行的数据加到第一行去
            for (int j = 1; j < sj_zlkk.Length; j++)
            {
                nwr[0] = Convert.ToDecimal(nwr[0]) + Convert.ToDecimal(sj_zlkk[j][0]);
                nwr[1] = Convert.ToDecimal(nwr[1]) + Convert.ToDecimal(sj_zlkk[j][1]);
                nwr[2] = Convert.ToDecimal(nwr[2]) + Convert.ToDecimal(sj_zlkk[j][2]);
                nwr[3] = Convert.ToDecimal(nwr[3]) + Convert.ToDecimal(sj_zlkk[j][3]);
                nwr[4] = Convert.ToDecimal(nwr[4]) + Convert.ToDecimal(sj_zlkk[j][4]);
                nwr[5] = Convert.ToDecimal(nwr[5]) + Convert.ToDecimal(sj_zlkk[j][5]);
                nwr[6] = Convert.ToDecimal(nwr[6]) + Convert.ToDecimal(sj_zlkk[j][6]);
                nwr[7] = Convert.ToDecimal(nwr[7]) + Convert.ToDecimal(sj_zlkk[j][7]);
                nwr[8] = Convert.ToDecimal(nwr[8]) + Convert.ToDecimal(sj_zlkk[j][8]);
                nwr[9] = Convert.ToDecimal(nwr[9]) + Convert.ToDecimal(sj_zlkk[j][9]);
                nwr[10] = Convert.ToDecimal(nwr[10]) + Convert.ToDecimal(sj_zlkk[j][10]);
                nwr[11] = Convert.ToDecimal(nwr[11]) + Convert.ToDecimal(sj_zlkk[j][11]);
                nwr[12] = Convert.ToDecimal(nwr[12]) + Convert.ToDecimal(sj_zlkk[j][12]);
            }


            decimal a = 0;
            decimal b = 0;
            if (mb_dy.Length > 0 && sj_dy.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_dy[0][i] == DBNull.Value ? 0 : mb_dy[0][i]);
                    //b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]);
                    b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]) + Convert.ToDecimal(nwr[i] == DBNull.Value ? 0 : nwr[i]);
                    newrow_dy[i] = Math.Abs(decimal.Round(a / (b == 0 ? a : b) * 100, 2));
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
                    newrow_lj[i] = decimal.Round(a / (b == 0 ? a : b) * 100, 2);
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

        /// <summary>
        /// 新综合，ds中包含服务部，华东本部,质量扣款
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultALL2()
        {
            DataSet ds = new DataSet();
            DataTable dt_r_mb = this.SetR_MB2();
            DataTable dt_r_sj = this.SetR_SJ2();
            DataTable dt_r_kzl = SetR_KZL2(dt_r_mb, dt_r_sj);
            ds.Tables.Add(dt_r_mb);
            ds.Tables.Add(dt_r_sj);
            ds.Tables.Add(dt_r_kzl);
            return ds;
        }

        /// <summary>
        /// 新综合，ds中包含服务部，华东本部,质量扣款
        /// </summary>
        /// <returns></returns>
        public DataSet GetResultALL3()
        {
            DataSet ds = new DataSet();
            DataTable dt_r_mb = this.SetR_MB2();
            ///////DataTable dt_r_sj = this.SetR_SJ2();
            DataTable dt_r_sj = this.SetR_SJ3();
            DataTable dt_r_kzl = SetR_KZL3(dt_r_mb, dt_r_sj);
            ds.Tables.Add(dt_r_mb);
            ds.Tables.Add(dt_r_sj);
            ds.Tables.Add(dt_r_kzl);
            return ds;
        }
        #endregion

        public DataSet GetResultHJ(DataSet dsout, DataSet dsin, DataSet dsall, DataSet dsOther)
        {
            DataSet ds = new DataSet();
            if (dsout != null)
            {
                #region 目标合计
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsout.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);

                DataRow[] R_MB_LJ = dsout.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #region 实际合计
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsout.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);

                DataRow[] R_SJ_LJ = dsout.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }
            else if (dsin != null)
            {
                #region 目标合计
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsin.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);

                DataRow[] R_MB_LJ = dsin.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #region 实际合计
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsin.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);

                DataRow[] R_SJ_LJ = dsin.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }
            else
            {
                #region 目标合计
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsall.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);

                DataRow[] R_MB_LJ = dsall.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #region 实际合计
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsall.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);

                DataRow[] R_SJ_LJ = dsall.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }



            return ds;
        }

        public DataSet GetResultHJ2(DataSet dsout, DataSet dsin, DataSet dsall, DataSet dsOther)
        {
            DataSet ds = new DataSet();
            if (dsout != null)
            {
                #region 目标合计

                #region 目标当月
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsout.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow[] KM_MB_DY = dsOther.Tables["KM_MB"].Select("type='MB_DY'");

                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i])
                                + Convert.ToDouble(KM_MB_DY[0][i] == DBNull.Value ? 0 : KM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);
                #endregion

                #region 目标累计
                DataRow[] R_MB_LJ = dsout.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                DataRow[] KM_MB_LJ = dsOther.Tables["KM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i])
                                + Convert.ToDouble(KM_MB_LJ[0][i] == DBNull.Value ? 0 : KM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #endregion

                #region 实际合计

                #region 实际当月
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsout.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                DataRow[] KM_SJ_DY = dsOther.Tables["KM_SJ"].Select("type='SJ_DY'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i])
                                + Convert.ToDouble(KM_SJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);
                #endregion

                #region 实际质量扣款
                //DataRow[] R_SJ_ZLKK_DY = dsout.Tables["R_SJ"].Select("type='SJ_ZLKK'");//质量扣款//逻辑改变
                DataRow[] R_SJ_ZLKK_DY = dsout.Tables["R_SJ"].Select("type like '%ZLKK'");
                //R的质量扣款有多行需要加起来
                DataRow nwr = dsout.Tables["R_SJ"].NewRow();
                nwr[0] = 0; nwr[1] = 0; nwr[2] = 0; nwr[3] = 0; nwr[4] = 0; nwr[5] = 0; nwr[6] = 0; nwr[7] = 0; nwr[8] = 0; nwr[9] = 0; nwr[10] = 0; nwr[11] = 0; nwr[12] = 0;



                //这里把几行的数据加到第一行去
                for (int j = 1; j < R_SJ_ZLKK_DY.Length; j++)
                {
                    nwr[0] = Convert.ToDecimal(nwr[0]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][0]);
                    nwr[1] = Convert.ToDecimal(nwr[1]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][1]);
                    nwr[2] = Convert.ToDecimal(nwr[2]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][2]);
                    nwr[3] = Convert.ToDecimal(nwr[3]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][3]);
                    nwr[4] = Convert.ToDecimal(nwr[4]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][4]);
                    nwr[5] = Convert.ToDecimal(nwr[5]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][5]);
                    nwr[6] = Convert.ToDecimal(nwr[6]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][6]);
                    nwr[7] = Convert.ToDecimal(nwr[7]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][7]);
                    nwr[8] = Convert.ToDecimal(nwr[8]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][8]);
                    nwr[9] = Convert.ToDecimal(nwr[9]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][9]);
                    nwr[10] = Convert.ToDecimal(nwr[10]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][10]);
                    nwr[11] = Convert.ToDecimal(nwr[11]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][11]);
                    nwr[12] = Convert.ToDecimal(nwr[12]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][12]);
                }


                DataRow[] AA_SJ_ZLKK_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] AH_SJ_ZLKK_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] P_SJ_ZLKK_DY = dsOther.Tables["P_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] CM_SJ_ZLKK_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] KM_SJ_ZLKK_DY = dsOther.Tables["KM_SJ"].Select("type='SJ_ZLKK'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(nwr[i] == DBNull.Value ? 0 : nwr[i])
                                + Convert.ToDouble(AA_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AA_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(AH_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AH_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(P_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : P_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(CM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CM_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(KM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : KM_SJ_ZLKK_DY[0][i]);
                }
                newrow[13] = "SJ_ZLKK";
                dtTOT_SJ.Rows.Add(newrow);
                #endregion

                #region 实际累计
                DataRow[] R_SJ_LJ = dsout.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                DataRow[] KM_SJ_LJ = dsOther.Tables["KM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i])
                                + Convert.ToDouble(KM_SJ_LJ[0][i] == DBNull.Value ? 0 : KM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }
            else if (dsin != null)
            {
                #region 目标合计
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsin.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);

                DataRow[] R_MB_LJ = dsin.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #region 实际合计
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsin.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);
                DataRow[] R_SJ_LJ = dsin.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }
            else
            {
                #region 目标合计
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsall.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);

                DataRow[] R_MB_LJ = dsall.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #region 实际合计
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsall.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                DataRow[] KM_SJ_DY = dsOther.Tables["KM_SJ"].Select("type='SJ_DY'");//柯茂

                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i])
                                + Convert.ToDouble(KM_SJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);


                DataRow[] R_SJ_ZLKK_DY = dsall.Tables["R_SJ"].Select("type='SJ_ZLKK'");//质量扣款
                DataRow[] AA_SJ_ZLKK_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] AH_SJ_ZLKK_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] P_SJ_ZLKK_DY = dsOther.Tables["P_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] CM_SJ_ZLKK_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] KM_SJ_ZLKK_DY = dsOther.Tables["KM_SJ"].Select("type='SJ_ZLKK'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : R_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(AA_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AA_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(AH_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AH_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(P_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : P_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(CM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CM_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(KM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : KM_SJ_ZLKK_DY[0][i]);
                }
                newrow[13] = "SJ_ZLKK";
                dtTOT_SJ.Rows.Add(newrow);


                DataRow[] R_SJ_LJ = dsall.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                DataRow[] KM_SJ_LJ = dsOther.Tables["KM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i])
                                + Convert.ToDouble(KM_SJ_LJ[0][i] == DBNull.Value ? 0 : KM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }



            return ds;
        }

        public DataSet GetResultHJ3(DataSet dsout, DataSet dsin, DataSet dsall, DataSet dsOther)
        {
            DataSet ds = new DataSet();
            if (dsout != null)
            {
                #region 目标合计

                #region 目标当月
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsout.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow[] KM_MB_DY = dsOther.Tables["KM_MB"].Select("type='MB_DY'");

                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i])
                                + Convert.ToDouble(KM_MB_DY[0][i] == DBNull.Value ? 0 : KM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);
                #endregion

                #region 目标累计
                DataRow[] R_MB_LJ = dsout.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                DataRow[] KM_MB_LJ = dsOther.Tables["KM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i])
                                + Convert.ToDouble(KM_MB_LJ[0][i] == DBNull.Value ? 0 : KM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #endregion

                #region 实际合计

                #region 实际当月
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsout.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                DataRow[] KM_SJ_DY = dsOther.Tables["KM_SJ"].Select("type='SJ_DY'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i])
                                + Convert.ToDouble(KM_SJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);
                #endregion

                #region 实际质量扣款
                //DataRow[] R_SJ_ZLKK_DY = dsout.Tables["R_SJ"].Select("type='SJ_ZLKK'");//质量扣款//逻辑改变
                DataRow[] R_SJ_ZLKK_DY = dsout.Tables["R_SJ"].Select("type like '%ZLKK'");
                //R的质量扣款有多行需要加起来
                //这里把几行的数据加到第一行去
                DataRow nwr = dsout.Tables["R_SJ"].NewRow();
                nwr[0] = 0; nwr[1] = 0; nwr[2] = 0; nwr[3] = 0; nwr[4] = 0; nwr[5] = 0; nwr[6] = 0; nwr[7] = 0; nwr[8] = 0; nwr[9] = 0; nwr[10] = 0; nwr[11] = 0; nwr[12] = 0;



                //这里把几行的数据加到第一行去
                for (int j = 1; j < R_SJ_ZLKK_DY.Length; j++)
                {
                    nwr[0] = Convert.ToDecimal(nwr[0]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][0]);
                    nwr[1] = Convert.ToDecimal(nwr[1]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][1]);
                    nwr[2] = Convert.ToDecimal(nwr[2]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][2]);
                    nwr[3] = Convert.ToDecimal(nwr[3]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][3]);
                    nwr[4] = Convert.ToDecimal(nwr[4]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][4]);
                    nwr[5] = Convert.ToDecimal(nwr[5]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][5]);
                    nwr[6] = Convert.ToDecimal(nwr[6]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][6]);
                    nwr[7] = Convert.ToDecimal(nwr[7]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][7]);
                    nwr[8] = Convert.ToDecimal(nwr[8]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][8]);
                    nwr[9] = Convert.ToDecimal(nwr[9]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][9]);
                    nwr[10] = Convert.ToDecimal(nwr[10]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][10]);
                    nwr[11] = Convert.ToDecimal(nwr[11]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][11]);
                    nwr[12] = Convert.ToDecimal(nwr[12]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][12]);
                }


                DataRow[] AA_SJ_ZLKK_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] AH_SJ_ZLKK_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] P_SJ_ZLKK_DY = dsOther.Tables["P_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] CM_SJ_ZLKK_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] KM_SJ_ZLKK_DY = dsOther.Tables["KM_SJ"].Select("type='SJ_ZLKK'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(nwr[i] == DBNull.Value ? 0 : nwr[i])
                                + Convert.ToDouble(AA_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AA_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(AH_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AH_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(P_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : P_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(CM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CM_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(KM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : KM_SJ_ZLKK_DY[0][i]);
                }
                newrow[13] = "SJ_ZLKK";
                dtTOT_SJ.Rows.Add(newrow);
                #endregion

                #region 实际累计
                DataRow[] R_SJ_LJ = dsout.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                DataRow[] KM_SJ_LJ = dsOther.Tables["KM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i])
                                + Convert.ToDouble(KM_SJ_LJ[0][i] == DBNull.Value ? 0 : KM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }
            else if (dsin != null)
            {
                #region 目标合计
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsin.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);

                DataRow[] R_MB_LJ = dsin.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #region 实际合计
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsin.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);
                DataRow[] R_SJ_LJ = dsin.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }
            else
            {
                #region 目标合计
                DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
                DataRow[] R_MB_DY = dsall.Tables["R_MB"].Select("type='MB_DY'");
                DataRow[] AA_MB_DY = dsOther.Tables["AA_MB"].Select("type='MB_DY'");
                DataRow[] AH_MB_DY = dsOther.Tables["AH_MB"].Select("type='MB_DY'");
                DataRow[] P_MB_DY = dsOther.Tables["P_MB"].Select("type='MB_DY'");
                DataRow[] CM_MB_DY = dsOther.Tables["CM_MB"].Select("type='MB_DY'");
                DataRow newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_DY[0][i] == DBNull.Value ? 0 : R_MB_DY[0][i])
                                + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                                + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                                + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                                + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i]);
                }
                newrow[13] = "MB_DY";
                dtTOT_MB.Rows.Add(newrow);

                DataRow[] R_MB_LJ = dsall.Tables["R_MB"].Select("type='MB_LJ'");
                DataRow[] AA_MB_LJ = dsOther.Tables["AA_MB"].Select("type='MB_LJ'");
                DataRow[] AH_MB_LJ = dsOther.Tables["AH_MB"].Select("type='MB_LJ'");
                DataRow[] P_MB_LJ = dsOther.Tables["P_MB"].Select("type='MB_LJ'");
                DataRow[] CM_MB_LJ = dsOther.Tables["CM_MB"].Select("type='MB_LJ'");
                newrow = dtTOT_MB.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_MB_LJ[0][i] == DBNull.Value ? 0 : R_MB_LJ[0][i])
                                + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                                + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                                + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                                + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i]);
                }
                newrow[13] = "MB_LJ";
                dtTOT_MB.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_MB);
                #endregion

                #region 实际合计
                DataTable dtTOT_SJ = BuildTB("TOT_SJ");
                DataRow[] R_SJ_DY = dsall.Tables["R_SJ"].Select("type='SJ_DY'");
                DataRow[] AA_SJ_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_DY'");
                DataRow[] AH_SJ_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_DY'");
                DataRow[] P_SJ_DY = dsOther.Tables["P_SJ"].Select("type='SJ_DY'");
                DataRow[] CM_SJ_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_DY'");
                DataRow[] KM_SJ_DY = dsOther.Tables["KM_SJ"].Select("type='SJ_DY'");//柯茂

                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_DY[0][i] == DBNull.Value ? 0 : R_SJ_DY[0][i])
                                + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                                + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                                + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                                + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i])
                                + Convert.ToDouble(KM_SJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DY[0][i]);
                }
                newrow[13] = "SJ_DY";
                dtTOT_SJ.Rows.Add(newrow);


                DataRow[] R_SJ_ZLKK_DY = dsall.Tables["R_SJ"].Select("type  like '%ZLKK'");//质量扣款
                //这里把几行的数据加到第一行去
                DataRow nwr = dsall.Tables["R_SJ"].NewRow();
                nwr[0] = 0; nwr[1] = 0; nwr[2] = 0; nwr[3] = 0; nwr[4] = 0; nwr[5] = 0; nwr[6] = 0; nwr[7] = 0; nwr[8] = 0; nwr[9] = 0; nwr[10] = 0; nwr[11] = 0; nwr[12] = 0;



                //这里把几行的数据加到第一行去
                for (int j = 1; j < R_SJ_ZLKK_DY.Length; j++)
                {
                    nwr[0] = Convert.ToDecimal(nwr[0]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][0]);
                    nwr[1] = Convert.ToDecimal(nwr[1]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][1]);
                    nwr[2] = Convert.ToDecimal(nwr[2]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][2]);
                    nwr[3] = Convert.ToDecimal(nwr[3]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][3]);
                    nwr[4] = Convert.ToDecimal(nwr[4]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][4]);
                    nwr[5] = Convert.ToDecimal(nwr[5]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][5]);
                    nwr[6] = Convert.ToDecimal(nwr[6]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][6]);
                    nwr[7] = Convert.ToDecimal(nwr[7]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][7]);
                    nwr[8] = Convert.ToDecimal(nwr[8]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][8]);
                    nwr[9] = Convert.ToDecimal(nwr[9]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][9]);
                    nwr[10] = Convert.ToDecimal(nwr[10]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][10]);
                    nwr[11] = Convert.ToDecimal(nwr[11]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][11]);
                    nwr[12] = Convert.ToDecimal(nwr[12]) + Convert.ToDecimal(R_SJ_ZLKK_DY[j][12]);
                }




                DataRow[] AA_SJ_ZLKK_DY = dsOther.Tables["AA_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] AH_SJ_ZLKK_DY = dsOther.Tables["AH_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] P_SJ_ZLKK_DY = dsOther.Tables["P_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] CM_SJ_ZLKK_DY = dsOther.Tables["CM_SJ"].Select("type='SJ_ZLKK'");
                DataRow[] KM_SJ_ZLKK_DY = dsOther.Tables["KM_SJ"].Select("type='SJ_ZLKK'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(nwr[i] == DBNull.Value ? 0 : nwr[i])
                                + Convert.ToDouble(AA_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AA_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(AH_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AH_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(P_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : P_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(CM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CM_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(KM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : KM_SJ_ZLKK_DY[0][i]);
                }
                newrow[13] = "SJ_ZLKK";
                dtTOT_SJ.Rows.Add(newrow);


                DataRow[] R_SJ_LJ = dsall.Tables["R_SJ"].Select("type='SJ_LJ'");
                DataRow[] AA_SJ_LJ = dsOther.Tables["AA_SJ"].Select("type='SJ_LJ'");
                DataRow[] AH_SJ_LJ = dsOther.Tables["AH_SJ"].Select("type='SJ_LJ'");
                DataRow[] P_SJ_LJ = dsOther.Tables["P_SJ"].Select("type='SJ_LJ'");
                DataRow[] CM_SJ_LJ = dsOther.Tables["CM_SJ"].Select("type='SJ_LJ'");
                DataRow[] KM_SJ_LJ = dsOther.Tables["KM_SJ"].Select("type='SJ_LJ'");
                newrow = dtTOT_SJ.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    newrow[i] = Convert.ToDouble(R_SJ_LJ[0][i] == DBNull.Value ? 0 : R_SJ_LJ[0][i])
                                + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                                + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                                + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                                + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i])
                                + Convert.ToDouble(KM_SJ_LJ[0][i] == DBNull.Value ? 0 : KM_SJ_LJ[0][i]);
                }
                newrow[13] = "SJ_LJ";
                dtTOT_SJ.Rows.Add(newrow);
                ds.Tables.Add(dtTOT_SJ);
                #endregion

                #region 控制率
                DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
                ds.Tables.Add(dtTOT_KZL);
                #endregion
            }



            return ds;
        }

        public DataSet GetResultHJ4(DataSet dsall)
        {
            DataSet ds = new DataSet();

            #region 目标合计
            DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
            DataRow[] FW_MB_DY = dsall.Tables["FW_MB"].Select("type='MB_DY'");
            DataRow[] HD_MB_DY = dsall.Tables["HD_MB"].Select("type='MB_DY'");
            DataRow[] NJ_MB_DY = dsall.Tables["NJ_MB"].Select("type='MB_DY'");
            DataRow[] GZ_MB_DY = dsall.Tables["GZ_MB"].Select("type='MB_DY'");
            DataRow[] JN_MB_DY = dsall.Tables["JN_MB"].Select("type='MB_DY'");
            DataRow[] CQ_MB_DY = dsall.Tables["CQ_MB"].Select("type='MB_DY'");

            DataRow[] AA_MB_DY = dsall.Tables["AA_MB"].Select("type='MB_DY'");
            DataRow[] AH_MB_DY = dsall.Tables["AH_MB"].Select("type='MB_DY'");
            DataRow[] P_MB_DY = dsall.Tables["P_MB"].Select("type='MB_DY'");
            DataRow[] CM_MB_DY = dsall.Tables["CM_MB"].Select("type='MB_DY'");
            DataRow[] KM_MB_DY = dsall.Tables["KM_MB"].Select("type='MB_DY'");
            DataRow newrow = dtTOT_MB.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = Convert.ToDouble(FW_MB_DY[0][i] == DBNull.Value ? 0 : FW_MB_DY[0][i])
                            + Convert.ToDouble(HD_MB_DY[0][i] == DBNull.Value ? 0 : HD_MB_DY[0][i])
                            + Convert.ToDouble(NJ_MB_DY[0][i] == DBNull.Value ? 0 : NJ_MB_DY[0][i])
                            + Convert.ToDouble(GZ_MB_DY[0][i] == DBNull.Value ? 0 : GZ_MB_DY[0][i])
                            + Convert.ToDouble(JN_MB_DY[0][i] == DBNull.Value ? 0 : JN_MB_DY[0][i])
                            + Convert.ToDouble(CQ_MB_DY[0][i] == DBNull.Value ? 0 : CQ_MB_DY[0][i])

                            + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                            + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                            + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                            + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i])
                            + Convert.ToDouble(KM_MB_DY[0][i] == DBNull.Value ? 0 : KM_MB_DY[0][i]);
            }
            newrow[13] = "MB_DY";
            dtTOT_MB.Rows.Add(newrow);

            DataRow[] FW_MB_LJ = dsall.Tables["FW_MB"].Select("type='MB_LJ'");
            DataRow[] HD_MB_LJ = dsall.Tables["HD_MB"].Select("type='MB_LJ'");
            DataRow[] NJ_MB_LJ = dsall.Tables["NJ_MB"].Select("type='MB_LJ'");
            DataRow[] GZ_MB_LJ = dsall.Tables["GZ_MB"].Select("type='MB_LJ'");
            DataRow[] JN_MB_LJ = dsall.Tables["JN_MB"].Select("type='MB_LJ'");
            DataRow[] CQ_MB_LJ = dsall.Tables["CQ_MB"].Select("type='MB_LJ'");

            DataRow[] AA_MB_LJ = dsall.Tables["AA_MB"].Select("type='MB_LJ'");
            DataRow[] AH_MB_LJ = dsall.Tables["AH_MB"].Select("type='MB_LJ'");
            DataRow[] P_MB_LJ = dsall.Tables["P_MB"].Select("type='MB_LJ'");
            DataRow[] CM_MB_LJ = dsall.Tables["CM_MB"].Select("type='MB_LJ'");
            DataRow[] KM_MB_LJ = dsall.Tables["KM_MB"].Select("type='MB_LJ'");
            newrow = dtTOT_MB.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = Convert.ToDouble(FW_MB_LJ[0][i] == DBNull.Value ? 0 : FW_MB_LJ[0][i])
                            + Convert.ToDouble(HD_MB_LJ[0][i] == DBNull.Value ? 0 : HD_MB_LJ[0][i])
                            + Convert.ToDouble(NJ_MB_LJ[0][i] == DBNull.Value ? 0 : NJ_MB_LJ[0][i])
                            + Convert.ToDouble(GZ_MB_LJ[0][i] == DBNull.Value ? 0 : GZ_MB_LJ[0][i])
                            + Convert.ToDouble(JN_MB_LJ[0][i] == DBNull.Value ? 0 : JN_MB_LJ[0][i])
                            + Convert.ToDouble(CQ_MB_LJ[0][i] == DBNull.Value ? 0 : CQ_MB_LJ[0][i])

                            + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                            + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                            + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                            + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i])
                            + Convert.ToDouble(KM_MB_LJ[0][i] == DBNull.Value ? 0 : KM_MB_LJ[0][i]);
            }
            newrow[13] = "MB_LJ";
            dtTOT_MB.Rows.Add(newrow);
            ds.Tables.Add(dtTOT_MB);
            #endregion

            #region 实际合计
            DataTable dtTOT_SJ = BuildTB("TOT_SJ");
            DataRow[] FW_SJ_DY = dsall.Tables["FW_SJ"].Select("type='SJ_DY'");
            DataRow[] HD_SJ_DY = dsall.Tables["HD_SJ"].Select("type='SJ_DY'");
            DataRow[] NJ_SJ_DY = dsall.Tables["NJ_SJ"].Select("type='SJ_DY'");
            DataRow[] GZ_SJ_DY = dsall.Tables["GZ_SJ"].Select("type='SJ_DY'");
            DataRow[] JN_SJ_DY = dsall.Tables["JN_SJ"].Select("type='SJ_DY'");
            DataRow[] CQ_SJ_DY = dsall.Tables["CQ_SJ"].Select("type='SJ_DY'");

            DataRow[] AA_SJ_DY = dsall.Tables["AA_SJ"].Select("type='SJ_DY'");
            DataRow[] AH_SJ_DY = dsall.Tables["AH_SJ"].Select("type='SJ_DY'");
            DataRow[] P_SJ_DY = dsall.Tables["P_SJ"].Select("type='SJ_DY'");
            DataRow[] CM_SJ_DY = dsall.Tables["CM_SJ"].Select("type='SJ_DY'");
            DataRow[] KM_SJ_DY = dsall.Tables["KM_SJ"].Select("type='SJ_DY'");//柯茂

            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = Convert.ToDouble(FW_SJ_DY[0][i] == DBNull.Value ? 0 : FW_SJ_DY[0][i])
                            + Convert.ToDouble(HD_SJ_DY[0][i] == DBNull.Value ? 0 : HD_SJ_DY[0][i])
                            + Convert.ToDouble(NJ_SJ_DY[0][i] == DBNull.Value ? 0 : NJ_SJ_DY[0][i])
                            + Convert.ToDouble(GZ_SJ_DY[0][i] == DBNull.Value ? 0 : GZ_SJ_DY[0][i])
                            + Convert.ToDouble(JN_SJ_DY[0][i] == DBNull.Value ? 0 : JN_SJ_DY[0][i])
                            + Convert.ToDouble(CQ_SJ_DY[0][i] == DBNull.Value ? 0 : CQ_SJ_DY[0][i])

                            + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                            + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                            + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                            + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i])
                            + Convert.ToDouble(KM_SJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DY[0][i]);
            }
            newrow[13] = "SJ_DY";
            dtTOT_SJ.Rows.Add(newrow);

            DataRow[] FW_SJ_ZLKK_DY = dsall.Tables["FW_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] HD_SJ_ZLKK_DY = dsall.Tables["HD_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] NJ_SJ_ZLKK_DY = dsall.Tables["NJ_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] JN_SJ_ZLKK_DY = dsall.Tables["JN_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] GZ_SJ_ZLKK_DY = dsall.Tables["GZ_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] CQ_SJ_ZLKK_DY = dsall.Tables["CQ_SJ"].Select("type='SJ_ZLKK'");

            DataRow[] AA_SJ_ZLKK_DY = dsall.Tables["AA_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] AH_SJ_ZLKK_DY = dsall.Tables["AH_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] P_SJ_ZLKK_DY = dsall.Tables["P_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] CM_SJ_ZLKK_DY = dsall.Tables["CM_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] KM_SJ_ZLKK_DY = dsall.Tables["KM_SJ"].Select("type='SJ_ZLKK'");
            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] =
                                +Convert.ToDouble(FW_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : FW_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(HD_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : HD_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(NJ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : NJ_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(JN_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : JN_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(GZ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : GZ_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(CQ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CQ_SJ_ZLKK_DY[0][i])

                            + Convert.ToDouble(AA_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AA_SJ_ZLKK_DY[0][i])
                            + Convert.ToDouble(AH_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AH_SJ_ZLKK_DY[0][i])
                            + Convert.ToDouble(P_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : P_SJ_ZLKK_DY[0][i])
                            + Convert.ToDouble(CM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CM_SJ_ZLKK_DY[0][i])
                            + Convert.ToDouble(KM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : KM_SJ_ZLKK_DY[0][i]);
            }
            newrow[13] = "SJ_ZLKK";
            dtTOT_SJ.Rows.Add(newrow);




            //ADD BY C1368 
            DataRow[] FW_SJ_DYHJ_DY = dsall.Tables["FW_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] HD_SJ_DYHJ_DY = dsall.Tables["HD_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] NJ_SJ_DYHJ_DY = dsall.Tables["NJ_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] JN_SJ_DYHJ_DY = dsall.Tables["JN_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] GZ_SJ_DYHJ_DY = dsall.Tables["GZ_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] CQ_SJ_DYHJ_DY = dsall.Tables["CQ_SJ"].Select("type='SJ_DYHJ'");

            DataRow[] AA_SJ_DYHJ_DY = dsall.Tables["AA_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] AH_SJ_DYHJ_DY = dsall.Tables["AH_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] P_SJ_DYHJ_DY = dsall.Tables["P_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] CM_SJ_DYHJ_DY = dsall.Tables["CM_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] KM_SJ_DYHJ_DY = dsall.Tables["KM_SJ"].Select("type='SJ_DYHJ'");
            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] =
                                +Convert.ToDouble(FW_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : FW_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(HD_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : HD_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(NJ_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : NJ_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(JN_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : JN_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(GZ_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : GZ_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(CQ_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : CQ_SJ_DYHJ_DY[0][i])

                            + Convert.ToDouble(AA_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DYHJ_DY[0][i])
                            + Convert.ToDouble(AH_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DYHJ_DY[0][i])
                            + Convert.ToDouble(P_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DYHJ_DY[0][i])
                            + Convert.ToDouble(CM_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DYHJ_DY[0][i])
                            + Convert.ToDouble(KM_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DYHJ_DY[0][i]);
            }
            newrow[13] = "SJ_DYHJ";
            dtTOT_SJ.Rows.Add(newrow);


            DataRow[] FW_SJ_LJ = dsall.Tables["FW_SJ"].Select("type='SJ_LJ'");
            DataRow[] HD_SJ_LJ = dsall.Tables["HD_SJ"].Select("type='SJ_LJ'");
            DataRow[] NJ_SJ_LJ = dsall.Tables["NJ_SJ"].Select("type='SJ_LJ'");
            DataRow[] JN_SJ_LJ = dsall.Tables["JN_SJ"].Select("type='SJ_LJ'");
            DataRow[] GZ_SJ_LJ = dsall.Tables["GZ_SJ"].Select("type='SJ_LJ'");
            DataRow[] CQ_SJ_LJ = dsall.Tables["CQ_SJ"].Select("type='SJ_LJ'");

            DataRow[] AA_SJ_LJ = dsall.Tables["AA_SJ"].Select("type='SJ_LJ'");
            DataRow[] AH_SJ_LJ = dsall.Tables["AH_SJ"].Select("type='SJ_LJ'");
            DataRow[] P_SJ_LJ = dsall.Tables["P_SJ"].Select("type='SJ_LJ'");
            DataRow[] CM_SJ_LJ = dsall.Tables["CM_SJ"].Select("type='SJ_LJ'");
            DataRow[] KM_SJ_LJ = dsall.Tables["KM_SJ"].Select("type='SJ_LJ'");
            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = Convert.ToDouble(FW_SJ_LJ[0][i] == DBNull.Value ? 0 : FW_SJ_LJ[0][i])
                            + Convert.ToDouble(HD_SJ_LJ[0][i] == DBNull.Value ? 0 : HD_SJ_LJ[0][i])
                            + Convert.ToDouble(NJ_SJ_LJ[0][i] == DBNull.Value ? 0 : NJ_SJ_LJ[0][i])
                            + Convert.ToDouble(JN_SJ_LJ[0][i] == DBNull.Value ? 0 : JN_SJ_LJ[0][i])
                            + Convert.ToDouble(GZ_SJ_LJ[0][i] == DBNull.Value ? 0 : GZ_SJ_LJ[0][i])
                            + Convert.ToDouble(CQ_SJ_LJ[0][i] == DBNull.Value ? 0 : CQ_SJ_LJ[0][i])

                            + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                            + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                            + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                            + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i])
                            + Convert.ToDouble(KM_SJ_LJ[0][i] == DBNull.Value ? 0 : KM_SJ_LJ[0][i]);
            }
            newrow[13] = "SJ_LJ";
            dtTOT_SJ.Rows.Add(newrow);
            ds.Tables.Add(dtTOT_SJ);
            #endregion

            #region 控制率
            DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
            ds.Tables.Add(dtTOT_KZL);
            #endregion



            return ds;
        }

        public DataSet GetResultHJ_OUT(DataSet dsall)
        {
            DataSet ds = new DataSet();

            #region 目标合计
            DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
            //DataRow[] FW_MB_DY = dsall.Tables["FW_MB"].Select("type='MB_DY'");
            DataRow[] HD_MB_DY = dsall.Tables["HD_MB"].Select("type='MB_DY'");
            DataRow[] NJ_MB_DY = dsall.Tables["NJ_MB"].Select("type='MB_DY'");
            DataRow[] GZ_MB_DY = dsall.Tables["GZ_MB"].Select("type='MB_DY'");
            DataRow[] JN_MB_DY = dsall.Tables["JN_MB"].Select("type='MB_DY'");
            DataRow[] CQ_MB_DY = dsall.Tables["CQ_MB"].Select("type='MB_DY'");

            DataRow[] AA_MB_DY = dsall.Tables["AA_MB"].Select("type='MB_DY'");
            DataRow[] AH_MB_DY = dsall.Tables["AH_MB"].Select("type='MB_DY'");
            DataRow[] P_MB_DY = dsall.Tables["P_MB"].Select("type='MB_DY'");
            DataRow[] CM_MB_DY = dsall.Tables["CM_MB"].Select("type='MB_DY'");
            DataRow[] KM_MB_DY = dsall.Tables["KM_MB"].Select("type='MB_DY'");
            DataRow newrow = dtTOT_MB.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = //Convert.ToDouble(FW_MB_DY[0][i] == DBNull.Value ? 0 : FW_MB_DY[0][i])
                             Convert.ToDouble(HD_MB_DY[0][i] == DBNull.Value ? 0 : HD_MB_DY[0][i])
                            + Convert.ToDouble(NJ_MB_DY[0][i] == DBNull.Value ? 0 : NJ_MB_DY[0][i])
                            + Convert.ToDouble(GZ_MB_DY[0][i] == DBNull.Value ? 0 : GZ_MB_DY[0][i])
                            + Convert.ToDouble(JN_MB_DY[0][i] == DBNull.Value ? 0 : JN_MB_DY[0][i])
                            + Convert.ToDouble(CQ_MB_DY[0][i] == DBNull.Value ? 0 : CQ_MB_DY[0][i])

                            + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                            + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                            + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                            + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i])
                            + Convert.ToDouble(KM_MB_DY[0][i] == DBNull.Value ? 0 : KM_MB_DY[0][i]);
            }
            newrow[13] = "MB_DY";
            dtTOT_MB.Rows.Add(newrow);

            //DataRow[] FW_MB_LJ = dsall.Tables["FW_MB"].Select("type='MB_LJ'");
            DataRow[] HD_MB_LJ = dsall.Tables["HD_MB"].Select("type='MB_LJ'");
            DataRow[] NJ_MB_LJ = dsall.Tables["NJ_MB"].Select("type='MB_LJ'");
            DataRow[] GZ_MB_LJ = dsall.Tables["GZ_MB"].Select("type='MB_LJ'");
            DataRow[] JN_MB_LJ = dsall.Tables["JN_MB"].Select("type='MB_LJ'");
            DataRow[] CQ_MB_LJ = dsall.Tables["CQ_MB"].Select("type='MB_LJ'");

            DataRow[] AA_MB_LJ = dsall.Tables["AA_MB"].Select("type='MB_LJ'");
            DataRow[] AH_MB_LJ = dsall.Tables["AH_MB"].Select("type='MB_LJ'");
            DataRow[] P_MB_LJ = dsall.Tables["P_MB"].Select("type='MB_LJ'");
            DataRow[] CM_MB_LJ = dsall.Tables["CM_MB"].Select("type='MB_LJ'");
            DataRow[] KM_MB_LJ = dsall.Tables["KM_MB"].Select("type='MB_LJ'");
            newrow = dtTOT_MB.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = //Convert.ToDouble(FW_MB_LJ[0][i] == DBNull.Value ? 0 : FW_MB_LJ[0][i])
                             Convert.ToDouble(HD_MB_LJ[0][i] == DBNull.Value ? 0 : HD_MB_LJ[0][i])
                            + Convert.ToDouble(NJ_MB_LJ[0][i] == DBNull.Value ? 0 : NJ_MB_LJ[0][i])
                            + Convert.ToDouble(GZ_MB_LJ[0][i] == DBNull.Value ? 0 : GZ_MB_LJ[0][i])
                            + Convert.ToDouble(JN_MB_LJ[0][i] == DBNull.Value ? 0 : JN_MB_LJ[0][i])
                            + Convert.ToDouble(CQ_MB_LJ[0][i] == DBNull.Value ? 0 : CQ_MB_LJ[0][i])

                            + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                            + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                            + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                            + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i])
                            + Convert.ToDouble(KM_MB_LJ[0][i] == DBNull.Value ? 0 : KM_MB_LJ[0][i]);
            }
            newrow[13] = "MB_LJ";
            dtTOT_MB.Rows.Add(newrow);
            ds.Tables.Add(dtTOT_MB);
            #endregion

            #region 实际合计
            DataTable dtTOT_SJ = BuildTB("TOT_SJ");
            //DataRow[] FW_SJ_DY = dsall.Tables["FW_SJ"].Select("type='SJ_DY'");
            DataRow[] HD_SJ_DY = dsall.Tables["HD_SJ"].Select("type='SJ_DY'");
            DataRow[] NJ_SJ_DY = dsall.Tables["NJ_SJ"].Select("type='SJ_DY'");
            DataRow[] GZ_SJ_DY = dsall.Tables["GZ_SJ"].Select("type='SJ_DY'");
            DataRow[] JN_SJ_DY = dsall.Tables["JN_SJ"].Select("type='SJ_DY'");
            DataRow[] CQ_SJ_DY = dsall.Tables["CQ_SJ"].Select("type='SJ_DY'");

            DataRow[] AA_SJ_DY = dsall.Tables["AA_SJ"].Select("type='SJ_DY'");
            DataRow[] AH_SJ_DY = dsall.Tables["AH_SJ"].Select("type='SJ_DY'");
            DataRow[] P_SJ_DY = dsall.Tables["P_SJ"].Select("type='SJ_DY'");
            DataRow[] CM_SJ_DY = dsall.Tables["CM_SJ"].Select("type='SJ_DY'");
            DataRow[] KM_SJ_DY = dsall.Tables["KM_SJ"].Select("type='SJ_DY'");//柯茂

            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = //Convert.ToDouble(FW_SJ_DY[0][i] == DBNull.Value ? 0 : FW_SJ_DY[0][i])
                             Convert.ToDouble(HD_SJ_DY[0][i] == DBNull.Value ? 0 : HD_SJ_DY[0][i])
                            + Convert.ToDouble(NJ_SJ_DY[0][i] == DBNull.Value ? 0 : NJ_SJ_DY[0][i])
                            + Convert.ToDouble(GZ_SJ_DY[0][i] == DBNull.Value ? 0 : GZ_SJ_DY[0][i])
                            + Convert.ToDouble(JN_SJ_DY[0][i] == DBNull.Value ? 0 : JN_SJ_DY[0][i])
                            + Convert.ToDouble(CQ_SJ_DY[0][i] == DBNull.Value ? 0 : CQ_SJ_DY[0][i])

                            + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                            + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                            + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                            + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i])
                            + Convert.ToDouble(KM_SJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DY[0][i]);
            }
            newrow[13] = "SJ_DY";
            dtTOT_SJ.Rows.Add(newrow);

            //DataRow[] FW_SJ_ZLKK_DY = dsall.Tables["FW_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] HD_SJ_ZLKK_DY = dsall.Tables["HD_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] NJ_SJ_ZLKK_DY = dsall.Tables["NJ_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] JN_SJ_ZLKK_DY = dsall.Tables["JN_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] GZ_SJ_ZLKK_DY = dsall.Tables["GZ_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] CQ_SJ_ZLKK_DY = dsall.Tables["CQ_SJ"].Select("type='SJ_ZLKK'");

            DataRow[] AA_SJ_ZLKK_DY = dsall.Tables["AA_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] AH_SJ_ZLKK_DY = dsall.Tables["AH_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] P_SJ_ZLKK_DY = dsall.Tables["P_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] CM_SJ_ZLKK_DY = dsall.Tables["CM_SJ"].Select("type='SJ_ZLKK'");
            DataRow[] KM_SJ_ZLKK_DY = dsall.Tables["KM_SJ"].Select("type='SJ_ZLKK'");
            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] =
                    //+Convert.ToDouble(FW_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : FW_SJ_ZLKK_DY[0][i])
                                 Convert.ToDouble(HD_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : HD_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(NJ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : NJ_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(JN_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : JN_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(GZ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : GZ_SJ_ZLKK_DY[0][i])
                                + Convert.ToDouble(CQ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CQ_SJ_ZLKK_DY[0][i])

                            + Convert.ToDouble(AA_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AA_SJ_ZLKK_DY[0][i])
                            + Convert.ToDouble(AH_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AH_SJ_ZLKK_DY[0][i])
                            + Convert.ToDouble(P_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : P_SJ_ZLKK_DY[0][i])
                            + Convert.ToDouble(CM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CM_SJ_ZLKK_DY[0][i])
                            + Convert.ToDouble(KM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : KM_SJ_ZLKK_DY[0][i]);
            }
            newrow[13] = "SJ_ZLKK";
            dtTOT_SJ.Rows.Add(newrow);


            //ADD BY C1368 
            //DataRow[] FW_SJ_DYHJ_DY = dsall.Tables["FW_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] HD_SJ_DYHJ_DY = dsall.Tables["HD_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] NJ_SJ_DYHJ_DY = dsall.Tables["NJ_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] JN_SJ_DYHJ_DY = dsall.Tables["JN_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] GZ_SJ_DYHJ_DY = dsall.Tables["GZ_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] CQ_SJ_DYHJ_DY = dsall.Tables["CQ_SJ"].Select("type='SJ_DYHJ'");

            DataRow[] AA_SJ_DYHJ_DY = dsall.Tables["AA_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] AH_SJ_DYHJ_DY = dsall.Tables["AH_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] P_SJ_DYHJ_DY = dsall.Tables["P_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] CM_SJ_DYHJ_DY = dsall.Tables["CM_SJ"].Select("type='SJ_DYHJ'");
            DataRow[] KM_SJ_DYHJ_DY = dsall.Tables["KM_SJ"].Select("type='SJ_DYHJ'");
            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] =
                                //Convert.ToDouble(FW_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : FW_SJ_DYHJ_DY[0][i])
                                Convert.ToDouble(HD_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : HD_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(NJ_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : NJ_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(JN_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : JN_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(GZ_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : GZ_SJ_DYHJ_DY[0][i])
                                + Convert.ToDouble(CQ_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : CQ_SJ_DYHJ_DY[0][i])

                            + Convert.ToDouble(AA_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DYHJ_DY[0][i])
                            + Convert.ToDouble(AH_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DYHJ_DY[0][i])
                            + Convert.ToDouble(P_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DYHJ_DY[0][i])
                            + Convert.ToDouble(CM_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DYHJ_DY[0][i])
                            + Convert.ToDouble(KM_SJ_DYHJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DYHJ_DY[0][i]);
            }
            newrow[13] = "SJ_DYHJ";
            dtTOT_SJ.Rows.Add(newrow);


            //DataRow[] FW_SJ_LJ = dsall.Tables["FW_SJ"].Select("type='SJ_LJ'");
            DataRow[] HD_SJ_LJ = dsall.Tables["HD_SJ"].Select("type='SJ_LJ'");
            DataRow[] NJ_SJ_LJ = dsall.Tables["NJ_SJ"].Select("type='SJ_LJ'");
            DataRow[] JN_SJ_LJ = dsall.Tables["JN_SJ"].Select("type='SJ_LJ'");
            DataRow[] GZ_SJ_LJ = dsall.Tables["GZ_SJ"].Select("type='SJ_LJ'");
            DataRow[] CQ_SJ_LJ = dsall.Tables["CQ_SJ"].Select("type='SJ_LJ'");

            DataRow[] AA_SJ_LJ = dsall.Tables["AA_SJ"].Select("type='SJ_LJ'");
            DataRow[] AH_SJ_LJ = dsall.Tables["AH_SJ"].Select("type='SJ_LJ'");
            DataRow[] P_SJ_LJ = dsall.Tables["P_SJ"].Select("type='SJ_LJ'");
            DataRow[] CM_SJ_LJ = dsall.Tables["CM_SJ"].Select("type='SJ_LJ'");
            DataRow[] KM_SJ_LJ = dsall.Tables["KM_SJ"].Select("type='SJ_LJ'");
            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = //Convert.ToDouble(FW_SJ_LJ[0][i] == DBNull.Value ? 0 : FW_SJ_LJ[0][i])
                             Convert.ToDouble(HD_SJ_LJ[0][i] == DBNull.Value ? 0 : HD_SJ_LJ[0][i])
                            + Convert.ToDouble(NJ_SJ_LJ[0][i] == DBNull.Value ? 0 : NJ_SJ_LJ[0][i])
                            + Convert.ToDouble(JN_SJ_LJ[0][i] == DBNull.Value ? 0 : JN_SJ_LJ[0][i])
                            + Convert.ToDouble(GZ_SJ_LJ[0][i] == DBNull.Value ? 0 : GZ_SJ_LJ[0][i])
                            + Convert.ToDouble(CQ_SJ_LJ[0][i] == DBNull.Value ? 0 : CQ_SJ_LJ[0][i])

                            + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                            + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                            + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                            + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i])
                            + Convert.ToDouble(KM_SJ_LJ[0][i] == DBNull.Value ? 0 : KM_SJ_LJ[0][i]);
            }
            newrow[13] = "SJ_LJ";
            dtTOT_SJ.Rows.Add(newrow);
            ds.Tables.Add(dtTOT_SJ);
            #endregion

            #region 控制率
            DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
            ds.Tables.Add(dtTOT_KZL);
            #endregion



            return ds;
        }


        public DataSet GetResultHJ_IN(DataSet dsall)
        {
            DataSet ds = new DataSet();

            #region 目标合计
            DataTable dtTOT_MB = this.BuildTB("TOT_MB");//合计
            DataRow[] FW_MB_DY = dsall.Tables["FW_MB"].Select("type='MB_DY'");
            //DataRow[] HD_MB_DY = dsall.Tables["HD_MB"].Select("type='MB_DY'");
            DataRow[] NJ_MB_DY = dsall.Tables["NJ_MB"].Select("type='MB_DY'");
            DataRow[] GZ_MB_DY = dsall.Tables["GZ_MB"].Select("type='MB_DY'");
            DataRow[] JN_MB_DY = dsall.Tables["JN_MB"].Select("type='MB_DY'");
            DataRow[] CQ_MB_DY = dsall.Tables["CQ_MB"].Select("type='MB_DY'");

            DataRow[] AA_MB_DY = dsall.Tables["AA_MB"].Select("type='MB_DY'");
            DataRow[] AH_MB_DY = dsall.Tables["AH_MB"].Select("type='MB_DY'");
            DataRow[] P_MB_DY = dsall.Tables["P_MB"].Select("type='MB_DY'");
            DataRow[] CM_MB_DY = dsall.Tables["CM_MB"].Select("type='MB_DY'");
            DataRow[] KM_MB_DY = dsall.Tables["KM_MB"].Select("type='MB_DY'");
            DataRow newrow = dtTOT_MB.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = Convert.ToDouble(FW_MB_DY[0][i] == DBNull.Value ? 0 : FW_MB_DY[0][i])
                    //+ Convert.ToDouble(HD_MB_DY[0][i] == DBNull.Value ? 0 : HD_MB_DY[0][i])
                            + Convert.ToDouble(NJ_MB_DY[0][i] == DBNull.Value ? 0 : NJ_MB_DY[0][i])
                            + Convert.ToDouble(GZ_MB_DY[0][i] == DBNull.Value ? 0 : GZ_MB_DY[0][i])
                            + Convert.ToDouble(JN_MB_DY[0][i] == DBNull.Value ? 0 : JN_MB_DY[0][i])
                            + Convert.ToDouble(CQ_MB_DY[0][i] == DBNull.Value ? 0 : CQ_MB_DY[0][i])

                            + Convert.ToDouble(AA_MB_DY[0][i] == DBNull.Value ? 0 : AA_MB_DY[0][i])
                            + Convert.ToDouble(AH_MB_DY[0][i] == DBNull.Value ? 0 : AH_MB_DY[0][i])
                            + Convert.ToDouble(P_MB_DY[0][i] == DBNull.Value ? 0 : P_MB_DY[0][i])
                            + Convert.ToDouble(CM_MB_DY[0][i] == DBNull.Value ? 0 : CM_MB_DY[0][i])
                             + Convert.ToDouble(KM_MB_DY[0][i] == DBNull.Value ? 0 : KM_MB_DY[0][i]);
            }
            newrow[13] = "MB_DY";
            dtTOT_MB.Rows.Add(newrow);

            DataRow[] FW_MB_LJ = dsall.Tables["FW_MB"].Select("type='MB_LJ'");
            // DataRow[] HD_MB_LJ = dsall.Tables["HD_MB"].Select("type='MB_LJ'");
            DataRow[] NJ_MB_LJ = dsall.Tables["NJ_MB"].Select("type='MB_LJ'");
            DataRow[] GZ_MB_LJ = dsall.Tables["GZ_MB"].Select("type='MB_LJ'");
            DataRow[] JN_MB_LJ = dsall.Tables["JN_MB"].Select("type='MB_LJ'");
            DataRow[] CQ_MB_LJ = dsall.Tables["CQ_MB"].Select("type='MB_LJ'");

            DataRow[] AA_MB_LJ = dsall.Tables["AA_MB"].Select("type='MB_LJ'");
            DataRow[] AH_MB_LJ = dsall.Tables["AH_MB"].Select("type='MB_LJ'");
            DataRow[] P_MB_LJ = dsall.Tables["P_MB"].Select("type='MB_LJ'");
            DataRow[] CM_MB_LJ = dsall.Tables["CM_MB"].Select("type='MB_LJ'");
            DataRow[] KM_MB_LJ = dsall.Tables["KM_MB"].Select("type='MB_LJ'");
            newrow = dtTOT_MB.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = Convert.ToDouble(FW_MB_LJ[0][i] == DBNull.Value ? 0 : FW_MB_LJ[0][i])
                    //+ Convert.ToDouble(HD_MB_LJ[0][i] == DBNull.Value ? 0 : HD_MB_LJ[0][i])
                            + Convert.ToDouble(NJ_MB_LJ[0][i] == DBNull.Value ? 0 : NJ_MB_LJ[0][i])
                            + Convert.ToDouble(GZ_MB_LJ[0][i] == DBNull.Value ? 0 : GZ_MB_LJ[0][i])
                            + Convert.ToDouble(JN_MB_LJ[0][i] == DBNull.Value ? 0 : JN_MB_LJ[0][i])
                            + Convert.ToDouble(CQ_MB_LJ[0][i] == DBNull.Value ? 0 : CQ_MB_LJ[0][i])

                            + Convert.ToDouble(AA_MB_LJ[0][i] == DBNull.Value ? 0 : AA_MB_LJ[0][i])
                            + Convert.ToDouble(AH_MB_LJ[0][i] == DBNull.Value ? 0 : AH_MB_LJ[0][i])
                            + Convert.ToDouble(P_MB_LJ[0][i] == DBNull.Value ? 0 : P_MB_LJ[0][i])
                            + Convert.ToDouble(CM_MB_LJ[0][i] == DBNull.Value ? 0 : CM_MB_LJ[0][i])
                            + Convert.ToDouble(KM_MB_LJ[0][i] == DBNull.Value ? 0 : KM_MB_LJ[0][i]);
            }
            newrow[13] = "MB_LJ";
            dtTOT_MB.Rows.Add(newrow);
            ds.Tables.Add(dtTOT_MB);
            #endregion

            #region 实际合计
            DataTable dtTOT_SJ = BuildTB("TOT_SJ");
            DataRow[] FW_SJ_DY = dsall.Tables["FW_SJ"].Select("type='SJ_DY'");
            // DataRow[] HD_SJ_DY = dsall.Tables["HD_SJ"].Select("type='SJ_DY'");
            DataRow[] NJ_SJ_DY = dsall.Tables["NJ_SJ"].Select("type='SJ_DY'");
            DataRow[] GZ_SJ_DY = dsall.Tables["GZ_SJ"].Select("type='SJ_DY'");
            DataRow[] JN_SJ_DY = dsall.Tables["JN_SJ"].Select("type='SJ_DY'");
            DataRow[] CQ_SJ_DY = dsall.Tables["CQ_SJ"].Select("type='SJ_DY'");

            DataRow[] AA_SJ_DY = dsall.Tables["AA_SJ"].Select("type='SJ_DY'");
            DataRow[] AH_SJ_DY = dsall.Tables["AH_SJ"].Select("type='SJ_DY'");
            DataRow[] P_SJ_DY = dsall.Tables["P_SJ"].Select("type='SJ_DY'");
            DataRow[] CM_SJ_DY = dsall.Tables["CM_SJ"].Select("type='SJ_DY'");
            DataRow[] KM_SJ_DY = dsall.Tables["KM_SJ"].Select("type='SJ_DY'");//柯茂

            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = Convert.ToDouble(FW_SJ_DY[0][i] == DBNull.Value ? 0 : FW_SJ_DY[0][i])
                    //+ Convert.ToDouble(HD_SJ_DY[0][i] == DBNull.Value ? 0 : HD_SJ_DY[0][i])
                            + Convert.ToDouble(NJ_SJ_DY[0][i] == DBNull.Value ? 0 : NJ_SJ_DY[0][i])
                            + Convert.ToDouble(GZ_SJ_DY[0][i] == DBNull.Value ? 0 : GZ_SJ_DY[0][i])
                            + Convert.ToDouble(JN_SJ_DY[0][i] == DBNull.Value ? 0 : JN_SJ_DY[0][i])
                            + Convert.ToDouble(CQ_SJ_DY[0][i] == DBNull.Value ? 0 : CQ_SJ_DY[0][i])

                            + Convert.ToDouble(AA_SJ_DY[0][i] == DBNull.Value ? 0 : AA_SJ_DY[0][i])
                            + Convert.ToDouble(AH_SJ_DY[0][i] == DBNull.Value ? 0 : AH_SJ_DY[0][i])
                            + Convert.ToDouble(P_SJ_DY[0][i] == DBNull.Value ? 0 : P_SJ_DY[0][i])
                            + Convert.ToDouble(CM_SJ_DY[0][i] == DBNull.Value ? 0 : CM_SJ_DY[0][i])
                            + Convert.ToDouble(KM_SJ_DY[0][i] == DBNull.Value ? 0 : KM_SJ_DY[0][i]);
            }
            newrow[13] = "SJ_DY";
            dtTOT_SJ.Rows.Add(newrow);

            //DataRow[] FW_SJ_ZLKK_DY = dsall.Tables["FW_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] HD_SJ_ZLKK_DY = dsall.Tables["HD_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] NJ_SJ_ZLKK_DY = dsall.Tables["NJ_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] JN_SJ_ZLKK_DY = dsall.Tables["JN_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] GZ_SJ_ZLKK_DY = dsall.Tables["GZ_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] CQ_SJ_ZLKK_DY = dsall.Tables["CQ_SJ"].Select("type='SJ_ZLKK'");

            //DataRow[] AA_SJ_ZLKK_DY = dsall.Tables["AA_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] AH_SJ_ZLKK_DY = dsall.Tables["AH_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] P_SJ_ZLKK_DY = dsall.Tables["P_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] CM_SJ_ZLKK_DY = dsall.Tables["CM_SJ"].Select("type='SJ_ZLKK'");
            //DataRow[] KM_SJ_ZLKK_DY = dsall.Tables["KM_SJ"].Select("type='SJ_ZLKK'");
            //newrow = dtTOT_SJ.NewRow();
            //for (int i = 0; i < 13; i++)
            //{
            //    newrow[i] =
            //                    +Convert.ToDouble(FW_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : FW_SJ_ZLKK_DY[0][i])
            //                    //+ Convert.ToDouble(HD_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : HD_SJ_ZLKK_DY[0][i])
            //                    + Convert.ToDouble(NJ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : NJ_SJ_ZLKK_DY[0][i])
            //                    + Convert.ToDouble(JN_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : JN_SJ_ZLKK_DY[0][i])
            //                    + Convert.ToDouble(GZ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : GZ_SJ_ZLKK_DY[0][i])
            //                    + Convert.ToDouble(CQ_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CQ_SJ_ZLKK_DY[0][i])

            //                + Convert.ToDouble(AA_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AA_SJ_ZLKK_DY[0][i])
            //                + Convert.ToDouble(AH_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : AH_SJ_ZLKK_DY[0][i])
            //                + Convert.ToDouble(P_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : P_SJ_ZLKK_DY[0][i])
            //                + Convert.ToDouble(CM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : CM_SJ_ZLKK_DY[0][i])
            //                + Convert.ToDouble(KM_SJ_ZLKK_DY[0][i] == DBNull.Value ? 0 : KM_SJ_ZLKK_DY[0][i]);
            //}
            //newrow[13] = "SJ_ZLKK";
            //dtTOT_SJ.Rows.Add(newrow);


            DataRow[] FW_SJ_LJ = dsall.Tables["FW_SJ"].Select("type='SJ_LJ'");
            //DataRow[] HD_SJ_LJ = dsall.Tables["HD_SJ"].Select("type='SJ_LJ'");
            DataRow[] NJ_SJ_LJ = dsall.Tables["NJ_SJ"].Select("type='SJ_LJ'");
            DataRow[] JN_SJ_LJ = dsall.Tables["JN_SJ"].Select("type='SJ_LJ'");
            DataRow[] GZ_SJ_LJ = dsall.Tables["GZ_SJ"].Select("type='SJ_LJ'");
            DataRow[] CQ_SJ_LJ = dsall.Tables["CQ_SJ"].Select("type='SJ_LJ'");

            DataRow[] AA_SJ_LJ = dsall.Tables["AA_SJ"].Select("type='SJ_LJ'");
            DataRow[] AH_SJ_LJ = dsall.Tables["AH_SJ"].Select("type='SJ_LJ'");
            DataRow[] P_SJ_LJ = dsall.Tables["P_SJ"].Select("type='SJ_LJ'");
            DataRow[] CM_SJ_LJ = dsall.Tables["CM_SJ"].Select("type='SJ_LJ'");
            DataRow[] KM_SJ_LJ = dsall.Tables["KM_SJ"].Select("type='SJ_LJ'");
            newrow = dtTOT_SJ.NewRow();
            for (int i = 0; i < 13; i++)
            {
                newrow[i] = Convert.ToDouble(FW_SJ_LJ[0][i] == DBNull.Value ? 0 : FW_SJ_LJ[0][i])
                    //+ Convert.ToDouble(HD_SJ_LJ[0][i] == DBNull.Value ? 0 : HD_SJ_LJ[0][i])
                            + Convert.ToDouble(NJ_SJ_LJ[0][i] == DBNull.Value ? 0 : NJ_SJ_LJ[0][i])
                            + Convert.ToDouble(JN_SJ_LJ[0][i] == DBNull.Value ? 0 : JN_SJ_LJ[0][i])
                            + Convert.ToDouble(GZ_SJ_LJ[0][i] == DBNull.Value ? 0 : GZ_SJ_LJ[0][i])
                            + Convert.ToDouble(CQ_SJ_LJ[0][i] == DBNull.Value ? 0 : CQ_SJ_LJ[0][i])

                            + Convert.ToDouble(AA_SJ_LJ[0][i] == DBNull.Value ? 0 : AA_SJ_LJ[0][i])
                            + Convert.ToDouble(AH_SJ_LJ[0][i] == DBNull.Value ? 0 : AH_SJ_LJ[0][i])
                            + Convert.ToDouble(P_SJ_LJ[0][i] == DBNull.Value ? 0 : P_SJ_LJ[0][i])
                            + Convert.ToDouble(CM_SJ_LJ[0][i] == DBNull.Value ? 0 : CM_SJ_LJ[0][i])
                            + Convert.ToDouble(KM_SJ_LJ[0][i] == DBNull.Value ? 0 : KM_SJ_LJ[0][i]);
            }
            newrow[13] = "SJ_LJ";
            dtTOT_SJ.Rows.Add(newrow);
            ds.Tables.Add(dtTOT_SJ);
            #endregion

            #region 控制率
            DataTable dtTOT_KZL = SetKZL("TOT_KZL", dtTOT_MB, dtTOT_SJ);
            ds.Tables.Add(dtTOT_KZL);
            #endregion



            return ds;
        }
        //-------------------------------------------------------------------------------------------

        #region 得到非R系其他产品别相关
        /// <summary>
        /// 
        /// </summary>
        /// <param name="protype"></param>
        /// <param name="year"></param>
        /// <param name="dtR_SJ"></param>
        /// <param name="dtbsc"></param>
        /// <param name="rowname"></param>
        /// <param name="type">0综合   1厂外  2场内</param>
        private void SetDB_SJ(string protype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname, int type)
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
            switch (type)
            {
                case 1:
                    for (int i = 0; i < 12; i++)
                    {
                        //mancost = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                        //wxll = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
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
                    break;
                case 2:
                    for (int i = 0; i < 12; i++)
                    {
                        mancost = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                        wxll = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
                        //rhdfwll = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                        //rhdtravelcost = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                        //rhdfare = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                        in1 = mancost + wxll + rhdfwll + rhdtravelcost + rhdfare;
                        total += in1;
                        newrow[i] = in1;
                    }
                    newrow[12] = total;
                    newrow[13] = rowname;
                    dtR_SJ.Rows.Add(newrow);
                    break;
                default:
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
                    break;
            }
        }
        /// <summary>
        /// 质量扣款
        /// </summary>
        private void SetDB_ZLKK(string protype, int year, DataTable dt_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dt_SJ.NewRow();
            decimal total = 0;
            decimal zlkk = 0;
            decimal in1 = 0;
            for (int i = 0; i < 12; i++)
            {
                zlkk = dtbsc.Select("protype='" + protype + "' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("zlkk"));
                in1 = zlkk;
                total += in1;
                newrow[i] = in1;
            }
            newrow[12] = total;
            newrow[13] = rowname;
            dt_SJ.Rows.Add(newrow);
        }
        public DataTable SetKZL(string tbname, DataTable MB, DataTable SJ)
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
                    if (a == 0 && b != 0)
                    {
                        newrow_dy[i] = 0.00;
                    }
                    else if (a != 0 && b == 0)
                    {
                        newrow_dy[i] = Math.Abs(decimal.Round(a / a * 100, 2));
                    }
                    else if (a == 0 && b == 0)
                    {
                        newrow_dy[i] = Math.Abs(decimal.Round(1 / 1 * 100, 2));
                    }
                    else
                    {
                        newrow_dy[i] = Math.Abs(decimal.Round(a / b * 100, 2));
                    }
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
                    if (a==0&&b==0)
                    {
                        newrow_lj[i] = 0;
                    }else
                    {
                        newrow_lj[i] = Math.Abs(decimal.Round(a / (b == 0 ? a : b) * 100, 2));
                    }
                }
                newrow_lj[13] = "LJ_KZL";
            }
            #endregion

            dtKZL.Rows.Add(newrow_dy);
            dtKZL.Rows.Add(newrow_lj);
            return dtKZL;
        }

        public DataTable SetKZL2(string tbname, DataTable MB, DataTable SJ)
        {
            DataTable dtKZL = BuildTB(tbname);

            #region DY_KZL 当月控制率
            DataRow newrow_dy = dtKZL.NewRow();//当月控制率
            DataRow[] mb_dy = MB.Select("type='MB_DY'");//目标当月
            DataRow[] sj_dy = SJ.Select("type='SJ_DY' or type='SJ_ZLKK'");//实际当月+实际质量扣款

            decimal a = 0;
            decimal b = 0;
            if (mb_dy.Length > 0 && sj_dy.Length > 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    a = Convert.ToDecimal(mb_dy[0][i] == DBNull.Value ? 0 : mb_dy[0][i]);
                    b = Convert.ToDecimal(sj_dy[0][i] == DBNull.Value ? 0 : sj_dy[0][i]) + Convert.ToDecimal(sj_dy[1][i] == DBNull.Value ? 0 : sj_dy[1][i]);
                    if (a == 0 && b != 0)
                    {
                        newrow_dy[i] = 0.00;
                    }
                    else if (a != 0 && b == 0)
                    {
                        newrow_dy[i] = Math.Abs(decimal.Round(a / a * 100, 2));
                    }
                    else if (a == 0 && b == 0)
                    {
                        newrow_dy[i] = Math.Abs(decimal.Round(1 / 1 * 100, 2));
                    }
                    else
                    {
                        newrow_dy[i] = Math.Abs(decimal.Round(a / b * 100, 2));
                    }
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

                    if (a == 0 && b == 0)
                    {
                        newrow_lj[i] = 100;
                    }
                    else
                    {
                        newrow_lj[i] = Math.Abs(decimal.Round(a / (b == 0 ? a : b) * 100, 2));
                    }
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
            SetDBLJByNum(dtAA_MB, 0, "MB_LJ");
            return dtAA_MB;
        }
        /// <summary>
        /// AA实际综合
        /// </summary>
        /// <param name="dtbsc"></param>
        /// <returns></returns>
        private DataTable SetAA_SJ(DataTable dtbsc)
        {
            DataTable dtAA_SJ = BuildTB("AA_SJ");
            SetDB_SJ("AA", RptDatetime.Year, dtAA_SJ, dtbsc, "SJ_DY", 0);
            SetDB_ZLKK("AA", RptDatetime.Year, dtAA_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBAddByNum(dtAA_SJ,new int[2]{0,1},"SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtAA_SJ, 0, 1, "SJ_LJ");
            return dtAA_SJ;
        }
        private DataTable SetAA_SJ2(DataTable dtbsc)
        {
            DataTable dtAA_SJ = BuildTB("AA_SJ");
            SetDB_SJ("AA", RptDatetime.Year, dtAA_SJ, dtbsc, "SJ_DY", 0);
            SetDB_ZLKK("AA", RptDatetime.Year, dtAA_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBAddByNum(dtAA_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtAA_SJ, 0, 1, "SJ_LJ");
            return dtAA_SJ;
        }

        private DataTable SetAA_SJ_IN(DataTable dtbsc)
        {
            DataTable dtAA_SJ = BuildTB("AA_SJ");
            SetDB_SJ("AA", RptDatetime.Year, dtAA_SJ, dtbsc, "SJ_DY", 0);
            SetDBLJByNum(dtAA_SJ, 0, "SJ_LJ");
            return dtAA_SJ;
        }
        private DataTable SetAA_SJ_OTHER(DataTable dtbsc)
        {
            DataTable dtAA_SJ = BuildTB("AA_SJ");
            SetDB_SJ("AA", RptDatetime.Year, dtAA_SJ, dtbsc, "SJ_DY", 0);
            //SetDB_ZLKK("AA", RptDatetime.Year, dtAA_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBLJByNum(dtAA_SJ, 0, "SJ_LJ");
            return dtAA_SJ;
        }
        private DataTable SetAA_KZL(DataTable AA_MB, DataTable AA_SJ)
        {
            DataTable dtKZL = this.SetKZL("AA_KZL", AA_MB, AA_SJ);
            return dtKZL;
        }
        private DataTable SetAA_KZL2(DataTable AA_MB, DataTable AA_SJ)
        {
            DataTable dtKZL = this.SetKZL2("AA_KZL", AA_MB, AA_SJ);
            return dtKZL;
        }
        #endregion

        #region AH
        private DataTable SetAH_MB()
        {
            DataTable dtAH_MB = BuildTB("AH_MB");
            MB_HELP(ReportName.Fw_AH, "MB_DY", dtAH_MB);
            SetDBLJByNum(dtAH_MB, 0, "MB_LJ");
            return dtAH_MB;
        }
        private DataTable SetAH_SJ(DataTable dtbsc)
        {
            DataTable dtAH_SJ = BuildTB("AH_SJ");
            SetDB_SJ("AH", RptDatetime.Year, dtAH_SJ, dtbsc, "SJ_DY", 0);
            SetDB_ZLKK("AH", RptDatetime.Year, dtAH_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBAddByNum(dtAH_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtAH_SJ, 0, 1, "SJ_LJ");
            return dtAH_SJ;
        }
        private DataTable SetAH_SJ_IN(DataTable dtbsc)
        {
            DataTable dtAH_SJ = BuildTB("AH_SJ");
            SetDB_SJ("AH", RptDatetime.Year, dtAH_SJ, dtbsc, "SJ_DY", 0);
            SetDBLJByNum(dtAH_SJ, 0, "SJ_LJ");
            return dtAH_SJ;
        }
        private DataTable SetAH_SJ_OTHER(DataTable dtbsc)
        {
            DataTable dtAH_SJ = BuildTB("AH_SJ");
            SetDB_SJ("AH", RptDatetime.Year, dtAH_SJ, dtbsc, "SJ_DY", 0);
            //SetDB_ZLKK("AH", RptDatetime.Year, dtAH_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBLJByNum(dtAH_SJ, 0, "SJ_LJ");
            return dtAH_SJ;
        }
        private DataTable SetAH_KZL(DataTable AH_MB, DataTable AH_SJ)
        {
            DataTable dtKZL = this.SetKZL("AH_KZL", AH_MB, AH_SJ);
            return dtKZL;
        }
        private DataTable SetAH_KZL2(DataTable AH_MB, DataTable AH_SJ)
        {
            DataTable dtKZL = this.SetKZL2("AH_KZL", AH_MB, AH_SJ);
            return dtKZL;
        }
        #endregion

        #region P
        private DataTable SetP_MB()
        {
            DataTable dtP_MB = BuildTB("P_MB");
            MB_HELP(ReportName.Fw_P, "MB_DY", dtP_MB);
            SetDBLJByNum(dtP_MB, 0, "MB_LJ");
            return dtP_MB;
        }
        private DataTable SetP_SJ(DataTable dtbsc)
        {
            DataTable dtP_SJ = BuildTB("P_SJ");
            SetDB_SJ("P", RptDatetime.Year, dtP_SJ, dtbsc, "SJ_DY", 0);
            SetDB_ZLKK("P", RptDatetime.Year, dtP_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBAddByNum(dtP_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtP_SJ, 0, 1, "SJ_LJ");
            return dtP_SJ;
        }
        private DataTable SetP_SJ_IN(DataTable dtbsc)
        {
            DataTable dtP_SJ = BuildTB("P_SJ");
            SetDB_SJ("P", RptDatetime.Year, dtP_SJ, dtbsc, "SJ_DY", 0);
            SetDBLJByNum(dtP_SJ, 0, "SJ_LJ");
            return dtP_SJ;
        }
        private DataTable SetP_SJ_OTHER(DataTable dtbsc)
        {
            DataTable dtP_SJ = BuildTB("P_SJ");
            SetDB_SJ("P", RptDatetime.Year, dtP_SJ, dtbsc, "SJ_DY", 0);
            //SetDB_ZLKK("P", RptDatetime.Year, dtP_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBLJByNum(dtP_SJ, 0, "SJ_LJ");
            return dtP_SJ;
        }
        private DataTable SetP_KZL(DataTable P_MB, DataTable P_SJ)
        {
            DataTable dtKZL = this.SetKZL("P_KZL", P_MB, P_SJ);
            return dtKZL;
        }
        private DataTable SetP_KZL2(DataTable P_MB, DataTable P_SJ)
        {
            DataTable dtKZL = this.SetKZL2("P_KZL", P_MB, P_SJ);
            return dtKZL;
        }
        #endregion

        #region CM
        private DataTable SetCM_MB()
        {
            DataTable dtCM_MB = BuildTB("CM_MB");
            MB_HELP(ReportName.Fw_CM, "MB_DY", dtCM_MB);
            SetDBLJByNum(dtCM_MB, 0, "MB_LJ");
            return dtCM_MB;
        }
        private DataTable SetCM_SJ(DataTable dtbsc)
        {
            DataTable dtCM_SJ = BuildTB("CM_SJ");
            SetDB_SJ("CM", RptDatetime.Year, dtCM_SJ, dtbsc, "SJ_DY", 0);
            SetDB_ZLKK("CM", RptDatetime.Year, dtCM_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBAddByNum(dtCM_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtCM_SJ, 0, 1, "SJ_LJ");
            return dtCM_SJ;
        }
        private DataTable SetCM_SJ_IN(DataTable dtbsc)
        {
            DataTable dtCM_SJ = BuildTB("CM_SJ");
            SetDB_SJ("CM", RptDatetime.Year, dtCM_SJ, dtbsc, "SJ_DY", 0);
            SetDBLJByNum(dtCM_SJ, 0, "SJ_LJ");
            return dtCM_SJ;
        }
        private DataTable SetCM_SJ_OTHER(DataTable dtbsc)
        {
            DataTable dtCM_SJ = BuildTB("CM_SJ");
            SetDB_SJ("CM", RptDatetime.Year, dtCM_SJ, dtbsc, "SJ_DY", 0);
            //SetDB_ZLKK("CM", RptDatetime.Year, dtCM_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBLJByNum(dtCM_SJ, 0, "SJ_LJ");
            return dtCM_SJ;
        }
        private DataTable SetCM_KZL(DataTable CM_MB, DataTable CM_SJ)
        {
            DataTable dtKZL = this.SetKZL("CM_KZL", CM_MB, CM_SJ);
            return dtKZL;
        }
        private DataTable SetCM_KZL2(DataTable CM_MB, DataTable CM_SJ)
        {
            DataTable dtKZL = this.SetKZL2("CM_KZL", CM_MB, CM_SJ);
            return dtKZL;
        }
        #endregion

        #region KM
        private DataTable SetKM_MB()
        {
            DataTable dtKM_MB = BuildTB("KM_MB");
            MB_HELP(ReportName.FW_KM, "MB_DY", dtKM_MB);
            SetDBLJByNum(dtKM_MB, 0, "MB_LJ");
            return dtKM_MB;
        }

        private DataTable SetKM_SJ(DataTable dtbsc)
        {
            DataTable dtKM_SJ = BuildTB("KM_SJ");
            SetDB_SJ("KM", RptDatetime.Year, dtKM_SJ, dtbsc, "SJ_DY", 0);
            SetDB_ZLKK("KM", RptDatetime.Year, dtKM_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBAddByNum(dtKM_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtKM_SJ, 0, 1, "SJ_LJ");
            return dtKM_SJ;
        }
        private DataTable SetKM_SJ_IN(DataTable dtbsc)
        {
            DataTable dtKM_SJ = BuildTB("KM_SJ");
            SetDB_SJ("KM", RptDatetime.Year, dtKM_SJ, dtbsc, "SJ_DY", 0);
            SetDBLJByNum(dtKM_SJ, 0, "SJ_LJ");
            return dtKM_SJ;
        }

        private DataTable SetKM_SJ_OTHER(DataTable dtbsc)
        {
            DataTable dtKM_SJ = BuildTB("KM_SJ");
            SetDB_SJ("KM", RptDatetime.Year, dtKM_SJ, dtbsc, "SJ_DY", 0);
            //SetDB_ZLKK("KM", RptDatetime.Year, dtKM_SJ, dtbsc, "SJ_ZLKK");//质量扣款
            SetDBLJByNum(dtKM_SJ, 0, "SJ_LJ");
            return dtKM_SJ;
        }
        private DataTable SetKM_KZL(DataTable KM_MB, DataTable KM_SJ)
        {
            DataTable dtKZL = this.SetKZL("KM_KZL", KM_MB, KM_SJ);
            return dtKZL;
        }
        private DataTable SetKM_KZL2(DataTable KM_MB, DataTable KM_SJ)
        {
            DataTable dtKZL = this.SetKZL2("KM_KZL", KM_MB, KM_SJ);
            return dtKZL;
        }

        #endregion



        #region 服务部
        /// <summary>
        /// 服务部目标
        /// </summary>
        /// <returns></returns>
        private DataTable SetFW_MB()
        {
            DataTable dtFW_MB = BuildTB("FW_MB");
            this.MB_HELP_FWB(ReportName.Fw_R_HD, "MB_DY", dtFW_MB);
            SetDBLJByNum(dtFW_MB, 0, "MB_LJ");//当月
            return dtFW_MB;
        }
        /// <summary>
        /// 服务部实际
        /// </summary>
        /// <returns></returns>
        private DataTable SetFW_SJ(DataTable dtbsc)
        {
            DataTable dtFW_SJ = BuildTB("FW_SJ");
            this.SetDB_R_SJ_FWB("R", "HD", RptDatetime.Year, dtFW_SJ, dtbsc, "SJ_DY");
            //this.SetDB_R_SJ_ZLKK("R", "HD", RptDatetime.Year, dtFW_SJ, dtbsc, "SJ_ZLKK");
            this.SetDB_FW_SJ_ZLKK("R", "HD", RptDatetime.Year, dtFW_SJ, dtbsc, "SJ_ZLKK");
            SetDBAddByNum(dtFW_SJ,new int[2]{0,1},"SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtFW_SJ, 0, 1, "SJ_LJ");
            return dtFW_SJ;
        }
        private DataTable SetFW_SJ_IN(DataTable dtbsc)
        {
            DataTable dtFW_SJ = BuildTB("FW_SJ");
            this.SetDB_R_SJ_FWB("R", "HD", RptDatetime.Year, dtFW_SJ, dtbsc, "SJ_DY");
            //this.SetDB_R_SJ_ZLKK("R", "HD", RptDatetime.Year, dtFW_SJ, dtbsc, "SJ_ZLKK");
            //this.SetDB_FW_SJ_ZLKK("R", "HD", RptDatetime.Year, dtFW_SJ, dtbsc, "SJ_ZLKK");
            SetDBLJByNum(dtFW_SJ, 0, "SJ_LJ");
            return dtFW_SJ;
        }
        /// <summary>
        /// 服务部控制率
        /// </summary>
        /// <returns></returns>
        private DataTable SetFW_KZL(DataTable mb, DataTable sj)
        {
            DataTable dtFW_KZL = SetKZL2("FW_KZL", mb, sj);
            return dtFW_KZL;
        }
        private DataTable SetFW_KZL_IN(DataTable mb, DataTable sj)
        {
            DataTable dtFW_KZL = SetKZL("FW_KZL", mb, sj);
            return dtFW_KZL;
        }
        #endregion

        #region 华东
        private DataTable SetHD_MB()
        {
            DataTable dtHD_MB = BuildTB("HD_MB");
            this.MB_HELP_HDBB(ReportName.Fw_R_HD, "MB_DY", dtHD_MB);//华东
            SetDBLJByNum(dtHD_MB, 0, "MB_LJ");//当月
            return dtHD_MB;
        }
        private DataTable SetHD_SJ(DataTable dtbsc)
        {
            DataTable dtHD_SJ = BuildTB("HD_SJ");
            this.SetDB_R_SJ_HDBB("R", "HD", RptDatetime.Year, dtHD_SJ, dtbsc, "SJ_DY");
            this.SetDB_HD_SJ_ZLKK("R", "HD", RptDatetime.Year, dtHD_SJ, dtbsc, "SJ_ZLKK");
            SetDBAddByNum(dtHD_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtHD_SJ, 0, 1, "SJ_LJ");
            return dtHD_SJ;
        }
        private DataTable SetHD_KZL(DataTable mb, DataTable sj)
        {
            DataTable dtHD_KZL = SetKZL2("HD_KZL", mb, sj);
            return dtHD_KZL;
        }
        #endregion

        #region 南京
        private DataTable SetNJ_MB()
        {
            DataTable dtNJ_MB = BuildTB("NJ_MB");
            this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            SetDBLJByNum(dtNJ_MB, 0, "MB_LJ");//当月
            return dtNJ_MB;
        }
        private DataTable SetNJ_SJ(DataTable dtbsc)
        {
            DataTable dtNJ_SJ = BuildTB("NJ_SJ");
            this.SetDB_R_SJ("R", "NJ", RptDatetime.Year, dtNJ_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "NJ", RptDatetime.Year, dtNJ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtNJ_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtNJ_SJ, 0, 1, "SJ_LJ");
            return dtNJ_SJ;
        }
        private DataTable SetNJ_MB_OUT()//厂外
        {
            DataTable dtNJ_MB = BuildTB("NJ_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_OUT_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);
            SetDBLJByNum(dtNJ_MB, 0, "MB_LJ");//当月
            return dtNJ_MB;
        }
        private DataTable SetNJ_SJ_OUT(DataTable dtbsc)//厂外
        {
            DataTable dtNJ_SJ = BuildTB("NJ_SJ");
            this.SetDB_R_SJ_OUT("R", "NJ", RptDatetime.Year, dtNJ_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "NJ", RptDatetime.Year, dtNJ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtNJ_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtNJ_SJ, 0, 1, "SJ_LJ");
            return dtNJ_SJ;
        }
        private DataTable SetNJ_MB_IN()//厂外
        {
            DataTable dtNJ_MB = BuildTB("NJ_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_IN_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);
            SetDBLJByNum(dtNJ_MB, 0, "MB_LJ");//当月
            return dtNJ_MB;
        }
        private DataTable SetNJ_SJ_IN(DataTable dtbsc)//厂外
        {
            DataTable dtNJ_SJ = BuildTB("NJ_SJ");
            this.SetDB_R_SJ_IN("R", "NJ", RptDatetime.Year, dtNJ_SJ, dtbsc, "SJ_DY");
            //this.SetDB_R_SJ_ZLKK("R", "NJ", RptDatetime.Year, dtNJ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBLJByNum(dtNJ_SJ, 0, "SJ_LJ");
            return dtNJ_SJ;
        }
        private DataTable SetNJ_KZL(DataTable mb, DataTable sj)
        {
            DataTable dtNJ_KZL = SetKZL2("NJ_KZL", mb, sj);
            return dtNJ_KZL;
        }
        private DataTable SetNJ_KZL_IN(DataTable mb, DataTable sj)
        {
            DataTable dtNJ_KZL = SetKZL("NJ_KZL", mb, sj);
            return dtNJ_KZL;
        }
        #endregion

        #region 济南
        private DataTable SetJN_MB()
        {
            DataTable dtJN_MB = BuildTB("JN_MB");
            this.MB_HELP(ReportName.Fw_R_HB, "MB_DY", dtJN_MB);//南京
            SetDBLJByNum(dtJN_MB, 0, "MB_LJ");//当月
            return dtJN_MB;
        }
        private DataTable SetJN_SJ(DataTable dtbsc)
        {
            DataTable dtJN_SJ = BuildTB("JN_SJ");
            this.SetDB_R_SJ("R", "HB", RptDatetime.Year, dtJN_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "HB", RptDatetime.Year, dtJN_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtJN_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtJN_SJ, 0, 1, "SJ_LJ");
            return dtJN_SJ;
        }
        private DataTable SetJN_MB_OUT()//厂外
        {
            DataTable dtJN_MB = BuildTB("JN_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_OUT_HELP(ReportName.Fw_R_HB, "MB_DY", dtJN_MB);
            SetDBLJByNum(dtJN_MB, 0, "MB_LJ");//当月
            return dtJN_MB;
        }
        private DataTable SetJN_SJ_OUT(DataTable dtbsc)//厂外
        {
            DataTable dtJN_SJ = BuildTB("JN_SJ");
            this.SetDB_R_SJ_OUT("R", "HB", RptDatetime.Year, dtJN_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "HB", RptDatetime.Year, dtJN_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtJN_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtJN_SJ, 0, 1, "SJ_LJ");
            return dtJN_SJ;
        }
        private DataTable SetJN_MB_IN()//厂外
        {
            DataTable dtJN_MB = BuildTB("JN_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_IN_HELP(ReportName.Fw_R_HB, "MB_DY", dtJN_MB);
            SetDBLJByNum(dtJN_MB, 0, "MB_LJ");//当月
            return dtJN_MB;
        }
        private DataTable SetJN_SJ_IN(DataTable dtbsc)//厂外
        {
            DataTable dtJN_SJ = BuildTB("JN_SJ");
            this.SetDB_R_SJ_IN("R", "HB", RptDatetime.Year, dtJN_SJ, dtbsc, "SJ_DY");
            //this.SetDB_R_SJ_ZLKK("R", "HB", RptDatetime.Year, dtJN_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBLJByNum(dtJN_SJ, 0, "SJ_LJ");
            return dtJN_SJ;
        }
        private DataTable SetJN_KZL(DataTable mb, DataTable sj)
        {
            DataTable dtJN_KZL = SetKZL2("JN_KZL", mb, sj);
            return dtJN_KZL;
        }
        private DataTable SetJN_KZL_IN(DataTable mb, DataTable sj)
        {
            DataTable dtJN_KZL = SetKZL("JN_KZL", mb, sj);
            return dtJN_KZL;
        }
        #endregion

        #region 广州
        /// <summary>
        /// 服务部目标
        /// </summary>
        /// <returns></returns>
        private DataTable SetGZ_MB()
        {
            DataTable dtGZ_MB = BuildTB("GZ_MB");
            this.MB_HELP(ReportName.Fw_R_HN, "MB_DY", dtGZ_MB);
            SetDBLJByNum(dtGZ_MB, 0, "MB_LJ");//当月
            return dtGZ_MB;
        }
        /// <summary>
        /// 服务部实际
        /// </summary>
        /// <returns></returns>
        private DataTable SetGZ_SJ(DataTable dtbsc)
        {
            DataTable dtGZ_SJ = BuildTB("GZ_SJ");
            this.SetDB_R_SJ("R", "HN", RptDatetime.Year, dtGZ_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "HN", RptDatetime.Year, dtGZ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtGZ_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtGZ_SJ, 0, 1, "SJ_LJ");
            return dtGZ_SJ;
        }
        private DataTable SetGZ_MB_OUT()//厂外
        {
            DataTable dtGZ_MB = BuildTB("GZ_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_OUT_HELP(ReportName.Fw_R_HN, "MB_DY", dtGZ_MB);
            SetDBLJByNum(dtGZ_MB, 0, "MB_LJ");//当月
            return dtGZ_MB;
        }
        private DataTable SetGZ_SJ_OUT(DataTable dtbsc)//厂外
        {
            DataTable dtGZ_SJ = BuildTB("GZ_SJ");
            this.SetDB_R_SJ_OUT("R", "HN", RptDatetime.Year, dtGZ_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "HN", RptDatetime.Year, dtGZ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtGZ_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtGZ_SJ, 0, 1, "SJ_LJ");
            return dtGZ_SJ;
        }
        private DataTable SetGZ_MB_IN()//厂外
        {
            DataTable dtGZ_MB = BuildTB("GZ_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_IN_HELP(ReportName.Fw_R_HN, "MB_DY", dtGZ_MB);
            SetDBLJByNum(dtGZ_MB, 0, "MB_LJ");//当月
            return dtGZ_MB;
        }
        private DataTable SetGZ_SJ_IN(DataTable dtbsc)//厂外
        {
            DataTable dtGZ_SJ = BuildTB("GZ_SJ");
            this.SetDB_R_SJ_IN("R", "HN", RptDatetime.Year, dtGZ_SJ, dtbsc, "SJ_DY");
            //this.SetDB_R_SJ_ZLKK("R", "HN", RptDatetime.Year, dtGZ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBLJByNum(dtGZ_SJ, 0, "SJ_LJ");
            return dtGZ_SJ;
        }
        /// <summary>
        /// 服务部控制率
        /// </summary>
        /// <returns></returns>
        private DataTable SetGZ_KZL(DataTable mb, DataTable sj)
        {
            DataTable dtGZ_KZL = SetKZL2("GZ_KZL", mb, sj);
            return dtGZ_KZL;
        }
        private DataTable SetGZ_KZL_IN(DataTable mb, DataTable sj)
        {
            DataTable dtGZ_KZL = SetKZL("GZ_KZL", mb, sj);
            return dtGZ_KZL;
        }
        #endregion

        #region 重庆
        private DataTable SetCQ_MB()
        {
            DataTable dtCQ_MB = BuildTB("CQ_MB");
            this.MB_HELP(ReportName.Fw_R_CQ, "MB_DY", dtCQ_MB);
            SetDBLJByNum(dtCQ_MB, 0, "MB_LJ");//当月
            return dtCQ_MB;
        }
        private DataTable SetCQ_SJ(DataTable dtbsc)
        {
            DataTable dtCQ_SJ = BuildTB("CQ_SJ");
            this.SetDB_R_SJ("R", "CQ", RptDatetime.Year, dtCQ_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "CQ", RptDatetime.Year, dtCQ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtCQ_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtCQ_SJ, 0, 1, "SJ_LJ");
            return dtCQ_SJ;
        }
        private DataTable SetCQ_MB_OUT()//厂外
        {
            DataTable dtCQ_MB = BuildTB("CQ_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_OUT_HELP(ReportName.Fw_R_CQ, "MB_DY", dtCQ_MB);
            SetDBLJByNum(dtCQ_MB, 0, "MB_LJ");//当月
            return dtCQ_MB;
        }
        private DataTable SetCQ_SJ_OUT(DataTable dtbsc)//厂外
        {
            DataTable dtCQ_SJ = BuildTB("CQ_SJ");
            this.SetDB_R_SJ_OUT("R", "CQ", RptDatetime.Year, dtCQ_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "CQ", RptDatetime.Year, dtCQ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtCQ_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtCQ_SJ, 0, 1, "SJ_LJ");
            return dtCQ_SJ;
        }
        private DataTable SetCQ_MB_IN()//厂外
        {
            DataTable dtCQ_MB = BuildTB("CQ_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_IN_HELP(ReportName.Fw_R_CQ, "MB_DY", dtCQ_MB);
            SetDBLJByNum(dtCQ_MB, 0, "MB_LJ");//当月
            return dtCQ_MB;
        }
        private DataTable SetCQ_SJ_IN(DataTable dtbsc)//厂外
        {
            DataTable dtCQ_SJ = BuildTB("CQ_SJ");
            this.SetDB_R_SJ_IN("R", "CQ", RptDatetime.Year, dtCQ_SJ, dtbsc, "SJ_DY");
            //this.SetDB_R_SJ_ZLKK("R", "CQ", RptDatetime.Year, dtCQ_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBLJByNum(dtCQ_SJ, 0, "SJ_LJ");
            return dtCQ_SJ;
        }
        private DataTable SetCQ_KZL(DataTable mb, DataTable sj)
        {
            DataTable dtCQ_KZL = SetKZL2("CQ_KZL", mb, sj);
            return dtCQ_KZL;
        }
        private DataTable SetCQ_KZL_IN(DataTable mb, DataTable sj)
        {
            DataTable dtCQ_KZL = SetKZL("CQ_KZL", mb, sj);
            return dtCQ_KZL;
        }
        #endregion


        #region 境外
        private DataTable SetJW_SJ(DataTable dtbsc)
        {
            DataTable dtJW_SJ = BuildTB("JW_SJ");
            this.SetDB_R_JW("R", "JW", RptDatetime.Year, dtJW_SJ, dtbsc, "SJ_R");
            this.SetDB_A_JW("A", "JW", RptDatetime.Year, dtJW_SJ, dtbsc, "SJ_A");
            this.SetDB_P_JW("P", "JW", RptDatetime.Year, dtJW_SJ, dtbsc, "SJ_P");
            this.SetDB_CM_JW("JWCM", "JW", RptDatetime.Year, dtJW_SJ, dtbsc, "SJ_CM");
            SetDBAddByNum(dtJW_SJ, new int[4] { 0, 1, 2, 3 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum(dtJW_SJ, 4, "SJ_LJ");
            return dtJW_SJ;
        }
        #endregion

        #region 制冷产品部
        private DataTable SetZL_MB()
        {
            DataTable dtZL_MB = BuildTB("ZL_MB");
            this.MB_HELP(ReportName.Fw_R_ZL, "MB_DY", dtZL_MB);//南京
            SetDBLJByNum(dtZL_MB, 0, "MB_LJ");//当月
            return dtZL_MB;
        }
        private DataTable SetZL_SJ(DataTable dtbsc)
        {
            DataTable dtZL_SJ = BuildTB("ZL_SJ");
            this.SetDB_R_SJ("R", "ZL", RptDatetime.Year, dtZL_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "ZL", RptDatetime.Year, dtZL_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtZL_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtZL_SJ, 0, 1, "SJ_LJ");
            return dtZL_SJ;
        }
        private DataTable SetZL_MB_OUT()//厂外
        {
            DataTable dtZL_MB = BuildTB("ZL_MB");
            //this.MB_HELP(ReportName.Fw_R_NJ, "MB_DY", dtNJ_MB);//南京
            R_MB_OUT_HELP(ReportName.Fw_R_ZL, "MB_DY", dtZL_MB);
            SetDBLJByNum(dtZL_MB, 0, "MB_LJ");//当月
            return dtZL_MB;
        }
        private DataTable SetZL_SJ_OUT(DataTable dtbsc)//厂外
        {
            DataTable dtZL_SJ = BuildTB("ZL_SJ");
            this.SetDB_R_SJ_OUT("R", "ZL", RptDatetime.Year, dtZL_SJ, dtbsc, "SJ_DY");
            this.SetDB_R_SJ_ZLKK("R", "NJ", RptDatetime.Year, dtZL_SJ, dtbsc, "SJ_ZLKK");//SJ_RNJ_ZLKK
            SetDBAddByNum(dtZL_SJ, new int[2] { 0, 1 }, "SJ_DYHJ");//当月合计=当月+质量扣款
            SetDBLJByNum2(dtZL_SJ, 0, 1, "SJ_LJ");
            return dtZL_SJ;
        }
        private DataTable SetZL_MB_IN()//厂外
        {
            DataTable dtZL_MB = BuildTB("ZL_MB");
            //this.MB_HELP(ReportName.Fw_R_ZL, "MB_DY", dtZL_MB);//南京
            R_MB_IN_HELP(ReportName.Fw_R_ZL, "MB_DY", dtZL_MB);
            SetDBLJByNum(dtZL_MB, 0, "MB_LJ");//当月
            return dtZL_MB;
        }
        private DataTable SetZL_SJ_IN(DataTable dtbsc)//厂外
        {
            DataTable dtZL_SJ = BuildTB("ZL_SJ");
            this.SetDB_R_SJ_IN("R", "ZL", RptDatetime.Year, dtZL_SJ, dtbsc, "SJ_DY");
            //this.SetDB_R_SJ_ZLKK("R", "ZL", RptDatetime.Year, dtZL_SJ, dtbsc, "SJ_ZLKK");//SJ_RZL_ZLKK
            SetDBLJByNum(dtZL_SJ, 0, "SJ_LJ");
            return dtZL_SJ;
        }
        private DataTable SetZL_KZL(DataTable mb, DataTable sj)
        {
            DataTable dtZL_KZL = SetKZL2("ZL_KZL", mb, sj);
            return dtZL_KZL;
        }
        private DataTable SetZL_KZL_IN(DataTable mb, DataTable sj)
        {
            DataTable dtZL_KZL = SetKZL("ZL_KZL", mb, sj);
            return dtZL_KZL;
        }
        #endregion

        public DataSet GetResultOther()
        {
            //string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            //dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            //connSybaseC = dbhC.GetConnection(ConstrC);
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
            DataTable dt_p_sj = SetP_SJ_OTHER(dt);
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
        public DataSet GetResultOther2()
        {
            //string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            //dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            //connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataSet ds = new DataSet();

            DataTable dt_aa_mb = SetAA_MB();
            DataTable dt_aa_sj = SetAA_SJ_OTHER(dt);
            DataTable dt_aa_kzl = SetAA_KZL(dt_aa_mb, dt_aa_sj);
            DataTable dt_ah_mb = SetAH_MB();
            DataTable dt_ah_sj = SetAH_SJ_OTHER(dt);
            DataTable dt_ah_kzl = SetAH_KZL(dt_ah_mb, dt_ah_sj);
            DataTable dt_p_mb = SetP_MB();
            DataTable dt_p_sj = SetP_SJ_OTHER(dt);
            DataTable dt_p_kzl = SetP_KZL(dt_p_mb, dt_p_sj);
            DataTable dt_cm_mb = SetCM_MB();
            DataTable dt_cm_sj = SetCM_SJ_OTHER(dt);
            DataTable dt_cm_kzl = SetCM_KZL(dt_cm_mb, dt_cm_sj);

            DataTable dt_km_mb = SetKM_MB();
            DataTable dt_km_sj = SetKM_SJ_OTHER(dt);
            DataTable dt_km_kzl = SetKM_KZL(dt_km_mb, dt_km_sj);

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

            ds.Tables.Add(dt_km_mb);
            ds.Tables.Add(dt_km_sj);
            ds.Tables.Add(dt_km_kzl);

            return ds;
        }
        public DataSet GetResultOther3()
        {
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataSet ds = new DataSet();

            DataTable dt_aa_mb = SetAA_MB();
            DataTable dt_aa_sj = SetAA_SJ(dt);
            DataTable dt_aa_kzl = SetAA_KZL2(dt_aa_mb, dt_aa_sj);
            DataTable dt_ah_mb = SetAH_MB();
            DataTable dt_ah_sj = SetAH_SJ(dt);
            DataTable dt_ah_kzl = SetAH_KZL2(dt_ah_mb, dt_ah_sj);
            DataTable dt_p_mb = SetP_MB();
            DataTable dt_p_sj = SetP_SJ(dt);
            DataTable dt_p_kzl = SetP_KZL2(dt_p_mb, dt_p_sj);
            DataTable dt_cm_mb = SetCM_MB();
            DataTable dt_cm_sj = SetCM_SJ(dt);
            DataTable dt_cm_kzl = SetCM_KZL2(dt_cm_mb, dt_cm_sj);

            DataTable dt_km_mb = SetKM_MB();
            DataTable dt_km_sj = SetKM_SJ(dt);
            DataTable dt_km_kzl = SetKM_KZL2(dt_km_mb, dt_km_sj);

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

            ds.Tables.Add(dt_km_mb);
            ds.Tables.Add(dt_km_sj);
            ds.Tables.Add(dt_km_kzl);

            return ds;
        }


        /// <summary>
        /// 综合查询新20151009
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllDataSource()
        {
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataSet ds = new DataSet();

            DataTable dt_fw_mb = SetFW_MB();
            DataTable dt_fw_sj = SetFW_SJ(dt);
            DataTable dt_fw_kzl = SetFW_KZL(dt_fw_mb, dt_fw_sj);

            DataTable dt_hd_mb = SetHD_MB();
            DataTable dt_hd_sj = SetHD_SJ(dt);
            DataTable dt_hd_kzl = SetHD_KZL(dt_hd_mb, dt_hd_sj);

            DataTable dt_zl_mb = SetZL_MB();
            DataTable dt_zl_sj = SetZL_SJ(dt);
            DataTable dt_zl_kzl = SetZL_KZL(dt_zl_mb, dt_zl_sj);

            DataTable dt_nj_mb = SetNJ_MB();
            DataTable dt_nj_sj = SetNJ_SJ(dt);
            DataTable dt_nj_kzl = SetNJ_KZL(dt_nj_mb, dt_nj_sj);

            DataTable dt_gz_mb = SetGZ_MB();
            DataTable dt_gz_sj = SetGZ_SJ(dt);
            DataTable dt_gz_kzl = SetGZ_KZL(dt_gz_mb, dt_gz_sj);

            DataTable dt_jn_mb = SetJN_MB();
            DataTable dt_jn_sj = SetJN_SJ(dt);
            DataTable dt_jn_kzl = SetJN_KZL(dt_jn_mb, dt_jn_sj);

            DataTable dt_cq_mb = SetCQ_MB();
            DataTable dt_cq_sj = SetCQ_SJ(dt);
            DataTable dt_cq_kzl = SetCQ_KZL(dt_cq_mb, dt_cq_sj);


            //ADD BY C1368 增加境外的数据
            DataTable dt_jw_sj = SetJW_SJ(dt);



            DataTable dt_aa_mb = SetAA_MB();
            DataTable dt_aa_sj = SetAA_SJ(dt);
            DataTable dt_aa_kzl = SetAA_KZL2(dt_aa_mb, dt_aa_sj);
            DataTable dt_ah_mb = SetAH_MB();
            DataTable dt_ah_sj = SetAH_SJ(dt);
            DataTable dt_ah_kzl = SetAH_KZL2(dt_ah_mb, dt_ah_sj);
            DataTable dt_p_mb = SetP_MB();
            DataTable dt_p_sj = SetP_SJ(dt);
            DataTable dt_p_kzl = SetP_KZL2(dt_p_mb, dt_p_sj);
            DataTable dt_cm_mb = SetCM_MB();
            DataTable dt_cm_sj = SetCM_SJ(dt);
            DataTable dt_cm_kzl = SetCM_KZL2(dt_cm_mb, dt_cm_sj);

            DataTable dt_km_mb = SetKM_MB();
            DataTable dt_km_sj = SetKM_SJ(dt);
            DataTable dt_km_kzl = SetKM_KZL2(dt_km_mb, dt_km_sj);

            ds.Tables.Add(dt_fw_mb);
            ds.Tables.Add(dt_fw_sj);
            ds.Tables.Add(dt_fw_kzl);
            ds.Tables.Add(dt_hd_mb);
            ds.Tables.Add(dt_hd_sj);
            ds.Tables.Add(dt_hd_kzl);

            ds.Tables.Add(dt_zl_mb);
            ds.Tables.Add(dt_zl_sj);
            ds.Tables.Add(dt_zl_kzl);

            ds.Tables.Add(dt_nj_mb);
            ds.Tables.Add(dt_nj_sj);
            ds.Tables.Add(dt_nj_kzl);
            ds.Tables.Add(dt_jn_mb);
            ds.Tables.Add(dt_jn_sj);
            ds.Tables.Add(dt_jn_kzl);

            ds.Tables.Add(dt_gz_mb);
            ds.Tables.Add(dt_gz_sj);
            ds.Tables.Add(dt_gz_kzl);

            ds.Tables.Add(dt_cq_mb);
            ds.Tables.Add(dt_cq_sj);
            ds.Tables.Add(dt_cq_kzl);

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

            ds.Tables.Add(dt_km_mb);
            ds.Tables.Add(dt_km_sj);
            ds.Tables.Add(dt_km_kzl);

            ds.Tables.Add(dt_jw_sj);


            return ds;

        }

        /// <summary>
        /// 综合查询新20151009
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllDataSourceOut()
        {
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataSet ds = new DataSet();

            //DataTable dt_fw_mb = SetFW_MB();
            //DataTable dt_fw_sj = SetFW_SJ(dt);
            //DataTable dt_fw_kzl = SetFW_KZL(dt_fw_mb, dt_fw_sj);

            DataTable dt_hd_mb = SetHD_MB();
            DataTable dt_hd_sj = SetHD_SJ(dt);
            DataTable dt_hd_kzl = SetHD_KZL(dt_hd_mb, dt_hd_sj);

            DataTable dt_nj_mb = SetNJ_MB_OUT();
            DataTable dt_nj_sj = SetNJ_SJ_OUT(dt);
            DataTable dt_nj_kzl = SetNJ_KZL(dt_nj_mb, dt_nj_sj);

            DataTable dt_gz_mb = SetGZ_MB_OUT();
            DataTable dt_gz_sj = SetGZ_SJ_OUT(dt);
            DataTable dt_gz_kzl = SetGZ_KZL(dt_gz_mb, dt_gz_sj);

            DataTable dt_jn_mb = SetJN_MB_OUT();
            DataTable dt_jn_sj = SetJN_SJ_OUT(dt);
            DataTable dt_jn_kzl = SetJN_KZL(dt_jn_mb, dt_jn_sj);

            DataTable dt_cq_mb = SetCQ_MB_OUT();
            DataTable dt_cq_sj = SetCQ_SJ_OUT(dt);
            DataTable dt_cq_kzl = SetCQ_KZL(dt_cq_mb, dt_cq_sj);

            DataTable dt_zl_mb = SetZL_MB_OUT();
            DataTable dt_zl_sj = SetZL_SJ_OUT(dt);
            DataTable dt_zl_kzl = SetZL_KZL(dt_nj_mb, dt_nj_sj);

            DataTable dt_aa_mb = SetAA_MB();
            DataTable dt_aa_sj = SetAA_SJ(dt);
            DataTable dt_aa_kzl = SetAA_KZL2(dt_aa_mb, dt_aa_sj);
            DataTable dt_ah_mb = SetAH_MB();
            DataTable dt_ah_sj = SetAH_SJ(dt);
            DataTable dt_ah_kzl = SetAH_KZL2(dt_ah_mb, dt_ah_sj);
            DataTable dt_p_mb = SetP_MB();
            DataTable dt_p_sj = SetP_SJ(dt);
            DataTable dt_p_kzl = SetP_KZL2(dt_p_mb, dt_p_sj);
            DataTable dt_cm_mb = SetCM_MB();
            DataTable dt_cm_sj = SetCM_SJ(dt);
            DataTable dt_cm_kzl = SetCM_KZL2(dt_cm_mb, dt_cm_sj);

            DataTable dt_km_mb = SetKM_MB();
            DataTable dt_km_sj = SetKM_SJ(dt);
            DataTable dt_km_kzl = SetKM_KZL2(dt_km_mb, dt_km_sj);

            ds.Tables.Add(dt_hd_mb);
            ds.Tables.Add(dt_hd_sj);
            ds.Tables.Add(dt_hd_kzl);

            ds.Tables.Add(dt_zl_mb);
            ds.Tables.Add(dt_zl_sj);
            ds.Tables.Add(dt_zl_kzl);

            ds.Tables.Add(dt_nj_mb);
            ds.Tables.Add(dt_nj_sj);
            ds.Tables.Add(dt_nj_kzl);
            ds.Tables.Add(dt_jn_mb);
            ds.Tables.Add(dt_jn_sj);
            ds.Tables.Add(dt_jn_kzl);

            ds.Tables.Add(dt_gz_mb);
            ds.Tables.Add(dt_gz_sj);
            ds.Tables.Add(dt_gz_kzl);



            ds.Tables.Add(dt_cq_mb);
            ds.Tables.Add(dt_cq_sj);
            ds.Tables.Add(dt_cq_kzl);

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

            ds.Tables.Add(dt_km_mb);
            ds.Tables.Add(dt_km_sj);
            ds.Tables.Add(dt_km_kzl);

            return ds;

        }

        /// <summary>
        /// 综合查询新20151009
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllDataSourceIN()
        {
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            DataSet ds = new DataSet();

            DataTable dt_fw_mb = SetFW_MB();
            DataTable dt_fw_sj = SetFW_SJ_IN(dt);
            DataTable dt_fw_kzl = SetFW_KZL_IN(dt_fw_mb, dt_fw_sj);

            //DataTable dt_hd_mb = SetHD_MB();
            //DataTable dt_hd_sj = SetHD_SJ(dt);
            //DataTable dt_hd_kzl = SetHD_KZL(dt_hd_mb, dt_hd_sj);

            DataTable dt_zl_mb = SetZL_MB_IN();
            DataTable dt_zl_sj = SetZL_SJ_IN(dt);
            DataTable dt_zl_kzl = SetZL_KZL_IN(dt_zl_mb, dt_zl_sj);

            DataTable dt_nj_mb = SetNJ_MB_IN();
            DataTable dt_nj_sj = SetNJ_SJ_IN(dt);
            DataTable dt_nj_kzl = SetNJ_KZL_IN(dt_nj_mb, dt_nj_sj);

            DataTable dt_gz_mb = SetGZ_MB_IN();
            DataTable dt_gz_sj = SetGZ_SJ_IN(dt);
            DataTable dt_gz_kzl = SetGZ_KZL_IN(dt_gz_mb, dt_gz_sj);

            DataTable dt_jn_mb = SetJN_MB_IN();
            DataTable dt_jn_sj = SetJN_SJ_IN(dt);
            DataTable dt_jn_kzl = SetJN_KZL_IN(dt_jn_mb, dt_jn_sj);

            DataTable dt_cq_mb = SetCQ_MB_IN();
            DataTable dt_cq_sj = SetCQ_SJ_IN(dt);
            DataTable dt_cq_kzl = SetCQ_KZL_IN(dt_cq_mb, dt_cq_sj);



            DataTable dt_aa_mb = SetAA_MB();
            DataTable dt_aa_sj = SetAA_SJ_IN(dt);
            DataTable dt_aa_kzl = SetAA_KZL(dt_aa_mb, dt_aa_sj);
            DataTable dt_ah_mb = SetAH_MB();
            DataTable dt_ah_sj = SetAH_SJ_IN(dt);
            DataTable dt_ah_kzl = SetAH_KZL(dt_ah_mb, dt_ah_sj);
            DataTable dt_p_mb = SetP_MB();
            DataTable dt_p_sj = SetP_SJ_IN(dt);
            DataTable dt_p_kzl = SetP_KZL(dt_p_mb, dt_p_sj);
            DataTable dt_cm_mb = SetCM_MB();
            DataTable dt_cm_sj = SetCM_SJ_IN(dt);
            DataTable dt_cm_kzl = SetCM_KZL(dt_cm_mb, dt_cm_sj);

            DataTable dt_km_mb = SetKM_MB();
            DataTable dt_km_sj = SetKM_SJ_IN(dt);
            DataTable dt_km_kzl = SetKM_KZL(dt_km_mb, dt_km_sj);

            ds.Tables.Add(dt_fw_mb);
            ds.Tables.Add(dt_fw_sj);
            ds.Tables.Add(dt_fw_kzl);

            ds.Tables.Add(dt_zl_mb);
            ds.Tables.Add(dt_zl_sj);
            ds.Tables.Add(dt_zl_kzl);

            ds.Tables.Add(dt_nj_mb);
            ds.Tables.Add(dt_nj_sj);
            ds.Tables.Add(dt_nj_kzl);
            ds.Tables.Add(dt_jn_mb);
            ds.Tables.Add(dt_jn_sj);
            ds.Tables.Add(dt_jn_kzl);

            ds.Tables.Add(dt_gz_mb);
            ds.Tables.Add(dt_gz_sj);
            ds.Tables.Add(dt_gz_kzl);

            ds.Tables.Add(dt_cq_mb);
            ds.Tables.Add(dt_cq_sj);
            ds.Tables.Add(dt_cq_kzl);

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

            ds.Tables.Add(dt_km_mb);
            ds.Tables.Add(dt_km_sj);
            ds.Tables.Add(dt_km_kzl);

            return ds;

        }
        #endregion


        //---------------------------------------------------------------------------------------------


    }
}
