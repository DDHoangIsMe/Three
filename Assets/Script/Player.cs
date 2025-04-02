using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    SphereCollider gatherCollider_;
    [SerializeField]
    GameObject body_;
    GameObject camera_;

    // Mono part
    void Awake()
    {
        body_ = transform.parent.gameObject;
        SetUp();
    }

    void Start()
    {
        InvokeRepeating("GetItem", 3f, 0.5f);
    }

    void Update()
    {
        //Throw item
        if (Input.GetKeyDown(KeyCode.T)) {
            CreateTI();
        }
        //Use item
        if (Input.GetMouseButtonDown(1)) {
            ActionItem();
        }
        //Change tool/item
        for (int i = 1; i <= 9; i++) {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i)) {
                DataManage.Instance.ChooseItem(i);
                break;
            }
        }
        //Break block
        if (Input.GetMouseButton(0)) {
            DataManage.Instance.BreakBlock(GetPositionOfTarget());
        }
        else if (Input.GetMouseButtonUp(0)) {
            DataManage.Instance.StopBreaking();
        }
        //
        
    }
    
    void FixedUpdate()
    {
        this.transform.position = body_.transform.position + new Vector3(0, 0.5f, 0);
        //Move this body use the input
        if (Input.GetKey(KeyCode.W)) {
            body_.transform.position += body_.transform.forward * SettingData.PLAYER_SPEED;
        }
        if (Input.GetKey(KeyCode.S)) {
            body_.transform.position -= body_.transform.forward * SettingData.PLAYER_SPEED;
        }
        if (Input.GetKey(KeyCode.A)) {
            body_.transform.position -= body_.transform.right * SettingData.PLAYER_SPEED;
        }
        if (Input.GetKey(KeyCode.D)) {
            body_.transform.position += body_.transform.right * SettingData.PLAYER_SPEED;
        }
        if (Input.GetKey(KeyCode.Space)) {
            if (body_.GetComponent<Body>().IsJumpAble()) {
                body_.GetComponent<Rigidbody>().AddForce(Vector3.up * SettingData.PLAYER_JUMP, ForceMode.Impulse);
            }
        }
    }
    
    //
    private void SetUp() {
        camera_ = transform.GetChild(0).gameObject;
        // gatherCollider_ = gameObject.AddComponent<SphereCollider>();
        // gatherCollider_.radius = 1.5f;
        // gatherCollider_.isTrigger = true;
    }

    private void GetItem()
    {
        Collider[] colliders = Physics.OverlapSphere(body_.transform.position, gameObject.GetComponent<SphereCollider>().radius);
        foreach (Collider item in colliders) {
            if (item.gameObject.tag == "Item") {
                item.gameObject.GetComponent<TargetItem>().CollectFromPlayer(this);
            }
        }
        // Debug.Log("1");
        // if (collision.gameObject.CompareTag(SettingData.GATHERABLE_TAG)) {
        // Debug.Log("2");
        //     TargetItem tempItem = collision.gameObject.GetComponent<TargetItem>();
        //     tempItem.CollectFromPlayer(this);
        // }
    }

    //Feature
    public void CreateTI() {
        GameObject newObj_ = DataManage.Instance.GenerateItem<TargetItem>();
        if (newObj_ == null) {
            return;
        }
        newObj_.transform.position = body_.transform.position + body_.transform.forward * 0.5f;
        //Vector3 direction = new Vector3(body_.transform.forward.x, transform.localRotation.eulerAngles.y, body_.transform.forward.z);
        newObj_.GetComponent<TargetItem>().ThrowItem(camera_.GetComponent<CameraPlayer>().GetDirection());
    }

    public void FacingCamera(float x) {
        body_.transform.Rotate(Vector3.up, x);
    }

    private RaycastHit GetPositionOfTarget() =>  camera_.GetComponent<CameraPlayer>().GetRayHit();  

    private void ActionItem() {
        RaycastHit hit = GetPositionOfTarget();
        if (hit.collider == null) 
            return;
        DataManage.Instance.ActionBlock(hit);
        // GameObject newObj_ = DataManage.Instance.GenerateBlock<TargetBlock>();
        // newObj_.GetComponent<TargetBlock>().PlaceBlock(hit);
    }

    public void ResetPlayer() {
        body_.transform.localPosition = Vector3.zero;
    }
}
