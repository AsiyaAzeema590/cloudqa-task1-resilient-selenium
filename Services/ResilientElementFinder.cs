using OpenQA.Selenium;
using CloudQA.Task1.Models;

namespace CloudQA.Task1.Services;

public class ResilientElementFinder
{
    private readonly IWebDriver _driver;

    public ResilientElementFinder(IWebDriver driver)
    {
        _driver = driver;
    }

    public IWebElement FindFirstName()
    {
        return FindBestElement(
            "First Name",
            new[] { "input" },
            50
        );
    }

    public IWebElement FindFemaleRadio()
    {
        return FindBestElement(
            "Female",
            new[] { "input" },
            50
        );
    }

    public IWebElement FindStateDropdown()
    {
        return FindBestElement(
            "State",
            new[] { "select" },
            50
        );
    }

    private IWebElement FindBestElement(
        string target,
        string[] allowedTags,
        int minimumScore)
    {
        var candidates =
            new List<(IWebElement Element, ElementCandidate Info)>();

        string targetNormalized =
            NormalizeText(target);

        foreach (string tag in allowedTags)
        {
            var elements =
                _driver.FindElements(By.TagName(tag));

            foreach (var element in elements)
            {
                try
                {
                    if (!element.Displayed)
                        continue;

                    ElementCandidate candidate =
                        AnalyzeElement(
                            element,
                            target,
                            targetNormalized
                        );

                    if (candidate.Score >= minimumScore)
                    {
                        candidates.Add(
                            (element, candidate)
                        );
                    }
                }
                catch
                {
                    // Ignore elements that cannot be inspected.
                }
            }
        }

        if (candidates.Count == 0)
        {
            throw new NoSuchElementException(
                $"Could not find a reliable element for '{target}'."
            );
        }

        var best =
            candidates
                .OrderByDescending(x => x.Info.Score)
                .First();

        Console.WriteLine();
        Console.WriteLine($"Found '{target}'");
        Console.WriteLine($"Score: {best.Info.Score}");
        Console.WriteLine(best.Info);

        return best.Element;
    }

    private ElementCandidate AnalyzeElement(
        IWebElement element,
        string target,
        string targetNormalized)
    {
        string tagName =
            element.TagName;

        string id =
            GetAttribute(element, "id");

        string name =
            GetAttribute(element, "name");

        string type =
            GetAttribute(element, "type");

        string placeholder =
            GetAttribute(element, "placeholder");

        string text =
            element.Text;

        string nearbyText =
            GetNearbyText(element);

        int score = 0;

        // ----------------------------------------
        // Strong signal: associated text
        // ----------------------------------------

        if (HasAssociatedText(
            element,
            target,
            targetNormalized))
        {
            score += 60;
        }

        // ----------------------------------------
        // Nearby text
        // ----------------------------------------

        if (NormalizeText(nearbyText)
            .Contains(targetNormalized))
        {
            score += 30;
        }

        // ----------------------------------------
        // Placeholder
        // ----------------------------------------

        if (NormalizeText(placeholder)
            .Contains(targetNormalized))
        {
            score += 25;
        }

        // ----------------------------------------
        // Name
        // ----------------------------------------

        if (NormalizeText(name)
            .Contains(targetNormalized))
        {
            score += 20;
        }

        // ----------------------------------------
        // ID
        // ----------------------------------------

        if (NormalizeText(id)
            .Contains(targetNormalized))
        {
            score += 15;
        }

        // ----------------------------------------
        // Element type
        // ----------------------------------------

        if (target.Equals(
                "First Name",
                StringComparison.OrdinalIgnoreCase)
            &&
            type.Equals(
                "text",
                StringComparison.OrdinalIgnoreCase))
        {
            score += 10;
        }

        if (target.Equals(
                "Female",
                StringComparison.OrdinalIgnoreCase)
            &&
            type.Equals(
                "radio",
                StringComparison.OrdinalIgnoreCase))
        {
            score += 10;
        }

        if (target.Equals(
                "State",
                StringComparison.OrdinalIgnoreCase)
            &&
            tagName.Equals(
                "select",
                StringComparison.OrdinalIgnoreCase))
        {
            score += 10;
        }

        return new ElementCandidate
        {
            TagName = tagName,
            Id = id,
            Name = name,
            Type = type,
            Placeholder = placeholder,
            Text = text,
            NearbyText = nearbyText,
            Score = score
        };
    }

    private bool HasAssociatedText(
        IWebElement element,
        string target,
        string targetNormalized)
    {
        try
        {
            string elementId =
                GetAttribute(element, "id");

            // Check associated label/span only when
            // the element has an ID.
            if (!string.IsNullOrWhiteSpace(elementId))
            {
                var associatedElements =
                    _driver.FindElements(
                        By.CssSelector(
                            $"label[for='{elementId}'], " +
                            $"span[for='{elementId}'], " +
                            $"span[name='{elementId}']"
                        )
                    );

                foreach (var associatedElement
                         in associatedElements)
                {
                    string associatedText =
                        associatedElement.Text.Trim();

                    if (NormalizeText(associatedText)
                        .Contains(targetNormalized))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private string GetNearbyText(
        IWebElement element)
    {
        try
        {
            IWebElement parent =
                element.FindElement(
                    By.XPath("..")
                );

            return parent.Text ?? "";
        }
        catch
        {
            return "";
        }
    }

    private string GetAttribute(
        IWebElement element,
        string attribute)
    {
        try
        {
            return element.GetAttribute(
                attribute
            ) ?? "";
        }
        catch
        {
            return "";
        }
    }

    private string NormalizeText(
        string value)
    {
        return value
            .ToLowerInvariant()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("_", "")
            .Trim();
    }
}