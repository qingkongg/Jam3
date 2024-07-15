using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.transform.position += Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            gameObject.transform.position += Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            gameObject.transform.position += Vector3.down;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            gameObject.transform.Rotate(0, 0, -90);
        }
    }
}
