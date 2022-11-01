using System;
using System.Collections.Generic;
using System.Linq;
using Volvo.TaxCalculator.Core.Enums;
using Volvo.TaxCalculator.Core.Interfaces;
using Volvo.TaxCalculator.Core.Models;

public class CongestionTaxService : ICongestionTaxService
{
    // Can be loaded from the settings
    private readonly int _maxFeeAmount = 60;
    private readonly TimeSpan _timeInteval = TimeSpan.FromMinutes(60);

    // Can be loaded from the settings
    private readonly List<DateTime> _holidaysList = new List<DateTime>()
    {
        new DateTime(2013,01,01),
        new DateTime(2013,01,06),
        new DateTime(2013,03,29),
        new DateTime(2013,03,31),
        new DateTime(2013,04,01),
        new DateTime(2013,05,09),
        new DateTime(2013,05,19),
        new DateTime(2013,05,20),
        new DateTime(2013,06,06),
        new DateTime(2013,06,22),
        new DateTime(2013,11,01),
        new DateTime(2013,12,25),
        new DateTime(2013,12,26),
        new DateTime(2013,12,31)
    };
    // Can be loaded from the settings
    private readonly Dictionary<TimeSpan, int> _hourlyRates = new Dictionary<TimeSpan, int>()
    {
        {new TimeSpan(06,00,00), 8 },
        {new TimeSpan(06,30,00), 13 },
        {new TimeSpan(07,00,00), 18 },
        {new TimeSpan(08,00,00), 13 },
        {new TimeSpan(08,30,00), 8 },
        {new TimeSpan(15,00,00), 13 },
        {new TimeSpan(15,30,00), 18 },
        {new TimeSpan(17,00,00), 13 },
        {new TimeSpan(18,00,00), 8 },
        {new TimeSpan(18,30,00), 0 }
    };

    /// <summary>
    /// Calculate the total toll fee for one day
    /// </summary>
    /// <param name="vehicle">the vehicle</param>
    /// <param name="dates">date and time of all passes on one day</param>
    /// <returns>the total congestion tax for that day</returns>
    public int GetTax(DateTime[] dates, Vehicle vehicle)
    {
        // in case of specific upfront charges rules
        int totalFee = 0;

        // If the dates list is empty - return zero
        if (dates.Length == 0) { return totalFee; }
        // No need to make calculations if the vehicle is toll free
        if (IsTollFreeVehicle(vehicle)) { return totalFee; }      


        // We dont know in which order the dates array is
        var orderedDates = dates.OrderBy(p => p.Ticks).ToList();
        
        // Group entries in a specific time interval to comply with Single Charge Rule
        var groupedEntriesList = GroupDatesByTimeInterval(orderedDates, _timeInteval);
                        
        foreach (var entriesGroup in groupedEntriesList)
        {
            totalFee += CalculateFeeForGroup(entriesGroup);
        }

        // Comply with maximum fee amount rule
        return totalFee > _maxFeeAmount ? _maxFeeAmount : totalFee;
    }

    /// <summary>
    /// Group dates based on the time interval between them
    /// </summary>
    /// <param name="datesList">List of dates</param>
    /// <param name="timeInterval">Interval parameter</param>
    /// <returns>List of sublists with entries lying in the specific time inteval</returns>
    private List<List<DateTime>> GroupDatesByTimeInterval(List<DateTime> datesList, TimeSpan timeInterval)
    {
        var bufferDate = DateTime.MinValue;
        var bufferDateList = new List<DateTime>();
        var groupedEntriesList = new List<List<DateTime>>();

        foreach (var orderedDate in datesList)
        {
            // If entry is within the time interval - add it to the buffer list
            if (orderedDate - bufferDate < timeInterval)
            {
                bufferDateList.Add(orderedDate);
            }
            else
            {
                // If entry is not in the time interval - add the buffer list to results, clear buffer list, set entry as a new buffer date and add it to the buffer list                
                if (bufferDateList.Count > 0) groupedEntriesList.Add(new List<DateTime>(bufferDateList));
                bufferDateList.Clear();

                bufferDate = orderedDate;
                bufferDateList.Add(orderedDate);
            }
            // If it is the last entry - add the buffer list to results
            if (orderedDate == datesList.Last())
            {
                groupedEntriesList.Add(new List<DateTime>(bufferDateList));
            }
        }
        return groupedEntriesList;
    }

    /// <summary>
    /// Calculate highest fee for a group of dates to comply with single charge rule
    /// </summary>
    /// <param name="dates"></param>
    /// <returns></returns>
    private int CalculateFeeForGroup(IEnumerable<DateTime> dates)
    {
        var fee = 0;
        foreach (var date in dates)
        {
            var calculatedFee = GetTollFee(date);
            if (calculatedFee > fee) fee = calculatedFee;
        }
        return fee;
    }

    /// <summary>
    /// Verification of the tax rules
    /// </summary>
    /// <param name="date"></param>
    /// <param name="vehicle"></param>
    /// <returns>Fee amount</returns>
    private int GetTollFee(DateTime date)
    {
        if (IsTollFreeDate(date)) return 0;

        var timeInterval = date.TimeOfDay;
        // Hourly rates are defined in a separate dictionary
        var rateInterval = _hourlyRates
            .Where(p => timeInterval > p.Key);

        if(!rateInterval.Any())
        {
            return _hourlyRates.Last().Value;
        }
        return rateInterval.Last().Value;
    }

    /// <summary>
    /// Verifies the provided value against the dictionary
    /// </summary>
    /// <param name="vehicle">Vehicle param</param>
    /// <returns></returns>
    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicle.VehicleType);
    }

    /// <summary>
    /// Verifies the provided date against defined holidays list and tax rules
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
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
}