using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public GameObject[] invisibleObjects;
    public Material invisible;
    public void OnEnable()
    {
        SpriteRenderer sr;

        foreach (GameObject material in invisibleObjects)
        {
            if(material == null)
            {
                return;
            }

            sr = material.GetComponent<SpriteRenderer>();
            Debug.Log("Called");
            sr.material = invisible;
        }
    }
}
