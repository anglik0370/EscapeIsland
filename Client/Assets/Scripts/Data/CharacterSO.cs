using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sex
{
    Male,
    Female
}
[CreateAssetMenu(fileName = "new CharacterSO", menuName = "Character")]
public class CharacterSO : ScriptableObject
{
    public int id;
    public Sprite profileImg;
    public Sprite standImg;
    public string charName;
    public Sex sex;
}
