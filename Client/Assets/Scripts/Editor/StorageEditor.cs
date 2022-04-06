using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StorageEditor : EditorWindow
{
    private bool isGameStarted;

    private ItemSO selectedItem;
    private int amount;

    [MenuItem("Debug Editor/Storage Editor")]
    public static void OpenEditor()
    {
        StorageEditor window = EditorWindow.GetWindow(typeof(StorageEditor)) as StorageEditor;
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Space(20.0f);

        GUILayout.Label("Storage Editor", EditorStyles.boldLabel);

        GUILayout.Space(10.0f);

        if(isGameStarted)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Selected Item");

            selectedItem = EditorGUILayout.ObjectField(selectedItem, typeof(ItemSO), false) as ItemSO;

            amount = EditorGUILayout.IntField(amount, GUILayout.Width(30f));

            if (GUILayout.Button("Add"))
            {
                if(selectedItem != null && !selectedItem.canRefining)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        StorageManager.Instance.AddItem(selectedItem);
                    }
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10.0f);

            GUILayout.BeginHorizontal();

            GUILayout.Label("Add AllItem");

            if (GUILayout.Button("Excute"))
            {
                StorageManager.Instance.FillAllItem();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("게임이 사직되지 않았습니다 " + isGameStarted, MessageType.Error);
        }

        GUILayout.EndVertical();
    }

    private void Awake()
    {
        isGameStarted = false;

        EventManager.SubGameStart(p =>
        {
            isGameStarted = true;
        });

        EventManager.SubGameOver(goc =>
        {
            isGameStarted = false;
        });

        EventManager.SubExitRoom(() =>
        {
            isGameStarted = false;
        });
    }
}
