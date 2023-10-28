using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PoolRoomData : MonoBehaviourPunCallbacks
{
    //TWO
    public void AddPlayerPointForTwo(int playerOnePoints, int playerTwoPoints)
    {
        int[] currentPoints = getPlayerPointsFor2();

        PhotonNetwork.CurrentRoom.SetPlayerOnePoints(currentPoints[0] + playerOnePoints);
        PhotonNetwork.CurrentRoom.SetPlayerTwoPoints(currentPoints[1] + playerTwoPoints);
    }
    public int[] getPlayerPointsFor2()
    {
        int[] currentPoints = new int[] 
        { 
            PhotonNetwork.CurrentRoom.GetPlayerOnePoints(),
            PhotonNetwork.CurrentRoom.GetPlayerTwoPoints() 
        };

        return currentPoints;
    }


    //THREE
    public void AddPlayerPointForThree(int playerOnePoints, int playerTwoPoints, int playerThreePoints)
    {
        int[] currentPoints = getPlayerPointsFor3();

        PhotonNetwork.CurrentRoom.SetPlayerOnePoints(currentPoints[0] + playerOnePoints);
        PhotonNetwork.CurrentRoom.SetPlayerThreePoints(currentPoints[1] + playerTwoPoints);
        PhotonNetwork.CurrentRoom.SetPlayerThreePoints(currentPoints[2] + playerThreePoints);
    }
    public int[] getPlayerPointsFor3()
    {
        int[] currentPoints = new int[] 
        { 
            PhotonNetwork.CurrentRoom.GetPlayerOnePoints(),
            PhotonNetwork.CurrentRoom.GetPlayerTwoPoints(),
            PhotonNetwork.CurrentRoom.GetPlayerThreePoints() 
        };

        return currentPoints;
    }



    //FOUR
    public void AddPlayerPointForFour(int playerOnePoints, int playerTwoPoints, int playerThreePoints, int playerFourPoints)
    {
        int[] currentPoints = getPlayerPointsFor4();

        PhotonNetwork.CurrentRoom.SetPlayerOnePoints(currentPoints[0] + playerOnePoints);
        PhotonNetwork.CurrentRoom.SetPlayerTwoPoints(currentPoints[1] + playerTwoPoints);
        PhotonNetwork.CurrentRoom.SetPlayerThreePoints(currentPoints[2] + playerThreePoints);
        PhotonNetwork.CurrentRoom.SetPlayerFourPoints(currentPoints[3] + playerFourPoints);
    }
    public int[] getPlayerPointsFor4()
    {
        int[] currentPoints = new int[]
        {
            PhotonNetwork.CurrentRoom.GetPlayerOnePoints(),
            PhotonNetwork.CurrentRoom.GetPlayerTwoPoints(),
            PhotonNetwork.CurrentRoom.GetPlayerThreePoints(),
            PhotonNetwork.CurrentRoom.GetPlayerFourPoints()
        };

        return currentPoints;
    }

    //Five
    public void AddPlayerPointForFive(int playerOnePoints, int playerTwoPoints, int playerThreePoints, int playerFourPoints, int playerFivePoints)
    {
        int[] currentPoints = getPlayerPointsFor5();

        PhotonNetwork.CurrentRoom.SetPlayerOnePoints(currentPoints[0] + playerOnePoints);
        PhotonNetwork.CurrentRoom.SetPlayerTwoPoints(currentPoints[1] + playerTwoPoints);
        PhotonNetwork.CurrentRoom.SetPlayerThreePoints(currentPoints[2] + playerThreePoints);
        PhotonNetwork.CurrentRoom.SetPlayerFourPoints(currentPoints[3] + playerFourPoints);
        PhotonNetwork.CurrentRoom.SetPlayerFivePoints(currentPoints[4] + playerFivePoints);
    }
    public int[] getPlayerPointsFor5()
    {
        int[] currentPoints = new int[]
        {
            PhotonNetwork.CurrentRoom.GetPlayerOnePoints(),
            PhotonNetwork.CurrentRoom.GetPlayerTwoPoints(),
            PhotonNetwork.CurrentRoom.GetPlayerThreePoints(),
            PhotonNetwork.CurrentRoom.GetPlayerFourPoints(),
            PhotonNetwork.CurrentRoom.GetPlayerFivePoints()
        };

        return currentPoints;
    }



    //SIX
    public void AddPlayerPointForSix(int playerOnePoints, int playerTwoPoints, int playerThreePoints, int playerFourPoints, int playerFivePoints, int playerSixPoints)
    {
        int[] currentPoints = getPlayerPointsFor6();

        PhotonNetwork.CurrentRoom.SetPlayerOnePoints(currentPoints[0] + playerOnePoints);
        PhotonNetwork.CurrentRoom.SetPlayerTwoPoints(currentPoints[1] + playerTwoPoints);
        PhotonNetwork.CurrentRoom.SetPlayerThreePoints(currentPoints[2] + playerThreePoints);
        PhotonNetwork.CurrentRoom.SetPlayerFourPoints(currentPoints[3] + playerFourPoints);
        PhotonNetwork.CurrentRoom.SetPlayerFivePoints(currentPoints[4] + playerFivePoints);
        PhotonNetwork.CurrentRoom.SetPlayerSixPoints(currentPoints[5] + playerSixPoints);
    }
    public int[] getPlayerPointsFor6()
    {
        int[] currentPoints = new int[]
        {
             PhotonNetwork.CurrentRoom.GetPlayerOnePoints(),
            PhotonNetwork.CurrentRoom.GetPlayerTwoPoints(),
            PhotonNetwork.CurrentRoom.GetPlayerThreePoints(),
            PhotonNetwork.CurrentRoom.GetPlayerFourPoints(),
            PhotonNetwork.CurrentRoom.GetPlayerFivePoints(), 
            PhotonNetwork.CurrentRoom.GetPlayerSixPoints()
        };

        return currentPoints;
    }


    //getplayerpoints
    public int[] GetPlayerFinalPoints(int totalPlayers)
    {
        
        switch (totalPlayers)
        {
            case 2:
               return getPlayerPointsFor2();     
            case 3:
                return getPlayerPointsFor3();              
            case 4:
                return getPlayerPointsFor4();                
            case 5:
                return getPlayerPointsFor5();          
            case 6:
                return getPlayerPointsFor6();
            default:
                return null;
        }
    
    }
}

public static class PoolRoomDataExtension
{
    public static readonly string PLAYER_one_PROP_KEY = "playerOne";
    public static readonly string PLAYER_two_PROP_KEY = "playerTwo";
    public static readonly string PLAYER_three_PROP_KEY = "playerThree";
    public static readonly string PLAYER_four_PROP_KEY = "playerFour";
    public static readonly string PLAYER_five_PROP_KEY = "playerFive";
    public static readonly string PLAYER_six_PROP_KEY = "playerSix";


    //PLAYER ONE
    public static void SetPlayerOnePoints(this Room room, int pointsToBeAdded)
    {
        if (room == null || room.CustomProperties == null)
        {
            return;
        }

        ExitGames.Client.Photon.Hashtable turnProps = new ExitGames.Client.Photon.Hashtable();
        turnProps[PLAYER_one_PROP_KEY] = pointsToBeAdded;

        room.SetCustomProperties(turnProps);
    }
    public static int GetPlayerOnePoints(this RoomInfo room)
    {
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(PLAYER_one_PROP_KEY))
        {
            return 0;
        }

        return (int)room.CustomProperties[PLAYER_one_PROP_KEY];
    }


    //PLAYER TWO
    public static void SetPlayerTwoPoints(this Room room, int pointsToBeAdded)
    {
        if (room == null || room.CustomProperties == null)
        {
            return;
        }

        ExitGames.Client.Photon.Hashtable turnProps = new ExitGames.Client.Photon.Hashtable();
        turnProps[PLAYER_two_PROP_KEY] = pointsToBeAdded;

        room.SetCustomProperties(turnProps);
    }
    public static int GetPlayerTwoPoints(this RoomInfo room)
    {
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(PLAYER_two_PROP_KEY))
        {
            return 0;
        }

        return (int)room.CustomProperties[PLAYER_two_PROP_KEY];
    }


    //PLAYER THREE
    public static void SetPlayerThreePoints(this Room room, int pointsToBeAdded)
    {
        if (room == null || room.CustomProperties == null)
        {
            return;
        }

        ExitGames.Client.Photon.Hashtable turnProps = new ExitGames.Client.Photon.Hashtable();
        turnProps[PLAYER_three_PROP_KEY] = pointsToBeAdded;

        room.SetCustomProperties(turnProps);
    }
    public static int GetPlayerThreePoints(this RoomInfo room)
    {
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(PLAYER_three_PROP_KEY))
        {
            return 0;
        }

        return (int)room.CustomProperties[PLAYER_three_PROP_KEY];
    }


    //PLAYER FOUR
    public static void SetPlayerFourPoints(this Room room, int pointsToBeAdded)
    {
        if (room == null || room.CustomProperties == null)
        {
            return;
        }

        ExitGames.Client.Photon.Hashtable turnProps = new ExitGames.Client.Photon.Hashtable();
        turnProps[PLAYER_four_PROP_KEY] = pointsToBeAdded;

        room.SetCustomProperties(turnProps);
    }
    public static int GetPlayerFourPoints(this RoomInfo room)
    {
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(PLAYER_four_PROP_KEY))
        {
            return 0;
        }

        return (int)room.CustomProperties[PLAYER_four_PROP_KEY];
    }


    //PLAYER FIVE
    public static void SetPlayerFivePoints(this Room room, int pointsToBeAdded)
    {
        if (room == null || room.CustomProperties == null)
        {
            return;
        }

        ExitGames.Client.Photon.Hashtable turnProps = new ExitGames.Client.Photon.Hashtable();
        turnProps[PLAYER_five_PROP_KEY] = pointsToBeAdded;

        room.SetCustomProperties(turnProps);
    }
    public static int GetPlayerFivePoints(this RoomInfo room)
    {
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(PLAYER_five_PROP_KEY))
        {
            return 0;
        }

        return (int)room.CustomProperties[PLAYER_five_PROP_KEY];
    }


    //PLAYER SIX
    public static void SetPlayerSixPoints(this Room room, int pointsToBeAdded)
    {
        if (room == null || room.CustomProperties == null)
        {
            return;
        }

        ExitGames.Client.Photon.Hashtable turnProps = new ExitGames.Client.Photon.Hashtable();
        turnProps[PLAYER_six_PROP_KEY] = pointsToBeAdded;

        room.SetCustomProperties(turnProps);
    }
    public static int GetPlayerSixPoints(this RoomInfo room)
    {
        if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(PLAYER_six_PROP_KEY))
        {
            return 0;
        }

        return (int)room.CustomProperties[PLAYER_six_PROP_KEY];
    }
}
