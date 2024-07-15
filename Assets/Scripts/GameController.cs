using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colorstate;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    //记录了从起始位置开始的偏移
    public float Xoffset = 0;
    public float Yoffset = 0;
    public static float X_Offset = 0;
    public static float Y_Offset = 0;

    //记录了每个单元格大小
    public float Unit_X = 1;
    public float Unit_Y = 1;

    //记录了场景大小
    public int Height = 10;
    public int Width = 10;
    public static int rowNum = 10;//列数
    public static int colNum = 10;//行数

    //记录所有要生成方块的数组,生成的位置
    public GameObject[] FallingBlocks;
    public float FallingPositin_X = 0;
    public float FallingPositin_Y = 0;

    //存放物体
    public static Transform[,] Grid ;

    private List<List<ColorState>> m_GameManager;
    private List<List<bool>> m_CancelManager;

    //游戏运行的参数
    public float CD = 3;//落地后三秒产生下一个


    public static bool Isfallen = false;
    private float m_timer = 0;
    void Start()
    {
        X_Offset = Xoffset;
        Y_Offset = Yoffset;
        colNum = Height;
        rowNum = Width;
        Grid = new Transform[rowNum, colNum];
        //首先初始化删除控制数组和颜色记录（游戏管理）数组
        m_CancelManager = new List<List<bool>>();
        for(int row = 0;row < rowNum; row++)
        {
            List<bool> list = new List<bool>();
            for(int col = 0;col < colNum; col++)
            {
                list.Add(false);
            }
            m_CancelManager.Add(list);
        }

        m_GameManager = new List<List<ColorState>>();
        for (int row = 0; row < rowNum; row++)
        {
            List<ColorState> list = new List<ColorState>();
            for (int col = 0; col < colNum; col++)
            {
                list.Add(ColorState.None);
            }
            m_GameManager.Add(list);
        }

        //开始第一个生成
        RandomGenerateBlock();
    }

    // Update is called once per frame
    void Update()
    {
        //上一个已经落地
        if (Isfallen)
        {
            m_timer += Time.deltaTime;
            if(m_timer >= CD)//到达CD
            {
                RandomGenerateBlock();
                Isfallen=false;
                m_timer=0;
            }
        }
    }

    private void RandomGenerateBlock()
    {
        int index = Random.Range(0,FallingBlocks.Length);
        Instantiate(FallingBlocks[index],new Vector3(X_Offset+FallingPositin_X,Y_Offset+FallingPositin_Y,0),Quaternion.identity);

    }

    public static Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),Mathf.Round(v.y));
    }

    public static bool IsInside(Vector2 v)
    {
        return ((int)v.x >= X_Offset && (int)v.x <= X_Offset + rowNum -1 && (int)v.y >= Y_Offset);
    }
}
