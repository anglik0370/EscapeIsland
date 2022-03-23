using UnityEngine;

public interface IMapObject
{
    public Transform GetTrm();
    public Transform GetInteractionTrm();

    public Sprite GetSprite();
}
