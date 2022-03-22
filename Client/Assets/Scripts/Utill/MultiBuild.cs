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
        PerformWin64Build(4);
        Debug.Log($"{4}플레이어 빌드");
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
                DebugVO vo = new DebugVO(true, false, CheatType.None);
                string payload = JsonUtility.ToJson(vo);

                File.WriteAllText($"Builds/Win64/{GetProjectName()}{i}/Debug.json", payload);
            }
            else if (i == 2)
            {
                DebugVO vo = new DebugVO(false, true, CheatType.KillPlayer);
                string payload = JsonUtility.ToJson(vo);

                File.WriteAllText($"Builds/Win64/{GetProjectName()}{i}/Debug.json", payload);
            }
            else if (i == 3)
            {
                DebugVO vo = new DebugVO(false, false, CheatType.GetItem);
                string payload = JsonUtility.ToJson(vo);

                File.WriteAllText($"Builds/Win64/{GetProjectName()}{i}/Debug.json", payload);
            }
            else if (i == 4)
            {
                DebugVO vo = new DebugVO(false, false, CheatType.FillItem);
                string payload = JsonUtility.ToJson(vo);

                File.WriteAllText($"Builds/Win64/{GetProjectName()}{i}/Debug.json", payload);
            }
        }
    }
#endif
}
