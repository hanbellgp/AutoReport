using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hanbell.AutoReport.Core;
using System.Data;

namespace Hanbell.AutoReport.Config
{

    public class SpecOrderNotificationConfig : NotificationConfig
    {
        public SpecOrderNotificationConfig()
        {
        }

        public SpecOrderNotificationConfig(DBServerType dbType, string connName, string notification)
        {
            PrepareDBUtil(dbType, Base.GetDBConnectionString(connName));
            this.ds = new DSSpecOrderNotification();
            this.reportList.Add(new SpecOrderNotificationReport());
            this.args = Base.GetParameter(notification, this.ToString());
        }

        public override void InitData()
        {

            ResetSpecialOrderDetailState();

            string sqlstr = "SELECT cdrspec.id,cdrspec.projname as project,cdrspec.itnbr as prod,cdrspec.itdsc as prodname,cdrspec.shipday1,cdrspec.shipday2,cdrspec.shipid," +
                "cdrspec.manday1,cdrspec.manday2,cdrspec.finday1,cdrspec.finday2,cdrspec.manid," +
                "cdrspecdetail.seq,cdrspecdetail.itdsc,cdrspecdetail.itnbr,cdrspecdetail.qty,cdrspecdetail.recqty,cdrspecdetail.rdman,cdrspecdetail.rdday1,cdrspecdetail.rdday2," +
                "cdrspecdetail.prman,cdrspecdetail.prday1,cdrspecdetail.prday2,cdrspecdetail.prno,cdrspecdetail.purman,cdrspecdetail.purday1,cdrspecdetail.purday2," +
                "cdrspecdetail.accday2,cdrspecdetail.recday2,cdrspecdetail.acceptno FROM cdrspec,cdrspecdetail   " +
                "WHERE ( cdrspec.facno = cdrspecdetail.facno ) and  ( cdrspec.id = cdrspecdetail.id ) and (cdrspec.status<>'C') and (cdrspecdetail.state=0) " +
                "ORDER BY cdrspec.facno ASC, cdrspec.id ASC    ";
            Fill(sqlstr, ds, "tblcdrspec");

            Fill("select userno,username from secuser ", ds, "tbluser");

        }

        public override void ConfigData()
        {
            if (ds.Tables["tblcdrspec"].Rows.Count == 0) return;

            foreach (DataRow row in ds.Tables["tblcdrspec"].Rows)
            {
                foreach (DataRow user in ds.Tables["tbluser"].Rows)
                {
                    if (row["rdman"].Equals(user["userno"]))
                    {
                        row["rdman"] = user["username"];
                    }
                    if (row["prman"].Equals(user["userno"]))
                    {
                        row["prman"] = user["username"];
                    }
                    if (row["purman"].Equals(user["userno"]))
                    {
                        row["purman"] = user["username"];
                    }
                }
            }
            string optstr = "";
            DataRow newRow;
            foreach (DataRow row in ds.Tables["tblcdrspec"].Rows)
            {

                newRow = ds.Tables["tblresults"].NewRow();
                newRow["id"] = (optstr == row["id"].ToString() ? "" : row["id"]);
                newRow["project"] = (optstr == row["id"].ToString() ? "" : row["project"]);
                newRow["mark"] = (optstr == row["id"].ToString() ? "" : "制令");
                newRow["plan"] = (optstr == row["id"].ToString() ? DBNull.Value : row["manday1"]);
                newRow["fact"] = (optstr == row["id"].ToString() ? DBNull.Value : row["manday2"]);
                newRow["formid"] = (optstr == row["id"].ToString() ? "" : row["manid"].ToString());
                newRow["seq"] = row["seq"];
                newRow["itdsc"] = row["itdsc"];
                newRow["qty"] = row["qty"];
                newRow["recqty"] = row["recqty"];
                newRow["dutydept"] = "技术";
                newRow["dutyman"] = row["rdman"];
                newRow["dplan"] = row["rdday1"];
                newRow["dfact"] = row["rdday2"];
                ds.Tables["tblresults"].Rows.Add(newRow);

                newRow = ds.Tables["tblresults"].NewRow();
                newRow["id"] = "";
                newRow["project"] = (optstr == row["id"].ToString() ? "" : row["prodname"]);
                newRow["mark"] = (optstr == row["id"].ToString() ? "" : "入库");
                newRow["plan"] = (optstr == row["id"].ToString() ? DBNull.Value : row["finday1"]);
                newRow["fact"] = (optstr == row["id"].ToString() ? DBNull.Value : row["finday2"]);
                newRow["formid"] = (optstr == row["id"].ToString() ? "" : row["manid"].ToString());
                newRow["seq"] = DBNull.Value;
                newRow["itdsc"] = row["itnbr"];
                newRow["qty"] = DBNull.Value;
                newRow["recqty"] = DBNull.Value;
                newRow["dutydept"] = "生管";
                newRow["dutyman"] = row["prman"];
                newRow["dplan"] = row["prday1"];
                newRow["dfact"] = row["prday2"];
                ds.Tables["tblresults"].Rows.Add(newRow);

                newRow = ds.Tables["tblresults"].NewRow();
                newRow["id"] = "";
                newRow["project"] = (optstr == row["id"].ToString() ? "" : row["prod"]);
                newRow["mark"] = (optstr == row["id"].ToString() ? "" : "出货");
                newRow["plan"] = (optstr == row["id"].ToString() ? DBNull.Value : row["shipday1"]);
                newRow["fact"] = (optstr == row["id"].ToString() ? DBNull.Value : row["shipday2"]);
                newRow["formid"] = (optstr == row["id"].ToString() ? "" : row["shipid"].ToString());
                newRow["seq"] = DBNull.Value;
                newRow["itdsc"] = row["remark"]; ;
                newRow["qty"] = DBNull.Value;
                newRow["recqty"] = DBNull.Value;
                newRow["dutydept"] = "采购";
                newRow["dutyman"] = row["purman"];
                newRow["dplan"] = row["purday1"];
                newRow["dfact"] = row["purday2"];
                ds.Tables["tblresults"].Rows.Add(newRow);

                optstr = row["id"].ToString();

            }
            DateTime date = DateTime.Now.Date;
            foreach (DataRow row in ds.Tables["tblresults"].Rows)
            {
                if (row["plan"].ToString() == "" || row["dplan"].ToString() == "")
                {
                    continue;
                }
                if (row["fact"] != DBNull.Value)
                {
                    row["delay"] = (DateTime.Parse(row["plan"].ToString()).Date - DateTime.Parse(row["fact"].ToString()).Date).Days;
                }
                else if (row["fact"] == DBNull.Value && row["plan"] != DBNull.Value && DateTime.Parse(row["plan"].ToString()).Date < date.Date)
                {
                    row["delay"] = (DateTime.Parse(row["plan"].ToString()).Date - date).Days;
                }
                if (row["dfact"] != DBNull.Value)
                {
                    row["ddelay"] = (DateTime.Parse(row["dplan"].ToString()).Date - DateTime.Parse(row["dfact"].ToString()).Date).Days;
                }
                else if (row["dfact"] == DBNull.Value && DateTime.Parse(row["dplan"].ToString()).Date < date.Date)
                {
                    row["ddelay"] = (DateTime.Parse(row["dplan"].ToString()).Date - date).Days;
                }
            }

            ds.AcceptChanges();
        }

        protected void ResetSpecialOrderState()
        {
            string[] orderid;
            string cdrno, manno, shpno;
            int ctrseq, state;
            DateTime mandate, findate, shpdate;
            DataTable tblcdrspec;
            DataTable tblmanmas;
            DataTable tblcdrhad;

            StringBuilder sb1 = new StringBuilder();
            sb1.Append("SELECT manmas.manno,manmas.issdate,manmas.finqty,manmas.findate,mancdrmap.fixnr,mancdrmap.varnr,mancdrmap.mapqty FROM manmas,mancdrmap ");
            sb1.Append(" WHERE (manmas.facno = mancdrmap.facno) AND (manmas.manno = mancdrmap.manno) AND (manmas.manstatus <> 'X') AND  ");
            sb1.Append("(manmas.facno = '{0}') AND (manmas.itnbrf = '{1}') AND  (mancdrmap.cdrno = '{2}') AND (mancdrmap.ctrseq = {3}) ");

            StringBuilder sb2 = new StringBuilder();
            sb2.Append(" select cdrhad.shpno,cdrhad.shpdate from cdrhad,cdrdta where cdrhad.facno=cdrdta.facno and cdrhad.shpno=cdrdta.shpno and cdrhad.houtsta='Y' ");
            sb2.Append(" and cdrdta.facno='{0}' and cdrdta.cdrno='{1}' and cdrdta.ctrseq={2} ");

            string updatesql11 = "update cdrspec set manid='{0}',manday2='{1}' where facno='{2}' and id='{3}' ";
            string updatesql12 = "update cdrspec set finday2='{0}' where facno='{1}' and id='{2}' ";

            string updatesql21 = "update cdrspec set shipid='{0}',shipday2='{1}' where facno='{2}' and id='{3}' ";

            tblcdrspec = GetQueryTable("select facno,id,orderid,itnbr from cdrspec where status='Y' and isnull(orderid,'')<>'' and (finday2 is null) ");
            foreach (DataRow row in tblcdrspec.Rows)
            {
                orderid = Base.GetSeparatedContent(row["orderid"].ToString(), "-");
                if (orderid.Length != 2) continue;

                cdrno = orderid[0].ToString();
                ctrseq = int.Parse(orderid[1].ToString());

                tblmanmas = GetQueryTable(string.Format(sb1.ToString(), row["facno"].ToString(), row["itnbr"].ToString(), cdrno, ctrseq));
                if ((tblmanmas != null) && (tblmanmas.Rows.Count > 0))
                {
                    manno = "";
                    mandate = new DateTime();
                    findate = new DateTime();
                    foreach (DataRow item in tblmanmas.Rows)
                    {
                        manno += item["manno"].ToString() + ";";
                        if (item["issdate"].ToString() != "")
                        {
                            mandate = DateTime.Parse(item["issdate"].ToString());
                        }
                        if (item["findate"].ToString() != "")
                        {
                            findate = DateTime.Parse(item["findate"].ToString());
                        }
                    }
                    if (mandate.Year != 0001)
                    {
                        ExecSql(string.Format(updatesql11, manno, mandate, row["facno"], row["id"]));
                    }
                    if (findate.Year != 0001)
                    {
                        ExecSql(string.Format(updatesql12, findate, row["facno"], row["id"]));
                    }
                }
                tblmanmas.Dispose();
            }

            tblcdrspec = GetQueryTable("select facno,id,orderid,itnbr from cdrspec where status='Y' and isnull(orderid,'')<>'' and (shipday2 is null) ");
            foreach (DataRow row in tblcdrspec.Rows)
            {
                orderid = Base.GetSeparatedContent(row["orderid"].ToString(), "-");
                if (orderid.Length != 2) continue;

                cdrno = orderid[0].ToString();
                ctrseq = int.Parse(orderid[1].ToString());

                tblcdrhad = GetQueryTable(string.Format(sb2.ToString(), row["facno"].ToString(), cdrno, ctrseq));
                if ((tblcdrhad != null) && (tblcdrhad.Rows.Count > 0))
                {
                    shpno = "";
                    shpdate = new DateTime();
                    foreach (DataRow item in tblcdrhad.Rows)
                    {
                        shpno += item["shpno"].ToString() + ";";
                        shpdate = DateTime.Parse(item["shpdate"].ToString());
                    }
                    if (shpdate.Year != 1)
                    {
                        ExecSql(string.Format(updatesql21, shpno, shpdate, row["facno"], row["id"]));
                    }

                }
                tblcdrhad.Dispose();
            }


        }

        protected void ResetSpecialOrderDetailState()
        {
            string acceptno;
            DateTime acceptdate, recivedate;
            decimal qty;
            int state;
            StringBuilder puracd = new StringBuilder();
            puracd.Append("select b.pono,e.acceptno,e.acceptdate,e.recivedate,e.okqy1 from  purdta b,purdtamap c,purdask d,puracd e ");
            puracd.Append(" where  b.facno=c.facno and b.pono=c.pono and b.trseq=c.trseq and  c.facno=d.facno and c.srcno=d.prno and c.srcseq=d.trseq and b.facno=e.facno and  b.prono=e.prono and b.pono=e.pono and b.trseq=e.ponotrseq ");
            puracd.Append(" and e.accsta='Y' and e.okqy1>0 and d.prno='{0}' and d.itnbr='{1}' ");

            StringBuilder sfcwad = new StringBuilder();
            sfcwad.Append("SELECT sfcwad.facno,sfcwad.inpno,sfcwad.manno,sfcwad.itnbr,sfcwad.attqty1,sfcwah.indat,sfcwad.varnr,sfcwad.fixnr FROM sfcwad,sfcwah ");
            sfcwad.Append(" WHERE (sfcwah.facno = sfcwad.facno) AND (sfcwah.prono = sfcwad.prono) AND (sfcwah.inpno = sfcwad.inpno) AND (sfcwah.stats = '2') AND ");
            sfcwad.Append(" ((sfcwad.facno = '{0}') AND (sfcwad.manno = '{1}') AND (sfcwad.itnbr = '{2}'))  ");

            StringBuilder assacd = new StringBuilder();
            assacd.Append("SELECT puracd.facno,puracd.prono,puracd.acceptno,puracd.acceptdate,puracd.pono,puracd.ponotrseq,puracd.itnbr,puracd.recivedate,puracd.okqy1  ");
            assacd.Append(" FROM puracd,purdta,asspurhad ");
            assacd.Append(" WHERE (puracd.facno = purdta.facno) AND  (puracd.prono = purdta.prono) AND  (puracd.pono = purdta.pono) AND  (puracd.ponotrseq = purdta.trseq) AND  ");
            assacd.Append(" ((purdta.facno = asspurhad.facno) AND (purdta.prono = asspurhad.prono) AND (purdta.pono = asspurhad.pono)) AND ");
            assacd.Append(" ((puracd.accsta = 'Y') AND (puracd.okqy1 > 0) AND (asspurhad.prono = '1') AND ");
            assacd.Append(" (asspurhad.facno = '{0}') AND (asspurhad.manno = '{1}') AND  (asspurhad.itnbr ='{2}'))");

            string updatesql = "update cdrspecdetail set pono='{0}',acceptno='{1}',accday2='{2}',recday2='{3}',recqty={4},state={5} where id='{6}' and seq={7} ";

            DataTable tblcdrspecdetail = GetQueryTable("select facno,id,seq,itnbr,qty,prno,morpcode from cdrspecdetail where state=0 and isnull(itnbr,'')<>'' and isnull(prno,'')<>'' ");
            DataTable tblpuracd;
            DataTable tblsfcwad;
            DataTable tblassacd;

            foreach (DataRow row in tblcdrspecdetail.Rows)
            {
                //请购采购
                if (row["prno"].ToString().Substring(0, 1) == "N")
                {
                    //采购验收
                    tblpuracd = GetQueryTable(string.Format(puracd.ToString(), row["prno"].ToString(), row["itnbr"].ToString()));
                    if ((tblpuracd != null) && (tblpuracd.Rows.Count > 0))
                    {
                        acceptno = "";
                        acceptdate = new DateTime();
                        recivedate = new DateTime();
                        qty = 0;
                        foreach (DataRow item in tblpuracd.Rows)
                        {
                            acceptno += item["acceptno"].ToString() + ";";
                            acceptdate = DateTime.Parse(item["acceptdate"].ToString());
                            recivedate = DateTime.Parse(item["recivedate"].ToString());
                            qty += Decimal.Parse(item["okqy1"].ToString());
                        }
                        if (qty.Equals(Decimal.Parse(row["qty"].ToString())))
                        {
                            state = 1;
                        }
                        else
                        {
                            state = 0;
                        }
                        if (acceptdate.Year != 1 && recivedate.Year != 1)
                        {
                            ExecSql(string.Format(updatesql, tblpuracd.Rows[0]["pono"], acceptno, acceptdate, recivedate, qty, state, row["id"], row["seq"]));
                        }
                        tblpuracd.Dispose();
                    }
                }
                //生产制令
                if ((row["prno"].ToString().Substring(0, 1) == "M") && (row["morpcode"].ToString().Substring(0, 1) != "A"))
                {
                    //制令入库
                    tblsfcwad = GetQueryTable(string.Format(sfcwad.ToString(), row["facno"].ToString(), row["prno"].ToString(), row["itnbr"].ToString()));
                    if ((tblsfcwad != null) && (tblsfcwad.Rows.Count > 0))
                    {
                        acceptno = "";
                        acceptdate = new DateTime();
                        recivedate = new DateTime();
                        qty = 0;
                        foreach (DataRow item in tblsfcwad.Rows)
                        {
                            acceptno += item["inpno"].ToString() + ";";
                            acceptdate = DateTime.Parse(item["indat"].ToString());
                            recivedate = DateTime.Parse(item["indat"].ToString());
                            qty += Decimal.Parse(item["attqty1"].ToString());
                        }
                        if (qty.Equals(Decimal.Parse(row["qty"].ToString())))
                        {
                            state = 1;
                        }
                        else
                        {
                            state = 0;
                        }
                        if (acceptdate.Year != 1 && recivedate.Year != 1)
                        {
                            ExecSql(string.Format(updatesql, "", acceptno, acceptdate, recivedate, qty, state, row["id"], row["seq"]));
                        }
                        tblsfcwad.Dispose();
                    }
                }
                //托工制令
                if ((row["prno"].ToString().Substring(0, 1) == "M") && (row["morpcode"].ToString().Substring(0, 1) == "A"))
                {
                    //托工验收
                    tblassacd = GetQueryTable(string.Format(assacd.ToString(), row["facno"].ToString(), row["prno"].ToString(), row["itnbr"].ToString()));
                    if ((tblassacd != null) && (tblassacd.Rows.Count > 0))
                    {
                        acceptno = "";
                        acceptdate = new DateTime();
                        recivedate = new DateTime();
                        qty = 0;
                        foreach (DataRow item in tblassacd.Rows)
                        {
                            acceptno += item["acceptno"].ToString() + ";";
                            acceptdate = DateTime.Parse(item["acceptdate"].ToString());
                            recivedate = DateTime.Parse(item["recivedate"].ToString());
                            qty += Decimal.Parse(item["okqy1"].ToString());
                        }
                        if (qty.Equals(Decimal.Parse(row["qty"].ToString())))
                        {
                            state = 1;
                        }
                        else
                        {
                            state = 0;
                        }
                        if (acceptdate.Year != 1 && recivedate.Year != 1)
                        {
                            ExecSql(string.Format(updatesql, "", acceptno, acceptdate, recivedate, qty, state, row["id"], row["seq"]));
                        }
                        tblassacd.Dispose();
                    }
                }
            }
        }

    }
}