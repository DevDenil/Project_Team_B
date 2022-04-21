using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(Transform Root, float Height)
    {
        StartCoroutine(Following(Root, Height));
    }
    IEnumerator Following(Transform Root, float Height)
    {
        while (Root != null)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(Root.position);
            pos.y += Height;
            this.GetComponent<RectTransform>().position = pos;
            yield return null;
        }
    }
}
