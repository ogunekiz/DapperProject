using Dapper;
using DapperProject.API.DataContext;
using DapperProject.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DapperProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public ProductController(DapperDbContext context)
        {
            _connection = context.CreateConnection();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = "SELECT * FROM Products";
            var products = await _connection.QueryAsync<Product>(query);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = "SELECT * FROM Products WHERE Id = @Id";
            var product = await _connection.QuerySingleOrDefaultAsync<Product>(query, new { Id = id });

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var query = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price); SELECT last_insert_rowid();";
            var id = await _connection.ExecuteScalarAsync<int>(query, product);

            product.Id = id;
            return CreatedAtAction(nameof(GetById), new { id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            var query = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(query, product);

            if (rowsAffected == 0)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var query = "DELETE FROM Products WHERE Id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(query, new { Id = id });

            if (rowsAffected == 0)
                return NotFound();

            return NoContent();
        }
    }
}
