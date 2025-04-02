using UnityEngine;

enum TargetState
{
    IDLE,
    THROW,
    COLLECT,
    HIDE
}

public class TargetItem : AbstractObject, ITarget<ItemData>
{
    [SerializeField, Header("Testing zone")]
    private Material material_;
    [Header("Testing zone")]
    private ItemData data_ = new ItemData();
    private TargetState state_ = TargetState.HIDE;
    private Player player_;

    //mono part
    protected override void OnAwake()
    {
        ItemData data = new ItemData();
    }
    
    void FixedUpdate()
    {
        // Spin this object
        if (state_ == TargetState.IDLE)
        {
            transform.Rotate(Vector3.up, 1.0f);
        }
        else if (state_ == TargetState.COLLECT)
        {
            transform.position = Vector3.Lerp(transform.position, player_.transform.position, SettingData.PLAYER_GATHER * Time.deltaTime);
            if (Vector3.Distance(transform.position, player_.transform.position) < 0.2f)
            {
                DataManage.Instance.AddItemToInventory(data_);
                DestroyObject<TargetItem>();
            }
        }
    }

    //Default part
    public override void DestroyObject<T>()
    {
        state_ = TargetState.HIDE;
        gameObject.SetActive(false);
        player_ = null;
    }

    public override void SpawnObject()
    {
        ChangeImage();
        gameObject.SetActive(true);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        Invoke("IdleMode", 3.0f);
    }

    public override void ScaleObject(float scale)
    {
        // Implementation for scaling the object belong to special event scale % 3 == 0
    }

    private void ChangeImage()
    {
        gameObject.GetComponent<Renderer>().material = data_.GetMaterial();
        material_ = data_.GetMaterial();
    }

    //____________________________________________________________________________________________________
    public override void PutDataToObject(string strData)
    {
        string[] data = strData.Split('+');
        Material mat = Resources.Load<Material>($"{ObjectData.MATERIAL_PATH}{data[1]}");
        if (data_ != null)
        {
            data_.UpdateData(data[0], mat, data[2] == "true", data[3] == "true", data[4] == "true");
        }
        SpawnObject();
    }

    public void ThrowItem(Vector3 direction)
    {   
        state_ = TargetState.THROW;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        gameObject.SetActive(true);
        if (rb != null)
        {
            rb.AddForce(direction * ObjectData.THROW_SPEED);
        }
    }

    private void IdleMode()
    {
        state_ = TargetState.IDLE;
    }

    public void CollectFromPlayer(Player player)
    {
        if (state_ == TargetState.IDLE)
        {
            player_ = player;
            state_ = TargetState.COLLECT;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public ItemData GetData()
    {
        return data_;
    }
}
