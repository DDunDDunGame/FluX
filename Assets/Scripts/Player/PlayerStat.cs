using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStat : IDamageable
{ 
    public PlayerStat(Player player)
    {
        this.player = player;
        health = maxHealth;
        bullet = bulletBox;
        player.StartCoroutine(FuelDecrease(1, 1));
        SetHPText();
        SetBulletText();
    }

    private readonly Player player;

    private bool IsInvicible
    {
        get {
            return Time.time < invicibleTime + invicibleTimer; 
        }
    }
    private float invicibleTimer = -Mathf.Infinity;
    private float invicibleTime = 1f;

    public int HitCount { get; private set; }

    // 추후엔 레벨에 따라 변화할 수 있도록 수정
    // ScriptableObject 생각 중
    private float maxHealth = 100;
    private float health;

    private int bulletBox = 5;
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

        player.Volume.SetActive(true);
        player.Volume.DisableSmooth(invicibleTime);
    }

    private void ReduceFuel(float damage)
    {
        health -= damage * (1 - defense);
        SetHPText();
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
        SetHPText();
    }

    public bool CanUsingBullet(int bullet)
    {
        return this.bullet - bullet >= 0;
    }

    public void UseBullet(int bullet)
    {
        this.bullet -= bullet;
        SetBulletText();
    }

    public void RestoreBullet(int bullet)
    {
        if(this.bullet + bullet > bulletBox) { this.bullet = bulletBox; }
        else { this.bullet += bullet; }
        SetBulletText();
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

    private void SetHPText()
    {
        player.HpText.text = $"HP: {health}";
    }

    private void SetBulletText()
    {
        player.BulletText.text = $"Bullet: {bullet}";
    }

    public void ResetHitCount()
    {
        HitCount = 0;
    }
}
