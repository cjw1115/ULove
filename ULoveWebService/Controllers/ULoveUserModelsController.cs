using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ULoveWebService.Attributes;
using ULoveWebService.Models;

namespace ULoveWebService.Controllers
{
    [TokenAuthorize]
    public class ULoveUserModelsController : ApiController
    {
        private ULoveWebServiceContext db = new ULoveWebServiceContext();

        
        // GET: api/ULoveUserModels
        public IQueryable<ULoveUserModel> GetULoveUserModels()
        {
            return db.ULoveUserModels;
        }

        // GET: api/ULoveUserModels/5
        [ResponseType(typeof(ULoveUserModel))]
        public IHttpActionResult GetULoveUserModel(int id)
        {
            ULoveUserModel uLoveUserModel = db.ULoveUserModels.Find(id);
            if (uLoveUserModel == null)
            {
                return NotFound();
            }

            return Ok(uLoveUserModel);
        }

        // PUT: api/ULoveUserModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutULoveUserModel(int id, ULoveUserModel uLoveUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uLoveUserModel.ID)
            {
                return BadRequest();
            }

            db.Entry(uLoveUserModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ULoveUserModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ULoveUserModels
        [ResponseType(typeof(ULoveUserModel))]
        public IHttpActionResult PostULoveUserModel(ULoveUserModel uLoveUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ULoveUserModels.Add(uLoveUserModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = uLoveUserModel.ID }, uLoveUserModel);
        }

        // DELETE: api/ULoveUserModels/5
        [ResponseType(typeof(ULoveUserModel))]
        public IHttpActionResult DeleteULoveUserModel(int id)
        {
            ULoveUserModel uLoveUserModel = db.ULoveUserModels.Find(id);
            if (uLoveUserModel == null)
            {
                return NotFound();
            }

            db.ULoveUserModels.Remove(uLoveUserModel);
            db.SaveChanges();

            return Ok(uLoveUserModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ULoveUserModelExists(int id)
        {
            return db.ULoveUserModels.Count(e => e.ID == id) > 0;
        }
    }
}