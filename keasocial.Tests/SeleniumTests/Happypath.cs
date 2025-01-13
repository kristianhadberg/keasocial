using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using SeleniumExtras.WaitHelpers;

namespace keasocial.Tests.SeleniumTests
{
    [Trait("Category", "Selenium")]
    public class Happypath
    {

        private readonly IWebDriver _driver;
        private readonly string _baseUrl;

        public Happypath()
        {
            _driver = new ChromeDriver();
            _baseUrl = "http://localhost:5260";
        }

        [Fact]
        [Trait("Selenium", "All")]
        [Trait("Selenium", "1")]
        //dotnet test --filter "Selenium=All" To run all tests
        //dotnet test --filter "Selenium=1" To run test number 1 etc.
        public void CreatePostTest()
        {
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_baseUrl);
            Thread.Sleep(1000);

            //Find and click GetWeather button
            var GetWeatherButton = _driver.FindElement(By.Id("get-weather"));
            Thread.Sleep(1000);
            GetWeatherButton.Click();
            Thread.Sleep(1000);

            // Find Create input field and submit button
            var inputField = _driver.FindElement(By.Name("content"));
            var submitButton = _driver.FindElement(By.XPath("//input[@value='createPost']"));

            // Fill input with information and submit
            const string postContet = "Hello there! This is me testing if i can make a post!";
            inputField.SendKeys(postContet);
            submitButton.Click();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for the alert to appear then press OK
            wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert = _driver.SwitchTo().Alert();
            Thread.Sleep(1000);
            alert.Accept();

            Thread.Sleep(1000);
            inputField.Clear();
            Thread.Sleep(1000);

            _driver.Quit();
        }

        [Fact]
        [Trait("Selenium", "All")]
        [Trait("Selenium", "2")]
        //dotnet test --filter "Selenium=2"
        public void GetAllPostsTest()
        {
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_baseUrl);
            Thread.Sleep(1000);

            // Find GetAllPosts button and click it
            var Getallpostsbutton = _driver.FindElement(By.Id("get-all-posts"));
            Getallpostsbutton.Click();
            Thread.Sleep(1000);

            // Find ClearPosts button and click it
            var ClearPostsButton = _driver.FindElement(By.Id("clear-posts"));
            ClearPostsButton.Click();
            Thread.Sleep(1000);

            _driver.Quit();
        }

        [Fact]
        [Trait("Selenium", "All")]
        [Trait("Selenium", "3")]
        //dotnet test --filter "Selenium=3"
        public void GetPostByIdTest()
        {
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_baseUrl);
            Thread.Sleep(1000);

            // Find GetPostById input field and button
            var FetchIdInput = _driver.FindElement(By.Name("number"));
            var GetPostIdButton = _driver.FindElement(By.Id("get-post-by-id"));
            FetchIdInput.Clear();
            Thread.Sleep(1000);
            // Fill input with information and submit
            FetchIdInput.SendKeys("1");
            Thread.Sleep(1000);
            GetPostIdButton.Click();
            Thread.Sleep(1000);

            // Assert that the correct post is created and displayed
            //var postHeader = _driver.FindElement(By.Id(""));
            var postHeader = _driver.FindElement(By.XPath("//div[@class='post']/h3[text()='Post #1']"));
            Assert.Equal("Post #1", postHeader.Text);
            Thread.Sleep(3000);

            _driver.Quit();
        }
    }
}