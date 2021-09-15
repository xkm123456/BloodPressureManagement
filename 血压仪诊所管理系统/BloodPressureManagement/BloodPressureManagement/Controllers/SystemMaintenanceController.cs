using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodPressureManagement.Controllers
{
    /// <summary>
    /// 系统维护
    /// </summary>
    public class SystemMaintenanceController : Controller
    {
        //系统设置 system-setting.html
        public ActionResult System_Setting()
        {
            return View();
        }

        // 分配角色权限
        public ActionResult Role_Cate()
        {
            return View();
        }

        // 新增角色信息
        public ActionResult Role_Add()
        {
            return View();
        }

        //	编辑 
        //public ActionResult Order_View()
        //{
        //    return View();
        //}

        // 角色授权 role_shouquan.html
        public ActionResult Role_Shouquan()
        {
            return View();
        }

        // 编辑角色信息 role_find.html
        public ActionResult Role_Find()
        {
            return View();
        }

        // 编辑项目信息
        public ActionResult Edit_Project()
        {
            return View();
        }
    }
}