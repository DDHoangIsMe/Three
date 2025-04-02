using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

class SegmentSeed
{
    private string type_;
    public string name_ { get; private set; }
    public Vector3 position_ { get; private set; }
    //
    public Type GetTypeObject() => Type.GetType(type_);
    //
    public SegmentSeed(string type, string name, string x, string y, string z)
    {
        type_ = type;
        name_ = name;
        position_ = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
    }


}

public class Generator : MonoBehaviour
{
    [SerializeField, Range(1, 20)]
    private int[] size_ = new int[] {1, 1, 1};
    [SerializeField]
    Material mat_;
    private int progressCoroutine_;
    private bool isProgress_ = false;
    // Start is called before the first frame update


    void Awake()
    {
        List<IEnumerator> list = new List<IEnumerator>() {PlayFabController.Instance.GetUserData(new List<string>() {SettingData.SEED})};
        StartCoroutine(PrepareAll(list));
    }

    void Start()
    {
        LoadObject();
        GiveItem();
    }

    void Update()
    {
        if (isProgress_) {

        }
        //Save
        else if (Input.GetKeyDown(KeyCode.O))
        {
            //Block
            StringBuilder dataSaveBlock = PoolManage.Instance.GetAllObject<TargetBlock, BlockData>();
            List<IEnumerator> list = new List<IEnumerator>() {PlayFabController.Instance.SetUserData(SettingData.SEED, dataSaveBlock.ToString())};
            StartCoroutine(PrepareAll(list));
        }
        //Load
        else if (Input.GetKeyDown(KeyCode.P))
        {
            LoadObject();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            GenerateBlockDefault();
        }
    }
    //Prepare client
    private IEnumerator TrackCoroutine(IEnumerator func) {
        Debug.Log(isProgress_);
        yield return StartCoroutine(func);
        Debug.Log(isProgress_);
        progressCoroutine_++;
    }
    private IEnumerator PrepareAll(List<IEnumerator> list) {
        if (!isProgress_) {
            isProgress_ = !isProgress_;
            progressCoroutine_ = 0;
            foreach (IEnumerator item in list) {
                StartCoroutine(TrackCoroutine(item));
            }
            Debug.Log(isProgress_);
            yield return new WaitUntil (() => progressCoroutine_ == list.Count);
            Debug.Log(isProgress_);
            isProgress_ = !isProgress_;
        }
    }

    //Block
    private void GenerateBlockWorld(string seed) {
        DataManage.Instance.ClearWorld<TargetBlock>();
        string[] segmentSeed = seed.Split("`");
        Type tempType = Type.GetType(segmentSeed.First());
        for (int i = 1; i < segmentSeed.Length; i++) 
        {
            string[] unit = segmentSeed[i].Split("+");
            StringBuilder tempName = new StringBuilder();
            tempName.Append(unit.First());
            GameObject newObj = DataManage.Instance.GenerateBlock(tempName);
            newObj.SetActive(true);
            newObj.transform.position = new Vector3(int.Parse(unit[1]), int.Parse(unit[2]), int.Parse(unit[3]));
        }
    }

    private void GenerateBlockDefault() {
        DataManage.Instance.ClearWorld<TargetBlock>();
        StringBuilder nameBlock = new StringBuilder();
        for (int i = -size_[0]; i < size_[0]; i++)
        {
            for (int j = 0; j < size_[1]; j++)
            {
                for (int k = -size_[2]; k < size_[2]; k++)
                {
                    nameBlock.Clear();
                    //Random 0 to 1
                    if (8 > UnityEngine.Random.Range(0, 10))
                    {
                        nameBlock.Append("DIRT");
                    }
                    else {
                        nameBlock.Append("STONE");
                    }
                    GameObject tempObj = DataManage.Instance.GenerateBlock(nameBlock);
                    if (tempObj != null) {
                        tempObj.transform.position = new Vector3(i, j, k);
                        tempObj.SetActive(true);
                    }
                }
            }
        }
    }

    private void LoadObject() 
    {
        List<IEnumerator> list = new List<IEnumerator>() {PlayFabController.Instance.GetUserData(new List<string>() {SettingData.SEED})};
        StartCoroutine(PrepareAll(list));
        if (PlayFabController.seed_ != null && PlayFabController.seed_ != "")
        {Debug.Log(PlayFabController.seed_);
            GenerateBlockWorld(PlayFabController.seed_);
        }
        else {
            GenerateBlockDefault();
        }
        Debug.Log(PlayFabController.seed_ == null ? "1" : "2");
    }

    private void GiveItem() {
        ItemData temp = new ItemData();
        temp.UpdateData("Dirt", mat_, true, false, true);
        DataManage.Instance.AddItemToInventory(temp, 10);
    }
}
