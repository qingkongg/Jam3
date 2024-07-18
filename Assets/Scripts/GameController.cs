using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colorstate;
using static Unity.Collections.AllocatorManager;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public AudioSource BlockClear;
    public AudioSource BlockFallen;
    public AudioSource FallenAudio;
    public GameObject ClearAnimation;
    public UIZoomAndCenter UIController;
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
    public int Height;
    public int Width;
    public static int rowNum;//����
    public static int colNum;//����

    //��¼����Ҫ���ɷ��������,���ɵ�λ��
    public GameObject[] FallingBlocks;
    public float FallingPositin_X = 0;
    public float FallingPositin_Y = 0;

    //�������
    public static Transform[,] Grid;

    public static ColorState[,] GameManager;
    public static bool[,] CancelManager;

    //��Ϸ���еĲ���
    public float CD = 30;//��غ����������һ��

    public static int Point = 0;
    public int PointThree = 4;
    public int PointFour = 10;
    public int PointFive = 16;


    public static bool Isfallen = false;
    public static bool IsfallenAudio = false;
    public float m_timer = 0;

    private bool m_isClear = false;
    private static bool m_isOver = false;
    public static bool m_isPaused = false;
    void Start()
    {
        X_Offset = Xoffset;
        Y_Offset = Yoffset;
        colNum = Height;
        rowNum = Width;
        Grid = new Transform[rowNum, colNum+1];
        GameManager = new ColorState[rowNum, colNum+1];
        CancelManager = new bool[rowNum, colNum + 1];
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                CancelManager[i, j] = false;
                GameManager[i, j] = ColorState.None;
            }
        }
        RandomGenerateBlock();
        Point = 0;
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                CancelManager[i, j] = false;
                GameManager[i, j] = ColorState.None;
                if (Grid[i,j] != null)
                {
                    Destroy(Grid[i,j].gameObject);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (m_isOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
        if(Point >= 150)
        {
            BlockController.ColorRange = 5;
        }

        //��һ���Ѿ����
        if (Isfallen && !m_isPaused)
        {
            if (IsfallenAudio) 
            {
                FallenAudio.Play();
                IsfallenAudio = false ;
            }
            m_timer += Time.deltaTime;
            if (m_timer >= CD)//����CD
            {
                RandomGenerateBlock();
                //Debug.Log("x");
                Isfallen = false;
                m_timer = 0;
            }
        }

        detectClear();
        clearGrid();
    }

    private void RandomGenerateBlock()
    {
        int index = Random.Range(0, FallingBlocks.Length);
        Instantiate(FallingBlocks[index], new Vector3(X_Offset + FallingPositin_X, Y_Offset + FallingPositin_Y, 0), Quaternion.identity);

    }

    public static Vector2 RoundVec2(Vector2 v)
    {
        //Debug.Log("From" + v + "to" + Mathf.Round(v.x) + " " + Mathf.Round(v.y));
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool IsInside(Vector2 v)
    {
        return ((int)v.x >= X_Offset && (int)v.x <= X_Offset + rowNum - 1 && (int)v.y >= Y_Offset && (int)v.y < Y_Offset + colNum);
    }

    //如果下方为空，则往下边走
    private void updateColor()
    {
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 1; j < colNum; j++)
            {
                if (Grid[i, j - 1] == null && Grid[i, j] != null)
                {
                    int x = 1;
                    while (j - x - 1 >= 0 && Grid[i, j - 1 - x] == null)
                    {
                        x++;
                    }
                    StartCoroutine(MoveBlockDown(i, j, x, Grid[i, j]));
                    Grid[i, j - x] = Grid[i, j];
                    Grid[i, j] = null;
                }
            }
        }
    }

    private IEnumerator MoveBlockDown(int i, int j, int x, Transform origin)
    {
        BlockClear.Play();
        Transform block = origin;
        Vector3 targetPosition = block.position + Vector3.down * x;
        float duration = 0.2f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            block.position = Vector3.Lerp(block.position, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        block.position = targetPosition;
        GameManager[i, j - x] = GameManager[i, j];
        GameManager[i, j] = ColorState.None;
        BlockFallen.Play();
    }


    private void detectClear()
    {
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                if (GameManager[i, j] != ColorState.None)
                {
                    if (detectFive(i, j) || detectFour(i, j) || detectThree(i, j))
                    {
                        // A match was found, so trigger block clearing
                        Point += (detectFive(i, j) ? PointFive : (detectFour(i, j) ? PointFour : PointThree));
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
                if (CancelManager[i, j] == true && Grid[i, j] != null)
                {
                    Instantiate(ClearAnimation, Grid[i, j].position, Quaternion.identity);
                    Destroy(Grid[i, j].gameObject);
                    Grid[i, j] = null;
                    GameManager[i, j] = ColorState.None;
                    CancelManager[i, j] = false;
                    m_isClear = true;
                }
            }
        }
        if (m_isClear)
        {
            updateColor();
            m_isClear = false;
        }
    }


    public static void GameOver()
    {
        UIZoomAndCenter.StartAnimation();
        m_isOver = true;
    }

   
    private bool detectThree(int row, int col)
    {
        if (detectThreeCol(row, col))
        {
            if (detectThreeRow(row, col))
            {
                CancelManager[row, col] = true;
                CancelManager[row, col + 1] = true;
                CancelManager[row, col + 2] = true;
                Point += PointThree;
            }
            else if (detectThreeRow(row + 1, col))
            {
                CancelManager[row + 1, col] = true;
                CancelManager[row + 1, col + 1] = true;
                CancelManager[row + 1, col + 2] = true;
                Point += PointThree;
            }
            else if (detectThreeRow(row + 2, col))
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
        else if (detectThreeRow(row, col))
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

    private bool detectThreeRow(int row, int col)
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

    private bool detectThreeCol(int row, int col)
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

    private bool detectFour(int row, int col)
    {
        if (col + 3 < colNum)
        {
            if (GameManager[row, col] == GameManager[row, col + 1] && GameManager[row, col] == GameManager[row, col + 2] && GameManager[row, col] == GameManager[row, col + 3])
            {
                CancelManager[row, col] = true;
                CancelManager[row, col + 1] = true;
                CancelManager[row, col + 2] = true;
                CancelManager[row, col + 3] = true;
                return true;
            }
        }
        else if (row + 3 < rowNum)
        {
            if (GameManager[row, col] == GameManager[row + 1, col] && GameManager[row, col] == GameManager[row + 2, col] && GameManager[row, col] == GameManager[row + 3, col])
            {
                
                CancelManager[row, col] = true;
                CancelManager[row + 1, col] = true;
                CancelManager[row + 2, col] = true;
                CancelManager[row + 3, col] = true;
                return true;
            }
        }
        return false;
    }

    private bool detectFive(int row, int col)
    {
        if (col + 4 < colNum)
        {
            if (GameManager[row, col] == GameManager[row, col + 1] && GameManager[row, col] == GameManager[row, col + 2] && GameManager[row, col] == GameManager[row, col + 3] && GameManager[row, col] == GameManager[row, col + 4])
            {
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
