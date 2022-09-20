using System.Collections.Generic;

[System.Serializable]
public class MicVO : VO
{
    public float[] voiceData;

    public MicVO()
    {

    }

    public MicVO(float[] data)
    {
        voiceData = data;
    }
}
