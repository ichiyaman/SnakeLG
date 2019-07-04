using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// Snake1 (ベジェ曲線移動version)
///   1. 出現 
///   2. 次のTargetを決定
/// 　3. 次のTargetまで移動
/// 　4. Goto -> 2.
/// </summary>
public class Snake1 : MonoBehaviour
{
    enum Stat
    {
        None,
        Init,
        Wait,
        Move,
    }
    
    // ---- private ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- ----- 
    private const int TailLong = 100;   
    private int _iPoint;  // 現在のポイントの index

    private List<Vector3> _listPos ; 
    
    private float _timeStart;
    private float _timeMove; 
    private float _timeNextMove;
    
    private delegate void MyUpdate();
    private MyUpdate _myUpdate = () => { };
    private ParticleSystem _particle;
    
    // Start is called before the first frame update
    void Start()
    {
        _particle = GetComponentInChildren<ParticleSystem>();
        Debug.Assert(_particle!=null, "_particle!=null");
        _particle.Stop();
        
        ChangeState(Stat.Init);
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
            case Stat.Init:
                _myUpdate = () => { };
                SetupTargetPos();
                ChangeState(Stat.Wait);
                break;
            case Stat.Wait:
                _particle.Play();
                CoroutineUtils.CallWaitForSeconds(1.0f , () => { 
                    _particle.Stop();
                    Debug.Log("Stop");
                });
                
                _timeNextMove = Time.time + Random.Range(SnakeManager.Instance.TimeWaitMin,SnakeManager.Instance.TimeWaitMax);
                Debug.Log("wait:" + (_timeNextMove - Time.time) + ":" + SnakeManager.Instance.TimeWaitMax); 
                _myUpdate = UpdateWait ;
                break;
            case Stat.Move:
                _timeMove = Random.Range(SnakeManager.Instance.TimeMoveMin,SnakeManager.Instance.TimeMoveMax);
                Debug.Log("move:" + _timeMove); 
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
            transform.position = GetPointBezier(t);
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
        int i;

        for (i = 0; i < 3; i++)
        {
            _listPos.Shift();
        }

        for (i = 0; i < 3; i++)
        {
            _listPos.Add(SnakeManager.Instance.GetNextPoint());
        }

        // _listPos.ForEach( x => { Debug.Log("pos:" + x); } ); //  遅いよ
    }

    /// <summary>
    /// 初回のみ4つ持ってくる
    /// </summary>
    public void SetupTargetPos()
    {
        _listPos = new List<Vector3>();

        for (int i = 0; i < 4; i++)
        {
            _listPos.Add(SnakeManager.Instance.GetNextPoint() ) ;
        }
        
       // _listPos.ForEach( x => { Debug.Log("pos:" + x); } ); //  遅いよ
    }

    /// <summary>
    /// GetPointBezier
    /// 　ベジェ曲線 計算用
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    Vector3 GetPointBezier(float t)
    {
        var oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * _listPos[0] +
               3f * oneMinusT * oneMinusT * t * _listPos[1] +
               3f * oneMinusT * t * t * _listPos[2] +
               t * t * t * _listPos[3];
    }    
}
