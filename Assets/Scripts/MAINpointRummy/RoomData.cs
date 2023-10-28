using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class RoomData : MonoBehaviour
{
    public int Turn
    {
        get { return PhotonNetwork.CurrentRoom.GetTurn(); }
        set
        {

            PhotonNetwork.CurrentRoom.SetTurn(value, true);
        }
    }

    public float TurnDuration = 10f;

    public float ElapsedTimeInTurn
    {
        get { return ((float)(PhotonNetwork.ServerTimestamp - PhotonNetwork.CurrentRoom.GetTurnStart())) / 1000.0f; }
    }

    public float RemainingSecondsInTurn
    {
        get { return Mathf.Max(0f, this.TurnDuration - this.ElapsedTimeInTurn); }
    }

    public bool IsOver
    {
        get { return this.RemainingSecondsInTurn <= 0f; }
    }
}

public static class RoodDataExtension
{
    public static readonly string TURN_PROP_KEY = "Turn";
    public static readonly string TURN_START_PROP_KEY = "TStart";
    public static void SetTurn(this Room room, int turn, bool setStartTime = false)
    {
        if (room == null || room.CustomProperties == null)
        {
            return;
        }

        ExitGames.Client.Photon.Hashtable turnProps = new ExitGames.Client.Photon.Hashtable();
        turnProps[TURN_PROP_KEY] = turn;
        if (setStartTime)
        {
            turnProps[TURN_START_PROP_KEY] = PhotonNetwork.ServerTimestamp;
        }

        room.SetCustomProperties(turnProps);
    }

    public static int GetTurn(this RoomInfo room)
    {
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TURN_PROP_KEY))
        {
            return 0;
        }

        return (int)room.CustomProperties[TURN_PROP_KEY];
    }

    public static int GetTurnStart(this RoomInfo room)
    {
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TURN_START_PROP_KEY))
        {
            return 0;
        }

        return (int)room.CustomProperties[TURN_START_PROP_KEY];
    }
}
