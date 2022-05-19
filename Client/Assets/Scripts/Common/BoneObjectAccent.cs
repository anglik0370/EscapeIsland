using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoneObjectAccent : MonoBehaviour
{
    private readonly Vector3 CHAR_SCALE = new Vector3(0.35f, 0.35f);

    private readonly Vector3 ORIGIN_ROT = Vector3.zero;
    private readonly Vector3 FLIP_ROT = new Vector3(0, 180, 0);

    private const float DEFAULT_SCALE_Z = 1;
    private const float FLIP_SCALE_Z = -1;

    [SerializeField]
    private Material outlineMat;
    
    [SerializeField]
    private Color outlineColor = Color.white;
    [SerializeField]
    private float outlineThickness = 4f;

    [SerializeField]
    private string outlineLayerName;

    [SerializeField]
    private List<CharacterSO> charSOList;

    [SerializeField]
    private List<GameObject> prefabList;

    [SerializeField]
    private GameObject curCharObj;

    public void Awake()
    {
        charSOList = Resources.LoadAll<CharacterSO>("CharacterSO").ToList();
        prefabList = new List<GameObject>();

        curCharObj = null;

        //transform.localScale = CHAR_SCALE;

        for (int i = 0; i < charSOList.Count; i++)
        {
            GameObject obj = Instantiate(charSOList[i].playerPrefab, transform);

            GameObject.Destroy(obj.GetComponent<SpritePositionSort>());
            GameObject.Destroy(obj.transform.Find("Ghost").gameObject);

            List<SpriteRenderer> srList = obj.GetComponent<CharComponentHolder>().srList;

            for (int j = 0; j < srList.Count; j++)
            {
                srList[j].sortingLayerName = outlineLayerName;

                srList[j].material = outlineMat;
                srList[j].material.SetColor("_OutlineColor", outlineColor);
                srList[j].material.SetFloat("_OutlineThickness", outlineThickness);
            }

            obj.SetActive(false);

            prefabList.Add(obj);
        }
    }

    public void Enable(Vector3 pos, CharComponentHolder cch, bool isFlip)
    {
        //transform.position = pos;

        //for (int i = 0; i < prefabList.Count; i++)
        //{
        //    curCharObj = prefabList.Find(x => x.GetComponent<CharComponentHolder>().charSO.id == cch.charSO.id);
        //}

        //if(curCharObj ==  null)
        //{
        //    print("그런 캐릭터는 여기 없다.");
        //    return;
        //}

        //curCharObj.SetActive(true);

        //CharComponentHolder curCCH = curCharObj.GetComponent<CharComponentHolder>();

        //curCharObj.transform.rotation = Quaternion.Euler(isFlip ? FLIP_ROT : ORIGIN_ROT);
        //curCharObj.transform.localPosition = isFlip ? curCharObj.transform.localPosition : new Vector3(-curCCH.charSO.adjsutPos.x, curCCH.charSO.adjsutPos.y, curCCH.charSO.adjsutPos.z);
        //curCharObj.transform.localScale = new Vector3(curCharObj.transform.localScale.x, curCharObj.transform.localScale.y, isFlip ? FLIP_SCALE_Z : DEFAULT_SCALE_Z);

        //for (int i = 0; i < curCCH.boneTrmList.Count; i++)
        //{
        //    curCCH.boneTrmList[i].position = cch.boneTrmList[i].position;
        //    curCCH.boneTrmList[i].rotation = cch.boneTrmList[i].rotation;
        //}
    }

    public void Disable()
    {
        if (curCharObj == null) return;

        curCharObj.SetActive(false);
        curCharObj = null;
    }
}
