using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicManager : ISetAble
{
    //public static MicManager Instance { get; private set; }

    //private int Frequency => AudioSettings.outputSampleRate;

    //private AudioSource audioSource;

    //private string device = "";

    //private bool isRecording = false;

    //private int lastSample = 0;

    //private void Awake()
    //{
    //    Instance = this;

    //    audioSource = GetComponent<AudioSource>();
    //}

    //protected override void Start()
    //{
    //    base.Start();

    //    Application.RequestUserAuthorization(UserAuthorization.Microphone);

    //    MicOnOff(true);
    //}

    //private void FixedUpdate()
    //{
    //    if (isRecording)
    //    {
    //        int pos = Microphone.GetPosition(device);
    //        int diff = pos - lastSample;

    //        if(diff > 0)
    //        {
    //            float[] samples = new float[diff * audioSource.clip.channels];
    //            audioSource.clip.GetData(samples, lastSample);
    //            //byte[] ba = ToByteArray(samples);

    //            MicVO vo = new MicVO(samples);
    //            DataVO dataVO = new DataVO("VOICE", JsonUtility.ToJson(vo));

    //            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    //        }
                
    //        lastSample = pos;

    //    }
    //}

    //private void OnDestroy()
    //{
    //    MicOnOff(false);
    //}

    //private void OnApplicationQuit()
    //{
    //    MicOnOff(false);
    //}

    //public void SetMic()
    //{
    //    isRecording = true;
    //}

    //public void MicOnOff(bool on)
    //{
    //    if(Microphone.devices.Length > 0)
    //    {
    //        device = Microphone.devices[0];

    //        if(on)
    //        {
    //            audioSource.clip = Microphone.Start(device, true, 10, Frequency);
    //            audioSource.loop = true;
    //            audioSource.mute = false;

    //            while (!(Microphone.GetPosition(null) > 0)) { }
    //            audioSource.Play();

    //        }
    //        else
    //        {
    //            Microphone.End(device);
    //            audioSource.clip = null;
    //        }
    //    }
    //    else
    //    {
    //        print("asd");
    //        UIManager.Instance.AlertText("x", AlertType.Warning);
    //    }

    //    isRecording = on;
    //}

    //public byte[] ToByteArray(float[] floatArray)
    //{
    //    int len = floatArray.Length * 4;
    //    byte[] byteArray = new byte[len];
    //    int pos = 0;
    //    foreach (float f in floatArray)
    //    {
    //        byte[] data = System.BitConverter.GetBytes(f);
    //        System.Array.Copy(data, 0, byteArray, pos, 4);
    //        pos += 4;
    //    }
    //    return byteArray;
    //}

    //public float[] ToFloatArray(byte[] byteArray)
    //{
    //    int len = byteArray.Length / 4;
    //    float[] floatArray = new float[len];
    //    for (int i = 0; i < byteArray.Length; i += 4)
    //    {
    //        floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
    //    }
    //    return floatArray;
    //}

    //public AudioClip GetClip(float[] floatArr)
    //{
    //    AudioClip clip = AudioClip.Create("", floatArr.Length, 1, Frequency, false);
    //    clip.SetData(floatArr, 0);
    //    return clip;
    //}
}
