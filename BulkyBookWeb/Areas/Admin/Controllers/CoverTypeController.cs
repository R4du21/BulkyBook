using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypes = _unitOfWork.CoverType.GetAll();
        return View(objCoverTypes);
    }

    //GET - Create
    public IActionResult Create()
    {
        return View();
    }

    //POST - Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType obj)
    {
        if(obj.Name == string.Empty)
        {
            ModelState.AddModelError("emptyName", "The name cannot be empty.");
        }
        if(ModelState.IsValid)
        {
            _unitOfWork.CoverType.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cover type created successfully!";
            return RedirectToAction("Index");
        }
        return View();
    }

    //GET - Edit
    public IActionResult Edit(int? id)
    {
        if (id == null || id ==0 )
        {
            return NotFound();
        }

        var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u=>u.Id == id);

        if(CoverTypeFromDbFirst == null)
        {
            return NotFound();
        }

        return View(CoverTypeFromDbFirst);
    }

    //POST - Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit (CoverType obj)
    {
        if(obj.Id == null || obj.Name == string.Empty)
        {
            ModelState.AddModelError("emptyFields", "The fields cannot be empty.");
        }

        if(ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cover type edited successfully!";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //GET - Remove
    public IActionResult Remove (int? id)
    {
        if(id == null || id == 0 )
        {
            return NotFound();
        }

        var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        if(coverTypeFromDbFirst == null)
        {
            return NotFound();
        }

        return View(coverTypeFromDbFirst);
    }

    //POST - Remove
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove (CoverType obj)
    {
        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Cover type removed successfully!";
        return RedirectToAction("Index");
    }
}
