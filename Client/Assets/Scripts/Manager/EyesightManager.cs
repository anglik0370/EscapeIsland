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

    void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });

        EventManager.SubGameOver(goc =>
        {
            LabObjSetActive(false);
        });
    }

    void Update()
    {
        if (player == null) return;

        LabObjSetActive(Physics2D.IsTouching(labAreaCollider, player.FootCollider));
    }

    private void LabObjSetActive(bool active)
    {
        if (labObjList[0].gameObject.activeSelf == active) return;

        labObjList.ForEach(x => x.gameObject.SetActive(active));
        anotherLabObjList.ForEach(x => x.SetActive(active));
    }

    //이녀석은 각 방 안에 플레이어가 있는지를 검사하고 그에 따라 오브젝트를 껏다켰다 해주는 놈이다
}
