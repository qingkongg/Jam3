using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitCountDisplay : MonoBehaviour
{
    public Text textComponent;
    int hitCount = 0;
    float m_Timer = 0;
    float m_Minute = 0;
    float m_Second = 0;



    void Update()
    {
        m_Timer += Time.deltaTime;
        m_Minute = Mathf.Floor(m_Timer / 60f);
        m_Second = Mathf.Floor(m_Timer % 60f);
        if (m_Second < 10f)
        {
            textComponent.text = "Score: " + GameController.Point + "\n" + "Time: " + m_Minute.ToString() + ":0" + m_Second.ToString();
        }
        else
        {
            textComponent.text = "Score: " + GameController.Point + "\n" + "Time: " + m_Minute.ToString() + ":" + m_Second.ToString();
        }
    }
}
