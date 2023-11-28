using Banking_Application.Models;
using Banking_Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository repository;

        public UserController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UserModel>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await repository.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var user = await repository.GetUserAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddUser([FromBody] string userName)
        {
            try
            {
                var newUser = await repository.AddUserAsync(userName);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await repository.DeleteUserAsync(userId);
                if (!result)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                return Ok($"User with ID {userId} deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("{userId}/accounts")]
        [ProducesResponseType(typeof(AccountModel), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddAccount(int userId, [FromBody] decimal initialBalance)
        {
            try
            {
                var account = await repository.AddAccountAsync(userId, initialBalance);
                return Ok(account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{userId}/accounts/{accountId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAccount(int userId, int accountId)
        {
            try
            {
                var result = await repository.DeleteAccountAsync(userId, accountId);
                if (!result)
                {
                    return NotFound($"Account with ID {accountId} not found for user with ID {userId}.");
                }
                return Ok($"Account with ID {accountId} deleted for user with ID {userId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("deposit/{accountId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Deposit(int accountId, [FromBody] decimal amount)
        {
            try
            {
                var result = await repository.DepositAsync(accountId, amount);
                if (!result)
                {
                    return BadRequest("Deposit failed. Check if the amount is positive and does not exceed $10,000.");
                }
                return Ok("Deposit successful.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("withdraw/{accountId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Withdraw(int accountId, [FromBody] decimal amount)
        {
            try
            {
                var result = await repository.WithdrawAsync(accountId, amount);
                if (!result)
                {
                    return BadRequest("Withdrawal failed. Check if the amount is positive, does not exceed 90% of the account balance, and leaves at least $100 in the account.");
                }
                return Ok("Withdrawal successful.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
