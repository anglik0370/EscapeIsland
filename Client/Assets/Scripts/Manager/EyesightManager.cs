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
    //�̳༮�� �� �� �ȿ� �÷��̾ �ִ����� �˻��ϰ� �׿� ���� ������Ʈ�� �����״� ���ִ� ���̴�
}
