using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager
{
    private static Action<Player> EnterRoom = p => { };
    private static Action<Player> GameStart = p => { };
    private static Action ExitRoom = () => { };

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
