using BackendAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace BackendAPI
{
    public class PatientsRepository
    {
        private readonly PatientContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public PatientsRepository(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _baseUrl = configuration["MyApiSettings:BaseUrl"];
        }

        public async Task<IEnumerable<Patient>> GetPatientsAsync(string searchString = "")
        {
            IQueryable<Patient> patientsQuery = _context.Patients;

            if (!string.IsNullOrEmpty(searchString))
            {
                patientsQuery = patientsQuery.Where(p => p.Name.Contains(searchString));
            }

            return await patientsQuery.ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            Patient patientToDelete = await _context.Patients.FindAsync(id);
            if (patientToDelete != null)
            {
                _context.Patients.Remove(patientToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}