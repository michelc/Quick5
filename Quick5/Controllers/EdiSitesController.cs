﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class EdiSitesController : Controller
    {
        //
        // GET: /EdiSites/Details/5

        public ActionResult Details(int id)
        {
            var db = new ExtraBase();
            var site = db.EdiSites.Get(id);

            site.EdiAccord = db.EdiAccords.Get(site.Accord_ID);
            site.Client = db.Clients.List(site.NSiret).FirstOrDefault();
            if (site.Client == null) site.Client = new Client();

            return View(site);
        }

    }
}