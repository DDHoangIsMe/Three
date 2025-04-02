using System.Collections;
using UnityEngine;

enum BreakProgress
{
    NONE,
    CRACK,
    FRAGILE,
    BREAKING,
    BROKEN
}
public class TargetBlock : AbstractObject, ITarget<BlockData>
{
    private BlockData data_ = new BlockData();
    private Coroutine breakProgress_;
    private GameObject crackEffect_;
    //mono
    void Start()
    {
        crackEffect_ = transform.GetChild(0).gameObject;
    }
    // Default part
    public override void DestroyObject<T>()
    {
        GameObject newObj_ = DataManage.Instance.GenerateItem<TargetItem>(data_.GetName());
        if (newObj_ == null) {
            
            return;
        }
        newObj_.transform.position = transform.position;
        gameObject.SetActive(false);
    }
    public override void ScaleObject(float scale)
    {
        // Implementation for scaling the object
    }
    public override void SpawnObject()
    {
        //
    }
    //____________________________________________________________________________________________________
    public BlockData GetData() 
    {
        return data_;
    }
    
    public override void PutDataToObject(string strData)
    {
        string[] data = strData.Split('+');
        Material mat = Resources.Load<Material>($"{ObjectData.MATERIAL_PATH}{data[1]}");
        if (data_ != null)
        {
            data_.UpdateData(data[0], mat, data[2] == "true", data[3] == "true", int.Parse(data[4]));
            gameObject.GetComponent<Renderer>().material = data_.GetMaterial();
            gameObject.GetComponent<Rigidbody>().isKinematic = data_.IsKinematic();
        }
    }
    public void PlaceBlock(RaycastHit hit)
    {
        if (hit.collider != null)
        {
            transform.position = hit.collider.gameObject.transform.position + hit.normal;
            gameObject.SetActive(true);
        }
        else {
            DestroyObject<TargetBlock>();
        }
    }
    public void StartBreaking()
    {
        if (breakProgress_ != null)
        {
            return;
        }
        int timeChange = data_.GetHealth() / (int)BreakProgress.BROKEN;
        breakProgress_ = StartCoroutine(BreakingProgress(timeChange));
    }
    public void StopBreaking()
    {
        if (breakProgress_ != null) {
            StopCoroutine(breakProgress_);
            breakProgress_ = null;
        }
        crackEffect_.gameObject.SetActive(false);
    }
    private IEnumerator BreakingProgress(int time) {
        BreakPatern(BreakProgress.NONE);
        yield return new WaitForSeconds(time);
        BreakPatern(BreakProgress.CRACK);
        yield return new WaitForSeconds(time);
        BreakPatern(BreakProgress.FRAGILE);
        yield return new WaitForSeconds(time);
        BreakPatern(BreakProgress.BREAKING);
        yield return new WaitForSeconds(time);
        BreakPatern(BreakProgress.BROKEN);
    }

    private void BreakPatern(BreakProgress state)
    {
        switch (state)
        {
            case BreakProgress.NONE:
                crackEffect_.gameObject.SetActive(true);
                break;
            case BreakProgress.BROKEN:
                crackEffect_.gameObject.SetActive(false);
                DestroyObject<TargetBlock>();
                break;
        }
        crackEffect_.GetComponent<Renderer>().material.SetFloat("_ThreshHold", SettingData.BREAK_THRESHOLD * (int)state);       
    }
}