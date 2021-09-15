using BloodPressureManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodPressureManagement.Controllers
{

    public class LoginController : Controller
    {
        BloodPressureManagementEntities db = new BloodPressureManagementEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        //用户登录
        [HttpPost]
        public ContentResult UserLogin(string Uacount, string Upwd)
        {

            int count = db.UserInfo.Where(s => s.Uacount == Uacount && s.Upwd == Upwd).Count();
            if (count > 0)
            {
                UserInfo ulist = db.UserInfo.Where(s => s.Uacount == Uacount && s.Upwd == Upwd).FirstOrDefault();
                Session["UserInfo"] = ulist;
                return Content("YES");
            }
            else
            {
                return Content("NO");
            }
        }
        //管理员登录
        [HttpPost]
        public ActionResult AdminLogin(string Aacount, string Apwd)
        {
            int count = 0;
            count = db.SuperAdmin.Where(s => s.Aacount == Aacount && s.Apwd == Apwd).Count();
            if (count == 0)
            {
                count = db.AdminInfo.Where(s => s.Aacount == Aacount && s.Apwd == Apwd).Count();
                if (count > 0)
                {
                    AdminInfo alist = db.AdminInfo.Where(s => s.Aacount == Aacount && s.Apwd == Apwd).FirstOrDefault();
                    Session["Admin"] = alist;
                    //登录成功后跳转到指定用户界面
                    return Content("YES");
                }
                else
                {
                    return Content("NO");
                }
            }
            else
            {
                SuperAdmin slist = db.SuperAdmin.Where(s => s.Aacount == Aacount && s.Apwd == Apwd).FirstOrDefault();
                Session["SuperAdmin"] = slist;
                return Content("YES");
            }


        }
        //用户账号验证
        [HttpPost]
        public ContentResult UserAcountCheck(string Uacount)
        {
            int count = db.UserInfo.Where(s => s.Uacount == Uacount).Count();
            if (count > 0)
            {
                return Content("YES");
            }
            else
            {
                return Content("NO");
            }
        }
        //管理员账号验证
        [HttpPost]
        public ContentResult AdminAcountCheck(string Aacount)
        {
            int count = 0;
            count = db.AdminInfo.Where(s => s.Aacount == Aacount).Count();
            if (count == 0)
                count = db.SuperAdmin.Where(s => s.Aacount == Aacount).Count();
            if (count > 0)
            {
                return Content("YES");
            }
            else
            {
                return Content("NO");
            }
        }
        //跳转登录成功后页面
        public ActionResult Suc()
        {
            return View();
        }
    }
}