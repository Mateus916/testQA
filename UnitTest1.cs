using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace TestAutomation
{
    [Binding]
    public class CorreiosCepTests
    {
        private IWebDriver _driver;

        [BeforeScenario]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [AfterScenario]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Given(@"Eu estou na página inicial dos Correios")]
        public void DadoEuEstouNaPaginaInicialDosCorreios()
        {
            _driver.Navigate().GoToUrl("https://www.correios.com.br/");
        }

        [When(@"Eu procuro pelo CEP (.*)")]
        public void QuandoEuProcuroPeloCEP(string cep)
        {
            var searchBox = _driver.FindElement(By.Id("relaxation"));
            searchBox.Clear();
            searchBox.SendKeys(cep);
            searchBox.SendKeys(Keys.Enter);
        }

        [Then(@"Eu confirmo que o CEP não existe")]
        public void EntaoEuConfirmoQueOCepNaoExiste()
        {
            var result = _driver.FindElement(By.CssSelector(".resultado"));
            Assert.IsTrue(result.Text.Contains("não encontrado"));
        }

        [Then(@"Eu confirmo que o resultado seja '(.*)'")]
        public void EntaoEuConfirmoQueOResultadoSeja(string expectedResult)
        {
            var result = _driver.FindElement(By.CssSelector(".resultado"));
            Assert.IsTrue(result.Text.Contains(expectedResult));
        }

        [When(@"Eu procuro pelo rastreamento de código '(.*)'")]
        public void QuandoEuProcuroPeloRastreamentoDeCodigo(string codigo)
        {
            _driver.FindElement(By.LinkText("Rastreamento")).Click();
            var trackingBox = _driver.FindElement(By.XPath("//input[@id='objeto']"));
            trackingBox.SendKeys(codigo);
            _driver.FindElement(By.XPath("//button[@type='submit']")).Click();
        }

        [Then(@"Eu confirmo que o código não está correto")]
        public void EntaoEuConfirmoQueOCodigoNaoEstaCorreto()
        {
            var result = _driver.FindElement(By.XPath("//p[contains(text(), 'código inválido')]"));
            Assert.IsTrue(result.Displayed);
        }
    }
}
