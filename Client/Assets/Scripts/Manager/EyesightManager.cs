using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EyesightManager : MonoBehaviour
{
    private Player player;
    private Collider2D playerCol;

    [SerializeField]
    private Collider2D labAreaCollider;
    [SerializeField]
    private Transform labObjParentTrm;
    [SerializeField]
    private List<Transform> labObjList;
    [SerializeField]
    private List<GameObject> anotherLabObjList;

    private void Awake()
    {
        labObjList = labObjParentTrm.GetComponentsInChildren<Transform>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
            playerCol = player.FootCollider;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        if(Physics2D.IsTouching(labAreaCollider, playerCol))
        {
            //이러면 연구소 안에 있는거임
            labObjList.ForEach(x => x.gameObject.SetActive(true));
            anotherLabObjList.ForEach(x => x.SetActive(true));
        }
        else
        {
            labObjList.ForEach(x => x.gameObject.SetActive(false));
            anotherLabObjList.ForEach(x => x.SetActive(false));
        }
    }

    //이녀석은 각 방 안에 플레이어가 있는지를 검사하고 그에 따라 오브젝트를 껏다켰다 해주는 놈이다
}
