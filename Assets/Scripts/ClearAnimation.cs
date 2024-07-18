using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAnimation : MonoBehaviour
{
    public float TimeDuration = 1;

    private float m_timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer >= TimeDuration)
        {
            Destroy(gameObject);
        }
    }
}
