using Inventory_Management_system.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Inventory_Management_system.Controllers
{
    public class PurchaseController : Controller
    {
        Inventory_managementEntities db = new Inventory_managementEntities();

        // GET: Purchase
        public ActionResult Index()
        {
            var prod = db.tblPurchases.OrderByDescending(x => x.id).ToList();
            return View(prod);
        }

        public ActionResult DisplayStock()
        {
            var stock = db.tblPurchases.OrderByDescending(x => x.id).ToList();
            return View(stock);
        }
        public ActionResult StockDetails(int id)
        {
            //var stockDetails = db.tblPurchases.Find(id);
            var stockDetails = db.tblPurchases.Where(x=>x.id==id).FirstOrDefault();
            return View(stockDetails);
        }
        [HttpGet]
        public ActionResult PurchaseProduct() //Create
        {
            return View();
        }

        [HttpPost]
        public ActionResult PurchaseProduct(tblPurchase ps)
        {
            db.tblPurchases.Add(ps);
            db.SaveChanges();
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

            var data = db.tblPurchases.Find(id);

            if(data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(tblPurchase ps)
        {
            var prod = db.tblPurchases.Select(x => x.Purchase_pro).ToList(); //to deopdown menu of product
            ViewBag.ProductName = new SelectList(prod);

            //tblPurchase obj = new tblPurchase();
            // obj = db.tblPurchases.Find(ps.id);

            tblPurchase obj = db.tblPurchases.SingleOrDefault(x => x.id == ps.id);
            obj.Purchase_qnty = ps.Purchase_qnty;
            obj.Purchase_date = ps.Purchase_date;
            obj.Purchase_pro = ps.Purchase_pro;
            db.SaveChanges();
            return RedirectToAction("index");


            //-------------------- OR --------------------- 
            //if (ModelState.IsValid)
            //{
            //    db.Entry(ps).State = EntityState.Modified;
            //    return RedirectToAction("Index");
            //}

            //return View(ps);

        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var data = db.tblPurchases.Find(id);
            return View(data);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var data = db.tblPurchases.Find(id);
            return View(data);
        }
        [HttpPost]
        public ActionResult Delete(tblPurchase ps)
        {
            var data = db.tblPurchases.Find(ps.id);
            db.tblPurchases.Remove(data);
            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}