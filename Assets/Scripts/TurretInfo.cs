using UnityEngine;

[CreateAssetMenu(fileName = "TurretInfo", menuName = "Turret", order = 0)]
public class TurretInfo : ScriptableObject
{
    public int Id;
    public Sprite Image;
    public int BuyPrice;
    public int UpgradePrice;
    public float FireRate;
    public float Range;
    public float BaseDamage;
    public float BulletSpeed;
    public float RotationDelay;

}
