﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class ClientsController : Controller
    {
        //
        // GET: /Clients/

        public ActionResult Index(string q)
        {
            var clients = new List<Client>();
            if (!string.IsNullOrEmpty(q))
            {
                if (q.Length > 3)
                {
                    var db = new SqlExtra();
                    clients = db.GetClients(q).ToList();
                }
            }

            return View(clients);
        }

        //
        // GET: /Clients/Details/5

        public ViewResult Details(int id)
        {
            var db = new SqlExtra();
            var client = db.GetClient(id);

            return View(client);
        }
    }
}
