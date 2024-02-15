using Inventory_Management_system.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Inventory_Management_system.Controllers
{
    public class SaleController : Controller
    {
        Inventory_managementEntities db = new Inventory_managementEntities();

        // GET: Sale
        public ActionResult Index()
        {
            var salelist = db.tblSales.OrderByDescending(x => x.id).ToList();
            return View(salelist);
        }

        [HttpGet] //create
        public ActionResult SaleProduct()
        {
            var prod = db.tblPurchases.Select(x => x.Purchase_pro).ToList();
            ViewBag.ProductName = new SelectList(prod);
            return View();
        }

        [HttpPost]
        public ActionResult SaleProduct(tblSale s)
        {
            int saleQuantity = int.Parse(s.Sale_qnty);
            if (1 > saleQuantity)
            {
                TempData["zero"] = "Sale Qantity Should be More then 0";
                return RedirectToAction("SaleProduct");
            }
            s.Sale_date = DateTime.Now;
            var name = s.Sale_prod;
            var purchaseQuantity = db.tblPurchases.Where(p => p.Purchase_pro == name).Select(p => p.Purchase_qnty).FirstOrDefault(); //total stock
            int pQ = int.Parse(purchaseQuantity); //convert Purchase stock to int

            TempData["Stock"] = pQ;
            TempData["msg"] = "Available Stock is : ";

            if (saleQuantity > pQ)
            {
                return RedirectToAction("SaleProduct");
            }
            else
            {
                int updateStock = pQ - saleQuantity; // new stock after sale

                var productToUpdate = db.tblPurchases.FirstOrDefault(x => x.Purchase_pro == name);
                if (productToUpdate != null)
                {

                    productToUpdate.Purchase_qnty = updateStock.ToString();
                    db.tblSales.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
            }

            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var prod = db.tblPurchases.Select(x => x.Purchase_pro).ToList(); //to deopdown menu of product
            ViewBag.ProductName = new SelectList(prod);

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var data = db.tblSales.Find(id);

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(tblSale s)
        {
            var prod = db.tblPurchases.Select(x => x.Purchase_pro).ToList(); //to dropdown menu of product
            ViewBag.ProductName = new SelectList(prod);



            tblSale obj = db.tblSales.SingleOrDefault(x => x.id == s.id);
            int preSale = int.Parse(obj.Sale_qnty); //previous sale qnty

            var name = s.Sale_prod;

            var TotalQuantity = db.tblPurchases.Where(x => x.Purchase_pro == name).Select(x => x.Purchase_qnty).FirstOrDefault(); //total stock 
            int NewTotal = int.Parse(TotalQuantity); // new total convert to int

            int UpdateSale = int.Parse(s.Sale_qnty); // edited Sale Quantity // new qnty

            int NewDifferece;
            if (preSale > UpdateSale)
            {
                NewDifferece = preSale - UpdateSale;
                NewTotal -= NewDifferece;
            }
            else
            {
                NewDifferece = UpdateSale - preSale;
                NewTotal -= NewDifferece;
            }
            ////
            var productToUpdate = db.tblPurchases.FirstOrDefault(x => x.Purchase_pro == name);
            if (productToUpdate != null)
            {
                productToUpdate.Purchase_qnty = NewTotal.ToString();

                //////

                //db.tblSales.Add(s);
                //----- OR -----//
                obj.Sale_qnty = s.Sale_qnty;

                //////////

                db.SaveChanges();
                return RedirectToAction("index");
            }
            return RedirectToAction("index");


            //-------------------- OR ---------------------  
            //if (ModelState.IsValid)
            //{
            //  db.Entry(s).State = EntityState.Modified;   //To Save Changes
            //    return RedirectToAction("Index");
            //}

            //return View(s);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var data = db.tblSales.Find(id);
            return View(data);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var data = db.tblSales.Find(id);
            return View(data);
        }
        [HttpPost]
        public ActionResult Delete(tblSale s)
        {
            var data = db.tblSales.Find(s.id);
            db.tblSales.Remove(data);
            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}