using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

namespace Hanbell.GRpt.ServiceCB.MFCB.ToKFJAD
{
    /// <summary>
    /// 客服结案单计算
    /// </summary>
    public class RunToKFJADCB
    {
        #region
        public int Year { get; set; }
        public int Month { get; set; }
        Hanbell.DBUtility.IDbHelper dbhC = null;
        Hanbell.DBUtility.IDbHelper dbhG = null;
        Hanbell.DBUtility.IDbHelper dbhJ = null;
        Hanbell.DBUtility.IDbHelper dbhN = null;
        System.Data.IDbConnection connSybaseC = null;
        System.Data.IDbConnection connSybaseG = null;
        System.Data.IDbConnection connSybaseJ = null;
        System.Data.IDbConnection connSybaseN = null;
        /// <summary>
        /// 去除仓库
        /// </summary>
        public string CWarehNot { get; set; }
        public string NWarehNot { get; set; }
        public string JWarehNot { get; set; }
        public string GWarehNot { get; set; }


        Hanbell.DBUtility.IDbHelper sqldbh = null;
        System.Data.IDbConnection connSql = null;//sql链接
        string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
        string ConstrG = Hanbell.DBUtility.PubConstant.GetConnectionString("connection2");
        string ConstrJ = Hanbell.DBUtility.PubConstant.GetConnectionString("connection3");
        string ConstrN = Hanbell.DBUtility.PubConstant.GetConnectionString("connection4");
        string ConstrSql = Hanbell.DBUtility.PubConstant.GetConnectionString("conn1");

        public RunToKFJADCB(int year,int month)
        {
            this.Year = year;
            this.Month = month;
            sqldbh = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.SqlClient);
            connSql = sqldbh.GetConnection(ConstrSql);

            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);

            dbhG = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseG = dbhG.GetConnection(ConstrG);

            dbhJ = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseJ = dbhJ.GetConnection(ConstrJ);

            dbhN = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseN = dbhN.GetConnection(ConstrN);
            this.SetWarehNot();
        }
        public RunToKFJADCB()
        {

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void SetWarehNot()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "\\config\\common\\NotWareh.xml");
            XmlNode node = doc.DocumentElement.SelectSingleNode("Cwareh");
            this.CWarehNot = node.InnerText;
            node = doc.DocumentElement.SelectSingleNode("Nwareh");
            this.NWarehNot = node.InnerText;
            node = doc.DocumentElement.SelectSingleNode("Jwareh");
            this.JWarehNot = node.InnerText;
            node = doc.DocumentElement.SelectSingleNode("Gwareh");
            this.GWarehNot = node.InnerText;
        }


        public void StartRun()
        {
            BulidMaster();
        }


        /// <summary>
        /// 得到结案单对应的客服单号
        /// </summary>
        /// <param name="enddate">结案日期</param>
        /// <returns></returns>
        private System.Data.DataTable GetTB_KFDH()
        {
            string sql = string.Format(@"select hzkfjad003 from hzkfjad 
                            left join resda 
                            on hzkfjad002=resda002
                            where hzkfjad001='HZKFJAD' and resda021='2' 
                            and (hzkfjad009 like '%免费' or hzkfjad009 like '%待判')
                            and month(CONVERT(datetime,resda019))={0} 
                            and year(CONVERT(datetime,resda019))={1}", Month, Year);
            DataTable dt = sqldbh.Query(connSql, sql);
            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    return dt;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到结案的服务单号
        /// 客服单号对应的服务单号
        /// hzfwd004客服单号
        /// hzfwd002系统服务单号
        /// hzfwd006服务单号
        /// </summary>
        /// <param name="tb_kfdh">客服单号表</param>
        /// <returns></returns>
        private DataTable GetTB_FWDH(DataTable tb_kfdh)
        {
            if (tb_kfdh != null)
            {
                if (tb_kfdh.Rows.Count > 0)
                {
                    string sqlwherein = GetSqlWhereIn(tb_kfdh, 0);
                    if (string.IsNullOrEmpty(sqlwherein) == false)
                    {
                        string sql = string.Format("select hzfwd004,hzfwd002,hzfwd006 from where  hzfwd001='HZFWD' and  hzfwd004 in ({0})", sqlwherein);
                        DataTable dt = sqldbh.Query(connSql, sql);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                return dt;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 通过客服单号得到服务领料单头
        /// hzfwlld002  领料单系统单号
        /// hzfwlld003  客服单号
        /// hzfwlld004  服务单号
        /// hzfwlld005  领料单号
        /// hzfwlld011  ERP回写单号
        /// hmark1      区域别
        /// hmark2      产品别
        /// </summary>
        /// <param name="tb_fwdh"></param>
        /// <returns></returns>
        private DataTable GetTB_FWLLDH(DataTable tb_kfdh)
        {
            if (tb_kfdh != null)
            {
                if (tb_kfdh.Rows.Count > 0)
                {
                    string sqlwherein = GetSqlWhereIn(tb_kfdh, 0);
                    if (string.IsNullOrEmpty(sqlwherein) == false)
                    {
                        string sql = string.Format(@"select hzfwlld002,hzfwlld003,hzfwlld004,hzfwlld005,hzfwlld011,hmark1,hmark2
                                                        from hzfwlld where hzfwlld001='HZFWLLD' and hzfwlld003 in ({0})", sqlwherein);
                        DataTable dt = sqldbh.Query(connSql,sql);
                        if (dt!=null)
                        {
                            if (dt.Rows.Count>0)
                            {
                                return dt;
                            }
                        }
                    }
                }
            }   
            return null;
        }

        /// <summary>
        /// 构建TB
        /// 通过客服单号从领料单得到对应的产品别，区域别
        /// hzfwlld003 客服单号
        /// hmark1 区域别
        /// hmark2 产品别
        /// </summary>
        /// <param name="tb_kfdh"></param>
        /// <returns></returns>
        private DataTable BulidTB(DataTable tb_kfdh,int year,int month)
        {
            if (tb_kfdh != null)
            {
                if (tb_kfdh.Rows.Count > 0)
                {
                    string sqlwherein = GetSqlWhereIn(tb_kfdh, 0);
                    if (string.IsNullOrEmpty(sqlwherein) == false)
                    {
                        string sql = string.Format(@"select hzfwlld003,hmark1,hmark2 from hzfwlld where hzfwlld001='HZFWLLD' 
                                                    and hzfwlld003 in ({0}) 
                                                    group by hzfwlld003,hmark1,hmark2", sqlwherein);
                        DataTable dt = sqldbh.Query(connSql, sql);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                DataTable dtb = this.BulidTB();
                                foreach (DataRow row in dt.Rows)
                                {
                                    DataRow newrow = dtb.NewRow();
                                    newrow["servicetype"] = 0;
                                    newrow["protype"] = row["hmark2"];
                                    newrow["areatype"] = row["hmark1"];
                                    newrow["kfdh"] = row["hzfwlld003"];
                                    newrow["year"] = year;
                                    newrow["month"] = month;
                                    dtb.Rows.Add(newrow);
                                }
                                return dtb;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 赋值最终表
        /// </summary>
        private void BulidMaster()
        {
            DataTable dtkfd = this.GetTB_KFDH();//客服单
            DataTable dtfwlld = this.GetTB_FWLLDH(dtkfd);//领料单
            DataTable dtC_IAF = this.GetTBC_IAF(dtfwlld);//总公司IAF单据交易明细
            DataTable dtG_IAF = this.GetTBG_IAF(dtfwlld);//广州
            DataTable dtN_IAF = this.GetTBN_IAF(dtfwlld);//南京
            DataTable dtJ_IAF = this.GetTBJ_IAF(dtfwlld);//济南
            DataTable dt_MAN = this.getTB_MAN();//制令
            DataTable dtIAG = this.GetTB_IAG();//服务退料
            DataTable dtMaster = this.BulidTB(dtkfd, this.Year, this.Month);//构建要更新至bsc中的表结构
            if (dtMaster!=null)
            {
                if (dtMaster.Rows.Count>0)
                {
                    for (int i = 0; i < dtMaster.Rows.Count; i++)
                    {
                        string kfdh = dtMaster.Rows[i]["kfdh"].ToString();//客服单号
                        decimal tramtsIAF = 0;
                        decimal tramtsIAG = 0;
                        decimal tramtsBXD = 0;
                        decimal tramtsJZGHD = 0;
                        decimal tramtsMAN = 0;
                        if (string.IsNullOrEmpty(kfdh) == false)
                        {
                            #region 物料发生成本
                            #region 服务领料成本
                            DataRow[] lld = dtfwlld.Select("hzfwlld003='" + kfdh + "'");//根据客服单号得到领料单
                            if (lld.Length>0)
                            {
                                string erpno = string.Empty;//回写ERP单号
                                foreach (DataRow row in lld)
                                {
                                    erpno = row["hzfwlld011"].ToString();
                                    if (string.IsNullOrEmpty(erpno) == false)
                                    {
                                        tramtsIAF += this.GetIAF(erpno, dtC_IAF, dtG_IAF, dtN_IAF, dtJ_IAF);//ERP_IAF交易明细库存成本金额
                                    }
                                }
                            }
                            #endregion

                            #region 服务退料金额
                            tramtsIAG = GetIAG(kfdh, dtIAG);
                            #endregion
                            #endregion

                            #region 差旅发生成本
                            //费用报销单
                            tramtsBXD = GetBXD(kfdh);

                            //借支归还单
                            tramtsJZGHD = GetJZGHD(kfdh);

                            #endregion

                            #region 制令发生成本
                            if (dt_MAN!=null)
                            {
                                tramtsMAN = this.GetMAN(kfdh, dt_MAN);
                            }
                            #endregion

                            #region 运费发生成本
                            #endregion
                        }
                        dtMaster.Rows[i]["fwll"] = tramtsIAF - tramtsIAG;//服务领料成本IAF-IAG
                        dtMaster.Rows[i]["travelcost"] = tramtsBXD + tramtsJZGHD;//报销单，借支归还单
                        dtMaster.Rows[i]["mancost"] = tramtsMAN;
                    }
                    InsertBSC(dtMaster, "bsc_tb_mfservicejad");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        private void InsertBSC(System.Data.DataTable dt, string tableName)
        {
            List<string> list = new List<string>();

            foreach (System.Data.DataRow row in dt.Rows)
            {
                StringBuilder sb = new StringBuilder("");
                sb.AppendFormat("insert into {0} values(", tableName);
                sb.AppendFormat("{0},{1},{2},'{3}','{4}','{5}','{6}',{7},{8},{9},{10},{11})"
                                    , row[0].ToString(), row[1].ToString(), row[2].ToString()
                                    , row[3].ToString(), row[4].ToString(), row[5].ToString()
                                    , row[6].ToString(), row[7].ToString() == "" ? "0" : row[7].ToString(), row[8].ToString() == "" ? "0" : row[8].ToString()
                                    , row[9].ToString()== "" ? "0" : row[9].ToString(), row[10].ToString()== "" ? "0" : row[10].ToString(), row[11].ToString()== "" ? "0" : row[11].ToString());
                list.Add(sb.ToString());
            }
            try
            {
                dbhC.ExecuteSqlTran(connSybaseC, list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kfdh">客服单号</param>
        /// <returns></returns>
        private decimal GetBXD(string kfdh)
        {
            decimal tramts = 0;
            string sql = string.Format(@"select sum(bmpb16c) tramts from bxd002_2 left join resda 
                                on resda002=bxd002_2.bxd002_2002 
                                where 
                                resda021=2 and
                                kfdhd02='{0}' and 
                                kfdhd02 <> ''", kfdh);
            DataTable dt = sqldbh.Query(connSql, sql);
            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    tramts = string.IsNullOrEmpty(dt.Rows[0][0].ToString()) == false ? Convert.ToDecimal(dt.Rows[0][0]) : 0;
                }
            }
            return tramts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kfdh">客服单号</param>
        /// <returns></returns>
        private decimal GetJZGHD(string kfdh)
        {
            decimal tramts = 0;
            string sql = string.Format(@"select sum(bmhb14c) tramts from jzghd02 left join resda 
                                        on resda002=jzghd02.jzghd02002 
                                        where 
                                        resda021=2 and
                                        kfdhd02='{0}' and 
                                        kfdhd02 <> '' ", kfdh);
            DataTable dt = sqldbh.Query(connSql, sql);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    tramts = string.IsNullOrEmpty(dt.Rows[0][0].ToString()) == false ? Convert.ToDecimal(dt.Rows[0][0]) : 0;
                }
            }
            return tramts;
        }

        /// <summary>
        /// 根据ERP回写单号得到ERP交易明细
        /// </summary>
        /// <param name="erpno"></param>
        private decimal GetIAF(string erpno,DataTable dtc,DataTable dtg,DataTable dtn,DataTable dtj)
        {
            decimal tramt = 0;
            if (string.IsNullOrEmpty(erpno) == false)
            {
                string facno = erpno.Substring(0,4);
                switch (facno)
                {
                    case "CIAF":
                        DataRow[] rowsc = dtc.Select("trno='" + erpno + "'");
                        if (rowsc.Length>0)
                        {
                            foreach (DataRow row in rowsc)
                            {
                                tramt += (Convert.ToDecimal(row["tramt"]));
                            }
                        }
                        break;
                    case "GIAF":
                        DataRow[] rowsg = dtg.Select("trno='" + erpno + "'");
                        if (rowsg.Length > 0)
                        {
                            foreach (DataRow row in rowsg)
                            {
                                tramt += (Convert.ToDecimal(row["tramt"]));
                            }
                        }
                        break;
                    case "NIAF":
                        DataRow[] rowsn = dtc.Select("trno='" + erpno + "'");
                        if (rowsn.Length > 0)
                        {
                            foreach (DataRow row in rowsn)
                            {
                                tramt += (Convert.ToDecimal(row["tramt"]));
                            }
                        }
                        break;
                    case "JIAF":
                        DataRow[] rowsj = dtc.Select("trno='" + erpno + "'");
                        if (rowsj.Length > 0)
                        {
                            foreach (DataRow row in rowsj)
                            {
                                tramt += (Convert.ToDecimal(row["tramt"]));
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return tramt;
        }

        /// <summary>
        /// 根据客服单号得到退料金额
        /// </summary>
        /// <param name="kfno"></param>
        /// <returns></returns>
        private decimal GetIAG(string kfno, DataTable dtIAG)
        {
            decimal tramts = 0;
            if (dtIAG!=null)
            {
                if (dtIAG.Rows.Count>0)
                {
                    DataRow[] rows = dtIAG.Select("kfno='" + kfno + "'");
                    if (rows.Length > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            if (string.IsNullOrEmpty(row["tramt"].ToString()) == false)
                            {
                                tramts += (Convert.ToDecimal(row["tramt"]));
                            }
                        }
                    }
                }
            }            
            return tramts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kfdh"></param>
        /// <param name="dt_MAN"></param>
        /// <returns></returns>
        private decimal GetMAN(string kfdh,DataTable dt_MAN)
        {
            decimal tramts = 0;
            DataRow[] rows = dt_MAN.Select("kfdh='" + kfdh + "'");
            if (rows!=null)
            {
                if (rows.Length > 0)
                {
                    tramts = rows.Sum(p => p.Field<decimal>("tramt"));
                }
            }
            return tramts;
        }


        #region 各公司IAF查询条件sqlin
        /// <summary>
        /// 得到总公司的领料单号的sqlin
        /// </summary>
        /// <param name="fwlldh">服务领料单表</param>
        /// <returns></returns>
        private string GetC_IAF_sqlin(DataTable fwlldh)
        {
            DataRow[] rows = fwlldh.Select("hzfwlld011 like 'CIAF%'");
            StringBuilder sb = new StringBuilder("");
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    sb.Append("'" + rows[i]["hzfwlld011"].ToString() + "',");
                }
                if (sb.ToString().Length>0)
                {
                    return sb.ToString().Substring(0, sb.ToString().Length - 1);
                }
            }
            return null;
        }

        /// <summary>
        /// 得到广州的领料单号的sqlin
        /// </summary>
        /// <param name="fwlldh">服务领料单表</param>
        /// <returns></returns>
        private string GetG_IAF_sqlin(DataTable fwlldh)
        {
            DataRow[] rows = fwlldh.Select("hzfwlld011 like 'GIAF%'");
            StringBuilder sb = new StringBuilder("");
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    sb.Append("'" + rows[i]["hzfwlld011"].ToString() + "',");
                }
                if (sb.ToString().Length > 0)
                {
                    return sb.ToString().Substring(0, sb.ToString().Length - 1);
                }
            }
            return null;
        }

        /// <summary>
        /// 得到广州的领料单号的sqlin
        /// </summary>
        /// <param name="fwlldh">服务领料单表</param>
        /// <returns></returns>
        private string GetJ_IAF_sqlin(DataTable fwlldh)
        {
            DataRow[] rows = fwlldh.Select("hzfwlld011 like 'JIAF%'");
            StringBuilder sb = new StringBuilder("");
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    sb.Append("'" + rows[i]["hzfwlld011"].ToString() + "',");
                }
                if (sb.ToString().Length > 0)
                {
                    return sb.ToString().Substring(0, sb.ToString().Length - 1);
                }
            }
            return null;
        }

        /// <summary>
        /// 得到广州的领料单号的sqlin
        /// </summary>
        /// <param name="fwlldh">服务领料单表</param>
        /// <returns></returns>
        private string GetN_IAF_sqlin(DataTable fwlldh)
        {
            DataRow[] rows = fwlldh.Select("hzfwlld011 like 'NIAF%'");
            StringBuilder sb = new StringBuilder("");
            if (rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    sb.Append("'" + rows[i]["hzfwlld011"].ToString() + "',");
                }
                if (sb.ToString().Length > 0)
                {
                    return sb.ToString().Substring(0, sb.ToString().Length - 1);
                }
            }
            return null;
        }


        #endregion

        #region 得到各公司发生服务领料的交易明细资料IAF 1001
        /// <summary>
        /// 上海
        /// </summary>
        /// <param name="fwlldh"></param>
        /// <returns></returns>
        private DataTable GetTBC_IAF(DataTable fwlldh)
        {
            if (fwlldh!=null)
            {
                if (fwlldh.Rows.Count>0)
                {
                    string sqlwherein = GetC_IAF_sqlin(fwlldh);
                    if (string.IsNullOrEmpty(sqlwherein) == false)
                    {
                        string sql = string.Format(@"select b.kfno,b.fwno,a.trno,sum(a.tramt) as tramt from 
                                                invtrnh a left join invhadh b on a.trno=b.trno
                                                where a.facno=b.facno
                                                and a.prono=b.prono and a.trtype=b.trtype
                                                and a.facno='C' and b.facno='C'
                                                and a.prono='1' and b.prono='1'
                                                and a.trtype='IAF' and b.trtype='IAF' 
                                                and a.trno in ({0}) 
                                                group by a.trno,b.kfno,b.fwno", sqlwherein);
                        DataTable dt = dbhC.Query(connSybaseC, sql);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                return dt;
                            }
                        }
                    }
                }
            }            
            return null;
        }

        /// <summary>
        /// 广州
        /// </summary>
        /// <param name="fwlldh"></param>
        /// <returns></returns>
        private DataTable GetTBG_IAF(DataTable fwlldh)
        {
            if (fwlldh != null)
            {
                if (fwlldh.Rows.Count > 0)
                {
                    string sqlwherein = GetG_IAF_sqlin(fwlldh);
                    if (string.IsNullOrEmpty(sqlwherein) == false)
                    {
                        string sql = string.Format(@"select b.kfno,b.fwno,a.trno,sum(a.tramt) as tramt from 
                                                invtrnh a left join invhadh b on a.trno=b.trno
                                                where a.facno=b.facno
                                                and a.prono=b.prono and a.trtype=b.trtype
                                                and a.facno='G' and b.facno='G'
                                                and a.prono='1' and b.prono='1'
                                                and a.trtype='IAF' and b.trtype='IAF' 
                                                and a.trno in ({0}) 
                                                group by a.trno,b.kfno,b.fwno", sqlwherein);
                        DataTable dt = dbhG.Query(connSybaseG, sql);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                return dt;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 南京
        /// </summary>
        /// <param name="fwlldh"></param>
        /// <returns></returns>
        private DataTable GetTBN_IAF(DataTable fwlldh)
        {
            if (fwlldh != null)
            {
                if (fwlldh.Rows.Count > 0)
                {
                    string sqlwherein = GetN_IAF_sqlin(fwlldh);
                    if (string.IsNullOrEmpty(sqlwherein) == false)
                    {
                        string sql = string.Format(@"select b.kfno,b.fwno,a.trno,sum(a.tramt) as tramt from 
                                                invtrnh a left join invhadh b on a.trno=b.trno
                                                where a.facno=b.facno
                                                and a.prono=b.prono and a.trtype=b.trtype
                                                and a.facno='N' and b.facno='N'
                                                and a.prono='1' and b.prono='1'
                                                and a.trtype='IAF' and b.trtype='IAF' 
                                                and a.trno in ({0}) 
                                                group by a.trno,b.kfno,b.fwno", sqlwherein);
                        DataTable dt = dbhN.Query(connSybaseN, sql);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                return dt;
                            }
                        }
                    }
                }
            }
                    
            return null;
        }


        /// <summary>
        /// 济南
        /// </summary>
        /// <param name="fwlldh"></param>
        /// <returns></returns>
        private DataTable GetTBJ_IAF(DataTable fwlldh)
        {
            if (fwlldh != null)
            {
                if (fwlldh.Rows.Count > 0)
                {
                    string sqlwherein = GetJ_IAF_sqlin(fwlldh);
                    if (string.IsNullOrEmpty(sqlwherein) == false)
                    {
                        string sql = string.Format(@"select b.kfno,b.fwno,a.trno,sum(a.tramt) as tramt from 
                                                invtrnh a left join invhadh b on a.trno=b.trno
                                                where a.facno=b.facno
                                                and a.prono=b.prono and a.trtype=b.trtype
                                                and a.facno='J' and b.facno='J'
                                                and a.prono='1' and b.prono='1'
                                                and a.trtype='IAF' and b.trtype='IAF' 
                                                and a.trno in ({0}) 
                                                group by a.trno,b.kfno,b.fwno", sqlwherein);
                        DataTable dt = dbhJ.Query(connSybaseJ, sql);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                return dt;
                            }
                        }
                    }
                }
            }
            return null;
        }




        #endregion


        #region 服务退料的交易明细资料IAF_1002
        /// <summary>
        /// 得到所有退料单
        /// </summary>
        /// <returns></returns>
        private DataTable GetTB_IAG()
        {
            DataTable dtmaster = null;
            string sql = @"select b.kfno,b.fwno,a.trno,sum(a.tramt) as tramt from 
                                invtrnh a left join invhadh b on a.trno=b.trno,invmas c
                                where a.facno=b.facno
                                and a.prono=b.prono and a.trtype=b.trtype and a.itnbr=c.itnbr
                                and a.wareh not in ({1})
                                and a.facno='{0}' and b.facno='{0}'
                                and a.prono='1' and b.prono='1'
                                and a.trtype='IAG' and b.trtype='IAG' 
                                and b.resno='1002'
                                and b.kfno is not null and b.kfno <> ''
                                group by a.trno,b.kfno,b.fwno";
            DataTable dtc = dbhC.Query(connSybaseC, string.Format(sql, "C",this.CWarehNot));
            DataTable dtg = dbhG.Query(connSybaseG, string.Format(sql, "G",this.GWarehNot));
            DataTable dtj = dbhJ.Query(connSybaseJ, string.Format(sql, "J",this.JWarehNot));
            DataTable dtn = dbhN.Query(connSybaseN, string.Format(sql, "N",this.NWarehNot));
            DataSet ds = new DataSet();
            ds.Tables.Add(dtc);
            ds.Tables.Add(dtg);
            ds.Tables.Add(dtj);
            ds.Tables.Add(dtn);

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i] != null)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        if (dtmaster == null)
                        {
                            dtmaster = ds.Tables[i].Copy();
                        }
                        else
                        {
                            dtmaster.Merge(ds.Tables[i]);
                        }
                    }
                }
            }
            return dtmaster;
        }

        #endregion

        /// <summary>
        /// 得到制令交易明细
        /// </summary>
        /// <returns></returns>
        private DataTable getTB_MAN()
        {
            string sql = string.Format("select * from bsc_tb_mfservicemid where kfdh is not null and kfdh <> '' and year={0}", this.Year);
            DataTable dt = dbhC.Query(sql);
            if (dt!=null)
            {
                if (dt.Rows.Count>0)
                {
                    return dt;
                }
            }
            return null;
        }



        /// <summary>
        /// 每月更新中间表
        /// </summary>
        /// <param name="date"></param>
        public void Insert_bsc_tb_mfservicemid(string yyyyMM)
        {
            string sql = string.Format(@"insert into bsc_tb_mfservicemid 
                                            select year(b.trdate) year,month(b.trdate) month,a.manno,a.iocode,a.kfdh,case a.iocode when '1' then -sum(b.ttmatm) else sum(b.ttmatm) end  as tramt
                                            from(
	                                            SELECT 	manpih.facno,manpih.prono, manpih.manno,manpih.itnbrf,y.itdsc as itnbrfdsc,manpih.issdepno,manpih.trtype,manpih.pisno,
				                                            manpih.iocode,manpid.seqnr,manmas.kfdh
	                                            FROM manpid 
	                                            left outer join invmas x 
	                                            on x.itnbr=manpid.altitnbr,manpih 
	                                            left outer join invmas y 
	                                            on y.itnbr=manpih.itnbrf   
	                                            left outer join manmas 
	                                            on manmas.facno=manpih.facno 
	                                            and  manmas.prono=manpih.prono 
	                                            and manmas.manno=manpih.manno 
	                                            WHERE ( manpih.facno = manpid.facno ) 
	                                            and ( manpih.prono = manpid.prono ) 
	                                            and ( manpih.pisno = manpid.pisno ) 
	                                            and ( ( manpih.issstatus = 'C' ) ) 
	                                            and manpid.altitnbr<>'3188-GBR6254-FW' 
	                                            and manpih.facno='C' and manpih.prono='1' 	
	                                            and manpid.wareh not in({1})
	                                            and manmas.typecode='02' 
	                                            and (manpih.issdepno='9900') 
	                                            and (convert(varchar(6),manpih.issdate,112)='{0}')  
                                            UNION    
                                            SELECT 	manreh.facno, manreh.prono,manreh.manno,manreh.itnbrf, t.itdsc as  itnbrfdsc,manreh.retdepno, manreh.trtype,manreh.retno,
			                                            manreh.iocode, manred.seqnr, manmas.kfdh 
                                            FROM manred 
                                            left outer join invmas s 
                                            on s.itnbr=manred.altitnbr, manreh 
                                            left outer join invmas t 
                                            on t.itnbr=manreh.itnbrf 
                                            left outer join manmas 
                                            on manmas.facno=manreh.facno 
                                            and  manmas.prono=manreh.prono 
                                            and manmas.manno=manreh.manno 
                                            WHERE ( manreh.facno = manred.facno ) 
                                            and ( manreh.prono = manred.prono ) 
                                            and ( manreh.retno = manred.retno ) 
                                            and ( ( manreh.issstatus = 'C' ) ) 
                                            and manred.altitnbr<>'3188-GBR6254-FW' 
                                            and manreh.facno='C' and manreh.prono='1' 
                                            and manred.wareh not in({1})
                                            and manmas.typecode='02' 
                                            and (manreh.retdepno='9900' ) 
                                            and (convert(varchar(6),manreh.retdate,112)='{0}')) a 
                                            left join invtrnh b 
                                            on a.facno=b.facno 
                                            and a.trtype=b.trtype 
                                            and a.pisno=b.trno 
                                            and a.seqnr=b.trseq 
                                            where b.facno='C' 
                                            and b.prono='1' 
                                            group by a.manno,a.kfdh,a.iocode,month(b.trdate),year(b.trdate)", yyyyMM,this.CWarehNot);
            try
            {                
                int ret = dbhC.ExecuteSql(connSybaseC, sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 得到sqlin
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i">以table中的哪一列为基础</param>
        /// <returns></returns>
        private string GetSqlWhereIn(DataTable dt,int i)
        {
            StringBuilder sb = new StringBuilder("");
            string sqlwherein = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                if (string.IsNullOrEmpty(row[0].ToString().Trim()) == false)
                {
                    sb.Append("'" + row[i] + "',");
                }
            }
            if (sb.ToString().Length>0)
            {
                sqlwherein = sb.ToString().Substring(0, sb.ToString().Length - 1);
                return sqlwherein;
            }
            return null;
        }


        private System.Data.DataTable BulidTB()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("servicetype",typeof(int));
            dt.Columns.Add("year",typeof(int));
            dt.Columns.Add("month",typeof(int));
            dt.Columns.Add("kfdh",typeof(string));
            dt.Columns.Add("jadh",typeof(string));
            dt.Columns.Add("protype",typeof(string));
            dt.Columns.Add("areatype",typeof(string));
            dt.Columns.Add("mancost",typeof(decimal));
            dt.Columns.Add("wxll",typeof(decimal));
            dt.Columns.Add("fwll",typeof(decimal));
            dt.Columns.Add("travelcost",typeof(decimal));
            dt.Columns.Add("fare", typeof(decimal));
            return dt;
        }

    }
}
