using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Models;
using Microsoft.Data.SqlClient;
using AdventureWorks.Services;
using AdventureWorks.ViewModel;

namespace AdventureWorks.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AdventureWorks2016Context _context;
        private readonly IFactoriaEspecificaciones _factoria;

        public ProductsController(AdventureWorks2016Context context,IFactoriaEspecificaciones fabrica)
        {
            _context = context;
            _factoria = fabrica;
        }

        // GET: Products

        [Route("[controller]")]
        [Route("[controller]/{consulta}")]
        public async Task<IActionResult> Index(string? consulta)
        {
            var adventureWorks2016Context = _context.Products.Include(p => p.ProductModel).Include(p => p.ProductSubcategory).Include(p => p.SizeUnitMeasureCodeNavigation).Include(p => p.WeightUnitMeasureCodeNavigation);

            if (consulta == null) return View(await adventureWorks2016Context.ToListAsync());

            var consulta1 = adventureWorks2016Context.
                Where(x => x.Color == "Red" || x.Color == "Green" && x.ListPrice > 1).
                OrderBy(x => x.SafetyStockLevel);

            var consulta2 = adventureWorks2016Context.
                Where(x => x.Color == "Red" && x.ProductSubcategoryId != 2 && !x.Name.EndsWith("x") && !x.Name.EndsWith("a") && !x.Name.EndsWith("e") && !x.Name.EndsWith("i") && !x.Name.EndsWith("o") && !x.Name.EndsWith("u")).
                OrderBy(x => x.Name);

            var consulta3 = adventureWorks2016Context.
                Where(x => x.Name.StartsWith("A") || x.Name.StartsWith("B") || x.Name.StartsWith("C") || x.Name.Contains("e")).
                OrderBy(x => x.SellStartDate).
                ThenBy(x => x.Color);

            var FabricaConsultas = _factoria.dameInstancia(EnumeracionEjercicios.Ejercicio);
            var consulta4 = (FabricaConsultas as IConsultaProducto).dameProductos(adventureWorks2016Context);
            //var consulta4 =
            //    adventureWorks2016Context.Where(x => x.Color == "Red" || x.Color == "Black" || x.Color == "Blue" && x.ListPrice >2 && x.Name.StartsWith("A") || x.Name.StartsWith("B") || x.Name.StartsWith("C")).
            //        OrderBy(x=>x.Name);

            switch (consulta.ToLower())
            {
                case "consulta1": return View(await consulta1.ToListAsync());
                case "consulta2": return View(await consulta2.ToListAsync());
                case "consulta3": return View(await consulta3.ToListAsync());
                case "consulta4": return View(consulta4);
                default: return View(await adventureWorks2016Context.ToListAsync());
            }
        }

        public async Task<IActionResult> Compacto()
        {
            var resultado = from SaleOrd in _context.SalesOrderDetails
                join Pro in _context.Products
                    on SaleOrd.ProductId equals Pro.ProductId
                select new SalesOrderProductViewModel()
                {
                    Id = SaleOrd.ProductId,
                    NombreProducto = Pro.Name,
                    ColorProducto = Pro.Color,
                    PrecioUnitarioProducto = SaleOrd.UnitPrice
                };
            /*var consulta = resultado.Where(x=>x.O)*/;
            return View(resultado);
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductModel)
                .Include(p => p.ProductSubcategory)
                .Include(p => p.SizeUnitMeasureCodeNavigation)
                .Include(p => p.WeightUnitMeasureCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductModelId"] = new SelectList(_context.ProductModels, "ProductModelId", "ProductModelId");
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "ProductSubcategoryId", "ProductSubcategoryId");
            ViewData["SizeUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode");
            ViewData["WeightUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,ProductNumber,MakeFlag,FinishedGoodsFlag,Color,SafetyStockLevel,ReorderPoint,StandardCost,ListPrice,Size,SizeUnitMeasureCode,WeightUnitMeasureCode,Weight,DaysToManufacture,ProductLine,Class,Style,ProductSubcategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,Rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductModelId"] = new SelectList(_context.ProductModels, "ProductModelId", "ProductModelId", product.ProductModelId);
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "ProductSubcategoryId", "ProductSubcategoryId", product.ProductSubcategoryId);
            ViewData["SizeUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.SizeUnitMeasureCode);
            ViewData["WeightUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.WeightUnitMeasureCode);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductModelId"] = new SelectList(_context.ProductModels, "ProductModelId", "ProductModelId", product.ProductModelId);
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "ProductSubcategoryId", "ProductSubcategoryId", product.ProductSubcategoryId);
            ViewData["SizeUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.SizeUnitMeasureCode);
            ViewData["WeightUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.WeightUnitMeasureCode);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ProductNumber,MakeFlag,FinishedGoodsFlag,Color,SafetyStockLevel,ReorderPoint,StandardCost,ListPrice,Size,SizeUnitMeasureCode,WeightUnitMeasureCode,Weight,DaysToManufacture,ProductLine,Class,Style,ProductSubcategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,Rowguid,ModifiedDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["ProductModelId"] = new SelectList(_context.ProductModels, "ProductModelId", "ProductModelId", product.ProductModelId);
            ViewData["ProductSubcategoryId"] = new SelectList(_context.ProductSubcategories, "ProductSubcategoryId", "ProductSubcategoryId", product.ProductSubcategoryId);
            ViewData["SizeUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.SizeUnitMeasureCode);
            ViewData["WeightUnitMeasureCode"] = new SelectList(_context.UnitMeasures, "UnitMeasureCode", "UnitMeasureCode", product.WeightUnitMeasureCode);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductModel)
                .Include(p => p.ProductSubcategory)
                .Include(p => p.SizeUnitMeasureCodeNavigation)
                .Include(p => p.WeightUnitMeasureCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
