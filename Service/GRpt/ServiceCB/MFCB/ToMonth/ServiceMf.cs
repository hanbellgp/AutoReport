using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Hanbell.BSC.Report;

namespace Hanbell.GRpt.ServiceCB.MFCB.ToMonth
{
    public class ServiceMf : BscReport
    {
        /// <summary>
        /// 报表日期
        /// </summary>
        public DateTime RptDatetime { get; set; }

        public override DataSet GetResult(string indexSn, DateTime dateNow)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(base.GetBscData(indexSn));
            return ds;
        }


        public DataSet GetResult()
        {
            System.Data.IDbConnection connSybaseC = null;
            Hanbell.DBUtility.IDbHelper dbhC = null;
            DataSet ds = new DataSet();
            DataTable dtmastermb = BuildTB("R_MB");//目标表
            DataTable dtmastersj = BuildTB("R_SJ");//实际表
            string sn = string.Empty;
            DataTable dtbos = null;



            #region R系 目标


            #region R_HD_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_HD);//服务免费成本R华东
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R华东" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtmastermb.NewRow();
                DataRow newrow2 = dtmastermb.NewRow();
                double total1 = 0;
                double total2 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrow1[13] = "MB_OUT_RHD";//行类型：MB_OUT_RHD =目标_外_R华东
                newrow2[13] = "MB_IN_RHD";//行类型：MB_IN_RHD =目标_内_R华东
                dtmastermb.Rows.Add(newrow1);
                dtmastermb.Rows.Add(newrow2);
            }
            #endregion

            #region R_NJ_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_NJ);//服务免费成本R南京
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R南京" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtmastermb.NewRow();
                DataRow newrow2 = dtmastermb.NewRow();
                double total1 = 0;
                double total2 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrow1[13] = "MB_OUT_RNJ";//行类型：MB_OUT_RHD =目标_外_R南京
                newrow2[13] = "MB_IN_RNJ";//行类型：MB_IN_RHD =目标_内_R南京
                dtmastermb.Rows.Add(newrow1);
                dtmastermb.Rows.Add(newrow2);
            }
            #endregion

            #region R_HB_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_HB);//服务免费成本R华北
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R华北" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtmastermb.NewRow();
                DataRow newrow2 = dtmastermb.NewRow();
                double total1 = 0;
                double total2 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrow1[13] = "MB_OUT_RHB";//行类型：MB_OUT_RHD =目标_外_R华北
                newrow2[13] = "MB_IN_RHB";//行类型：MB_IN_RHD =目标_内_R华北
                dtmastermb.Rows.Add(newrow1);
                dtmastermb.Rows.Add(newrow2);
            }
            #endregion

            #region R_HN_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_HN);//服务免费成本R华南
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R华南" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtmastermb.NewRow();
                DataRow newrow2 = dtmastermb.NewRow();
                double total1 = 0;
                double total2 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrow1[13] = "MB_OUT_RHN";//行类型：MB_OUT_RHD =目标_外_R华南
                newrow2[13] = "MB_IN_RHN";//行类型：MB_IN_RHD =目标_内_R华南
                dtmastermb.Rows.Add(newrow1);
                dtmastermb.Rows.Add(newrow2);
            }
            #endregion

            #region R_CQ_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_CQ);//服务免费成本R重庆
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R重庆" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtmastermb.NewRow();
                DataRow newrow2 = dtmastermb.NewRow();
                double total1 = 0;
                double total2 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrow1[13] = "MB_OUT_RCQ";//行类型：MB_OUT_RHD =目标_外_R重庆
                newrow2[13] = "MB_IN_RCQ";//行类型：MB_IN_RHD =目标_内_R重庆
                dtmastermb.Rows.Add(newrow1);
                dtmastermb.Rows.Add(newrow2);
            }
            #endregion

            #region 当月目标合计外
            DataRow[] mboutrows = dtmastermb.Select("type like 'MB_OUT%'");
            if (mboutrows.Length > 0)
            {
                DataRow newrowmbhjout = dtmastermb.NewRow();
                foreach (DataRow row in mboutrows)
                {
                    //newrow[0] = Convert.ToDouble(newrow[0]) + Convert.ToDouble(row[0]);
                    for (int i = 0; i <= 12; i++)
                    {
                        newrowmbhjout[i] = Convert.ToDouble(newrowmbhjout[i] == DBNull.Value ? 0 : newrowmbhjout[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);
                    }
                }
                newrowmbhjout[13] = "MB_OUT_HJ";//目标合计外
                dtmastermb.Rows.Add(newrowmbhjout);
            }
            #endregion

            #region 当月目标合计内
            DataRow[] mbintrows = dtmastermb.Select("type like 'MB_IN%'");
            if (mbintrows.Length > 0)
            {
                DataRow newrowmbhjin = dtmastermb.NewRow();
                foreach (DataRow row in mbintrows)
                {
                    //newrow[0] = Convert.ToDouble(newrow[0]) + Convert.ToDouble(row[0]);
                    for (int i = 0; i <= 12; i++)
                    {
                        newrowmbhjin[i] = Convert.ToDouble(newrowmbhjin[i] == DBNull.Value ? 0 : newrowmbhjin[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);
                    }
                }
                newrowmbhjin[13] = "MB_IN_HJ";//目标合计内
                dtmastermb.Rows.Add(newrowmbhjin);
            }
            #endregion

            #region 当月目标累计外
            DataRow[] mboutlj = dtmastermb.Select("type = 'MB_OUT_HJ'");//目标累计外
            if (mboutlj.Length > 0)
            {
                DataRow newrowmbljout = dtmastermb.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        newrowmbljout[i] = Convert.ToDouble(newrowmbljout[i] == DBNull.Value ? 0 : newrowmbljout[i]) + Convert.ToDouble(mboutlj[0][j] == DBNull.Value ? 0 : mboutlj[0][j]);
                    }
                }
                newrowmbljout[13] = "MB_OUT_LJ";
                dtmastermb.Rows.Add(newrowmbljout);
            }
            #endregion

            #region 当月目标累计内
            DataRow[] mbinlj = dtmastermb.Select("type = 'MB_IN_HJ'");//目标累计内
            if (mboutlj.Length > 0)
            {
                DataRow newrowmbljin = dtmastermb.NewRow();
                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        newrowmbljin[i] = Convert.ToDouble(newrowmbljin[i] == DBNull.Value ? 0 : newrowmbljin[i]) + Convert.ToDouble(mbinlj[0][j] == DBNull.Value ? 0 : mbinlj[0][j]);
                    }
                }
                newrowmbljin[13] = "MB_IN_LJ";
                dtmastermb.Rows.Add(newrowmbljin);
            }
            #endregion


            ds.Tables.Add(dtmastermb);//dataset增加table["R_MB"]

            #endregion

            #region R系 实际
            string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            if (dt.Rows.Count > 0)
            {
                //Bulid_R_HD_OUT(dt, dtmastersj, RptDatetime.Year);//R_华东_外
                //Bulid_R_HD_IN(dt, dtmastersj, RptDatetime.Year);//R_华东_内

                //Bulid_R_NJ_OUT(dt, dtmastersj, RptDatetime.Year);//R_南京_外
                //Bulid_R_NJ_IN(dt, dtmastersj, RptDatetime.Year);//R_南京_内

                //Bulid_R_HB_OUT(dt, dtmastersj, RptDatetime.Year);//R_华北_外
                //Bulid_R_HB_IN(dt, dtmastersj, RptDatetime.Year);//R_华北_内

                //Bulid_R_HN_OUT(dt, dtmastersj, RptDatetime.Year);//R_华南_外
                //Bulid_R_HN_IN(dt, dtmastersj, RptDatetime.Year);//R_华南_内

                //Bulid_R_CQ_OUT(dt, dtmastersj, RptDatetime.Year);//R_重庆_外
                //Bulid_R_CQ_IN(dt, dtmastersj, RptDatetime.Year);//R_重庆_内

                #region 当月实际合计外
                DataRow[] sjoutrows = dtmastersj.Select("type like 'SJ_OUT%'");
                if (sjoutrows.Length > 0)
                {
                    DataRow newrowsjhjout = dtmastersj.NewRow();
                    foreach (DataRow row in sjoutrows)
                    {
                        //newrow[0] = Convert.ToDouble(newrow[0]) + Convert.ToDouble(row[0]);
                        for (int i = 0; i <= 12; i++)
                        {
                            newrowsjhjout[i] = Convert.ToDouble(newrowsjhjout[i] == DBNull.Value ? 0 : newrowsjhjout[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);
                        }
                    }
                    newrowsjhjout[13] = "SJ_OUT_HJ";//目标合计外
                    dtmastersj.Rows.Add(newrowsjhjout);
                }
                #endregion

                #region 当月实际合计内
                DataRow[] sjinrows = dtmastersj.Select("type like 'SJ_IN%'");
                if (sjinrows.Length > 0)
                {
                    DataRow newrowsjhjin = dtmastersj.NewRow();
                    foreach (DataRow row in sjinrows)
                    {
                        //newrow[0] = Convert.ToDouble(newrow[0]) + Convert.ToDouble(row[0]);
                        for (int i = 0; i <= 12; i++)
                        {
                            newrowsjhjin[i] = Convert.ToDouble(newrowsjhjin[i] == DBNull.Value ? 0 : newrowsjhjin[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);
                        }
                    }
                    newrowsjhjin[13] = "SJ_IN_HJ";//实际合计内
                    dtmastersj.Rows.Add(newrowsjhjin);
                }
                #endregion

                #region 当月实际累计外
                DataRow[] sjoutljrows = dtmastersj.Select("type = 'SJ_OUT_HJ'");
                if (sjoutljrows.Length > 0)
                {
                    DataRow newrowsjljout = dtmastersj.NewRow();
                    for (int i = 0; i < 13; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            newrowsjljout[i] = Convert.ToDouble(newrowsjljout[i] == DBNull.Value ? 0 : newrowsjljout[i]) + Convert.ToDouble(sjoutljrows[0][j] == DBNull.Value ? 0 : sjoutljrows[0][j]);
                        }
                    }
                    newrowsjljout[13] = "SJ_OUT_LJ";
                    dtmastersj.Rows.Add(newrowsjljout);
                }
                #endregion

                #region 当月实际累计内
                DataRow[] sjinljrows = dtmastersj.Select("type = 'SJ_IN_HJ'");
                if (sjinljrows.Length > 0)
                {
                    DataRow newrowsjljin = dtmastersj.NewRow();
                    for (int i = 0; i < 13; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            newrowsjljin[i] = Convert.ToDouble(newrowsjljin[i] == DBNull.Value ? 0 : newrowsjljin[i]) + Convert.ToDouble(sjinljrows[0][j] == DBNull.Value ? 0 : sjinljrows[0][j]);
                        }
                    }
                    newrowsjljin[13] = "SJ_IN_LJ";
                    dtmastersj.Rows.Add(newrowsjljin);
                }
                #endregion                
            }

            #endregion

            #region 当月控制率

            DataTable dtR_KZL = BuildTB("R_KZL");
            //当月目标内外合计/当月实际内外合计×100%
            DataRow[] mbhjout = dtmastermb.Select("type='MB_OUT_HJ'");//目标合计外
            DataRow[] mbhjin = dtmastermb.Select("type='MB_IN_HJ'");//目标合计内

            DataRow[] sjhjout = dtmastersj.Select("type='SJ_OUT_HJ'");//实际合计外
            DataRow[] sjhjin = dtmastersj.Select("type='SJ_IN_HJ'");//实际合计内

            DataRow newrowkzlHJ = dtmastersj.NewRow();//控制率
            DataRow newrowkzlHJ2 = dtR_KZL.NewRow();

            decimal a = 0;
            decimal b = 0;
            for (int i = 0; i < 13; i++)
            {
                a = Convert.ToDecimal(mbhjout[0][i] == DBNull.Value ? 0 : mbhjout[0][i]) + Convert.ToDecimal(mbhjin[0][i] == DBNull.Value ? 0 : mbhjin[0][i]);
                b = Convert.ToDecimal(sjhjout[0][i] == DBNull.Value ? 0 : sjhjout[0][i]) + Convert.ToDecimal(sjhjin[0][i] == DBNull.Value ? 0 : sjhjin[0][i]);
                newrowkzlHJ[i] = decimal.Round(a * 100 / (b == 0 ? 1 : b), 2);
                newrowkzlHJ2[i] = decimal.Round(a * 100 / (b == 0 ? 1 : b), 2);
            }
            dtR_KZL.Rows.Add(newrowkzlHJ2);
            #endregion

            #region 累计控制率
            //当月目标内外累计/当月实际内外累计×100%
            DataRow[] mbljout = dtmastermb.Select("type='MB_OUT_LJ'");//目标合计外
            DataRow[] mbljin = dtmastermb.Select("type='MB_IN_LJ'");//目标合计内

            DataRow[] sjljout = dtmastersj.Select("type='SJ_OUT_LJ'");//实际合计外
            DataRow[] sjljin = dtmastersj.Select("type='SJ_IN_LJ'");//实际合计内

            DataRow newrowkzlLJ = dtmastersj.NewRow();//控制率
            DataRow newrowkzlLJ2 = dtR_KZL.NewRow();//控制率
            a = 0;
            b = 0;
            for (int i = 0; i < 13; i++)
            {
                a = Convert.ToDecimal(mbljout[0][i] == DBNull.Value ? 0 : mbljout[0][i]) + Convert.ToDecimal(mbljin[0][i] == DBNull.Value ? 0 : mbljin[0][i]);
                b = Convert.ToDecimal(sjljout[0][i] == DBNull.Value ? 0 : sjljout[0][i]) + Convert.ToDecimal(sjljin[0][i] == DBNull.Value ? 0 : sjljin[0][i]);
                newrowkzlLJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                newrowkzlLJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
            }
            dtR_KZL.Rows.Add(newrowkzlLJ2);
            #endregion

            ds.Tables.Add(dtR_KZL);//dataset增加table["R_KZL"]R控制率

            #region 非R系

            DataTable dtother = BuildTB("Other");//非R系


            DataTable tbAA_MB = this.BuildTB("AA_MB");//AA目标
            DataTable tbAA_SJ = this.BuildTB("AA_SJ");//AA实际
            DataTable tbAA_KZL = this.BuildTB("AA_KZL");//AA控制率

            DataTable tbAH_MB = this.BuildTB("AH_MB");//AH目标
            DataTable tbAH_SJ = this.BuildTB("AH_SJ");//AH实际
            DataTable tbAH_KZL = this.BuildTB("AH_KZL");//AH控制率

            DataTable tbP_MB = this.BuildTB("P_MB");//P目标
            DataTable tbP_SJ = this.BuildTB("P_SJ");//P实际
            DataTable tbP_KZL = this.BuildTB("P_KZL");//P控制率

            DataTable tbCM_MB = this.BuildTB("CM_MB");//AA目标
            DataTable tbCM_SJ = this.BuildTB("CM_SJ");//CM实际
            DataTable tbCM_KZL = this.BuildTB("CM_KZL");//CM控制率



            #region AA
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_AA);//服务免费成本AA
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本AA" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrowAA_MB_HJ = dtother.NewRow();
                DataRow newrowAA_MB_LJ = dtother.NewRow();
                DataRow newrowAA_MB_HJ2 = tbAA_MB.NewRow();
                DataRow newrowAA_MB_LJ2 = tbAA_MB.NewRow();
                double total1 = 0;
                double c = 0;
                double d = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    d = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowAA_MB_HJ[i] = c + d;//当月合计赋值
                    newrowAA_MB_HJ2[i] = c + d;
                    total1 += Convert.ToDouble(newrowAA_MB_HJ[i]);//合计累加
                    newrowAA_MB_LJ[i] = total1;//当月累计赋值
                    newrowAA_MB_LJ2[i] = total1;
                }
                newrowAA_MB_HJ[12] = total1;//合计总和
                newrowAA_MB_HJ[13] = "MB_AA_HJ";
                newrowAA_MB_LJ[13] = "MB_AA_LJ";
                newrowAA_MB_HJ2[12] = total1;//合计总和
                newrowAA_MB_HJ2[13] = "MB_AA_HJ";
                newrowAA_MB_LJ2[13] = "MB_AA_LJ";
                //AA目标加入
                dtother.Rows.Add(newrowAA_MB_HJ);
                dtother.Rows.Add(newrowAA_MB_LJ);//加入Other
                tbAA_MB.Rows.Add(newrowAA_MB_HJ2);//加入单个table
                tbAA_MB.Rows.Add(newrowAA_MB_LJ2);
                ds.Tables.Add(tbAA_MB);//ds加入
                //AA实际加入
                this.Bulid_AA_SJ(dt, dtother, tbAA_SJ, RptDatetime.Year);
                ds.Tables.Add(tbAA_SJ);

                //AA控制率
                DataRow newrowAA_KZL_HJ = dtother.NewRow();//控制率合计
                DataRow newrowAA_KZL_LJ = dtother.NewRow();//控制率累计
                DataRow newrowAA_KZL_HJ2 = tbAA_KZL.NewRow();//控制率合计
                DataRow newrowAA_KZL_LJ2 = tbAA_KZL.NewRow();//控制率累计
                for (int i = 0; i < 12; i++)
                {
                    a = dtother.Select("type='MB_AA_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AA_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowAA_KZL_HJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowAA_KZL_HJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                    a = dtother.Select("type='MB_AA_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AA_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowAA_KZL_LJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowAA_KZL_LJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                }
                newrowAA_KZL_HJ[13] = "AA_KZL_HJ";
                newrowAA_KZL_LJ[13] = "AA_KZL_LJ";
                newrowAA_KZL_HJ2[13] = "AA_KZL_HJ";
                newrowAA_KZL_LJ2[13] = "AA_KZL_LJ";
                dtother.Rows.Add(newrowAA_KZL_HJ);
                dtother.Rows.Add(newrowAA_KZL_LJ);

                tbAA_KZL.Rows.Add(newrowAA_KZL_HJ2);
                tbAA_KZL.Rows.Add(newrowAA_KZL_LJ2);
                ds.Tables.Add(tbAA_KZL);
            }
            #endregion

            #region AH
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_AH);//服务免费成本AH
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本AH" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrowAH_MB_HJ = dtother.NewRow();
                DataRow newrowAH_MB_LJ = dtother.NewRow();
                DataRow newrowAH_MB_HJ2 = tbAH_MB.NewRow();
                DataRow newrowAH_MB_LJ2 = tbAH_MB.NewRow();
                double total1 = 0;
                double c = 0;
                double d = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    d = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowAH_MB_HJ[i] = c + d;//当月合计赋值
                    newrowAH_MB_HJ2[i] = c + d;//当月合计赋值
                    total1 += Convert.ToDouble(newrowAH_MB_HJ[i]);//合计累加
                    newrowAH_MB_LJ[i] = total1;//当月累计赋值
                    newrowAH_MB_LJ2[i] = total1;//当月累计赋值
                }
                newrowAH_MB_HJ[12] = total1;//合计总和
                newrowAH_MB_HJ[13] = "MB_AH_HJ";
                newrowAH_MB_LJ[13] = "MB_AH_LJ";
                newrowAH_MB_HJ2[12] = total1;//合计总和
                newrowAH_MB_HJ2[13] = "MB_AH_HJ";
                newrowAH_MB_LJ2[13] = "MB_AH_LJ";
                //AH目标加入
                dtother.Rows.Add(newrowAH_MB_HJ);
                dtother.Rows.Add(newrowAH_MB_LJ);
                tbAH_MB.Rows.Add(newrowAH_MB_HJ2);
                tbAH_MB.Rows.Add(newrowAH_MB_LJ2);
                ds.Tables.Add(tbAH_MB);
                //AH实际加入
                this.Bulid_AH_SJ(dt, dtother,tbAH_SJ, RptDatetime.Year);
                ds.Tables.Add(tbAH_SJ);
                //AH控制率
                DataRow newrowAH_KZL_HJ = dtother.NewRow();//控制率合计
                DataRow newrowAH_KZL_LJ = dtother.NewRow();//控制率累计
                DataRow newrowAH_KZL_HJ2 = tbAH_KZL.NewRow();//控制率合计
                DataRow newrowAH_KZL_LJ2 = tbAH_KZL.NewRow();//控制率累计
                for (int i = 0; i < 12; i++)
                {
                    a = dtother.Select("type='MB_AH_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AH_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowAH_KZL_HJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowAH_KZL_HJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                    a = dtother.Select("type='MB_AH_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AH_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowAH_KZL_LJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowAH_KZL_LJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                }
                newrowAH_KZL_HJ[13] = "AH_KZL_HJ";
                newrowAH_KZL_LJ[13] = "AH_KZL_LJ";
                newrowAH_KZL_HJ2[13] = "AH_KZL_HJ";
                newrowAH_KZL_LJ2[13] = "AH_KZL_LJ";

                dtother.Rows.Add(newrowAH_KZL_HJ);
                dtother.Rows.Add(newrowAH_KZL_LJ);
                tbAH_KZL.Rows.Add(newrowAH_KZL_HJ2);
                tbAH_KZL.Rows.Add(newrowAH_KZL_LJ2);
                ds.Tables.Add(tbAH_KZL);
            }
            #endregion

            #region P
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_P);//服务免费成本P
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本P" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrowP_MB_HJ = dtother.NewRow();
                DataRow newrowP_MB_LJ = dtother.NewRow();
                DataRow newrowP_MB_HJ2 = tbP_MB.NewRow();
                DataRow newrowP_MB_LJ2 = tbP_MB.NewRow();
                double total1 = 0;
                double c = 0;
                double d = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    d = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowP_MB_HJ[i] = c + d;//当月合计赋值
                    newrowP_MB_HJ2[i] = c + d;//当月合计赋值
                    total1 += Convert.ToDouble(newrowP_MB_HJ[i]);//合计累加
                    newrowP_MB_LJ[i] = total1;//当月累计赋值
                    newrowP_MB_LJ2[i] = total1;//当月累计赋值
                }
                newrowP_MB_HJ[12] = total1;//合计总和
                newrowP_MB_HJ[13] = "MB_P_HJ";
                newrowP_MB_LJ[13] = "MB_P_LJ";
                newrowP_MB_HJ2[12] = total1;//合计总和
                newrowP_MB_HJ2[13] = "MB_P_HJ";
                newrowP_MB_LJ2[13] = "MB_P_LJ";
                //AH目标加入
                dtother.Rows.Add(newrowP_MB_HJ);
                dtother.Rows.Add(newrowP_MB_LJ);
                tbP_MB.Rows.Add(newrowP_MB_HJ2);
                tbP_MB.Rows.Add(newrowP_MB_LJ2);
                ds.Tables.Add(tbP_MB);
                //AH实际加入
                this.Bulid_P_SJ(dt, dtother, tbP_SJ, RptDatetime.Year);
                ds.Tables.Add(tbP_SJ);
                //P控制率
                DataRow newrowP_KZL_HJ = dtother.NewRow();//控制率合计
                DataRow newrowP_KZL_LJ = dtother.NewRow();//控制率累计
                DataRow newrowP_KZL_HJ2 = tbP_KZL.NewRow();//控制率合计
                DataRow newrowP_KZL_LJ2 = tbP_KZL.NewRow();//控制率累计
                for (int i = 0; i < 12; i++)
                {
                    a = dtother.Select("type='MB_P_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_P_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowP_KZL_HJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowP_KZL_HJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                    a = dtother.Select("type='MB_P_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_P_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowP_KZL_LJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowP_KZL_LJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                }
                newrowP_KZL_HJ[13] = "P_KZL_HJ";
                newrowP_KZL_LJ[13] = "P_KZL_LJ";
                newrowP_KZL_HJ2[13] = "P_KZL_HJ";
                newrowP_KZL_LJ2[13] = "P_KZL_LJ";

                dtother.Rows.Add(newrowP_KZL_HJ);
                dtother.Rows.Add(newrowP_KZL_LJ);
                tbP_KZL.Rows.Add(newrowP_KZL_HJ2);
                tbP_KZL.Rows.Add(newrowP_KZL_LJ2);
                ds.Tables.Add(tbP_KZL);
            }
            #endregion

            #region CM
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_CM);//服务免费成本CM
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本CM" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            //ds.Tables.Add(this.GetResult(sn, RptDatetime).Tables[0].Copy());
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrowCM_MB_HJ = dtother.NewRow();
                DataRow newrowCM_MB_LJ = dtother.NewRow();
                DataRow newrowCM_MB_HJ2 = tbCM_MB.NewRow();
                DataRow newrowCM_MB_LJ2 = tbCM_MB.NewRow();
                double total1 = 0;
                double c = 0;
                double d = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    d = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowCM_MB_HJ[i] = c + d;//当月合计赋值
                    newrowCM_MB_HJ2[i] = c + d;//当月合计赋值

                    total1 += Convert.ToDouble(newrowCM_MB_HJ[i]);//合计累加
                    newrowCM_MB_LJ[i] = total1;//当月累计赋值
                    newrowCM_MB_LJ2[i] = total1;//当月累计赋值
                }
                newrowCM_MB_HJ[12] = total1;//合计总和
                newrowCM_MB_HJ[13] = "MB_CM_HJ";
                newrowCM_MB_LJ[13] = "MB_CM_LJ";
                newrowCM_MB_HJ2[12] = total1;//合计总和
                newrowCM_MB_HJ2[13] = "MB_CM_HJ";
                newrowCM_MB_LJ2[13] = "MB_CM_LJ";
                //CM目标加入
                dtother.Rows.Add(newrowCM_MB_HJ);
                dtother.Rows.Add(newrowCM_MB_LJ);
                tbCM_MB.Rows.Add(newrowCM_MB_HJ2);
                tbCM_MB.Rows.Add(newrowCM_MB_LJ2);
                ds.Tables.Add(tbCM_MB);
                //CM实际加入
                this.Bulid_CM_SJ(dt, dtother, tbCM_SJ, RptDatetime.Year);
                ds.Tables.Add(tbCM_SJ);
                //CM控制率
                DataRow newrowCM_KZL_HJ = dtother.NewRow();//控制率合计
                DataRow newrowCM_KZL_LJ = dtother.NewRow();//控制率累计
                DataRow newrowCM_KZL_HJ2 = tbCM_KZL.NewRow();//控制率合计
                DataRow newrowCM_KZL_LJ2 = tbCM_KZL.NewRow();//控制率累计
                for (int i = 0; i < 12; i++)
                {
                    a = dtother.Select("type='MB_CM_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_CM_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowCM_KZL_HJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowCM_KZL_HJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                    a = dtother.Select("type='MB_CM_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AA_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowCM_KZL_LJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowCM_KZL_LJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                }
                newrowCM_KZL_HJ[13] = "CM_KZL_HJ";
                newrowCM_KZL_LJ[13] = "CM_KZL_LJ";
                newrowCM_KZL_HJ2[13] = "CM_KZL_HJ";
                newrowCM_KZL_LJ2[13] = "CM_KZL_LJ";

                dtother.Rows.Add(newrowCM_KZL_HJ);
                dtother.Rows.Add(newrowCM_KZL_LJ);
                tbCM_KZL.Rows.Add(newrowCM_KZL_HJ2);
                tbCM_KZL.Rows.Add(newrowCM_KZL_LJ2);
                ds.Tables.Add(tbCM_KZL);
            }
            #endregion

            #endregion

            ds.Tables.Add(dtother);//dataset增加table["Other"]



            return ds;
        }

        
        /// <summary>
        /// R_目标_外：R_MB_OUT
        /// R_目标_内：R_MB_IN
        /// </summary>
        /// <returns></returns>
        public DataSet GetResult2()
        {
            System.Data.IDbConnection connSybaseC = null;
            Hanbell.DBUtility.IDbHelper dbhC = null;
            DataSet ds = new DataSet();
            DataTable dtR_MB_IN = BuildTB("R_MB_IN");//目标表内
            DataTable dtR_MB_OUT = BuildTB("R_MB_OUT");//目标表外

            DataTable dtR_SJ_IN = BuildTB("R_SJ_IN");//实际表内
            DataTable dtR_SJ_OUT = BuildTB("R_SJ_OUT");//实际表外
            
            DataTable dtR_KZL_OUT = BuildTB("R_KZL_OUT");//控制率外
            DataTable dtR_KZL_IN = BuildTB("R_KZL_IN");//控制率内

            DataTable dtR_MB = BuildTB("R_MB");//合并外
            DataTable dtR_SJ = BuildTB("R_SJ");//合并内
            DataTable dtR_KZL = BuildTB("R_KZL");//合并控制率

            string sn = string.Empty;
            DataTable dtbos = null;



            #region R系 目标


            #region R_HD_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_HD);//服务免费成本R华东
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R华东" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtR_MB_OUT.NewRow();//R华东目标外
                DataRow newrow2 = dtR_MB_IN.NewRow();//R华东目标内

                DataRow newrowhdtot = dtR_MB.NewRow();//R华东目标合并

                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowhdtot[i] = dtbos.Select("dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//内外合并

                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                    total3 += Convert.ToDouble(newrowhdtot[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrowhdtot[12] = total3;
                newrow1[13] = "MB_OUT_RHD";//行类型：MB_OUT_RHD =目标_外_R华东
                newrow2[13] = "MB_IN_RHD";//行类型：MB_IN_RHD =目标_内_R华东
                newrowhdtot[13] = "MB_RHD";//目标华东
                dtR_MB_OUT.Rows.Add(newrow1);
                dtR_MB_IN.Rows.Add(newrow2);
                dtR_MB.Rows.Add(newrowhdtot);
            }
            #endregion

            #region R_NJ_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_NJ);//服务免费成本R南京
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R南京" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtR_MB_OUT.NewRow();
                DataRow newrow2 = dtR_MB_IN.NewRow();
                DataRow newrownjtot = dtR_MB.NewRow();//R华东目标合并
                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrownjtot[i] = dtbos.Select("dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                    total3 += Convert.ToDouble(newrownjtot[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrownjtot[12] = total3;
                newrow1[13] = "MB_OUT_RNJ";//行类型：MB_OUT_RHD =目标_外_R南京
                newrow2[13] = "MB_IN_RNJ";//行类型：MB_IN_RHD =目标_内_R南京
                newrownjtot[13] = "MB_RNJ";
                dtR_MB_OUT.Rows.Add(newrow1);
                dtR_MB_IN.Rows.Add(newrow2);
                dtR_MB.Rows.Add(newrownjtot);
            }
            #endregion

            #region R_HB_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_HB);//服务免费成本R华北
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R华北" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtR_MB_OUT.NewRow();
                DataRow newrow2 = dtR_MB_IN.NewRow();
                DataRow newrowhbtot = dtR_MB.NewRow();//R华北目标合并
                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowhbtot[i] = dtbos.Select("dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//内外合并
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                    total3 += Convert.ToDouble(newrowhbtot[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrowhbtot[12] = total3;
                newrow1[13] = "MB_OUT_RHB";//行类型：MB_OUT_RHD =目标_外_R华北
                newrow2[13] = "MB_IN_RHB";//行类型：MB_IN_RHD =目标_内_R华北
                newrowhbtot[13] = "MB_RHB";
                dtR_MB_OUT.Rows.Add(newrow1);
                dtR_MB_IN.Rows.Add(newrow2);
                dtR_MB.Rows.Add(newrowhbtot);
            }
            #endregion

            #region R_HN_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_HN);//服务免费成本R华南
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R华南" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtR_MB_OUT.NewRow();
                DataRow newrow2 = dtR_MB_IN.NewRow();
                DataRow newrowhntot = dtR_MB.NewRow();//R华南目标合并
                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowhntot[i] = dtbos.Select("dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//内外合并
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                    total3 += Convert.ToDouble(newrowhntot[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrowhntot[12] = total3;
                newrow1[13] = "MB_OUT_RHN";//行类型：MB_OUT_RHD =目标_外_R华南
                newrow2[13] = "MB_IN_RHN";//行类型：MB_IN_RHD =目标_内_R华南
                newrowhntot[13] = "MB_RHN";
                dtR_MB_OUT.Rows.Add(newrow1);
                dtR_MB_IN.Rows.Add(newrow2);
                dtR_MB.Rows.Add(newrowhntot);
            }
            #endregion

            #region R_CQ_外内
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_R_CQ);//服务免费成本R重庆
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本R重庆" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrow1 = dtR_MB_OUT.NewRow();
                DataRow newrow2 = dtR_MB_IN.NewRow();
                DataRow newrowcqtot = dtR_MB.NewRow();//R华南目标合并
                double total1 = 0;
                double total2 = 0;
                double total3 = 0;
                for (int i = 0; i < 12; i++)
                {
                    newrow1[i] = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    newrow2[i] = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowcqtot[i]=dtbos.Select("dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//内外合并
                    total1 += Convert.ToDouble(newrow1[i]);
                    total2 += Convert.ToDouble(newrow2[i]);
                    total3 += Convert.ToDouble(newrowcqtot[i]);
                }
                newrow1[12] = total1;
                newrow2[12] = total2;
                newrowcqtot[12] = total3;
                newrow1[13] = "MB_OUT_RCQ";//行类型：MB_OUT_RHD =目标_外_R重庆
                newrow2[13] = "MB_IN_RCQ";//行类型：MB_IN_RHD =目标_内_R重庆
                newrowcqtot[13] = "MB_RCQ";
                dtR_MB_OUT.Rows.Add(newrow1);
                dtR_MB_IN.Rows.Add(newrow2);
                dtR_MB.Rows.Add(newrowcqtot);
            }
            #endregion

            #region 当月目标合计外
            DataRow[] mboutrows = dtR_MB_OUT.Select("type like 'MB_OUT%'");
            DataRow newrowmbhj = dtR_MB.NewRow();//目标合计
            if (mboutrows.Length > 0)
            {
                DataRow newrowmbhjout = dtR_MB_OUT.NewRow();//目标合计外

                foreach (DataRow row in mboutrows)
                {
                    //newrow[0] = Convert.ToDouble(newrow[0]) + Convert.ToDouble(row[0]);
                    for (int i = 0; i <= 12; i++)
                    {
                        newrowmbhjout[i] = Convert.ToDouble(newrowmbhjout[i] == DBNull.Value ? 0 : newrowmbhjout[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);
                        newrowmbhj[i] = Convert.ToDouble(newrowmbhj[i] == DBNull.Value ? 0 : newrowmbhj[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);//目标合计
                    }
                }
                newrowmbhjout[13] = "MB_OUT_HJ";//目标合计外
                dtR_MB_OUT.Rows.Add(newrowmbhjout);
            }
            #endregion

            #region 当月目标合计内
            DataRow[] mbintrows = dtR_MB_IN.Select("type like 'MB_IN%'");
            if (mbintrows.Length > 0)
            {
                DataRow newrowmbhjin = dtR_MB_IN.NewRow();
                foreach (DataRow row in mbintrows)
                {
                    //newrow[0] = Convert.ToDouble(newrow[0]) + Convert.ToDouble(row[0]);
                    for (int i = 0; i <= 12; i++)
                    {
                        newrowmbhjin[i] = Convert.ToDouble(newrowmbhjin[i] == DBNull.Value ? 0 : newrowmbhjin[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);
                        newrowmbhj[i] = Convert.ToDouble(newrowmbhj[i] == DBNull.Value ? 0 : newrowmbhj[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);//目标合计
                    }
                }
                newrowmbhjin[13] = "MB_IN_HJ";//目标合计内
                dtR_MB_IN.Rows.Add(newrowmbhjin);
            }
            newrowmbhj[13] = "MB_HJ";//目标合计
            dtR_MB.Rows.Add(newrowmbhj);
            #endregion

            #region 当月目标累计外
            //DataRow[] mboutlj = dtR_MB_OUT.Select("type = 'MB_OUT_HJ'");//目标累计外
            //if (mboutlj.Length > 0)
            //{
            //    DataRow newrowmbljout = dtR_MB_OUT.NewRow();
            //    for (int i = 0; i < 13; i++)
            //    {
            //        for (int j = 0; j < i; j++)
            //        {
            //            newrowmbljout[i] = Convert.ToDouble(newrowmbljout[i] == DBNull.Value ? 0 : newrowmbljout[i]) + Convert.ToDouble(mboutlj[0][j] == DBNull.Value ? 0 : mboutlj[0][j]);
            //        }
            //    }
            //    newrowmbljout[13] = "MB_OUT_LJ";
            //    dtR_MB_OUT.Rows.Add(newrowmbljout);
            //}
            #endregion

            #region 当月目标累计内
            //DataRow[] mbinlj = dtR_MB_IN.Select("type = 'MB_IN_HJ'");//目标累计内
            //if (mboutlj.Length > 0)
            //{
            //    DataRow newrowmbljin = dtR_MB_IN.NewRow();
            //    for (int i = 0; i < 13; i++)
            //    {
            //        for (int j = 0; j < i; j++)
            //        {
            //            newrowmbljin[i] = Convert.ToDouble(newrowmbljin[i] == DBNull.Value ? 0 : newrowmbljin[i]) + Convert.ToDouble(mbinlj[0][j] == DBNull.Value ? 0 : mbinlj[0][j]);
            //        }
            //    }
            //    newrowmbljin[13] = "MB_IN_LJ";
            //    dtR_MB_IN.Rows.Add(newrowmbljin);
            //}
            #endregion

            #region 当月目标累计
            //目标累计=目标合计累计
            DataRow newrowmblj = dtR_MB.NewRow();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    newrowmblj[i] = Convert.ToDouble(newrowmblj[i] == DBNull.Value ? 0 : newrowmblj[i]) + Convert.ToDouble(newrowmbhj[j] == DBNull.Value ? 0 : newrowmbhj[j]);
                }
            }
            newrowmblj[13] = "MB_LJ";
            dtR_MB.Rows.Add(newrowmblj);
            #endregion

            ds.Tables.Add(dtR_MB_OUT);//dataset增加table["R_MB_OUT"]
            ds.Tables.Add(dtR_MB_IN);//dataset增加table["R_MB_IN"]
            ds.Tables.Add(dtR_MB);
            #endregion

            #region R系 实际
            string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);
            string sql = string.Format("select * from bsc_tb_mfservice where year={0}", RptDatetime.Year);
            DataTable dt = dbhC.Query(connSybaseC, sql);

            if (dt.Rows.Count > 0)
            {


                #region
                //Bulid_R_HD_OUT(dt, dtR_SJ_OUT, RptDatetime.Year);//R_华东_外
                //Bulid_R_HD_IN(dt, dtR_SJ_IN, RptDatetime.Year);//R_华东_内
                //Bulid_R_NJ_OUT(dt, dtR_SJ_OUT, RptDatetime.Year);//R_南京_外
                //Bulid_R_NJ_IN(dt, dtR_SJ_IN, RptDatetime.Year);//R_南京_内
                //Bulid_R_HB_OUT(dt, dtR_SJ_OUT, RptDatetime.Year);//R_华北_外
                //Bulid_R_HB_IN(dt, dtR_SJ_IN, RptDatetime.Year);//R_华北_内
                //Bulid_R_HN_OUT(dt, dtR_SJ_OUT, RptDatetime.Year);//R_华南_外
                //Bulid_R_HN_IN(dt, dtR_SJ_IN, RptDatetime.Year);//R_华南_内
                //Bulid_R_CQ_OUT(dt, dtR_SJ_OUT, RptDatetime.Year);//R_重庆_外
                //Bulid_R_CQ_IN(dt, dtR_SJ_IN, RptDatetime.Year);//R_重庆_内


                this.SetDB_R_SJ_OUT("R", "HD", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHD");
                this.SetDB_R_SJ_IN("R", "HD", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHD");
                this.SetDB_R_SJ_OUT("R", "NJ", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RNJ");
                this.SetDB_R_SJ_IN("R", "NJ", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RNJ");
                this.SetDB_R_SJ_OUT("R", "HB", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHB");
                this.SetDB_R_SJ_IN("R", "HB", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHB");
                this.SetDB_R_SJ_OUT("R", "HN", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RHN");
                this.SetDB_R_SJ_IN("R", "HN", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RHN");
                this.SetDB_R_SJ_OUT("R", "CQ", RptDatetime.Year, dtR_SJ_OUT, dt, "SJ_RCQ");
                this.SetDB_R_SJ_IN("R", "CQ", RptDatetime.Year, dtR_SJ_IN, dt, "SJ_RCQ");
                #endregion
                
                #region R实际华东
                this.SetDB_R_SJ("R", "HD", RptDatetime.Year, dtR_SJ, dt, "SJ_RHD");
                this.SetDB_R_SJ("R", "NJ", RptDatetime.Year, dtR_SJ, dt, "SJ_RNJ");
                this.SetDB_R_SJ("R", "HB", RptDatetime.Year, dtR_SJ, dt, "SJ_RHB");
                this.SetDB_R_SJ("R", "HN", RptDatetime.Year, dtR_SJ, dt, "SJ_RHN");
                this.SetDB_R_SJ("R", "CQ", RptDatetime.Year, dtR_SJ, dt, "SJ_RCQ");
                int[] num = new int[5] { 1, 2, 3, 4, 5 };//要相加行的数组
                SetDBAddByNum(dtR_SJ, num, "SJ_HJ");//实际合计
                SetDBLJByNum(dtR_SJ, 6, "SJ_LJ");//实际累计
                #endregion

                #region 当月实际合计外





                //DataRow[] sjoutrows = dtR_SJ_OUT.Select("type like 'SJ_OUT%'");
                //if (sjoutrows.Length > 0)
                //{
                //    DataRow newrowsjhjout = dtR_SJ_OUT.NewRow();
                //    foreach (DataRow row in sjoutrows)
                //    {
                //        //newrow[0] = Convert.ToDouble(newrow[0]) + Convert.ToDouble(row[0]);
                //        for (int i = 0; i <= 12; i++)
                //        {
                //            newrowsjhjout[i] = Convert.ToDouble(newrowsjhjout[i] == DBNull.Value ? 0 : newrowsjhjout[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);
                //        }
                //    }
                //    newrowsjhjout[13] = "SJ_OUT_HJ";//目标合计外
                //    dtR_SJ_OUT.Rows.Add(newrowsjhjout);
                //}
                //ds.Tables.Add(dtR_SJ_OUT);
                #endregion

                #region 当月实际合计内
                DataRow[] sjinrows = dtR_SJ_IN.Select("type like 'SJ_IN%'");
                if (sjinrows.Length > 0)
                {
                    DataRow newrowsjhjin = dtR_SJ_IN.NewRow();
                    foreach (DataRow row in sjinrows)
                    {
                        //newrow[0] = Convert.ToDouble(newrow[0]) + Convert.ToDouble(row[0]);
                        for (int i = 0; i <= 12; i++)
                        {
                            newrowsjhjin[i] = Convert.ToDouble(newrowsjhjin[i] == DBNull.Value ? 0 : newrowsjhjin[i]) + Convert.ToDouble(row[i] == DBNull.Value ? 0 : row[i]);
                        }
                    }
                    newrowsjhjin[13] = "SJ_IN_HJ";//实际合计内
                    dtR_SJ_IN.Rows.Add(newrowsjhjin);
                }
                ds.Tables.Add(dtR_SJ_IN);
                #endregion

                #region 当月实际累计外
                DataRow[] sjoutljrows = dtR_SJ_OUT.Select("type = 'SJ_OUT_HJ'");
                if (sjoutljrows.Length > 0)
                {
                    DataRow newrowsjljout = dtR_SJ_OUT.NewRow();
                    for (int i = 0; i < 13; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            newrowsjljout[i] = Convert.ToDouble(newrowsjljout[i] == DBNull.Value ? 0 : newrowsjljout[i]) + Convert.ToDouble(sjoutljrows[0][j] == DBNull.Value ? 0 : sjoutljrows[0][j]);
                        }
                    }
                    newrowsjljout[13] = "SJ_OUT_LJ";
                    dtR_SJ_OUT.Rows.Add(newrowsjljout);
                }
                ds.Tables.Add(dtR_SJ_OUT);
                #endregion

                #region 当月实际累计内
                DataRow[] sjinljrows = dtR_SJ_IN.Select("type = 'SJ_IN_HJ'");
                if (sjinljrows.Length > 0)
                {
                    DataRow newrowsjljin = dtR_SJ_IN.NewRow();
                    for (int i = 0; i < 13; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            newrowsjljin[i] = Convert.ToDouble(newrowsjljin[i] == DBNull.Value ? 0 : newrowsjljin[i]) + Convert.ToDouble(sjinljrows[0][j] == DBNull.Value ? 0 : sjinljrows[0][j]);
                        }
                    }
                    newrowsjljin[13] = "SJ_IN_LJ";
                    dtR_SJ_IN.Rows.Add(newrowsjljin);
                }
                ds.Tables.Add(dtR_SJ_IN);
                #endregion
            }

            #endregion

            #region 当月控制率

            //当月目标内外合计/当月实际内外合计×100%
            DataRow[] mbhjout = dtR_MB_OUT.Select("type='MB_OUT_HJ'");//目标合计外
            DataRow[] mbhjin = dtR_MB_IN.Select("type='MB_IN_HJ'");//目标合计内

            DataRow[] sjhjout = dtR_SJ_OUT.Select("type='SJ_OUT_HJ'");//实际合计外
            DataRow[] sjhjin = dtR_SJ_IN.Select("type='SJ_IN_HJ'");//实际合计内

            DataRow newrowkzlHJ2 = dtR_KZL_OUT.NewRow();//外
            DataRow newrowkzlHJ = dtR_KZL_IN.NewRow();//内

            decimal a = 0;
            decimal b = 0;
            for (int i = 0; i < 13; i++)
            {
                a = Convert.ToDecimal(mbhjout[0][i] == DBNull.Value ? 0 : mbhjout[0][i]);
                b = Convert.ToDecimal(sjhjout[0][i] == DBNull.Value ? 0 : sjhjout[0][i]);
                newrowkzlHJ2[i] = decimal.Round(a * 100 / (b == 0 ? 1 : b), 2);

                a = Convert.ToDecimal(mbhjin[0][i] == DBNull.Value ? 0 : mbhjin[0][i]);
                b = Convert.ToDecimal(sjhjin[0][i] == DBNull.Value ? 0 : sjhjin[0][i]);
                newrowkzlHJ[i] = decimal.Round(a * 100 / (b == 0 ? 1 : b), 2);
            }
            dtR_KZL_OUT.Rows.Add(newrowkzlHJ2);
            dtR_KZL_IN.Rows.Add(newrowkzlHJ);
            #endregion

            #region 累计控制率
            //当月目标内外累计/当月实际内外累计×100%
            DataRow[] mbljout = dtR_MB_OUT.Select("type='MB_OUT_LJ'");//目标累计外
            DataRow[] mbljin = dtR_MB_IN.Select("type='MB_IN_LJ'");//目标累计内

            DataRow[] sjljout = dtR_SJ_OUT.Select("type='SJ_OUT_LJ'");//实际累计外
            DataRow[] sjljin = dtR_SJ_IN.Select("type='SJ_IN_LJ'");//实际累计内

            DataRow newrowkzlLJ = dtR_KZL_OUT.NewRow();//累计控制率外
            DataRow newrowkzlLJ2 = dtR_KZL_IN.NewRow();//累计控制率内
            a = 0;
            b = 0;
            for (int i = 0; i < 13; i++)
            {
                a = Convert.ToDecimal(mbljout[0][i] == DBNull.Value ? 0 : mbljout[0][i]);
                b = Convert.ToDecimal(sjljout[0][i] == DBNull.Value ? 0 : sjljout[0][i]);
                
                newrowkzlLJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                a = Convert.ToDecimal(mbljin[0][i] == DBNull.Value ? 0 : mbljin[0][i]);
                b = Convert.ToDecimal(sjljin[0][i] == DBNull.Value ? 0 : sjljin[0][i]);
                newrowkzlLJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
            }
            //dtR_KZL.Rows.Add(newrowkzlLJ2);
            dtR_KZL_OUT.Rows.Add(newrowkzlLJ2);
            dtR_KZL_IN.Rows.Add(newrowkzlLJ);
            #endregion

            ds.Tables.Add(dtR_KZL_OUT);//R当月控制率外
            ds.Tables.Add(dtR_KZL_IN);//R当月控制率内
            //ds.Tables.Add(dtR_KZL);//dataset增加table["R_KZL"]R控制率

            #region 非R系

            DataTable dtother = BuildTB("Other");//非R系


            DataTable tbAA_MB = this.BuildTB("AA_MB");//AA目标
            DataTable tbAA_SJ = this.BuildTB("AA_SJ");//AA实际
            DataTable tbAA_KZL = this.BuildTB("AA_KZL");//AA控制率

            DataTable tbAH_MB = this.BuildTB("AH_MB");//AH目标
            DataTable tbAH_SJ = this.BuildTB("AH_SJ");//AH实际
            DataTable tbAH_KZL = this.BuildTB("AH_KZL");//AH控制率

            DataTable tbP_MB = this.BuildTB("P_MB");//P目标
            DataTable tbP_SJ = this.BuildTB("P_SJ");//P实际
            DataTable tbP_KZL = this.BuildTB("P_KZL");//P控制率

            DataTable tbCM_MB = this.BuildTB("CM_MB");//AA目标
            DataTable tbCM_SJ = this.BuildTB("CM_SJ");//CM实际
            DataTable tbCM_KZL = this.BuildTB("CM_KZL");//CM控制率



            #region AA
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_AA);//服务免费成本AA
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本AA" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrowAA_MB_HJ = dtother.NewRow();
                DataRow newrowAA_MB_LJ = dtother.NewRow();
                DataRow newrowAA_MB_HJ2 = tbAA_MB.NewRow();
                DataRow newrowAA_MB_LJ2 = tbAA_MB.NewRow();
                double total1 = 0;
                double c = 0;
                double d = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    d = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowAA_MB_HJ[i] = c + d;//当月合计赋值
                    newrowAA_MB_HJ2[i] = c + d;
                    total1 += Convert.ToDouble(newrowAA_MB_HJ[i]);//合计累加
                    newrowAA_MB_LJ[i] = total1;//当月累计赋值
                    newrowAA_MB_LJ2[i] = total1;
                }
                newrowAA_MB_HJ[12] = total1;//合计总和
                newrowAA_MB_HJ[13] = "MB_AA_HJ";
                newrowAA_MB_LJ[13] = "MB_AA_LJ";
                newrowAA_MB_HJ2[12] = total1;//合计总和
                newrowAA_MB_HJ2[13] = "MB_AA_HJ";
                newrowAA_MB_LJ2[13] = "MB_AA_LJ";
                //AA目标加入
                dtother.Rows.Add(newrowAA_MB_HJ);
                dtother.Rows.Add(newrowAA_MB_LJ);//加入Other
                tbAA_MB.Rows.Add(newrowAA_MB_HJ2);//加入单个table
                tbAA_MB.Rows.Add(newrowAA_MB_LJ2);
                ds.Tables.Add(tbAA_MB);//ds加入
                //AA实际加入
                this.Bulid_AA_SJ(dt, dtother, tbAA_SJ, RptDatetime.Year);
                ds.Tables.Add(tbAA_SJ);

                //AA控制率
                DataRow newrowAA_KZL_HJ = dtother.NewRow();//控制率合计
                DataRow newrowAA_KZL_LJ = dtother.NewRow();//控制率累计
                DataRow newrowAA_KZL_HJ2 = tbAA_KZL.NewRow();//控制率合计
                DataRow newrowAA_KZL_LJ2 = tbAA_KZL.NewRow();//控制率累计
                for (int i = 0; i < 12; i++)
                {
                    a = dtother.Select("type='MB_AA_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AA_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowAA_KZL_HJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowAA_KZL_HJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                    a = dtother.Select("type='MB_AA_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AA_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowAA_KZL_LJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowAA_KZL_LJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                }
                newrowAA_KZL_HJ[13] = "AA_KZL_HJ";
                newrowAA_KZL_LJ[13] = "AA_KZL_LJ";
                newrowAA_KZL_HJ2[13] = "AA_KZL_HJ";
                newrowAA_KZL_LJ2[13] = "AA_KZL_LJ";
                dtother.Rows.Add(newrowAA_KZL_HJ);
                dtother.Rows.Add(newrowAA_KZL_LJ);

                tbAA_KZL.Rows.Add(newrowAA_KZL_HJ2);
                tbAA_KZL.Rows.Add(newrowAA_KZL_LJ2);
                ds.Tables.Add(tbAA_KZL);
            }
            #endregion

            #region AH
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_AH);//服务免费成本AH
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本AH" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrowAH_MB_HJ = dtother.NewRow();
                DataRow newrowAH_MB_LJ = dtother.NewRow();
                DataRow newrowAH_MB_HJ2 = tbAH_MB.NewRow();
                DataRow newrowAH_MB_LJ2 = tbAH_MB.NewRow();
                double total1 = 0;
                double c = 0;
                double d = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    d = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowAH_MB_HJ[i] = c + d;//当月合计赋值
                    newrowAH_MB_HJ2[i] = c + d;//当月合计赋值
                    total1 += Convert.ToDouble(newrowAH_MB_HJ[i]);//合计累加
                    newrowAH_MB_LJ[i] = total1;//当月累计赋值
                    newrowAH_MB_LJ2[i] = total1;//当月累计赋值
                }
                newrowAH_MB_HJ[12] = total1;//合计总和
                newrowAH_MB_HJ[13] = "MB_AH_HJ";
                newrowAH_MB_LJ[13] = "MB_AH_LJ";
                newrowAH_MB_HJ2[12] = total1;//合计总和
                newrowAH_MB_HJ2[13] = "MB_AH_HJ";
                newrowAH_MB_LJ2[13] = "MB_AH_LJ";
                //AH目标加入
                dtother.Rows.Add(newrowAH_MB_HJ);
                dtother.Rows.Add(newrowAH_MB_LJ);
                tbAH_MB.Rows.Add(newrowAH_MB_HJ2);
                tbAH_MB.Rows.Add(newrowAH_MB_LJ2);
                ds.Tables.Add(tbAH_MB);
                //AH实际加入
                this.Bulid_AH_SJ(dt, dtother, tbAH_SJ, RptDatetime.Year);
                ds.Tables.Add(tbAH_SJ);
                //AH控制率
                DataRow newrowAH_KZL_HJ = dtother.NewRow();//控制率合计
                DataRow newrowAH_KZL_LJ = dtother.NewRow();//控制率累计
                DataRow newrowAH_KZL_HJ2 = tbAH_KZL.NewRow();//控制率合计
                DataRow newrowAH_KZL_LJ2 = tbAH_KZL.NewRow();//控制率累计
                for (int i = 0; i < 12; i++)
                {
                    a = dtother.Select("type='MB_AH_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AH_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowAH_KZL_HJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowAH_KZL_HJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                    a = dtother.Select("type='MB_AH_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AH_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowAH_KZL_LJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowAH_KZL_LJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                }
                newrowAH_KZL_HJ[13] = "AH_KZL_HJ";
                newrowAH_KZL_LJ[13] = "AH_KZL_LJ";
                newrowAH_KZL_HJ2[13] = "AH_KZL_HJ";
                newrowAH_KZL_LJ2[13] = "AH_KZL_LJ";

                dtother.Rows.Add(newrowAH_KZL_HJ);
                dtother.Rows.Add(newrowAH_KZL_LJ);
                tbAH_KZL.Rows.Add(newrowAH_KZL_HJ2);
                tbAH_KZL.Rows.Add(newrowAH_KZL_LJ2);
                ds.Tables.Add(tbAH_KZL);
            }
            #endregion

            #region P
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_P);//服务免费成本P
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本P" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrowP_MB_HJ = dtother.NewRow();
                DataRow newrowP_MB_LJ = dtother.NewRow();
                DataRow newrowP_MB_HJ2 = tbP_MB.NewRow();
                DataRow newrowP_MB_LJ2 = tbP_MB.NewRow();
                double total1 = 0;
                double c = 0;
                double d = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    d = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowP_MB_HJ[i] = c + d;//当月合计赋值
                    newrowP_MB_HJ2[i] = c + d;//当月合计赋值
                    total1 += Convert.ToDouble(newrowP_MB_HJ[i]);//合计累加
                    newrowP_MB_LJ[i] = total1;//当月累计赋值
                    newrowP_MB_LJ2[i] = total1;//当月累计赋值
                }
                newrowP_MB_HJ[12] = total1;//合计总和
                newrowP_MB_HJ[13] = "MB_P_HJ";
                newrowP_MB_LJ[13] = "MB_P_LJ";
                newrowP_MB_HJ2[12] = total1;//合计总和
                newrowP_MB_HJ2[13] = "MB_P_HJ";
                newrowP_MB_LJ2[13] = "MB_P_LJ";
                //AH目标加入
                dtother.Rows.Add(newrowP_MB_HJ);
                dtother.Rows.Add(newrowP_MB_LJ);
                tbP_MB.Rows.Add(newrowP_MB_HJ2);
                tbP_MB.Rows.Add(newrowP_MB_LJ2);
                ds.Tables.Add(tbP_MB);
                //AH实际加入
                this.Bulid_P_SJ(dt, dtother, tbP_SJ, RptDatetime.Year);
                ds.Tables.Add(tbP_SJ);
                //P控制率
                DataRow newrowP_KZL_HJ = dtother.NewRow();//控制率合计
                DataRow newrowP_KZL_LJ = dtother.NewRow();//控制率累计
                DataRow newrowP_KZL_HJ2 = tbP_KZL.NewRow();//控制率合计
                DataRow newrowP_KZL_LJ2 = tbP_KZL.NewRow();//控制率累计
                for (int i = 0; i < 12; i++)
                {
                    a = dtother.Select("type='MB_P_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_P_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowP_KZL_HJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowP_KZL_HJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                    a = dtother.Select("type='MB_P_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_P_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowP_KZL_LJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowP_KZL_LJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                }
                newrowP_KZL_HJ[13] = "P_KZL_HJ";
                newrowP_KZL_LJ[13] = "P_KZL_LJ";
                newrowP_KZL_HJ2[13] = "P_KZL_HJ";
                newrowP_KZL_LJ2[13] = "P_KZL_LJ";

                dtother.Rows.Add(newrowP_KZL_HJ);
                dtother.Rows.Add(newrowP_KZL_LJ);
                tbP_KZL.Rows.Add(newrowP_KZL_HJ2);
                tbP_KZL.Rows.Add(newrowP_KZL_LJ2);
                ds.Tables.Add(tbP_KZL);
            }
            #endregion

            #region CM
            //得到所有的 指标编号
            sn = Hanbell.BSC.Report.BscYearIndex.GetYearIndex(RptDatetime.Year, Hanbell.BSC.Report.ReportName.Fw_CM);//服务免费成本CM
            if (string.IsNullOrEmpty(sn))
            {
                throw new Exception("服务免费成本CM" + RptDatetime.Year + "指标未设定！");
            }
            //得到结果集
            //ds.Tables.Add(this.GetResult(sn, RptDatetime).Tables[0].Copy());
            dtbos = this.GetResult(sn, RptDatetime).Tables[0];
            if (dtbos.Rows.Count > 0)
            {
                DataRow newrowCM_MB_HJ = dtother.NewRow();
                DataRow newrowCM_MB_LJ = dtother.NewRow();
                DataRow newrowCM_MB_HJ2 = tbCM_MB.NewRow();
                DataRow newrowCM_MB_LJ2 = tbCM_MB.NewRow();
                double total1 = 0;
                double c = 0;
                double d = 0;
                for (int i = 0; i < 12; i++)
                {
                    c = dtbos.Select("dtype='去年同期' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂外
                    d = dtbos.Select("dtype='目标值' and dmonth=" + (i + 1)).Sum(p => p.Field<double>("dobjvalue"));//厂内
                    newrowCM_MB_HJ[i] = c + d;//当月合计赋值
                    newrowCM_MB_HJ2[i] = c + d;//当月合计赋值

                    total1 += Convert.ToDouble(newrowCM_MB_HJ[i]);//合计累加
                    newrowCM_MB_LJ[i] = total1;//当月累计赋值
                    newrowCM_MB_LJ2[i] = total1;//当月累计赋值
                }
                newrowCM_MB_HJ[12] = total1;//合计总和
                newrowCM_MB_HJ[13] = "MB_CM_HJ";
                newrowCM_MB_LJ[13] = "MB_CM_LJ";
                newrowCM_MB_HJ2[12] = total1;//合计总和
                newrowCM_MB_HJ2[13] = "MB_CM_HJ";
                newrowCM_MB_LJ2[13] = "MB_CM_LJ";
                //CM目标加入
                dtother.Rows.Add(newrowCM_MB_HJ);
                dtother.Rows.Add(newrowCM_MB_LJ);
                tbCM_MB.Rows.Add(newrowCM_MB_HJ2);
                tbCM_MB.Rows.Add(newrowCM_MB_LJ2);
                ds.Tables.Add(tbCM_MB);
                //CM实际加入
                this.Bulid_CM_SJ(dt, dtother, tbCM_SJ, RptDatetime.Year);
                ds.Tables.Add(tbCM_SJ);
                //CM控制率
                DataRow newrowCM_KZL_HJ = dtother.NewRow();//控制率合计
                DataRow newrowCM_KZL_LJ = dtother.NewRow();//控制率累计
                DataRow newrowCM_KZL_HJ2 = tbCM_KZL.NewRow();//控制率合计
                DataRow newrowCM_KZL_LJ2 = tbCM_KZL.NewRow();//控制率累计
                for (int i = 0; i < 12; i++)
                {
                    a = dtother.Select("type='MB_CM_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_CM_HJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowCM_KZL_HJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowCM_KZL_HJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);

                    a = dtother.Select("type='MB_CM_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    b = dtother.Select("type='SJ_AA_LJ'").Sum(p => p.Field<decimal>((i + 1).ToString()));
                    newrowCM_KZL_LJ[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                    newrowCM_KZL_LJ2[i] = decimal.Round(a / (b == 0 ? 1 : b), 2);
                }
                newrowCM_KZL_HJ[13] = "CM_KZL_HJ";
                newrowCM_KZL_LJ[13] = "CM_KZL_LJ";
                newrowCM_KZL_HJ2[13] = "CM_KZL_HJ";
                newrowCM_KZL_LJ2[13] = "CM_KZL_LJ";

                dtother.Rows.Add(newrowCM_KZL_HJ);
                dtother.Rows.Add(newrowCM_KZL_LJ);
                tbCM_KZL.Rows.Add(newrowCM_KZL_HJ2);
                tbCM_KZL.Rows.Add(newrowCM_KZL_LJ2);
                ds.Tables.Add(tbCM_KZL);
            }
            #endregion

            #endregion

            ds.Tables.Add(dtother);//dataset增加table["Other"]



            return ds;
        }















        /// <summary>
        /// 
        /// </summary>
        /// <param name="protype">产品别</param>
        /// <param name="areatype">区域别</param>
        /// <param name="year">年份</param>
        /// <param name="dtR_SJ">搭建的表</param>
        /// <param name="dtbsc">ERP存储发生数据的表</param>
        /// <param name="rowname">插入dtR_SJ表中行的第13列名称</param>
        private void SetDB_R_SJ(string protype, string areatype, int year, DataTable dtR_SJ, DataTable dtbsc, string rowname)
        {
            DataRow newrowR_SJ_HD = dtR_SJ.NewRow();
            decimal rhdfwll = 0;
            decimal rhdtravelcost = 0;
            decimal rhdfare = 0;
            decimal mancost = 0;
            decimal wxll = 0;
            decimal in1 = 0;
            decimal total = 0;
            for (int i = 0; i < 12; i++)
            {
                rhdfwll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + RptDatetime.Year).Sum(p => p.Field<decimal>("fwll"));//服务领料
                rhdtravelcost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + RptDatetime.Year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                rhdfare = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + RptDatetime.Year).Sum(p => p.Field<decimal>("fare"));//运费
                mancost = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + RptDatetime.Year).Sum(p => p.Field<decimal>("mancost"));//服务领料
                wxll = dtbsc.Select("protype='" + protype + "' and areatype='" + areatype + "' and month=" + (i + 1) + " and year=" + RptDatetime.Year).Sum(p => p.Field<decimal>("wxll"));//差旅
                in1 = rhdfwll + rhdtravelcost + rhdfare + mancost + wxll;
                total += in1;
                newrowR_SJ_HD[i] = in1;
            }
            newrowR_SJ_HD[12] = total;
            newrowR_SJ_HD[13] = rowname;
            dtR_SJ.Rows.Add(newrowR_SJ_HD);
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
        /// 
        /// </summary>
        /// <param name="protype"></param>
        /// <param name="areatype"></param>
        /// <param name="year"></param>
        /// <param name="dtR_SJ_OUT"></param>
        /// <param name="dtbsc"></param>
        /// <param name="rowname"></param>
        private void SetDB_R_SJ_IN(string protype, string areatype, int year, DataTable dtR_SJ_IN, DataTable dtbsc, string rowname)
        {
            DataRow newrow = dtR_SJ_IN.NewRow();
            decimal total = 0;
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

        #region

        #region
        //#region R_HD
 
        ///// <summary>
        ///// R冷媒华东厂外
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_HD_OUT(DataTable dtbsc, DataTable dtmaster,int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_HD_OUT
        //    decimal rhdfwll = 0;
        //    decimal rhdtravelcost = 0;
        //    decimal rhdfare = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        rhdfwll = dtbsc.Select("protype='R' and areatype='HD' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
        //        rhdtravelcost = dtbsc.Select("protype='R' and areatype='HD' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
        //        rhdfare = dtbsc.Select("protype='R' and areatype='HD' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
        //        in1 = rhdfwll + rhdtravelcost + rhdfare;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_OUT_RHD";//实际_外_R华东
        //    dtmaster.Rows.Add(newrow);
        //}
        ///// <summary>
        ///// R冷媒华东厂内
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_HD_IN(DataTable dtbsc, DataTable dtmaster,int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_HD_OUT
        //    decimal mancost = 0;
        //    decimal wxll = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        mancost = dtbsc.Select("protype='R' and areatype='HD' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
        //        wxll = dtbsc.Select("protype='R' and areatype='HD' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
        //        in1 = mancost + wxll;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_IN_RHD";//实际_内_R华东
        //    dtmaster.Rows.Add(newrow);
        //}
        //#endregion

        //#region R_NJ
        ///// <summary>
        ///// R冷媒南京厂外
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_NJ_OUT(DataTable dtbsc, DataTable dtmaster,int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_NJ_OUT
        //    decimal rhdfwll = 0;
        //    decimal rhdtravelcost = 0;
        //    decimal rhdfare = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        rhdfwll = dtbsc.Select("protype='R' and areatype='NJ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
        //        rhdtravelcost = dtbsc.Select("protype='R' and areatype='NJ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
        //        rhdfare = dtbsc.Select("protype='R' and areatype='NJ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
        //        in1 = rhdfwll + rhdtravelcost + rhdfare;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_OUT_RNJ";//实际_外_R南京
        //    dtmaster.Rows.Add(newrow);
        //}
        ///// <summary>
        ///// R冷媒华东厂内
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_NJ_IN(DataTable dtbsc, DataTable dtmaster,int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_NJ_OUT
        //    decimal mancost = 0;
        //    decimal wxll = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        mancost = dtbsc.Select("protype='R' and areatype='NJ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
        //        wxll = dtbsc.Select("protype='R' and areatype='NJ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
        //        in1 = mancost + wxll;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_IN_RNJ";//实际_内_R南京
        //    dtmaster.Rows.Add(newrow);
        //}

        //#endregion

        //#region R_HB

        ///// <summary>
        ///// R冷媒南京厂外
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_HB_OUT(DataTable dtbsc, DataTable dtmaster,int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_HB_OUT
        //    decimal rhdfwll = 0;
        //    decimal rhdtravelcost = 0;
        //    decimal rhdfare = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        rhdfwll = dtbsc.Select("protype='R' and areatype='HB' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
        //        rhdtravelcost = dtbsc.Select("protype='R' and areatype='HB' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
        //        rhdfare = dtbsc.Select("protype='R' and areatype='HB' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
        //        in1 = rhdfwll + rhdtravelcost + rhdfare;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_OUT_RHB";//实际_外_R华东
        //    dtmaster.Rows.Add(newrow);
        //}
        ///// <summary>
        ///// R冷媒华北厂内
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_HB_IN(DataTable dtbsc, DataTable dtmaster,int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_HB_OUT
        //    decimal mancost = 0;
        //    decimal wxll = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        mancost = dtbsc.Select("protype='R' and areatype='HB' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
        //        wxll = dtbsc.Select("protype='R' and areatype='HB' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
        //        in1 = mancost + wxll;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_IN_RHB";//实际_外_R华东
        //    dtmaster.Rows.Add(newrow);
        //}
        //#endregion

        //#region R_HN

        ///// <summary>
        ///// R冷媒华南厂外
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_HN_OUT(DataTable dtbsc, DataTable dtmaster, int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_HN_OUT
        //    decimal rhdfwll = 0;
        //    decimal rhdtravelcost = 0;
        //    decimal rhdfare = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        rhdfwll = dtbsc.Select("protype='R' and areatype='HN' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
        //        rhdtravelcost = dtbsc.Select("protype='R' and areatype='HN' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
        //        rhdfare = dtbsc.Select("protype='R' and areatype='HN' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
        //        in1 = rhdfwll + rhdtravelcost + rhdfare;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_OUT_RHN";//实际_外_R华东
        //    dtmaster.Rows.Add(newrow);
        //}
        ///// <summary>
        ///// R冷媒华南厂内
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_HN_IN(DataTable dtbsc, DataTable dtmaster,int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_HN_OUT
        //    decimal mancost = 0;
        //    decimal wxll = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        mancost = dtbsc.Select("protype='R' and areatype='HN' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
        //        wxll = dtbsc.Select("protype='R' and areatype='HN' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
        //        in1 = mancost + wxll;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_IN_RHN";//实际_外_R华东
        //    dtmaster.Rows.Add(newrow);
        //}
        //#endregion

        //#region R_CQ

        ///// <summary>
        ///// R冷媒重庆厂外SJ_OUT_RCQ
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_CQ_OUT(DataTable dtbsc, DataTable dtmaster,int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_CQ_OUT
        //    decimal rhdfwll = 0;
        //    decimal rhdtravelcost = 0;
        //    decimal rhdfare = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        rhdfwll = dtbsc.Select("protype='R' and areatype='CQ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务领料
        //        rhdtravelcost = dtbsc.Select("protype='R' and areatype='CQ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
        //        rhdfare = dtbsc.Select("protype='R' and areatype='CQ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
        //        in1 = rhdfwll + rhdtravelcost + rhdfare;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_OUT_RCQ";//实际_外_R华东
        //    dtmaster.Rows.Add(newrow);
        //}
        ///// <summary>
        ///// R冷媒重庆厂内SJ_IN_RCQ
        ///// </summary>
        ///// <param name="dtbsc"></param>
        ///// <param name="dtmaster"></param>
        ///// <param name="year"></param>
        //private void Bulid_R_CQ_IN(DataTable dtbsc, DataTable dtmaster, int year)
        //{
        //    DataRow newrow = dtmaster.NewRow();
        //    decimal total = 0;
        //    //R_CQ_OUT
        //    decimal mancost = 0;
        //    decimal wxll = 0;
        //    decimal in1 = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        mancost = dtbsc.Select("protype='R' and areatype='CQ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//服务领料
        //        wxll = dtbsc.Select("protype='R' and areatype='CQ' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//差旅
        //        in1 = mancost + wxll;
        //        total += in1;
        //        newrow[i] = in1;
        //    }
        //    newrow[12] = total;
        //    newrow[13] = "SJ_IN_RCQ";//实际_外_R华东
        //    dtmaster.Rows.Add(newrow);
        //}
        //#endregion
#endregion

        #region R_合计
        /// <summary>
        /// 当月实际合计厂外
        /// </summary>
        /// <param name="dtmaster"></param>
        private void Bulid_R_TOTAL_OUT(DataTable dtmaster)
        {
            int[] no = new int[5] { 0, 2, 4, 6, 8 };
            DataRow newrow = dtmaster.NewRow();
            for (int i = 0; i < no.Length; i++)
            {
                for (int j = 0; j < dtmaster.Columns.Count; j++)
                {
                    newrow[j] = Convert.ToDouble(newrow[j]) + Convert.ToDouble(dtmaster.Rows[no[i]][j]);

                }
            }
            dtmaster.Rows.Add(newrow);
        }
        /// <summary>
        /// 当月实际合计厂内
        /// </summary>
        /// <param name="dtmaster"></param>
        private void Bulid_R_TOTAL_IN(DataTable dtmaster)
        {

            int[] no = new int[5] { 1, 3, 5, 7, 9 };
            DataRow newrow = dtmaster.NewRow();
            for (int i = 0; i < no.Length; i++)
            {
                for (int j = 0; j < dtmaster.Columns.Count; j++)
                {
                    newrow[j] = Convert.ToDouble(newrow[j]) + Convert.ToDouble(dtmaster.Rows[no[i]][j]);

                }
            }
            dtmaster.Rows.Add(newrow);
        }
        #endregion

        #region R_累计
        /// <summary>
        /// 当月实际累计厂外
        /// </summary>
        /// <param name="dtmaster"></param>
        private void Bulid_R_LeiJi_OUT(DataTable dtmaster)
        {
            int no = 10;//累加的行dtmaster第10行
            DataRow newrow = dtmaster.NewRow();
            for (int i = 0; i < dtmaster.Columns.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    newrow[i] = Convert.ToDouble(newrow[i]) + Convert.ToDouble(dtmaster.Rows[no][j]);
                }
            }
            dtmaster.Rows.Add(newrow);
        }
        /// <summary>
        /// 当月实际累计厂内
        /// </summary>
        /// <param name="dtmaster"></param>
        private void Bulid_R_LeiJi_IN(DataTable dtmaster)
        {
            int no = 11;//累加的行dtmaster第10行
            DataRow newrow = dtmaster.NewRow();
            for (int i = 0; i < dtmaster.Columns.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    newrow[i] = Convert.ToDouble(newrow[i]) + Convert.ToDouble(dtmaster.Rows[no][j]);
                }
            }
            dtmaster.Rows.Add(newrow);
        }
        #endregion


        #region AA
        /// <summary>
        /// 实际AA
        /// </summary>
        /// <param name="dtbsc"></param>
        /// <param name="dtmaster"></param>
        /// <param name="year"></param>
        private void Bulid_AA_SJ(DataTable dtbsc, DataTable dtmaster, DataTable tbAA_SJ, int year)
        {
            DataRow newrowSJ = dtmaster.NewRow();
            DataRow newrowLJ = dtmaster.NewRow();
            DataRow newrowSJ2 = tbAA_SJ.NewRow();
            DataRow newrowLJ2 = tbAA_SJ.NewRow();
            decimal total = 0;
            decimal totlj = 0;//累计
            //R_CQ_OUT
            decimal mancost = 0;
            decimal wxll = 0;
            decimal fwll = 0;
            decimal travelcost = 0;
            decimal fare = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='AA' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//制令
                wxll = dtbsc.Select("protype='AA' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//维修
                travelcost = dtbsc.Select("protype='AA' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                fwll = dtbsc.Select("protype='AA' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务
                fare = dtbsc.Select("protype='AA' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                total = mancost + wxll + travelcost + fwll + fare;
                totlj += total;
                newrowSJ[i] = total;//实际
                newrowLJ[i] = totlj;//累计
                newrowSJ2[i] = total;//实际
                newrowLJ2[i] = totlj;//累计
            }
            newrowSJ[12] = total;
            newrowSJ[13] = "SJ_AA_HJ";//实际_AA_合计
            newrowLJ[13] = "SJ_AA_LJ";//实际_AA_累计
            newrowSJ2[12] = total;
            newrowSJ2[13] = "SJ_AA_HJ";//实际_AA_合计
            newrowLJ2[13] = "SJ_AA_LJ";//实际_AA_累计
            dtmaster.Rows.Add(newrowSJ);
            dtmaster.Rows.Add(newrowLJ);
            tbAA_SJ.Rows.Add(newrowSJ2);
            tbAA_SJ.Rows.Add(newrowLJ2);
        }
        #endregion

        #region AH
        /// <summary>
        /// 实际AH
        /// </summary>
        /// <param name="dtbsc"></param>
        /// <param name="dtmaster"></param>
        /// <param name="year"></param>
        private void Bulid_AH_SJ(DataTable dtbsc, DataTable dtmaster,DataTable tbAH_SJ, int year)
        {
            DataRow newrowSJ = dtmaster.NewRow();
            DataRow newrowLJ = dtmaster.NewRow();
            DataRow newrowSJ2 = tbAH_SJ.NewRow();
            DataRow newrowLJ2 = tbAH_SJ.NewRow();
            decimal total = 0;
            decimal totlj = 0;//累计
            decimal mancost = 0;
            decimal wxll = 0;
            decimal fwll = 0;
            decimal travelcost = 0;
            decimal fare = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='AH' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//制令
                wxll = dtbsc.Select("protype='AH' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//维修
                travelcost = dtbsc.Select("protype='AH' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                fwll = dtbsc.Select("protype='AH' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务
                fare = dtbsc.Select("protype='AH' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                total = mancost + wxll + travelcost + fwll + fare;
                totlj += total;
                newrowSJ[i] = total;//实际
                newrowLJ[i] = totlj;//累计
                newrowSJ2[i] = total;//实际
                newrowLJ2[i] = totlj;//累计
            }
            newrowSJ[12] = total;
            newrowSJ[13] = "SJ_AH_HJ";//实际_AH_合计
            newrowLJ[13] = "SJ_AH_LJ";//实际_AH_累计
            newrowSJ2[12] = total;
            newrowSJ2[13] = "SJ_AH_HJ";//实际_AH_合计
            newrowLJ2[13] = "SJ_AH_LJ";//实际_AH_累计
            dtmaster.Rows.Add(newrowSJ);
            dtmaster.Rows.Add(newrowLJ);
            tbAH_SJ.Rows.Add(newrowSJ2);
            tbAH_SJ.Rows.Add(newrowLJ2);
        }
        #endregion

        #region P
        /// <summary>
        /// 实际P
        /// </summary>
        /// <param name="dtbsc"></param>
        /// <param name="dtmaster"></param>
        /// <param name="year"></param>
        private void Bulid_P_SJ(DataTable dtbsc, DataTable dtmaster,DataTable tbP_SJ, int year)
        {
            DataRow newrowSJ = dtmaster.NewRow();
            DataRow newrowLJ = dtmaster.NewRow();
            DataRow newrowSJ2 = tbP_SJ.NewRow();
            DataRow newrowLJ2 = tbP_SJ.NewRow();
            decimal total = 0;
            decimal totlj = 0;//累计
            decimal mancost = 0;
            decimal wxll = 0;
            decimal fwll = 0;
            decimal travelcost = 0;
            decimal fare = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='P' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//制令
                wxll = dtbsc.Select("protype='P' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//维修
                travelcost = dtbsc.Select("protype='P' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                fwll = dtbsc.Select("protype='P' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务
                fare = dtbsc.Select("protype='P' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                total = mancost + wxll + travelcost + fwll + fare;
                totlj += total;
                newrowSJ[i] = total;//实际
                newrowLJ[i] = totlj;//累计
                newrowSJ2[i] = total;//实际
                newrowLJ2[i] = totlj;//累计
            }
            newrowSJ[12] = total;
            newrowSJ[13] = "SJ_P_HJ";//实际_AA_合计
            newrowLJ[13] = "SJ_P_LJ";//实际_LJ_累计
            newrowSJ2[12] = total;
            newrowSJ2[13] = "SJ_P_HJ";//实际_AA_合计
            newrowLJ2[13] = "SJ_P_LJ";//实际_LJ_累计
            dtmaster.Rows.Add(newrowSJ);
            dtmaster.Rows.Add(newrowLJ);
            tbP_SJ.Rows.Add(newrowSJ2);
            tbP_SJ.Rows.Add(newrowLJ2);
        }
        #endregion

        #region CM
        /// <summary>
        /// 实际CM
        /// </summary>
        /// <param name="dtbsc"></param>
        /// <param name="dtmaster"></param>
        /// <param name="year"></param>
        private void Bulid_CM_SJ(DataTable dtbsc, DataTable dtmaster,DataTable tbCM_SJ, int year)
        {
            DataRow newrowSJ = dtmaster.NewRow();
            DataRow newrowLJ = dtmaster.NewRow();
            DataRow newrowSJ2 = tbCM_SJ.NewRow();
            DataRow newrowLJ2 = tbCM_SJ.NewRow();
            decimal total = 0;
            decimal totlj = 0;//累计
            decimal mancost = 0;
            decimal wxll = 0;
            decimal fwll = 0;
            decimal travelcost = 0;
            decimal fare = 0;
            for (int i = 0; i < 12; i++)
            {
                mancost = dtbsc.Select("protype='CM' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("mancost"));//制令
                wxll = dtbsc.Select("protype='CM' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("wxll"));//维修
                travelcost = dtbsc.Select("protype='CM' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("travelcost"));//差旅
                fwll = dtbsc.Select("protype='CM' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fwll"));//服务
                fare = dtbsc.Select("protype='CM' and month=" + (i + 1) + " and year=" + year).Sum(p => p.Field<decimal>("fare"));//运费
                total = mancost + wxll + travelcost + fwll + fare;
                totlj += total;
                newrowSJ[i] = total;//实际
                newrowLJ[i] = totlj;//累计
                newrowSJ2[i] = total;//实际
                newrowLJ2[i] = totlj;//累计
            }
            newrowSJ[12] = total;
            newrowSJ[13] = "SJ_CM_HJ";//实际_AA_合计
            newrowLJ[13] = "SJ_CM_LJ";//实际_LJ_累计
            newrowSJ2[12] = total;
            newrowSJ2[13] = "SJ_CM_HJ";//实际_AA_合计
            newrowLJ2[13] = "SJ_CM_LJ";//实际_LJ_累计
            dtmaster.Rows.Add(newrowSJ);
            dtmaster.Rows.Add(newrowLJ);
            tbCM_SJ.Rows.Add(newrowSJ2);
            tbCM_SJ.Rows.Add(newrowLJ2);
        }
        #endregion

        #region 总合计
        #endregion

        #region
        #endregion

        #region
        #endregion

        #region
        #endregion

        #region
        #endregion

        #region
        #endregion

        #region
        #endregion

        #region
        #endregion

        #region
        #endregion


        /// <summary>
        /// 构建表结构
        /// </summary>
        /// <returns></returns>
        private DataTable BuildTB(string tablename)
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
        #endregion


    }
}
