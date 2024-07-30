using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DbContexts;
using Pharmacy.Models;

namespace Pharmacy.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _IWebHostEnvironment;

        public MedicinesController(ApplicationDbContext context, IWebHostEnvironment iWebHostEnvironment)
        {
            _context = context;
            _IWebHostEnvironment = iWebHostEnvironment;
        }

        // GET: Medicines
        public async Task<IActionResult> Index(string? search)
        {
            ViewBag.SearchText = search;

            IQueryable<Medicine> queryableMeds = _context.Medicines.AsQueryable();

            if (string.IsNullOrEmpty(search) == false)
            {
                queryableMeds = queryableMeds.Where(med => med.MedicineName.Contains(search));
            }
            var applicationDbContext = _context.Medicines.Include(m => m.Categories);
            //await applicationDbContext.ToListAsync()
            return View("Index", queryableMeds.ToList());
        }

        // GET: Medicines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                .Include(m => m.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // GET: Medicines/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewBag.AllCategories = _context.Categories.ToList();

            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("Id,MedicineName,Price,QuantityInStock,ExpirationDate,Description,Manufacturer,PrescriptionRequirements,Barcode,StorageConditions,CategoryId,ImagePath")]
        public async Task<IActionResult> Create( Medicine medicine)
        {
            if(medicine.ExpirationDate.Date<DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Expired!!!!!!!!!");
            }    
            if (ModelState.IsValid)
            {


                if (medicine.ImageFile == null)
                {
                    medicine.ImagePath = "\\images\\No_Image.png";
                }
                else
                {
                    //GUID global unique identifier 
                    Guid imageGuid = Guid.NewGuid();
                    string imageExtension = Path.GetExtension(medicine.ImageFile.FileName);

                    medicine.ImagePath = "\\images\\" + imageGuid + imageExtension;
                    string imageUploadPath = _IWebHostEnvironment.WebRootPath + medicine.ImagePath;

                    FileStream imageStream = new FileStream(imageUploadPath, FileMode.Create);
                    medicine.ImageFile.CopyTo(imageStream);
                    imageStream.Dispose();

                }


                _context.Add(medicine);
                await _context.SaveChangesAsync();
                //_context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", medicine.CategoryId);
                ViewBag.AllCategories = _context.Categories.ToList();
                return View(medicine);
            }
            
        }

        // GET: Medicines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", medicine.CategoryId);
            ViewBag.AllCategories = _context.Categories.ToList();

            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Bind("Id,MedicineName,Price,QuantityInStock,ExpirationDate,Description,Manufacturer,PrescriptionRequirements,Barcode,StorageConditions,CategoryId,ImagePath")]
        public async Task<IActionResult> Edit(int id, Medicine medicine)
        {
            if (id != medicine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (medicine.ImageFile == null)
                {
                    medicine.ImagePath = "\\images\\No_Image.png";
                }
                else
                {
                    //GUID global unique identifier 
                    Guid imageGuid = Guid.NewGuid();
                    string imageExtension = Path.GetExtension(medicine.ImageFile.FileName);

                    medicine.ImagePath = "\\images\\" + imageGuid + imageExtension;
                    string imageUploadPath = _IWebHostEnvironment.WebRootPath + medicine.ImagePath;

                    FileStream imageStream = new FileStream(imageUploadPath, FileMode.Create);
                    medicine.ImageFile.CopyTo(imageStream);
                    imageStream.Dispose();

                }
                try
                {
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(medicine.Id))
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
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", medicine.CategoryId);
            ViewBag.AllCategories = _context.Categories.ToList();

            return View(medicine);
        }

        // GET: Medicines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                .Include(m => m.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine != null)
            {
                _context.Medicines.Remove(medicine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
