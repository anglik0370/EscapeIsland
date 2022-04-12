using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilClass
{
    /// <summary>
    /// 투명한 색입니다
    /// </summary>
    public static Color limpidityColor = new Color(0, 0, 0, 0);
    /// <summary>
    /// 흰색입니다
    /// </summary>
    public static Color opacityColor = new Color(1, 1, 1, 1);

    /// <summary>
    /// Percent를 입력받아 확률을 계산합니다
    /// </summary>
    /// <param name="percent">확률</param>
    /// <returns>확률에 따른 성공, 실패를 반환합니다</returns>
    public static bool GetResult(float percent)
    {
        if (percent < 1)
        {
            percent = 1;
        }

        percent = percent / 100;

        bool result = false;
        int randAccuracy = 100;
        float randHitRange = percent * randAccuracy;
        int rand = UnityEngine.Random.Range(1, randAccuracy + 1);
        if (rand <= randHitRange)
        {
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 캔버스 그룹을 컨트롤하기 편하게 해주는 함수입니다
    /// (기본은 끄는 기능)
    /// </summary>
    /// <param name="cvs">컨트롤할 캔버스 그룹</param>
    /// <param name="alpha">투명도</param>
    /// <param name="blockRaycasts">BlockRaycast 여부</param>
    /// <param name="interactable">상호작용 가능 여부</param>
    public static void SetCanvasGroup(CanvasGroup cvs, float alpha = 0, bool blockRaycasts = false, bool interactable = false)
    {
        cvs.alpha = alpha;
        cvs.blocksRaycasts = blockRaycasts;
        cvs.interactable = interactable;
    }
}
