using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement parameters")]
    public float speed = 2f;
    public Vector2 inputVec;
    public Hand[] hands;
    public RuntimeAnimatorController[] aminCon;

    public Scanner scanner;
    Animator animator;
    SpriteRenderer sprite;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.runtimeAnimatorController = aminCon[GameManager.Instance.playerId];
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive) return;
        inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive) return;
        rb.velocity = inputVec.normalized * speed * 100f * Time.fixedDeltaTime;
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive) return;
        animator.SetFloat("speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            sprite.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
            return;

        GameManager.Instance.health -= Time.deltaTime * 10;

        if (GameManager.Instance.health < 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}
