using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCategories.Models;

namespace ProductCategories.Controllers
{
    public class HomeController : Controller
    {
         private ProCatContext _context;
        
            public HomeController(ProCatContext context)
            {
                _context = context;
            }
            public IActionResult Index()
            {
                return View("index");
            }
            [HttpPost("RegisterProcess")]
            public IActionResult Register(RegisterViewModel user){
                    if(ModelState.IsValid){
                        var userList = _context.users.Where(p => p.email== user.register_email).FirstOrDefault();
                        if(userList != null){
                            if(user.register_email == userList.email){
                                ModelState.AddModelError("register_email", "email exists");
                                return View("index");
                            }
                        }
                                                    
                    PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                    user.register_password = Hasher.HashPassword(user, user.register_password);
                    users User = new users(){
                        first_name = user.first_name,
                        last_name = user.last_name,
                        email = user.register_email,
                        password = user.register_password,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
                    };
                    _context.Add(User);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("Id", (int)User.id);
                    return RedirectToAction("Dashboard");
                }else{
                    return View("index");
                }
            }

            [HttpPost("LoginProcess")]
            public IActionResult Login(LoginViewModel User){
                if(ModelState.IsValid){
                    users user = _context.users.Where(p => p.email== User.login_Email).SingleOrDefault();
                    if(user == null){
                        ModelState.AddModelError("login_Email", "Not a valid email or password");
                        System.Console.WriteLine("Not a valid email or password");
                        return View("index");
                    }
                    else if(user != null && User.login_Password != null)
                        {
                            var Hasher = new PasswordHasher<users>();
                            if( 0 !=Hasher.VerifyHashedPassword(user, user.password, User.login_Password)){
                                HttpContext.Session.SetInt32("Id", (int)user.id);
                                int? id = HttpContext.Session.GetInt32("Id");
                            return RedirectToAction("Dashboard");
                            }
                            // else{
                            //     ModelState.AddModelError("login_Email", "Not a valid email or password");
                            //     System.Console.WriteLine("Not a valid email or password");
                            //     // ViewBag.error =  "Not a valid email or password";
                            // return View("index");
                        // }
                    }
                    
                }
                return View("index");
            }
            
            [HttpGet("dashboard")]
            public IActionResult Dashboard(){
                return View("dashboard");
            }

            [HttpGet("products")]
            public IActionResult Products(){
                List <products> allProducts = _context.products.ToList();
                ViewBag.allProducts = allProducts;
                return View("product");
            }

            [HttpGet("products/new")]
            public IActionResult NewProduct(){
                return View("createproduct");
            }

            [HttpPost("products/create")]
            public IActionResult CreateProduct(ProductViewModel newproduct){
                int? id = HttpContext.Session.GetInt32("Id");
                if(ModelState.IsValid){
                    products product = new products(){
                        usersid = (int)id,
                        name = newproduct.Name,
                        description = newproduct.description,
                        price = newproduct.price,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
                    };
                    _context.Add(product);
                    _context.SaveChanges();
                    return RedirectToAction("Products");
                }
                return View("createproduct");
            }

            [HttpGet("products/{id}")]
            public IActionResult ProductDetail(int id){
                products ProCat = _context.products.Where(x=>x.id == id).Include(z=>z.productscategories).ThenInclude(x=>x.category).FirstOrDefault();
                List <categories> allCategories = _context.categories.ToList();
                ViewBag.ProCat = ProCat;
                ViewBag.allCategories = allCategories;
                return View("productdetail");
            }

            [HttpPost("AddCategory")]
            public IActionResult AddCategory(int categoryid, int productid){
                var catpro = _context.productscategories.Where(x=>x.categoriesid == categoryid).Where(x=>x.productsid == productid).FirstOrDefault();
                if(catpro == null){
                    productscategories connect = new productscategories(){
                    categoriesid = categoryid,
                    productsid = productid,
                    };
                _context.Add(connect);
                _context.SaveChanges();
                return RedirectToAction("ProductDetail", new{id = productid});
                }
                return RedirectToAction("ProductDetail", new{id = productid});         
            }

            [HttpGet("categories")]
            public IActionResult Categories(){
                List <categories> allCategories = _context.categories.ToList();
                ViewBag.allCategories = allCategories;
                return View("category");
            }


            [HttpGet("categories/new")]
            public IActionResult NewCategory(){
                return View("createcategory"); 
            }

            [HttpPost("categories/create")]
            public IActionResult CreateCategory(CategoryViewModel newCat){
                int? id = HttpContext.Session.GetInt32("Id");
                if(ModelState.IsValid){
                    categories newcat = new categories(){
                        usersid = (int)id,
                        name = newCat.Name,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
                    };
                    _context.Add(newcat);
                    _context.SaveChanges();
                    return RedirectToAction("Categories");
                }
                return View("category");
            }

            [HttpGet("categories/{id}")]
            public IActionResult CategoryDetail(int id){
                categories category = _context.categories.Where(x=>x.id == id).Include(x=>x.CatPro).ThenInclude(x=>x.product).FirstOrDefault();
                List <products> allProducts = _context.products.ToList();
                ViewBag.category = category;
                ViewBag.allProducts = allProducts;
                return View("categorydetail");
            }

            [HttpPost("AddProduct")]
            public IActionResult addPorduct(int productid, int categoryid){
                    var catpro = _context.productscategories.Where(x=>x.categoriesid == categoryid).Where(x=>x.productsid == productid).FirstOrDefault();
                    if(catpro == null){
                        productscategories connect = new productscategories(){
                    categoriesid = categoryid,
                    productsid = productid,
                };
                _context.Add(connect);
                _context.SaveChanges();
                return RedirectToAction("CategoryDetail", new{id = categoryid});
                }else{
                    return RedirectToAction("CategoryDetail", new{id = categoryid});
                }
                    
                
            }


             [HttpGet("logout")]
            public IActionResult logout(){
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }



    }
}
