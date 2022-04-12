using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilClass
{
    /// <summary>
    /// ������ ���Դϴ�
    /// </summary>
    public static Color limpidityColor = new Color(0, 0, 0, 0);
    /// <summary>
    /// ����Դϴ�
    /// </summary>
    public static Color opacityColor = new Color(1, 1, 1, 1);

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
    public static void SetCanvasGroup(CanvasGroup cvs, float alpha = 0, bool blockRaycasts = false, bool interactable = false)
    {
        cvs.alpha = alpha;
        cvs.blocksRaycasts = blockRaycasts;
        cvs.interactable = interactable;
    }
}
