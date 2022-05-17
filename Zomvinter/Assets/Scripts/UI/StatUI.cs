using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    #region 오브젝트 참조 영역
    [SerializeField]
    /// <summary> 스킬 포인트 텍스트 </summary>
    TMPro.TMP_Text SkillPointText;
    #endregion

    #region 버튼 영역
    [SerializeField]
    /// <summary> 버튼 영역 </summary>
    Transform BtnArea;
    /// <summary> 버튼 배열 </summary>
    Button[] StatUpBtn;
    #endregion

    #region 게이지 영역
    [SerializeField]
    /// <summary> 힘 게이지 영역 </summary>
    Transform StrengthGague;
    [SerializeField]
    /// <summary> 건강 게이지 영역 </summary>
    Transform ConstitutionGague;
    [SerializeField]
    /// <summary> 손재주 게이지 영역 </summary>
    Transform DexterityGague;
    [SerializeField]
    /// <summary> 지구력 게이지 영역 </summary>
    Transform EnduranceGauge;
    [SerializeField]
    /// <summary> 지능 게이지 영역 </summary>
    Transform IntelligenceGague;

    /// <summary> 힘 게이지 바 </summary>
    Image[] StrengthBar;
    /// <summary> 건강 게이지 바 </summary>
    Image[] ConstitutionBar;
    /// <summary> 손재주 게이지 바 </summary>
    Image[] DexterityBar;
    /// <summary> 지구력 게이지 바 </summary>
    Image[] EnduranceBar;
    /// <summary> 지능 게이지 바 </summary>
    Image[] IntelligenceBar;
    #endregion

    #region 스텟 영역
    /// <summary> 힘 레벨 값 </summary>
    int StrengthLevel = 0;
    /// <summary> 건강 레벨 값 </summary>
    int ConstitutionLevel = 0;
    /// <summary> 손재주 레벨 값 </summary>
    int DexterityLevel = 0;
    /// <summary> 지구력 레벨 값 </summary>
    int EnduranceLevel = 0;
    /// <summary> 지능 레벨 값 </summary>
    int IntelligenceLevel = 0;
    #endregion

    /// <summary> 스킬 포인트 값 </summary>
     static int SP = 0;

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
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
    #region Private 함수
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
    #region Public 함수
    public void AddSP(int amount)
    {
        SP += amount;
    }
    #endregion

    /***********************************************************************
    *                               Button Listener
    ***********************************************************************/
    #region 버튼 리스너
    /// <summary> 힘 레벨 업 - 버튼 리스너 </summary>
    public void StrengthUp()
    {
        if (!isMaxSkill(StrengthLevel))
        {
            StrengthLevel = UseSP(StrengthLevel);
            StrengthBar[StrengthLevel - 1].color = Color.red;
            DeActiveSkillBtn(0, StrengthLevel);
        }
    }

    /// <summary> 건강 레벨 업 - 버튼 리스너 </summary>
    public void ConstitutionUp()
    {
        if (!isMaxSkill(ConstitutionLevel))
        {
            ConstitutionLevel = UseSP(ConstitutionLevel);
            ConstitutionBar[ConstitutionLevel - 1].color = Color.red;
            DeActiveSkillBtn(1, ConstitutionLevel);
        }
    }

    /// <summary> 손재주 레벨 업 - 버튼 리스너 </summary>
    public void DexterityUp()
    {
        if (!isMaxSkill(DexterityLevel))
        {
            DexterityLevel = UseSP(DexterityLevel);
            DexterityBar[DexterityLevel - 1].color = Color.red;
            DeActiveSkillBtn(2, DexterityLevel);
        }
    }

    /// <summary> 지구력 레벨 업 - 버튼 리스너 </summary>
    public void EnduranceUp()
    {
        if (!isMaxSkill(EnduranceLevel))
        {
            EnduranceLevel = UseSP(EnduranceLevel);
            EnduranceBar[EnduranceLevel - 1].color = Color.red;
            DeActiveSkillBtn(3, EnduranceLevel);
        }
    }

    /// <summary> 지능 레벨 업 - 버튼 리스너 </summary>
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
