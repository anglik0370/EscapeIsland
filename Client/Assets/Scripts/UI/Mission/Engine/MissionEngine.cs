using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MissionEngine : MonoBehaviour, IMission
{
    private CanvasGroup cvs;
    public CanvasGroup Cvs => cvs;

    [SerializeField]
    private Transform wireParentTrm;
    private List<WireMObj> wireList;

    [SerializeField]
    private Transform wireOrderImgParentTrm;
    private List<Image> wireOrderImgList;

    [SerializeField]
    private MissionType missionType;
    public MissionType MissionType => missionType;

    private void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        wireList = wireParentTrm.GetComponentsInChildren<WireMObj>().ToList();
        wireOrderImgList = wireOrderImgParentTrm.GetComponentsInChildren<Image>().ToList();
    }

    public void Init()
    {
        wireList.ForEach(x => x.Init());

        List<int> orderList = new List<int>(4) { 1, 2, 3, 4 };

        for (int i = 0; i < 4; i++)
        {
            int idx = UnityEngine.Random.Range(0, orderList.Count);

            wireList[i].SetCuttingOrder(orderList[idx]);
            wireOrderImgList[orderList[idx] - 1].sprite = wireList[i].Sprite;

            orderList.RemoveAt(idx);
        }
    }
}
