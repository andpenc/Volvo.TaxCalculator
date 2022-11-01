# Congestion Tax Calculator assignment

Below you can find a short description on what was done and what should have been done if the timeframe was bigger.
It also contains the "Bonus" part proposal on what can be added.

## Process

I have used .net core "congestion-tax-calculator" as a base, started with adding a simple NUnit tests project with "list of dates scribbled on a post-it" as a test data. 
In the scenario it is mentioned that the code might contain logical bugs, so that was the top priority to verify and fix. 
Some time was spent on removing the dummy code provided in the base project, cleaning up the enums, splitting logic into several methods inside the "tax calculator" class. 
I have added tax rates and holidays as a private objects in order to reduce the amount of hardcoded values inside the methods and to be able to load it later (wasn't implemented) from the settings. 
The part with the all tax calculation logic and necessary models became the Core project.
Then the Functions and DTO projects were added, I went with the HTTP trigger as a base, addeed DI for the tax calculation service. 
In the end - the postman collection was added (in the test folder) in order to have an easy way to manually test the function in case it was necessary.

## Possible improvements / bonus part ideas

Due to a restricted time amount I have only implemented basic test with test cases for the tax calculation logic. So a couple of improvements can be made there. 
1) Full unit tests coverage on Core and Functions projects. 
2) API tests can be added.
3) As a prerequisite to the previous two points (also bonus scenario) - move the tax rates, max fee amount, holidays list to a json file that can be loaded as a settings from (for example) blob storage and then injected in the tax calculation service.
4) Request validation logic should be added to the endpoint, also additional other types of endpoints rather than HTTP trigger only.
5) Models classes for the vehicles can be improved and extended if necessary.
6) More logging added where necessary.