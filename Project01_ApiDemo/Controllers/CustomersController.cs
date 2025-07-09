using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project01_ApiDemo.Context;
using Project01_ApiDemo.Entities;

namespace Project01_ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // Veritabanı işlemleri için gerekli context
        private readonly ApiContext _context;

        // Constructor - Dependency Injection ile context'i alıyoruz
        public CustomersController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/customers - Tüm müşterileri listele
        [HttpGet]
        public async Task<IActionResult> CustomerList()
        {
            // Tüm müşterileri veritabanından çek
            var customers = await _context.Customers.ToListAsync();

            // Eğer müşteri listesi boş ise 404 döndür
            if (customers.Count == 0)
                return NotFound("Müşteri bulunamadı!");

            // Başarılı ise müşteri listesini döndür
            return Ok(customers);
        }

        // GET: api/customers/{id} - ID'ye göre müşteri getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            // Belirtilen ID'deki müşteriyi bul
            var customer = await _context.Customers.FindAsync(id);

            // Eğer müşteri bulunamazsa 404 döndür
            if (customer == null)
                return NotFound("Müşteri bulunamadı!");

            // Müşteriyi döndür
            return Ok(customer);
        }

        // POST: api/customers - Yeni müşteri ekle
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            // ID'yi sıfırla (otomatik artan olması için)
            customer.CustomerId = 0;

            // Yeni müşteriyi veritabanına ekle
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            // Başarı mesajı ve eklenen müşteri bilgisini döndür
            return Ok(new
            {
                message = "Müşteri ekleme işlemi başarılı!",
                customer
            });
        }

        // PUT: api/customers - Müşteri bilgilerini güncelle
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(Customer updatedCustomer)
        {
            // Güncellenecek müşteriyi bul
            var customer = await _context.Customers.FindAsync(updatedCustomer.CustomerId);

            // Eğer müşteri bulunamazsa 404 döndür
            if (customer == null)
                return NotFound("Müşteri bulunamadı!");

            // Müşteri bilgilerini güncelle
            customer.CustomerName = updatedCustomer.CustomerName;
            customer.CustomerSurname = updatedCustomer.CustomerSurname;
            customer.CustomerBalance = updatedCustomer.CustomerBalance;

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Başarı mesajı ve güncellenmiş müşteri bilgisini döndür
            return Ok(new
            {
                message = "Müşteri güncelleme işlemi başarılı!",
                customer
            });
        }

        // DELETE: api/customers/{id} - Müşteriyi sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            // Silinecek müşteriyi bul
            var customer = await _context.Customers.FindAsync(id);

            // Eğer müşteri bulunamazsa 404 döndür
            if (customer == null)
                return NotFound("Müşteri bulunamadı!");

            // Müşteriyi sil ve değişiklikleri kaydet
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            // Başarı mesajı ve silinen müşteri bilgisini döndür
            return Ok(new
            {
                message = "Müşteri silme işlemi başarılı!",
                customer
            });
        }
    }
}