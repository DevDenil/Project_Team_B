using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    #region ������Ʈ ���� ����
    [SerializeField]
    /// <summary> ��ų ����Ʈ �ؽ�Ʈ </summary>
    TMPro.TMP_Text SkillPointText;
    #endregion

    #region ��ư ����
    [SerializeField]
    /// <summary> ��ư ���� </summary>
    Transform BtnArea;
    /// <summary> ��ư �迭 </summary>
    Button[] StatUpBtn;
    #endregion

    #region ������ ����
    [SerializeField]
    /// <summary> �� ������ ���� </summary>
    Transform StrengthGague;
    [SerializeField]
    /// <summary> �ǰ� ������ ���� </summary>
    Transform ConstitutionGague;
    [SerializeField]
    /// <summary> ������ ������ ���� </summary>
    Transform DexterityGague;
    [SerializeField]
    /// <summary> ������ ������ ���� </summary>
    Transform EnduranceGauge;
    [SerializeField]
    /// <summary> ���� ������ ���� </summary>
    Transform IntelligenceGague;

    /// <summary> �� ������ �� </summary>
    Image[] StrengthBar;
    /// <summary> �ǰ� ������ �� </summary>
    Image[] ConstitutionBar;
    /// <summary> ������ ������ �� </summary>
    Image[] DexterityBar;
    /// <summary> ������ ������ �� </summary>
    Image[] EnduranceBar;
    /// <summary> ���� ������ �� </summary>
    Image[] IntelligenceBar;
    #endregion

    #region ���� ����
    /// <summary> �� ���� �� </summary>
    int StrengthLevel = 0;
    /// <summary> �ǰ� ���� �� </summary>
    int ConstitutionLevel = 0;
    /// <summary> ������ ���� �� </summary>
    int DexterityLevel = 0;
    /// <summary> ������ ���� �� </summary>
    int EnduranceLevel = 0;
    /// <summary> ���� ���� �� </summary>
    int IntelligenceLevel = 0;
    #endregion

    /// <summary> ��ų ����Ʈ �� </summary>
     static int SP = 0;

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
    private void OnValidate()
    {
        StrengthBar = StrengthGague.GetComponentsInChildren<Image>();
        ConstitutionBar = ConstitutionGague.GetComponentsInChildren<Image>();
        DexterityBar = DexterityGague.GetComponentsInChildren<Image>();
        EnduranceBar = EnduranceGauge.GetComponentsInChildren<Image>();
        IntelligenceBar = IntelligenceGague.GetComponentsInChildren<Image>();
        StatUpBtn = GetComponentsInChildren<Button>();
    }

    void Update()
    {
        VisaulizeSP();
        VisualizeButtonArea();
    }

    private void Start()
    {
        SP = 14;
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private �Լ�
    private void VisaulizeSP()
    {
        SkillPointText.text = "SP : " + SP.ToString();
    }

    private void VisualizeButtonArea()
    {
        if (SP > 0)
        {
            BtnArea.gameObject.SetActive(true);
        }
        else
        {
            BtnArea.gameObject.SetActive(false);
        }
    }
    private void DeActiveSkillBtn(int index, int SkillLv)
    {
        if (SkillLv >= 7) { StatUpBtn[index].gameObject.SetActive(false); }
    }
    private int UseSP(int Skill)
    {
        if (Skill >= 0 || Skill < 7)
        {
            Skill += 1;
            SP -= 1;
            return Skill;
        }
        return Skill;
    }

    private bool isMaxSkill(int SkillLevel)
    {
        bool isMax = false;
        if(SkillLevel >= 7)
        {
            isMax = true;
        }
        else
        {
            isMax = false;
        }
        return isMax;
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�
    public void AddSP(int amount)
    {
        SP += amount;
    }
    #endregion

    /***********************************************************************
    *                               Button Listener
    ***********************************************************************/
    #region ��ư ������
    /// <summary> �� ���� �� - ��ư ������ </summary>
    public void StrengthUp()
    {
        if (!isMaxSkill(StrengthLevel))
        {
            StrengthLevel = UseSP(StrengthLevel);
            StrengthBar[StrengthLevel - 1].color = Color.red;
            DeActiveSkillBtn(0, StrengthLevel);
        }
    }

    /// <summary> �ǰ� ���� �� - ��ư ������ </summary>
    public void ConstitutionUp()
    {
        if (!isMaxSkill(ConstitutionLevel))
        {
            ConstitutionLevel = UseSP(ConstitutionLevel);
            ConstitutionBar[ConstitutionLevel - 1].color = Color.red;
            DeActiveSkillBtn(1, ConstitutionLevel);
        }
    }

    /// <summary> ������ ���� �� - ��ư ������ </summary>
    public void DexterityUp()
    {
        if (!isMaxSkill(DexterityLevel))
        {
            DexterityLevel = UseSP(DexterityLevel);
            DexterityBar[DexterityLevel - 1].color = Color.red;
            DeActiveSkillBtn(2, DexterityLevel);
        }
    }

    /// <summary> ������ ���� �� - ��ư ������ </summary>
    public void EnduranceUp()
    {
        if (!isMaxSkill(EnduranceLevel))
        {
            EnduranceLevel = UseSP(EnduranceLevel);
            EnduranceBar[EnduranceLevel - 1].color = Color.red;
            DeActiveSkillBtn(3, EnduranceLevel);
        }
    }

    /// <summary> ���� ���� �� - ��ư ������ </summary>
    public void IntelligenceUp()
    {
        if (!isMaxSkill(IntelligenceLevel))
        {
            IntelligenceLevel = UseSP(IntelligenceLevel);
            IntelligenceBar[IntelligenceLevel - 1].color = Color.red;
            DeActiveSkillBtn(4, IntelligenceLevel);
        }
    }
    #endregion
}
