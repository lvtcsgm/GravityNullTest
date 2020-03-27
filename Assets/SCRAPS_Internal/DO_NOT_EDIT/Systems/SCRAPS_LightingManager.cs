using UnityEngine;
using Funly.SkyStudio;

public class SCRAPS_LightingManager : MonoBehaviour {

    //public bool allowTimeToMove = true;

    public enum lType
    {
        indoors = 0,
        outdoors = 1
    }

    public static lType currentLighting = lType.outdoors;

    //new colors and settings for time of day
    [Header("Indoors")]
    public Color lm_indoorColor;
    public float lm_indoorInt;
    public float lm_indoorRef;
    public float lm_indoorFog;

    [Header("Outdoors")]
    public Color lm_outdoorColor;
    public float lm_outdoorInt;
    public float lm_outdoorRef;
    public float lm_outdoorFog;

    [Header("Other")]
    public float adjustSpeed = 0.15f;

    public TimeOfDayController tod_cont;
    private float last_tod;

    //public Light ambientLight;

    void Update()
    {
        if (currentLighting == lType.indoors)
        {
            if (last_tod != tod_cont.skyTime)
            {
                last_tod = tod_cont.skyTime;
                tod_cont.enabled = false;
            }

            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, lm_indoorInt, adjustSpeed * Time.deltaTime);
            RenderSettings.reflectionIntensity = Mathf.Lerp(RenderSettings.reflectionIntensity, lm_indoorRef, adjustSpeed * Time.deltaTime);

            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, lm_indoorFog, adjustSpeed * Time.deltaTime);

            //ambientLight.intensity = Mathf.Lerp(ambientLight.intensity, 0.3f, adjustSpeed * Time.deltaTime);
        }
        else
        {
            if(tod_cont.enabled == false)
            {
                tod_cont.enabled = true;
                tod_cont.skyTime = last_tod;
            }

            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, lm_outdoorInt, adjustSpeed * Time.deltaTime);
            RenderSettings.reflectionIntensity = Mathf.Lerp(RenderSettings.reflectionIntensity, lm_outdoorRef, adjustSpeed * Time.deltaTime);

            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, lm_outdoorFog, adjustSpeed * Time.deltaTime);

            //ambientLight.intensity = Mathf.Lerp(ambientLight.intensity, 0.8f, adjustSpeed * Time.deltaTime);
        }
    }
}