using Items;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class DropItem : MonoBehaviour
{
    public Item thisitem;

    public SpriteRenderer _spr;

    public int RotationSpeed;
    public bool isCoroutine;
                                                            
    public void Init(Item item)
    {
        thisitem = Instantiate(item);
        _spr.sprite = item.img;
    }

    public void Init()
    {
        int randint = Random.Range(0, SettingManager.Instance.ItemDic.Count);
        Items.Item item = Instantiate(SettingManager.Instance.ItemDic[randint]);
        Debug.Log(SettingManager.Instance.ItemDic[0] + " ��� �������� ���� ������");
        //Items.Item item =  Instantiate((SettingManager.Instance.ItemDic.TryGetValue(0, out Item item);
        thisitem = item;
        _spr.sprite = item.img;
    }

    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    async void Test()
    {
        await Task.Delay(3000);
        Debug.Log("3�� �� ��� ������ ����");
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * RotationSpeed);
    }

    private void OnMouseEnter()
    {

    }

    private void OnMouseDown()
    {
        if (thisitem == null || isCoroutine)
        {
            Debug.Log("3�� ���Դϴ�.");
            return;
        }
        if(Input.GetMouseButton(0))
        {
            ItemManager.Instance.AddItem(thisitem);     
            GameData.isPickingItem = false;
            StartCoroutine(ResetPickingItemFlag());
            
        }
    }

    private IEnumerator ResetPickingItemFlag()
    {
        isCoroutine = true;
        yield return new WaitForEndOfFrame(); // ������ ������ ���
        GameData.isPickingItem = false; // �������� �Դ� ������ ���� �� �÷��� ����
        Destroy(gameObject);
    }
}
