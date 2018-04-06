using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace EmployeeManagerTesting.PageObjects
{
    class MainPage
    {
        public MainPage(IWebDriver driver)
        {
            browser = driver;
        }

        private IWebDriver browser;
        private string url = "https://devmountain-qa.github.io/employee-manager-v2/build/index.html";
        public void MainPageNavigate()
        {
            browser.Navigate().GoToUrl(url);
            Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath(GetName("+ Add Employee"))));
        }

        public WebDriverWait Wait() { return new WebDriverWait(browser, TimeSpan.FromSeconds(3));}
        public string GetName(string name) { return "//ul/li[contains(.,\"" + name + "\")]"; }
        public IWebElement NameEntry() => browser.FindElement(By.CssSelector("input[name=\"nameEntry\"]"));
        public IWebElement PhoneEntry() => browser.FindElement(By.CssSelector("input[name=\"phoneEntry\"]"));
        public IWebElement EmailEntry() => browser.FindElement(By.CssSelector("input[name=\"emailEntry\"]"));
        public IWebElement TitleEntry() => browser.FindElement(By.CssSelector("input[name=\"titleEntry\"]"));
        public IWebElement SaveButton() => browser.FindElement(By.Id("saveBtn"));
        public IWebElement CancelButton() => browser.FindElement(By.CssSelector("button[name=\"cancel\"]"));
        public IWebElement DeleteButton() => browser.FindElement(By.CssSelector("button[name=\"delete\"]"));
        public IWebElement Title() => browser.FindElement(By.ClassName("titleText"));
        public IWebElement SearchBox() => browser.FindElement(By.CssSelector("input[name=\"searchBox\"]"));
        public IWebElement ErrorMessage() => browser.FindElement(By.ClassName("errorMessage"));
        public IWebElement NameLink(string name) => browser.FindElement(By.XPath(GetName(name))); 

        public void FillEntries(string name, string phone, string email, string title)
        {
            if (name != null)
            {
                NameEntry().Clear();
                NameEntry().SendKeys(name);
            }
            if (phone != null)
            {
                PhoneEntry().Clear();
                PhoneEntry().SendKeys(phone);
            }
            if (email != null)
            {
                EmailEntry().Clear();
                EmailEntry().SendKeys(email);
            }
            if (title != null)
            {
                TitleEntry().Clear();
                TitleEntry().SendKeys(title);
            }
        }

        public void CheckEntries(string name, string phone, string email, string title)
        {
            string nameValue = NameEntry().GetAttribute("value");
            string phoneValue = PhoneEntry().GetAttribute("value");
            string emailValue = EmailEntry().GetAttribute("value");
            string titleValue = TitleEntry().GetAttribute("value");

            Assert.IsTrue(nameValue.Contains(name), nameValue);
            Assert.IsTrue(phoneValue.Contains(phone), phoneValue);
            Assert.IsTrue(emailValue.Contains(email), emailValue);
            Assert.IsTrue(titleValue.Contains(title), titleValue);
        }

        public void CheckErrors(bool nameError, bool phoneError, bool emailError, bool titleError)
        {
            if(nameError == true)
            {
                string nameErrorText = "The name field must be between 1 and 30 characters long.";
                Assert.IsTrue(ErrorMessage().GetAttribute("innerText").Contains(nameErrorText), "does not contain error message");
            }
            if (phoneError == true)
            {
                string phoneErrorText = "The phone number must be 10 digits long.";
                Assert.IsTrue(ErrorMessage().GetAttribute("innerText").Contains(phoneErrorText), "does not contain error message");
            }
            if (emailError == true)
            {
                string emailErrorText = "The email field must be between 1 and 30 characters long.";
                Assert.IsTrue(ErrorMessage().GetAttribute("innerText").Contains(emailErrorText), "does not contain error message");
            }
            if (titleError == true)
            {
                string titleErrorText = "The title field must be between 1 and 30 characters long.";
                Assert.IsTrue(ErrorMessage().GetAttribute("innerText").Contains(titleErrorText), "does not contain error message");
            }
        }

        public void DeleteRecord(string name)
        {
            Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath(GetName(name))));
            NameLink(name).Click();
            DeleteButton().Click();
            IAlert alert = browser.SwitchTo().Alert();
            alert.Accept();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(browser.FindElements(By.XPath(GetName(name))).Count == 0, "Failed to delete");
        }

        public void CreateNewEmployee()
        {
            if (!browser.PageSource.Contains("New Employee"))
            {
                NameLink("+ Add Employee").Click();
                Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath(GetName("New Employee"))));
            }
            NameLink("New Employee").Click();
            CheckEntries("New Employee", "1111111111", "abc", "New Employee");
        }

        public void CreateEmployee(string name, string phone, string email, string title)
        {
            CreateNewEmployee();
            FillEntries(name, phone, email, title);
            SaveButton().Click();
        }
    }
}