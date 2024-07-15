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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += Vector3.down;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            transform.Rotate(0, 0, -90);
            position1.transform.Rotate(0, 0, 90);
            position2.transform.Rotate(0, 0, 90);
            position3.transform.Rotate(0, 0, 90);
            position4.transform.Rotate(0, 0, 90);
        }
    }

    void UpdateGrid()
    {
        for (int i = 0; i < GameController.colNum; i++)
        {
            for (int j = 0; j < GameController.colNum; j++)
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
            GameController.Grid[(int)v.x, (int)v.y] = child;
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
                if (GameController.Grid[(int)pos.x, (int)pos.y] != null && GameController.Grid[(int)pos.x, (int)pos.y].parent != gameObject.transform)
                {
                    return false;
                }
            }
        }
        return true;
    }
}

