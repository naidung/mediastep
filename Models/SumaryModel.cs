namespace TestProject.Models
{
    public class SumaryModel
    {
        public int ManID { get; set; }
        public string ManName { get; set; }
        public string EmpName { get; set; }
        public DateTime EmpDOB { get; set; }

        public static List<SumaryModel> GetSummary(TestDbContext TestDB)
        {
            var list1 = (from man in TestDB.Managers
                         join relate in TestDB.ManagerEmployee2s on man.ManagerId equals relate.ManagerId
                         join emp in TestDB.Employees on relate.EmpId equals emp.EmpId
                         select new SumaryModel { ManID = man.ManagerId, ManName = man.FullName, EmpName = emp.FullName, EmpDOB = emp.Dob }
                       ).GroupBy(p => p.ManID).Select(p => p.ToList()).ToList();
            
            var list2 = new List<SumaryModel>();
            list1.OrderBy(p => p[0].ManName).ToList().ForEach(p1=> p1.OrderBy(t=>t.EmpDOB).ToList().ForEach(p2=> list2.Add(p2)));
            return list2;
        }

    }
}
