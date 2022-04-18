using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lootable : MonoBehaviour
{
    public GameObject ItemTableUi;
    public GameObject text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        text.SetActive(true);
        if (Input.GetKeyDown(KeyCode.F))
        {
            text.SetActive(false);
            ItemTableUi.SetActive(true);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        text.SetActive(false);
    }
}
