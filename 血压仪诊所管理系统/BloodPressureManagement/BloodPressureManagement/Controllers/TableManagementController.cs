using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BloodPressureManagement.Models;
using Newtonsoft.Json;

namespace BloodPressureManagement.Controllers
{
    /// <summary>
    /// 报表管理
    /// </summary>
    public class TableManagementController : Controller
    {
        BloodPressureManagementEntities db = new BloodPressureManagementEntities();
        // GET: TableManagement
        // 检测指标统览
        public ActionResult Examiner_Test()
        {
            // 传递总店表数据
            ViewBag.Head_office = db.Head_office.ToList();
            Head_office h = db.Head_office.ToList().First();
            int hid = h.Hid;
            // 传递分店表数据
            ViewBag.Branch = db.Branch.Where(b => b.Hid == hid).ToList();
            List<UserInfo> list = db.UserInfo.ToList();

            return View(list);
        }

        // 总店、分店级联
        public ActionResult BranchByHid(int hid)
        {
            string bliststr = string.Empty;     // 保存每条记录序列化成Json的字符串
            string jsonstr = string.Empty;      // 保存返回的json
            dynamic d = new System.Dynamic.ExpandoObject();     // 创建一个动态类型保存要返回的每条数据对象
            List <Branch> blist = db.Branch.Where(b => b.Hid == hid).ToList();

            foreach (var item in blist)
            {
                d.bid = item.Bid;
                d.bname = item.Bnane;
                bliststr += "," + JsonConvert.SerializeObject(d);   // 将每行的数据对象序列化成Json字符串并拼接
            }
            jsonstr += "[" + bliststr.Substring(1) + "]";

            return Json(jsonstr,JsonRequestBehavior.AllowGet);
        }

        // 检测指标统览分页
        public ActionResult Jiancezhibiao_Paging(int page, int limit)
        {
            var sql = from c in db.UserInfo
                      join b in db.Branch on c.Bid equals b.Bid
                      join h in db.Head_office on b.Hid equals h.Hid
                      select new
                      {
                          c.Uid,
                          c.Uname,
                          c.Usex,
                          c.Uage,
                          h.Hname,
                          b.Bnane,
                          c.Phone
                      };
            // 创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = db.UserInfo.Count(),    // 总记录数
                ["data"] = sql.ToList().Skip((page - 1) * limit).Take(limit)    // 使用Skip + Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }


        // 检测指标统览筛选分页
        // sxdate_start表示当前时间
        public ActionResult ExaminerSX_Paging(int page, int limit)
        {
            var sql = from c in db.UserInfo
                      join p in db.Projects on c.Uid equals p.Uid
                      select new
                      {
                          c.Uname,
                          c.Usex,
                          c.Uage,
                          p.Decipher,
                          p.Pro_Time,
                          c.Phone,
                      };
            // 创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),    // 总记录数
                ["data"] = sql.ToList().Skip((page - 1) * limit).Take(limit)    // 使用Skip + Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExaminerSX_Find_Paging(int page, int limit, DateTime sxdate_end, DateTime sxdate_start)
        {
            var sql = from c in db.UserInfo
                      join p in db.Projects on c.Uid equals p.Uid
                      where sxdate_end <= p.Pro_Time && p.Pro_Time <= sxdate_start
                      select new
                      {
                          c.Uname,
                          c.Usex,
                          c.Uage,
                          p.Decipher,
                          p.Pro_Time,
                          c.Phone,
                      };
            // 创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),    // 总记录数
                ["data"] = sql.ToList().Skip((page - 1) * limit).Take(limit)    // 使用Skip + Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }

        // 检测指标查询
        public ActionResult Jiancezhibiao_Find(string name,string phone,int headid,int branchid)
        {
            var sql = from c in db.UserInfo
                      join b in db.Branch on c.Bid equals b.Bid
                      join h in db.Head_office on b.Hid equals h.Hid 
                      where c.Uname == name && c.Phone == phone && b.Bid == branchid
                      select new
                      {
                          c.Uid,
                          c.Uname,
                          c.Usex,
                          c.Uage,
                          h.Hname,
                          b.Bnane,
                          c.Phone
                      };
            // 创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),    // 总记录数
                ["data"] = sql.ToList()    // 使用Skip + Take实现分页
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }

        // 指标异常表报
        public ActionResult Zhibiao_Yichang()
        {
            string nan = "男";
            string nv = "女";
            var count = db.UserInfo.Count();
            var list = db.UserInfo.Where(u => u.Usex == nan).ToList().Count;
            int list1 = db.UserInfo.Where(u => u.Usex == nv).ToList().Count;
            ViewBag.count = count;
            ViewBag.male = list;
            ViewBag.female = list1;
            return View();
        }

        // 消费者数据图谱
        public ActionResult Xiaofeizhe_Tupu()
        {
            return View();
        }

        // 月度数据报表 yuedu_shuju.html
        public ActionResult Yuedu_Shuju()
        {
            return View();
        }

        //	合格率分析 hegelv_fenxi.html
        public ActionResult Hegelv_Fenxi()
        {
            return View();
        }

        //检测记录报表 jiancejilu_baobiao.html
        public ActionResult Jiancejilu_Baobiao()
        {
            return View();
        }

    }
}