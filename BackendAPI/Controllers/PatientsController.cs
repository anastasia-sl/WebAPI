using BackendAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BackendAPI.Models;
using System.Data.Entity.Infrastructure;
using System.Configuration;

namespace BackendAPI.Controllers
{
    public class PatientsController : ApiController
    {
        private readonly PatientContext _db = new PatientContext(ConfigurationManager.ConnectionStrings[0].ConnectionString);

        // GET: api/patients
        public IEnumerable<Patient> GetPatients(string searchString = "")
        {
            var patients = _db.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.Name.Contains(searchString));
            }

            return patients.ToList();
        }

        // GET: api/patients/5
        public async Task<IHttpActionResult> GetPatient(int id)
        {
            Patient patient = await _db.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // POST: api/patients
        public async Task<IHttpActionResult> PostPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = patient.Id }, patient);
        }

        // PUT: api/patients/5
        public async Task<IHttpActionResult> PutPatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.Id)
            {
                return BadRequest();
            }

            _db.Entry(patient).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // DELETE: api/patients/5
        public async Task<IHttpActionResult> DeletePatient(int id)
        {
            Patient patient = await _db.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return _db.Patients.Count(e => e.Id == id) > 0;
        }

        // GET: api/patients?name={searchString}
        public IHttpActionResult GetPatientsByName(string name)
        {
            var filteredPatients = _db.Patients.Where(p => p.Name.Contains(name)).ToList();
            return Ok(filteredPatients);
        }

        // POST: api/patients
        public IHttpActionResult PostPatients(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Patients.Add(patient);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.Id }, patient);
        }
    }
}