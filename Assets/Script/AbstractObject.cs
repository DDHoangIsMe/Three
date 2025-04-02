using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AbstractObject  : MonoBehaviour
{
    void Awake()
    {
        thisObj_ = this.gameObject;
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void OnStart()
    {
        
    }

    protected GameObject thisObj_;
    protected readonly float weight_;
    public abstract void ScaleObject(float scale);
    public abstract void SpawnObject();
    public abstract void DestroyObject<T>() where T : Object;
    public virtual void PutDataToObject(string data) {
        // Trien khai
    }
    public void ScaleObjectUp(Vector2 scale) {
        this.gameObject.transform.localScale += new Vector3(scale.x, scale.y, 0);
    }
    public void ScaleObjectDown(Vector2 scale) {
        this.gameObject.transform.localScale -= new Vector3(scale.x, scale.y, 0);
    }

    public void OnPause()
    {
        thisObj_.SetActive(false);
    }
}
