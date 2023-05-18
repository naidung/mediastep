using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TestProject.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TestDbContext _TestDB;

        public HomeController(TestDbContext TestDB, ILogger<HomeController> logger)
        {
            _logger = logger;
            _TestDB = TestDB;
        }

        public IActionResult Index(ManEmpVM? _VM=null)
        {
            if(_VM.managers == null && _VM.employees==null)
            {
                var mans = (from man in _TestDB.Managers
                            select man as Manager).ToList();
                var emps = (from emp in _TestDB.Employees
                            select emp as Employee).ToList();
                var VM = new ManEmpVM
                {
                    employees = emps,
                    managers = mans
                };

                return View(VM);
            }
            if (_VM.managers == null) _VM.managers = new List<Manager>();
            if (_VM.employees == null) _VM.employees = new List<Employee>();
            return View(_VM);
        }

        [HttpPost]
        public JsonResult SearchManByString(string SearchString)
        {
            if (SearchString == null)
            {
                SearchString = "";
            }
            var mans = (from man in _TestDB.Managers
                        select man as Manager).ToList();
            mans = mans.Where(p => p.FullName.ToLower().Contains(SearchString.ToLower())).ToList();

            return Json(JsonConvert.SerializeObject(mans));
        }

        [HttpPost]
        public JsonResult SearchEmpByString(string SearchString)
        {
            if (SearchString == null)
            {
                SearchString = "";
            }
            var emps = (from emp in _TestDB.Employees
                        select emp as Employee).ToList();
            emps = emps.Where(p => p.FullName.ToLower().Contains(SearchString.ToLower())).ToList();

            return Json(JsonConvert.SerializeObject(emps));
        }

        [HttpPost]
        public JsonResult InsertEmpAndRelateTable(string data)
        {
            var obj = JsonConvert.DeserializeObject<InsertEmpAndRelateTableModel>(data);

            _TestDB.Employees.Add(new Employee { FullName = obj.EmpName, Dob = obj.EmpDOB });

            _TestDB.SaveChanges();

            long recentlyID = _TestDB.Employees.Max(p => p.EmpId);

            foreach (var id in obj.IDs)
            {
                _TestDB.ManagerEmployee2s.Add(new ManagerEmployee2 { EmpId = recentlyID, ManagerId = id });
                _TestDB.SaveChanges();
            }

            return Json(true);
        }

        public IActionResult Privacy()
        {
            var vm = SumaryModel.GetSummary(_TestDB);
            return View(vm);
        }

        [HttpPost]
        public JsonResult AddMoreManager(string name)
        {
            try
            {
                var man = new Manager() { FullName = name };
                _TestDB.Managers.Add(man);
                _TestDB.SaveChanges();
                return new JsonResult(true);
            }
            catch(Exception ex)
            {
                return new JsonResult(false);
            }
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}