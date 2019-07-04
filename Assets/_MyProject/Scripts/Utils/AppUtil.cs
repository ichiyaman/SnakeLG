using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TouchInfo
/// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
/// Multi touchはほぼ意識してないので注意
/// https://qiita.com/tempura/items/4a5482ff6247ec8873df
/// </summary>
// この enum は値かえちゃいやーんよ
public enum TouchInfo
{
    None = 99, // タッチなし 
    Began = 0, // タッチ開始
    Moved = 1, // タッチ移動 
    Stationary = 2, // タッチ静止
    Ended = 3, // タッチ終了 
    Canceled = 4, // タッチキャンセル
}

/// <summary>
/// AppUtil
/// </summary>
public static class AppUtil
{
    private static Vector3 TouchPosition = Vector3.zero;

    /// <summary>
    /// タッチ情報を取得(エディタと実機を考慮)
    /// </summary>
    /// <returns>タッチ情報。タッチされていない場合は null</returns>
    public static TouchInfo GetTouch(int index)
    {
        #if UNITY_EDITOR
        
        // Input Classの中は propertyになってると思うが未確認
        if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
        if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
        if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
        
        #else
        
        if (Input.touchCount > 0)
        {
            return (TouchInfo)((int)Input.GetTouch(index).phase);
        }
        
        #endif
        return TouchInfo.None;
    }

    /// <summary>
    /// タッチポジションを取得(エディタと実機を考慮)
    ///   Editor字は 1 タッチでしか動作不能
    /// </summary>
    /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetTouchPosition(int index)
    {
        #if UNITY_EDITOR

        TouchInfo touch = GetTouch(index);
        if (touch != TouchInfo.None) { return Input.mousePosition; }
            
        #else            
            
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(index);
            TouchPosition.x = touch.position.x;
            TouchPosition.y = touch.position.y;
            return TouchPosition;
        }
        #endif
        return Vector3.zero;
    }

    /// <summary>
    /// タッチワールドポジションを取得(エディタと実機を考慮)
    /// </summary>
    /// <param name='camera'>カメラ</param>
    /// <returns>タッチワールドポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetTouchWorldPosition(Camera camera)
    {
        return camera.ScreenToWorldPoint(GetTouchPosition(0));
    }


    /// <summary>
    /// MakeSeq
    ///  指定された数の数列を返す
    /// </summary>
    /// <param name="max"></param>
    /// <returns></returns>
    public static List<int> MakeSeq(int max)
    {
        List<int> listRet = new List<int>();
        int i;

        for (i = 0; i < max; i++)
        {
            listRet.Add(i);
        }
        return listRet; 
    }
}
