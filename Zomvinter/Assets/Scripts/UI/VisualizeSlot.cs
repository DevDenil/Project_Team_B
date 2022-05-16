using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeSlot : MonoBehaviour
{
    [SerializeField]
    ///<summary> 슬롯 타입 </summary>
    private ItemType SlotType;

    #region 오므젝트 참조 영역
    ///<summary> 아이템 데이터 </summary>
    private ItemData Data = null;

    ///<summary> 아이템 이미지 오브젝트 </summary>
    private GameObject _itemGo;
    ///<summary> 아이템 이미지 </summary>
    private Image _itemImage;

    ///<summary> 아이템 텍스트 오브젝트 </summary>
    private GameObject _textGo;
    ///<summary> 아이템 텍스트 </summary>
    private TMPro.TMP_Text _itemText;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트

    /// <summary> 디버깅용 에디터 이벤트 메서드 (Awake나 Start에 동일한 코드를 작성 할 것) </summary>
    private void OnValidate()
    {
        FindItemGo();
        FIndTextGO();
        GetScripts();
    }

    private void Awake()
    {
        FindItemGo();
        FIndTextGO(); 
        GetScripts();
    }

    private void Update()
    {
        UpdateIcon();
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private 함수
    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    private void GetScripts()
    {
        _itemImage = _itemGo.GetComponent<Image>();
        _itemText = _textGo.GetComponent<TMPro.TMP_Text>();
    }

    /// <summary> 아이템 데이터로부터 이미지와 텍스트를 설정 </summary>
    private void UpdateIcon()
    {
        // 1. 아이템 데이터가 있는 경우
        if (Data != null)
        {
            // Active는 가장 먼저
            ShowText();
            _itemImage.sprite = Data.ItemImage;

            // 1-1. 아이템 데이터가 CountableData인 경우
            if (Data is CountableItemData)
            {
                //_itemText.text = (CountableItem)((CountableItemData)Data)
            }
            // 1-2 아이템 데이터가 CountableData가 아닌 경우
            else
            {
                _itemText.text = Data.ItemName;
            }
        }
        // 2. 아이템 데이터가 없는 경우
        else
        {
            _itemImage.sprite = null;
            _itemText.text = "";
            //Deactive는 가장 나중에
            HideText();
        }
    }

    /// <summary> _itemGo 게임 오브젝트 찾는 메서드 </summary>
    private void FindItemGo()
    {
        if(_itemGo == null)
        {
            _itemGo = this.transform.GetChild(0).gameObject;
        }
    }

    /// <summary> _textGo 게임 오브젝트 찾는 메서드 </summary>
    private void FIndTextGO()
    {
        // 1. _textGo가 null 인 경우
        if(_textGo == null)
        {
            _textGo = this.transform.GetChild(1).gameObject;
            
            // 1-1. SlotType이 Primary 이거나 Secondary 인 경우
            if (this.SlotType == ItemType.Primary || this.SlotType == ItemType.Secondary)
            {
                _textGo = _textGo.transform.GetChild(0).gameObject;
            }
        }
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수
    public void SetItemData(ItemData data) { Data = data; }
    #endregion
}
