using AdminDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using WebMatrix.WebData;

namespace AdminDemo.Controllers
{
    public class UserController : Controller
    {

        string MailBody = "<!DOCTYPE html>" +
                               "<html> " +
                                   "<body style=\"background -color:#ff7f26;text-align:center;\"> " +
                                   "<h1 style=\"color:#051a80;\">Welcome to Mail</h1> " +
                                   "<h2 style=\"color:#051a80;\">Please find the attached files.</h2> " +
                                   "<label style=\"color:orange;font-size:100px;border:5px dotted;border-radius:50px\"></label> " +
                                   "</body> " +
                               "</html>";
        string subject = "Welcome to Mail.";
        string mailTitle = "Email from .Net Core App";
        //string fromEmail = "Your Email";
        //string fromEmailPassword = "Your Password";


















        //IHttpContextAccessor httpContextAccessor;
        private readonly AdminContext _uc;
        private readonly AdminContext _appSettings;





        public UserController(AdminContext uc, AdminContext appSettings)
        {
            _uc = uc;
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {

            try
            {
                List<UserViewModel> objlist =
               (from c in _uc.Tbl_User_Master
                join
                  s in _uc.Tbl_Roll_Master on c.RollId equals s.RollId
                join
                d in _uc.Tbl_Department_Master on c.DeptId equals d.DeptId
                select new { c, s, d }).ToList().Select(x => new UserViewModel
                {
                    UserId = x.c.UserId,
                    FullName = x.c.FullName,
                    DateOfBirth = x.c.DateOfBirth,
                    RollId = x.c.RollId,
                    RollName = x.s.RollName,
                    UserName = x.c.UserName,
                    Email = x.c.Email,
                    Password = x.c.Password,
                    DeptId = x.d.DeptId,
                    DepartmentName = x.d.DepartmentName,

                }).ToList();
                return View(objlist);


            }
            catch (Exception ex)
            {
                return View(null);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexLogin(UserMaster objlogin)
        {

            if (ModelState.IsValid)
            {
                var login = _uc.Tbl_User_Master.Where(u => u.isDelete == false && u.UserName.Equals(objlogin.UserName) && u.Password.Equals(objlogin.Password)).FirstOrDefault();
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





        //get
        public IActionResult Create()
        {

            var context = _uc.Tbl_Roll_Master.Select(x => new SelectListItem { Text = x.RollName, Value = x.RollId.ToString() });
            List<SelectList> list = new List<SelectList>().ToList();
            ViewBag.RollList = context;

            var cont = _uc.Tbl_Department_Master.Select(x => new SelectListItem { Text = x.DepartmentName, Value = x.DeptId.ToString() });
            List<SelectList> list1 = new List<SelectList>().ToList();
            ViewBag.DeptList = cont;
            return View();

        }

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserMaster obj)
        {
            if (ModelState.IsValid)
            {

                var sessionUser = HttpContext.Session.GetString("USer");
                obj.CreatedBy = sessionUser;
                obj.UpdatedBy = sessionUser;

                _uc.Tbl_User_Master.Add(obj);
                _uc.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }





        //GET-EDIT
        public IActionResult Edit(int? id)
        {
            var context = _uc.Tbl_Roll_Master.Select(x => new SelectListItem { Text = x.RollName, Value = x.RollId.ToString() });
            List<SelectList> list = new List<SelectList>().ToList();
            ViewBag.RollList = context;

            var cont = _uc.Tbl_Department_Master.Select(x => new SelectListItem { Text = x.DepartmentName, Value = x.DeptId.ToString() });
            List<SelectList> list1 = new List<SelectList>().ToList();
            ViewBag.DeptList = cont;


            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _uc.Tbl_User_Master.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        //Post-Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserMaster obj)
        {
            //if (ModelState.IsValid)
            //{

                var sessionUser = HttpContext.Session.GetString("USer");
                obj.CreatedBy = sessionUser;
                obj.UpdatedBy = sessionUser;
                _uc.Tbl_User_Master.Update(obj);
                _uc.SaveChanges();
                return RedirectToAction("Index");

           // }

            return View(obj);
        }


        //GET-Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _uc.Tbl_User_Master.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        //Post-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(UserMaster user)
        {
            var obj = _uc.Tbl_User_Master.Find(user.UserId);

            if (obj == null)
            {
                return NotFound();

            }
            _uc.Tbl_User_Master.Remove(obj);
            _uc.SaveChanges();
            return RedirectToAction("Index");




        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserViewModel user)
        {

            bool isvalid = _uc.Tbl_User_Master.Any(u => u.UserName == user.UserName && u.Password == user.Password);
            if (isvalid == true)
            {

                HttpContext.Session.SetString("USer", user.UserName);
                ViewBag.message = user.UserName;
                return RedirectToAction("Index");

            }
            if (isvalid == false)

            {
                ViewBag.message = "Password is wrong";
            }

            return View();

        }

        public IActionResult Logout()
        {
            //Session.addon
            return RedirectToAction("Login");

        }


        public ActionResult dashboard()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult dashboard(string Password, string newPassword, string Confirmpwd, ChangePasswordViewModel change)
        {
            Login objlogin = new Login();
            //string user = TempData["user"].ToString();
            //int id = int.Parse(TempData["id"].ToString());
            var login = _uc.Tbl_User_Master.Where(u => u.isDelete == false && u.Password.Equals(change.Password)).FirstOrDefault();
            if (login.Password == Password)
            {
                if (ModelState.IsValid)
                {
                    //login.ConfirmPassword = Confirmpwd;
                    login.Password = newPassword;
                    _uc.Entry(login).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _uc.SaveChanges();
                    TempData["msg"] = "<script>alert('Password has been changed successfully !!!');</script>";
                    return RedirectToAction("Index");
                }
                //else
                //{
                //    TempData["msg"] = "<script>alert('New password match !!! Please check');</script>";
                //}
            }
            else
            {
                TempData["msg"] = "<script>alert('Old password not match !!! Please check entered old password');</script>";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse("from_address@example.com"));
            //email.To.Add(MailboxAddress.Parse("to_address@example.com"));
            //email.Subject = "Test Email Subject";
            //email.Body = new TextPart(TextFormat.Plain) { Text = "Example Plain Text Message Body" };
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string Email)
        {
            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse(from));
            //email.To.Add(MailboxAddress.Parse(to));
            //email.Subject = subject;
            //email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
           // string Path = "C:\\project\\AdminDemo\\AdminDemo\\wwwroot\\Templates\\EmailTemplate\\Register_EmailTemplate.html";
            string FilePath = "C:\\project\\AdminDemo\\AdminDemo\\wwwroot\\Templates\\EmailTemplate\\Register_EmailTemplate.html";
            System.IO.StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            //MailText = MailText();





            string code = Guid.NewGuid().ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("noreply@archesoftronix.com", mailTitle);
            mail.To.Add(new MailAddress(Email));
            mail.Subject = subject;
          
            var lnkHref = "<a href='" + Url.Action("ResetPassword", "User", new { ResetPasswordCode = code, email = Email }, "http") + "'>Reset Password</a>";
            MailText = MailText.Replace("[link]", lnkHref);
           mail.Body = "<b>Please find the Password Reset Link. </b><br/>" + MailText;
           
            //mail.Body = MailBody;
            using (var context = new AdminContext())
            {
                var user = _uc.Tbl_User_Master.Where(a => a.Email == Email).FirstOrDefault();
                if (user != null)
                {
                    user.ResetPasswordCode = code;
                    _uc.SaveChanges();
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail1.archesoftronix.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new System.Net.NetworkCredential("noreply@archesoftronix.com", "N0123ply", "");
                    //smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
                    //smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
                    smtp.Send(mail);
                    // smtp.Disconnect(true);

                }
            }
            return View("Login");
        }
        [HttpGet]
        public ActionResult ResetPassword(string ResetPasswordCode, string Email)
        {
            var user =_uc.Tbl_User_Master.Where(x => x.Email == Email).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.code = ResetPasswordCode;
                model.Email = Email;
                return View(model);
            }
            else
            {
                return BadRequest();
            }  
            //return View();
        }

        //public ActionResult ResetPassword(ResetPasswordModel resetPassword)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool resetResponse = WebSecurity.ResetPassword(resetPassword.Code, resetPassword.NewPassword);
        //        if (resetResponse)
        //        {

        //            //var login = _uc.Tbl_User_Master.Where(u => u.isDelete == false && u.Password(resetPassword.NewPassword)).FirstOrDefault();


        //            //_uc.Tbl_User_Master.Update(login);
        //            //_uc.SaveChanges();

        //            ViewBag.Message = "Successfully Changed";
        //            //return RedirectToAction("Login");
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Something went horribly wrong!";
        //        }
        //    }
        //    return View("Login");
        //}




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel resetPassword)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var context = new AdminContext())
                {
                    var user = _uc.Tbl_User_Master.Where(a => a.ResetPasswordCode == resetPassword.code).FirstOrDefault();
                    if (user != null)
                    {
                        //you can encrypt password here, we are not doing it
                        user.Password = resetPassword.NewPassword;
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        //context.Configuration.ValidateOnSaveEnabled = false;
                        _uc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View("Login");
        }





    }
}







