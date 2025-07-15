using CareerCrafterMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CareerCrafterMVC.Controllers
{
    public class EmployerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public EmployerController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CareerCrafter.API");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        /* ---------- GET: /Employer/CompleteProfile ---------- */
        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var token = HttpContext.Session.GetString("JWToken");

            if (userId == null || string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/v1/Employee/user/{userId}");

            var content = await response.Content.ReadAsStringAsync();
            var statusCode = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                // 👇 This means profile already exists, redirect user
                return RedirectToAction("Index", "JobListing");
            }

            ModelState.AddModelError("", $"Status: {statusCode}");
            ModelState.AddModelError("", $"Content: {content}");
            return View();
        }

        /* ---------- POST: /Employer/CompleteProfile ---------- */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(Employee model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // ⚠️ Check if profile already exists
            var existsResp = await _httpClient.GetAsync($"api/v1/Employee/user/{userId}");
            if (existsResp.IsSuccessStatusCode)
            {
                TempData["Error"] = "Profile already exists.";
                return RedirectToAction("Index", "JobListing");
            }

            // Proceed with profile creation
            var dto = new
            {
                CompanyName = model.CompanyName,
                ContactEmail = model.ContactEmail
            };

            var json = JsonSerializer.Serialize(dto, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/v1/Employee", content);

            if (response.IsSuccessStatusCode)
            {
                var respJson = await response.Content.ReadAsStringAsync();
                var created = JsonSerializer.Deserialize<Employee>(respJson, _jsonOptions);
                HttpContext.Session.SetInt32("EmployerId", created.EmployeeId);
                return RedirectToAction("Index", "JobListing");
            }

            var apiErr = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Profile completion failed: {apiErr}");
            return View(model);
        }


        /* ---------- GET: /Employer/CheckProfile ---------- */
        public async Task<IActionResult> CheckProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var token = HttpContext.Session.GetString("JWToken");

            if (userId == null || string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/v1/Employee/user/{userId}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "JobListing");

            return RedirectToAction("CompleteProfile");
        }
    }
}
