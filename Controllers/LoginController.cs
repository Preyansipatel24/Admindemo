using AdminDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo;
using System.Linq;

namespace AdminDemo.Controllers
{
    public class LoginController : Controller
    {
        private readonly AdminContext _Lc;
        public LoginController(AdminContext Lc)
        {
            _Lc = Lc;
        }
        public ActionResult IndexLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IndexLogin(UserMaster objlogin)
        {

            if (ModelState.IsValid)
            {
                var login = _Lc.Tbl_User_Master.Where(u => u.isDelete == false && u.UserName.Equals(objlogin.UserName) && u.Password.Equals(objlogin.Password)).FirstOrDefault();
                if (login != null)
                {
                    TempData["user"] = login.UserName;
                    TempData["id"] = login.UserId;

                    TempData.Keep("id"); TempData.Keep("user");
                    return RedirectToAction("dashboard");
                }
                else
                {
                    TempData["msg"] = "<script>alert('Username and Password does not match');</script>";
                }
            }

            return View(objlogin);
        }
        //change password
        public ActionResult dashboard()
        {
            if (TempData["user"] == null)
            {
                return RedirectToAction("Index");
            }
            else { }
            return View();
        }
        [HttpPost]
        public ActionResult dashboard(string Password, string newPassword, string Confirmpwd)
        {
            Login objlogin = new Login();
            string user = TempData["user"].ToString();
            int id = int.Parse(TempData["id"].ToString());
            var login = _Lc.Tbl_User_Master.Where(u => u.isDelete == false && u.UserName.Equals(user) && u.UserId.Equals(id)).FirstOrDefault();
            if (login.Password == Password)
            {
                if (Password == newPassword)
                {
                    //login.ConfirmPassword = Confirmpwd;
                    login.Password = newPassword;
                    _Lc.Entry(login).State = EntityState.Modified;
                    _Lc.SaveChanges();
                    TempData["msg"] = "<script>alert('Password has been changed successfully !!!');</script>";
                }
                else
                {
                    TempData["msg"] = "<script>alert('New password match !!! Please check');</script>";
                }
            }
            else
            {
                TempData["msg"] = "<script>alert('Old password not match !!! Please check entered old password');</script>";
            }
            return View();
        }
           }
}
    


