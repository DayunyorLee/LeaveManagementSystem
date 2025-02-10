using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using AutoMapper;
using LeaveManagementSystem.Web.Services;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController (ILeaveTypesService _leaveTypesService) : Controller
    {

        private const string NameExistsValidationMessage = "This leave type already exists in th database";

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            var viewData = await _leaveTypesService.GetAll();
            //var data = await _context.LeaveTypes.ToListAsync();

            //var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);

            //var viewData = data.Select(q => new IndexVM
            //{
            //    Id = q.Id,
            //    Name = q.Name,
            //    NumberOfDays = q.NumberOfDays,
            //});


            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = leaveType;

            return View(viewData);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
        {

            if (await _leaveTypesService.CheckIfLeaveTypeExists(leaveTypeCreate.Name))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name),NameExistsValidationMessage);
            }

            if (leaveTypeCreate.Name.Contains("vacation"))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name),"Name should not contain vacation");
            }

            if (ModelState.IsValid)
            {
               await _leaveTypesService.Create(leaveTypeCreate);
              //  var leaveType = _mapper.Map<LeaveType>(leaveTypeCreate);
              //  _context.Add(leaveType);
             //   await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeCreate);
        }

      

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeEditVM>(id.Value);
            //var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            //var viewData = _mapper.Map<LeaveTypeEditVM>(leaveType);
            

            return View(leaveType);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
        {

            if (await _leaveTypesService.CheckIfLeaveTypeExistsForEdit(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidationMessage);
            }

            if (id != leaveTypeEdit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _leaveTypesService.Edit(leaveTypeEdit);
                    //var leaveType = _mapper.Map<LeaveType>(leaveTypeEdit);
                    //_context.Update(leaveType);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! _leaveTypesService.LeaveTypeExists(leaveTypeEdit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeEdit);
        }

 

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeDeleteVM>(id.Value);
            //var leaveType = await _context.LeaveTypes
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = leaveType;// _mapper.Map<LeaveTypeDeleteVM>(leaveType);

            return View(viewData);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           // var leaveType = _leaveTypesService.Get<LeaveTypeDeleteVM>(id);// await _context.LeaveTypes.FindAsync(id);
           // if (leaveType != null)
           // {
                await _leaveTypesService.Remove(id);
                //_context.LeaveTypes.Remove(leaveType);
           // }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
    }
}
