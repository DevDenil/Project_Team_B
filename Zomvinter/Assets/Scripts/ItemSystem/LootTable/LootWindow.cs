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
    // ���İ����� LootWindow�� ���ų� ���� ����.
    private CanvasGroup canvasGroup;

    // ���Ȯ�� ��� �� Ȯ���� ������
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
    // ���� �����ִ� �������� Ȯ��
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
        //LootWindow�� �ݰ� �ٽ� ���� �������� ��� ���̴°� �����մϴ�.
        pages.Clear();
        ClearButtons();

        // �ش� ���ӿ�����Ʈ�� �Ⱥ��̵��� ����
        canvasGroup.alpha = 0;
        // �ش� ���ӿ�����Ʈ�� Ŭ���� �ȵǵ��� ����
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

        // �������� �������� ���ٸ�
        if (pages[pageIndex].Count == 0)
        {
            // �ش� ������ ����
            pages.Remove(pages[pageIndex]);

            // ������ �������̰� ������ ��ȣ�� 0���� ũ��.
            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }

            // ������ ����
            AddLoot();
        }
    }
    */
}
