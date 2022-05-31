using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class RoofControl : MonoBehaviour
{
    public GameObject Roof;
    public LayerMask Player;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");
        if((Player & (1<<other.gameObject.layer)) != 0)
        {
            Roof.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((Player & (1 << other.gameObject.layer)) != 0)
        {
            Roof.SetActive(true);
        }
    }
}
