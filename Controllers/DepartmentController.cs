using AdminDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AdminDemo.Controllers
{
    public class DepartmentController : Controller
        
    {
        private readonly AdminContext _dc;

        public DepartmentController(AdminContext dc)
        {
            _dc = dc;
        }
        public IActionResult IndexDepartment()
        {
            IEnumerable<DepartmentMaster> DeptList = _dc.Tbl_Department_Master;
            return View(DeptList);
        }
        //Get
        public IActionResult CreateDepartment()
        {
            return View();
        }
        //post

        [HttpPost]
       [ValidateAntiForgeryToken]
       public IActionResult CreateDepartment(DepartmentMaster dept)
        {
            if (ModelState.IsValid)
            {
                _dc.Tbl_Department_Master.Add(dept);
                _dc.SaveChanges();
                return RedirectToAction("IndexDepartment");
            }
            return View(dept);

        }
    

        //get-edit

        public IActionResult EditDepartment(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }
            var department = _dc.Tbl_Department_Master.Find(id);    
            if(department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        //post-edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDepartment(DepartmentMaster department)
        {
            if(ModelState.IsValid)
            {
                _dc.Tbl_Department_Master.Update(department);
                _dc.SaveChanges();
                return RedirectToAction("IndexDepartment");
            }
            return View(department);

        }


        //get-delete
        public IActionResult DeleteDepartment(int? id)
        {
            if(id == 0 || id == null)
            {
                NotFound();
            }
            var department = _dc.Tbl_Department_Master.Find(id);
            if (department == null)
            {
                NotFound();
            }
            return View(department);
        }

        //post-delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePostDepartment(DepartmentMaster department)
        {
            var obj = _dc.Tbl_Department_Master.Find(department.DeptId);
            if(obj == null)
            {
                return NotFound();
            }
            _dc.Tbl_Department_Master.Remove(obj);
            _dc.SaveChanges();
            return RedirectToAction("IndexDepartment");

        }
    }
}
