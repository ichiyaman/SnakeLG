using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public static class Game
{
    public const int NumCounterHuman = 4 ; // カウンターに何人座れるか
    // ---- ゲームの状態(scene) ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
    public enum Stat
    {
        None,
        Init,
        Title,
        StartGame,
        InGame,
        Pause,
        Result,
        Quit,
#if RELEASE
#else
        Debug,
#endif
    }
   
#if RELEASE
    public const bool IsDebug = false; 
#else
    public const bool IsDebug = true;

    public enum DebugStart
    {
        None,               // 通常スタート
        Endress,            // Endress start
        TimeLimit,          // TimeLimit start
        Tutorial,           // Tutorial  
        RankingTimeLimit,   // TimeLimit start
        RankingTutorial,    // Tutorial  
    }

    public enum DebugStat
    {
        None,
        Normal,
        UI,                    
        Tool,                  
    }

    public enum DebugBit
    {
        None,
        Invincible,              // 無敵
        NoNavigation,            // navi子無効
        NoBgm,                   // Bgm無し
        NoSe,                    // Se無し
        FastHuman,               // 敵早回し (開発用)
        AllEnemy,                // 全員出てくる
        NoEnemy,                 // 敵出てこない
        NoThief,                 // 強盗出てこない
        Dying,                   // 瀕死(後１撃で死亡)
        NoSave,                  // Saveしない
        NoPlatform,              // Platformにアクセスしない
        NoPlayFab,               // PlayFabにアクセスしない
        NoMenu,                  // スタート時にメニューを表示しない
        NoXms,                   // クリスマス無し
        NoPsSetup,               // PS setup skip
        NoSceneLoad,             // No Scene Load
        ShowFps,                 // Show FPS
        NoTrophy,                // NoTrophy
        NoNetwork                // No net work connection
    }
#endif
}
