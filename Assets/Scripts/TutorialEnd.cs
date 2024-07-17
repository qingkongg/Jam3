using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnd : MonoBehaviour
{
    bool m_End = false;
    float m_Timer = 0;
    public GameObject player;
    public float loadTime;
    public AudioSource winSound;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            m_End = true;
            winSound.Play();
        }
    }
    void FixedUpdate()
    {
        if (m_End)
        {
            m_Timer += Time.fixedDeltaTime;
        }
        if (m_Timer > loadTime)
        {
            SceneManager.LoadScene(2);
        }
    }
}
