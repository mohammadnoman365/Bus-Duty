using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BusLights : MonoBehaviour
{
    public enum Side
    {
        Reverse,
        Brake
    }

    [System.Serializable]
    public struct Light
    {
        public GameObject lightObject;
        public Side side;
    }

    public bool isReverseLightsOn;
    public bool isBrakeLightsOn;

    public List<Light> lights;

    public void SetReverseLights(bool state)
    {
        foreach (var light in lights)
        {
            if (light.side == Side.Reverse)
                light.lightObject.SetActive(state);
        }
    }

    public void SetBrakeLights(bool state)
    {
        foreach (var light in lights)
        {
            if (light.side == Side.Brake)
                light.lightObject.SetActive(state);
        }
    }
}
