using DataAccessLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLayer
{
    public class ProductBL
    {
        ProductDAL data_access = new ProductDAL();
        public Product FindProductByID(int id)
        {
            return data_access.FindProductByID(id);
        }

        public List<Product> GetAllProducts()
        {
            return data_access.GetAllProducts();
        }

        public List<Product> GetProductsByPrice(string orderProduct, int? category, string? text, int? upperprice, int? lowerprice)
        {
            return data_access.GetProductsByPrice(orderProduct,category,text,upperprice,lowerprice);
        }

        public List<Category> GetCategories()
        {
            return data_access.ListCategories();
        }

        public void AddProduct(Product input)
        {
            data_access.AddProduct(input);
        }

        public List<Product> FindProductBySellerID(int id)
        {
            var all_products = data_access.GetAllProducts();
            var newlist = all_products.Where(p => p.Sellerid == id).ToList();
            return newlist;
        }

        public void DeleteProduct(int id)
        {
            data_access.DeleteProduct(id);
        }

		public void UpdateProduct(Product updating_product)
		{
            data_access.UpdateProduct(updating_product);
		}
	}
}
