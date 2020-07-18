using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class turnOffOnGameStart : MonoBehaviour
{
    public bool testing;
    public Light2D mylight;
    // Start is called before the first frame update
    void Start()
    {
        mylight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!testing)
        {
            mylight.intensity = 0;
        }
        
    }
}
