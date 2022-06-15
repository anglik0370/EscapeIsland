using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ConvertRecipe", menuName = "SO/ConvertRecipeSO")]
public class ConvertRecipeSO : ScriptableObject
{
    public ItemSO beforeItem;
    public ItemSO afterItem; 
}
