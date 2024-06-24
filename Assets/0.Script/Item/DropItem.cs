using Items;
using Save;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class DropItem : MonoBehaviour
{
    public ItemData thisitem;

    public SpriteRenderer _spr;

    public int RotationSpeed;
    public bool isCoroutine;
                                                            
    public void Init(ItemData getitem)
    {
        thisitem = getitem;
        var itemspr = SettingManager.Instance.AitemSprites.Where
            (item => item.imgid == getitem.id).ToArray();

        _spr.sprite = itemspr[0].itemSpr;
    }

    public void Init()
    {
        int randint = Random.Range(0, SettingManager.Instance.AitemSprites.Count);
        ItemData newitem = new ItemData();
        newitem.id = SettingManager.Instance.AitemSprites[randint].imgid;
        newitem.type = SettingManager.Instance.AitemSprites[randint].type;
        newitem.Lank = SettingManager.Instance.AitemSprites[randint].Lank;
         thisitem = newitem;
        var itemspr = SettingManager.Instance.AitemSprites.Where
           (item => item.imgid == newitem.id).ToArray();
        _spr.sprite = itemspr[0].itemSpr;
        thisitem.isNull = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Test();
    }

    async void Test()
    {
        await Task.Delay(3000);
       // Debug.Log("3�� �� ��� ������ ����");
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
           // Debug.Log("3�� ���Դϴ�.");
            return;
        }
        if(Input.GetMouseButton(0))
        {
            PlayManager.Instance.MousePositionNoClick(transform.position);
            StartCoroutine(ResetPickingItemFlag());
            
        }
    }

    private IEnumerator ResetPickingItemFlag()
    {
        Vector3 beforePoint = PlayManager.Instance.pointPosition;

        isCoroutine = true;

        while (Vector3.Distance(beforePoint, PlayManager.Instance.player.transform.position)  < 2 
            || beforePoint == PlayManager.Instance.pointPosition)
        {
            yield return null;

            float Checksqr = Vector3.SqrMagnitude(beforePoint - PlayManager.Instance.player.transform.position);
            //Debug.Log("������ �ȸԾ����� ���� Ȯ�� 1 �÷��̾� �Ÿ�Ȯ�� 0.5���� ũ�� ���� �ȸԾ���: " + Checksqr);
            //Debug.Log("������ �ȸԾ����� ���� Ȯ�� 2 Ŭ���Ѱ��� �޶��� (beforePoint != PlayManager.Instance.pointPosition) : " + (beforePoint != PlayManager.Instance.pointPosition));
            if (Checksqr < 0.5f)
            {
                Debug.Log("�Ÿ� Ȯ��" + Checksqr);
                ItemManager.Instance.AddItem(thisitem);
                Debug.Log("�������� �÷��̾� �翡 ���� �� ������ ȹ��");
                GameData.isPickingItem = false; // �������� �Դ� ������ ���� �� �÷��� ����

                gameObject.SetActive(false);
            }

            if (beforePoint != PlayManager.Instance.pointPosition)
            {
                Debug.Log("�÷��̾ ������ ���̶� �ٸ� �� �����۸��� �ٸ��� Ŭ����");
                break;
            }
        }


        isCoroutine = false;
        yield return new WaitForEndOfFrame(); // ������ ������ ���
        
    }
}
