using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Hanbell.GRpt.ServiceCB.MFCB.ToMonth
{
    public class RunToMonthCB
    {
        public int IsWritemsg { get; set; }//

        #region
        Hanbell.DBUtility.IDbHelper dbhC = null;
        Hanbell.DBUtility.IDbHelper dbhG = null;
        Hanbell.DBUtility.IDbHelper dbhC4 = null;//增加重庆
        Hanbell.DBUtility.IDbHelper dbhJ = null;
        Hanbell.DBUtility.IDbHelper dbhN = null;
        Hanbell.DBUtility.IDbHelper dbhK = null;
        Hanbell.DBUtility.IDbHelper dbNewOA = null;
        Hanbell.DBUtility.IDbHelper dbCRM= null;
        System.Data.IDbConnection connSybaseC = null;
        System.Data.IDbConnection connSybaseG = null;
        System.Data.IDbConnection connSybaseC4 = null;//增加重庆
        System.Data.IDbConnection connSybaseJ = null;
        System.Data.IDbConnection connSybaseN = null;
        System.Data.IDbConnection connSybaseK = null;
        System.Data.IDbConnection connSybaseNewOA = null;
        System.Data.IDbConnection connSybaseCRM = null;

        Hanbell.DBUtility.IDbHelper sqldbh = null;
        System.Data.IDbConnection connSql = null;//sql链接
        string ConstrC = Hanbell.DBUtility.PubConstant.GetConnectionString("connection1");
        string ConstrG = Hanbell.DBUtility.PubConstant.GetConnectionString("connection2");
        string ConstrJ = Hanbell.DBUtility.PubConstant.GetConnectionString("connection4");
        string ConstrN = Hanbell.DBUtility.PubConstant.GetConnectionString("connection3");
        string ConstrC4 = Hanbell.DBUtility.PubConstant.GetConnectionString("connectionCQ");
        string ConstrK = Hanbell.DBUtility.PubConstant.GetConnectionString("connection9");//20150206新增柯茂
        string ConstrSql = Hanbell.DBUtility.PubConstant.GetConnectionString("conn1");
        string ConstrNewOA = Hanbell.DBUtility.PubConstant.GetConnectionString("conNewOA");
        string ConstrCRM = Hanbell.DBUtility.PubConstant.GetConnectionString("conCRM");

        /// <summary>
        /// 去除仓库
        /// </summary>
        public string CWarehNot { get; set; }
        public string NWarehNot { get; set; }
        public string JWarehNot { get; set; }
        public string GWarehNot { get; set; }
        public string C4WarehNot { get; set; }
        public string CmanWarehNot { get; set; }
        public string GmanWarehNot { get; set; }
        public string C4manWarehNot { get; set; }
        public string NmanWarehNot { get; set; }
        public string JmanWarehNot { get; set; }


        /// <summary>
        /// 明细中间表
        /// </summary>
        private System.Data.DataTable DtMsg { get; set; }


        public RunToMonthCB(int year, int month)
        {
            Year = year;
            Month = month;
            sqldbh = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.SqlClient);
            connSql = sqldbh.GetConnection(ConstrSql);

            dbhC = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC = dbhC.GetConnection(ConstrC);

            dbhG = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseG = dbhG.GetConnection(ConstrG);

            dbhC4 = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseC4 = dbhC4.GetConnection(ConstrC4);

            dbhJ = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseJ = dbhJ.GetConnection(ConstrJ);

            dbhN = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseN = dbhN.GetConnection(ConstrN);

            dbhK = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.Sybase);
            connSybaseK = dbhK.GetConnection(ConstrK);

            dbNewOA = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.SqlClient);
            connSybaseNewOA = dbNewOA.GetConnection(ConstrNewOA);


            dbCRM = Hanbell.DBUtility.DbHelperManager.CreateDbHelper(Hanbell.DBUtility.DbHelperType.SqlClient);
            connSybaseCRM = dbCRM.GetConnection(ConstrCRM);

            this.SetWarehNot();

        }
        #endregion

        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="date"></param>
        //public void StartRun(int year, int month)
        //{
        //    //SSS();
        //    InitDtbos(year, month);
        //}

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
            node = doc.DocumentElement.SelectSingleNode("C4wareh");
            this.C4WarehNot = node.InnerText;
            node = doc.DocumentElement.SelectSingleNode("Cmannot");
            this.CmanWarehNot = node.InnerText;
            node = doc.DocumentElement.SelectSingleNode("Gmannot");
            this.GmanWarehNot = node.InnerText;
            node = doc.DocumentElement.SelectSingleNode("C4mannot");
            this.C4manWarehNot = node.InnerText;
            node = doc.DocumentElement.SelectSingleNode("Nmannot");
            this.NmanWarehNot = node.InnerText;
            node = doc.DocumentElement.SelectSingleNode("Jmannot");
            this.JmanWarehNot = node.InnerText;
        }

        public static int Year { get; set; }
        public static int Month { get; set; }


        public System.Data.DataTable DtMaster { get; set; }


        /// <summary>
        /// 初始化bos表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public void InitDtbos()
        {
            //System.Data.DataTable Dtbos = this.BuildDtbos(year, month);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("no", typeof(string));
            dt.Columns.Add("amts", typeof(string));
            dt.Columns.Add("cpb", typeof(string));
            dt.Columns.Add("qyb", typeof(string));
            this.DtMsg = dt;

            System.Data.DataTable Dtbos = BuildDtbos(Year, Month);
            try
            {

                ////服务免费1001-1002   客诉领料-客诉退料外
                this.GetCInvtrn_1001_1002(Year, Month, Dtbos);
                this.GetGInvtrn_1001_1002(Year, Month, Dtbos);
                this.GetC4Invtrn_1001_1002(Year, Month, Dtbos);//增加重庆
                this.GetJInvtrn_1001_1002(Year, Month, Dtbos);
                this.GetNInvtrn_1001_1002(Year, Month, Dtbos);
                this.GetKInvtrn_1001_1002(Year, Month, Dtbos);//柯茂
                ///////维修免费1003-1004   拆修免费领料-拆修退料内
                this.GetCInvtrn_1003_1004(Year, Month, Dtbos);
                this.GetGInvtrn_1003_1004(Year, Month, Dtbos);
                this.GetC4Invtrn_1003_1004(Year, Month, Dtbos);//增加重庆
                this.GetJInvtrn_1003_1004(Year, Month, Dtbos);
                this.GetNInvtrn_1003_1004(Year, Month, Dtbos);
                this.GetInvtrn_K01_K14(Year, Month, Dtbos);

                ///////质量扣款逻辑加入20150901
                this.GetCutMoney(Year, Month, Dtbos);

                ////重工制令
                this.GetInvtrn_02(Year, Month, Dtbos);

                //////销售折让CDR310逻辑加入20150911
                this.GetDiscount(Year, Month, Dtbos);


                this.GetCLF(Year, Month, Dtbos);//差旅外
                this.GetKDF(Year, Month, Dtbos);//快递
                this.GetWLF(Year, Month, Dtbos);//物流
                //this.GetInv310_IAK(Year, Month, Dtbos);//借入归还单运费

                DtMaster = Dtbos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 更新 数据库表
        /// </summary>
        public void UpdateTable()
        {
            InsertBSC(DtMaster, "bsc_tb_mfservice");
        }

        /// <summary>
        /// 向数据库插入映射表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        private void InsertBSC(System.Data.DataTable dt, string tableName)
        {
            List<string> list = new List<string>();

            foreach (System.Data.DataRow row in dt.Rows)
            {
                StringBuilder sb = new StringBuilder("");
                sb.AppendFormat("insert into {0}(servicetype,year,month,protype,areatype,innertarget,outtarget,mancost,wxll,fwll,travelcost,fare,zlkk,xszr) values(", tableName);
                sb.AppendFormat("{0},{1},{2},'{3}','{4}',{5},{6},{7},{8},{9},{10},{11},{12},{13})"
                                    , row[0].ToString(), row[1].ToString(), row[2].ToString()
                                    , row[3].ToString(), row[4].ToString(), row[5].ToString()
                                    , row[6].ToString(), row[7].ToString(), row[8].ToString()
                                    , row[9].ToString(), row[10].ToString(), row[11].ToString()
                                    , row[12].ToString(), row[13].ToString());
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

        #region 服务免费



        #region 服务领料


        /// <summary>
        /// CM逻辑
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="Dtbos"></param>
        private void GetInvtrn_K01_K14(int year, int month, System.Data.DataTable Dtbos)
        {
            #region
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAA','IAB'", "C", trdate, "'K01','K14'", this.CWarehNot);//客诉领料1001,客诉退料1002,IAF服务领料单IAG服务退料
            //给dt中的产品别，区域别赋值

            if (dtIAF_1001 != null)
            {
                #region
                //                StringBuilder sb = new StringBuilder("");
                //                string sqlwhere = string.Empty;
                //                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                //                {
                //                    //得到交易单号
                //                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();

                //                    //根据交易单号得到对应INV310单据中的客服单号
                //                    if (string.IsNullOrEmpty(trno))
                //                    {
                //                        continue;
                //                    }
                //                    sb.Append("'" + trno + "',");
                //                }
                //                if (sb.Length > 0)
                //                {
                //                    sqlwhere = sb.ToString().Substring(0, sb.ToString().Length - 1);//得到in('','')条件
                //                }
                //                System.Data.DataTable dt = dbhC.Query(connSybaseC, string.Format(@"select trno,kfno,fwno,fwllno,sourceno,wxno,case hmark2 when 'R' then hmark1 else 'HD' end hmark1,hmark2 
                //                                                                                        from invhadh where facno='C' and prono='1' 
                //                                                                                        and trno in ({0})", sqlwhere));
                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        #region //将dt中维护客服单号的单据区域别变更为OA中客服单号对应的区域别

                //        string sql = string.Empty;
                //        System.Data.DataTable dt_ = null;
                //        string kfdh = string.Empty;
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            sql = "select resak029 from hzkfd,resak where resak001=hzkfd.hzkfd005 and hzkfd003='{0}'";
                //            kfdh = dt.Rows[i]["kfno"].ToString();
                //            if (string.IsNullOrEmpty(kfdh) || kfdh.Length != 14)
                //            {
                //                continue;
                //            }
                //            kfdh = kfdh.Substring(0, 14);
                //            sql = string.Format(sql, kfdh);
                //            dt_ = sqldbh.Query(connSql, sql);
                //            if (dt_ != null)
                //            {
                //                if (dt_.Rows.Count > 0)
                //                {
                //                    if (string.IsNullOrEmpty(dt_.Rows[0][0].ToString()) == false)
                //                    {
                //                        if (dt.Rows[i]["hmark2"].ToString() == "R")
                //                        {
                //                            dt.Rows[i]["hmark1"] = dt_.Rows[0][0].ToString().Substring(0, 2);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        #endregion
                //        for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                //        {
                //            for (int j = 0; j < dt.Rows.Count; j++)
                //            {
                //                if (dtIAF_1001.Rows[i]["trno"].ToString() == dt.Rows[j]["trno"].ToString())
                //                {
                //                    dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[j]["hmark2"].ToString();
                //                    dtIAF_1001.Rows[i]["qybie"] = dt.Rows[j]["hmark1"].ToString();
                //                }
                //            }
                //        }
                //        #region 明细日志
                //        //if (IsWritemsg == 1)
                //        //{
                //        //    foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                //        //    {
                //        //        Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString()+"\n", "fwll.txt");
                //        //    }
                //        //}
                //        #endregion

                //    }
                //}
                #endregion

                #region Dtbos赋值
                DTHelp(dtIAF_1001, Dtbos, "wxll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        string cpbie1 = dtIAF_1001.Rows[j]["cpbie"].ToString();
                //        string cpbie2 = Dtbos.Rows[i]["protype"].ToString();
                //        string qubie1 = dtIAF_1001.Rows[j]["qybie"].ToString();
                //        string qubie2 = Dtbos.Rows[i]["areatype"].ToString();
                //        if (cpbie1 == cpbie2 && qubie1 == qubie2)//如果产品别，区域别相等
                //        {
                //            Dtbos.Rows[i]["wxll"] = Convert.ToDouble(Dtbos.Rows[i]["wxll"] == DBNull.Value ? "0" : Dtbos.Rows[i]["wxll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"] == DBNull.Value ? "0" : dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}


                #region 明细日志
                if (IsWritemsg == 1)
                {
                    foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                    {
                        //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                        System.Data.DataRow r = this.DtMsg.NewRow();
                        r[0] = row[0];
                        r[1] = row[1];
                        r[2] = row[2];
                        r[3] = row[3];
                        DtMsg.Rows.Add(r.ItemArray);
                    }
                }
                #endregion

                #endregion
            }

            #endregion

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="Dtbos"></param>
        /// <returns></returns>
        private void GetCInvtrn_1001_1002(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF，IAG
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAF','IAG','IAB'", "C", trdate, "'1001','1002','1013','1014','Z08'", this.CWarehNot);//客诉领料1001,客诉退料1002,IAF服务领料单IAG服务退料
            //给dt中的产品别，区域别赋值

            if (dtIAF_1001 != null)
            {
                StringBuilder sb = new StringBuilder("");
                string sqlwhere = string.Empty;
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();

                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    sb.Append("'" + trno + "',");
                }
                System.Data.DataTable dt = new System.Data.DataTable();
                if (sb.Length > 0)
                {
                    sqlwhere = sb.ToString().Substring(0, sb.ToString().Length - 1);//得到in('','')条件
                    dt = dbhC.Query(connSybaseC, string.Format(@"select trno,kfno,fwno,fwllno,sourceno,wxno,case hmark2 when 'R' then hmark1 else 'HD' end hmark1,hmark2 
                                                                                        from invhadh where facno='C' and prono='1' 
                                                                                        and trno in ({0})", sqlwhere));
                }
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        #region //将dt中维护客服单号的单据区域别变更为OA中客服单号对应的区域别

                        string sql = string.Empty;
                        System.Data.DataTable dt_ = null;
                        string kfdh = string.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sql = "select resak029 from hzkfd,resak where resak001=hzkfd.hzkfd005 and hzkfd003='{0}'";
                            kfdh = dt.Rows[i]["kfno"].ToString();
                            if (string.IsNullOrEmpty(kfdh) || kfdh.Length != 14)
                            {
                                continue;
                            }
                            kfdh = kfdh.Substring(0, 14);
                            sql = string.Format(sql, kfdh);
                            dt_ = sqldbh.Query(connSql, sql);
                            if (dt_ != null)
                            {
                                if (dt_.Rows.Count > 0)
                                {
                                    if (string.IsNullOrEmpty(dt_.Rows[0][0].ToString()) == false)
                                    {
                                        if (dt.Rows[i]["hmark2"].ToString() == "R")
                                        {
                                            dt.Rows[i]["hmark1"] = dt_.Rows[0][0].ToString().Substring(0, 2);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (dtIAF_1001.Rows[i]["trno"].ToString() == dt.Rows[j]["trno"].ToString())
                                {
                                    dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[j]["hmark2"].ToString();
                                    dtIAF_1001.Rows[i]["qybie"] = dt.Rows[j]["hmark1"].ToString();
                                }
                            }
                        }
                        #region 明细日志
                        if (IsWritemsg == 1)
                        {
                            foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                            {
                                //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                                System.Data.DataRow r = this.DtMsg.NewRow();
                                r[0] = row[0].ToString();
                                r[1] = row[1].ToString();
                                r[2] = row[2].ToString();
                                r[3] = row[3].ToString();
                                DtMsg.Rows.Add(r.ItemArray);

                            }
                        }
                        #endregion

                    }
                }
                #region Dtbos赋值

                DTHelp(dtIAF_1001, Dtbos, "fwll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        string cpbie1 = dtIAF_1001.Rows[j]["cpbie"].ToString();
                //        string cpbie2 = Dtbos.Rows[i]["protype"].ToString();
                //        string qubie1 = dtIAF_1001.Rows[j]["qybie"].ToString();
                //        string qubie2 = Dtbos.Rows[i]["areatype"].ToString();
                //        if (cpbie1 == cpbie2 && qubie1 == qubie2)//如果产品别，区域别相等
                //        {
                //            Dtbos.Rows[i]["fwll"] = Convert.ToDouble(Dtbos.Rows[i]["fwll"] == DBNull.Value ? "0" : Dtbos.Rows[i]["fwll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"] == DBNull.Value ? "0" : dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 广州
        /// </summary>
        /// <returns></returns>
        private void GetGInvtrn_1001_1002(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF,IAG
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAF','IAG'", "G", trdate, "'1001','1002','1013','1014'", this.GWarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {
                StringBuilder sb = new StringBuilder("");
                string sqlwhere = string.Empty;
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();

                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    sb.Append("'" + trno + "',");
                }
                System.Data.DataTable dt = new System.Data.DataTable();
                if (sb.Length > 0)
                {
                    sqlwhere = sb.ToString().Substring(0, sb.ToString().Length - 1);//得到in('','')条件
                    dt = dbhG.Query(connSybaseG, string.Format(@"select trno,kfno,fwno,fwllno,sourceno,wxno,case hmark2 when 'R' then hmark1 else 'HD' end hmark1,hmark2 
                                                                                        from invhadh where facno='G' and prono='1' 
                                                                                        and trno in ({0})", sqlwhere));
                }
                if (dt.Rows.Count > 0)
                {
                    #region //将dt中维护客服单号的单据区域别变更为OA中客服单号对应的区域别


                    System.Data.DataTable dt_ = null;
                    string kfdh = string.Empty;
                    string sql = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "select resak029 from hzkfd,resak where resak001=hzkfd.hzkfd005 and hzkfd003='{0}'";
                        kfdh = dt.Rows[i]["kfno"].ToString();
                        if (string.IsNullOrEmpty(kfdh) || kfdh.Length != 14)
                        {
                            continue;
                        }
                        kfdh = kfdh.Substring(0, 14);
                        sql = string.Format(sql, kfdh);
                        dt_ = sqldbh.Query(connSql, sql);
                        if (dt_ != null)
                        {
                            if (dt_.Rows.Count > 0)
                            {
                                if (string.IsNullOrEmpty(dt_.Rows[0][0].ToString()) == false)
                                {
                                    if (dt.Rows[i]["hmark2"].ToString() == "R")
                                    {
                                        dt.Rows[i]["hmark1"] = dt_.Rows[0][0].ToString().Substring(0, 2);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dtIAF_1001.Rows[i]["trno"].ToString() == dt.Rows[j]["trno"].ToString())
                            {
                                dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[j]["hmark2"].ToString();
                                dtIAF_1001.Rows[i]["qybie"] = dt.Rows[j]["hmark1"].ToString();
                            }
                        }
                    }

                    #region 明细日志
                    if (IsWritemsg == 1)
                    {
                        foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                        {
                            //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                            System.Data.DataRow r = this.DtMsg.NewRow();
                            r[0] = row[0];
                            r[1] = row[1];
                            r[2] = row[2];
                            r[3] = row[3];
                            DtMsg.Rows.Add(r.ItemArray);
                        }
                    }
                    #endregion

                }
                //给dt中的产品别，区域别赋值
                #region Dtbos
                DTHelp(dtIAF_1001, Dtbos, "fwll");


                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["fwll"] = Convert.ToDouble(Dtbos.Rows[i]["fwll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }

            #endregion
        }


        /// <summary>
        /// 重庆
        /// </summary>
        /// <returns></returns>
        private void GetC4Invtrn_1001_1002(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF,IAG
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAF','IAG'", "C4", trdate, "'1001','1002','1013','1014'", this.C4WarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {
                StringBuilder sb = new StringBuilder("");
                string sqlwhere = string.Empty;
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();

                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    sb.Append("'" + trno + "',");
                }
                System.Data.DataTable dt = new System.Data.DataTable();
                if (sb.Length > 0)
                {
                    sqlwhere = sb.ToString().Substring(0, sb.ToString().Length - 1);//得到in('','')条件
                    dt = dbhC4.Query(connSybaseC4, string.Format(@"select trno,kfno,fwno,fwllno,sourceno,wxno,case hmark2 when 'R' then hmark1 else 'HD' end hmark1,hmark2 
                                                                                        from invhadh where facno='C4' and prono='1' 
                                                                                        and trno in ({0})", sqlwhere));
                }
                if (dt.Rows.Count > 0)
                {
                    #region //将dt中维护客服单号的单据区域别变更为OA中客服单号对应的区域别


                    System.Data.DataTable dt_ = null;
                    string kfdh = string.Empty;
                    string sql = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "select resak029 from hzkfd,resak where resak001=hzkfd.hzkfd005 and hzkfd003='{0}'";
                        kfdh = dt.Rows[i]["kfno"].ToString();
                        if (string.IsNullOrEmpty(kfdh) || kfdh.Length != 14)
                        {
                            continue;
                        }
                        kfdh = kfdh.Substring(0, 14);
                        sql = string.Format(sql, kfdh);
                        dt_ = sqldbh.Query(connSql, sql);
                        if (dt_ != null)
                        {
                            if (dt_.Rows.Count > 0)
                            {
                                if (string.IsNullOrEmpty(dt_.Rows[0][0].ToString()) == false)
                                {
                                    if (dt.Rows[i]["hmark2"].ToString() == "R")
                                    {
                                        dt.Rows[i]["hmark1"] = dt_.Rows[0][0].ToString().Substring(0, 2);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dtIAF_1001.Rows[i]["trno"].ToString() == dt.Rows[j]["trno"].ToString())
                            {
                                dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[j]["hmark2"].ToString();
                                dtIAF_1001.Rows[i]["qybie"] = dt.Rows[j]["hmark1"].ToString();
                            }
                        }
                    }

                    #region 明细日志
                    if (IsWritemsg == 1)
                    {
                        foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                        {
                            //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                            System.Data.DataRow r = this.DtMsg.NewRow();
                            r[0] = row[0];
                            r[1] = row[1];
                            r[2] = row[2];
                            r[3] = row[3];
                            DtMsg.Rows.Add(r.ItemArray);
                        }
                    }
                    #endregion

                }
                //给dt中的产品别，区域别赋值
                #region Dtbos
                DTHelp(dtIAF_1001, Dtbos, "fwll");


                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["fwll"] = Convert.ToDouble(Dtbos.Rows[i]["fwll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 济南
        /// </summary>
        /// <returns></returns>
        private void GetJInvtrn_1001_1002(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF,IAG
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAF','IAG'", "J", trdate, "'1001','1002','1013','1014'", this.JWarehNot);//客诉领料1001,IAF服务领料单
            //给dt中的产品别，区域别赋值
            if (dtIAF_1001 != null)
            {
                StringBuilder sb = new StringBuilder("");
                string sqlwhere = string.Empty;
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();

                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    sb.Append("'" + trno + "',");
                }
                System.Data.DataTable dt = new System.Data.DataTable();
                if (sb.Length > 0)
                {
                    sqlwhere = sb.ToString().Substring(0, sb.ToString().Length - 1);//得到in('','')条件
                    dt = dbhJ.Query(connSybaseJ, string.Format(@"select trno,kfno,fwno,fwllno,sourceno,wxno, case hmark2 when 'R' then hmark1 else 'HD' end hmark1,hmark2 
                                                                                        from invhadh where facno='J' and prono='1' 
                                                                                        and trno in ({0})", sqlwhere));
                }
                if (dt.Rows.Count > 0)
                {
                    #region //将dt中维护客服单号的单据区域别变更为OA中客服单号对应的区域别

                    string sql = string.Empty;
                    System.Data.DataTable dt_ = null;
                    string kfdh = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "select resak029 from hzkfd,resak where resak001=hzkfd.hzkfd005 and hzkfd003='{0}'";
                        kfdh = dt.Rows[i]["kfno"].ToString();
                        if (string.IsNullOrEmpty(kfdh) || kfdh.Length != 14)
                        {
                            continue;
                        }
                        kfdh = kfdh.Substring(0, 14);
                        sql = string.Format(sql, kfdh);
                        dt_ = sqldbh.Query(connSql, sql);
                        if (dt_ != null)
                        {
                            if (dt_.Rows.Count > 0)
                            {
                                if (string.IsNullOrEmpty(dt_.Rows[0][0].ToString()) == false)
                                {
                                    if (dt.Rows[i]["hmark2"].ToString() == "R")
                                    {
                                        dt.Rows[i]["hmark1"] = dt_.Rows[0][0].ToString().Substring(0, 2);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dtIAF_1001.Rows[i]["trno"].ToString() == dt.Rows[j]["trno"].ToString())
                            {
                                dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[j]["hmark2"].ToString();
                                dtIAF_1001.Rows[i]["qybie"] = dt.Rows[j]["hmark1"].ToString();
                            }
                        }
                    }


                    #region 明细日志
                    if (IsWritemsg == 1)
                    {
                        foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                        {
                            //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                            System.Data.DataRow r = this.DtMsg.NewRow();
                            r[0] = row[0];
                            r[1] = row[1];
                            r[2] = row[2];
                            r[3] = row[3];
                            DtMsg.Rows.Add(r.ItemArray);
                        }
                    }
                    #endregion

                }
                #region Dtbos
                DTHelp(dtIAF_1001, Dtbos, "fwll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["fwll"] = Convert.ToDouble(Dtbos.Rows[i]["fwll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }
            #endregion
        }



        /// <summary>
        /// 南京
        /// </summary>
        /// <returns></returns>
        private void GetNInvtrn_1001_1002(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF,IAG
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAF','IAG'", "N", trdate, "'1001','1002','1013','1014'", this.NWarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {
                StringBuilder sb = new StringBuilder("");
                string sqlwhere = string.Empty;
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();

                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    sb.Append("'" + trno + "',");
                }
                System.Data.DataTable dt = new System.Data.DataTable(); ;
                if (sb.Length > 0)
                {
                    sqlwhere = sb.ToString().Substring(0, sb.ToString().Length - 1);//得到in('','')条件
                    dt = dbhN.Query(connSybaseN, string.Format(@"select trno,kfno,fwno,fwllno,sourceno,wxno,case hmark2 when 'R' then hmark1 else 'HD' end hmark1,hmark2 
                                                                                        from invhadh where facno='N' and prono='1' 
                                                                                        and trno in ({0})", sqlwhere));
                }
                if (dt.Rows.Count > 0)
                {
                    #region //将dt中维护客服单号的单据区域别变更为OA中客服单号对应的区域别

                    string sql = string.Empty;
                    System.Data.DataTable dt_ = null;
                    string kfdh = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "select resak029 from hzkfd,resak where resak001=hzkfd.hzkfd005 and hzkfd003='{0}'";
                        kfdh = dt.Rows[i]["kfno"].ToString();
                        if (string.IsNullOrEmpty(kfdh) || kfdh.Length != 14)
                        {
                            continue;
                        }
                        kfdh = kfdh.Substring(0, 14);
                        sql = string.Format(sql, kfdh);
                        dt_ = sqldbh.Query(connSql, sql);
                        if (dt_ != null)
                        {
                            if (dt_.Rows.Count > 0)
                            {
                                if (string.IsNullOrEmpty(dt_.Rows[0][0].ToString()) == false)
                                {
                                    if (dt.Rows[i]["hmark2"].ToString() == "R")
                                    {
                                        dt.Rows[i]["hmark1"] = dt_.Rows[0][0].ToString().Substring(0, 2);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dtIAF_1001.Rows[i]["trno"].ToString() == dt.Rows[j]["trno"].ToString())
                            {
                                dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[j]["hmark2"].ToString();
                                dtIAF_1001.Rows[i]["qybie"] = dt.Rows[j]["hmark1"].ToString();
                            }
                        }
                    }



                    #region 明细日志
                    if (IsWritemsg == 1)
                    {
                        foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                        {
                            //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                            System.Data.DataRow r = this.DtMsg.NewRow();
                            r[0] = row[0];
                            r[1] = row[1];
                            r[2] = row[2];
                            r[3] = row[3];
                            DtMsg.Rows.Add(r.ItemArray);
                        }
                    }
                    #endregion

                }
                #region Dtbos
                DTHelp(dtIAF_1001, Dtbos, "fwll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["fwll"] = Convert.ToDouble(Dtbos.Rows[i]["fwll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 柯茂
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="Dtbos"></param>
        private void GetKInvtrn_1001_1002(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF,IAG
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            string sqli = string.Format(@"SELECT invtrnh.trno,sum(case invtrnh.iocode when '1' then -invtrnh.tramt else invtrnh.tramt end) cost,  
                                '' cpbie,
                                '' qybie  
                                FROM invtrnh ,           
                                invmas ,           
                                invwh ,           
                                invdou ,           
                                invcls     
                                WHERE ( invmas.itnbr = invtrnh.itnbr ) and          
                                ( invtrnh.facno = invwh.facno ) and          
                                ( invtrnh.prono = invwh.prono ) and          
                                ( invtrnh.wareh = invwh.wareh ) and          
                                ( invdou.trtype = invtrnh.trtype ) and          
                                ( invcls.itcls = invmas.itcls ) and          
                                ( ( invdou.syscode = '10'  ) And          
                                (invdou.reskind is not null and          
                                ( ltrim(invdou.reskind) <> '') ) And          
                                (invdou.iocode in ('1','2') or          
                                ( invdou.iocode = '3'  and invdou.trtype in ('IAF','IAG')))
                                and ( invwh.costyn = 'Y' ) )   
                                AND (invtrnh.facno='K' and invtrnh.prono='1' and invdou.depdsckind = 'CA')
                                and convert(varchar(6),invtrnh.trdate,112)='{0}'
                                group by invtrnh.trno", trdate);
            System.Data.DataTable dtIAF_1001 = dbhK.Query(connSybaseK, sqli);
            //System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAF','IAG'", "K", trdate, "'1001','1002'", this.NWarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {
                StringBuilder sb = new StringBuilder("");
                string sqlwhere = string.Empty;
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();

                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    sb.Append("'" + trno + "',");
                }
                System.Data.DataTable dt = new System.Data.DataTable(); ;
                if (sb.Length > 0)
                {
                    sqlwhere = sb.ToString().Substring(0, sb.ToString().Length - 1);//得到in('','')条件
                    dt = dbhK.Query(connSybaseK, string.Format(@"select trno,kfno,fwno,fwllno,sourceno,wxno,case hmark2 when 'R' then hmark1 else 'HD' end hmark1,hmark2 
                                                                                        from invhadh where facno='K' and prono='1' 
                                                                                        and trno in ({0})", sqlwhere));
                }
                if (dt.Rows.Count > 0)
                {
                    #region //将dt中维护客服单号的单据区域别变更为OA中客服单号对应的区域别

                    string sql = string.Empty;
                    System.Data.DataTable dt_ = null;
                    string kfdh = string.Empty;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sql = "select resak029 from hzkfd,resak where resak001=hzkfd.hzkfd005 and hzkfd003='{0}'";
                        kfdh = dt.Rows[i]["kfno"].ToString();
                        if (string.IsNullOrEmpty(kfdh) || kfdh.Length != 14)
                        {
                            continue;
                        }
                        kfdh = kfdh.Substring(0, 14);
                        sql = string.Format(sql, kfdh);
                        dt_ = sqldbh.Query(connSql, sql);
                        if (dt_ != null)
                        {
                            if (dt_.Rows.Count > 0)
                            {
                                if (string.IsNullOrEmpty(dt_.Rows[0][0].ToString()) == false)
                                {
                                    if (dt.Rows[i]["hmark2"].ToString() == "R")
                                    {
                                        dt.Rows[i]["hmark1"] = dt_.Rows[0][0].ToString().Substring(0, 2);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dtIAF_1001.Rows[i]["trno"].ToString() == dt.Rows[j]["trno"].ToString())
                            {
                                //dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[j]["hmark2"].ToString();
                                dtIAF_1001.Rows[i]["cpbie"] = "KM";//柯茂产品别是CM，报表中需要KM以区分总公司CM，应该理解为事业部
                                dtIAF_1001.Rows[i]["qybie"] = dt.Rows[j]["hmark1"].ToString();
                            }
                        }
                    }



                    #region 明细日志
                    if (IsWritemsg == 1)
                    {
                        foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                        {
                            //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                            System.Data.DataRow r = this.DtMsg.NewRow();
                            r[0] = row[0];
                            r[1] = row[1];
                            r[2] = row[2];
                            r[3] = row[3];
                            DtMsg.Rows.Add(r.ItemArray);
                        }
                    }
                    #endregion

                }
                #region Dtbos
                DTHelp(dtIAF_1001, Dtbos, "fwll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["fwll"] = Convert.ToDouble(Dtbos.Rows[i]["fwll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }

            #endregion
        }
        #endregion


        #region 交易明细辅助方法

        /// <summary>
        /// 交易明细，去除整机
        /// </summary>
        /// <param name="trtype">单据类别</param>
        /// <param name="facno">公司别</param>
        /// <param name="trddate">交易日期</param>
        /// <param name="rescode">交易原因</param>
        /// <returns></returns>
        private System.Data.DataTable GetInvtrnhTable(string trtype, string facno, string trddate, string rescode, string warehnot)
        {
            System.Data.DataTable dt = null;
            string sql = string.Format(@"select	b.trno,			                                   
                                               	sum(case b.iocode when '1' then -b.tramt else b.tramt end) cost,
                                               	'' cpbie,
												'' qybie
   	                                    FROM invmas  a  left outer join invcls d on a.itcls=d.itcls,invtrnh b 
   	                                    left outer join invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
   	                                    left outer join invdou e on e.trtype=b.trtype  
   	                                    left outer join invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
   	                                    WHERE ( b.itnbr = a.itnbr )
														
		                                    and b.facno='{0}' 
		                                    and b.prono='1' and c.wareh not in ({4})
                                            and b.rescode in ({1})
		                                    and b.trtype in ({2}) 
		                                    and convert(varchar(6),trdate,112)='{3}' 
										group by b.trno order by b.trno", facno, rescode, trtype, trddate, warehnot);
            //去处制冷营销的单据逻辑，2015年统计到CM中----待定20150728





            if (trtype == "'IAA','IAB'")
            {
                sql = string.Format(@"select	b.trno,			                                   
                                               	sum(case b.iocode when '1' then -b.tramt else b.tramt end) cost,
                                               	'CM' cpbie,
												'HD' qybie
   	                                    FROM invmas  a  left outer join invcls d on a.itcls=d.itcls,invtrnh b 
   	                                    left outer join invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
   	                                    left outer join invdou e on e.trtype=b.trtype  
   	                                    left outer join invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
   	                                    WHERE ( b.itnbr = a.itnbr )
		                                    and b.facno='{0}' 
		                                    and b.prono='1' 
											AND b.depno='RTFW'
                                            and c.wareh not in ({4})
                                            and b.rescode in ({1})
		                                    and b.trtype in ({2}) 
		                                    and convert(varchar(6),trdate,112)='{3}' 
										group by b.trno order by b.trno", facno, rescode, trtype, trddate, warehnot);
            }

            try
            {
                switch (facno)
                {
                    case "C":
                        dt = dbhC.Query(connSybaseC, sql);
                        break;
                    case "G":
                        dt = dbhG.Query(connSybaseG, sql);
                        break;
                    case "C4":
                        dt = dbhC4.Query(connSybaseC4, sql);
                        break;
                    case "J":
                        dt = dbhJ.Query(connSybaseJ, sql);
                        break;
                    case "N":
                        dt = dbhN.Query(connSybaseN, sql);
                        break;
                    case "K":
                        dt = dbhK.Query(connSybaseK, sql);
                        break;
                    default:
                        break;
                }

                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;

        }

        #endregion


        #region 差旅

        private void GetCLF2(int year, int month, System.Data.DataTable Dtbos)
        {
            System.Data.DataTable dtfwusers = this.XmlToDt();//得到xml所有相关服务人员
            GetCLF(year, month, "C", dtfwusers, Dtbos);
            GetCLF(year, month, "G", dtfwusers, Dtbos);
            GetCLF(year, month, "N", dtfwusers, Dtbos);
            GetCLF(year, month, "J", dtfwusers, Dtbos);


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="facno"></param>
        /// <returns></returns>
        private void GetCLF(int year, int month, string facno, System.Data.DataTable dtfwusers, System.Data.DataTable Dtbos)
        {
            DateTime tms = new DateTime(year, month, 1);//月第一天
            DateTime tme = tms.AddMonths(1).AddDays(-1);//月最后一天
            string sql = string.Format(@"SELECT  acctrn.cramt,acctrn.detno,'' qybie,'' cpbie
                                            FROM accnam,accclass,accdan,acctrn,acchkno  
                                           WHERE ( acchkno.facno = acctrn.facno ) and  
                                                 ( acchkno.vouda = acctrn.vouda ) and  
                                                 ( acchkno.vouno = acctrn.vouno ) and  
                                                 ( accnam.accno = accclass.accno ) and  
                                                 ( accclass.accno = accdan.accno ) and  
                                                 ( accdan.accno = acctrn.accno ) and  
                                                 ( accdan.detno = acctrn.detno )  
                                        and acctrn.vouno in 
                                        (
 		                                        SELECT  acctrn.vouno
    	                                        FROM    accnam,accclass,accdan,acctrn,acchkno  
   	                                        WHERE ( acchkno.facno = acctrn.facno )   
                                                 and( acchkno.vouda = acctrn.vouda )   
                                                 and( acchkno.vouno = acctrn.vouno )   
                                                 and( accnam.accno = accclass.accno )   
                                                 and( accclass.accno = accdan.accno )   
                                                 and( accdan.accno = acctrn.accno ) 
			                                        and( accdan.detno = acctrn.detno )   
			                                        and ckind='GE' and accnam.accno='6617'
 			                                        AND ( acchkno.yeavoi = 'P' and acctrn.vouda >= '{0}' and acctrn.vouda <= '{1}' and acctrn.facno = '{2}' and accclass.ckind ='GE')
                                        )
                                        and (acchkno.yeavoi = 'P' and acctrn.vouda >= '{0}' and acctrn.vouda <= '{1}' and acctrn.facno = '{2}' 
                                        and accclass.ckind ='9E') ", tms.ToString("yyyyMMdd"), tme.ToString("yyyyMMdd"), facno);

            System.Data.DataTable dt = null;
            string qybie = string.Empty;
            string cpbie = string.Empty;
            try
            {
                switch (facno)
                {
                    case "C":
                        dt = dbhC.Query(connSybaseC, sql);
                        break;
                    case "G":
                        dt = dbhG.Query(connSybaseG, sql);
                        break;
                    case "N":
                        dt = dbhN.Query(connSybaseN, sql);
                        break;
                    case "J":
                        dt = dbhJ.Query(connSybaseJ, sql);
                        break;
                    default:
                        dt = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string userno = dt.Rows[i]["detno"].ToString();
                    for (int j = 0; j < dtfwusers.Rows.Count; j++)
                    {
                        if (dtfwusers.Rows[j]["no"].ToString() == userno)
                        {
                            dt.Rows[i]["cpbie"] = dtfwusers.Rows[j]["cpbie"];
                            dt.Rows[i]["qybie"] = dtfwusers.Rows[j]["qybie"];
                        }
                    }
                }
                for (int i = 0; i < Dtbos.Rows.Count; i++)
                {
                    cpbie = Dtbos.Rows[i]["protype"].ToString();
                    qybie = Dtbos.Rows[i]["areatype"].ToString();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (cpbie == dt.Rows[j]["cpbie"].ToString() && qybie == dt.Rows[j]["qybie"].ToString())
                        {
                            Dtbos.Rows[i]["travelcost"] = Convert.ToDouble(Dtbos.Rows[i]["travelcost"] == DBNull.Value ? "0" : Dtbos.Rows[i]["travelcost"]) + Convert.ToDouble(dt.Rows[j]["cramt"] == DBNull.Value ? "0" : dt.Rows[j]["cramt"]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private System.Data.DataTable XmlToDt()
        {
            System.Data.DataTable dt = null;
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "\\config\\common\\NotWareh.xml");
            XmlNodeList nodelist = doc.DocumentElement.SelectNodes("userno");
            if (nodelist.Count > 0)
            {
                dt = new System.Data.DataTable();
                dt.Columns.Add("facno", typeof(string));
                dt.Columns.Add("qybie", typeof(string));
                dt.Columns.Add("cpbie", typeof(string));
                dt.Columns.Add("no", typeof(string));
                System.Data.DataRow row = null;
                foreach (XmlNode node in nodelist)
                {
                    row = dt.NewRow();
                    row["facno"] = node.Attributes["facno"].Value;
                    row["qybie"] = node.Attributes["qybie"].Value;
                    row["cpbie"] = node.Attributes["cpbie"].Value;
                    row["no"] = node.Attributes["no"].Value;
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }


        /// <summary>
        /// 根据结算日期得到差旅费
        /// </summary>
        /// <param name="trdate"></param>
        /// <returns></returns>
        private void GetCLF(int year, int month, System.Data.DataTable Dtbos)
        {
            //OA链接
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dt = this.GetBxd2(trdate, "2");//dt列：qybie,cpbie,cost,creator,fwdh
            System.Data.DataTable dt2 = this.GetJZGHD(trdate, "2");
            System.Data.DataTable dt3 = this.GetCRM(trdate);//CRM费用申请单
            if (dt != null)
            {
                for (int i = 0; i < Dtbos.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (Dtbos.Rows[i]["areatype"].ToString() == dt.Rows[j]["qybie"].ToString()
                            && Dtbos.Rows[i]["protype"].ToString() == dt.Rows[j]["cpbie"].ToString())//如果产品别，区域别，厂内外相等则赋值
                        {
                            Dtbos.Rows[i]["travelcost"] = Convert.ToDouble(Dtbos.Rows[i]["travelcost"] == DBNull.Value ? "0" : Dtbos.Rows[i]["travelcost"]) + Convert.ToDouble(dt.Rows[j]["cost"] == DBNull.Value ? "0" : dt.Rows[j]["cost"]);
                        }
                    }
                }
            }

            if (dt2 != null)
            {
                for (int i = 0; i < Dtbos.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        if (Dtbos.Rows[i]["areatype"].ToString() == dt2.Rows[j]["qybie"].ToString()
                            && Dtbos.Rows[i]["protype"].ToString() == dt2.Rows[j]["cpbie"].ToString())//如果产品别，区域别，厂内外相等则赋值
                        {
                            Dtbos.Rows[i]["travelcost"] = Convert.ToDouble(Dtbos.Rows[i]["travelcost"] == DBNull.Value ? "0" : Dtbos.Rows[i]["travelcost"]) + Convert.ToDouble(dt2.Rows[j]["cost"] == DBNull.Value ? "0" : dt2.Rows[j]["cost"]);
                        }
                    }
                }
            }


            if (dt3 != null)
            {
                for (int i = 0; i < Dtbos.Rows.Count; i++)
                {
                    for (int j = 0; j < dt3.Rows.Count; j++)
                    {
                        if (Dtbos.Rows[i]["areatype"].ToString() == dt3.Rows[j]["qybie"].ToString()
                            && Dtbos.Rows[i]["protype"].ToString() == dt3.Rows[j]["cpbie"].ToString())//如果产品别，区域别，厂内外相等则赋值
                        {
                            Dtbos.Rows[i]["travelcost"] = Convert.ToDouble(Dtbos.Rows[i]["travelcost"] == DBNull.Value ? "0" : Dtbos.Rows[i]["travelcost"]) + Convert.ToDouble(dt3.Rows[j]["cost"] == DBNull.Value ? "0" : dt3.Rows[j]["cost"]);
                        }
                    }
                }
            }

        }



        /// <summary>
        /// 根据结案状态及结案日期得到费用报销单相关
        /// </summary>
        /// <param name="resda019">结案日期</param>
        /// <param name="resda021">审核状态</param>
        /// <returns></returns>
        private System.Data.DataTable GetBxd2(string resda019, string resda021)
        {


            //            select bxd002_1002,bmpb20f,bmpa07c from bxd002_1,resda,bxd002
            //where resda002=bxd002_1.bxd002_1002 and bxd002.bxd002002=bxd002_1002 
            //and bmpb05c in ('6617','6717','5117','5309') 
            //and resda021='2' 
            //and MONTH(resda019)=7 and YEAR(resda019)=2014
            string sql = string.Format(@"select	case when a.resak015 like '1D5%' THEN 'CQ' 
		                                        when a.resak015 like '1D%' THEN 'HN'
		                                        WHEN a.resak015 like '1E%' THEN  'NJ'
		                                        WHEN a.resak015 like '1C%' THEN  'HB'
                                                when a.resak015 like '1V%' THEN 'CQ' 
		                                        ELSE 'HD' end qybie,
                                                    case b.select1 when 'AT' then 'AH' WHEN 'ATWL' THEN 'AH'
                                                    WHEN 'AZ' THEN 'AA' WHEN 'AZWL' THEN 'AA' 
                                                    WHEN 'P' THEN 'P' WHEN 'PWL' THEN 'P'
                                                    WHEN 'R' THEN 'R' WHEN 'RWL' THEN 'R'
                                                    WHEN 'RTZ' THEN 'KM' WHEN 'RTZWL' THEN 'KM' ELSE 'R' END as 'cpbie',
                                                    a.bmpb16c as 'cost',
                                                    a.CREATOR as 'creator',
                                                    a.fwdhd02 as 'fwdh'
                                            		
                                            from (
                                                    select	k.resak015,
		                                                    x.bmpb16c,
		                                                    x.fwdhd02,
		                                                    x.CREATOR 
                                                    from resak k right join 
                                                    (
	                                                    select	a.fwdhd02,
			                                                    a.bmpb16c,
			                                                    a.CREATOR
	                                                    from bxd002_2 a 
	                                                    left join resda b
	                                                    on a.bxd002_2002=b.resda002,bxd002
	                                                    where b.resda021='{0}' 
                                                        and bxd002.bxd002002=a.bxd002_2002
                                                        and bxd002.xz='1'
	                                                    and a.fwdhd02 is not null 
	                                                    and left(resda019,7)='{1}'
                                                        and a.fwxz='mf'
                                                    ) x on k.resak001=x.CREATOR
                                            ) a left join hzfwd b 
                                            on a.fwdhd02=b.hzfwd006 where a.fwdhd02<>''", resda021, resda019.Insert(4, "/"));//{0}审核状态 {1}结案日期

            System.Data.DataTable dt = null;
            try
            {
                dt = sqldbh.Query(connSql, sql);
                //if (dt.Rows.Count > 0)
                //{
                //    string fwdh = string.Empty;
                //    System.Data.DataTable dt2 = null;
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        fwdh = dt.Rows[i]["fwdh"].ToString();//服务单号
                //        //判断服务单号对应的客服单是否是免费的
                //        sql = string.Format("select hzkfjad009 from hzfwd,hzkfd left join hzkfjad on hzkfd.hzkfd003=hzkfjad.hzkfjad003 where hzfwd.hzfwd004=hzkfd.hzkfd003 and hzfwd006='{0}'", fwdh);
                //        dt2 = sqldbh.Query(connSql, sql);
                //        if (dt2 != null)
                //        {
                //            if (dt2.Rows.Count > 0)
                //            {
                //                string val = dt2.Rows[0][0].ToString();
                //                if (val == "客户原因收费" || val == "汉钟原因收费")
                //                {
                //                    dt.Rows.RemoveAt(i);
                //                    i--;
                //                }
                //            }
                //        }
                //    }
                //    #region 明细日志
                //    if (IsWritemsg == 4)
                //    {
                //        foreach (System.Data.DataRow row in dt.Rows)
                //        {
                //            Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\t" + row[4].ToString() + "\r\n", "fwll.txt");
                //        }
                //    }
                //    #endregion





                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }
        /// <summary>
        /// 借支归还单
        /// </summary>
        /// <param name="resda019"></param>
        /// <param name="resda021"></param>
        /// <returns></returns>
        private System.Data.DataTable GetJZGHD(string resda019, string resda021)
        {

            //            select jzghd002,bmhb20f,bmha07c from jzghd01,resda,jzghd 
            //where resda002=jzghd01.jzghd01002 and jzghd.jzghd002=jzghd01002 
            //and bmhb05c in ('6617','6717','5117','5309')
            //and resda021='2'
            //and MONTH(resda019)=7 and YEAR(resda019)=2014
            //            string sql = string.Format(@"select	case when a.resak015 like '1D5%' THEN 'CQ' 
            //		                                        when a.resak015 like '1D%' THEN 'HN'
            //		                                        WHEN a.resak015 like '1E%' THEN  'NJ'
            //		                                        WHEN a.resak015 like '1C%' THEN  'HB'
            //		                                        ELSE 'HD' end qybie,
            //                                                    case b.select1 when 'AT' then 'AH' WHEN 'ATWL' THEN 'AH'
            //                                                    WHEN 'AZ' THEN 'AA' WHEN 'AZWL' THEN 'AA' 
            //                                                    WHEN 'P' THEN 'P' WHEN 'PWL' THEN 'P'
            //                                                    WHEN 'R' THEN 'R' WHEN 'RWL' THEN 'R'
            //                                                    WHEN 'RTZ' THEN 'KM' WHEN 'RTZWL' THEN 'KM' ELSE 'R' END as 'cpbie',
            //                                                    a.bmhb14c as 'cost',
            //                                                    a.CREATOR as 'creator',
            //                                                    a.fwdhd02 as 'fwdh'
            //                                            		
            //                                            from (
            //                                                    select	k.resak015,
            //		                                                    x.bmhb14c,
            //		                                                    x.fwdhd02,
            //		                                                    x.CREATOR 
            //                                                    from resak k right join 
            //                                                    (
            //	                                                    select	a.fwdhd02,
            //			                                                    a.bmhb14c,
            //			                                                    a.CREATOR
            //	                                                    from jzghd02 a 
            //	                                                    left join resda b
            //	                                                    on a.jzghd02002=b.resda002,jzghd
            //	                                                    where b.resda021='{0}' 
            //                                                        and jzghd.jzghd002=a.jzghd02002
            //                                                        and jzghd.xz='1'
            //	                                                    and a.fwdhd02 is not null 
            //	                                                    and left(resda019,7)='{1}' and a.fwxz='mf'
            //                                                    ) x on k.resak001=x.CREATOR
            //                                            ) a left join hzfwd b 
            //                                            on a.fwdhd02=b.hzfwd006 where a.fwdhd02<>''", resda021, resda019.Insert(4, "/"));//{0}审核状态 {1}结案日期
            string sql = string.Format(@"select left(b.hzkfd008,2) as qybie, case a.select1 when 'AT' then 'AH' WHEN 'ATWL' THEN 'AH'
                                                    WHEN 'AZ' THEN 'AA' WHEN 'AZWL' THEN 'AA' 
                                                    WHEN 'P' THEN 'P' WHEN 'PWL' THEN 'P'
                                                    WHEN 'R' THEN 'R' WHEN 'RWL' THEN 'R'
                                                    WHEN 'RTZ' THEN 'KM' WHEN 'RTZWL' THEN 'KM' ELSE 'R' END as 'cpbie',a.hzfwd007 as 'cost',a.hzfwd005 as 'creator',a.hzfwd006 as 'fwdh'
                                from hzfwd a left join resda c on a.hzfwd002=c.resda002,hzkfd b
                                where c.resda021='{0}' 
                                and  a.hzfwd004=b.hzkfd003 
                                and b.hzkfd023 in ('HZMF','KHMF','KMKHMF','KMMF','ZRQT')
                                and left(a.paydate,7)='{1}'", resda021, resda019.Insert(4, "-"));//{0}审核状态 {1}结案日期
            System.Data.DataTable dt = null;
            try
            {
                dt = sqldbh.Query(connSql, sql);
                //if (dt.Rows.Count > 0)
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        string fwdh = string.Empty;
                //        System.Data.DataTable dt2 = null;
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            fwdh = dt.Rows[i]["fwdh"].ToString();//服务单号
                //            //判断服务单号对应的客服单是否是免费的
                //            sql = string.Format("select hzkfjad009 from hzfwd,hzkfd left join hzkfjad on hzkfd.hzkfd003=hzkfjad.hzkfjad003 where hzfwd.hzfwd004=hzkfd.hzkfd003 and hzfwd006='{0}'", fwdh);
                //            dt2 = sqldbh.Query(connSql, sql);
                //            if (dt2 != null)
                //            {
                //                if (dt2.Rows.Count > 0)
                //                {
                //                    string val = dt2.Rows[0][0].ToString();
                //                    if (val == "客户原因收费" || val == "汉钟原因收费")
                //                    {
                //                        dt.Rows.RemoveAt(i);
                //                        i--;
                //                    }
                //                }
                //            }
                //        }
                //        #region 明细日志
                //        if (IsWritemsg == 4)
                //        {
                //            foreach (System.Data.DataRow row in dt.Rows)
                //            {
                //                Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\t" + row[4].ToString() + "\r\n", "fwll.txt");
                //            }
                //        }
                //        #endregion



                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }


        /// <summary>
        /// CRM费用申请单
        /// </summary>
        /// <param name="resda019"></param>
        /// <param name="resda021"></param>
        /// <returns></returns>
        private System.Data.DataTable GetCRM(string resda019)
        {


            string sql = string.Format(@"select distinct ( case when a.MY004 like '1V%' THEN 'CQ' 
                                when a.MY004 like '1D%' THEN 'HN'
                                when a.MY004 like '1E%' THEN  'NJ'
                                when a.MY004 like '1C%' THEN  'HB'
                                when a.MY004 like '1F%' THEN  'ZL'
                                ELSE 'HD' end ) as 'qybie',ltrim(rtrim(c.TC197))  as 'cpbie',a.MY008  as 'cost',a.MY005 as 'creator',b.MZ005+b.MZ006 as fwdh from PORMY a, PORMZ b,REPTC c 
                                where a.MY001=b.MZ001 AND a.MY002=b.MZ002 AND c.TC001=b.MZ005 AND c.TC002=b.MZ006
                                and left(a.MY024,6)='{0}'", resda019);//{1}结案日期
            System.Data.DataTable dt = null;
            try
            {
                dt = dbCRM.Query(connSybaseCRM, sql);
                //if (dt.Rows.Count > 0)
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        string fwdh = string.Empty;
                //        System.Data.DataTable dt2 = null;
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            fwdh = dt.Rows[i]["fwdh"].ToString();//服务单号
                //            //判断服务单号对应的客服单是否是免费的
                //            sql = string.Format("select hzkfjad009 from hzfwd,hzkfd left join hzkfjad on hzkfd.hzkfd003=hzkfjad.hzkfjad003 where hzfwd.hzfwd004=hzkfd.hzkfd003 and hzfwd006='{0}'", fwdh);
                //            dt2 = sqldbh.Query(connSql, sql);
                //            if (dt2 != null)
                //            {
                //                if (dt2.Rows.Count > 0)
                //                {
                //                    string val = dt2.Rows[0][0].ToString();
                //                    if (val == "客户原因收费" || val == "汉钟原因收费")
                //                    {
                //                        dt.Rows.RemoveAt(i);
                //                        i--;
                //                    }
                //                }
                //            }
                //        }
                //        #region 明细日志
                //        if (IsWritemsg == 4)
                //        {
                //            foreach (System.Data.DataRow row in dt.Rows)
                //            {
                //                Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\t" + row[4].ToString() + "\r\n", "fwll.txt");
                //            }
                //        }
                //        #endregion



                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        #endregion


        #region 快递厂内FWZYD
        /// <summary>
        /// 服务支援单中的快递费
        /// </summary>
        /// <param name="trdate">日期</param>
        /// <returns></returns>
        private void GetKDF(int year, int month, System.Data.DataTable Dtbos)
        {
            //OA
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dt = this.GetFWZYD(trdate);//dt列：qybie,cpbie,cost,creator,fwdh
            if (dt != null)
            {
                DTHelp(dt, Dtbos, "fare");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dt.Rows.Count; j++)
                //    {
                //        if (Dtbos.Rows[i]["areatype"].ToString() == dt.Rows[j]["qybie"].ToString()
                //            && Dtbos.Rows[i]["protype"].ToString() == dt.Rows[j]["cpbie"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["fare"] = Convert.ToDouble(Dtbos.Rows[i]["fare"] == DBNull.Value ? 0 : Dtbos.Rows[i]["fare"]) + Convert.ToDouble(dt.Rows[j]["cost"] == DBNull.Value ? 0 : dt.Rows[j]["cost"]);
                //        }
                //    }
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resda019">结案日期</param>
        /// <param name="resda021">审核状态</param>
        /// <returns></returns>
        private System.Data.DataTable GetFWZYD(string resda019)
        {
            string sql = string.Format(@"SELECT 
                                                        CASE WHEN hzfwzyd_v01.xqrdept LIKE '1D5%' THEN 'CQ'
                                                        WHEN hzfwzyd_v01.xqrdept LIKE '1D%' THEN 'HN'
                                                        WHEN hzfwzyd_v01.xqrdept LIKE '1C%' THEN 'HB'
                                                        WHEN hzfwzyd_v01.xqrdept LIKE '1E%' THEN 'NJ'
                                                        ELSE 'HD'
                                                        END AS 'qybie',
                                                        CASE hzfwzyd_v01.sffs WHEN '6' THEN 'R'
                                                        WHEN '7' THEN 'AH'
                                                        WHEN '8' THEN 'AA'
                                                        WHEN '9' THEN 'P'
                                                        WHEN '10' THEN 'KM' END AS 'cpbie',
                                                        hzfwzyd_v01.fy AS 'cost'
                                                        FROM hzfwzyd_v01,resda
                                                        WHERE resda.resda001=hzfwzyd_v01.hzfwzyd_v01001 
                                                        AND resda.resda002=hzfwzyd_v01.hzfwzyd_v01002 
                                                        AND resda.resda021='2'
                                                        AND hzfwzyd_v01.fy>'0'
                                                        AND hzfwzyd_v01.sffs>'5'
                                                        AND LEFT(resda.resda019,7)='{0}'", resda019.Insert(4, "/"));
            System.Data.DataTable dt = null;
            try
            {
                dt = sqldbh.Query(connSql, sql);
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        #endregion


        #region 物流
        private void GetWLF(int year, int month, System.Data.DataTable Dtbos)
        {
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dt1 = this.GetCDRN20(year, month);//借出单
            System.Data.DataTable dt2 = this.GetBXD1(trdate, "2");//借出归还到付运费，报销单身1


            if (dt1 != null)
            {
                for (int i = 0; i < Dtbos.Rows.Count; i++)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        if (Dtbos.Rows[i]["areatype"].ToString() == dt1.Rows[j]["qybie"].ToString()
                            && Dtbos.Rows[i]["protype"].ToString() == dt1.Rows[j]["cpbie"].ToString())//如果产品别，区域别，厂内外相等则赋值
                        {
                            Dtbos.Rows[i]["fare"] = Convert.ToDouble(Dtbos.Rows[i]["fare"] == DBNull.Value ? "0" : Dtbos.Rows[i]["fare"]) + Convert.ToDouble(dt1.Rows[j]["freight"] == DBNull.Value ? "0" : dt1.Rows[j]["freight"]);
                        }
                    }
                }
            }
            if (dt2 != null)
            {
                for (int i = 0; i < Dtbos.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        if (Dtbos.Rows[i]["areatype"].ToString() == dt2.Rows[j]["qybie"].ToString()
                            && Dtbos.Rows[i]["protype"].ToString() == dt2.Rows[j]["cpbie"].ToString())//如果产品别，区域别，厂内外相等则赋值
                        {
                            Dtbos.Rows[i]["fare"] = Convert.ToDouble(Dtbos.Rows[i]["fare"] == DBNull.Value ? "0" : Dtbos.Rows[i]["fare"]) + Convert.ToDouble(dt2.Rows[j]["cost"] == DBNull.Value ? "0" : dt2.Rows[j]["cost"]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// CDRN20维护的运费。OA抛转借出单，当月确认的CDRN20单据运费。
        /// </summary>
        /// <param name="trdate">日期</param>
        /// <returns></returns>
        private System.Data.DataTable GetCDRN20(int year, int month)
        {
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            //借出单默认区域别为HD
            string sql = string.Format(@"select a.fwno,
                                                a.kfno,
                                                b.freight,
                                                'HD' as qybie,
                                                '' as cpbie
                                        from cdrlnhad a left join cdrfre b
                                        on a.trno=b.shpno
                                        where 
                                        a.facno='C' 
                                        and a.prono='1'
                                        and b.freight>0
                                        and a.fwno is not null 
                                        and a.status='Y' 
                                        and a.cdrobtype='AOG' 
                                        and convert(varchar(6),a.trdate,112)='{0}'", trdate);
            System.Data.DataTable dt = null;
            try
            {
                dt = dbhC.Query(connSybaseC, sql);
                if (dt.Rows.Count > 0)
                {
                    sql = @"select    case select1 when 'AT' then 'AH' WHEN 'ATWL' THEN 'AH'
		                                            WHEN 'AZ' THEN 'AA' WHEN 'AZWL' THEN 'AA' 
		                                            WHEN 'P' THEN 'P' WHEN 'PWL' THEN 'P'
		                                            WHEN 'R' THEN 'R' WHEN 'RWL' THEN 'R'
		                                            WHEN 'RTZ' THEN 'CM' WHEN 'RTZWL' THEN 'CM' ELSE 'R' END cpbie
                                        from hzfwd 
                                        where hzfwd006='{0}'";
                    //给dt中的cpbie赋值，根据ERP中得到服务单号到OA中搜索相关产品别
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string fwdh = dt.Rows[i]["fwno"].ToString();//服务单号
                        System.Data.DataTable dtfwdh = sqldbh.Query(connSql, string.Format(sql, fwdh));
                        string cpbie = string.Empty;
                        if (dtfwdh.Rows.Count > 0)
                        {
                            cpbie = dtfwdh.Rows[0][0].ToString();
                        }
                        else
                        {
                            //服务单号填写不正确导致无法搜索到OA服务单号对应的数据那么默认为R
                            cpbie = "R";
                        }
                        dt.Rows[i]["cpbie"] = cpbie;
                    }


                    #region 明细日志
                    if (IsWritemsg == 4)
                    {
                        foreach (System.Data.DataRow row in dt.Rows)
                        {
                            Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\t" + row[4].ToString() + "\r\n", "KDF.txt");
                        }
                    }
                    #endregion







                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 借出归还发生运费维护于OA费用报销单
        /// </summary>
        /// <param name="resda019">结案日期</param>
        /// <param name="resda021">审核状态</param>
        /// <returns></returns>
        private System.Data.DataTable GetBXD1(string resda019, string resda021)
        {
            string sql = string.Format(@"select	a.qybie,
		                                        case select1 when 'AT' then 'AH' WHEN 'ATWL' THEN 'AH'
		                                        WHEN 'AZ' THEN 'AA' WHEN 'AZWL' THEN 'AA' 
		                                        WHEN 'P' THEN 'P' WHEN 'PWL' THEN 'P'
		                                        WHEN 'R' THEN 'R' WHEN 'RWL' THEN 'R'
		                                        WHEN 'RTZ' THEN 'CM' WHEN 'RTZWL' THEN 'CM' ELSE 'R' END cpbie,
		                                        a.bmpb20f as cost
                                        		
                                        from (
                                        select	case when b.resak015 like '1D5%' THEN 'CQ' 
		                                        when b.resak015 like '1D%' THEN 'HN'
		                                        WHEN b.resak015 like '1E%' THEN  'NJ'
		                                        WHEN b.resak015 like '1C%' THEN  'HB'
		                                        ELSE 'HD' end qybie,
		                                        a.kfdhd01,
		                                        a.fwdhd01,
		                                        a.CREATOR,
		                                        a.bmpb20f
                                        from (
		                                        select	a.kfdhd01,
				                                        a.fwdhd01,
				                                        a.CREATOR,
				                                        a.bmpb20f
                                        				
		                                        from bxd002_1 a left join resda b
		                                        on a.bxd002_1002=b.resda002 
		                                        where a.bmpb05c like '%30'
		                                        and (fwdhd01 is not null or kfdhd01 is not null)
		                                        and b.resda021='{0}' and left(b.resda019,7)='{1}'
	                                        ) a left join resak b on a.CREATOR=b.resak001
                                        ) a left join hzfwd b on a.fwdhd01=b.hzfwd006", resda021, resda019.Insert(4, "/"));
            System.Data.DataTable dt = null;
            try
            {
                dt = sqldbh.Query(connSql, sql);
                if (dt.Rows.Count > 0)
                {

                    if (IsWritemsg == 4)
                    {
                        foreach (System.Data.DataRow row in dt.Rows)
                        {
                            Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\r\n", "KDF.txt");
                        }
                    }



                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        #endregion

        #endregion

        #region 维修免费


        #region  维修免费厂内领退料
        /// <summary>
        /// 总公司
        /// </summary>
        /// <returns></returns>
        private void GetCInvtrn_1003_1004(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAG','IAF'", "C", trdate, "'1004','1003','0003'", this.CWarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {
                //给dt中的产品别，区域别赋值
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();
                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    System.Data.DataTable dt = dbhC.Query(connSybaseC, string.Format(@"select kfno,fwno,fwllno,wxno,
                                                                                         case hmark2 when 'AH' then 'HD' when 'AA' then 'HD' when 'P' then 'HD' when 'CM' then 'HD' else hmark1 end hmark1,
                                                                                         hmark2 from invhadh where facno='C' and prono='1' and trno = '{0}'", trno));
                    if (dt.Rows.Count > 0)
                    {
                        dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[0]["hmark2"];//产品别
                        dtIAF_1001.Rows[i]["qybie"] = dt.Rows[0]["hmark1"];//区域别
                    }
                }

                #region 明细日志
                if (IsWritemsg == 1)
                {
                    foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                    {
                        //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                        System.Data.DataRow r = this.DtMsg.NewRow();
                        r[0] = row[0];
                        r[1] = row[1];
                        r[2] = row[2];
                        r[3] = row[3];
                        DtMsg.Rows.Add(r.ItemArray);
                    }
                }
                #endregion



                #region IAF--->Dtbos   IAF单据转Dtbos
                DTHelp(dtIAF_1001, Dtbos, "wxll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["wxll"] = Convert.ToDouble(Dtbos.Rows[i]["wxll"] == DBNull.Value ? "0" : Dtbos.Rows[i]["wxll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"] == DBNull.Value ? "0" : dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion

            }
            #endregion
        }

        /// <summary>
        /// 广州
        /// </summary>
        /// <returns></returns>
        private void GetGInvtrn_1003_1004(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAG','IAF'", "G", trdate, "'1004','1003','0003'", this.GWarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {

                //给dt中的产品别，区域别赋值
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();
                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    System.Data.DataTable dt = dbhG.Query(connSybaseG, string.Format(@"select kfno,fwno,fwllno,wxno,
                                                                                         case hmark2 when 'AH' then 'HD' when 'AA' then 'HD' when 'P' then 'HD' when 'CM' then 'HD' else hmark1 end hmark1,
                                                                                         hmark2 from invhadh where facno='G' and prono='1' and trno = '{0}'", trno));
                    if (dt.Rows.Count > 0)
                    {

                        dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[0]["hmark2"];//产品别
                        dtIAF_1001.Rows[i]["qybie"] = dt.Rows[0]["hmark1"];//区域别
                        if (dtIAF_1001.Rows[i]["cpbie"].ToString() == "AH")
                        {

                        }
                    }
                }

                #region 明细日志
                if (IsWritemsg == 1)
                {
                    foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                    {
                        //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                        System.Data.DataRow r = this.DtMsg.NewRow();
                        r[0] = row[0];
                        r[1] = row[1];
                        r[2] = row[2];
                        r[3] = row[3];
                        DtMsg.Rows.Add(r.ItemArray);
                    }
                }
                #endregion

                #region IAF--->Dtbos   IAF单据转Dtbos
                DTHelp(dtIAF_1001, Dtbos, "wxll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["wxll"] = Convert.ToDouble(Dtbos.Rows[i]["wxll"] == DBNull.Value ? "0" : Dtbos.Rows[i]["wxll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"] == DBNull.Value ? "0" : dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }
            #endregion
        }


        /// <summary>
        /// 重庆
        /// </summary>
        /// <returns></returns>
        private void GetC4Invtrn_1003_1004(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAG','IAF'", "C4", trdate, "'1004','1003','0003'", this.C4WarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {

                //给dt中的产品别，区域别赋值
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();
                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    System.Data.DataTable dt = dbhC4.Query(connSybaseC4, string.Format(@"select kfno,fwno,fwllno,wxno,
                                                                                         case hmark2 when 'AH' then 'HD' when 'AA' then 'HD' when 'P' then 'HD' when 'CM' then 'HD' else hmark1 end hmark1,
                                                                                         hmark2 from invhadh where facno='C4' and prono='1' and trno = '{0}'", trno));
                    if (dt.Rows.Count > 0)
                    {

                        dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[0]["hmark2"];//产品别
                        dtIAF_1001.Rows[i]["qybie"] = dt.Rows[0]["hmark1"];//区域别
                        if (dtIAF_1001.Rows[i]["cpbie"].ToString() == "AH")
                        {

                        }
                    }
                }

                #region 明细日志
                if (IsWritemsg == 1)
                {
                    foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                    {
                        //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                        System.Data.DataRow r = this.DtMsg.NewRow();
                        r[0] = row[0];
                        r[1] = row[1];
                        r[2] = row[2];
                        r[3] = row[3];
                        DtMsg.Rows.Add(r.ItemArray);
                    }
                }
                #endregion

                #region IAF--->Dtbos   IAF单据转Dtbos
                DTHelp(dtIAF_1001, Dtbos, "wxll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["wxll"] = Convert.ToDouble(Dtbos.Rows[i]["wxll"] == DBNull.Value ? "0" : Dtbos.Rows[i]["wxll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"] == DBNull.Value ? "0" : dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }
            #endregion
        }


        /// <summary>
        /// 济南
        /// </summary>
        /// <returns></returns>
        private void GetJInvtrn_1003_1004(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAG','IAF'", "J", trdate, "'1004','1003','0003'", this.JWarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {
                //给dt中的产品别，区域别赋值
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();
                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    System.Data.DataTable dt = dbhJ.Query(connSybaseJ, string.Format(@"select kfno,fwno,fwllno,wxno,
                                                                                         case hmark2 when 'AH' then 'HD' when 'AA' then 'HD' when 'P' then 'HD' when 'CM' then 'HD' else hmark1 end hmark1,
                                                                                         hmark2 from invhadh where facno='J' and prono='1' and trno = '{0}'", trno));
                    if (dt.Rows.Count > 0)
                    {
                        dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[0]["hmark2"];//产品别
                        dtIAF_1001.Rows[i]["qybie"] = dt.Rows[0]["hmark1"];//区域别
                    }
                }



                #region 明细日志
                if (IsWritemsg == 1)
                {
                    foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                    {
                        //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                        System.Data.DataRow r = this.DtMsg.NewRow();
                        r[0] = row[0];
                        r[1] = row[1];
                        r[2] = row[2];
                        r[3] = row[3];
                        DtMsg.Rows.Add(r.ItemArray);
                    }
                }
                #endregion
                #region IAF--->Dtbos   IAF单据转Dtbos
                DTHelp(dtIAF_1001, Dtbos, "wxll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["wxll"] = Convert.ToDouble(Dtbos.Rows[i]["wxll"] == DBNull.Value ? "0" : Dtbos.Rows[i]["wxll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"] == DBNull.Value ? "0" : dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }
            #endregion
        }



        /// <summary>
        /// 南京
        /// </summary>
        /// <returns></returns>
        private void GetNInvtrn_1003_1004(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAG','IAF'", "N", trdate, "'1004','1003','0003'", this.NWarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {
                //给dt中的产品别，区域别赋值
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();
                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    System.Data.DataTable dt = dbhN.Query(connSybaseN, string.Format(@"select kfno,fwno,fwllno,wxno,
                                                                                         case hmark2 when 'AH' then 'HD' when 'AA' then 'HD' when 'P' then 'HD' when 'CM' then 'HD' else hmark1 end hmark1,
                                                                                         hmark2 from invhadh where facno='N' and prono='1'and trno = '{0}'", trno));
                    if (dt.Rows.Count > 0)
                    {
                        dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[0]["hmark2"];//产品别
                        dtIAF_1001.Rows[i]["qybie"] = dt.Rows[0]["hmark1"];//区域别
                    }
                }



                #region 明细日志
                if (IsWritemsg == 1)
                {
                    foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                    {
                        //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                        System.Data.DataRow r = this.DtMsg.NewRow();
                        r[0] = row[0];
                        r[1] = row[1];
                        r[2] = row[2];
                        r[3] = row[3];
                        DtMsg.Rows.Add(r.ItemArray);
                    }
                }
                #endregion
                #region IAF--->Dtbos   IAF单据转Dtbos
                DTHelp(dtIAF_1001, Dtbos, "wxll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["wxll"] = Convert.ToDouble(Dtbos.Rows[i]["wxll"] == DBNull.Value ? "0" : Dtbos.Rows[i]["wxll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"] == DBNull.Value ? "0" : dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 柯茂
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="Dtbos"></param>
        private void GetKInvtrn_1003_1004(int year, int month, System.Data.DataTable Dtbos)
        {
            #region IAF
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            System.Data.DataTable dtIAF_1001 = GetInvtrnhTable("'IAG','IAF'", "K", trdate, "'1004','1003','0003'", this.NWarehNot);//客诉领料1001,IAF服务领料单
            if (dtIAF_1001 != null)
            {
                //给dt中的产品别，区域别赋值
                for (int i = 0; i < dtIAF_1001.Rows.Count; i++)
                {
                    //得到交易单号
                    string trno = dtIAF_1001.Rows[i]["trno"].ToString();
                    //根据交易单号得到对应INV310单据中的客服单号
                    if (string.IsNullOrEmpty(trno))
                    {
                        continue;
                    }
                    System.Data.DataTable dt = dbhK.Query(connSybaseK, string.Format(@"select kfno,fwno,fwllno,wxno,
                                                                                         case hmark2 when 'AH' then 'HD' when 'AA' then 'HD' when 'P' then 'HD' when 'CM' then 'HD' else hmark1 end hmark1,
                                                                                         hmark2 from invhadh where facno='K' and prono='1'and trno = '{0}'", trno));
                    if (dt.Rows.Count > 0)
                    {
                        //dtIAF_1001.Rows[i]["cpbie"] = dt.Rows[0]["hmark2"];//产品别
                        dtIAF_1001.Rows[i]["cpbie"] = "KM";//产品别
                        dtIAF_1001.Rows[i]["qybie"] = dt.Rows[0]["hmark1"];//区域别
                    }
                }



                #region 明细日志
                if (IsWritemsg == 1)
                {
                    foreach (System.Data.DataRow row in dtIAF_1001.Rows)
                    {
                        //Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "fwll.txt");
                        System.Data.DataRow r = this.DtMsg.NewRow();
                        r[0] = row[0];
                        r[1] = row[1];
                        r[2] = row[2];
                        r[3] = row[3];
                        DtMsg.Rows.Add(r.ItemArray);
                    }
                }
                #endregion
                #region IAF--->Dtbos   IAF单据转Dtbos
                DTHelp(dtIAF_1001, Dtbos, "wxll");
                //for (int i = 0; i < Dtbos.Rows.Count; i++)
                //{
                //    for (int j = 0; j < dtIAF_1001.Rows.Count; j++)
                //    {
                //        if (dtIAF_1001.Rows[j]["cpbie"].ToString() == Dtbos.Rows[i]["protype"].ToString()
                //            && dtIAF_1001.Rows[j]["qybie"].ToString() == Dtbos.Rows[i]["areatype"].ToString())//如果产品别，区域别，厂内外相等则赋值
                //        {
                //            Dtbos.Rows[i]["wxll"] = Convert.ToDouble(Dtbos.Rows[i]["wxll"] == DBNull.Value ? "0" : Dtbos.Rows[i]["wxll"]) + Convert.ToDouble(dtIAF_1001.Rows[j]["cost"] == DBNull.Value ? "0" : dtIAF_1001.Rows[j]["cost"]);
                //        }
                //    }
                //}

                #endregion
            }
            #endregion
        }
        #endregion

        #region 重工制令

        /// <summary>
        /// 制令领退料_重工制令_整机去除
        /// </summary>
        /// <param name="trdate"></param>
        /// <returns></returns>
        private void GetInvtrn_02(int year, int month, System.Data.DataTable Dtbos)
        {
            #region sqlinv
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            /*
             * select 	a.facno,a.prono,a.manno,a.typecode,a.kfdh 'kfno',
 sum(case a.iocode when '1' then -b.ttmatm else b.ttmatm end)  as cost,a.remark1 cpbie,'HD' qybie
from(
	SELECT 	manpih.facno,manpih.prono, manpih.manno,
			manmas.typecode,manpih.itnbrf,y.itdsc as itnbrfdsc,manpih.issdepno,    
			manpih.trtype,manpih.pisno,
			manpih.iocode,
			manpid.seqnr,
			manmas.kfdh,
			manmas.remark1

	FROM manpid left outer join invmas x on x.itnbr=manpid.altitnbr,   
	manpih left outer join invmas y on y.itnbr=manpih.itnbrf   
	 left outer join manmas on manmas.facno=manpih.facno and  manmas.prono=manpih.prono and manmas.manno=manpih.manno 
	WHERE ( manpih.facno = manpid.facno ) and   
	( manpih.prono = manpid.prono ) and   
	( manpih.pisno = manpid.pisno ) and   
	( ( manpih.issstatus = 'C' ) )   
	and manpid.altitnbr<>'3188-GBR6254-FW'
	//and manmas.remark1='R'
	and manpih.facno='C' and manpih.prono='1' 
	and manpid.wareh not in ('NG02','KF01','KF05','KF03')
	and manmas.typecode in ('02','05')
	and (manpih.issdepno='9900') 
	and (convert(varchar(6),manpih.issdate,112)='201503')  
	UNION    
	SELECT 	manreh.facno, manreh.prono,manreh.manno,
			manmas.typecode,manreh.itnbrf, t.itdsc as  itnbrfdsc,manreh.retdepno,    
			manreh.trtype,manreh.retno,
			manreh.iocode,   
			manred.seqnr,
			manmas.kfdh,
			manmas.remark1
	FROM manred left outer join invmas s on s.itnbr=manred.altitnbr,   
	manreh left outer join invmas t on t.itnbr=manreh.itnbrf 
	 left outer join manmas on manmas.facno=manreh.facno and  manmas.prono=manreh.prono and manmas.manno=manreh.manno    
	WHERE ( manreh.facno = manred.facno ) and   
	( manreh.prono = manred.prono ) and   
	( manreh.retno = manred.retno ) and   
	( ( manreh.issstatus = 'C' ) ) 
	and manred.altitnbr<>'3188-GBR6254-FW'	
	//and manmas.remark1='R'
	and manreh.facno='C' and manreh.prono='1' 
	and manred.wareh not in ('NG02','KF01','KF05','KF03')
	and manmas.typecode in ('02','05')
	and (manreh.retdepno='9900')
	and (convert(varchar(6),manreh.retdate,112)='201503')  
	) a 
left join invtrnh b 
on a.facno=b.facno 
	and a.trtype=b.trtype 
	and a.pisno=b.trno 
	and a.seqnr=b.trseq 
where b.facno='C' and b.prono='1' 
	group by a.facno,a.prono,a.manno,a.typecode,a.kfdh,a.remark1;
             * 
             */
            string sqlinv = string.Format(@"select 	a.facno,a.prono,a.manno,a.typecode,a.kfdh 'kfno',
			                                        sum(case a.iocode when '1' then -b.ttmatm else b.ttmatm end)  as cost,
                                                    case a.remark1 when 'FW' then 'R' ELSE a.remark1 end  'cpbie',
                                                    'HD' qybie
                                                from(
	                                                SELECT 	manpih.facno,manpih.prono, manpih.manno,
			                                                manmas.typecode,manpih.itnbrf,y.itdsc as itnbrfdsc,manpih.issdepno,    
			                                                manpih.trtype,manpih.pisno,
			                                                manpih.iocode,
			                                                manpid.seqnr,
                                                            manmas.kfdh,
			                                                manmas.remark1

	                                                FROM manpid left outer join invmas x on x.itnbr=manpid.altitnbr,   
	                                                manpih left outer join invmas y on y.itnbr=manpih.itnbrf   
                                                    left outer join manmas on manmas.facno=manpih.facno and  manmas.prono=manpih.prono and manmas.manno=manpih.manno 
	                                                WHERE ( manpih.facno = manpid.facno ) and   
	                                                ( manpih.prono = manpid.prono ) and   
	                                                ( manpih.pisno = manpid.pisno ) and   
	                                                ( ( manpih.issstatus = 'C' ) )   
	                                                and manpid.altitnbr<>'3188-GBR6254-FW'
	                                                and manpih.facno='C' and manpih.prono='1' 
	                                                and manpid.wareh not in ({1})
	                                                and manmas.typecode='02' 
	                                                and (manpih.issdepno='9900') 
	                                                and (convert(varchar(6),manpih.issdate,112)='{0}')  
	                                                UNION    
	                                                SELECT 	manreh.facno, manreh.prono,manreh.manno,
			                                                manmas.typecode,manreh.itnbrf, t.itdsc as  itnbrfdsc,manreh.retdepno,    
			                                                manreh.trtype,manreh.retno,
			                                                manreh.iocode,   
			                                                manred.seqnr,
                                                            manmas.kfdh ,
			                                                manmas.remark1
	                                                FROM manred left outer join invmas s on s.itnbr=manred.altitnbr,   
	                                                manreh left outer join invmas t on t.itnbr=manreh.itnbrf 
                                                    left outer join manmas on manmas.facno=manreh.facno and  manmas.prono=manreh.prono and manmas.manno=manreh.manno    
	                                                WHERE ( manreh.facno = manred.facno ) and   
	                                                ( manreh.prono = manred.prono ) and   
	                                                ( manreh.retno = manred.retno ) and   
	                                                ( ( manreh.issstatus = 'C' ) ) 
	                                                and manred.altitnbr<>'3188-GBR6254-FW'	
	                                                and manreh.facno='C' and manreh.prono='1' 
	                                                and manred.wareh not in ({1})
	                                                and manmas.typecode='02' 
	                                                and (manreh.retdepno='9900' )  
	                                                and (convert(varchar(6),manreh.retdate,112)='{0}')  
	                                                ) a 
                                                left join invtrnh b 
                                                on a.facno=b.facno 
	                                                and a.trtype=b.trtype 
	                                                and a.pisno=b.trno 
	                                                and a.seqnr=b.trseq 
                                                where b.facno='C' and b.prono='1' 
	                                                group by a.facno,a.prono,a.manno,a.typecode,a.kfdh,a.remark1", trdate, this.CmanWarehNot);//重工制令，9900，整机去除，领退料抵扣，确认状态。
            #endregion
            System.Data.DataTable dtinv = null;//facno,prono,manno,typecode,kfno,cost

            try
            {
                dtinv = dbhC.Query(connSybaseC, sqlinv);
                if (dtinv.Rows.Count > 0)
                {
                    #region 明细日志
                    if (IsWritemsg == 1)
                    {
                        foreach (System.Data.DataRow row in dtinv.Rows)
                        {
                            //Wrlog(row[2].ToString() + "\t" + row[5].ToString() + "\t" + row[6].ToString() + "\t" + row[7].ToString() + "\r\n", "fwll.txt");
                            System.Data.DataRow r = this.DtMsg.NewRow();
                            r[0] = row[2];
                            r[1] = row[5];
                            r[2] = row[6];
                            r[3] = row[7];
                            DtMsg.Rows.Add(r.ItemArray);
                        }
                    }
                    #endregion


                    DTHelp(dtinv, Dtbos, "mancost");
                    //for (int i = 0; i < Dtbos.Rows.Count; i++)
                    //{
                    //    for (int j = 0; j < dtinv.Rows.Count; j++)
                    //    {
                    //        if (Dtbos.Rows[i]["protype"].ToString() == dtinv.Rows[j]["cpbie"].ToString()
                    //            && Dtbos.Rows[i]["areatype"].ToString() == dtinv.Rows[j]["qybie"].ToString())
                    //        {
                    //            Dtbos.Rows[i]["mancost"] = Convert.ToDouble(Dtbos.Rows[i]["mancost"] == DBNull.Value ? "0" : Dtbos.Rows[i]["mancost"]) + Convert.ToDouble(dtinv.Rows[j]["cost"] == DBNull.Value ? "0" : dtinv.Rows[j]["cost"]);
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }






        #endregion


        #region 运费成本 借入单(OA费用报销)  借入归还单（ERPINV310-IAK单据）

        /// <summary>
        /// 借入归还单
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private void GetInv310_IAK(int year, int month, System.Data.DataTable Dtbos)
        {
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }

            string sql = string.Format(@"select a.trno,b.freight cost,a.kfno,'' cpbie,'' qybie
                                        from invhadh a left join cdrfre b
                                        on a.trno=b.shpno
                                        where b.freight > 0 and a.kfno is not null and a.kfno <> '' 
                                        and a.status = 'Y' and a.trtype = 'IAK' 
                                        and convert(varchar(6),a.trdate,112) = '{0}'", trdate);
            System.Data.DataTable dtinv = null;
            try
            {
                dtinv = dbhC.Query(connSybaseC, sql);
                if (dtinv.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder("");
                    string sqlwhere = string.Empty;
                    string kfno = string.Empty;
                    for (int i = 0; i < dtinv.Rows.Count; i++)
                    {
                        kfno = dtinv.Rows[i]["kfno"].ToString();
                        if (kfno.IndexOf('/') != -1)
                        {
                            kfno = kfno.Split('/')[0];
                        }
                        sb.Append("'" + kfno + "',");
                    }
                    if (sb.Length > 0)
                    {
                        sqlwhere = sb.ToString().Substring(0, sb.ToString().Length - 1);//客服单号
                    }

                    //tb中cpbie,qybie赋值
                    sql = string.Format(@"select	case a.hzkfd008 when 'HNKF' then 'HN'
		                                            when 'HDKF' then 'HD' 
		                                            when 'HBKF' then 'HB'
		                                            when 'NJKF' then 'NJ'
		                                            when 'CQKF' then 'CQ' END qybie,
		                                            case b.hzkfd_d01005 when 'AT' then 'AH' WHEN 'ATWL' THEN 'AH'
		                                            WHEN 'AZ' THEN 'AA' WHEN 'AZWL' THEN 'AA' 
		                                            WHEN 'P' THEN 'P' WHEN 'PWL' THEN 'P'
		                                            WHEN 'R' THEN 'R' WHEN 'RWL' THEN 'R'
		                                            WHEN 'RTZ' THEN 'CM' WHEN 'RTZWL' THEN 'CM' ELSE 'R' END cpbie,a.hzkfd003
                                            from hzkfd a left join hzkfd_d01 b
                                            on a.hzkfd002 = b.hzkfd_d01002
                                            where hzkfd001='HZKFD' and hzkfd003 in ({0})", sqlwhere);
                    System.Data.DataTable dt = sqldbh.Query(connSql, sql);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtinv.Rows.Count; i++)
                        {
                            kfno = dtinv.Rows[i]["kfno"].ToString();
                            if (kfno.IndexOf('/') != -1)
                            {
                                kfno = kfno.Split('/')[0];
                            }
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (kfno == dt.Rows[j]["hzkfd003"].ToString())
                                {
                                    dtinv.Rows[i]["qybie"] = dt.Rows[j]["qybie"].ToString();
                                    dtinv.Rows[i]["cpbie"] = dt.Rows[j]["cpbie"].ToString();
                                    if (dtinv.Rows[i]["cpbie"].ToString() == "AA" || dtinv.Rows[i]["cpbie"].ToString() == "AH" || dtinv.Rows[i]["cpbie"].ToString() == "P" || dtinv.Rows[i]["cpbie"].ToString() == "CM")//如果产品别是AA,AH,P,CM区域别设为HD
                                    {
                                        dtinv.Rows[i]["qybie"] = "HD";
                                    }
                                }
                            }
                        }
                        if (IsWritemsg == 4)
                        {
                            foreach (System.Data.DataRow row in dtinv.Rows)
                            {
                                Wrlog(row[0].ToString() + "\t" + row[1].ToString() + "\t" + row[2].ToString() + "\t" + row[3].ToString() + "\r\n", "KDF.txt");
                            }
                        }

                        DTHelp(dtinv, Dtbos, "fare");
                        //for (int i = 0; i < Dtbos.Rows.Count; i++)
                        //{
                        //    for (int j = 0; j < dtinv.Rows.Count; j++)
                        //    {
                        //        if (Dtbos.Rows[i]["protype"].ToString() == dtinv.Rows[j]["cpbie"].ToString()
                        //            && Dtbos.Rows[i]["areatype"].ToString() == dtinv.Rows[j]["qybie"].ToString())
                        //        {
                        //            Dtbos.Rows[i]["fare"] = Convert.ToDouble(Dtbos.Rows[i]["fare"] == DBNull.Value ? "0" : Dtbos.Rows[i]["fare"]) + Convert.ToDouble(dtinv.Rows[j]["cost"] == DBNull.Value ? "0" : dtinv.Rows[j]["cost"]);
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion


        #endregion

        #region 质量扣款



        private void GetCutMoney(int year, int month, System.Data.DataTable dtbos)
        {
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            string sql = string.Format(@"SELECT armrech.recno,ABS(SUM(ISNULL(armrec.recamt,0))) as cost,armrech.hmark1 as cpbie,armrech.hmark2 as qybie FROM armrech,armrec 
                                                        WHERE armrech.facno=armrec.facno 
                                                        AND armrech.recno=armrec.recno 
                                                        AND armrec.rectype='1'
                                                        AND armrech.zlk='Y'
                                                        AND armrech.recstat='1'
                                                        AND convert(VARCHAR(6),armrech.ltrndate,112)='{0}'
                                                        GROUP BY armrech.recno,armrech.hmark1,armrech.hmark2
                                                        UNION ALL
                                                        SELECT armpmm.trno,ABS(SUM(ISNULL(armpmm.pmamt,0))) as cost,armpmm.hmark1 as cpbie,armpmm.hmark2 as qybie FROM armpmm  
                                                        WHERE 
                                                        armpmm.zlk='Y'
                                                        AND convert(VARCHAR(6),armpmm.trdat,112)='{0}'
                                                        GROUP BY armpmm.hmark1,armpmm.hmark2,trno", trdate);
            System.Data.DataTable dtc = dbhC.Query(connSybaseC, sql);
            System.Data.DataTable dtg = dbhG.Query(connSybaseG, sql);
            System.Data.DataTable dtj = dbhJ.Query(connSybaseJ, sql);
            System.Data.DataTable dtn = dbhN.Query(connSybaseN, sql);
            if (dtc.Rows.Count > 0)
            {
                DTHelp(dtc, dtbos, "ZLKK");
            }
            if (dtg.Rows.Count > 0)
            {
                DTHelp(dtg, dtbos, "ZLKK");
            }
            if (dtj.Rows.Count > 0)
            {
                DTHelp(dtj, dtbos, "ZLKK");
            }
            if (dtn.Rows.Count > 0)
            {
                DTHelp(dtn, dtbos, "ZLKK");
            }
        }

        private void DTHelp(System.Data.DataTable dtfrom, System.Data.DataTable dtbos, string type)
        {
            System.Data.DataRow[] row_r_hd = dtbos.Select("protype='R' and areatype='HD'");
            System.Data.DataRow[] row_r_hb = dtbos.Select("protype='R' and areatype='HB'");
            System.Data.DataRow[] row_r_nj = dtbos.Select("protype='R' and areatype='NJ'");
            System.Data.DataRow[] row_r_cq = dtbos.Select("protype='R' and areatype='CQ'");
            System.Data.DataRow[] row_r_zl = dtbos.Select("protype='R' and areatype='ZL'");
            System.Data.DataRow[] row_r_hn = dtbos.Select("protype='R' and areatype='HN'");
            System.Data.DataRow[] row_cm_hd = dtbos.Select("protype='CM' and areatype='HD'");
            System.Data.DataRow[] row_km_hd = dtbos.Select("protype='KM' and areatype='HD'");
            System.Data.DataRow[] row_aa_hd = dtbos.Select("protype='AA' and areatype='HD'");
            System.Data.DataRow[] row_ah_hd = dtbos.Select("protype='AH' and areatype='HD'");
            System.Data.DataRow[] row_p_hd = dtbos.Select("protype='P' and areatype='HD'");

            for (int j = 0; j < dtfrom.Rows.Count; j++)
            {
                string cpb = dtfrom.Rows[j]["cpbie"].ToString();
                string qyb = dtfrom.Rows[j]["qybie"].ToString();
                decimal amts = Convert.ToDecimal(dtfrom.Rows[j]["cost"].ToString());
                switch (cpb)
                {
                    case "CM":
                        row_cm_hd[0][type] = Convert.ToDecimal(row_cm_hd[0][type]) + amts;
                        break;
                    case "P":
                        row_p_hd[0][type] = Convert.ToDecimal(row_p_hd[0][type]) + amts;
                        break;
                    case "KM":
                        row_km_hd[0][type] = Convert.ToDecimal(row_km_hd[0][type]) + amts;
                        break;
                    case "AA":
                        row_aa_hd[0][type] = Convert.ToDecimal(row_aa_hd[0][type]) + amts;
                        break;
                    case "AH":
                        row_ah_hd[0][type] = Convert.ToDecimal(row_ah_hd[0][type]) + amts;
                        break;
                    default:
                        switch (qyb)
                        {
                            case "HD":
                                row_r_hd[0][type] = Convert.ToDecimal(row_r_hd[0][type]) + amts;
                                break;
                            case "HB":
                                row_r_hb[0][type] = Convert.ToDecimal(row_r_hb[0][type]) + amts;
                                break;
                            case "HN":
                                row_r_hn[0][type] = Convert.ToDecimal(row_r_hn[0][type]) + amts;
                                break;
                            case "NJ":
                                row_r_nj[0][type] = Convert.ToDecimal(row_r_nj[0][type]) + amts;
                                break;
                            case "CQ":
                                row_r_cq[0][type] = Convert.ToDecimal(row_r_cq[0][type]) + amts;
                                break;
                            case "ZL":
                                row_r_zl[0][type] = Convert.ToDecimal(row_r_zl[0][type]) + amts;
                                break;
                            default:
                                break;
                        }
                        break;
                }
            }
        }
        #endregion

        #region 销售折让

        private void GetDiscount(int year, int month, System.Data.DataTable dtbos)
        {
            string trdate = year.ToString() + month.ToString();
            if (month < 10)
            {
                trdate = trdate.Insert(4, "0");
            }
            string sql = string.Format(@"SELECT 	cdrhmas.cdrno,
		                        cdrhmas.discount AS cost,
		                        hmark1 AS cpbie,
		                        CASE WHEN depno LIKE '1D5%' THEN 'CQ' ELSE 
			                        CASE cdrhmas.areacode 	WHEN 'HD1' THEN 'NJ' 
									                        WHEN 'HD2' THEN 'HD' 
									                        WHEN 'HZ' THEN '' 
									                        WHEN 'JN01' THEN 'HB' 
									                        WHEN 'NJ01' THEN 'NJ' 
									                        WHEN 'SH01' THEN 'HD' 
									                        WHEN 'GZ01' THEN 'HN' 
									                        ELSE cdrhmas.areacode
									                        END
		                        END AS qybie,cdrhmas.areacode
                        FROM cdrhmas
                        WHERE hmark1 IS NOT NULL and discount>0 AND Convert(VARCHAR(6),cfmdate,112)='{0}'", trdate);
            System.Data.DataTable dtc = dbhC.Query(connSybaseC, sql);
            System.Data.DataTable dtg = dbhG.Query(connSybaseG, sql);
            System.Data.DataTable dtj = dbhJ.Query(connSybaseJ, sql);
            System.Data.DataTable dtn = dbhN.Query(connSybaseN, sql);
            if (dtc.Rows.Count > 0)
            {
                DTHelp(dtc, dtbos, "XSZR");
            }
            if (dtg.Rows.Count > 0)
            {
                DTHelp(dtg, dtbos, "XSZR");
            }
            if (dtj.Rows.Count > 0)
            {
                DTHelp(dtj, dtbos, "XSZR");
            }
            if (dtn.Rows.Count > 0)
            {
                DTHelp(dtn, dtbos, "XSZR");
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private static System.Data.DataTable BuildDtbos(int year, int month)
        {
            System.Data.DataTable Dtbos = new System.Data.DataTable();
            Dtbos.Columns.Add("servicetype", typeof(int));      //收费类型0免费1收费
            Dtbos.Columns.Add("year", typeof(int));             //年份
            Dtbos.Columns.Add("month", typeof(int));            //月份
            Dtbos.Columns.Add("protype", typeof(string));       //产品别
            Dtbos.Columns.Add("areatype", typeof(string));      //区域别
            Dtbos.Columns.Add("innertarget", typeof(double));   //内目标.
            Dtbos.Columns.Add("outtarget", typeof(double));     //外目标
            Dtbos.Columns.Add("mancost", typeof(double));       //制令
            Dtbos.Columns.Add("wxll", typeof(double));          //维修领料
            Dtbos.Columns.Add("fwll", typeof(double));          //服务领料
            Dtbos.Columns.Add("travelcost", typeof(double));    //差旅费
            Dtbos.Columns.Add("fare", typeof(double));          //运费
            Dtbos.Columns.Add("zlkk", typeof(double));
            Dtbos.Columns.Add("xszr", typeof(double));
            System.Data.DataRow rowR_HD = Dtbos.NewRow();
            rowR_HD[0] = 0;//免费
            rowR_HD[1] = year;
            rowR_HD[2] = month;
            rowR_HD[3] = "R";
            rowR_HD[4] = "HD"; ;
            rowR_HD[5] = 0;
            rowR_HD[6] = 0;
            rowR_HD[7] = 0;
            rowR_HD[8] = 0;
            rowR_HD[9] = 0;
            rowR_HD[10] = 0;
            rowR_HD[11] = 0;
            rowR_HD[12] = 0;
            rowR_HD[13] = 0;
            Dtbos.Rows.Add(rowR_HD);

            System.Data.DataRow rowR_HN = Dtbos.NewRow();
            rowR_HN[0] = 0;//免费
            rowR_HN[1] = year;
            rowR_HN[2] = month;
            rowR_HN[3] = "R";
            rowR_HN[4] = "HN"; ;
            rowR_HN[5] = 0;
            rowR_HN[6] = 0;
            rowR_HN[7] = 0;
            rowR_HN[8] = 0;
            rowR_HN[9] = 0;
            rowR_HN[10] = 0;
            rowR_HN[11] = 0;
            rowR_HN[12] = 0;
            rowR_HN[13] = 0;
            Dtbos.Rows.Add(rowR_HN);

            System.Data.DataRow rowR_HB = Dtbos.NewRow();
            rowR_HB[0] = 0;//免费
            rowR_HB[1] = year;
            rowR_HB[2] = month;
            rowR_HB[3] = "R";
            rowR_HB[4] = "HB"; ;
            rowR_HB[5] = 0;
            rowR_HB[6] = 0;
            rowR_HB[7] = 0;
            rowR_HB[8] = 0;
            rowR_HB[9] = 0;
            rowR_HB[10] = 0;
            rowR_HB[11] = 0;
            rowR_HB[12] = 0;
            rowR_HB[13] = 0;
            Dtbos.Rows.Add(rowR_HB);

            System.Data.DataRow rowR_NJ = Dtbos.NewRow();
            rowR_NJ[0] = 0;//免费
            rowR_NJ[1] = year;
            rowR_NJ[2] = month;
            rowR_NJ[3] = "R";
            rowR_NJ[4] = "NJ"; ;
            rowR_NJ[5] = 0;
            rowR_NJ[6] = 0;
            rowR_NJ[7] = 0;
            rowR_NJ[8] = 0;
            rowR_NJ[9] = 0;
            rowR_NJ[10] = 0;
            rowR_NJ[11] = 0;
            rowR_NJ[12] = 0;
            rowR_NJ[13] = 0;
            Dtbos.Rows.Add(rowR_NJ);

            System.Data.DataRow rowR_CQ = Dtbos.NewRow();
            rowR_CQ[0] = 0;//免费
            rowR_CQ[1] = year;
            rowR_CQ[2] = month;
            rowR_CQ[3] = "R";
            rowR_CQ[4] = "CQ"; ;
            rowR_CQ[5] = 0;
            rowR_CQ[6] = 0;
            rowR_CQ[7] = 0;
            rowR_CQ[8] = 0;
            rowR_CQ[9] = 0;
            rowR_CQ[10] = 0;
            rowR_CQ[11] = 0;
            rowR_CQ[12] = 0;
            rowR_CQ[13] = 0;
            Dtbos.Rows.Add(rowR_CQ);

            System.Data.DataRow rowR_ZL= Dtbos.NewRow();
            rowR_ZL[0] = 0;//免费
            rowR_ZL[1] = year;
            rowR_ZL[2] = month;
            rowR_ZL[3] = "R";
            rowR_ZL[4] = "ZL"; ;
            rowR_ZL[5] = 0;
            rowR_ZL[6] = 0;
            rowR_ZL[7] = 0;
            rowR_ZL[8] = 0;
            rowR_ZL[9] = 0;
            rowR_ZL[10] = 0;
            rowR_ZL[11] = 0;
            rowR_ZL[12] = 0;
            rowR_ZL[13] = 0;
            Dtbos.Rows.Add(rowR_ZL);

            System.Data.DataRow rowAA_HD = Dtbos.NewRow();
            rowAA_HD[0] = 0;//免费
            rowAA_HD[1] = year;
            rowAA_HD[2] = month;
            rowAA_HD[3] = "AA";
            rowAA_HD[4] = "HD"; ;
            rowAA_HD[5] = 0;
            rowAA_HD[6] = 0;
            rowAA_HD[7] = 0;
            rowAA_HD[8] = 0;
            rowAA_HD[9] = 0;
            rowAA_HD[10] = 0;
            rowAA_HD[11] = 0;
            rowAA_HD[12] = 0;
            rowAA_HD[13] = 0;
            Dtbos.Rows.Add(rowAA_HD);

            System.Data.DataRow rowAH_HD = Dtbos.NewRow();
            rowAH_HD[0] = 0;//免费
            rowAH_HD[1] = year;
            rowAH_HD[2] = month;
            rowAH_HD[3] = "AH";
            rowAH_HD[4] = "HD"; ;
            rowAH_HD[5] = 0;
            rowAH_HD[6] = 0;
            rowAH_HD[7] = 0;
            rowAH_HD[8] = 0;
            rowAH_HD[9] = 0;
            rowAH_HD[10] = 0;
            rowAH_HD[11] = 0;
            rowAH_HD[12] = 0;
            rowAH_HD[13] = 0;
            Dtbos.Rows.Add(rowAH_HD);

            System.Data.DataRow rowP_HD = Dtbos.NewRow();
            rowP_HD[0] = 0;//免费
            rowP_HD[1] = year;
            rowP_HD[2] = month;
            rowP_HD[3] = "P";
            rowP_HD[4] = "HD"; ;
            rowP_HD[5] = 0;
            rowP_HD[6] = 0;
            rowP_HD[7] = 0;
            rowP_HD[8] = 0;
            rowP_HD[9] = 0;
            rowP_HD[10] = 0;
            rowP_HD[11] = 0;
            rowP_HD[12] = 0;
            rowP_HD[13] = 0;
            Dtbos.Rows.Add(rowP_HD);

            System.Data.DataRow rowCM_HD = Dtbos.NewRow();
            rowCM_HD[0] = 0;//免费
            rowCM_HD[1] = year;
            rowCM_HD[2] = month;
            rowCM_HD[3] = "CM";
            rowCM_HD[4] = "HD"; ;
            rowCM_HD[5] = 0;
            rowCM_HD[6] = 0;
            rowCM_HD[7] = 0;
            rowCM_HD[8] = 0;
            rowCM_HD[9] = 0;
            rowCM_HD[10] = 0;
            rowCM_HD[11] = 0;
            rowCM_HD[12] = 0;
            rowCM_HD[13] = 0;
            Dtbos.Rows.Add(rowCM_HD);

            System.Data.DataRow rowKM_HD = Dtbos.NewRow();
            rowKM_HD[0] = 0;//免费
            rowKM_HD[1] = year;
            rowKM_HD[2] = month;
            rowKM_HD[3] = "KM";
            rowKM_HD[4] = "HD"; ;
            rowKM_HD[5] = 0;
            rowKM_HD[6] = 0;
            rowKM_HD[7] = 0;
            rowKM_HD[8] = 0;
            rowKM_HD[9] = 0;
            rowKM_HD[10] = 0;
            rowKM_HD[11] = 0;
            rowKM_HD[12] = 0;
            rowKM_HD[13] = 0;
            Dtbos.Rows.Add(rowKM_HD);


            return Dtbos;
        }

        #region 明细逻辑
        /// <summary>
        /// 得到 明细
        /// </summary>
        /// <returns></returns>
        public System.Data.DataSet GetMsg()
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            System.Data.DataTable dt = new System.Data.DataTable();//明细大表
            DateTime datefirst = new DateTime(Year, Month, 1);//当月第一天
            DateTime datelast = new DateTime(datefirst.AddMonths(1).Year, datefirst.AddMonths(1).Month, 1);//下月第一天
            #region IAF,IAG单据
            #region
            string sql = string.Format(@"SELECT b.trtype,e.typedsc,b.depno, (select x.cdesc from gzerp.dbo.miscode x where x.ckind=e.depdsckind and x.code=b.depno ) as depdsc, 
	 b.trno, convert(char(8),b.trdate,112) as trdate , convert(varchar(4),b.trseq), a.itnbr, a.itdsc, a.itcls, d.clsdsc,b.syscode,  b.wareh, c.whdsc,
	 convert(varchar(10),b.trnqy1),b.unmsr1, convert(varchar(10),b.tramt),   b.userno,    
	 case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end as iocodedsc,  
	 (b.hmark1 + ';' + b.hmark2 + ';' + f.mark1 + ';' + f.mark2 + ';' + f.mark3 + ';'       + f.mark4) as mark, 
	 e.reskind,
	 (select y.cdesc from gzerp.dbo.miscode y where y.ckind=e.reskind and y.code=b.rescode) as resdsc ,'' 'cpb','' 'qyb'
	 FROM gzerp.dbo.invmas  a  
		 left outer join gzerp.dbo.invcls d on a.itcls=d.itcls,gzerp.dbo.invtrnh b 
	 left outer join gzerp.dbo.invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
	 left outer join gzerp.dbo.invdou e on e.trtype=b.trtype  
	 left outer join gzerp.dbo.invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
	 WHERE ( b.itnbr = a.itnbr )   
		 and  b.facno='G' and b.prono='1' 
		 and (b.trtype in ('IAF','IAG')) 
		 and (b.trdate>='{0}' and b.trdate<'{1}')
union all
SELECT b.trtype,e.typedsc,b.depno, (select x.cdesc from cqerp.dbo.miscode x where x.ckind=e.depdsckind and x.code=b.depno ) as depdsc, 
	 b.trno, convert(char(8),b.trdate,112) as trdate , convert(varchar(4),b.trseq), a.itnbr, a.itdsc, a.itcls, d.clsdsc,b.syscode,  b.wareh, c.whdsc,
	 convert(varchar(10),b.trnqy1),b.unmsr1, convert(varchar(10),b.tramt),   b.userno,    
	 case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end as iocodedsc,  
	 (b.hmark1 + ';' + b.hmark2 + ';' + f.mark1 + ';' + f.mark2 + ';' + f.mark3 + ';'       + f.mark4) as mark, 
	 e.reskind,
	 (select y.cdesc from cqerp.dbo.miscode y where y.ckind=e.reskind and y.code=b.rescode) as resdsc ,'' 'cpb','' 'qyb'
	 FROM cqerp.dbo.invmas  a  
		 left outer join cqerp.dbo.invcls d on a.itcls=d.itcls,cqerp.dbo.invtrnh b 
	 left outer join cqerp.dbo.invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
	 left outer join cqerp.dbo.invdou e on e.trtype=b.trtype  
	 left outer join cqerp.dbo.invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
	 WHERE ( b.itnbr = a.itnbr )   
		 and  b.facno='C4' and b.prono='1' 
		 and (b.trtype in ('IAF','IAG')) 
		 and (b.trdate>='{0}' and b.trdate<'{1}')
union all
		 SELECT b.trtype,e.typedsc,b.depno, (select x.cdesc 
		 from njerp.dbo.miscode x where x.ckind=e.depdsckind and x.code=b.depno ) as depdsc, 
	 b.trno, convert(char(8),b.trdate,112) as trdate , convert(varchar(4),b.trseq), a.itnbr, a.itdsc,a.itcls, d.clsdsc,b.syscode,  b.wareh, c.whdsc,
	 convert(varchar(10),b.trnqy1),b.unmsr1, convert(varchar(10),b.tramt),   b.userno,
	 case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end as iocodedsc,  
	 (b.hmark1 + ';' + b.hmark2 + ';' + f.mark1 + ';' + f.mark2 + ';' + f.mark3 + ';'       + f.mark4) as mark, 
	 e.reskind,
	 (select y.cdesc from njerp.dbo.miscode y where y.ckind=e.reskind and y.code=b.rescode) as resdsc ,'' 'cpb','' 'qyb'
	 FROM njerp.dbo.invmas  a  
		 left outer join njerp.dbo.invcls d on a.itcls=d.itcls,
		 njerp.dbo.invtrnh b 
	 left outer join njerp.dbo.invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
	 left outer join njerp.dbo.invdou e on e.trtype=b.trtype  
	 left outer join njerp.dbo.invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
	 WHERE ( b.itnbr = a.itnbr )   
		 and  b.facno='N' and b.prono='1' 
		 and (b.trtype in ('IAF','IAG')) 
		 and (b.trdate>='{0}' and b.trdate<'{1}')
union all
		 SELECT b.trtype,e.typedsc,b.depno,
		 (select x.cdesc 
		 from jnerp..miscode x where x.ckind=e.depdsckind and x.code=b.depno ) as depdsc, 
	 b.trno, convert(char(8),b.trdate,112) as trdate , convert(varchar(4),b.trseq), a.itnbr, a.itdsc,a.itcls, d.clsdsc,b.syscode,  b.wareh, c.whdsc, 
		 convert(varchar(10),b.trnqy1),b.unmsr1, convert(varchar(10),b.tramt),   b.userno,
		 case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end as iocodedsc,  
	 (b.hmark1 + ';' + b.hmark2 + ';' + f.mark1 + ';' + f.mark2 + ';' + f.mark3 + ';'       + f.mark4) as mark, 
	 e.reskind,
	 (select y.cdesc 
		 from jnerp..miscode y where y.ckind=e.reskind and y.code=b.rescode) as resdsc ,'' 'cpb','' 'qyb'
		 FROM jnerp..invmas  a  
		 left outer join jnerp..invcls d on a.itcls=d.itcls,
		  jnerp..invtrnh b 
		 left outer join jnerp..invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
		 left outer join jnerp..invdou e on e.trtype=b.trtype  
		 left outer join jnerp..invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
		 WHERE ( b.itnbr = a.itnbr )   
		 and  b.facno='J' and b.prono='1' 
		 and (b.trtype in ('IAF','IAG')) 
		  and (b.trdate>='{0}' and b.trdate<'{1}')
union all
		 SELECT b.trtype,e.typedsc,b.depno, (select x.cdesc 
	 from test..miscode x where x.ckind=e.depdsckind and x.code=b.depno ) as depdsc, 
	 b.trno, convert(char(8),b.trdate,112) as trdate , convert(varchar(4),b.trseq), a.itnbr, a.itdsc,a.itcls, d.clsdsc,b.syscode,  b.wareh, c.whdsc,
	 convert(varchar(10),b.trnqy1),b.unmsr1, convert(varchar(10),b.tramt),   b.userno,
		case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end as iocodedsc,  
	 (b.hmark1 + ';' + b.hmark2 + ';' + f.mark1 + ';' + f.mark2 + ';' + f.mark3 + ';'       + f.mark4) as mark, 
	 e.reskind,
	 (select y.cdesc from test.dbo.miscode y where y.ckind=e.reskind and y.code=b.rescode) as resdsc ,'' 'cpb','' 'qyb'
	 FROM test.dbo.invmas  a  
		 left outer join .dbo.invcls d on a.itcls=d.itcls,
		 test..invtrnh b 
	 left outer join test.dbo.invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
	 left outer join test.dbo.invdou e on e.trtype=b.trtype  
	 left outer join test.dbo.invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
	 WHERE ( b.itnbr = a.itnbr )   
		 and  b.facno='C' and b.prono='1' 
		 and (b.trtype in ('IAF','IAG')) 
		 and (b.trdate>='{0}' and b.trdate<'{1}')
union all
		 SELECT b.trtype,e.typedsc,b.depno, (select x.cdesc 
	 from test..miscode x where x.ckind=e.depdsckind and x.code=b.depno ) as depdsc, 
	 b.trno, convert(char(8),b.trdate,112) as trdate , convert(varchar(4),b.trseq), a.itnbr, a.itdsc,a.itcls, d.clsdsc,b.syscode,  b.wareh, c.whdsc,
	 convert(varchar(10),b.trnqy1),b.unmsr1, convert(varchar(10),b.tramt),   b.userno,
	 case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end as iocodedsc,  
	 (b.hmark1 + ';' + b.hmark2 + ';' + f.mark1 + ';' + f.mark2 + ';' + f.mark3 + ';'       + f.mark4) as mark, 
	 e.reskind,
	 (select y.cdesc from test.dbo.miscode y where y.ckind=e.reskind and y.code=b.rescode) as resdsc ,'' 'cpb','' 'qyb'
	 FROM test.dbo.invmas  a  
		 left outer join .dbo.invcls d on a.itcls=d.itcls,
		 test..invtrnh b 
	 left outer join test.dbo.invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
	 left outer join test.dbo.invdou e on e.trtype=b.trtype  
	 left outer join test.dbo.invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
	 WHERE ( b.itnbr = a.itnbr )   
		 and  b.facno='C' and b.prono='1' 
		 and (b.trtype in ('IAB','IAA')) 
		 and (b.trdate>='{0}' and b.trdate<'{1}')
union all
		 SELECT b.trtype,e.typedsc,b.depno,
		 (select x.cdesc 
		 from comererp..miscode x where x.ckind=e.depdsckind and x.code=b.depno ) as depdsc, 
	 b.trno, convert(char(8),b.trdate,112) as trdate , convert(varchar(4),b.trseq), a.itnbr, a.itdsc,a.itcls, d.clsdsc,b.syscode,  b.wareh, c.whdsc, 
		 convert(varchar(10),b.trnqy1),b.unmsr1, convert(varchar(10),b.tramt),   b.userno,
		 case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end as iocodedsc,  
	 (b.hmark1 + ';' + b.hmark2 + ';' + f.mark1 + ';' + f.mark2 + ';' + f.mark3 + ';'       + f.mark4) as mark, 
	 e.reskind,
	 (select y.cdesc from comererp..miscode y where y.ckind=e.reskind and y.code=b.rescode) as resdsc ,'' 'cpb','' 'qyb'
		 FROM comererp..invmas  a  
		 left outer join comererp..invcls d on a.itcls=d.itcls,
		 comererp..invtrnh b 
		 left outer join comererp..invwh c on b.facno=c.facno and b.prono=c.prono and b.wareh=c.wareh   
		 left outer join comererp..invdou e on e.trtype=b.trtype  
		 left outer join comererp..invhdsc f on f.facno=b.facno and f.prono=b.prono and f.trno=b.trno 
		 WHERE  b.itnbr = a.itnbr   
		 and  (e.syscode = '10' And  e.reskind is not null and          
         ltrim(e.reskind) <> '' And (e.iocode in ('1','2') or (e.iocode = '3'  and e.trtype in ('IAF','IAG')))
         and c.costyn = 'Y')
		 and e.depdsckind = 'CA'		 
		 and b.trdate>='{0}' and b.trdate<'{1}'", datefirst.ToString("yyyyMMdd"), datelast.ToString("yyyyMMdd"));
            #endregion
            dt = dbhC.Query(connSybaseC, sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string no = dt.Rows[i]["trno"].ToString();
                    System.Data.DataRow[] ro = this.DtMsg.Select(string.Format("no='{0}'", no));
                    if (ro.Length < 1)
                    {
                        dt.Rows.RemoveAt(i);
                        i--;
                        continue;
                    }
                    dt.Rows[i]["cpb"] = ro[0]["cpb"];
                    dt.Rows[i]["qyb"] = ro[0]["qyb"];
                }
                System.Data.DataRow dr = dt.NewRow();
                //单据别	单据名称	对像别	对像名称	交易单号	交易日期	
                //序号	件号	名称	大类	大类名称	归类	库号	
                //仓库名称	数量	单位	金额	 录入人员 	出入库	备注	
                //原因别	原因内容	产品别	区域

                dr[0] = "单据别"; dr[1] = "单据名称"; dr[2] = "对像别"; dr[3] = "对像名称"; dr[4] = "交易单号"; dr[5] = "交易日期"; dr[6] = "序号"; dr[7] = "件号";
                dr[8] = "名称"; dr[9] = "大类"; dr[10] = "大类名称"; dr[11] = "归类"; dr[12] = "库号"; dr[13] = "仓库名称"; dr[14] = "数量"; dr[15] = "单位"; dr[16] = "金额"; dr[17] = "录入人员";
                dr[18] = "出入库"; dr[19] = "备注"; dr[20] = "原因别"; dr[21] = "原因内容"; dr[22] = "产品别"; dr[23] = "区域别";
                dt.Rows.InsertAt(dr, 0);
                ds.Tables.Add(dt);

            }
            #endregion

            #region 制令
            //            sql = string.Format(@"select a.facno,a.prono,a.manno,case a.typecode when '02' then '重工制令' end ,a.itnbrf,a.itdsc,a.cusno,a.cusna,b.depno,b.trtype,case b.trtype when 'MBA' then '整批领料单' when 'MBB' then '零星领料单' end 'trtypedsc',b.trno,convert(varchar(8),b.trdate,112) 'trdate',
            //                b.itnbr,invmas.itdsc,convert(varchar(10),b.trnqy1),b.unmsr1,convert(varchar(10),b.ttmatm),case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end,b.wareh,(select whdsc from invwh where wareh=b.wareh) as whdsc,'' cpb,'' qyb,'' from(SELECT 	manpih.facno,
            //					manpih.prono,manpih.manno,manmas.typecode,manpih.itnbrf,y.itdsc,manpih.issdepno,manpih.trtype,manpih.pisno,manpih.iocode,manpid.seqnr,manmas.cusno,cdrcus.cusna,manmas.kfdh FROM manpid left outer join invmas x on x.itnbr=manpid.altitnbr,manpih left outer join invmas y on y.itnbr=manpih.itnbrf         
            //				left outer join manmas on manmas.facno=manpih.facno	and  manmas.prono=manpih.prono and manmas.manno=manpih.manno left outer join cdrcus on cdrcus.cusno=manmas.cusno WHERE ( manpih.facno = manpid.facno ) and ( manpih.prono = manpid.prono ) and ( manpih.pisno = manpid.pisno ) and (( manpih.issstatus = 'C' ))	and manpid.altitnbr<>'3188-GBR6254-FW'
            //		and manpih.facno='C' and manpih.prono='1' and manpid.wareh not in ('NG02','KF01','KF05','KF03') and manmas.typecode='02' and (manpih.issdepno='9900') and (convert(varchar(6),manpih.issdate,112)='{0}') UNION SELECT manreh.facno, manreh.prono,manreh.manno,manmas.typecode,manreh.itnbrf, t.itdsc as  itnbrfdsc,manreh.retdepno,manreh.trtype,manreh.retno,
            //			manreh.iocode, manred.seqnr,manmas.cusno,cdrcus.cusna,manmas.kfdh FROM manred left outer join invmas s on s.itnbr=manred.altitnbr,manreh left outer join invmas t on t.itnbr=manreh.itnbrf left outer join manmas on manmas.facno=manreh.facno and  manmas.prono=manreh.prono and manmas.manno=manreh.manno left outer join cdrcus on cdrcus.cusno=manmas.cusno       
            //WHERE ( manreh.facno = manred.facno ) and ( manreh.prono = manred.prono ) and ( manreh.retno = manred.retno ) and (( manreh.issstatus = 'C' ) ) and manred.altitnbr<>'3188-GBR6254-FW' and manreh.facno='C' and manreh.prono='1' and manred.wareh not in ('NG02','KF01','KF05','KF03') and manmas.typecode='02' and (manreh.retdepno='9900' ) and (convert(varchar(6),manreh.retdate,112)='{0}')
            //) a left join invtrnh b  on a.facno=b.facno and a.trtype=b.trtype and a.pisno=b.trno and a.seqnr=b.trseq left join invmas on invmas.itnbr=b.itnbr where b.facno='C' and b.prono='1'", datefirst.ToString("yyyyMM"));
            sql = string.Format(@"select a.facno,a.prono,a.manno,case a.typecode when '02' then '重工制令' end ,a.itnbrf,a.itdsc,a.cusno,a.cusna,b.depno,b.trtype,case b.trtype when 'MBA' then '整批领料单' when 'MBB' then '零星领料单' end 'trtypedsc',b.trno,convert(varchar(8),b.trdate,112) 'trdate',
                b.itnbr,invmas.itdsc,convert(varchar(10),b.trnqy1),b.unmsr1,convert(varchar(10),b.ttmatm),case b. iocode when  '1'  then  '入库'  when '2'  then  '出库'  when '3'  then  '调拨'  end,b.wareh,(select whdsc from invwh where wareh=b.wareh) as whdsc,'' cpb,'' qyb,'' from(SELECT 	manpih.facno,
					manpih.prono,manpih.manno,manmas.typecode,manpih.itnbrf,y.itdsc,manpih.issdepno,manpih.trtype,manpih.pisno,manpih.iocode,manpid.seqnr,manmas.cusno,cdrcus.cusna,manmas.kfdh FROM manpid left outer join invmas x on x.itnbr=manpid.altitnbr,manpih left outer join invmas y on y.itnbr=manpih.itnbrf         
				left outer join manmas on manmas.facno=manpih.facno	and  manmas.prono=manpih.prono and manmas.manno=manpih.manno left outer join cdrcus on cdrcus.cusno=manmas.cusno WHERE ( manpih.facno = manpid.facno ) and ( manpih.prono = manpid.prono ) and ( manpih.pisno = manpid.pisno ) and (( manpih.issstatus = 'C' ))	and manpid.altitnbr<>'3188-GBR6254-FW'
		and manpih.facno='C' and manpih.prono='1' and manpid.wareh not in ('NG02','KF01','KF05','KF03','EKF01','EKF05','EKF03') and manmas.typecode='02' and (manpih.issdepno='9900') and (convert(varchar(6),manpih.issdate,112)='{0}') UNION SELECT manreh.facno, manreh.prono,manreh.manno,manmas.typecode,manreh.itnbrf, t.itdsc as  itnbrfdsc,manreh.retdepno,manreh.trtype,manreh.retno,
			manreh.iocode, manred.seqnr,manmas.cusno,cdrcus.cusna,manmas.kfdh FROM manred left outer join invmas s on s.itnbr=manred.altitnbr,manreh left outer join invmas t on t.itnbr=manreh.itnbrf left outer join manmas on manmas.facno=manreh.facno and  manmas.prono=manreh.prono and manmas.manno=manreh.manno left outer join cdrcus on cdrcus.cusno=manmas.cusno       
WHERE ( manreh.facno = manred.facno ) and ( manreh.prono = manred.prono ) and ( manreh.retno = manred.retno ) and (( manreh.issstatus = 'C' ) ) and manred.altitnbr<>'3188-GBR6254-FW' and manreh.facno='C' and manreh.prono='1' and manred.wareh not in ('NG02','KF01','KF05','KF03','EKF01','EKF05','EKF03') and manmas.typecode='02' and (manreh.retdepno='9900' ) and (convert(varchar(6),manreh.retdate,112)='{0}')
) a left join invtrnh b  on a.facno=b.facno and a.trtype=b.trtype and a.pisno=b.trno and a.seqnr=b.trseq left join invmas on invmas.itnbr=b.itnbr where b.facno='C' and b.prono='1'", datefirst.ToString("yyyyMM"));
            dt = dbhC.Query(connSybaseC, sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string no = dt.Rows[i]["manno"].ToString();
                    System.Data.DataRow[] ro = this.DtMsg.Select(string.Format("no='{0}'", no));
                    if (ro.Length < 1)
                    {
                        dt.Rows.RemoveAt(i);
                        i--;
                        continue;
                    }
                    dt.Rows[i]["cpb"] = ro[0]["cpb"];
                    dt.Rows[i]["qyb"] = ro[0]["qyb"];
                }
                //公司别	生产地	制令单号	制令等级	制令件号	名称	客户编号	客户简称	
                //领退料部门	单据别	单据名称  领退料单号	领退料日期	领退料件号	领退料件号名称	
                //数量	单位	金额	出入库	库号	仓库名称	产品别	区域
                System.Data.DataRow dr2 = dt.NewRow();
                dr2[0] = "公司别"; dr2[1] = "生产地"; dr2[2] = "制令单号"; dr2[3] = "制令等级"; dr2[4] = "制令件号"; dr2[5] = "名称"; dr2[6] = "客户编号"; dr2[7] = "客户简称";
                dr2[8] = "领退料部门"; dr2[9] = "单据别"; dr2[10] = "单据名称"; dr2[11] = "领退料单号"; dr2[12] = "领退料日期"; dr2[13] = "领退料件号"; dr2[14] = "领退料件号名称";
                dr2[15] = "数量"; dr2[16] = "单位"; dr2[17] = "金额";
                dr2[18] = "出入库"; dr2[19] = "库号"; dr2[20] = "仓库名称"; dr2[21] = "产品别"; dr2[22] = "区域"; dr2[23] = "";
                dt.Rows.InsertAt(dr2, 0);
                ds.Tables.Add(dt);

            }
            #endregion

            #region 质量扣款
            sql = string.Format(@"SELECT a.recno,Convert(VARCHAR(20),ABS(SUM(ISNULL(b.recamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM test.dbo.armrech AS a,test.dbo.armrec AS b WHERE a.facno=b.facno AND a.recno=b.recno AND b.rectype='1'AND a.zlk='Y'
                                                AND a.recstat='1' AND convert(VARCHAR(6),a.ltrndate,112)='{0}' GROUP BY b.recno,a.hmark1,a.hmark2
                                                UNION ALL
                                                SELECT a.trno,Convert(VARCHAR(20),ABS(SUM(ISNULL(a.pmamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM test.dbo.armpmm a WHERE a.zlk='Y' AND convert(VARCHAR(6),a.trdat,112)='{0}' GROUP BY a.hmark1,a.hmark2,trno
                                                UNION ALL
                                                SELECT a.recno,Convert(VARCHAR(20),ABS(SUM(ISNULL(b.recamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM gzerp.dbo.armrech AS a,gzerp.dbo.armrec AS b WHERE a.facno=b.facno AND a.recno=b.recno AND b.rectype='1'AND a.zlk='Y'
                                                AND a.recstat='1' AND convert(VARCHAR(6),a.ltrndate,112)='{0}' GROUP BY b.recno,a.hmark1,a.hmark2
                                                UNION ALL
                                                SELECT a.trno,Convert(VARCHAR(20),ABS(SUM(ISNULL(a.pmamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM gzerp.dbo.armpmm a WHERE a.zlk='Y' AND convert(VARCHAR(6),a.trdat,112)='{0}' GROUP BY a.hmark1,a.hmark2,trno
                                                UNION ALL 
                                                SELECT a.recno,Convert(VARCHAR(20),ABS(SUM(ISNULL(b.recamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM njerp.dbo.armrech AS a,njerp.dbo.armrec AS b WHERE a.facno=b.facno AND a.recno=b.recno AND b.rectype='1'AND a.zlk='Y'
                                                AND a.recstat='1' AND convert(VARCHAR(6),a.ltrndate,112)='{0}' GROUP BY b.recno,a.hmark1,a.hmark2
                                                UNION ALL
                                                SELECT a.trno,Convert(VARCHAR(20),ABS(SUM(ISNULL(a.pmamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM njerp.dbo.armpmm a WHERE a.zlk='Y' AND convert(VARCHAR(6),a.trdat,112)='{0}' GROUP BY a.hmark1,a.hmark2,trno
                                                UNION ALL
                                                SELECT a.recno,Convert(VARCHAR(20),ABS(SUM(ISNULL(b.recamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM jnerp.dbo.armrech AS a,jnerp.dbo.armrec AS b WHERE a.facno=b.facno AND a.recno=b.recno AND b.rectype='1'AND a.zlk='Y'
                                                AND a.recstat='1' AND convert(VARCHAR(6),a.ltrndate,112)='{0}' GROUP BY b.recno,a.hmark1,a.hmark2
                                                UNION ALL
                                                SELECT a.trno,Convert(VARCHAR(20),ABS(SUM(ISNULL(a.pmamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM jnerp.dbo.armpmm a WHERE a.zlk='Y' AND convert(VARCHAR(6),a.trdat,112)='{0}' GROUP BY a.hmark1,a.hmark2,trno
                                                UNION ALL
                                                SELECT a.recno,Convert(VARCHAR(20),ABS(SUM(ISNULL(b.recamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM comererp.dbo.armrech AS a,comererp.dbo.armrec AS b WHERE a.facno=b.facno AND a.recno=b.recno AND b.rectype='1'AND a.zlk='Y'
                                                AND a.recstat='1' AND convert(VARCHAR(6),a.ltrndate,112)='{0}' GROUP BY b.recno,a.hmark1,a.hmark2
                                                UNION ALL
                                                SELECT a.trno,Convert(VARCHAR(20),ABS(SUM(ISNULL(a.pmamt,0)))) as cost,a.hmark1 as cpbie,a.hmark2 as qybie ,'','','','','','','','','','','','','','','','','','','',''
                                                FROM comererp.dbo.armpmm a WHERE a.zlk='Y' AND convert(VARCHAR(6),a.trdat,112)='{0}' GROUP BY a.hmark1,a.hmark2,trno", datefirst.ToString("yyyyMM"));
            dt = dbhC.Query(connSybaseC, sql);
            if (dt.Rows.Count > 0)
            {
                System.Data.DataRow dr4 = dt.NewRow();
                dr4[0] = "单号"; dr4[1] = "金额"; dr4[2] = "产品别"; dr4[3] = "区域别"; dr4[4] = ""; dr4[5] = ""; dr4[6] = ""; dr4[7] = "";
                dr4[8] = ""; dr4[9] = ""; dr4[10] = ""; dr4[11] = ""; dr4[12] = ""; dr4[13] = ""; dr4[14] = ""; dr4[15] = ""; dr4[16] = ""; dr4[17] = "";
                dr4[18] = ""; dr4[19] = ""; dr4[20] = ""; dr4[21] = ""; dr4[22] = ""; dr4[23] = "";
                dt.Rows.InsertAt(dr4, 0);
                ds.Tables.Add(dt);
            }
            #endregion

            #region 销售折让
            sql = string.Format(@"SELECT a.cdrno,Convert(VARCHAR(20),a.discount) AS cost,a.hmark1 AS cpbie,CASE WHEN a.depno LIKE '1D5%' THEN 'CQ' ELSE 
                                                CASE a.areacode 	WHEN 'HD1' THEN 'NJ' when 'HD2' THEN 'HD' WHEN 'HZ' THEN '' WHEN 'JN01' THEN 'HB' 
                                                WHEN 'NJ01' THEN 'NJ' WHEN 'SH01' THEN 'HD' WHEN 'GZ01' THEN 'HN' ELSE a.areacode END END AS qybie,a.areacode,
                                                '','','','','','','','','','','','','','','','','','',''
                                                FROM test.dbo.cdrhmas as a WHERE a.hmark1 IS NOT NULL and a.discount>0 AND Convert(VARCHAR(6),a.cfmdate,112)='{0}'
                                                union all
                                                SELECT a.cdrno,Convert(VARCHAR(20),a.discount) AS cost,a.hmark1 AS cpbie,CASE WHEN a.depno LIKE '1D5%' THEN 'CQ' ELSE 
                                                CASE a.areacode 	WHEN 'HD1' THEN 'NJ' when 'HD2' THEN 'HD' WHEN 'HZ' THEN '' WHEN 'JN01' THEN 'HB' 
                                                WHEN 'NJ01' THEN 'NJ' WHEN 'SH01' THEN 'HD' WHEN 'GZ01' THEN 'HN' ELSE a.areacode END END AS qybie,a.areacode,
                                                '','','','','','','','','','','','','','','','','','',''
                                                FROM gzerp.dbo.cdrhmas as a WHERE a.hmark1 IS NOT NULL and a.discount>0 AND Convert(VARCHAR(6),a.cfmdate,112)='{0}'
                                                union all
                                                SELECT a.cdrno,Convert(VARCHAR(20),a.discount) AS cost,a.hmark1 AS cpbie,CASE WHEN a.depno LIKE '1D5%' THEN 'CQ' ELSE 
                                                CASE a.areacode 	WHEN 'HD1' THEN 'NJ' when 'HD2' THEN 'HD' WHEN 'HZ' THEN '' WHEN 'JN01' THEN 'HB' 
                                                WHEN 'NJ01' THEN 'NJ' WHEN 'SH01' THEN 'HD' WHEN 'GZ01' THEN 'HN' ELSE a.areacode END END AS qybie,a.areacode,
                                                '','','','','','','','','','','','','','','','','','',''
                                                FROM njerp.dbo.cdrhmas as a WHERE a.hmark1 IS NOT NULL and a.discount>0 AND Convert(VARCHAR(6),a.cfmdate,112)='{0}'
                                                union all
                                                SELECT a.cdrno,Convert(VARCHAR(20),a.discount) AS cost,a.hmark1 AS cpbie,CASE WHEN a.depno LIKE '1D5%' THEN 'CQ' ELSE 
                                                CASE a.areacode 	WHEN 'HD1' THEN 'NJ' when 'HD2' THEN 'HD' WHEN 'HZ' THEN '' WHEN 'JN01' THEN 'HB' 
                                                WHEN 'NJ01' THEN 'NJ' WHEN 'SH01' THEN 'HD' WHEN 'GZ01' THEN 'HN' ELSE a.areacode END END AS qybie,a.areacode,
                                                '','','','','','','','','','','','','','','','','','',''
                                                FROM jnerp.dbo.cdrhmas as a WHERE a.hmark1 IS NOT NULL and a.discount>0 AND Convert(VARCHAR(6),a.cfmdate,112)='{0}'
                                                union all
                                                SELECT a.cdrno,Convert(VARCHAR(20),a.discount) AS cost,a.hmark1 AS cpbie,CASE WHEN a.depno LIKE '1D5%' THEN 'CQ' ELSE 
                                                CASE a.areacode 	WHEN 'HD1' THEN 'NJ' when 'HD2' THEN 'HD' WHEN 'HZ' THEN '' WHEN 'JN01' THEN 'HB' 
                                                WHEN 'NJ01' THEN 'NJ' WHEN 'SH01' THEN 'HD' WHEN 'GZ01' THEN 'HN' ELSE a.areacode END END AS qybie,a.areacode,
                                                '','','','','','','','','','','','','','','','','','',''
                                                FROM comererp.dbo.cdrhmas as a WHERE a.hmark1 IS NOT NULL and a.discount>0 AND Convert(VARCHAR(6),a.cfmdate,112)='{0}'", datefirst.ToString("yyyyMM"));
            dt = dbhC.Query(connSybaseC, sql);
            if (dt.Rows.Count > 0)
            {

                System.Data.DataRow dr5 = dt.NewRow();
                dr5[0] = "单号"; dr5[1] = "金额"; dr5[2] = "产品别"; dr5[3] = "区域别"; dr5[4] = ""; dr5[5] = ""; dr5[6] = ""; dr5[7] = "";
                dr5[8] = ""; dr5[9] = ""; dr5[10] = ""; dr5[11] = ""; dr5[12] = ""; dr5[13] = ""; dr5[14] = ""; dr5[15] = ""; dr5[16] = ""; dr5[17] = "";
                dr5[18] = ""; dr5[19] = ""; dr5[20] = ""; dr5[21] = ""; dr5[22] = ""; dr5[23] = "";
                dt.Rows.InsertAt(dr5, 0);
                ds.Tables.Add(dt);
            }
            #endregion

            #region bsc_tb_mfservice  免费服务金额中间表
            sql = string.Format(@"select protype,areatype,convert(varchar(20),mancost),convert(varchar(20),wxll),convert(varchar(20),fwll),convert(varchar(20),travelcost)
                                                ,convert(varchar(20),fare),convert(varchar(20),zlkk),convert(varchar(20),xszr),'' as total,'','','','','','','','','','','','','','','' 
                                                from bsc_tb_mfservice where month={0} and year={1}", Month, Year);
            dt = dbhC.Query(connSybaseC, sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["total"] = Convert.ToDecimal(dt.Rows[i][2]) + Convert.ToDecimal(dt.Rows[i][3]) + Convert.ToDecimal(dt.Rows[i][4]) + Convert.ToDecimal(dt.Rows[i][5]) + Convert.ToDecimal(dt.Rows[i][6]) + Convert.ToDecimal(dt.Rows[i][7]) + Convert.ToDecimal(dt.Rows[i][8]);
            }
            System.Data.DataRow dr3 = dt.NewRow();
            //产品别 	 区域别 	 制令 	 维修 	 服务 	 差旅 	 运费 	 质量扣款 	 合计 
            dr3[0] = "产品别"; dr3[1] = "区域别"; dr3[2] = "制令"; dr3[3] = "维修"; dr3[4] = "服务"; dr3[5] = "差旅"; dr3[6] = "运费"; dr3[7] = "质量扣款";
            dr3[8] = "销售折让"; dr3[9] = "合计"; dr3[10] = ""; dr3[11] = ""; dr3[12] = ""; dr3[13] = ""; dr3[14] = ""; dr3[15] = ""; dr3[16] = ""; dr3[17] = "";
            dr3[18] = ""; dr3[19] = ""; dr3[20] = ""; dr3[21] = ""; dr3[22] = ""; dr3[23] = "";
            dt.Rows.InsertAt(dr3, 0);
            ds.Tables.Add(dt);
            #endregion

            return ds;
        }


        #endregion

        /// <summary>
        /// 日志log填写
        /// </summary>
        /// <param name="errorlog"></param>
        public void Wrlog(string txt, string filename)
        {
            //D:\C1150XP\C1150\BSC2\WebSite\Rdlc\FWServiceCost
            string strFilePath = "F:\\OldSystem\\C1150XP\\C1150\\BSC2\\WebSite\\Rdlc\\FWServiceCost\\" + filename;
            StringBuilder strLog = new StringBuilder();
            strLog.Append(txt);
            System.IO.File.AppendAllText(strFilePath, txt.ToString(), Encoding.Default);
        }

        //public void WriteLogs(string fileName, string txt)
        //{

        //    if (string.IsNullOrEmpty(fileName)) return;

        //    string hostName = "D:\\EasyFlow\\AppData\\ErrorLog\\" + this.formID;
        //    if (!System.IO.Directory.Exists(hostName))
        //    {
        //        System.IO.Directory.CreateDirectory(hostName);
        //    }
        //    string filePath = hostName + "\\" + fileName + ".txt";
        //    StringBuilder strLog = new StringBuilder();
        //    try
        //    {


        //        strLog.AppendFormat(">>>----------------------------------------\r\n");
        //        strLog.Append(txt + "\r\n");
        //        strLog.AppendFormat(">>>----------------------------------------\r\n");
        //        System.IO.File.AppendAllText(filePath, strLog.ToString(), Encoding.Default);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //}


    }
}
