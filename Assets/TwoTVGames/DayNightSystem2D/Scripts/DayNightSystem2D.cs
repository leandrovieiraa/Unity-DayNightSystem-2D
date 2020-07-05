using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum DayCycles
{
    Sunrise = 0,
    Day = 1,
    Sunset = 2,
    Night = 3,
    Midnight = 4
}

public class DayNightSystem2D : MonoBehaviour
{
    [Header("Controllers")]
    public Light2D globalLight; // global light
    public float cycleCurrentTime = 0; // current cycle time
    public float cycleMaxTime = 60; // duration of cycle
    public DayCycles dayCycle = DayCycles.Sunrise; // default cycle

    [Header("Cycle Colors")]
    public Color sunrise; // 6:00 at 10:00
    public Color day; // 10:00 at 16:00
    public Color sunset; // 16:00 20:00
    public Color night; // 20:00 at 00:00
    public Color midnight; // 00:00 at 06:00

    [Header("Objects")]
    public Light2D[] mapLights; // enable/disable in day/night states

    void Start() 
    {
        dayCycle = DayCycles.Sunrise; // start with sunrise state
        globalLight.color = sunrise; // start global color at sunrise
    }

     void Update()
     {
        // Update cycle time
        cycleCurrentTime += Time.deltaTime;

        // Check if cycle time reach cycle duration time
        if (cycleCurrentTime >= cycleMaxTime) 
        {
            cycleCurrentTime = 0; // back to 0 (restarting cycle time)
            dayCycle++; // change cycle state
        }

        // If reach final state we back to sunrise (Enum id 0)
        if(dayCycle > DayCycles.Midnight)
            dayCycle = 0;

        // percent it's an value between current and max time to make a color lerp smooth
        float percent = cycleCurrentTime / cycleMaxTime;

        // Sunrise state (you can do a lot of stuff based on every cycle state, like enable animals only in sunrise )
        if(dayCycle == DayCycles.Sunrise)
        {
            ControlLightMaps(false); // disable map light (keep enable only at night)
            globalLight.color = Color.Lerp(sunrise, day, percent);
        }

        // Mid Day state
        if(dayCycle == DayCycles.Day)
            globalLight.color = Color.Lerp(day, sunset, percent);

        // Sunset state
        if(dayCycle == DayCycles.Sunset)
            globalLight.color = Color.Lerp(sunset, night, percent);

        // Night state
        if(dayCycle == DayCycles.Night)
        {
            ControlLightMaps(true); // enable map lights (disable only in day states)
            globalLight.color = Color.Lerp(night, midnight, percent);        
        }

        // Midnight state
        if(dayCycle == DayCycles.Midnight)
            globalLight.color = Color.Lerp(midnight, day, percent);     
     }

     void ControlLightMaps(bool status)
     {
         // loop in light array of objects to enable/disable
         if(mapLights.Length > 0)
            foreach(Light2D _light in mapLights)
                _light.gameObject.SetActive(status);
     }
}
