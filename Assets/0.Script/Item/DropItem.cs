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
       // Debug.Log("3초 뒤 드롭 아이템 생성");
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
           // Debug.Log("3초 전입니다.");
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
            //Debug.Log("아이템 안먹어지는 이유 확인 1 플레이어 거리확인 0.5보다 크면 아직 안먹어짐: " + Checksqr);
            //Debug.Log("아이템 안먹어지는 이유 확인 2 클릭한곳이 달라짐 (beforePoint != PlayManager.Instance.pointPosition) : " + (beforePoint != PlayManager.Instance.pointPosition));
            if (Checksqr < 0.5f)
            {
                Debug.Log("거리 확인" + Checksqr);
                ItemManager.Instance.AddItem(thisitem);
                Debug.Log("아이템이 플레이어 곁에 왔음 즉 아이템 획득");
                GameData.isPickingItem = false; // 아이템을 먹는 동작이 끝난 후 플래그 해제

                gameObject.SetActive(false);
            }

            if (beforePoint != PlayManager.Instance.pointPosition)
            {
                Debug.Log("플레이어가 선택한 곳이랑 다름 즉 아이템말고 다른걸 클릭함");
                break;
            }
        }


        isCoroutine = false;
        yield return new WaitForEndOfFrame(); // 프레임 끝까지 대기
        
    }
}
