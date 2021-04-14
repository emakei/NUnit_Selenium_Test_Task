using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace NUnit_Selenium_Test_Task
{
    [TestFixture]
    public class ChromeTest
    {
        private IWebDriver driver;
        public string homeURL;
        public bool loggedIn;

        public void Login()
        {
            driver.Navigate().GoToUrl(homeURL);

            var wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.XPath("//span[contains(text(),'Войти в аккаунт')]")));
            driver.FindElement(By.XPath("//span[contains(text(),'Войти в аккаунт')]")).Click();

            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("doe474255@gmail.com");
            driver.FindElement(By.XPath("//span[contains(text(),'Продолжить через электронную почту')]")).Click();

            wait.Until(driver => driver.FindElement(By.Id("password")));
            driver.FindElement(By.Id("password")).SendKeys("doeBooking2021");
            driver.FindElement(By.XPath("//span[contains(text(),'Войти')]")).Click();

            loggedIn = true;
        }

        public void GoLoPreferences()
        {
            Assert.AreEqual(loggedIn, true, "Not logged in.");

            var wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.Id("profile-menu-trigger--title")));
            driver.FindElement(By.Id("profile-menu-trigger--title")).Click();

            wait.Until(driver => driver.FindElement(By.XPath("//span[contains(text(),'Управлять аккаунтом')]")));
            driver.FindElement(By.XPath("//span[contains(text(),'Управлять аккаунтом')]")).Click();

            wait.Until(driver => driver.FindElement(By.XPath("//div[contains(text(),'Manage preferences')]")));
            driver.FindElement(By.XPath("//div[contains(text(),'Manage preferences')]")).Click();
        }

        [Test(Description = "Change currency")]
        public void Change_currency()
        {
            Login();

            GoLoPreferences();

            var wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));

            wait.Until(driver => driver.FindElement(By.XPath("//button[@data-ga-label='Edit section: currency']")));
            driver.FindElement(By.XPath("//button[@data-ga-label='Edit section: currency']")).Click();

            wait.Until(driver => driver.FindElement(By.ClassName("bui-dropdown")));
            driver.FindElement(By.XPath("//div[@class='bui-dropdown']/button")).Click();

            wait.Until(driver => driver.FindElement(By.ClassName("bui-dropdown-menu__items")));
            driver.FindElement(By.XPath("//ul[@class='bui-dropdown-menu__items']/li[5]/button")).Click();
            
            wait.Until(driver => driver.FindElement(By.XPath("//button[@data-ga-label='Save section: currency']")));
            driver.FindElement(By.XPath("//button[@data-ga-label='Save section: currency']")).Click();

            // Currency now should be 'BGN'
            wait.Until(driver => driver.FindElement(By.XPath("//span[contains(text(),'BGN')]")));
        }

        [Test(Description = "Change language")]
        public void Change_language()
        {
            Login();

            GoLoPreferences();

            var wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));

            wait.Until(driver => driver.FindElement(By.XPath("//button[@data-ga-label='Edit section: language']")));
            driver.FindElement(By.XPath("//button[@data-ga-label='Edit section: language']")).Click();

            wait.Until(driver => driver.FindElement(By.CssSelector("button.bui-button.my-settings-dropdown-button.bui-button--secondary")));
            //fixme: element not interactable
            driver.FindElement(By.CssSelector("button.bui-button.my-settings-dropdown-button.bui-button--secondary")).Click();

            wait.Until(driver => driver.FindElement(By.ClassName("bui-dropdown-menu__items")));
            driver.FindElement(By.XPath("//ul[@class='bui-dropdown-menu__items']/li[6]/button")).Click();

            wait.Until(driver => driver.FindElement(By.XPath("//button[@data-ga-label='Save section: language']")));
            driver.FindElement(By.XPath("//button[@data-ga-label='Save section: language']")).Click();

            // Language now should be 'Filipino'
            wait.Until(driver => driver.FindElement(By.XPath("//span[@class='preferences-language-text' and text()='Filipino']")));
        }

        [Test(Description = "Go to tickets")]
        public void Go_ToTickets()
        {
            driver.Navigate().GoToUrl(homeURL);

            var wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.XPath("//a[@data-decider-header='flights']")));
            driver.FindElement(By.XPath("//a[@data-decider-header='flights']")).Click();

            //fixme: unable to find element. Try something better
            wait.Until(driver => driver.FindElement(By.XPath("//*[text()='Ищите авиабилеты на сотнях сайтов в один клик'")));
        }

        [Test(Description = "Check filter")]
        public void Check_filter()
        {
            driver.Navigate().GoToUrl(homeURL);

            var wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.Id("ss")));
            driver.FindElement(By.Id("ss")).SendKeys("Могилев");

            wait.Until(driver => driver.FindElement(By.XPath("//li[@data-label='Могилев, Могилёвская область, Беларусь']")));
            driver.FindElement(By.XPath("//li[@data-label='Могилев, Могилёвская область, Беларусь']")).Click();

            wait.Until(driver => driver.FindElement(By.ClassName("bui-calendar")));
            var outcomeDate = System.DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
            var incomeDate = System.DateTime.Today.AddDays(9).ToString("yyyy-MM-dd");

            wait.Until(driver => driver.FindElement(By.CssSelector("[data-date='" + outcomeDate + "'")));
            driver.FindElement(By.CssSelector("[data-date='" + outcomeDate + "'")).Click();

            wait.Until(driver => driver.FindElement(By.CssSelector("[data-date='" + incomeDate + "'")));
            driver.FindElement(By.CssSelector("[data-date='" + incomeDate + "'")).Click();

            wait.Until(driver => driver.FindElement(By.Id("xp__guests__toggle")));
            driver.FindElement(By.Id("xp__guests__toggle")).Click();

            // Check adults count
            wait.Until(driver => driver.FindElement(By.XPath("//div[@class='sb-group__field sb-group__field-adults']/div/div/span[@data-bui-ref='input-stepper-value']")));
            int adultsCount = int.Parse(driver.FindElement(By.XPath("//div[@class='sb-group__field sb-group__field-adults']/div/div/span[@data-bui-ref='input-stepper-value']")).Text);

            while(adultsCount > 2)
            {
                wait.Until(driver => driver.FindElement(By.XPath("//div[@class='sb-group__field sb-group__field-adults']/div/div/button[@data-bui-ref='input-stepper-subtract-button']")));
                driver.FindElement(By.XPath("//div[@class='sb-group__field sb-group__field-adults']/div/div/button[@data-bui-ref='input-stepper-subtract-button']")).Click();
                adultsCount--;
            }

            while (adultsCount < 2)
            {
                wait.Until(driver => driver.FindElement(By.XPath("//div[@class='sb-group__field sb-group__field-adults']/div/div/button[@data-bui-ref='input-stepper-add-button']")));
                driver.FindElement(By.XPath("//div[@class='sb-group__field sb-group__field-adults']/div/div/button[@data-bui-ref='input-stepper-add-button']")).Click();
                adultsCount++;
            }

            // Check children count
            wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group-children')]/div/div/span[@data-bui-ref='input-stepper-value']")));
            int childrenCount = int.Parse(driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group-children')]/div/div/span[@data-bui-ref='input-stepper-value']")).Text);

            while (childrenCount > 1)
            {
                wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group-children')]/div/div/button[@data-bui-ref='input-stepper-subtract-button']")));
                driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group-children')]/div/div/button[@data-bui-ref='input-stepper-subtract-button']")).Click();
                adultsCount--;
            }

            while (childrenCount < 1)
            {
                wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group__field-rooms')]/div/div/button[@data-bui-ref='input-stepper-add-button']")));
                driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group__field-rooms')]/div/div/button[@data-bui-ref='input-stepper-add-button']")).Click();
                childrenCount++;
            }

            // Check rooms count
            wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group__field-rooms')]/div/div/span[@data-bui-ref='input-stepper-value']")));
            int roomsCount = int.Parse(driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group__field-rooms')]/div/div/span[@data-bui-ref='input-stepper-value']")).Text);

            while (roomsCount > 1)
            {
                wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group__field-rooms')]/div/div/button[@data-bui-ref='input-stepper-subtract-button']")));
                driver.FindElement(By.XPath("//div[contains(@class,'sb-group__field') and contains(@class,'sb-group__field-rooms')]/div/div/button[@data-bui-ref='input-stepper-subtract-button']")).Click();
                roomsCount--;
            }

            // Submit button
            wait.Until(driver => driver.FindElement(By.ClassName("sb-searchbox__button")));
            driver.FindElement(By.ClassName("sb-searchbox__button")).Click();

            wait.Until(driver => driver.FindElement(By.XPath("//*[contains(text(),'Результаты поиска')]")));
        }

        [Test(Description = "Check login")]
        public void Check_login()
        {
            Login();
        }

        [TearDown]
        public void TearDownTest()
        {
            driver.Close();
            loggedIn = false;
        }

        [SetUp]
        public void SetupTest()
        {
            homeURL = "http://booking.com";
            driver = new ChromeDriver("/Users/eugenemakei/.local/bin");
            loggedIn = false;
        }
    }
    public class LocationResult
    {
        public string title { get; set; }
        public string location_type { get; set; }
        public int woeid { get; set; }
        public string latt_long { get; set; }
        public float getLatt() => int.Parse(latt_long.Split(',')[0]);
        public float getLong() => int.Parse(latt_long.Split(',')[1]);
    }

    [TestFixture]
    public class MetaWheaterTest
    {
        public string url;
        private HttpClient client;
        public string searchString;

        [Test(Description = "Testing location query")]
        public async Task GetLocationQueryResultsAsync()
        {
            var response = await client.GetAsync($"{url}/location/search/?query=/{searchString}");

            LocationResult[] arrayResutls = null;
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                arrayResutls = JsonSerializer.Deserialize<LocationResult[]>(json);
            }

            Assert.IsNotNull(arrayResutls, "Got nothing");

            var minskQuery = 
                from result in arrayResutls
                where result.title == "Minsk"
                select result;
            
            Assert.IsNotEmpty(minskQuery, "Result for 'Minsk' not found");

            // Check longitude, latitude
            var firstElement = minskQuery.First();
            // longitude
            Assert.GreaterOrEqual(firstElement.getLong(), 27.37, "Longitude less than real");
            Assert.LessOrEqual(firstElement.getLong(), 27.81, "Longitude greater than real");
            // latitude
            Assert.GreaterOrEqual(firstElement.getLatt(), 53.97, "Lattitude less than real");
            Assert.LessOrEqual(firstElement.getLatt(), 53.79, "Lattitude greater than real");

            // Get actual weather for today by woeid
            var weatherResponse = await client.GetAsync($"{url}/location/{firstElement.woeid}/{DateTime.Today.Year}/{DateTime.Today.Month}/{DateTime.Today.Day}/");
        }

        [TearDown]
        public void TearDownTest()
        {
            
        }

        [SetUp]
        public void SetupTest()
        {
            url = "https://www.metaweather.com/api";
            client = new HttpClient();
            searchString = Environment.GetEnvironmentVariable("TestSearchString");
            if(searchString == null)
            {
                searchString = "min";
            }

            Assert.IsNotEmpty(searchString, "Search string is empty");
        }
    }
}