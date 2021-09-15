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
    /// 系统账号管理
    /// </summary>

    public class SystemAccountManagementController : Controller
    {
        BloodPressureManagementEntities db = new BloodPressureManagementEntities();
        // GET: SystemAccountManagement

        // 系统账号列表
        public ActionResult Account_List()
        {
            return View();
        }
        //总店管理员新增账号
        public ActionResult Account_Add_Head()
        {
            List<Branch> hlist = db.Branch.ToList();
            return View(hlist);
        }
        //总店管理员添加保存账号信息
        public ContentResult Account_Add_Head_Save(string Aname, string Phone, int Bid)
        {
            try
            {
                AdminInfo alist = new AdminInfo();
                alist.Aacount = Phone;
                alist.Apwd = "shuya123";
                alist.Aname = Aname;
                alist.AOr = true;
                alist.Phone = Phone;
                alist.Bid = Bid;
                alist.Account_State = true;
                db.AdminInfo.Add(alist);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        // 分店管理员新增账号
        public ActionResult Account_Add()
        {
            List<Branch> blist = db.Branch.ToList();
            if (Session["SuperAdmin"] == null)
            {
                AdminInfo alist = (AdminInfo)Session["Admin"];
                blist = db.Branch.Where(s => s.Bid == alist.Bid).ToList();
            }
            return View(blist);
        }
        //分店管理员添加保存账号信息
        public ContentResult Account_Add_Save(string Aname, string Phone, int Bid)
        {
            try
            {
                AdminInfo alist = new AdminInfo();
                alist.Aacount = Phone;
                alist.Apwd = "shuya123";
                alist.Aname = Aname;
                alist.AOr = false;
                alist.Phone = Phone;
                alist.Bid = Bid;
                alist.Account_State = true;
                db.AdminInfo.Add(alist);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }

        // 编辑 account-edit.html
        public ActionResult Account_Edit(string id)
        {
            AdminInfo alist = db.AdminInfo.Find(int.Parse(id));
            Branch blist = db.Branch.Find(alist.Bid);
            ViewBag.address = blist.DetailAddress;
            return View(alist);
        }
        //编辑保存账号信息
        public ContentResult Account_Edit_Save(string Aid, string Aname, string Phone, string Address)
        {
            try
            {
                AdminInfo alist = db.AdminInfo.Find(int.Parse(Aid));
                alist.Aname = Aname;
                alist.Phone = Phone;
                Branch blist = db.Branch.Find(alist.Aid);
                blist.DetailAddress = Address;
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        // 重置密码 account-password.ntml
        [HttpPost]
        public ContentResult Account_Password(string id)
        {
            try
            {
                AdminInfo alist = db.AdminInfo.Find(int.Parse(id));
                alist.Apwd = "shuya123";
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //显示数据列表
        public ActionResult Show_list(string page, string limit)
        {
            var sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            if (Session["SuperAdmin"] == null)
            {
                AdminInfo alist = (AdminInfo)Session["Admin"];
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Bid == alist.Bid && a.AOr != true
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = db.AdminInfo.ToList().Count(),//总记录数
                ["data"] = sql.ToList().Skip((int.Parse(page) - 1) * int.Parse(limit)).Take(int.Parse(limit))//使用Skip+Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }
        //根据条件进行查询显示数据列表
        public ActionResult Show_Searchlist(string Aname, string Phone, string Account_State, string DetailAddress)
        {
            var sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            bool Aor;
            //模糊查询，联系电话不为空，其余为空
            if (Phone != "" && Aname == "" && Account_State == "" && DetailAddress == "")
            {
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Phone.Contains(Phone)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，联系电话、姓名不为空
            else if (Phone != "" && Aname != "" && Account_State == "" && DetailAddress == "")
            {
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Phone.Contains(Phone) && a.Aname.Contains(Aname)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，联系电话、状态不为空
            else if (Phone != "" && Aname == "" && Account_State != "" && DetailAddress == "")
            {
                if (Account_State == "1")
                {
                    Aor = true;
                }
                else
                {
                    Aor = false;
                }
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Account_State == Aor && a.Phone.Contains(Phone)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，联系电话、地址不为空
            else if (Phone != "" && Aname == "" && Account_State == "" && DetailAddress != "")
            {
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where b.DetailAddress.Contains(DetailAddress) && a.Phone == Phone
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，联系电话、姓名、地址不为空
            else if (Phone != "" && Aname != "" && Account_State == "" && DetailAddress != "")
            {
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Phone.Contains(Phone) && a.Aname.Contains(Aname) && b.DetailAddress.Contains(DetailAddress)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，联系电话、姓名、冻结状态不为空
            else if (Phone != "" && Aname != "" && Account_State != "" && DetailAddress == "")
            {
                if (Account_State == "1")
                {
                    Aor = true;
                }
                else
                {
                    Aor = false;
                }
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Phone.Contains(Phone) && a.Aname.Contains(Aname) && a.Account_State == Aor
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，姓名不为空，其余为空
            else if (Phone == "" && Aname != "" && Account_State == "" && DetailAddress == "")
            {
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Aname.Contains(Aname)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，姓名、冻结状态不为空
            else if (Phone == "" && Aname != "" && Account_State != "" && DetailAddress == "")
            {
                if (Account_State == "1")
                {
                    Aor = true;
                }
                else
                {
                    Aor = false;
                }
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Account_State == Aor && a.Aname.Contains(Aname)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，姓名、地址不为空
            else if (Phone == "" && Aname != "" && Account_State == "" && DetailAddress != "")
            {
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where b.DetailAddress.Contains(DetailAddress) && a.Aname.Contains(Aname)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，姓名、地址、状态不为空
            else if (Phone == "" && Aname != "" && Account_State != "" && DetailAddress != "")
            {
                if (Account_State == "1")
                {
                    Aor = true;
                }
                else
                {
                    Aor = false;
                }
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Account_State == Aor && a.Aname.Contains(Aname) && b.DetailAddress.Contains(DetailAddress)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //准确查询，状态不为空，其余为空
            else if (Phone == "" && Aname == "" && Account_State != "" && DetailAddress == "")
            {
                if (Account_State == "1")
                {
                    Aor = true;
                }
                else
                {
                    Aor = false;
                }
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Account_State == Aor
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，状态、地址不为空
            else if (Phone == "" && Aname == "" && Account_State != "" && DetailAddress != "")
            {
                if (Account_State == "1")
                {
                    Aor = true;
                }
                else
                {
                    Aor = false;
                }
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Account_State == Aor && b.DetailAddress.Contains(DetailAddress)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，地址不为空，其余为空
            else if (Phone == "" && Aname == "" && Account_State == "" && DetailAddress != "")
            {
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where b.DetailAddress.Contains(DetailAddress)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //模糊查询，都不为空
            else if (Phone != "" && Aname != "" && Account_State != "" && DetailAddress != "")
            {
                if (Account_State == "1")
                {
                    Aor = true;
                }
                else
                {
                    Aor = false;
                }
                sql = from a in db.AdminInfo
                      join b in db.Branch on a.Bid equals b.Bid
                      where a.Aname.Contains(Aname) && a.Phone.Contains(Phone) &&
                      a.Account_State == Aor && b.DetailAddress.Contains(DetailAddress)
                      select new
                      {
                          a.Aid,
                          a.Aacount,
                          a.Aname,
                          a.AOr,
                          a.Phone,
                          a.Account_State,
                          b.Bnane,
                          b.County,
                          b.DetailAddress
                      };
            }
            //创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = db.AdminInfo.ToList().Count(),//总记录数
                ["data"] = sql.ToList()//使用Skip+Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }
        //数据列表右侧操作栏删除功能
        [HttpPost]
        public ContentResult Account_Del(string Aid)
        {
            try
            {
                AdminInfo alist = db.AdminInfo.Find(int.Parse(Aid));
                db.AdminInfo.Remove(alist);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //冻结该管理员
        [HttpPost]
        public ContentResult Account_Frozen(string Aid)
        {
            try
            {
                AdminInfo alist = db.AdminInfo.Find(int.Parse(Aid));
                alist.Account_State = false;
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //启用该管理员
        public ContentResult Account_Enable(string Aid)
        {
            try
            {
                AdminInfo alist = db.AdminInfo.Find(int.Parse(Aid));
                alist.Account_State = true;
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }
        //判断管理员身份
        public ContentResult Check()
        {
            if (Session["SuperAdmin"] == null)
            {
                return Content("总店管理员");
            }
            else
            {
                return Content("超级管理员");
            }
        }
        ////根据条件查询用判断管理员身份
        //public ContentResult IsAdmin()
        //{
        //    if(Session["SuperAdmin"]==null)
        //    {
        //        AdminInfo alist = (AdminInfo)Session["Admin"];
        //        if()
        //        return Content("")
        //    }
        //}
    }
}