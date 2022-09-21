using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UtilClass
{
    /// <summary>
    /// 투명한 색입니다
    /// </summary>
    public static Color limpidityColor = new Color(1, 1, 1, 0);
    /// <summary>
    /// 흰색입니다
    /// </summary>
    public static Color opacityColor = new Color(1, 1, 1, 1);

    public static Color guideColor = new Color(1, 1, 1, 0.6f);

    public static readonly string HOST_TEXT = "HOST";
    public static readonly string READY_TEXT = "READY!";

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
    public static void SetCanvasGroup(CanvasGroup cvs, float alpha = 0, bool blockRaycasts = false, bool interactable = false, bool isTweenSkip = true)
    {
        if(isTweenSkip)
        {
            cvs.alpha = alpha;
            cvs.blocksRaycasts = blockRaycasts;
            cvs.interactable = interactable;
        }
        else
        {
            cvs.DOFade(alpha, 0.5f).OnComplete(()=>
            {
                cvs.blocksRaycasts = blockRaycasts;
                cvs.interactable = interactable;
            });
        }
    }

    /// <summary>
    /// 두 선의 시작점과 끝점이 주어져 있을 때 두 선의 교점을 반환합니다
    /// </summary>
    /// <param name="beginPoint">시작점 1</param>
    /// <param name="endPoint">끝점 1</param>
    /// <param name="beginDragPoint">시작점 2</param>
    /// <param name="endDragPoint">끝점 2</param>
    /// <returns></returns>
    public static Vector2 GetInterSection(Vector2 beginPoint, Vector2 endPoint, Vector3 beginDragPoint, Vector3 endDragPoint)
    {
        float m1 = (endPoint.y - beginPoint.y) / (endPoint.x - beginPoint.x);
        float m2 = (endDragPoint.y - beginDragPoint.y) / (endDragPoint.x - beginDragPoint.x);

        float x = (beginDragPoint.y - beginPoint.y + (m1 * beginPoint.x) - (m2 * beginDragPoint.x)) / (m1 - m2);
        float y = (m1 * x) - (m1 * beginPoint.x) + beginPoint.y;

        return new Vector2(x, y);
    }

    /// <summary>
    /// 교점이 시작점과 끝점 사이에 있는지를 반환합니다
    /// </summary>
    /// <param name="intersection">교점</param>
    /// <param name="beginPoint">시작점</param>
    /// <param name="endPoint">끝점</param>
    /// <returns></returns>
    public static bool CheckIntersectionInRange(Vector2 intersection, Vector2 beginPoint, Vector2 endPoint)
    {
        bool isHorizontal = (endPoint.x - beginPoint.x) > (endPoint.y - endPoint.y);

        if (isHorizontal)
        {
            return !(intersection.x < beginPoint.x || intersection.x > endPoint.x);
        }
        else
        {
            return !(intersection.y < beginPoint.y || intersection.y > endPoint.y);
        }
    }

    public static void ResolutionFix(int width, int height)
    {
        Camera camera = Camera.main;
        Rect rect = camera.rect;

        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)width / height);
        float scaleWidth = 1f / scaleHeight;

        if(scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        camera.rect = rect;
    }

    public static Color GetStateColor(bool isBuff)
    {
        return isBuff ? Color.blue : Color.red;
    }

    public  static Color GetTeamColor(Team team)
    {
        switch (team)
        {
            case Team.RED:
                return Color.red;
            case Team.BLUE:
                return Color.blue;
        }

        return Color.black;
    }
}
