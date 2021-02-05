using System;
using UnityEngine;

public class Planet
{
	public Planet(string name, double averageDistanceToSun, double diameter, double spinTimeEarthDays, double yearOrbitTimeInEarthDays, double gravityRelativeToEarth, double averageLowTemperature, double averageHighTemperature, string atmosphericContents, int numberOfMoons)
	{
		NumberOfMoons = numberOfMoons;
		AtmosphericContents = atmosphericContents;
		AverageHighTemperatureCelsius = averageHighTemperature;
		AverageLowTemperatureCelsius = averageLowTemperature;
		Gravity = gravityRelativeToEarth * 9.8;  // m/s
		YearOrbitTimeInEarthDays = yearOrbitTimeInEarthDays;
		DaySpinTimeHours = spinTimeEarthDays * TimeSpan.FromMinutes(23 * 60 + 56).TotalHours;
		AverageDistanceToSunKm = averageDistanceToSun;
		Name = name;
		DiameterKm = diameter;
	}

	public double DiameterKm { get; }
	public float DiameterUu
	{
		get
		{
			return (float)DiameterKm / 1000f;
		}
	}

	public float RadiusUu
	{
		get
		{
			return DiameterUu / 2f;
		}
	}

	public string Name { get; set; }
	public double AverageDistanceToSunKm { get; set; }
	public float AverageDistanceToSunUu
	{
		get
		{
			return (float)AverageDistanceToSunKm / 1000f;
		}
	}
	
	public double DaySpinTimeHours { get; set; }
	public double YearOrbitTimeInEarthDays { get; set; }
	public double Gravity { get; set; }
	public double AverageLowTemperatureCelsius { get; set; }
	public double AverageHighTemperatureCelsius { get; set; }
	public string AtmosphericContents { get; set; }
	public int NumberOfMoons { get; set; }
	public GameObject GameObject { get; set; }
	public bool IsInRetrograde { get; set; }
}
