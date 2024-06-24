using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateManager
{
    public enum State
    {
        StagePlay,
        StageClear,
        Option,        
        Title
    }

    public enum PlayState
    {
        Alive,
        Death,

    }

    public static int lank;
    public static bool isDeath;
    public static bool isPortal;
    public static bool isBossroomPortal;
    public static State gameState;
    public static PlayState playerState;
    public static StageManager stageManage;
    public static eStage stage;

    public enum eStage
    {
        Title,
        Town,
        Stage1,
        Stage2,

    }

    public static void ChagePlayerState(PlayState state)
    {
        playerState = state;
    }

    public static void ChageState(State state)
    {
        gameState = state;
    }
}
