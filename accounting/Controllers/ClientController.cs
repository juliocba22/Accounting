﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using accounting.Helpers;
using accounting.Infra;
using accounting.Models;
using accounting.Repositories;
using accounting.ViewModels;

namespace accounting.Controllers
{
    [Authorize]
    [CustomAuthorizeAttribute]
    [SessionExpireFilter]
    public class ClientController : Controller
    {
        #region Variables
        private AccountingEntities1 db = new AccountingEntities1();
        private IRepoCustom _repo = new RepoCustom();
        int _pageSize = 20;

        #endregion

        #region Listado
        // GET: clients
        public ActionResult Index(string razonsocial, int page=1)
        {
            ClientVMIndex model = new ClientVMIndex { razonSocial = razonsocial, page = page };
            try
            {
                IEnumerable<ListClient> list = _repo.ClientList(model.razonSocial);

                model.list = list.OrderBy(o => o.razonSocial).Skip((page - 1) * _pageSize).Take(_pageSize);
                model.pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = list.Count()
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }

            return View(model);
        }
        #endregion

        #region Detalle
        // GET: clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.client.Find(id);
            
            ClientVM c = new ClientVM()
            {
                id = client.id,
                codigo = client.codigo,
                razonSocial = client.razonSocial,
                nombreFantasia = client.nombreFantasia,
                localidad = client.localidad,
                provincia = client.provincia,
                personeria = client.personeria,
                telefono = client.telefono,
                email = client.email,
                emailFacturacon = client.emailFacturacon
            };

            return View(c);
        }
        #endregion

        #region Nuevo
        // GET: clients/Create
        public ActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientVM client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    client c = new client()
                    {
                        codigo = client.codigo,
                        razonSocial = client.razonSocial,
                        nombreFantasia = client.nombreFantasia,
                        localidad = client.localidad,
                        provincia = client.provincia,
                        personeria = client.personeria,
                        telefono = client.telefono,
                        email = client.email,
                        emailFacturacon = client.emailFacturacon,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1
                    };

                    db.client.Add(c);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }
        #endregion

        #region Edicion
        // GET: clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.client.Find(id);
            
            ClientVM c = new ClientVM ()
            {
                id = client.id,
                codigo = client.codigo,
                razonSocial = client.razonSocial,
                nombreFantasia = client.nombreFantasia,
                localidad = client.localidad,
                provincia = client.provincia,
                personeria = client.personeria,
                telefono = client.telefono,
                email = client.email,
                emailFacturacon = client.emailFacturacon
            };
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientVM client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    client c = new client()
                    {
                        id = client.id,
                        codigo = client.codigo,
                        razonSocial = client.razonSocial,
                        nombreFantasia = client.nombreFantasia,
                        localidad = client.localidad,
                        provincia = client.provincia,
                        personeria = client.personeria,
                        telefono = client.telefono,
                        email = client.email,
                        emailFacturacon = client.emailFacturacon,
                        update_date = DateTime.Now,
                        update_user_id = int.Parse(Session["UserID"].ToString()),
                        activo = 1
                    };

                    db.Entry(c).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }
        #endregion

        #region Eliminacion
        // GET: clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.client.Find(id);

            ClientVM c = new ClientVM()
            {
                id = client.id,
                codigo = client.codigo,
                razonSocial = client.razonSocial
            };

            return View(c);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                client client = db.client.Find(id);
                client.activo = 0;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Se produjo un error, en caso de persistir, ponerse en contacto con el Administrador.");
            }
            return View();
        }

        #endregion

        #region Metodos Privados
        public FileContentResult Export([Bind(Include = "razonSocial")] string razonSocial)
        {
            StringBuilder csv = new StringBuilder();
            IEnumerable<ReportClient> listado = _repo.ClientReport(razonSocial);

            csv.AppendLine("codigo;razonSocial;localidad;provincia;telefono;email;emailFacturacion");
            foreach (var item in listado)
                csv.AppendLine(item.ToString());

            string archivo = "ReporteListadoClientes_" + DateTime.Now.ToString("yyyyMMdd");
            return File(Encoding.Default.GetBytes(csv.ToString()), "text/csv", archivo + ".csv");
        }

        #endregion
    }
}
