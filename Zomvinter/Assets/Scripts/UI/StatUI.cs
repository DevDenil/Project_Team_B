using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public GameObject Upbtn;
    public TMPro.TMP_Text SPtext;
    public int sp = 0;
    public Image[] StrengthImage;
    public Image[] CadioImage;
    public Image[] HandicraftImage;
    public Image[] AgilityImage;
    public Image[] IntellectImage;
    int StrengthLevel = 0;
    int CadioLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        SPtext.text = "SP : " + sp.ToString();
        //sp = 3;
        //GameObject obj = transform.GetChild(3).GetChild(1).GetChild(0).gameObject;
        //sImage = obj.GetComponentsInChildren<Image>();
        SetList();
    }

    // Update is called once per frame
    void Update()
    {
        if (sp > 0)
        {
            Upbtn.SetActive(true);
        }
        else
        {
            Upbtn.SetActive(false);
        }
    }

    void SetList()
    {
        //GameObject obj = transform.GetChild(3).GetChild(1).gameObject;
        //Debug.Log(obj.transform.childCount);
        GameObject o = transform.GetChild(3).GetChild(1).GetChild(0).gameObject;
        StrengthImage = o.GetComponentsInChildren<Image>();
        o = transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        CadioImage = o.GetComponentsInChildren<Image>();
        o = transform.GetChild(3).GetChild(1).GetChild(2).gameObject;
        HandicraftImage = o.GetComponentsInChildren<Image>();
        o = transform.GetChild(3).GetChild(1).GetChild(3).gameObject;
        AgilityImage = o.GetComponentsInChildren<Image>();
        o = transform.GetChild(3).GetChild(1).GetChild(4).gameObject;
        IntellectImage = o.GetComponentsInChildren<Image>();
    }

    public void StrengthUp()
    {
        ++StrengthLevel;
        StrengthImage[StrengthLevel - 1].GetComponent<Image>().color = Color.red;
        --sp;
        SPtext.text = "SP : " + sp.ToString();
    }
    public void CadioUp()
    {
        ++CadioLevel;
        CadioImage[CadioLevel - 1].GetComponent<Image>().color = Color.red;
        --sp;
        SPtext.text = "SP : " + sp.ToString();
    }
    public void HandicraftUp()
    {
        
    }
    public void AgilityUp()
    {
       
    }
    public void IntellectUp()
    {
        
    }
}
