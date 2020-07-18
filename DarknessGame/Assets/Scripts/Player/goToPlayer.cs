using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goToPlayer : MonoBehaviour
{

    [SerializeField] public GameObject player;
    private Transform pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        pos.position = player.transform.position + new Vector3(-0.5f,0.5f,0f);
    }
}
