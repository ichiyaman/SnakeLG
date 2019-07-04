using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tail
/// </summary>
public class Tail : MonoBehaviour
{
    enum Stat
    {
        None,
        Wait,
        Move,
    }
    
    public GameObject objTarget;
    
    public float MoveSpeed = 1.0f;    
    public float WaitTime = 0.05f;    
    
    // ---- private ----
    private const float TimeMove = 0.2f;
    private Vector3 _posStart; 
    private Vector3 _posEnd;
    private float _timeStart;
    private float _timeNextMove;

    private delegate void MyUpdate();
    private MyUpdate _myUpdate = () => {};
    private int numGen = 0 ; 
    
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
                _myUpdate = () => {};
                break;
            case Stat.Wait:
                //_timeNextMove = Time.time + WaitTime * numGen ; 
                _timeNextMove = 0 ; 
                _myUpdate = UpdateWait ;
                break;
            case Stat.Move:
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
        float t = (Time.time - _timeStart) / TimeMove ;
		
        if (t >= 1.0f)
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
    void SetNextTarget()
    {
        _timeStart = Time.time;
        _posStart = transform.position;
        _posEnd = objTarget.transform.position;
    }

    /// <summary>
    /// SetTargetObj
    /// </summary>
    /// <param name="obj"></param>
    public void SetTargetObj(GameObject obj,int num)
    {
        objTarget = obj;
        numGen = num; 
    }
}
