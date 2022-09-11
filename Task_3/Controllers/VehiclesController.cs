using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Task_3.Data;
using Task_3.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Json;

namespace Task_3.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        readonly Regex regRegex = new("^([A-Za-z]{2}[0-9]{2}[ ][A-Za-z]{3})$");

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {

            VM_Vehicles vmVehicles = new();

            vmVehicles.Vehicles = await _context.Vehicles.ToListAsync();

            var selectDt = _context.Vehicles.Select(m => m.Fuel).Distinct().ToList().OrderBy(m => m);
            ViewData["DownloadData"] = new SelectList(selectDt);
            return _context.Vehicles != null ?
                        View(vmVehicles) :
                        Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");
        }


        public async Task<IActionResult> Download(string type)
        {
            if (type == null) { return RedirectToAction("index"); }

            var data = await _context.Vehicles.Where(m => m.Fuel == type.Normalize()).Select(m=> new { m.Reg, m.Make, m.Model, m.Colour, m.Fuel }).ToListAsync();
            StringBuilder sb = new StringBuilder();
            sb.Append(($"Reg, Make, Model, Colour, Fuel"));
            sb.Append("\r\n");
            for (int i = 0; i < data.Count; i++)
            {
                
                sb.Append(($"{data[i].Reg.ToString()}," +
                    $"{data[i].Make.ToString()}," +
                    $"{data[i].Model.ToString()}," +
                    $"{data[i].Colour.ToString()}," +
                    $"{data[i].Fuel.ToString()}"));
                sb.Append("\r\n");

            }

            Response.Headers.Add("Content-Disposition", $"Attachment; filename = {type}.csv");
            return new FileContentResult(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv");

        }

        public async Task<IActionResult> Upload(List<Vehicle> v)
        {

            VM_VehicleUpload vv = new();


            foreach(var i in v)
            {
                if (!regRegex.IsMatch(i.Reg))
                {     
                    i.Message = "Invalid Registration, not uploaded";
                    i.Error = true;
                }
                if(_context.Vehicles.Where(m=> m.Reg == i.Reg).ToList().Count() > 0)
                {                  
                    i.Message = "Duplicate Registration, not uploaded";
                    i.Error = true;
                }
                if(i.Reg.Normalize().Replace(" ","") == "")
                {
                    i.Message = "No Reg found";
                    i.Error = true;
                    

                }

                if (i.Error == false && _context.Vehicles.AsNoTracking().Where(m=> m.Reg == i.Reg).Count() == 0)
                {
                    i.Message = "";
                    await _context.Vehicles.AddAsync(i);
                    await _context.SaveChangesAsync();
                }
            }
            vv.InvalidRegCount = v.Where(m => m.Error == true).Count();
            vv.Vehicles = v;
            
            return View(vv);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            if(file == null) { return RedirectToAction("index"); }
            using var memorystream = new MemoryStream();

            await file.CopyToAsync(memorystream);

            StreamReader sr = new(memorystream);

            List<Vehicle> vehicles = new();

            bool firstRow = true;

            using (var fileStream = file.OpenReadStream())
            {
                using (var reader = new StreamReader(fileStream))
                {
                    string row;
                    while ((row = reader.ReadLine()) != null)
                    {
                        if (!firstRow)
                        {
                            string[] dt = row.Split(",");
                            vehicles.Add(new Vehicle
                            {
                                Reg = dt[0],
                                Make = dt[1].Normalize(),
                                Model = dt[2].Normalize(),
                                Colour = dt[3].Normalize(),
                                Fuel = dt[4].Normalize()
                            });
                        }
                        else
                        {
                            firstRow = false;
                        }
                    }
                }

            }
                
            while (sr.ReadLine() != null)
            {
                
                firstRow = false;
            }
            return await Upload(vehicles);

        }



    }
}
