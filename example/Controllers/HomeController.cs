using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication.Controllers
{
	public class HomeController : Controller
	{
		MembershipProvider mongoMember = Membership.Provider;


		public ActionResult Index()
		{
			ViewModel.Message = "Welcome to ASP.NET MVC!";
			
			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
