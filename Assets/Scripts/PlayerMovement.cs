using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;
    Rigidbody2D m_Rigidbody2D;
    Vector2 m_Movement;
    AudioSource m_AudioSource;
    public AudioSource hitWallSound;
    public bool paused = false;
    public GameObject wall;
    bool m_PausedStateAlreadyChanged = false;
    bool m_MovementTriggered;
    public float mazeGridSize;
    public float MoveTime;
    float m_MoveTimer;
    float previousHorizontal = 0f;
    float previousVertical = 0f;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    bool m_HitWall = false;
    bool m_MoveEnded = false;
    public int errorCount = 0;
    public void PlayerHitCount(int count)
    {
        count = errorCount;
    }
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        if (m_MoveEnded && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.5f && Mathf.Abs(Input.GetAxis("Vertical")) < 0.5f)
        {
            m_MoveEnded = false;
            Debug.Log("Key released. ");
        }
        if (Input.GetAxis("Cancel") == 1 && m_PausedStateAlreadyChanged == false)
        {
            if (paused)
            {
                paused = false;
            }
            else
            {
                paused = true;
            }
            m_PausedStateAlreadyChanged = true;
        }
        if (Input.GetAxis("Cancel") == 0)
        {
            m_PausedStateAlreadyChanged = false;
        }
        if (!paused)
        {
            float horizontal = Input.GetAxis("Vertical");
            float vertical = Input.GetAxis("Horizontal"); 
            m_Animator.SetBool("IsWalking", m_MovementTriggered);
            if (m_MovementTriggered && !m_MoveEnded)
            {
                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.Play();
                }
            }
            else
            {
                m_AudioSource.Stop();
            }
            if (!m_MoveEnded)
            {
                if (vertical > 0.5f && m_MovementTriggered == false)
                {
                    previousHorizontal = m_Rigidbody2D.position.x;
                    previousVertical = m_Rigidbody2D.position.y;
                    m_Animator.SetBool("MovesLeft", true);
                    m_MovementTriggered = true;
                    horizontalMove = 1f;
                }
                else if (vertical < -0.5f && m_MovementTriggered == false)
                {
                    previousHorizontal = m_Rigidbody2D.position.x;
                    previousVertical = m_Rigidbody2D.position.y;
                    m_Animator.SetBool("MovesRight", true);
                    m_MovementTriggered = true;
                    horizontalMove = -1f;
                }
                else if (horizontal > 0.5f && m_MovementTriggered == false)
                {
                    previousHorizontal = m_Rigidbody2D.position.x;
                    previousVertical = m_Rigidbody2D.position.y;
                    m_Animator.SetBool("MovesDown", true);
                    m_MovementTriggered = true;
                    verticalMove = 1f;
                }
                else if (horizontal < -0.5f && m_MovementTriggered == false)
                {
                    previousHorizontal = m_Rigidbody2D.position.x;
                    previousVertical = m_Rigidbody2D.position.y;
                    m_Animator.SetBool("MovesUp", true);
                    m_MovementTriggered = true;
                    verticalMove = -1f;
                }
            }
            m_Movement.Set(horizontalMove * mazeGridSize, verticalMove * mazeGridSize);
        }
        if (m_MovementTriggered)
        {
            if (m_MoveTimer >= MoveTime)
            {
                m_MoveTimer = - Time.fixedDeltaTime;
                m_MoveEnded = true;
                m_MovementTriggered = false;
                horizontalMove = 0f;
                verticalMove = 0f;
                m_Animator.SetBool("MovesUp", false);
                m_Animator.SetBool("MovesDown", false);
                m_Animator.SetBool("MovesRight", false);
                m_Animator.SetBool("MovesLeft", false);
                Debug.Log("Move ended. ");
                if (m_HitWall)
                {
                    errorCount++;
                }
                m_HitWall = false;
                m_Animator.SetBool("IsDead", false);
            }
            else if (!m_HitWall)
            {
                m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + m_Movement / MoveTime * Time.fixedDeltaTime);
            }
            else
            {
                m_Movement.Set((previousHorizontal - transform.position.x) / (MoveTime - m_MoveTimer) * Time.fixedDeltaTime, (previousVertical - transform.position.y) / (MoveTime - m_MoveTimer) * Time.fixedDeltaTime);
                Debug.Log(m_Movement);
                m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + m_Movement);
            }
            Debug.Log(m_MoveTimer);
            m_MoveTimer += Time.fixedDeltaTime;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == wall)
        {
            m_HitWall = true;
            m_Animator.SetBool("IsDead", true);
            hitWallSound.Play();
        }
    }
}
