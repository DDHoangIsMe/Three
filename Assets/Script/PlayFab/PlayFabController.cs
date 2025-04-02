using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine.Android;


public class PlayFabController : Singleton<PlayFabController>
{
    public static string seed_;
    public IEnumerator GetUserData(List<string> nameAtt) {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            Keys = nameAtt
        }, response => {
            foreach (var item in response.Data) {
                Debug.Log("Get user attribute " + item.Key);
                if (item.Key == SettingData.SEED) {
                    seed_ = item.Value.Value;
                }
            }
        }, error => {
            Debug.Log("Detail error: " + error.ErrorMessage);
        });
        yield return null;
    }
    public IEnumerator SetUserData(string key, string value) {
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest() {
                Data = new Dictionary<string, string>() {{key, value}},
                Permission = UserDataPermission.Public
            }, 
            response => {
                Debug.Log("Success update");
            },
            error => {
                Debug.Log(error.ErrorMessage);
            }
        );
        yield return null;
    }
}
