using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UtilClass
{
    /// <summary>
    /// ������ ���Դϴ�
    /// </summary>
    public static Color limpidityColor = new Color(1, 1, 1, 0);
    /// <summary>
    /// ����Դϴ�
    /// </summary>
    public static Color opacityColor = new Color(1, 1, 1, 1);

    public static Color guideColor = new Color(1, 1, 1, 0.6f);

    public static readonly string HOST_TEXT = "HOST";
    public static readonly string READY_TEXT = "READY!";

    /// <summary>
    /// Percent�� �Է¹޾� Ȯ���� ����մϴ�
    /// </summary>
    /// <param name="percent">Ȯ��</param>
    /// <returns>Ȯ���� ���� ����, ���и� ��ȯ�մϴ�</returns>
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
    /// ĵ���� �׷��� ��Ʈ���ϱ� ���ϰ� ���ִ� �Լ��Դϴ�
    /// (�⺻�� ���� ���)
    /// </summary>
    /// <param name="cvs">��Ʈ���� ĵ���� �׷�</param>
    /// <param name="alpha">����</param>
    /// <param name="blockRaycasts">BlockRaycast ����</param>
    /// <param name="interactable">��ȣ�ۿ� ���� ����</param>
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
    /// �� ���� �������� ������ �־��� ���� �� �� ���� ������ ��ȯ�մϴ�
    /// </summary>
    /// <param name="beginPoint">������ 1</param>
    /// <param name="endPoint">���� 1</param>
    /// <param name="beginDragPoint">������ 2</param>
    /// <param name="endDragPoint">���� 2</param>
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
    /// ������ �������� ���� ���̿� �ִ����� ��ȯ�մϴ�
    /// </summary>
    /// <param name="intersection">����</param>
    /// <param name="beginPoint">������</param>
    /// <param name="endPoint">����</param>
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
