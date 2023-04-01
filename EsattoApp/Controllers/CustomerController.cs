using Microsoft.AspNetCore.Mvc;

namespace EsattoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private static readonly List<Customer> CustomersList = new();

    // Seeding default data for test only
    public CustomerController()
    {
        if (CustomersList.Count == 0)
        {
            CustomersList.Add(new Customer(1, "John Doe", "123456789", DateTime.UtcNow, "London"));
            CustomersList.Add(new Customer(2, "Mary Luck", "987654321", DateTime.UtcNow.AddDays(-2), "Berlin"));
            CustomersList.Add(new Customer(3, "William Smith", "192837465", DateTime.UtcNow.AddHours(12), "Warsaw"));
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCustomers()
    {
        try
        {
            var customer = await Task.Run(() => CustomersList.ToList());
            return Ok(customer);
        }
        catch (Exception exception)
        {
            return Problem("Internal server error, please try again later...");
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        try
        {
            var customer = await Task.Run(() => CustomersList.FirstOrDefault(c => c.Id == id));
            if (customer != null)
            {
                return Ok(customer);
            }
            else
            {
                return NotFound("Not found that specific customer.");
            }
        }
        catch (Exception exception)
        {
            return Problem("Internal server error, please try again later...");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateNewCustomer([FromBody] Customer customer)
    {
        try
        {
            var newCustomerId = await Task.Run(() => CustomersList.Count == 0 ? 1 : CustomersList.Max(c => c.Id) + 1);

            customer.Id = newCustomerId;
            customer.CreationDate = DateTime.UtcNow;
            CustomersList.Add(customer);

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }
        catch (Exception exception)
        {
            return Problem("Internal server error, please try again later...");
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCustomerById(int id, [FromBody] Customer customer)
    {
        try
        {
            var customerToUpdate = await Task.Run(() => CustomersList.FirstOrDefault(c => c.Id == id));

            if (customerToUpdate != null)
            {
                customerToUpdate.Name = customer.Name;
                customerToUpdate.VatIdentificationNumber = customer.VatIdentificationNumber;
                customerToUpdate.Address = customer.Address;
                return Ok(customerToUpdate);
            }
            else
            {
                return NotFound($"Customer with ID {id} not found.");
            }
        }
        catch (Exception exception)
        {
            return Problem("Internal server error, please try again later...");
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCustomerById(int id)
    {
        try
        {
            var customerToDelete = await Task.Run(() => CustomersList.FirstOrDefault(c => c.Id == id));

            if (customerToDelete != null)
            {
                CustomersList.Remove(customerToDelete);
                return NoContent();
            }
            else
            {
                return NotFound($"Customer with ID {id} not found.");
            }
        }
        catch (Exception exception)
        {
            return Problem("Internal server error, please try again later...");
        }
    }
}