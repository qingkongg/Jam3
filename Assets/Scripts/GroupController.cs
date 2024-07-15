using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public GameObject position1;
    public GameObject position2;
    public GameObject position3;
    public GameObject position4;
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
            transform.Rotate(0, 0, 90);
            position1.transform.Rotate(0, 0, -90);
            position2.transform.Rotate(0, 0, -90);
            position3.transform.Rotate(0, 0, -90);
            position4.transform.Rotate(0, 0, -90);
        }
    }
}
