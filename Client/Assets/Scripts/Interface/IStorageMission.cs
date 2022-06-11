using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorageMission : IMission
{
    public ItemSO StorageItem { get; }

    public int MaxItemCount { get; }
    public int CurItemCount { get; }

    public void AddCurItem();
    public void UpdateCurItem();
}
