using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : Player
{
    public GameObject Upbtn;
    public TMPro.TMP_Text SPtext;
    public int sp = 0;
    public Image[] StrengthImage;
    public Image[] CadioImage;
    public Image[] HandicraftImage;
    public Image[] AgilityImage;
    public Image[] IntellectImage;

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

    /* --------------------스텟 증가시 호출되는 함수 부분----------------------- */
    public void StrengthUp()
    {
        ++Stat.Strength;
        Stat.MaxStamina += 10;
        StrengthImage[Stat.Strength - 1].GetComponent<Image>().color = Color.red;
        --sp;
        SPtext.text = "SP : " + sp.ToString();
    }
    public void CadioUp()
    {
        ++Stat.Cadio;
        Stat.MaxHP += 20;
        CadioImage[Stat.Cadio - 1].GetComponent<Image>().color = Color.red;
        --sp;
        SPtext.text = "SP : " + sp.ToString();
    }
    public void HandicraftUp()
    {
        ++Stat.Handicraft;
        HandicraftImage[Stat.Handicraft - 1].GetComponent<Image>().color = Color.red;
        --sp;
        SPtext.text = "SP : " + sp.ToString();
    }
    public void AgilityUp()
    {
        ++Stat.Agility;
        Stat.MoveSpeed += 0.5f;
        AgilityImage[Stat.Agility - 1].GetComponent<Image>().color = Color.red;
        --sp;
        SPtext.text = "SP : " + sp.ToString();
    }
    public void IntellectUp()
    {
        ++Stat.Intellect;
        IntellectImage[Stat.Intellect - 1].GetComponent<Image>().color = Color.red;
        --sp;
        SPtext.text = "SP : " + sp.ToString();
    }
    /* -------------------------------------------------------------------------- */
}
