using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LootWindow : MonoBehaviour
{
    /*
    private static LootWindow instance;
    public static LootWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LootWindow>();
            }

            return instance;
        }
    }
    // 알파값으로 LootWindow을 열거나 닫을 예정.
    private CanvasGroup canvasGroup;

    // 드랍확률 계산 후 확정된 아이템
    private List<Item> droppedLoot = new List<Item>();

    public void CreatePages(List<Item> items)
    {
        List<Item> page = new List<Item>();

        for (int i = 0, i < items.Count; i++)
        {
            page.Add(items[i]);
            if(page.Count == 4 || i = Item.Count -1)
            {
                
            }
        }
    }
    // 현재 열려있는 상태인지 확인
    public bool IsOpen
    {
        get { return canvasGroup.alpha > 0; }

    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Close()
    {
        //LootWindow를 닫고 다시 열때 아이템이 계속 쌓이는걸 방지합니다.
        pages.Clear();
        ClearButtons();

        // 해당 게임오브젝트가 안보이도록 변경
        canvasGroup.alpha = 0;
        // 해당 게임오브젝트가 클릭이 안되도록 변경
        canvasGroup.blocksRaycasts = false;

    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    public void TakeLoot(Item item)
    {
        pages[pageIndex].Remove(item);

        droppedLoot.Remove(item);

        // 페이지에 아이템이 없다면
        if (pages[pageIndex].Count == 0)
        {
            // 해당 페이지 삭제
            pages.Remove(pages[pageIndex]);

            // 마지막 페이지이고 페이지 번호가 0보다 크다.
            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }

            // 페이지 갱신
            AddLoot();
        }
    }
    */
}
