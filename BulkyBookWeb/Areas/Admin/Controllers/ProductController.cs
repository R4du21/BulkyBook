using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> objProduct = _unitOfWork.Product.GetAll();
        return View(objProduct);
    }

    //GET - Upsert
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        };

        if (id == null || id == 0)
        {          
            return View(productVM);
        }
        else
        {
            //update product
        }

        return View(productVM);
    }

    //POST - Upsert
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRoothPath = _hostEnvironment.WebRootPath; //get wwwroot path
            if(file!=null)
            {
                string fileName = Guid.NewGuid().ToString(); //creating a new file name
                var uploads = Path.Combine(wwwRoothPath, @"images\products"); //final location for the file
                var extension = Path.GetExtension(file.FileName); //getting the file extension from the filename

                using (var fileStreams = new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create)) //copying the final file
                {
                    file.CopyTo(fileStreams);
                }
                obj.Product.ImageUrl = @"\images\products\" + fileName + extension; //what will be saved in Db
            }
            _unitOfWork.Product.Add(obj.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully!";
            return RedirectToAction("Index");
        }
        return View(obj.Product);
    }

    //GET - Remove
    public IActionResult Remove(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        if (coverTypeFromDbFirst == null)
        {
            return NotFound();
        }

        return View(coverTypeFromDbFirst);
    }

    //POST - Remove
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(CoverType obj)
    {
        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Cover type removed successfully!";
        return RedirectToAction("Index");
    }
}
