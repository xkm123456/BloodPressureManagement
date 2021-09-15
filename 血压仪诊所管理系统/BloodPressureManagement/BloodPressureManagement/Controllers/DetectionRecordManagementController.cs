using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BloodPressureManagement.Models;

namespace BloodPressureManagement.Controllers
{
    /// <summary>
    /// 检测记录管理
    /// </summary>
    public class DetectionRecordManagementController : Controller
    {
        BloodPressureManagementEntities db = new BloodPressureManagementEntities();
        // GET: DetectionRecordManagement
        // 检测者列表
        public ActionResult Examiner_List()
        {
            // 获取分店
            ViewBag.BranchList = db.Branch.ToList();
            // 将检测者列表发送给视图进行显示
            return View(db.UserInfo.ToList());
        }

        // 分页
        public ActionResult Examiner_Paging(int page,int limit)
        {
            // 【2】多表查询实现分页
            var sql = from c in db.UserInfo
                      join b in db.Branch on c.Bid equals b.Bid
                      select new
                      {
                          c.Uid,
                          c.Uname,
                          c.Usex,
                          c.Uage,
                          c.Phone,
                          b.Bnane,
                          c.Number,
                          c.LastTime
                      };
            // 创建hashtable来返回layui特定的响应数据
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),    // 总记录数
                ["data"] = sql.ToList().Skip((page - 1) * limit).Take(limit)    // 使用Skip + Take实现分页
            };
            return Json(hashtable,JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        // 根据姓名、电话、分店来查询对应用户
        public ActionResult FindCustomerByNamePhone(string Cname,string Cphone,string Branch)
        {
            // 创建存储过程参数列表
            SqlParameter[] parms = new SqlParameter[3];
            // 将参数填入参数列表
            parms[0] = new SqlParameter("@Cname",Cname);
            parms[1] = new SqlParameter("@Cphone", Cphone);
            parms[2] = new SqlParameter("@BranchId", Branch);
            //int result = db.Database.SqlQuery<int>("exec proc_namePhoBranch @Cname,@Cphone,@BranchId", parms).Count();

            var result = db.Database.SqlQuery<UserInfo>("exec proc_namePhoBranch @Cname,@Cphone,@BranchId", parms).ToList();

            // 获取分店
            ViewBag.BranchList = db.Branch.ToList();
            //return View(result);
            return View("Examiner_List", result);
        }

        [HttpPost]
        public ContentResult Account_Password(string id)
        {
            try
            {
                UserInfo alist = db.UserInfo.Find(int.Parse(id));
                alist.Upwd = "shuya123";
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return Content("NO");
            }
        }

        // 根据指定的姓名、电话、分店id查询检测者
        public ActionResult Examiner_Find(string cname,string cphone,int bid)
        {
            // ef多表查询
            var sql = from c in db.UserInfo
                      join b in db.Branch on c.Bid equals b.Bid where c.Uname == cname && c.Phone == cphone && b.Bid == bid
                      select new
                      {
                          c.Uid,
                          c.Uname,
                          c.Usex,
                          c.Uage,
                          c.Phone,
                          c.IdentificationCard,
                          b.Bnane,
                          c.Number,
                          c.LastTime
                      };
            // 创建layui分页返回的数据哈希表
            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),    // 总记录数
                ["data"] = sql.ToList()    // 使用Skip + Take实现分页
            };
            ViewBag.List = sql.ToList();
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }

        // 新增检测者
        public ActionResult Examiner_Add()
        {
            ViewBag.msg = " ";
            // 获取分店，并传递给新增检测者视图
            ViewBag.Branch = db.Branch.ToList();
            return View();
        }

        // 新增检测记录
        public ActionResult Add_JianceRecord()
        {
            return View();
        }

        // Ajax检测是否重复添加检测者
        public ActionResult Examiner_IsRepeater(string IdentificationCard,string Number)
        {
            // 判断是否有此检测者，有就按照更新次数更新检测者检测次数即可，就不用添加检测者
            if (db.UserInfo.Where(c => c.IdentificationCard == IdentificationCard).Count() > 0)
            {
                UserInfo user = db.UserInfo.Where(cb => cb.IdentificationCard == IdentificationCard).FirstOrDefault();
                user.Number += int.Parse(Number);   // 更新次数
                db.SaveChanges();
                return Content("此患者已经被添加过！");
            }
            else
            {
                return Content("ok");
            }
        }

        [HttpPost]
        public ActionResult Examiner_Add(UserInfo user)
        {
            try
            {
                db.UserInfo.Add(user);
                ViewBag.Branch = db.Branch.ToList();
                ViewBag.Uid = user.Uid;
                db.SaveChanges();
                // 将检测者身份证传递给其它控制器
                return RedirectToAction("Examiner_Projects",new { IdCard = user.IdentificationCard });
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('出错了，请与管理员联系" + e.Message + "');</script>");
                return View();
            }
        }

        // 新增检测者项目
        // 检测记录报表 Examiner_Projects.html
        public ActionResult Examiner_Projects(string IdCard)
        {
            UserInfo user = db.UserInfo.Where(c => c.IdentificationCard == IdCard).FirstOrDefault();
            ViewBag.user = user;
            ViewBag.JcTime = user.LastTime; // 最后检测时间
            return View();
        }

        // 生成报告单jiancebaogaodan.html
        public ActionResult Jiancebaogaodan(string cid,string name,string gender,string age,string height,string weight)
        {
            int? real_cid = (int?)int.Parse(cid);
            ViewBag.name = name;
            ViewBag.gender = gender;
            ViewBag.age = age;
            ViewBag.height = height;
            ViewBag.weight = weight;
            // 根据指定用户id查找数据
            List<Projects> list_proj = db.Projects.Where(p => p.Uid == real_cid).ToList();
            return View(list_proj);
        }

        [HttpPost]
        public ActionResult Jiancebaogaodan(string cid,string shousuoya,string shuzhangya,string method,string pro_Time)
        {
            try
            {
                // 实例化检测项目(血压)表对象
                Projects proj = new Projects();

                proj.Uid = int.Parse(cid);
                proj.Dname = "血压";
                proj.Util = "mmHg";
                proj.ShouSuoYa = shousuoya;
                proj.ShuZhangYa = shuzhangya;
                proj.Method = method;
                proj.Pro_Time =DateTime.Parse(pro_Time);

                // 结果解读
                // 正常收缩压90-140  正常舒张压60-90
                // 如果收缩压低于90mmHg，舒张压低于60mmHg，属于低血压
                // 如果收缩压高于140mmHg，舒张压高于90mmHg，属于高血压
                string result = "根据您的检测结果分析如下：";
                if (int.Parse(shousuoya) < 90 || int.Parse(shuzhangya) < 60)
                {
                    result += "血压过低";
                }
                else if (int.Parse(shousuoya) > 140 || int.Parse(shuzhangya) > 60)
                {
                    result += "血压过高";
                }
                else
                {
                    result += "血压正常";
                }
                proj.Decipher = result;

                db.Projects.Add(proj);
                db.SaveChanges();
                return Content("ok");
            }
            // ef语句出现错误
            catch (DbEntityValidationException e)
            {
                Response.Write("<script>alert('出错了："+e.Message+",请联系管理员');</script>");
                return Content("no");
             }
            catch (Exception e1)
            {
                Response.Write("<script>alert('出错了：" + e1.Message + ",请联系管理员');</script>");
                return Content("no");
            }
        }

        // 检测记录Jiancejilu
        public ActionResult Jiancejilu()
        {
            // 获取分店
            ViewBag.Branch = db.Branch.ToList();
            // 查询所有检测记录
            return View(db.Projects.ToList());
        }

        // 检测记录查询
        // 根据电话，开始结束时间，分店id
        public ActionResult Jiancejilu_Find(string phone, DateTime start_time, DateTime end_time, int branchid)
        {
            List<UserInfo> list_user = db.UserInfo.ToList();
            var sql = from u in db.UserInfo
                      join b in db.Branch on u.Bid equals b.Bid
                      join p in db.Projects on u.Uid equals p.Uid
                      where start_time <= u.LastTime && u.LastTime <= end_time
                      && u.Phone == phone && u.Bid == branchid
                      select new
                      {
                          u.Uname,
                          u.Usex,
                          u.Uage,
                          u.Phone,
                          u.LastTime,
                          b.Bnane
                      };

            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),
                ["data"] = sql.ToList()
            };
            return Json(hashtable, JsonRequestBehavior.AllowGet);
        }

        // 分页
        public ActionResult Jiancejilu_Paging(int page, int limit)
        {
            List<UserInfo> list_user = db.UserInfo.ToList();
            var sql = from u in db.UserInfo
                      join b in db.Branch on u.Bid equals b.Bid
                      join p in db.Projects on u.Uid equals p.Uid
                      select new
                      {
                          u.Uname,
                          u.Usex,
                          u.Uage,
                          u.Phone,
                          u.LastTime,
                          b.Bnane
                      };

            Hashtable hashtable = new Hashtable
            {
                ["code"] = 0,
                ["msg"] = "",
                ["count"] = sql.ToList().Count(),    
                ["data"] = sql.ToList().Skip((page - 1) * limit).Take(limit)
            };
            return Json(hashtable,JsonRequestBehavior.AllowGet);
        }

        // 编辑 admin-edit.html
        public ActionResult Admin_Edit(string id)
        {
            int uid = int.Parse(id);
            ViewBag.Branch = db.Branch.ToList();
            return View(db.UserInfo.Find(uid));
        }

        // 更新用户信息
        [HttpPost]
        public ActionResult RefreshUserInfo(UserInfo cust)
        {
            try
            {
                // 获取用户的身份证
                string identificationcard = cust.IdentificationCard;
                UserInfo new_cust = db.UserInfo.Where(c => c.IdentificationCard == identificationcard).FirstOrDefault();
                int bid = (int)cust.Bid;
                new_cust.Uacount = "22";
                new_cust.Upwd = "123";
                new_cust.Uname = cust.Uname;
                new_cust.Uage = cust.Uage;
                new_cust.Usex = cust.Usex;
                new_cust.Phone = cust.Phone;
                new_cust.Bid = bid;
                new_cust.Number = cust.Number;
                new_cust.LastTime = cust.LastTime;
                db.SaveChanges();
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no" + e.Message);
            }
        }

        // 查看明细 Admin_Detail
        public ActionResult Admin_Detail(string id)
        {
            int uid = int.Parse(id);
            ViewBag.Branch = db.Branch.ToList();
            return View(db.UserInfo.Find(uid));
        }

        // 检测者列表
        public ActionResult Cust_MainPage()
        {
            return View();
        }
        public ActionResult Cust_Paging()
        {
            if (Session["UserInfo"] != null)
            {
                UserInfo user = Session["UserInfo"] as UserInfo;
                int uid = user.Uid;
                var sql = from u in db.UserInfo
                          join b in db.Branch on u.Bid equals b.Bid
                          join p in db.Projects on u.Uid equals p.Uid
                          where u.Uid == uid
                          select new
                          {
                              p.Dname,
                              p.Util,
                              p.ShouSuoYa,
                              p.ShuZhangYa,
                              b.Bnane,
                              p.Method,
                              p.Decipher,
                              u.LastTime

                          };
                // 创建layui分页返回的数据哈希表
                Hashtable hashtable = new Hashtable
                {
                    ["code"] = 0,
                    ["msg"] = "",
                    ["count"] = sql.ToList().Count(),    // 总记录数
                    ["data"] = sql.ToList()    // 使用Skip + Take实现分页
                };
                ViewBag.List = sql.ToList();
                return Json(hashtable, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var sql = from u in db.UserInfo
                          join b in db.Branch on u.Bid equals b.Bid
                          join p in db.Projects on u.Uid equals p.Uid
                          select new
                          {
                              p.Dname,
                              p.Util,
                              p.ShouSuoYa,
                              p.ShuZhangYa,
                              b.Bnane,
                              p.Method,
                              p.Decipher,
                              u.LastTime

                          };
                // 创建layui分页返回的数据哈希表
                Hashtable hashtable = new Hashtable
                {
                    ["code"] = 0,
                    ["msg"] = "",
                    ["count"] = sql.ToList().Count(),    // 总记录数
                    ["data"] = sql.ToList()    // 使用Skip + Take实现分页
                };
                ViewBag.List = sql.ToList();
                return Json(hashtable, JsonRequestBehavior.AllowGet);
            }
            
        }


        // 删除检测者
        public ActionResult Admin_Del(string id)
        {
            try
            {
                int uid = int.Parse(id);
                UserInfo cust = db.UserInfo.Where(c => c.Uid == uid).FirstOrDefault();
                db.UserInfo.Remove(cust);
                db.SaveChanges();
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no" + e.Message);
            }
        }

        // 批量删除检测者
        public ActionResult Batch_Examiner_Del(string custId_arr)
        {
            try
            {
                // 拆分id字符串为id数组
                string[] id_arr = custId_arr.Split(',');

                // 遍历每个检测者的id并删除对应检测者
                for (int i = 0; i < id_arr.Length; i++)
                {
                    int Uid = int.Parse(id_arr[i]);
                    UserInfo user = db.UserInfo.Find(Uid);
                    db.UserInfo.Remove(user);
                    db.SaveChanges();
                }
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }
    }
}