using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Onoff : MonoBehaviour
{
    [SerializeField] public GameObject player;
    private Transform myPos;
    [SerializeField] public KeyCode control;
    public bool on;
    private Light2D light;
    // Start is called before the first frame update
    void Start()
    {
        myPos = GetComponent<Transform>();
        light = GetComponent<Light2D>();
        on = true;
    }

    void getInput(){
        if(Input.GetKeyDown(control)){
            on = !on;
        }
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
        myPos.position = player.transform.position + new Vector3(-0.5f, 0.5f, 0f);

        if(on){
            light.intensity = 2;
        } else {
            light.intensity = 0;
        }
    }
}
