using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharComponentHolder : MonoBehaviour
{
    private const string containStr = "bone";

    public CharacterSO charSO;
    public Animator anim;
    public List<SpriteRenderer> srList;
    public List<Transform> boneTrmList;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        srList = GetComponentsInChildren<SpriteRenderer>().ToList();
        boneTrmList = GetComponentsInChildren<Transform>().Where(x => x.gameObject.name.Contains(containStr)).ToList();
    }
}
