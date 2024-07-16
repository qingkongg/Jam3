using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GroupController : MonoBehaviour
{
    public GameObject position1;
    public GameObject position2;
    public GameObject position3;
    public GameObject position4;

    public float FallCD = 1;

    private float m_theLastFall = 0;

    bool m_isActive = true;
    void Start()
    {
        GameController.Isfallen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isActive) { 
        m_theLastFall += Time.deltaTime;
        if (m_theLastFall >= FallCD)
        {
            m_theLastFall = 0;
            transform.position += Vector3.down;
            if (IsValidPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position -= Vector3.down;
                GameController.Isfallen = true;
                m_isActive = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.A) && m_isActive)
        {
            transform.position += Vector3.left;
            if (IsValidPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position += Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && m_isActive)
        {
            transform.position += Vector3.right;
            if (IsValidPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position -= Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) && m_isActive)
        {
            transform.position += Vector3.down;
            if (IsValidPos())
            {
                UpdateGrid();
            }
            else
            {
                transform.position -= Vector3.down;
                GameController.Isfallen = true;
                m_isActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && m_isActive)
        {
            transform.Rotate(0, 0, -90);
            if (IsValidPos())
            {
                UpdateGrid();
                position1.transform.Rotate(0, 0, 90);
                position2.transform.Rotate(0, 0, 90);
                position3.transform.Rotate(0, 0, 90);
                position4.transform.Rotate(0, 0, 90);
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
        }
    }
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
            Vector2 v = GameController.RoundVec2(child.position);
            GameController.Grid[(int)(v.x - GameController.X_Offset), (int)(v.y - GameController.Y_Offset)] = child;
        }
    }
    

    bool IsValidPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = child.position;
            if (!GameController.IsInside(pos))
            {
                return false;
            }
            else if (GameController.IsInside(pos))
            {
                if (GameController.Grid[(int)(pos.x - GameController.X_Offset), (int)(pos.y - GameController.Y_Offset)] != null && GameController.Grid[(int)(pos.x - GameController.X_Offset), (int)(pos.y - GameController.Y_Offset)].parent != gameObject.transform)
                {
                    return false;
                }
            }
        }
        return true;
    }
}



