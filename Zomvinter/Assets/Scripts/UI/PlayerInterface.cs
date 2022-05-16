using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    #region 스크립트 참조 영역
    ///<summary> Canvas </summary>
    private Canvas _canvas;
    ///<summary> Canvas 안에 있는 Inventory.cs </summary>
    private Inventory _inventory;

    ///<summary> 주 무기 슬롯 스크립트 </summary>
    private VisualizeSlot Primary;
    ///<summary> 보조 무기 슬롯 스크립트 </summary>
    private VisualizeSlot Seconcdary;
    ///<summary> 소모품 슬롯 스크립트 </summary>
    private VisualizeSlot[] Expands;
    #endregion

    #region 오브젝트 참조 영역
    ///<summary> 주 무기 슬롯 영역 </summary>
    private GameObject _primaryGo;
    ///<summary> 보조 무기 슬롯 영역 </summary>
    private GameObject _secondaryGo;
    ///<summary> 소모품 슬롯 영역 </summary>
    private GameObject _expandGo;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트

    /// <summary> 디버깅용 에디터 이벤트 메서드 (Awake에 동일한 코드를 작성 할 것) </summary>
    private void OnValidate()
    {
        GetScripts();
    }

    private void Awake()
    {
        GetScripts();
    }

    void Update()
    {
        UpdateSlot();
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private 함수
    /// <summary> 필요 스크립트 참조 메서드 </summary>
    private void GetScripts()
    {
        // Inventory.cs 참조
        _canvas = GetComponentInParent<Canvas>();
        _inventory = _canvas.GetComponentInChildren<Inventory>();

        // GameObject 영역 참조
        _primaryGo = transform.GetChild(0).Find("CurPrimary").gameObject;
        _secondaryGo = transform.GetChild(0).Find("CurSecondary").gameObject;
        _expandGo = transform.Find("Item").gameObject;

        // VisualizeSlot 참조
        Primary = _primaryGo.GetComponentInChildren<VisualizeSlot>();
        Seconcdary = _secondaryGo.GetComponentInChildren<VisualizeSlot>();
        Expands = _expandGo.GetComponentsInChildren<VisualizeSlot>();
    }

    /// <summary> HUD에 표시 될 슬롯들의 데이터 값 전달 메서드 </summary>
    private void UpdateSlot()
    {
        // 1. 현재 장착중인 주 무기가 null 이 아닐 경우
        if (_inventory.PrimaryItems[0] != null) // 추후 수정 필요
        {
            Primary.SetItemData(_inventory.PrimaryItems[0]);
        }
        // 1-1. null 인 경우
        else
        {
            Primary.SetItemData(null);
        }

        // 2. 현재 장착중인 보조무기가 null 이 아닌 경우
        if (_inventory.SecondaryItems != null)
        {
            Seconcdary.SetItemData(_inventory.SecondaryItems);
        }
        // 2-1. null 인 경우
        else
        {
            Seconcdary.SetItemData(null);
        }

        for (int i = 0; i < _inventory.ConsumableItems.Count; i++)
        {
            // 3. 현재 장착중인 소모품이 null이 아닌 경우
            if (_inventory.ConsumableItems[i] != null)
            {
                Expands[i].SetItemData(_inventory.ConsumableItems[i]);
            }
            // 3-1. null 인 경우
            else
            {
                Expands[i].SetItemData(null);
            }
        }
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수
    /// <summary> HUD 보이기 </summary>
    public void ShowHUD() => gameObject.SetActive(true);
    /// <summary> HUD 숨기기 </summary>
    public void HideHUD() => gameObject.SetActive(false);
    #endregion

}
