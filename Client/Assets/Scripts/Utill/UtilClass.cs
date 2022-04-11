using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilClass
{
    public static Color limpidityColor = new Color(0, 0, 0, 0);
    public static Color opacityColor = new Color(1, 1, 1, 1);

    //최소 확률은 1%임
    public static bool GetRandomResult(float percent)
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
}
