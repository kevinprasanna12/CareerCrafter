using CareerCrafterMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CareerCrafterMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CareerCrafter.API");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }
        // GET: AccountController
        [HttpGet]
        public IActionResult Register() => View();

        // POST: AccountController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserInfo model)
        {
            model.Role = "Employer";

            if (!ModelState.IsValid)
                return View(model);
            
            var json = JsonSerializer.Serialize(model, _jsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync("api/v1/Auth/register", content);

            // Check if the response indicates success
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Login");

            var apiError = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"API error: {apiError}");
            return View(model);
        }

        // GET: AccountController -- Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: AccountController -- Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var jsonData = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/v1/Auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid login credentials or server error.");
                return View(model);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, _jsonOptions);

            if (loginResponse == null)
            {
                ModelState.AddModelError("", "Login failed: Invalid response from server.");
                return View(model);
            }

            // Store session data
            HttpContext.Session.SetString("JWToken", loginResponse.Token);
            HttpContext.Session.SetInt32("UserId", loginResponse.UserId);
            HttpContext.Session.SetString("Username", loginResponse.UserName);
            HttpContext.Session.SetString("Role", loginResponse.Role);

            // Only Employers need to verify profile existence
            if (loginResponse.Role == "Employer")
            {
                // Attach token freshly
                var token = loginResponse.Token;
                var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/Employee/user/{loginResponse.UserId}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var empResponse = await _httpClient.SendAsync(request);

                Console.WriteLine($"empResponse.StatusCode: {empResponse.StatusCode}");

                if (empResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var empContent = await empResponse.Content.ReadAsStringAsync();
                    var employee = JsonSerializer.Deserialize<Employee>(empContent, _jsonOptions);

                    if (employee != null)
                    {
                        HttpContext.Session.SetInt32("EmployerId", employee.EmployeeId);
                        return RedirectToAction("Index", "JobListing");
                    }
                }
                else if (empResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Profile not found — go to CompleteProfile
                    return RedirectToAction("CompleteProfile", "Employer");
                }
                else
                {
                    //  Unexpected error from profile API
                    ModelState.AddModelError("", $"Unexpected error retrieving employer profile. Status: {empResponse.StatusCode}");
                    return View(model);
                }
            }

            // For non-employers
            return RedirectToAction("Index", "JobListing");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
