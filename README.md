# CloudQA Task 1 - Resilient Selenium Automation

## Overview

This project automates three fields from the CloudQA Automation Practice Form using C# and Selenium:

1. First Name text field
2. Female gender radio button
3. State dropdown

The main goal is to make the automation resilient to common webpage changes instead of depending only on fixed XPath or CSS selectors.

## Technologies

- C#
- .NET
- Selenium WebDriver
- Google Chrome
- Selenium Manager

## Project Structure

```text
CloudQA-Task1/
├── CloudQA-Task1.csproj
├── Program.cs
├── README.md
├── .gitignore
├── Models/
│   └── ElementCandidate.cs
├── Services/
│   ├── BrowserService.cs
│   └── ResilientElementFinder.cs
└── screenshots/
```

## How It Works

The automation does not depend only on fixed IDs such as `fname`, `female`, or `state`.

Instead, the `ResilientElementFinder`:

1. Searches candidate elements by their general HTML tag.
2. Inspects attributes such as ID, name, type and placeholder.
3. Checks nearby visible text.
4. Checks associated label or span text.
5. Normalizes text such as `First Name`, `first_name`, and `first-name`.
6. Assigns a score to each candidate.
7. Selects the highest-scoring candidate above a minimum confidence score.

This scoring approach helps the automation continue working when common attributes or the page structure change.

## Setup

Install:

- .NET SDK
- Google Chrome
- VS Code or another C# IDE

Clone the repository and install the required NuGet packages:

```bash
dotnet restore
```

## Run

Run using the default CloudQA URL:

```bash
dotnet run
```

A custom URL can also be supplied:

```bash
dotnet run -- "https://example.com/test-page"
```

## Test Flow

The automation:

1. Opens the configured URL.
2. Finds the First Name field.
3. Enters `Asiya`.
4. Verifies the entered value.
5. Finds and selects the Female radio button.
6. Finds the State dropdown.
7. Selects India.
8. Verifies the selected state.
9. Reports the result in the console.

## Failure Handling

If an exception occurs, the program attempts to save a screenshot inside the `screenshots` directory.

This helps with debugging when a modified page causes an element to become unrecognizable.

## Limitations

This is a heuristic-based resilient locator rather than a machine-learning model.

It can handle many common changes such as:

- Changed IDs
- Changed names
- Minor HTML structure changes
- Different text formatting
- Changed element positions
- Similar candidate elements using scoring

However, it cannot guarantee success when the page is completely redesigned, the target text is removed, or several elements have equally strong matching signals.

For a production framework, the scoring rules could be extended with additional signals, explicit configuration, accessibility attributes, DOM relationships, and historical locator information.

## Result

The current CloudQA Automation Practice Form was successfully tested for:

- First Name
- Female gender
- State = India

All three validations passed successfully.