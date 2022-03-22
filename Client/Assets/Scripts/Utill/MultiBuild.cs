using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MultiBuild : MonoBehaviour
{
#if UNITY_EDITOR
    //%(Ctrl), #(Shift), &(Alt)
    [MenuItem("MultiPlayer/4Player #b")]
    public static void BuildNPlayer()
    {
        PerformWin64Build(10);
        Debug.Log($"{10}플레이어 빌드");
    }

    public static string GetProjectName()
    {
        string[] names = Application.dataPath.Split('/');
        return names[names.Length - 2];
    }

    public static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }

    public static void PerformWin64Build(int playerCount)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

        for (int i = 1; i <= playerCount; i++)
        {
            BuildPipeline.BuildPlayer(GetScenePaths(),
                //여기경로에 JSON을 심어서 실행할때 파싱해서 멀티플렛폼 테스트를 편하게
                $"Builds/Win64/{GetProjectName()}{i}/{GetProjectName()}{i}.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.AutoRunPlayer);

            if (i == 1)
            {
                CreateDebugFile(i, true, true, false, true);
            }
            else if (i == playerCount)
            {
                CreateDebugFile(i, true, false, true);
            }
            else
            {
                CreateDebugFile(i, true, false, false);
            }
        }
    }

    public static void CreateDebugFile(int clientNum, bool autoLogin = false, bool createRoom = false, bool gameStart = false, bool isKidnapper = false)
    {
        DebugVO vo = new DebugVO();

        vo.clientId = clientNum;
        vo.autoLogin = autoLogin;
        vo.createRoom = createRoom;
        vo.isKidnapper = isKidnapper;

        string payload = JsonUtility.ToJson(vo);

        File.WriteAllText($"Builds/Win64/{GetProjectName()}{clientNum}/{GetProjectName()}{clientNum}_Data/Debug.json", payload);
    }
#endif
}
