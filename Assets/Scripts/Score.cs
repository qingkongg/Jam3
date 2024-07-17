using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Score : MonoBehaviour
{
    public GameController point;				
	
    void Start()
    {
        gameObject.Find(Text).GetComponent<Text>().text = "Score: " + point.Point;
    }

    void Update()
    {
        gameObject.Find(Text).GetComponent<Text>().text = "Score: " + point.Point;
    }
}

