using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
	public Transform Sun;
	float nextPlanetPosition;  // Anyone can change this at any time. 
	private const double oneEarthYearInDays = 365.25;
	private const float PlanetOffset = 100;
	public GameObject PlanetTextures;
	List<Planet> allPlanets = new List<Planet>();
	public GameObject TextPrototype;

	Planet mercury = new Planet("Mercury", 57900000, 4878, 59, 88, 0.38, -183, 427, "Sodium, Helium", 0);
	Planet venus = new Planet("Venus", 108160000, 12104, 243, 224, 0.9, 480, 480, "Carbon Dioxide (96%), Nitrogen (3.5%)", 0);
	Planet earth = new Planet("Earth", 149600000, 12756, 1, 365.25, 1, 14, 14, "Nitrogen(77%), Oxygen(21%)", 1);
	Planet mars = new Planet("Mars", 227936640, 6794, GetEarthDays(0, 24, 37), 687, 0.38, -63, -63, "Carbon Dioxide(95.3%), Argon", 2);
	Planet jupiter = new Planet("Jupiter", 778369000, 142984, GetEarthDays(0, 9, 55), 11.86 * oneEarthYearInDays, 2.64, -130, -130, "Hydrogen, Helium", 79);
	Planet saturn = new Planet("Saturn", 1427034000, 120536, GetEarthDays(0, 10, 39), 29 * oneEarthYearInDays, 1.16, -130, -130, "Hydrogen, Helium", 82);
	Planet uranus = new Planet("Uranus", 2870658186, 51118, GetEarthDays(0, 17, 14), 84 * oneEarthYearInDays, 1.11, -200, -200, "Hydrogen, Helium, Methane", 27);
	Planet neptune = new Planet("Neptune", 4496976000, 49532, GetEarthDays(0, 16, 7), 164.8 * oneEarthYearInDays, 1.21, -200, -200, "Hydrogen, Helium, Methane", 14);
	Planet zombie = new Planet("Zombie Planet", 949697600000, 56972340, GetEarthDays(0, 46, 7), 164.8 * oneEarthYearInDays, 1.21, -200, -200, "Zombies!", 69);
	
	static double GetEarthDays(int days, int hours, int minutes)
	{
		return new TimeSpan(days, hours, minutes, 0).TotalDays;
	}

	// Start is called before the first frame update
	void Start()
	{
		if (Sun == null)
			Debug.LogError($"{nameof(Sun)} is not assigned!!!");

		if (PlanetTextures == null)
			Debug.LogError($"{nameof(PlanetTextures)} is not assigned!!!");

		nextPlanetPosition = Sun.localScale.z / 2 + PlanetOffset;
		CreatePlanets();
	}


	Material GetPlanetMaterial(string planetName)
	{
		string lowerPlanetName = planetName.ToLower();
		Transform[] transforms = PlanetTextures.GetComponentsInChildren<Transform>();
		
		foreach (Transform transform in transforms)
			if (transform.gameObject.name.ToLower() == lowerPlanetName)
				return transform.gameObject.GetComponent<MeshRenderer>().material;
		return null;
	}

	void ModelPlanet(Planet planet)
	{
		GameObject planetObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		planet.GameObject = planetObject;
		/* Actual position led to problems in Unity - complaints about floating point accuracy. */
		//planetObject.transform.localPosition = new Vector3 (0, 0, planet.AverageDistanceToSunUu);
		planetObject.transform.localScale = new Vector3(planet.DiameterUu, planet.DiameterUu, planet.DiameterUu);
		nextPlanetPosition += planet.DiameterUu / 2;  // So we place the new planet at the edge of the previous value for nextPlanetPosition
		planetObject.transform.localPosition = new Vector3(0, 0, nextPlanetPosition);
		planetObject.name = planet.Name;

		AddPlanetLabel(0, planet.RadiusUu, nextPlanetPosition, planet.Name);

		nextPlanetPosition += planet.DiameterUu / 2 + PlanetOffset;
		MeshRenderer meshRenderer = planetObject.GetComponent<MeshRenderer>();
		if (meshRenderer == null)
		{
			Debug.LogError("There's no MeshRenderer on the primitive sphere we created.");
			return;
		}
		meshRenderer.material = GetPlanetMaterial(planet.Name);
	}

	void CreatePlanets()
	{
		allPlanets.Add(jupiter);
		allPlanets.Add(saturn);
		allPlanets.Add(earth);
		allPlanets.Add(venus);
		allPlanets.Add(mercury);
		allPlanets.Add(neptune);
		allPlanets.Add(uranus);
		allPlanets.Add(mars);
		allPlanets.Add(zombie);

		// Sort...
		allPlanets = allPlanets.OrderBy(x => x.AverageDistanceToSunKm).ToList();

		foreach (Planet planet in allPlanets)
			ModelPlanet(planet);
	}

	void AddPlanetLabel(int x, float planetRadius, float planetPositionZ, string name)
	{
		Vector3 position = new Vector3(x, planetRadius, planetPositionZ);
		GameObject planetText = Instantiate(TextPrototype, position, Quaternion.identity);
		MeshRenderer meshRenderer = planetText.GetComponent<MeshRenderer>();
		TextMesh textMesh = planetText.GetComponent<TextMesh>();
		meshRenderer.enabled = true;
		textMesh.text = name;
	}

	void FixedUpdate()
	{
		const float timeAccelerator = 1000f;
		float timeSecondsSinceGameStart = Time.time;
		const float secondsPerMinute = 60f;
		float timeMinutesSinceGameStart = timeSecondsSinceGameStart / secondsPerMinute;
		const float minutesPerHour = 60f;
		float timeHoursSinceGameStart = timeMinutesSinceGameStart / minutesPerHour;
		float acceleratedHoursSinceGameStart = timeHoursSinceGameStart * timeAccelerator;

		foreach (Planet planet in allPlanets)
		{
			float fractionOfASpin = (float)((acceleratedHoursSinceGameStart % planet.DaySpinTimeHours) / planet.DaySpinTimeHours);
			float degreesRotation = fractionOfASpin * 360;
			//planet.GameObject.transform.rotation = new Quaternion(0, 1, 0, degreesRotation);
			planet.GameObject.transform.rotation = Quaternion.AngleAxis(degreesRotation, Vector3.up); 
			//planet.DaySpinTimeHours
		}
	}
}
