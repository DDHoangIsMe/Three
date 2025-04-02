using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField]
    private GameObject lobby_;
    [SerializeField]
    private GameObject main_;
    private string email_;
    private string passWord_;
    private string userName_;
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "13E277";
        }
        // var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true};
        // PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest() {
                FunctionName = "InitiateNewAccount"
            },
            OnSuccessAction,
            OnFailAction
        );
    }

    private void OnSuccessAction(ExecuteCloudScriptResult result)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            Keys = new List<string>() {"Seed"}
        }, response => {
            if (response.Data.ContainsKey("Seed")) {
                DataManage.Instance.seed_ = response.Data["Seed"].Value;
            }
            else {
                Debug.Log("Continue without connection");
            }
            GoToMain();
        }, error => {
            Debug.Log("Error 404: Can't get 'Seed' data");
            Debug.Log("Detail: " + error.ErrorMessage);
        });
    }

    private void OnFailAction(PlayFabError error)
    {
//
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnRegistSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, Register success");
    }

    private void OnRegistFailure(PlayFabError error)
    {
        Debug.LogError("Error register: ");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void ConfirmLogin()
    {
        var request = new LoginWithEmailAddressRequest {
            Email = email_,
            Password = passWord_
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        PlayerPrefs.SetString("EMAIL", email_);
        PlayerPrefs.SetString("PASSWORD", passWord_);
    }

    public void ConfirmRegister()
    {
        Debug.Log("Email: " + email_ + " ,  Password have length = " + passWord_.Length);
        var request =  new RegisterPlayFabUserRequest {
            Email = email_,
            Password = passWord_,
            Username = userName_
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegistSuccess, OnRegistFailure);
    }

    public void OnChangeEmail(string text)
    {
        email_ = text;
    }

    public void OnChangePass(string text)
    {
        passWord_ = text;
    }

    public void OnChangeName(string text)
    {
        userName_ = text;
    }

    private void GoToMain()
    {
        main_.SetActive(true);
        lobby_.SetActive(false);
    }
}