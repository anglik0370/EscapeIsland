using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager
{
    private static Action<Player> EnterRoom = p => { };
    private static Action<Player> GameStart = p => { };
    private static Action BackToRoom = () => { };
    private static Action ExitRoom = () => { };
    private static Action<MeetingType> StartMeet = type => { };

    public static void SubStartMeet(Action<MeetingType> Callback)
    {
        StartMeet += Callback;
    }

    public static void OccurStartMeet(MeetingType type)
    {
        StartMeet?.Invoke(type);
    }

    public static void SubBackToRoom(Action Callback)
    {
        BackToRoom += Callback;
    }

    public static void OccurBackToRoom()
    {
        BackToRoom?.Invoke();
    }

    public static void SubExitRoom(Action Callback)
    {
        ExitRoom += Callback;
    }

    public static void OccurExitRoom()
    {
        ExitRoom?.Invoke();
    }

    public static void SubEnterRoom(Action<Player> Callback)
    {
        EnterRoom += Callback;
    }

    public static void OccurEnterRoom(Player p)
    {
        EnterRoom?.Invoke(p);
    }

    public static void SubGameStart(Action<Player> Callback)
    {
        GameStart += Callback;
    }

    public static void OccurGameStart(Player p)
    {
        GameStart?.Invoke(p);
    }
}
