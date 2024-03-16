using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.Models.Dashboard;
using VerificationSystem.Repositories;

namespace VerificationSystem.Controllers
{
    public class DashboardController : Controller
    {
        private DashboardRepository dashboardRepository;

        public DashboardController()
        {
            dashboardRepository = new DashboardRepository();
        }

        public async Task<ActionResult> Index()
        {
            var vm = new DashboardVM();

            vm.MainCountVM = await dashboardRepository.GetMainCounts();

          

            return View(vm);
        }

    }
}