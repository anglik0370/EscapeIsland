using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StorageEditor : EditorWindow
{
    private bool isGameStarted;

    //private ItemSO selectedItem;
    private int amount;

    private bool fillAllOnce;

    [MenuItem("Debug Editor/Storage Editor")]
    public static void OpenEditor()
    {
        StorageEditor window = EditorWindow.GetWindow(typeof(StorageEditor)) as StorageEditor;
        window.Show();
    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginVertical();

    //    GUILayout.Space(20.0f);

    //    GUILayout.Label("Storage Editor", EditorStyles.boldLabel);

    //    GUILayout.Space(10.0f);

    //    if(isGameStarted)
    //    {
    //        GUILayout.BeginHorizontal();

    //        GUILayout.Label("Selected Item");

    //        selectedItem = EditorGUILayout.ObjectField(selectedItem, typeof(ItemSO), false) as ItemSO;

    //        amount = EditorGUILayout.IntField(amount, GUILayout.Width(40f));

    //        GUILayout.EndHorizontal();

    //        GUILayout.Space(10.0f);

    //        GUILayout.BeginHorizontal();

    //        if (GUILayout.Button("Add"))
    //        {
    //            if (selectedItem != null)
    //            {
    //                for (int i = 0; i < amount; i++)
    //                {
    //                    StorageManager.Instance.AddItem(selectedItem);
    //                }
    //            }
    //        }

    //        if (GUILayout.Button("Remove"))
    //        {
    //            if (selectedItem != null)
    //            {
    //                for (int i = 0; i < amount; i++)
    //                {
    //                    StorageManager.Instance.RemoveItem(selectedItem);
    //                }
    //            }
    //        }

    //        GUILayout.EndHorizontal();

    //        EditorGUILayout.HelpBox("아이템을 한번에 모두 채우려면 아래의 버튼을 누르세요", MessageType.Info);

    //        if(GUILayout.Button("Fill All Item"))
    //        {
    //            if (!fillAllOnce)
    //            {
    //                StorageManager.Instance.FillAllItem();

    //                fillAllOnce = true;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        EditorGUILayout.HelpBox("게임이 사직되지 않았습니다 " + isGameStarted, MessageType.Error);
    //    }

    //    GUILayout.EndVertical();
    //}

    //private void Awake()
    //{
    //    isGameStarted = false;

    //    EventManager.SubGameStart(p =>
    //    {
    //        isGameStarted = true;
    //        fillAllOnce = false;
    //    });

    //    EventManager.SubGameOver(goc =>
    //    {
    //        isGameStarted = false;
    //        fillAllOnce = false;
    //    });

    //    EventManager.SubExitRoom(() =>
    //    {
    //        isGameStarted = false;
    //        fillAllOnce = false;
    //    });
    //}
}
