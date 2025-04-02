using System;
using System.Linq;
using System.Text;
using UnityEngine;

public class DataManage : Singleton<DataManage>
{
    private BoxCollider boxCollider_;
    private GameObject currentBlock_;
    public string seed_ { private get; set; }

    //Common
    private bool GetAttributeByStringObject(string name)
    {
        if (typeof(ObjectData).GetField(name).GetValue(null) == null) {
            return true;
        }
        return false;
    }
    
    //Create Item
    public GameObject GenerateItem<T>() where T : AbstractObject
    {
        if (typeof(T) == typeof(TargetItem))
        {
            ItemData item = InventoryUI.Instance.GetItem();
            if (item == null)
            {
                return null;
            }
            GameObject obj = PoolManage.Instance.GetObjectFromPool<T>();
            string nameObj = "ITEM_" + item.GetName().ToUpper();
            if (GetAttributeByStringObject(nameObj)) {
                return null;
            }
            obj.GetComponent<T>().PutDataToObject(typeof(ObjectData).GetField(nameObj).GetValue(null)?.ToString());
            obj.GetComponent<T>().SpawnObject();
            return obj;
        }
        return null;
    }
    
    public GameObject GenerateItem<T>(string nameItem) where T : AbstractObject
    {
        if (typeof(T) == typeof(TargetItem))
        {
            GameObject obj = PoolManage.Instance.GetObjectFromPool<T>();
            string nameObj = "ITEM_" + nameItem.ToUpper();
            if (GetAttributeByStringObject(nameObj)) {
                return null;
            }
            obj.GetComponent<T>().PutDataToObject(typeof(ObjectData).GetField(nameObj).GetValue(null)?.ToString());
            obj.GetComponent<T>().SpawnObject();
            return obj;
        }
        return null;
    }
    //Create block
    private GameObject GenerateBlock<T>() where T : AbstractObject
    {
        if (typeof(T) == typeof(TargetBlock))
        {
            ItemData item = InventoryUI.Instance.GetItem();
            if (item == null)
            {
                return null;
            }
            GameObject obj = PoolManage.Instance.GetObjectFromPool<T>();
            string nameObj = "BLOCK_" + item.GetName().ToUpper();
            if (GetAttributeByStringObject(nameObj)) {
                return null;
            }
            obj.GetComponent<T>().PutDataToObject(typeof(ObjectData).GetField(nameObj).GetValue(null)?.ToString());
            return obj;
        }
        return null;
    }
    
    private GameObject GenerateBlock<T>(string nameBlock) where T : AbstractObject
    {
        if (typeof(T) == typeof(TargetBlock))
        {
            GameObject obj = PoolManage.Instance.GetObjectFromPool<T>();
            string nameObj = "BLOCK_" + nameBlock;
            if (GetAttributeByStringObject(nameObj)) {
                return null;
            }
            string s = typeof(ObjectData).GetField(nameObj).GetValue(null)?.ToString();
            Debug.ClearDeveloperConsole();
            if (s != null) {
                obj.GetComponent<T>().PutDataToObject(typeof(ObjectData).GetField(nameObj).GetValue(null)?.ToString());
                return obj;
            }
        }
        return null;
    }
    
    public GameObject GenerateBlock(StringBuilder nameBlock) => GenerateBlock<TargetBlock>(nameBlock.ToString().ToUpper());

    public void ActionBlock(RaycastHit hit) {
        ItemData item = InventoryUI.Instance.GetItem();
        if (item == null)
        {
            return;
        }
        else if (item.IsUsable()) {
            //
        }
        else if (item.isPlaceAble_) {
            if (CheckSpace(hit)) {
                GameObject block = GenerateBlock<TargetBlock>();
                if (block == null) {
                    return;
                }
                block.GetComponent<TargetBlock>().PlaceBlock(hit);
                InventoryUI.Instance.UseItem();
            }
        }
    }
    
    //Check place block valid
    private bool CheckSpace(RaycastHit hit) {
        if (boxCollider_ == null) {
            boxCollider_ = new GameObject("ColliderManageCheck").AddComponent<BoxCollider>();
            boxCollider_.gameObject.tag = "GameController";
            
            boxCollider_.size = new Vector3(1f, 1f, 1f);
            boxCollider_.enabled = true;
        }
        boxCollider_.transform.position = hit.collider.gameObject.transform.position + hit.normal;
        //Check if there is any other collider in the place of boxCollider_
        Physics.CheckBox(boxCollider_.transform.position, boxCollider_.size / 2);
        Collider[] colliders = Physics.OverlapBox(boxCollider_.transform.position, boxCollider_.size / 2);
        foreach (var item in colliders) {
            if (SettingData.BLOCK_INVALID_TAG.Contains(item.tag)) {
                return false;
            }
        }
        return true;
    }

    public void ChooseItem(int index)
    {
        InventoryUI.Instance.ChooseItem(index);
    }

    public void BreakBlock(RaycastHit hit)
    {
        if (hit.collider == null || hit.collider.gameObject.tag != SettingData.BREAKABLE_TAG) {
            if (currentBlock_ != null) {
                StopBreaking();
                currentBlock_ = null;
            }
            return;
        }
        if (hit.collider.gameObject != currentBlock_) {
            if (currentBlock_ != null) {
                StopBreaking();
            }
            currentBlock_ = hit.collider.gameObject;
            currentBlock_.GetComponent<TargetBlock>().StartBreaking();
        }
    }
    
    public void StopBreaking() {
        if (currentBlock_ != null) {
            currentBlock_.GetComponent<TargetBlock>().StopBreaking();
            currentBlock_ = null;
        }
    }
    
    //Check Item
    public void AddItemToInventory(ItemData item) {
        InventoryUI.Instance.AddItem(item);
    }
    
    public void AddItemToInventory(ItemData item, int amount) {
        InventoryUI.Instance.AddItem(item, amount);
    }

    //World
    public void ClearWorld<T>()
    {
        PoolManage.Instance.ClearWorld<T>();
    }
}
