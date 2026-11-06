using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CloudQA.Task1.Services;

public class BrowserService
{
    public IWebDriver StartBrowser()
    {
        ChromeOptions options =
            new ChromeOptions();

        options.AddArgument(
            "--start-maximized"
        );

        options.PageLoadStrategy =
            PageLoadStrategy.Normal;

        return new ChromeDriver(options);
    }
}