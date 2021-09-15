using BloodPressureManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodPressureManagement.Controllers
{
    /// <summary>
    /// 个人中心
    /// </summary>
    public class PersonalCenterController : Controller
    {
        // GET: PersonalCenter


        BloodPressureManagementEntities db = new BloodPressureManagementEntities();
        // GET: PersonalCenter

        // 	个人信息
        public ActionResult Personal()
        {
            ViewBag.Branch = db.Branch.ToList();
            UserInfo u = Session["UserInfo"] as UserInfo;
            return View(u);
        }
        [HttpPost]
        public ActionResult Update(UserInfo customer)
        {
            UserInfo c = db.UserInfo.Find(customer.Uid);
            c.Uname = customer.Uname;
            c.Usex = customer.Usex;
            c.Uage = customer.Uage;
            c.Phone = customer.Phone;
            c.IdentificationCard = customer.IdentificationCard;
            c.Bid = customer.Bid;
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
        // 修改密码 
        public ActionResult Change_Password()
        {
            return View();
        }
        public ActionResult Xiugaimima(string Upwd, string Npassword, string Npassword1)
        {
            UserInfo user = Session["UserInfo"] as UserInfo;
            int id = user.Uid;
            UserInfo u = db.UserInfo.Find(id);
            if (u.Upwd == Upwd)
            {
                if (Npassword == Npassword1)
                {
                    u.Upwd = Npassword;
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