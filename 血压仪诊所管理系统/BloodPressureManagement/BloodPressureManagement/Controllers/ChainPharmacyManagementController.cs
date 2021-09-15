using BloodPressureManagement.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodPressureManagement.Controllers
{
    /// <summary>
    /// 连锁药房管理
    /// </summary>
    public class ChainPharmacyManagementController : Controller
    {
        BloodPressureManagementEntities db = new BloodPressureManagementEntities();
        // GET: ChainPharmacyManagement

        //总店视图
        public ActionResult Store_List()
        {
            return View();
        }
        //分店视图
        public ActionResult Store_List2()
        {
            return View();
        }
        //新增总店视图
        public ActionResult Store_Add()
        {
            return View();
        }
        //新增分店视图
        public ActionResult Branch_Add()
        {
            List<Head_office> hlist = db.Head_office.ToList();
            if (Session["SuperAdmin"] == null)
            {
                AdminInfo alist = (AdminInfo)Session["Admin"];
                Branch blist = db.Branch.Find(alist.Bid);
                Response.Write($"<script>console.log({blist.Hid})</script>");
                hlist = db.Head_office.Where(s => s.Hid == blist.Hid).ToList();
            }
            return View(hlist);
        }
        // 编辑总店信息 store_edit
        public ActionResult Store_Edit(string Hid)
        {
            Head_office hlist = db.Head_office.Find(int.Parse(Hid));
            return View(hlist);
        }
        //编辑分店信息
        public ActionResult Branch_Edit(string Bid)
        {
            Branch blist = db.Branch.Find(int.Parse(Bid));
            ViewBag.name = db.Head_office.Find(blist.Hid).Hname;
            return View(blist);
        }
        //总店列表
        public ActionResult Head_list(string page, string limit)
        {
            var sql = from h in db.Head_office
                      select new
                      {
                          h.Hid,
                          h.Hname,
                          h.Contact,
                          h.Phone
                      };
            //创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = db.Head_office.ToList().Count(),//总记录数
                ["data"] = sql.ToList().Skip((int.Parse(page) - 1) * int.Parse(limit)).Take(int.Parse(limit))//使用Skip+Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }
        //分店列表
        public ActionResult Branch_list(string page, string limit)
        {
            var sql = from b in db.Branch
                      join h in db.Head_office on b.Hid equals h.Hid
                      select new
                      {
                          b.Bid,
                          h.Hname,
                          b.Bnane,
                          b.DetailAddress

                      };
            if (Session["SuperAdmin"] == null)
            {
                AdminInfo alist = (AdminInfo)Session["Admin"];
                sql = from b in db.Branch
                      join h in db.Head_office on b.Hid equals h.Hid
                      where b.Bid == alist.Bid && h.Hid==b.Hid
                      select new
                      {
                          b.Bid,
                          h.Hname,
                          b.Bnane,
                          b.DetailAddress
                      };
            }
            //创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = db.Branch.ToList().Count(),//总记录数
                ["data"] = sql.ToList().Skip((int.Parse(page) - 1) * int.Parse(limit)).Take(int.Parse(limit))//使用Skip+Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //添加保存总店信息
        public ContentResult Store_add(string Hname, string Contact, string Phone)
        {
            try
            {
                Head_office hlist = new Head_office();
                hlist.Hname = Hname;
                hlist.Contact = Contact;
                hlist.Phone = Phone;
                db.Head_office.Add(hlist);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //添加保存分店信息
        public ContentResult Branch_Add_Save(string Bnane, int Hid, string DetailAddress)
        {
            try
            {
                Branch blist = new Branch();
                blist.Hid = Hid;
                blist.Bnane = Bnane;
                blist.Province = "湖南省";
                blist.City = "长沙市";
                blist.County = "长沙县";
                blist.DetailAddress = DetailAddress;
                db.Branch.Add(blist);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //编辑保存总店信息
        [HttpPost]
        public ContentResult Store_Save(string Hid, string Hname, string Contact, string Phone)
        {
            try
            {
                Head_office hlist = db.Head_office.Find(int.Parse(Hid));
                hlist.Hname = Hname;
                hlist.Contact = Contact;
                hlist.Phone = Phone;
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }


        }
        //编辑保存分店信息
        public ContentResult Branch_Edit_Save(string Bid, string Bnane, string DetailAddress)
        {
            try
            {
                Branch blist = db.Branch.Find(int.Parse(Bid));
                blist.Bnane = Bnane;
                blist.DetailAddress = DetailAddress;
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //删除门店列表
        [HttpPost]
        public ContentResult Store_Del(int Hid)
        {
            //防止删除功能崩溃导致页面崩溃
            try
            {
                Head_office hlist = db.Head_office.Find(Hid);
                db.Head_office.Remove(hlist);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //删除门店列表
        [HttpPost]
        public ContentResult Branch_Del(int Bid)
        {
            //防止删除功能崩溃导致页面崩溃
            try
            {
                Branch blist = db.Branch.Find(Bid);
                db.Branch.Remove(blist);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //根据条件查询总店信息
        public ActionResult Search(string Hid, string Hname)
        {
            //给变量赋初值，默认都查
            var sql = from h in db.Head_office
                      select new
                      {
                          h.Hid,
                          h.Hname,
                          h.Contact,
                          h.Phone
                      };
            int id;
            //第一种查询
            if (Hid != "" && Hname == "")
            {
                id = int.Parse(Hid);
                sql = from h in db.Head_office
                      where h.Hid == id
                      select new
                      {
                          h.Hid,
                          h.Hname,
                          h.Contact,
                          h.Phone
                      };
            }//第二种查询
            else if (Hid == "" && Hname != "")
            {
                sql = from h in db.Head_office
                      where h.Hname.Contains(Hname)
                      select new
                      {
                          h.Hid,
                          h.Hname,
                          h.Contact,
                          h.Phone
                      };
            }
            else if (Hid != "" && Hname != "")//两个条件都查询
            {
                id = int.Parse(Hid);
                sql = from h in db.Head_office
                      where h.Hid == id && h.Hname.Contains(Hname)
                      select new
                      {
                          h.Hid,
                          h.Hname,
                          h.Contact,
                          h.Phone
                      };
            }
            //创建layui分页返回的数据哈希表
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),//总记录数
                ["data"] = sql.ToList()//使用Skip+Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }
        //根据条件查询分店信息
        public ActionResult Branch_Search(string Bid, string Bnane)
        {
            //给变量赋初值，默认都查
            var sql = from b in db.Branch
                      join h in db.Head_office on b.Hid equals h.Hid
                      select new
                      {
                          b.Bid,
                          h.Hname,
                          b.Bnane,
                          b.DetailAddress
                      };
            int id;
            //第一种查询
            if (Bid != "" && Bnane == "")
            {
                id = int.Parse(Bid);
                sql = from b in db.Branch
                      join h in db.Head_office on b.Hid equals h.Hid
                      where b.Bid == id
                      select new
                      {
                          b.Bid,
                          h.Hname,
                          b.Bnane,
                          b.DetailAddress
                      };

            }//第二种查询
            else if (Bid == "" && Bnane != "")
            {
                sql = from b in db.Branch
                      join h in db.Head_office on b.Hid equals h.Hid
                      where b.Bnane.Contains(Bnane)
                      select new
                      {
                          b.Bid,
                          h.Hname,
                          b.Bnane,
                          b.DetailAddress
                      };
            }
            else if (Bid != "" && Bnane != "")//两个条件都查询
            {
                id = int.Parse(Bid);
                sql = from b in db.Branch
                      join h in db.Head_office on b.Hid equals h.Hid
                      where b.Bid == id && b.Bnane.Contains(Bnane)
                      select new
                      {
                          b.Bid,
                          h.Hname,
                          b.Bnane,
                          b.DetailAddress
                      };
            }
            //创建layui分页返回的数据哈希表
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),//总记录数
                ["data"] = sql.ToList()//使用Skip+Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }
    }
}