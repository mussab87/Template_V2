using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Template.Models;
using System.Data;
using System.Data.OleDb;
using Service.DataLayer.Models;
using System.ComponentModel;
using OfficeOpenXml;

namespace Template.Controllers
{
    public class ElementController : Controller
    {
        IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _hostinEnviroment;
        private readonly AppDBContext _context;

        public ElementController(IHttpClientFactory httpClientFactory, IWebHostEnvironment _hostinEnviroment, AppDBContext context)
        {
            _httpClientFactory = httpClientFactory;
            this._hostinEnviroment = _hostinEnviroment;
            _context = context;
        }

        public IActionResult DateElementTag()
        {
            return View();
        }

        public IActionResult ExcelImportToDB()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Upload(SingleFileModel model)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files"); // use configurable folder names

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.File.FileName);
                string fileName = model.FileName + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }

                //add data to db
                var list = new List<Product>();
                using (var stream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;// you need to buy license for it for commercial use or use any free tool for commercial use. you can try https://docs.microsoft.com/en-us/office/open-xml/open-xml-sdk . https://www.nuget.org/packages/DocumentFormat.OpenXml/
                    model.File.CopyTo(stream);
                    using (var Package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = Package.Workbook.Worksheets[0];
                        var rowcount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowcount; row++)
                        {
                            list.Add(new Product
                            {
                                Id = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                Rate = Convert.ToInt32(worksheet.Cells[row, 3].Value)
                            });
                        }

                    }
                }

                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        _context.Product.Add(item);
                        _context.SaveChanges();
                    }
                }

                //****************
                return View("ExcelImportToDB", model);
            }

            return View("ExcelImportToDB", model);
        }

        public IActionResult ExcelExportAndPrint()
        {
            return View();
        }

        public IActionResult EmpData(Soldier soldier)
        {
            return View(soldier);
        }

        [HttpPost]
        public async Task<IActionResult> Search(int soldierNo)
        {
            try

            {
                var client = _httpClientFactory.CreateClient("soldier");

                var response = await client.GetAsync($"/EmployeeData/api/Soldiers/{soldierNo}");

                var stream = await response.Content.ReadAsStreamAsync();

                var result = await JsonSerializer.DeserializeAsync<Soldier>(stream,

                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                return RedirectToAction("EmpData", result);

            }

            catch (Exception ex)

            {
                ModelState.AddModelError("", "حدث خطأ فضلا حاول مرة اخرى");
                return RedirectToAction("EmpData");
            }

        }

    }
}
