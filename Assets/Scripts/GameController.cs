using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colorstate;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    //��¼�˴���ʼλ�ÿ�ʼ��ƫ��
    public float Xoffset = 0;
    public float Yoffset = 0;
    public static float X_Offset = 0;
    public static float Y_Offset = 0;

    //��¼��ÿ����Ԫ���С
    public float Unit_X = 1;
    public float Unit_Y = 1;

    //��¼�˳�����С
    public int Height = 10;
    public int Width = 10;
    public static int rowNum = 9;//����
    public static int colNum = 10;//����

    //��¼����Ҫ���ɷ��������,���ɵ�λ��
    public GameObject[] FallingBlocks;
    public float FallingPositin_X = 0;
    public float FallingPositin_Y = 0;

    //�������
    public static Transform[,] Grid ;

    public static ColorState[,] GameManager;
    public static bool[,] CancelManager;

    //��Ϸ���еĲ���
    public float CD = 30;//��غ����������һ��

    public int Point = 0;
    public int PointThree = 4;
    public int PointFour = 10;
    public int PointFive = 16;


    public static bool Isfallen = false;
    public float m_timer = 0;

    private bool m_isClear = false;
    void Start()
    {
        X_Offset = Xoffset;
        Y_Offset = Yoffset;
        colNum = Height;
        rowNum = Width;
        Grid = new Transform[rowNum, colNum];
        GameManager = new ColorState[rowNum, colNum];
        CancelManager = new bool[rowNum, colNum];
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                CancelManager[i, j] = false;
            }
        }

        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                GameManager[i,j] = ColorState.None; 
            }
        }

        RandomGenerateBlock();
    }

    // Update is called once per frame
    void Update()
    {
		
        //��һ���Ѿ����
        if (Isfallen)
        {
            m_timer += Time.deltaTime;
            if(m_timer >= CD)//����CD
            {
                RandomGenerateBlock();
                Debug.Log("x");
                Isfallen =false;
                m_timer=0;
            }
        }

        detectClear();
        clearGrid();
    }

    private void RandomGenerateBlock()
    {
        int index = Random.Range(0,FallingBlocks.Length);
        Instantiate(FallingBlocks[index],new Vector3(X_Offset+FallingPositin_X,Y_Offset+FallingPositin_Y,0),Quaternion.identity);

    }

    public static Vector2 RoundVec2(Vector2 v)
    {
        //Debug.Log("From" + v + "to" + Mathf.Round(v.x) + " " + Mathf.Round(v.y));
        return new Vector2(Mathf.Round(v.x),Mathf.Round(v.y));
    }

    public static bool IsInside(Vector2 v)
    {
        return ((int)v.x >= X_Offset && (int)v.x <= X_Offset + rowNum - 1  && (int)v.y >= Y_Offset );
    }

    //如果下方为空，则往下边走
    private void updateColor()
    {
        for(int i = 0; i < rowNum; i++)
        {
            for (int j = 1;j < colNum; j++)
            {
                if (Grid[i,j-1] == null && Grid[i,j] != null)
                {
                    int x = 1;
                    while (j - x - 1 >= 0)
                    {
                        if (Grid[i, j - 1 - x] == null)
                        {
                            x++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    Debug.Log("(" + i + "," + j + "move to" + "(" + i + "," + (j-x));
                    Grid[i, j- x] = Grid[i, j];
                    Vector3 newposition = Grid[i, j- x].position + Vector3.down * x;
                    Grid[i,j- x].position = newposition;
                    Grid[i,j] = null;
                    GameManager[i, j- x] = GameManager[i, j];
                    GameManager[i, j] = ColorState.None;
                    Debug.Log("(" + i + "," + j + "move to" + "(" + i + "," + (j- x));
                }
            }
        }
    }

    private void detectClear()
    {
        for(int i = 0;i < rowNum; i++)
        {
            for(int j = 0;j < colNum; j++)
            {
                if (GameManager[i,j] != ColorState.None)
                {
                    if (detectFive(i, j))
                    {
                        Point += PointFive;
                    }
                    else if (detectFour(i, j))
                    {
                        Point += PointFour;
                    }
                    else if(detectThree(i, j))
                    {
                        Point += PointThree;
                    }
                }
            }
        }
    }

    private void clearGrid()
    {
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                if (CancelManager[i,j] == true)
                {
                    //Debug.Log(i + "," + j);
                    Destroy(Grid[i,j].gameObject);
                    Grid[i,j] = null ;
                    //Debug.Log(i + "," + j);
                    GameManager[i, j] = ColorState.None;
                    CancelManager[i,j] = false;
                    m_isClear = true;
                }
            }
        }
        if (m_isClear)
        {
            Debug.Log("Update");
            updateColor();
            m_isClear=false;
        }
    }

    public static void GameOver()
    {

    }

    private bool detectThree(int row,int col)
    {
        if (detectThreeCol(row, col))
        {
            if (detectThreeRow(row, col))
            {
                CancelManager[row,col] = true;
                CancelManager[row,col + 1] = true;
                CancelManager[row,col + 2] = true;
                Point += PointThree;
            }
            else if (detectThreeRow(row + 1, col))
            {
                CancelManager[row + 1, col] = true;
                CancelManager[row + 1, col + 1] = true;
                CancelManager[row + 1, col + 2] = true;
                Point += PointThree;
            }
            else if(detectThreeRow(row + 2, col))
            {
                CancelManager[row + 2, col] = true;
                CancelManager[row + 2, col + 1] = true;
                CancelManager[row + 2, col + 2] = true;
                Point += PointThree;
            }
            CancelManager[row, col] = true;
            CancelManager[row + 1, col] = true;
            CancelManager[row + 2, col] = true;
            Point += PointThree;
            return true;
        }
        if(detectThreeRow(row, col))
        {
            if (detectThreeCol(row, col))
            {
                CancelManager[row, col] = true;
                CancelManager[row + 1, col] = true;
                CancelManager[row + 2, col] = true;
                Point += PointThree;
            }
            else if (detectThreeRow(row, col + 1))
            {
                CancelManager[row, col + 1] = true;
                CancelManager[row + 1, col + 1] = true;
                CancelManager[row + 2, col + 1] = true;
                Point += PointThree;
            }
            else if (detectThreeCol(row, col + 2))
            {
                CancelManager[row, col + 2] = true;
                CancelManager[row + 1, col + 2] = true;
                CancelManager[row + 2, col + 2] = true;
                Point += PointThree;
            }
            CancelManager[row, col] = true;
            CancelManager[row, col + 1] = true;
            CancelManager[row, col + 2] = true;
            return true;
        }
        return false;
    }

    private bool detectThreeRow(int row,int col)
    {
        if (col + 2 < colNum)
        {
            if (GameManager[row, col] == GameManager[row, col + 1] && GameManager[row, col] == GameManager[row, col + 2]) 
            { 
                return true;
            }
        }
        return false;
    }

    private bool detectThreeCol(int row,int col)
    {
        if (row + 2 < rowNum)
        {
            if (GameManager[row, col] == GameManager[row + 1, col] && GameManager[row, col] == GameManager[row + 2, col]) 
            {
                return true;
            }
        }
        return false;
    }

    private bool detectFour(int row,int col)
    {
        if (col + 3 < colNum)
        {
            if (GameManager[row, col] == GameManager[row, col + 1] && GameManager[row, col] == GameManager[row, col + 2] && GameManager[row, col] == GameManager[row, col + 3] )
            {
                Point += 8;
                CancelManager[row, col] = true;
                CancelManager[row, col + 1] = true;
                CancelManager[row, col + 2] = true;
                CancelManager[row, col + 3] = true;
                return true;
            }
        }
        if (row + 3 < rowNum)
        {
            if (GameManager[row, col] == GameManager[row + 1, col] && GameManager[row, col] == GameManager[row + 2, col] && GameManager[row, col] == GameManager[row + 3, col])
            {
                Point += 8;
                CancelManager[row, col] = true;
                CancelManager[row + 1, col] = true;
                CancelManager[row + 2, col] = true;
                CancelManager[row + 3, col] = true;
                return true;
            }
        }
        return false;
    }

    private bool detectFive(int row,int col)
    {
        if(col + 4 < colNum)
        {
            if (GameManager[row,col] == GameManager[row,col+1] && GameManager[row, col] == GameManager[row, col + 2]&&GameManager[row, col] == GameManager[row, col + 3]&&GameManager[row, col] == GameManager[row, col + 4])
            {
                Point += 20;
                CancelManager[row, col] = true;
                CancelManager[row, col + 1] = true;
                CancelManager[row, col + 2] = true;
                CancelManager[row, col + 3] = true;
                CancelManager[row, col + 4] = true;
                return true;
            }
        }
        if (row + 4 < rowNum)
        {
            if (GameManager[row, col] == GameManager[row + 1, col] && GameManager[row, col] == GameManager[row + 2, col] && GameManager[row, col] == GameManager[row + 3, col] && GameManager[row, col] == GameManager[row + 4, col])
            {
                Point += 20;
                CancelManager[row, col] = true;
                CancelManager[row + 1, col] = true;
                CancelManager[row + 2, col] = true;
                CancelManager[row + 3, col] = true;
                CancelManager[row + 4, col] = true;
                return true;
            }
        }
        return false;
    }
}
