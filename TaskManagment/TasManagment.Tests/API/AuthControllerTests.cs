using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IdentityModel.Tokens.Jwt;
using TaskManagement.API.Controllers;

namespace TaskManagement.Tests.API.Controllers
{
    [TestClass]
    public class AuthControllerTests
    {
        private AuthController controller;

        [TestInitialize]
        public void Setup()
        {
            controller = new AuthController();
        }

        [TestMethod]
        public void Login_ValidAdminCredentials_ReturnsToken()
        {
            var request = new LoginRequest
            {
                Username = "admins",
                Password = "admin1234"
            };
            var result = controller.Login(request);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value!;
            var token = response.GetType().GetProperty("token")?.GetValue(response)?.ToString();

            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.AreEqual("admin-001", jwt.Claims.First(c => c.Type == "userId").Value);
            Assert.AreEqual("Admin", jwt.Claims.First(c => c.Type == "role" || c.Type.EndsWith("/role")).Value);
        }

        [TestMethod]
        public void Login_InvalidPassword_ReturnsUnauthorized()
        {
            var request = new LoginRequest
            {
                Username = "admins",
                Password = "wrongpassword"
            };
            var result = controller.Login(request);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public void Login_InvalidUsername_ReturnsUnauthorized()
        {
            var request = new LoginRequest
            {
                Username = "unknownuser",
                Password = "password"
            };

            var result = controller.Login(request);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public void Login_ValidUserCredentials_ReturnsCorrectClaims()
        {
            var request = new LoginRequest
            {
                Username = "users",
                Password = "user1234"
            };
            var result = controller.Login(request);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value!;
            var token = response.GetType().GetProperty("token")?.GetValue(response)?.ToString();

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Assert.AreEqual("user-001", jwt.Claims.First(c => c.Type == "userId").Value);
            Assert.AreEqual("User", jwt.Claims.First(c => c.Type.EndsWith("/role")).Value);
        }
    }
}
