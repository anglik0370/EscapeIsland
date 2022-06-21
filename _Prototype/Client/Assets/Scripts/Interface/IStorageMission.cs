using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorageMission : IMission
{
    public ItemSO StorageItem { get; }

    public Team Team { get; }

    public void SetTeam(Team team);
    public void UpdateCurItem();
}
