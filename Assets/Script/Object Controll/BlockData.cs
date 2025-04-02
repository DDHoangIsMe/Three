
using UnityEngine;

public class BlockData : IData
{
    private string blockName_;
    private Material blockMaterial_;
    private bool isUsable_;
    private bool isKinematic_;
    private int health_;
    public string GetName() => blockName_;
    public Material GetMaterial() => blockMaterial_;
    public bool IsUsable() => isUsable_? true : false;
    public bool IsKinematic() => isKinematic_;
    public int GetHealth() => health_;
    public void UpdateData(string name, Material material, bool usable, bool kinematic, int health = 10)
    {
        blockName_ = name;
        blockMaterial_ = material;
        isUsable_ = usable;
        isKinematic_ = kinematic;
        health_ = health;
    }
}
