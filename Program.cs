using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using CloudQA.Task1.Services;

// URL can be passed from the command line.
// If no URL is provided, the default CloudQA URL is used.
string url =
    args.Length > 0
        ? args[0]
        : "https://app.cloudqa.io/home/AutomationPracticeForm";

BrowserService browserService = new BrowserService();
IWebDriver driver = browserService.StartBrowser();

try
{
    Console.WriteLine("========================================");
    Console.WriteLine("CloudQA Task 1");
    Console.WriteLine("Resilient Selenium Automation");
    Console.WriteLine("========================================");

    Console.WriteLine();
    Console.WriteLine("Opening page...");
    Console.WriteLine("URL: " + url);

    driver.Navigate().GoToUrl(url);

    // Wait until the page is ready.
    WebDriverWait wait = new WebDriverWait(
        driver,
        TimeSpan.FromSeconds(20)
    );

    wait.Until(d =>
        ((IJavaScriptExecutor)d)
            .ExecuteScript("return document.readyState")
            ?.ToString() == "complete"
    );

    Console.WriteLine("Page loaded.");
    Console.WriteLine("Current URL: " + driver.Url);
    Console.WriteLine("Page is ready.");

    ResilientElementFinder finder =
        new ResilientElementFinder(driver);

    // ========================================
    // 1. FIRST NAME
    // ========================================

    Console.WriteLine();
    Console.WriteLine("[1/3] Finding First Name...");

    IWebElement firstName =
        finder.FindFirstName();

    wait.Until(d => firstName.Displayed);

    firstName.Clear();
    firstName.SendKeys("Asiya");

    string firstNameValue =
        firstName.GetAttribute("value") ?? "";

    Console.WriteLine(
        "Entered First Name: " + firstNameValue
    );

    if (firstNameValue.Equals(
        "Asiya",
        StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine(
            "✓ First Name verification passed."
        );
    }
    else
    {
        throw new Exception(
            "First Name verification failed."
        );
    }

    // ========================================
    // 2. GENDER
    // ========================================

    Console.WriteLine();
    Console.WriteLine(
        "[2/3] Finding Female gender option..."
    );

    IWebElement female =
        finder.FindFemaleRadio();

    wait.Until(d => female.Displayed);

    if (!female.Selected)
    {
        female.Click();
    }

    if (female.Selected)
    {
        Console.WriteLine(
            "✓ Female selected successfully."
        );
    }
    else
    {
        throw new Exception(
            "Female selection failed."
        );
    }

    // ========================================
    // 3. STATE
    // ========================================

    Console.WriteLine();
    Console.WriteLine(
        "[3/3] Finding State dropdown..."
    );

    IWebElement state =
        finder.FindStateDropdown();

    wait.Until(d => state.Displayed);

    SelectElement stateDropdown =
        new SelectElement(state);

    stateDropdown.SelectByText("India");

    string selectedState =
        stateDropdown.SelectedOption.Text.Trim();

    Console.WriteLine(
        "Selected State: " + selectedState
    );

    if (selectedState.Equals(
        "India",
        StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine(
            "✓ State verification passed."
        );
    }
    else
    {
        throw new Exception(
            "State verification failed."
        );
    }

    // ========================================
    // FINAL RESULT
    // ========================================

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("AUTOMATION COMPLETED SUCCESSFULLY");
    Console.WriteLine("========================================");

    Console.WriteLine();
    Console.WriteLine("Press ENTER to close the browser...");
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("TEST FAILED");
    Console.WriteLine("========================================");

    Console.WriteLine("Error: " + ex.Message);

    // Take screenshot when something fails.
    try
    {
        string screenshotDirectory =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "screenshots"
            );

        Directory.CreateDirectory(
            screenshotDirectory
        );

        string screenshotPath =
            Path.Combine(
                screenshotDirectory,
                $"failure_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            );

        Screenshot screenshot =
            ((ITakesScreenshot)driver)
                .GetScreenshot();

        screenshot.SaveAsFile(
            screenshotPath
        );

        Console.WriteLine(
            "Screenshot saved: " + screenshotPath
        );
    }
    catch
    {
        Console.WriteLine(
            "Could not save failure screenshot."
        );
    }

    Console.WriteLine();
    Console.WriteLine(
        "Press ENTER to close the browser..."
    );

    Console.ReadLine();
}
finally
{
    driver.Quit();
}