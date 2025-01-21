using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using EntityLayer;
using ETicaretSitesi.ExtraClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http.HttpResults;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace ETicaretSitesi.Controllers
{
    public class UserController : Controller
    {
        UserBL user_manager = new UserBL();
        ProductBL product_manager = new ProductBL();
        [HttpGet]
        public IActionResult register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult register(User kullanıcı)
        {
            if (ModelState.IsValid)
            {
                kullanıcı.Password = SHA256Converter.ComputeSha256Hash(kullanıcı.Password);
                kullanıcı.UserRegisterDate = DateTime.Now;
                kullanıcı.UserOnlineDate = DateTime.Now;
                user_manager.AddBl(kullanıcı);
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User kullanıcı)
        {
            if (kullanıcı.UserName==null || kullanıcı.Password == null)
            {
                ViewBag.errmsg = "Kullanıcı adı veya şifre boş girilemez";
                return View();
            }
            else
            {
                string hashed_pass = SHA256Converter.ComputeSha256Hash(kullanıcı.Password);
                bool result = user_manager.Login(kullanıcı.UserName, hashed_pass);
                if (result == true)
                {
                    int userid = user_manager.FindUserIDByUserName(kullanıcı.UserName);
                    HttpContext.Session.SetInt32("LoggedUserID", userid);
                    HttpContext.Session.SetString("LoggedUserOnline", "true");

                    // Kullanıcı giriş yaptıgında sepetim diye bir cookie ekilyoruz.
                    // Cookie var mı diye kontrol ettik. Aşagıda önce cookie'yi seçtik.
                    // Sonrasında equals ile seçilen deger boş mu diye baktık.

                    var sepetimvarmı = HttpContext.Request.Cookies.Where(c => c.Key == "sepetim").FirstOrDefault();
                    if (sepetimvarmı.Equals(new KeyValuePair<string, string>()))
                    {
                        CookieOptions cookieOptions = new CookieOptions();
                        cookieOptions.Expires = DateTime.Now.AddMonths(1);
                        cookieOptions.Secure = true;
                        cookieOptions.IsEssential = true;
                        cookieOptions.Path = "/";
                        HttpContext.Response.Cookies.Append("sepetim", $"{userid}:", cookieOptions);
                    }
                    return RedirectToAction("UserMainPage");
                }
                else
                {
                    ViewBag.errmsg = "Kullanıcı adı veya şifre hatalı";
                    return View();
                }
            }
        }
        public IActionResult MyProfile()
        {
            if (HttpContext.Session.GetString("LoggedUserOnline")=="true")
            {
				int id = Convert.ToInt32(HttpContext.Session.GetInt32("LoggedUserID"));
				return View(user_manager.MyProfile(id));
			}
            else
            {
                return RedirectToAction("Login");
            }
           
        }

        public IActionResult AddToCart(int productid)
        {
            int userid = -1;
            if (HttpContext.Session.GetInt32("LoggedUserID") == null)
            {
                // ip adresiyle sepet tutulmalı
            }
            else
            {
                userid = Convert.ToInt32(HttpContext.Session.GetInt32("LoggedUserID"));
            }

            var sepetimvarmı = HttpContext.Request.Cookies.Where(c => c.Key == "sepetim").FirstOrDefault();
            if (sepetimvarmı.Equals(new KeyValuePair<string, string>()))
            {
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddMonths(1);
                cookieOptions.Secure = true;
                cookieOptions.IsEssential = true;
                cookieOptions.Path = "/";
                // Giriş yapmadan sepete bir şey eklemeye çalışırsa burası çalışacak.
                HttpContext.Response.Cookies.Append("sepetim", $"{userid}:{productid}", cookieOptions);
            }
            else
            {
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddMonths(1);
                cookieOptions.Secure = true;
                cookieOptions.IsEssential = true;
                cookieOptions.Path = "/";
                // Önceki sepeti al
                string öncekisepet = HttpContext.Request.Cookies["sepetim"];
                string yenisepet = öncekisepet += $",{productid}";
                HttpContext.Response.Cookies.Append("sepetim", $"{yenisepet}", cookieOptions);
            }

            return RedirectToAction("MyCart");
        }

             
        public IActionResult MyCart()
        {
            
            string öncekisepet = HttpContext.Request.Cookies["sepetim"];
            string urunler = öncekisepet.Split(":")[1];
            string kullanıcı = öncekisepet.Split(":")[0];

            if (HttpContext.Session.GetInt32("LoggedUserID") == Convert.ToInt32(kullanıcı) || urunler != "")
            {
                var tekli_urunler = urunler.Split(",");
                List<ProductWithQuantity> urunlistesi = new List<ProductWithQuantity>();
                foreach (var urun in tekli_urunler)
                {
                    // Aşagıdaki count degerinin amacı ürünlerin sayısını buldurmak
                    int count = 0;

                    // Buradaki foreach'in amacı elimideki urun degerini diger tüm
                    // ürünlerle karşılaştırmak
                    foreach (var urun2 in tekli_urunler)
                    {
                        if (urun == urun2)
                        {
                            count++;
                        }
                    }

                    if (urun != "")
                    {
                        ProductWithQuantity productWithQuantity = new ProductWithQuantity();
                        productWithQuantity.product = product_manager.FindProductByID(Convert.ToInt32(urun));
                        productWithQuantity.quantity = count;

                        // Aşagıdaki deger sepette ürün zaten ekli mi diye kontrol amaçlı açıldı.

                        bool urun_zaten_ekli_mi = false;
                        foreach (var item in urunlistesi)
                        {
                            if (item.product.ProductId== productWithQuantity.product.ProductId)
                            {
                                urun_zaten_ekli_mi = true;
                            }
                          
                        }
                        if (urun_zaten_ekli_mi==true)
                        {

                        }
                        else
                        {
                            urunlistesi.Add(productWithQuantity);
                        }
                       

                    }
                }
                int toplamfiyat = 0;
                foreach (var urun in urunlistesi)
                {
                    toplamfiyat += urun.product.Price;
                }
                ViewBag.total = toplamfiyat;
                return View(urunlistesi);
            }
            else
            {
                return RedirectToAction("Login");
            }


        }


        // Clear cart metodunu yaz.
        public IActionResult ClearCart()
        {
            string öncekisepet = HttpContext.Request.Cookies["sepetim"];
            string kullanıcı = öncekisepet.Split(":")[0];
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMonths(1);
            cookieOptions.Secure = true;
            cookieOptions.IsEssential = true;
            cookieOptions.Path = "/";
            HttpContext.Response.Cookies.Append("sepetim", $"{kullanıcı}:", cookieOptions);
            return RedirectToAction("MyCart");
        }   


        [HttpGet]
        public IActionResult PasswordChange()
        {
			if (HttpContext.Session.GetString("LoggedUserOnline") == "true")
			{
				return View();
			}
			else
			{
				return RedirectToAction("Login");
			}
			
        }

        [HttpPost]
        public IActionResult PasswordChange(string oldpassword, string newpassword, string newpassword2)
        {
            if (oldpassword==null || newpassword == null || newpassword2 == null)
            {
                ViewBag.errmsg = "Şifrelerin hiçbiri boş girilemez";
                return View();
            }
            else
            {
                if (HttpContext.Session.GetString("LoggedUserOnline") == "true")
                {
                    int userid = Convert.ToInt32(HttpContext.Session.GetInt32("LoggedUserID"));
                    string hashed_old_pass = SHA256Converter.ComputeSha256Hash(oldpassword);
                    string hashed_new_pass_1 = SHA256Converter.ComputeSha256Hash(newpassword);
                    string hashed_new_pass_2 = SHA256Converter.ComputeSha256Hash(newpassword2);
                    bool result = user_manager.UpdatePassword(userid, hashed_old_pass, hashed_new_pass_1, hashed_new_pass_2);
                    if (result == true)
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("usermainpage");
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }

      
        /// <summary>
        /// Kullanıcının sepetinden ürün silme işlemini yapar.
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DeleteFromCart(int productid)
        {
            string öncekisepet = HttpContext.Request.Cookies["sepetim"];
            string urunler = öncekisepet.Split(":")[1];
            string kullanıcı = öncekisepet.Split(":")[0];
            var tekli_urunler = urunler.Split(",");
            List<string> urunlerlistesi = new List<string>();
            foreach (var urun in tekli_urunler)
            {
                if (urun != "")
                {
                    urunlerlistesi.Add(urun);
                }
            }
            urunlerlistesi.Remove(productid.ToString());
            string yeniurunler = "";
            foreach (var urun in urunlerlistesi)
            {
                yeniurunler += urun + ",";
            }
            //string sonurunler = yeniurunler.Remove(yeniurunler.Length - 1);
            string sonsepet = $"{kullanıcı}:{yeniurunler}";
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMonths(1);
            cookieOptions.Secure = true;
            cookieOptions.IsEssential = true;
            cookieOptions.Path = "/";
            HttpContext.Response.Cookies.Append("sepetim", $"{sonsepet}", cookieOptions);
            return RedirectToAction("MyCart");

        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedUserID");
            HttpContext.Session.Remove("LoggedUserOnline");
            return RedirectToAction("UserMainPage");
        }

        //[HttpGet]
        //public IActionResult AddAdress()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult AddAdress(Address adress)
        //{
        //    adress.UserID = Convert.ToInt32(HttpContext.Session.GetInt32("LoggedUserID"));
        //    user_manager.AddAdress(adress);
        //    return RedirectToAction("MyProfile");
        //}

        //[HttpGet]
        //public IActionResult EditAdress(int adressid)
        //{
        //    return View(user_manager.FindAdress(adressid));
        //}

        //[HttpPost]
        //public IActionResult EditAdress(Address adress)
        //{
        //    user_manager.EditAdress(adress);
        //    return RedirectToAction
        //public List<SelectListItem> LoadCategories()
        //{
        //    var list = new List<SelectListItem>();
        //    var category = product_manager.GetCategories();
        //    foreach (var item in category)
        //    {
        //        SelectListItem p = new SelectListItem();
        //        {
        //            p.Text = item.CategoryName;
        //            p.Value = item.CategoryId.ToString();
        //        }
        //        list.Add(p);
        //    }
        //    return list;
        //}


        public List<SelectListItem> LoadCategories()
        {
            var list = new List<SelectListItem>();
            var category = product_manager.GetCategories();
            foreach (var item in category)
            {
                SelectListItem p = new SelectListItem();
                {
                    p.Text = item.CategoryName;
                    p.Value = item.CategoryId.ToString();
                }
                list.Add(p);
            }
            return list;
        }


        [HttpGet]
        public IActionResult UserMainPage()
        {
            ViewBag.category = LoadCategories();
            var products = product_manager.GetAllProducts();
            return View(products);
        }

        [HttpPost]
        public IActionResult UserMainPage(string filterUpperPrice,string filterLowerPrice,string filterText,string filterCategory, string sıralama)
        {

            int? upper_price = 0;
            if (filterUpperPrice !=null)
            {
                upper_price = Convert.ToInt32(filterUpperPrice);
            }
            else
            {
                upper_price = null;
            }

            int? lower_price = 0;
            if (filterLowerPrice != null)
            {
                lower_price = Convert.ToInt32(filterLowerPrice);
            }
            else
            {
                lower_price = null;
            }

            int? category = 0;
            if (filterCategory != null)
            {
                category = Convert.ToInt32(filterCategory);
            }
            else
            {
                category = null;
            }


            string? text = "";
            if (filterText != null)
            {
                text = filterText;
            }
            else
            {
                text = null;
            }

            string sıralama_sekli = "";
            if (sıralama== "Fiyata Göre Artan Sırala")
            {
                sıralama_sekli = "asc";
            }
            else if (sıralama == "Fiyata Göre Azalan Sırala")
            {
                sıralama_sekli = "desc";
            }
            else
            {
               
            }
            //var asc_isaretli_mi = formc.Where(nesne => nesne.Key.Contains("Asc")).FirstOrDefault();
            //if (asc_isaretli_mi.Key!=null)
            //{
            //    sıralama_sekli = "asc";
            //}

            //var desc_isaretli_mi = formc.Where(nesne => nesne.Key.Contains("Desc")).FirstOrDefault();
            //if (desc_isaretli_mi.Key != null)
            //{
            //    sıralama_sekli = "desc";
            //}

            var result = product_manager.GetProductsByPrice(sıralama_sekli, category, text, upper_price, lower_price);
            ViewBag.category = LoadCategories();
            return View(result);
        }

        // Ürün detaylarını gösteren bir sayfa oluşturun.
        public IActionResult ProductDetail(int productid)
        {
            return View(product_manager.FindProductByID(productid));
        }


    }
}