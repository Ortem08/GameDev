using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private HealthConfig healthConfig;
    [SerializeField] private BellyfulConfig bellyfulConfig;

    private bool _isAlive = true;
    private int _hp;
    private int Hp
    {
        get => _hp;
        set
        {
            _hp = math.clamp(value, 0, healthConfig.maxHp);
            if (_hp <= 0)
            {
                _isAlive = false;
            }
        }
    }

    private int _bellyful;

    private int Bellyful
    {
        get => _bellyful;
        set
        {
            _bellyful = math.clamp(value, 0, bellyfulConfig.maxBellyful);
            if (_bellyful < bellyfulConfig.damageThreshold)
            {
                StartCoroutine(TakeHungerDamage());
            }
        }
    }

    private void Awake()
    {
        StartCoroutine(Starve());
    }

    private IEnumerator TakeHungerDamage()
    {
        while (Bellyful < bellyfulConfig.damageThreshold)
        {
            TakeDamage(bellyfulConfig.hungerDamage);
            yield return new WaitForSeconds(bellyfulConfig.damagePeriodInSeconds);
        }
    }
    
    private IEnumerator Starve()
    {
        while (_isAlive)
        {
            yield return new WaitForSeconds(bellyfulConfig.hungerPeriodInSecond);
            DecreaseBellyful(bellyfulConfig.hungerAmount);
        }
    }

    private void TakeDamage(int damage)
    {
        Hp -= damage;
    }

    private void Heal(int amount)
    {
        Hp += amount;
    }
    
    private void DecreaseBellyful(int amount)
    {
        Bellyful -= amount;
    }
}
