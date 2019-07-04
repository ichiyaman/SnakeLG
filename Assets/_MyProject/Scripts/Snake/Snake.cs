using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// Snake
///   1. 出現 
///   2. 次のTargetを決定
/// 　3. 次のTargetまで移動
/// 　4. Goto -> 2.
/// </summary>
public class Snake : MonoBehaviour
{
    enum Stat
    {
        None,
        Wait,
        Move,
    }
    
    // ---- private ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- 
    private const int TailLong = 5;   
    private int _iPoint;  // 現在のポイントの index

    private Vector3 _posStart; 
    private Vector3 _posEnd;
    private float _timeStart;
    private float _timeMove; 
    private float _timeNextMove;
    
    private delegate void MyUpdate();
    private MyUpdate _myUpdate = () => { }; 
    
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(Stat.Wait); 
    }

    // Update is called once per frame
    void Update()
    {
        _myUpdate();
    }

    /// <summary>
    /// ChangeState
    /// </summary>
    /// <param name="???"></param>
    void ChangeState(Stat stat)
    {
        switch (stat)
        {
            case Stat.None:
                _myUpdate = () => { };
                break;
            case Stat.Wait:
                _timeNextMove = Time.time + Random.Range(SnakeManager.Instance.TimeWaitMin,SnakeManager.Instance.TimeWaitMax);
                Debug.Log("wait:" + (_timeNextMove - Time.time) + ":" + SnakeManager.Instance.TimeWaitMax); 
                _myUpdate = UpdateWait ;
                break;
            case Stat.Move:
                _timeMove = Random.Range(SnakeManager.Instance.TimeMoveMin,SnakeManager.Instance.TimeMoveMax);
                _timeStart = Time.time; 
                SetNextTarget();
                _myUpdate = UpdateMove ;
                break;
        }
    }
    
    /// <summary>
    /// Move
    /// </summary>
    void UpdateMove()
    {
        float t = (Time.time - _timeStart) / _timeMove ;
        
        if (t > 1.0f)
        {
            ChangeState(Stat.Wait); 
        }
        else
        {
            transform.position = Vector3.Lerp(_posStart, _posEnd, t);
        }
    }

    /// <summary>
    /// Wait
    /// </summary>
    void UpdateWait()
    {
        if (Time.time > _timeNextMove)
        {
            ChangeState(Stat.Move); 
        }
    }    
    
    /// <summary>
    /// SetNextTargt()
    /// </summary>
    public void SetNextTarget()
    {
        _posStart = transform.position;
        _posEnd = SnakeManager.Instance.GetNextPoint();

        Debug.LogFormat("SetNextTarget {0} {1} {2} {3} ", _timeStart, _posStart, _posEnd, _timeMove ); 
    }
    
    
    
    
    
}
