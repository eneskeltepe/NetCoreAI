using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project02_ApiConsumeUI.Dtos;
using System.Text;
using System.Threading.Tasks;

namespace Project02_ApiConsumeUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Müşteri listesini getirir
        public async Task<IActionResult> CustomerList()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.GetAsync("https://localhost:7150/api/Customers");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResultCustomerDto>>(jsonData);
                    return View(values);
                }
                // API'dan hata dönerse boş liste gönder
                ViewBag.Error = "Müşteri listesi alınamadı.";
                return View(new List<ResultCustomerDto>());
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
                ViewBag.Error = "Bir hata oluştu: " + ex.Message;
                return View(new List<ResultCustomerDto>());
            }
        }

        // Yeni müşteri ekleme formunu gösterir
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        // Yeni müşteri ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                // Model doğrulama hatası varsa formu tekrar göster
                return View(createCustomerDto);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(createCustomerDto);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync("https://localhost:7150/api/Customers", stringContent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("CustomerList");
                }
                // API'dan hata dönerse kullanıcıya bilgi ver
                ViewBag.Error = "Müşteri eklenemedi.";
                return View(createCustomerDto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Bir hata oluştu: " + ex.Message;
                return View(createCustomerDto);
            }
        }

        // Müşteri silme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.DeleteAsync($"https://localhost:7150/api/Customers/{id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("CustomerList");
                }
                ViewBag.Error = "Müşteri silinemedi.";
                return RedirectToAction("CustomerList");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("CustomerList");
            }
        }

        // Müşteri güncelleme formunu gösterir
        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.GetAsync($"https://localhost:7150/api/Customers/{id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<GetByIdCustomerDto>(jsonData);
                    return View(values);
                }
                ViewBag.Error = "Müşteri bilgisi alınamadı.";
                return RedirectToAction("CustomerList");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("CustomerList");
            }
        }

        // Müşteri güncelleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            if (!ModelState.IsValid)
            {
                // Model doğrulama hatası varsa formu tekrar göster
                return View(updateCustomerDto);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(updateCustomerDto);
                StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseMessage = await client.PutAsync("https://localhost:7150/api/Customers", stringContent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("CustomerList");
                }
                ViewBag.Error = "Müşteri güncellenemedi.";
                return View(updateCustomerDto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Bir hata oluştu: " + ex.Message;
                return View(updateCustomerDto);
            }
        }
    }
}