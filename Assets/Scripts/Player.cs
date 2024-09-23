using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float speedBoost;
    public float timeRoll;

    public List<Transform> weaponPoints;
    [SerializeField] private int originalHealth;
    private int health;
    private float m_timeRoll;
    private bool isRoll;
    [SerializeField] private GameObject character;
    private Animator animator;
    private Rigidbody2D rb;
    private float originalSpeed;

    public float Health { get => health;}

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = character.GetComponent<Animator>();
    }
    private void Start()
    {
        health = originalHealth;
        GameGUIManager.instance.ShowHealthText(health, originalHealth);
        GameGUIManager.instance.ShowHealthImage(health, originalHealth);
        originalSpeed = moveSpeed;
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2 (moveX, moveY) * moveSpeed;

        if (moveX > 0)
        {
            character.transform.localScale = new Vector3(1, 1, 0);
        }
        else if (moveX < 0)
        {
            character.transform.localScale = new Vector3(-1, 1, 0);
        }
        animator.SetFloat("Walk", Mathf.Abs(moveX));

        if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1)) && m_timeRoll <= 0) 
        {
            moveSpeed = (originalSpeed + speedBoost);
            m_timeRoll = timeRoll;
            isRoll = true;
            animator.SetBool("Roll", true);
        }

        if(m_timeRoll <= 0 && isRoll)
        {
            isRoll = false;
            moveSpeed = originalSpeed;
            animator.SetBool("Roll", false);
        }
        else
        {
            m_timeRoll -= Time.deltaTime;
        }
    }

    public void ChangeHealth(int value)
    {
        health = Mathf.Clamp(health + value, 0, originalHealth);
        GameGUIManager.instance.ShowHealthText(health, originalHealth);
        GameGUIManager.instance.ShowHealthImage(health, originalHealth);
        if(health <= 0)
        {
            animator.SetTrigger("Death");
            Invoke("DestroyPlayer", 0.4f);
        }
    }

    public void DestroyPlayer()
    {
        if(GameManager.instance.IsWin == false)
        {
            GameManager.instance.IsGameOver = true;
        }
        Destroy(gameObject);
    }

    public void CreateWeapon(GameObject weapon)
    {
        if(weapon != null && weaponPoints != null && weaponPoints.Count > 0)
        {
            GameObject weaponNew = Instantiate(weapon, weaponPoints[0].position, Quaternion.identity);
            weaponNew.transform.SetParent(transform);
            weaponPoints.RemoveAt(0);
        }
    }
}
