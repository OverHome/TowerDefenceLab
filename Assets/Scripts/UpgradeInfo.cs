using UnityEngine;


[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade", order = 0)]
public class UpgradeInfo : ScriptableObject
{
    public string UpgradeName;
    public string UpgradeNameNeeded;
    public int CrystalCost;
    public Sprite UpgradeSprite;
    public Sprite HideSprite;
}
