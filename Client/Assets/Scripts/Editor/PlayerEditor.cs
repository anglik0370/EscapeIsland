using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerEditor : EditorWindow
{
    private Player mainplayer;

    private Player selectedPlayer;
    private Vector2 moveDir;

    private bool isOptionOpen;
    private bool isControllerOpen;

    [MenuItem("Debug Editor/Player Editor %#p")]
    public static void OpenEditor()
    {
        PlayerEditor window = EditorWindow.GetWindow(typeof(PlayerEditor)) as PlayerEditor;
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Space(20.0f);

        GUILayout.Label("Player Editor", EditorStyles.boldLabel);

        GUILayout.Space(10.0f);

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
                EditorGUILayout.HelpBox("플레이어가 선택되지 않았습니다", MessageType.Error);
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

                EditorGUILayout.HelpBox("컨트롤러를 사용하려면 IsRemote를 꺼줘야 합니다", MessageType.Info);

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
                EditorGUILayout.HelpBox("플레이어가 선택되지 않았습니다", MessageType.Error);
            }
        }
    }

    private void Awake()
    {
        EventManager.SubEnterRoom(p =>
        {
            mainplayer = p;
        });
    }

    private void Update()
    {
        if(selectedPlayer != null)
        {
            selectedPlayer.Move(moveDir);
        }
    }
}
