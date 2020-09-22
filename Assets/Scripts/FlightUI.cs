using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlightUI : MonoBehaviour
{
    // Configuration parameters
    public TMP_Text currentElevationText;
    public TMP_Text verticalToTargetText;
    public TMP_Text horizontalToTargetText;
    public Slider fuelGaugeSlider;
    public TMP_Text currentLivesRemaining;

    float rocketHeighOffset = 4.2f;
    float maxRocketFuel;

    // Cached component references
    RocketShip myPlayerShip;
    LandingPad myLandingPad;

    private void Start()
    {
        myPlayerShip = FindObjectOfType<RocketShip>();
        myLandingPad = FindObjectOfType<LandingPad>();
        maxRocketFuel = FindObjectOfType<RocketShip>().GetMaxFuelLevel();
        fuelGaugeSlider = GetComponentInChildren<Slider>();
    }
    void Update()
    {
        DisplayVertical();
        DisplayVerticalToTarget();
        DisplayHorizontalToTarget();
        DisplaySliderFuelLevel();
        DisplayLivesRemaining();
    }

    private void DisplayVertical()
    {
        float elevation = myPlayerShip.transform.position.y - rocketHeighOffset;
        currentElevationText.text = Mathf.Round(elevation).ToString();
    }

    private void DisplayVerticalToTarget()
    {
        float verticalDistance = myPlayerShip.transform.position.y - myLandingPad.transform.position.y;
        verticalToTargetText.text = Mathf.Round(verticalDistance).ToString();
    }

    private void DisplayHorizontalToTarget()
    {
        float horizontalDistance = myPlayerShip.transform.position.x - myLandingPad.transform.position.x;
        horizontalToTargetText.text = Mathf.Round(horizontalDistance).ToString();
    }

    public void DisplaySliderFuelLevel()
    {
        float fuelPercentage = myPlayerShip.GetCurrentFuelLevel() / maxRocketFuel;
        fuelGaugeSlider.value = fuelPercentage;
    }

    public void DisplayLivesRemaining()
    {
        currentLivesRemaining.text = GameManager.Instance.GetCurrentLives().ToString();
    }
}
