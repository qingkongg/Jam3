using Colorstate;
    // Update is called once per frame
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GroupController : MonoBehaviour
{
    public AudioSource HitAudio;

    public GameObject position1;
    public GameObject position2;
    public GameObject position3;
    public GameObject position4;



    public float FallCD = 1;

    private float m_theLastFall = 0;

    bool m_isActive = true;
    bool m_isUpdated = false;
    bool m_isValid = false;
    void Start()
    {
        GameController.Isfallen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isActive)
        {
            m_theLastFall += Time.deltaTime;
            if (m_theLastFall >= FallCD)
            {
                m_theLastFall = 0;
                Vector3 newPosition = transform.position + Vector3.down;
                transform.position = newPosition;
                if (IsValidPos(transform))
                {
                    UpdateGrid();
                }
                else
                {
                    GameController.IsfallenAudio = true;
                    GameController.Isfallen = true;
                    //Debug.Log("Isfallen!" + IsValidPos(transform));
                    m_isActive = false;
                    transform.position -= Vector3.down; // revert the position change
                }
            }
            if (Input.GetKeyDown(KeyCode.A) && m_isActive)
            {
                Vector3 newPosition = transform.position + Vector3.left;
                transform.position = newPosition;
                if (IsValidPos(transform))
                {
                    UpdateGrid();
                }
                else
                {
                    HitAudio.Play();
                    transform.position -= Vector3.left; // revert the position change
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) && m_isActive)
            {
                Vector3 newPosition = transform.position + Vector3.right;
                transform.position = newPosition;
                if (IsValidPos(transform))
                {
                    UpdateGrid();
                }
                else
                {
                    HitAudio.Play();
                    transform.position -= Vector3.right; // revert the position change
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && m_isActive)
            {
                Vector3 newPosition = transform.position + Vector3.down;
                transform.position = newPosition;
                if (IsValidPos(transform))
                {
                    UpdateGrid();
                }
                else
                {
                    //Debug.Log("TriggerDown IsFallen");
                    GameController.IsfallenAudio = true;
                    GameController.Isfallen = true;
                    m_isActive = false;
                    transform.position -= Vector3.down; // revert the position change
                }
            }

            if (Input.GetKeyDown(KeyCode.J) && m_isActive)
            {
                // 先保存当前的旋转状态
                Quaternion originalRotation = transform.rotation;

                // 预旋转
                transform.Rotate(0, 0, -90);

                // 检查旋转后的位置是否有效
                if (IsValidPos(transform))
                {
                    // 如果有效，更新网格
                    UpdateGrid();
                    // 旋转色块
                    position1.transform.Rotate(0, 0, 90);
                    position2.transform.Rotate(0, 0, 90);
                    position3.transform.Rotate(0, 0, 90);
                    position4.transform.Rotate(0, 0, 90);
                }
                else
                {
                    // 如果无效，恢复原来的旋转状态
                    HitAudio.Play();
                    transform.rotation = originalRotation;
                }
            }
        }
        else if (!m_isActive&& !m_isUpdated) 
        {
            m_isUpdated = true;
            GetBlockColors();
        }
    }

    bool IsValidPos(Transform proposedTransform)
    {
        foreach (Transform child in proposedTransform)
        {
            Vector2 pos = GameController.RoundVec2(child.transform.position + new Vector3(0.1f, 0.1f, 0));
            //Debug.Log("pos" + pos);
            if (!GameController.IsInside(pos) && pos.y < GameController.colNum + GameController.Y_Offset)
            {
                //Debug.Log("pos" + pos + "is not inside");
                return false;
            }
            else if (GameController.IsInside(pos))
            {
                m_isValid = true;
                if (GameController.Grid[(int)(pos.x - GameController.X_Offset), (int)(pos.y - GameController.Y_Offset)] != null && GameController.Grid[(int)(pos.x - GameController.X_Offset), (int)(pos.y - GameController.Y_Offset)].parent != proposedTransform)
                {
                    //Debug.Log("pos" + pos + "is not null");
                    return false;
                }
            }
        }
        return true;
    }

    void UpdateGrid()
    {
        for (int i = 0; i < GameController.colNum; i++)
        {
            for (int j = 0; j < GameController.rowNum; j++)
            {
                if (GameController.Grid[j, i] != null)
                {
                    if (GameController.Grid[j, i].parent == gameObject.transform)
                    {
                        GameController.Grid[j, i] = null;
                    }
                }
            }
        }
        foreach (Transform child in transform)
        {
            Vector2 v = GameController.RoundVec2(child.transform.position);
            if (Mathf.RoundToInt(v.x - GameController.X_Offset) < GameController.rowNum && Mathf.RoundToInt(v.y - GameController.Y_Offset) <= GameController.colNum)
            {
                GameController.Grid[Mathf.RoundToInt(v.x - GameController.X_Offset), Mathf.RoundToInt(v.y - GameController.Y_Offset)] = child;
            }
        }
    }


//落地后同步更新到记录颜色的GameManager颜色数组
    void GetBlockColors()
    {
        if (!m_isActive)
        {
            foreach (Transform child in transform)
            {
                BlockController blockController = child.GetComponent<BlockController>();
                if (blockController != null)
                {
                    ColorState colorState = blockController.Color;
                    Vector2 pos = GameController.RoundVec2(child.transform.position);
                    //Debug.Log((int)pos.x + (int)pos.y);
                    Debug.Log((int)pos.y + "+" + GameController.colNum);
                    if ((int)pos.y < GameController.colNum)
                    {
                        GameController.GameManager[(int)pos.x, (int)pos.y] = colorState;
                        //Debug.Log("position" + pos + "is" + colorState);
                    }
                    else if((int)pos.y >= GameController.colNum  && m_isValid && !GameController.m_isPaused)
                    {
                        Debug.Log("1");
                        GameController.GameOver();
                        GameController.m_isPaused = true;
                    }
                    
                }
            }
        }
    }
}



