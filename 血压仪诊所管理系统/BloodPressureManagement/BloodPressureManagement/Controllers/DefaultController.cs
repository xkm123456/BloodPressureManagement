using BloodPressureManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodPressureManagement.Controllers
{
        /// <summary>
        /// 后台首页
        /// </summary>
        public class DefaultController : Controller
        {
            BloodPressureManagementEntities db = new BloodPressureManagementEntities();
            // GET: Default
            public ActionResult Index()
            {
                if (Session["SuperAdmin"] == null)
                {
                    if (Session["Admin"] != null)
                    {
                        AdminInfo alist = (AdminInfo)Session["Admin"];
                        ViewBag.alist = alist.Aname;
                    }
                    else
                    {
                        return Redirect("/Login/Index");
                    }

                }
                else
                {
                    SuperAdmin slist = (SuperAdmin)Session["SuperAdmin"];
                    ViewBag.slist = slist.Aacount;
                }
                return View();
            }

            // 后台首页
            public ActionResult Welcome()
            {
                if (Session["SuperAdmin"] == null)
                {
                    AdminInfo alist = (AdminInfo)Session["Admin"];
                    ViewBag.alist = alist.Aname;
                }
                else
                {
                    SuperAdmin slist = (SuperAdmin)Session["SuperAdmin"];
                    ViewBag.slist = slist.Aacount;
                }
                ViewBag.time = DateTime.Now;
                return View();
            }
            public ActionResult Exit()
            {
                Session["SuperAdmin"] = null;
                Session["Admin"] = null;
                return Redirect("/Login/Index");
            }
            //检测管理员身份
            public ContentResult Check_Admin()
            {
                if (Session["SuperAdmin"] == null)
                {
                    if (Session["Admin"] != null)
                    {
                        AdminInfo alist = (AdminInfo)Session["Admin"];
                        //判断是否冻结
                        if (alist.Account_State == false)
                        {
                            return Content("冻结");
                        }
                        else
                        {
                            //判断是总店还是分店管理员
                            if (alist.AOr == false)
                            {
                                return Content("分店管理员");
                            }
                            else
                            {
                                return Content("总店管理员");
                            }
                        }
                    }
                    else
                    {
                        return Content("未登录");
                    }
                }
                else
                {
                    return Content("超级管理员");
                }
            }
        }
    }