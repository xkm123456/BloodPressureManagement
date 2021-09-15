using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodPressureManagement.Models;

namespace BloodPressureManagement.Controllers
{
    public class UserPageController : Controller
    {
        // GET: UserPage
        public ActionResult Index()
        {
            //判断是否登录
            if(Session["UserInfo"]==null)
            {
                Response.Write("<scriptlayer.open({icon: 7,title: '提示',content: '还未登录，即将跳转到登录页面'});</script>");
                return Redirect("/Login/Index");
            }
            else
            {
                UserInfo ulist = (UserInfo)Session["UserInfo"];
                ViewBag.time = DateTime.Now;
                return View(ulist);
            }
        }
        //退出
        public ActionResult Exit()
        {
            Session["UserInfo"] = null;
            return Redirect("/Login/Index");
        }
        public ActionResult User_Welcome()
        {
            UserInfo ulist = (UserInfo)Session["UserInfo"];
            ViewBag.time = DateTime.Now;
            return View(ulist);
        }
        
    }
}