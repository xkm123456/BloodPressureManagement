using BloodPressureManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodPressureManagement.Controllers
{
    public class AdminCenterController : Controller
    {
        BloodPressureManagementEntities db = new BloodPressureManagementEntities();
        // GET: AdminCenter
        public ActionResult Personal()
        {
            AdminInfo alist = new AdminInfo();
            if (Session["SuperAdmin"] == null)
            {
                alist = (AdminInfo)Session["Admin"];
                ViewBag.Branch = db.Branch.Where(s=>s.Bid==alist.Bid).ToList();
            }
            
            return View(alist);
        }
        public ActionResult Update(AdminInfo admin)
        {
            AdminInfo a = db.AdminInfo.Find(admin.Aid);
            a.Aname = admin.Aname;
            a.Phone = admin.Phone;
            a.Bid = admin.Bid;
            ViewBag.Branch = db.Branch.ToList();
            int fla = db.SaveChanges();
            if (fla > 0)
            {
                return Json(new
                {
                    Success = true,
                    Message = "修改成功"
                });
            }
            else
            {
                return Json(new
                {
                    Success = false,
                    Message = "修改失败"
                });
            }
        }
        public ActionResult Change_Password()
        {
            return View();
        }
        public ActionResult Xiugaimima(string Apwd, string Npassword, string Npassword1)
        {
            int id = 2;
            AdminInfo a = db.AdminInfo.Find(id);
            if (a.Apwd == Apwd)
            {
                if (Npassword == Npassword1)
                {
                    a.Apwd = Npassword;
                    db.SaveChanges();
                    return Json(new
                    {
                        Success = 1,
                        Message = "修改成功"
                    });

                }
                else
                {
                    return Json(new
                    {
                        Success = 2,
                        Message = "两次密码不一致"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Success = 3,
                    Message = "初始密码不对"
                });
            }
        }
    }
}