using CareerCrafterMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CareerCrafterMVC.Controllers
{
    public class JobListingController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;


        public JobListingController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CareerCrafter.API");

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }



        // GET: JobListing
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // ✅ Call /api/JobListing/my-jobs to get current employer's jobs
            var response = await _httpClient.GetAsync("api/v1/JobListing/my-jobs");

            if (response.IsSuccessStatusCode)
            {
                var jobListings = await response.Content.ReadFromJsonAsync<List<JobListing>>();
                return View(jobListings);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Failed to load job listings: {response.StatusCode}");
                ModelState.AddModelError("", $"Details: {errorContent}");
            }

            return View(new List<JobListing>());
        }



        // GET: JobListing/Create
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.JwtStatus = "JWT is missing from session.";
            }
            else
            {
                ViewBag.JwtStatus = $"JWT present: {token.Substring(0, 20)}...";
            }
            return View();
        }

        // POST: JobListing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobListing jobListing)
        {
            var employerId = HttpContext.Session.GetInt32("EmployerId");
            if (employerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(jobListing);
            }

            var jobListingDto = new JobListingCreateDto
            {
                Title = jobListing.Title,
                Description = jobListing.Description,
                Location = jobListing.Location,
                Salary = jobListing.Salary,
                Qualifications = jobListing.Qualifications,
            };

            //Attach JWT from session
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsJsonAsync("api/v1/JobListing", jobListingDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"StatusCode: {response.StatusCode}");
            ModelState.AddModelError("", $"Raw API Response: {errorContent}");
            return View(jobListing);
        }



        // GET: JobListing/Edit/5 --example
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/v1/JobListing/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var job = await response.Content.ReadFromJsonAsync<JobListing>();
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobListing job)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jobUpdateDto = new JobListingUpdateDto
            {
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                Salary = job.Salary,
                Qualifications = job.Qualifications
            };

            var response = await _httpClient.PutAsJsonAsync($"api/v1/JobListing/{job.JobListingId}", jobUpdateDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Failed to update: {response.StatusCode}");
                ModelState.AddModelError("", $"Details: {errorContent}");
                return View(job);
            }
        }


        // GET: JobListing/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"api/v1/JobListing/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jobListing = await response.Content.ReadFromJsonAsync<JobListing>();
                return View(jobListing);
            }

            ModelState.AddModelError("", "Job listing not found.");
            return RedirectToAction(nameof(Index));
        }

        // POST: JobListing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/v1/JobListing/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Failed to delete job listing.");
            return RedirectToAction(nameof(Index));
        }
    }
}
