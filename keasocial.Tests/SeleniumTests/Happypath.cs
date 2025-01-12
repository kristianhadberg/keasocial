using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using SeleniumExtras.WaitHelpers;

namespace keasocial.Tests.SeleniumTests
{
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
        public void Test()
        {
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_baseUrl);
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
            alert.Accept();

            Thread.Sleep(1000);
            inputField.Clear();
            Thread.Sleep(1000);
            // Find GetAllPosts button and click it
            var Getallpostsbutton = _driver.FindElement(By.Id("get-all-posts"));
            Getallpostsbutton.Click();
            Thread.Sleep(1000);

            // Find ClearPosts button and click it
            var ClearPostsButton = _driver.FindElement(By.Id("clear-posts"));
            ClearPostsButton.Click();
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

            // Assert that the correct post is created and displayed
            var postHeader = _driver.FindElement(By.XPath("//div[@class='post']/h3[text()='Post #1']"));
            Assert.Equal("Post #1", postHeader.Text);
            Thread.Sleep(2000);

            _driver.Quit();
        }
    }
}