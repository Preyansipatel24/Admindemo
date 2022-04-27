using AdminDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace AdminDemo.Controllers
{
    public class RollController : Controller
    {
        private readonly AdminContext _Rc;
        public RollController(AdminContext Rc)
        {
            _Rc = Rc;
        }
        public IActionResult IndexRoll()
        {
            IEnumerable<RollMaster> RollList = _Rc.Tbl_Roll_Master;
            return View(RollList);
        }
        //Get-Create
        public IActionResult CreateRoll()
        {
            return View();
        }

       //Post-Create
       [HttpPost]
       [ValidateAntiForgeryToken]
       public IActionResult CreateRoll(RollMaster obj)
        {
            if (ModelState.IsValid)
            {
                _Rc.Tbl_Roll_Master.Add(obj);
                _Rc.SaveChanges();
                return RedirectToAction("IndexRoll");
            }
            return View(obj);
        }

        //Get-Edit
        public IActionResult EditRoll(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _Rc.Tbl_Roll_Master.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoll(RollMaster roll)
        {
            if (ModelState.IsValid)
            {
                _Rc.Tbl_Roll_Master.Update(roll);
                _Rc.SaveChanges();
                return RedirectToAction("IndexRoll");
            }
            return View();
        }

        //GET-Delete
        public IActionResult DeleteRoll(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _Rc.Tbl_Roll_Master.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        //Post-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePostRoll(RollMaster roll)
        {
            var obj = _Rc.Tbl_Roll_Master.Find(roll.RollId);

            if (obj == null)
            {
                return NotFound();

            }
            _Rc.Tbl_Roll_Master.Remove(obj);
            _Rc.SaveChanges();
            return RedirectToAction("IndexRoll");




        }

    }
}
