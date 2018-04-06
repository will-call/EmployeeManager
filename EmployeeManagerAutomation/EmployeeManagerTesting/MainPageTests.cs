using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using EmployeeManagerTesting.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace EmployeeManagerTesting
{
    [TestClass]
    public class MainPageTests
    {
        private ChromeDriver chromeBrowser = new ChromeDriver();
        public string testName = "Thanos";
        public string testPhone = "0122333445";
        public string testEmail = "gems@infinity.com";
        public string testTitle = "Lover of Death";
        public string newEmployee = "New Employee";

        [TestMethod]
        public void OpenPage()
        {
            var mainPage = new MainPage(chromeBrowser);
            mainPage.MainPageNavigate();
            Assert.IsTrue(mainPage.Title().Text.Contains("Employee Manager"), "page not loaded?");
            chromeBrowser.Close();
        }

        [TestMethod]
        public void TestNewEmployee()
        {
            var mainPage = new MainPage(chromeBrowser);
            mainPage.MainPageNavigate();
            mainPage.CreateNewEmployee();
            chromeBrowser.Close();
        }

        [TestMethod]
        public void TestEmployeeNoCharacters()
        {
            var mainPage = new MainPage(chromeBrowser);
            mainPage.MainPageNavigate();
            mainPage.CreateEmployee(testName, testPhone, testEmail, testTitle);
            mainPage.Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath(mainPage.GetName(testName))));
            chromeBrowser.Navigate().Refresh();
            mainPage.Wait().Until(ExpectedConditions.ElementIsVisible(By.XPath(mainPage.GetName(testName))));
            mainPage.NameLink(testName).Click();
            mainPage.FillEntries(" " + Keys.Backspace, " " + Keys.Backspace, " " + Keys.Backspace, " " + Keys.Backspace);
            mainPage.SaveButton().Click();
            mainPage.CancelButton().Click();
            mainPage.CheckEntries(testName, testPhone, testEmail, testTitle);
            mainPage.DeleteRecord(testName);
            chromeBrowser.Close();
        }

        [TestMethod]
        public void TestEmployeeValidEntry()
        {
            var mainPage = new MainPage(chromeBrowser);
            mainPage.MainPageNavigate();
            mainPage.CreateEmployee(testName, testPhone, testEmail, testTitle);
            mainPage.CheckEntries(testName, testPhone, testEmail, testTitle);
            mainPage.DeleteRecord(testName);
            chromeBrowser.Close();
        }

        [TestMethod]
        public void TestErrorMessages()
        {
            var mainPage = new MainPage(chromeBrowser);
            mainPage.MainPageNavigate();
            mainPage.CreateNewEmployee();
            mainPage.NameLink(newEmployee).Click();
            mainPage.FillEntries(" " + Keys.Backspace, " " + Keys.Backspace, " " + Keys.Backspace, " " + Keys.Backspace);
            mainPage.SaveButton().Click();
            mainPage.CheckErrors(true, true, false, true);
            mainPage.CancelButton().Click();
            mainPage.DeleteRecord(newEmployee);
            chromeBrowser.Close();
        }

        [TestMethod]
        public void TestSearchBox()
        {
            var mainPage = new MainPage(chromeBrowser);
            mainPage.MainPageNavigate();
            mainPage.CreateEmployee(testName, testPhone, testEmail, testTitle);
            mainPage.SearchBox().SendKeys("Lover");
            Assert.IsTrue(mainPage.NameLink(testName).Displayed, "Record not displayed");
            mainPage.DeleteRecord(testName);
            chromeBrowser.Close();
        }

        [TestMethod]
        public void TestDeleteRecord()
        {
            var mainPage = new MainPage(chromeBrowser);
            mainPage.MainPageNavigate();
            mainPage.CreateNewEmployee();
            mainPage.DeleteRecord(newEmployee);
            chromeBrowser.Close();
        }
    }
}
