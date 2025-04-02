using UnityEngine;

public class ItemData : IData
{
    private string itemName_;
    private Material itemMaterial_;
    private bool isStackable_;
    private bool isUsable_;
    public bool isPlaceAble_ { get; private set;}
    public string GetName() => itemName_;
    public Material GetMaterial() => itemMaterial_;
    public bool IsStackable() => isStackable_;
    public bool IsUsable() => isUsable_;
    public void UpdateData(string name, Material material, bool stackable, bool usable, bool isPlaceAble)
    {
        itemName_ = name;
        itemMaterial_ = material;
        isStackable_ = stackable;
        isUsable_ = usable;
        isPlaceAble_ = isPlaceAble;
    }

    //Cóntructer
    public ItemData() {
        
    }
    
    public ItemData(string name, Material material, bool stackable, bool usable, bool isPlaceAble) {
        itemName_ = name;
        itemMaterial_ = material;
        isStackable_ = stackable;
        isUsable_ = usable;
        isPlaceAble_ = isPlaceAble;
    }
}
