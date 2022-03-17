using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MultiBuild : MonoBehaviour
{
    [Header("한번에 빌드할 클라이언트의 갯수")]
    public int numOfClient = 1;

#if UNITY_EDITOR
    //%(Ctrl), #(Shift), &(Alt)
    [MenuItem("MultiPlayer/4Player #b")]
    public static void BuildNPlayer()
    {
        PerformWin64Build(4);
        Debug.Log($"{4}플레이어 빌드");
    }

    static string GetProjectName()
    {
        string[] names = Application.dataPath.Split('/');
        return names[names.Length - 2];
    }

    static string[] GetScenePaths()
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

            File.WriteAllText($"Builds/Win64/{GetProjectName()}{i}/{GetProjectName()}{i}.json", "{\"msg:씨발 좀 돼라\"}");
        }
    }
#endif
}
