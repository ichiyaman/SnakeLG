using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// SnakeManager 
/// </summary>
public class SnakeManager : SingletonMonoBehaviourFast<SnakeManager>
{
    enum ModePointList
    {
        Auto,
        Custom
    }
    
    public float TimeMoveMin = 3.0f; 
    public float TimeMoveMax = 6.0f; 
    public float TimeWaitMin = 0.0f; 
    public float TimeWaitMax = 1.0f;
    
    public int TailLong = 20; 
    
    //public const int numPoint; 
    
    public Vector3 vecBox;     
    public Vector3 posCenter;

    // ---- private ---- ---- ---- ---- ---- ---- ----
    private const int DivXAuto = 5 ; 
    private const int DivYAuto = 5 ; 
    private const int DivZAuto = 2 ; 
    
    [SerializeField] private ModePointList mode; 
    [SerializeField] private GameObject _pfSnake = null ; 
    [SerializeField] private GameObject _pfTail = null ; 
    
    [SerializableAttribute]
    public class SnakePoint {
        public List<Vector3> List = new List<Vector3>();

        public SnakePoint(List<Vector3>  list){
            List = list;
        }
    }

//Inspectorに表示される
    [SerializeField]
    private SnakePoint _objSnakePoint = new SnakePoint(new List<Vector3>());
    
    private List<Vector3> _listNextSnakePoint = null ; 
    
    void Start()
    {
        if (mode == ModePointList.Auto)
        {
            _objSnakePoint.List.Clear(); 

            for (int z = 0; z < DivZAuto; z++)
            {
                for (int y = 0; y < DivYAuto; y++)
                {
                    for (int x = 0; x < DivXAuto; x++)
                    {
                        _objSnakePoint.List.Add(new Vector3( 1.0f / (x+1) -0.5f,1.0f / (y+1) -0.5f,1.0f / (z+1)  -0.5f ));
                    }
                }
            }
        }
        else
        {
            Debug.Assert(_objSnakePoint!=null, "_objSnakePoint!=null");
            Debug.Assert(_pfSnake != null, "_pfSnake != null");
            
        }

        MakeNextPoint();

        _listNextSnakePoint.ForEach(x => { Debug.Log(x);});

        SpawnSnake();

    }

    void Update()
    {
        
        
    }

    /// <summary>
    /// GetNextPoint
    ///   次の移動ポイントを取得
    /// </summary>
    /// <param name="iExplict">除外する番号</param>
    /// <returns></returns>
    public Vector3 GetNextPoint()
    {
        Vector3 ret; 
        ret = _listNextSnakePoint.Shift() ;
        if (_listNextSnakePoint.Count <= 0)
        {
            _listNextSnakePoint.Clear();
            MakeNextPoint();
        }
        return ret * 4f ; 
    }

    /// <summary>
    /// MakeNextPoint
    /// </summary>
    public void MakeNextPoint()
    {
        _listNextSnakePoint = new List<Vector3>(_objSnakePoint.List );
        _listNextSnakePoint.Shuffle();
    }

    /// <summary>
    /// SpawnSnake
    /// </summary>
    public void SpawnSnake()
    {
        GameObject objLast;
        Vector3 pos = GetNextPoint();
        GameObject snake = Instantiate(_pfSnake, pos, Quaternion.identity);
        // snake.GetComponent<Snake>().SetNextTarget();  << Snakeで必要

        objLast = snake; 

        // Add tail
        for (int i = 0; i < TailLong; i++)
        {
            GameObject tail = Instantiate(_pfTail, pos, Quaternion.identity);
            tail.GetComponent<Tail>().SetTargetObj(objLast, i+1);
            objLast = tail; 
        }
    }
}
