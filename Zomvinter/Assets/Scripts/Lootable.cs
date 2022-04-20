using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lootable : MonoBehaviour
{
    public GameObject ItemTableUi;
    public GameObject text;
    bool active = false;
    
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
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            text.SetActive(false);
            active = !active;
            ItemTableUi.SetActive(active);
        }
    }

    void OnTriggerExit(Collider other)
    {
        text.SetActive(false);
        ItemTableUi.SetActive(false);
    }
}
