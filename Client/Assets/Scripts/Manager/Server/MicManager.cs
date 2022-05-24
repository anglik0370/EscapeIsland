using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicManager : ISetAble
{
    public static MicManager Instance { get; private set; }

    private const int FREQUENCY = 44100;

    private AudioClip sendingClip = null;

    private AudioSource audioSource;

    private string device = "";

    private bool isRecording = false;

    private int lastSample = 0;
    private float loudness = 0;
    [SerializeField]
    private float sensitivity = 100;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();

        Application.RequestUserAuthorization(UserAuthorization.Microphone);

        MicOnOff(true);
    }

    private void FixedUpdate()
    {
        //recording중이더라도 일정 크기이상의 소리가 날때만 보내줘야함
        if (isRecording)
        {
            loudness = GetAveragedVolume(audioSource.clip.channels) * sensitivity;

            int pos = Microphone.GetPosition(device);
            int diff = pos - lastSample;

            if (diff > 0 && loudness > 7)
            {
                float[] samples = new float[diff * audioSource.clip.channels];
                audioSource.clip.GetData(samples, lastSample);
                //byte[] ba = ToByteArray(samples);

                print($"send");

                MicVO vo = new MicVO(samples);
                DataVO dataVO = new DataVO("VOICE", JsonUtility.ToJson(vo));

                SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
            }
            lastSample = pos;

        }
    }

    public void SetMic()
    {
        isRecording = true;
    }

    public void MicOnOff(bool on)
    {
        if(Microphone.devices.Length > 0)
        {
            device = Microphone.devices[0];

            if(on)
            {
                audioSource.clip = Microphone.Start(device, true, 100, FREQUENCY);
                audioSource.loop = true;
                audioSource.mute = false;

                while (!(Microphone.GetPosition(null) > 0)) { }
                audioSource.Play();

            }
            else
            {
                Microphone.End(device);
                audioSource.clip = null;
            }
        }
        else
        {
            print("asd");
        }

        isRecording = on;
    }

    private float GetAveragedVolume(int ch)
    {
        float[] data = new float[256];
        float a = 0;
        audioSource.GetOutputData(data, 0);

        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }

        return a / 256;
    }

    public byte[] ToByteArray(float[] floatArray)
    {
        int len = floatArray.Length * 4;
        byte[] byteArray = new byte[len];
        int pos = 0;
        foreach (float f in floatArray)
        {
            byte[] data = System.BitConverter.GetBytes(f);
            System.Array.Copy(data, 0, byteArray, pos, 4);
            pos += 4;
        }
        return byteArray;
    }

    public float[] ToFloatArray(byte[] byteArray)
    {
        int len = byteArray.Length / 4;
        float[] floatArray = new float[len];
        for (int i = 0; i < byteArray.Length; i += 4)
        {
            floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
        }
        return floatArray;
    }

    public AudioClip GetClip(float[] floatArr)
    {
        return AudioClip.Create("", floatArr.Length, 0, FREQUENCY, false);
    }
}
