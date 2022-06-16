using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sex
{
    Male,
    Female
}
[CreateAssetMenu(fileName = "new CharacterSO", menuName = "SO/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public int id;

    public GameObject playerPrefab;

    public Sprite profileImg;
    public Sprite ghostProfileImg;

    public Sprite standImg;
    public Sprite deadImg;

    public string charName;
    public Sex sex;

    public Vector3 adjsutPos;

    public SkillSO skill;
}
