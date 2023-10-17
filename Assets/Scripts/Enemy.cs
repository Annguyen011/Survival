using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;

    public Rigidbody2D target;
    private bool isLive;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sprite;
    private WaitForFixedUpdate wait;

    private void Awake()
    {
        if (target == null)
        {
            target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        }

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rb.simulated = true;
        anim.SetBool("Dead", false);
        sprite.sortingOrder = 2;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive) return;
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(nextVec + rb.position);
        rb.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive) return;
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }
        sprite.flipX = target.position.x < rb.position.x;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;

        rb.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && isLive)
        {
            health -= collision.GetComponent<Bullet>().damage;

            StartCoroutine(KnockBack());


            if (health <= 0)
            {
                isLive = false;
                coll.enabled = false;
                rb.simulated = false;
                sprite.sortingOrder = 1;
                anim.SetBool("Dead", true);

                GameManager.Instance.kill++;
                GameManager.Instance.GetExp();

                if (GameManager.Instance.isLive)
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);

            }
            else
            {
                anim.SetTrigger("Hit");
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

            }
        }
    }
}
