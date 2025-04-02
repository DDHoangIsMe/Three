using UnityEngine;

public class Body : MonoBehaviour
{
    private bool isGround_ = false;

    void OnCollisionStay(Collision collision)
    {
        if (CheckOnBlockTag(collision))
        {
            isGround_ = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (CheckOnBlockTag(collision))
        {
            isGround_ = false;
        }
    }

    private bool CheckOnBlockTag(Collision collision)
    {
        if (collision.gameObject.CompareTag(SettingData.BREAKABLE_TAG)) 
        {
            if (collision.transform.position.y + 1 < transform.position.y) {
                return true;
            }
        }
        return false;
    }
    
    public bool IsJumpAble() => isGround_;
}
