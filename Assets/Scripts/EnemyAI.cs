using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Thông số")]
    public int maxHealth;
    public bool roaming;
    public float moveSpeed;
    public float nextWPDistance; // khoảng cách tối thiểu điểm đến của quái
    public bool updateContinuesPath;
    public int damage;
    public float timeHitPlayer;
    private bool canHitPlayer;
    private int currentHealth;

    [Header("Bắn đạn")]
    public bool isShoot;
    public float firingForce;
    public GameObject bullet;
    public float timeShoot;
    private float m_timeShoot;

    private Seeker seeker;
    private Animator animator;
    private bool reachDestination; // check xem đã đến điểm cuối chưa
    Path path;
    Coroutine moveCoroutine;
    private Rigidbody2D rb;
    private UIEnemy ui;
    private bool isDie;
    private int id;
    private Renderer ren;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int Id 
    { 
        get => id;
        set
        {
            id = value;
            ren.sortingLayerName = "Enemy";
            ren.sortingOrder = id;
        }
    }

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ui = GetComponent<UIEnemy>();
        ren = GetComponent<Renderer>();
    }

    private void Start()
    {
        InvokeRepeating("CalculatePath", 0f, 0.5f);
        reachDestination = true;
        canHitPlayer = true;
        currentHealth = maxHealth;
        isDie = false;
        if (ui)
        {
            ui.ShowImageHealth(currentHealth, maxHealth);
        }

    }
    private void Update()
    {
        if (GameManager.instance.IsGameOver)
        {
            CancelInvoke("CalculatePath");
            return;
        }
        if (isDie) return;

        if (isShoot)
        {
            // tinhs vetor giữa quái và người chơi => xác định hướng bắn
            Vector2 direction = FindObjectOfType<Player>().transform.position - transform.position;
            if (m_timeShoot <= 0)
            {
                GameObject bulletNew = Instantiate(bullet, transform.position, Quaternion.identity);
                bulletNew.GetComponent<Rigidbody2D>().AddForce(direction.normalized * firingForce, ForceMode2D.Impulse);

                m_timeShoot = timeShoot;
            }
            m_timeShoot -= Time.deltaTime;
        }
    }

    void CalculatePath()
    {
        // địa điểm quái tìm đến
        Vector2 target = FindTarget();

        // kiểm tra đã tính toán đường xong chưa + đã đến đích
        // => tìm đường mới
        // updateContinues = true thì không cần đến đích
        if (seeker.IsDone() && (reachDestination || updateContinuesPath))
        {
            seeker.StartPath(transform.position, target, OnPathComplete);
        }
    }

    // được gọi sau khi seeker tính toán xong đường đi ngắn nhất từ start => end
    void OnPathComplete(Path p)
    {
        if (p.error) return;

        // gán path thành đường đi vừa tìm được
        path = p;
        MoveToTarget();
    }

    void MoveToTarget()
    {
        // nếu Coroutine đã chạy thì tắt và chạy lại
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }

    IEnumerator MoveToTargetCoroutine()
    {
        int currentWP = 0;
        reachDestination = false;

        //vectorPath thường là một dãy các điểm (hoặc vector)
        //mà đối tượng di chuyển sẽ đi qua để đến đích
        while (currentWP < path.vectorPath.Count)
        {
            // tính hướng là vector giữa 1 điêm trên đường đi của quái đến người chơi
            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - (Vector2)transform.position).normalized;
            Vector2 force = direction * moveSpeed;
            rb.velocity = force;

            if(rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
            else if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 0);
            }

            animator.SetFloat("Walk", Mathf.Abs(rb.velocity.x));
            // tính khoảng cách giữa quái đến điểm đang đến hiện tại
            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWP]);
            
            // nếu khoảng cách nhỏ hơn 1 khoảng cho trước => trỏ đến điểm đến tiếp theo
            if(distance < nextWPDistance)
            {
                currentWP++;
            }
            
            yield return null;
        }

        rb.velocity = Vector2.zero;
        animator.SetFloat("Walk", 0);

        // đã đi đến điểm cuối
        reachDestination = true;
    }

    Vector2 FindTarget()
    {
        Vector3 playerPos = FindObjectOfType<Player>().transform.position;
        // bay xung quanh Player
        if(roaming)
        {
            // random vector (0,0), (0,1), (0, -1), (-1, 1), ... * Random độ lớn
            return (Vector2)playerPos + (Random.Range(5, 10f) * new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f)).normalized);
        }
        else
        {
            return playerPos;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!canHitPlayer || isDie) return;

        Player player = col.GetComponent<Player>();
        if(player != null)
        {
            player.ChangeHealth(damage * -1);
            canHitPlayer = false;
            Invoke("ResetCanHitPlayer", timeHitPlayer);
        }
    }

    private void ResetCanHitPlayer()
    {
        canHitPlayer = true;
    }

    public void Hit(int value)
    {
        if (isDie) return;

        currentHealth = Mathf.Clamp(currentHealth - value, 0, maxHealth);
        animator.SetBool("Hit", true);
        if (ui)
        {
            ui.ShowImageHealth(CurrentHealth, maxHealth);
        }

        if(currentHealth <= 0)
        {
            GameManager.instance.Kill += 1;
            isDie = true;
            animator.SetTrigger("Death");
        }
        
    }

    // call in animation
    public void ResetAnimationHit()
    {
        animator.SetBool("Hit", false);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
