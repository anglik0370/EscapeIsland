using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorageMission : IMission
{
    public ItemSO StorageItem { get; }
}
