using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BethanysPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;


        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepositry)
        {
            // Provided by constructor Dependency Injection
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepositry;
        }

        public ViewResult List(string category)
        {

            IEnumerable<Pie> pies;
            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.Pies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            else
            {
                pies = _pieRepository.Pies.Where(p => p.Category.CategoryName == category)
                   .OrderBy(p => p.PieId);
                // да нахуя это надо?!
                currentCategory = _categoryRepository.Categories.FirstOrDefault(c => c.CategoryName == category).CategoryName;
            }

            // by default will search for View in Views/Pie/List.cshtml
            return View(new PiesListViewModel
            {
                Pies = pies,
                CurrentCategory = currentCategory
            });
        }
    }
}
