using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public float speed;
    public int count;

    private float timer;
    private Player player;

    private void Awake()
    {
        player = GameManager.Instance.player;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (!GameManager.Instance.isLive) return;
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;

            case 1:
                timer += Time.deltaTime;

                if (timer >= speed)
                {
                    timer = 0;
                    Fire();
                }

                break;

            default: break;
        }

    }
    public void Init(ItemData data)
    {
        name = "weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.Instance.poolManager.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.Instance.poolManager.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;

                Batch();
                break;

            case 1:
                speed = .3f;
                break;

            default: break;
        }

        Hand hand = player.hands[(int)data.type];
        hand.sprite.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }

    private void Fire()
    {
        if (!player.scanner.nearsetTarget) return;

        Vector3 targetPos = player.scanner.nearsetTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir.Normalize();

        Transform bullet = GameManager.Instance.poolManager.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

    }



    private void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.poolManager.Get(prefabId).transform;
                bullet.SetParent(transform);
            }

            Vector3 rotVec = Vector3.forward * 360 * i / count;

            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;

            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }
    }
}
