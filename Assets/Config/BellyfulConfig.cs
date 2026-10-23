using UnityEngine;

[CreateAssetMenu(fileName = "HungerConfig", menuName = "Scriptable Objects/HungerConfig")]
public class BellyfulConfig : ScriptableObject
{
    public int hungerDamage;
    
    public int damagePeriodInSeconds;
    
    public int damageThreshold;

    public int maxBellyful;
    
    public int hungerPeriodInSecond;
    
    public int hungerAmount;
}
