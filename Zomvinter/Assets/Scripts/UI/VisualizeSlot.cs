using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeSlot : MonoBehaviour
{
    [SerializeField]
    private ItemType SlotType;

    [SerializeField]
    public ItemData Data = null;

    [SerializeField]
    private GameObject _itemGo;
    [SerializeField]
    private GameObject _textGo;


    Image _itemImage;

    TMPro.TMP_Text _itemText;

    private void OnValidate()
    {
        _itemImage = _itemGo.GetComponent<Image>();
        _itemText = _textGo.GetComponent<TMPro.TMP_Text>();
    }
    private void Start()
    {

    }

    private void Update()
    {
        UpdateData();
    }

    /// <summary> 아이템 데이터로부터 이미지와 텍스트를 설정 </summary>
    private void UpdateData ()
    {
        if (Data != null)
        {
            _itemImage.sprite = Data.ItemImage;
            if (Data is CountableItemData)
            {
                //_itemText.text = (CountableItem)((CountableItemData)Data)
            }
            else
            {
                _itemText.text = Data.ItemName;
            }
        }
        else
        {
            _itemImage.sprite = null;
            _itemText.text = "";
        }
    }
}
