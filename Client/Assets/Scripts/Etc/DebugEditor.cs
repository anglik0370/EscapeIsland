using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugEditor : EditorWindow
{
    private Player selectedPlayer;
    private Vector2 moveDir;

    private bool isOptionOpen;
    private bool isControllerOpen;

    [MenuItem("Debug Editor/Open Editor %#d")]
    public static void OpenEditor()
    {
        DebugEditor window = EditorWindow.GetWindow(typeof(DebugEditor)) as DebugEditor;
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Controller", EditorStyles.boldLabel);

        GUILayout.BeginVertical();

        selectedPlayer = EditorGUILayout.ObjectField("Select Player", selectedPlayer, typeof(Player), true) as Player;

        GUILayout.EndVertical();

        GUILayout.Space(20.0f);

        isOptionOpen = EditorGUILayout.Foldout(isOptionOpen, "Player Option", true);

        if(isOptionOpen)
        {
            if (selectedPlayer != null)
            {
                selectedPlayer.isRemote = EditorGUILayout.Toggle("IsRemote", selectedPlayer.isRemote);
            }
            else
            {
                EditorGUILayout.HelpBox("�÷��̾ ���õ��� �ʾҽ��ϴ�", MessageType.Error);
            }
        }

        GUILayout.Space(20.0f);

        isControllerOpen = EditorGUILayout.Foldout(isControllerOpen, "Controller", true);

        if (isControllerOpen)
        {
            if(selectedPlayer != null)
            {
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();
                bool isUp = (GUILayout.RepeatButton("UP", GUILayout.Width(50), GUILayout.Height(50)));
                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();
                bool isLeft = (GUILayout.RepeatButton("LEFT", GUILayout.Width(50), GUILayout.Height(50)));
                GUILayout.Space(50);
                bool isRight = (GUILayout.RepeatButton("RIGHT", GUILayout.Width(50), GUILayout.Height(50)));
                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();
                bool isDown = (GUILayout.RepeatButton("DOWN", GUILayout.Width(50), GUILayout.Height(50)));
                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();

                EditorGUILayout.HelpBox("��Ʈ�ѷ��� ����Ϸ��� IsRemote�� ����� �մϴ�", MessageType.Info);

                GUILayout.EndVertical();

                if (isUp)
                {
                    moveDir = Vector2.up;
                }
                else if (isLeft)
                {
                    moveDir = Vector2.left;
                }
                else if (isRight)
                {
                    moveDir = Vector2.right;
                }
                else if (isDown)
                {
                    moveDir = Vector2.down;
                }
                else
                {
                    moveDir = Vector2.zero;
                }
            }
            else
            {
                EditorGUILayout.HelpBox("�÷��̾ ���õ��� �ʾҽ��ϴ�", MessageType.Error);
            }
        }
    }

    private void Update()
    {
        if(selectedPlayer != null)
        {
            selectedPlayer.Move(moveDir);
        }
    }
}
