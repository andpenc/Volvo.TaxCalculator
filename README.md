# Congestion Tax Calculator assignment

Below you can find a short description on what was done and what should have been done if the timeframe was bigger.
It also contains the "Bonus" part proposal on what can be added.

## Assumptions

- The expected input is a JSON body with an array of dates and a vehicle type
- All the data in request belongs to the one vehicle

## Process

I have used .net core "congestion-tax-calculator" as a base, started with adding a simple NUnit tests project with "list of dates scribbled on a post-it" as a test data. 
In the scenario it is mentioned that the code might contain logical bugs, so that was the top priority to verify and fix. 
Some time was spent on removing the dummy code provided in the base project, cleaning up the enums, splitting logic into several methods inside the "tax calculator" class. 
I have added tax rates and holidays as a private objects in order to reduce the amount of hardcoded values inside the methods and to be able to load it later (wasn't implemented) from the settings. 
The part with the all tax calculation logic and necessary models became the Core project.
Then the Functions and DTO projects were added, I went with the HTTP trigger as a base, addeed DI for the tax calculation service. 
In the end - the postman collection was added (in the test folder) in order to have an easy way to manually test the function in case it was necessary.

## Possible improvements

Due to a restricted time amount I have only implemented basic test with test cases for the tax calculation logic. So a couple of improvements can be made there. 
1) Full unit tests coverage on Core and Functions projects. 
2) API tests can be added. Also automated testing through CI/CD pipeline and possble code analysis tools, coverage reports.
3) As a prerequisite to the previous two points (also bonus scenario) - move the tax rates, max fee amount, holidays list and other parameters to a json file that can be loaded as a settings from (for example) blob storage and then injected in the tax calculation service.
4) Request validation logic should be added to the endpoint, also additional other types of endpoints rather than HTTP trigger only.
5) Models classes for the vehicles can be improved and extended if necessary.
6) More logging added where necessary.

## Bonus Scenario

If we think of a possible changes as part of the bonus scenario:
```
    
    private readonly int _maxFeeAmount = 60;
    private readonly TimeSpan _timeInteval = TimeSpan.FromMinutes(60);

    
    private readonly List<DateTime> _holidaysList = new List<DateTime>()
    {
        new DateTime(2013,01,01)
        ...
    };
    
    private readonly Dictionary<TimeSpan, int> _hourlyRates = new Dictionary<TimeSpan, int>()
    {
        {new TimeSpan(06,00,00), 8 }
        ...
    };
```

This section can be moved to a settings class and be stored in a json file to support multiple cities\different tax rules. Settings then can be injected throught the DI to CongestionTaxService.

```
    private bool IsTollFreeDate(DateTime date)
    {
        // Tax free weekends
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        // Assignment scope is 2013 only
        if (date.Year == 2013)
        {
            // July is toll free
            if(date.Month == 7){ return true; }            
            
            // If date is a holiday or a day before a holiday
            if(_holidaysList.Contains(date.Date) || _holidaysList.Contains(date.AddDays(1).Date)){
                return true;
            }
        }
        return false;
    }
```
In this method we can also parameterize the "amount of days before a holiday", "toll free months", "toll free days of the week"

## Notes

I was hesitant to add Swagger description to the Azure function, because it is done through opensource package which is not actively maintaned and can be a possible security vulnerability going forward.