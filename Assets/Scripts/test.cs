using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationSpeed = 90f; // ÿ����ת�ĽǶ�
    public static Transform[,] Grid = new Transform[10,10];

    void Update()
    {
        // ����Ƿ��� J ��
        if (Input.GetKeyDown(KeyCode.J))
        {
            // ��ת������
            transform.Rotate(0, 0, -90);

            UpdateGrid();
        }
    }
    void UpdateGrid()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (Grid[j,i] !=null)
                {
                    if (Grid[j, i].parent == gameObject.transform)
                    {
                        //Debug.Log("position:" + Grid[j,i].position + Grid[j, i].name + j + i);
                        Grid[j, i] = null;
                        
                    }
                }
            }
        }
        foreach (Transform child in transform)
        {
            Vector2 v = child.position;

            Grid[(int)v.x , (int)v.y] = child;
            Debug.Log(string.Format("{0},{1},{2}",v, (int)v.x, (int)v.y));

            Vector3 worldPosition = child.position;
            //Debug.Log(child.name + " World Position: " + worldPosition);
        }
    }
}
