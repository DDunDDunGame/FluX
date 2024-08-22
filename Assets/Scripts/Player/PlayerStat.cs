using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStat : IDamageable
{ 
    public PlayerStat(Player player)
    {
        this.player = player;
        health = maxHealth;
        bullet = 0;
        player.StartCoroutine(FuelDecrease(1, 1));

        player.HpSlider.maxValue = maxHealth;
        SetHPSlider(health);
        player.BulletGroup.Init(bulletBox);
    }

    private readonly Player player;

    private bool IsInvicible
    {
        get {
            return Time.time < invicibleTime + invicibleTimer; 
        }
    }
    private float invicibleTimer = -Mathf.Infinity;
    private float invicibleTime = 3f;
    private float hitTime = 1f;

    public int HitCount { get; private set; }

    // 추후엔 레벨에 따라 변화할 수 있도록 수정
    // ScriptableObject 생각 중
    private float maxHealth = 100;
    private float health;

    private int bulletBox = 10;
    private int bullet;

    private float defense = 0;
    private int damage = 10;

    public int Damage
    {
        get => damage;
    }

    public void TakeDamage(float damage)
    {
        if(IsInvicible) { return; }
        HitCount++;
        invicibleTimer = Time.time;

        ReduceFuel(damage);

        player.HitVolume.SetActive(true);
        player.HitVolume.DisableSmooth(hitTime);
        player.ChangeSpriteByHit(hitTime);

        player.InvicibleVolume.SetActive(true);
        player.InvicibleVolume.SetActive(false, invicibleTime);
        
    }

    private void ReduceFuel(float damage)
    {
        health -= damage * (1 - defense);
        SetHPSlider(this.health);
        if (health <= 0)
        {
            Die();
            return;
        }
    }

    public void RestoreHealth(int health)
    {
        if (this.health + health <= maxHealth) { this.health += health; }
        else { this.health = maxHealth; }
        SetHPSlider(this.health);
    }

    public bool CanUsingBullet(int bullet)
    {
        return this.bullet - bullet >= 0;
    }

    public void UseBullet(int bullet)
    {
        this.bullet -= bullet;
        player.BulletGroup.Decrement(bullet);
    }

    public void RestoreBullet(int bullet)
    {
        if(this.bullet == bulletBox) { return; }

        if(this.bullet + bullet > bulletBox)
        {
            player.BulletGroup.Increment(bulletBox-this.bullet);
            this.bullet = bulletBox;
        }
        else 
        { 
            this.bullet += bullet;
            player.BulletGroup.Increment(bullet);
        }
    }

    private void Die()
    {
        // 죽을 시 처리
        player.gameObject.SetActive(false);
    }

    private IEnumerator FuelDecrease(float perSeconds, float damage)
    {
        while (true)
        {
            yield return new WaitForSeconds(perSeconds);
            ReduceFuel(damage);
        }
    }

    private void SetHPSlider(float value)
    {
        player.HpSlider.value = value;
    }

    public void ResetHitCount()
    {
        HitCount = 0;
    }
}
