using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EyesightManager : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private Transform objectParentTrm;

    [SerializeField]
    private List<GameObject> otherList;

    private List<AreaStateHolder> areaStateHolderList;

    private void Awake()
    {
        areaStateHolderList = objectParentTrm.GetComponentsInChildren<AreaStateHolder>().ToList();
    }

    void Start()
    {
        EventManager.SubEnterRoom(p =>
        {
            player = p;
        });
    }

    void Update()
    {
        if (player == null) return;

        for (int i = 0; i < areaStateHolderList.Count; i++)
        {
            areaStateHolderList[i].Sr.color = areaStateHolderList[i].AreaState == player.AreaState ? UtilClass.opacityColor : UtilClass.limpidityColor;
        }

        otherList[0].SetActive(player.AreaState == AreaState.BottleStorage);
        otherList[1].SetActive(player.AreaState == AreaState.RefineryInLab);
    }
    //이녀석은 각 방 안에 플레이어가 있는지를 검사하고 그에 따라 오브젝트를 껏다켰다 해주는 놈이다
}
