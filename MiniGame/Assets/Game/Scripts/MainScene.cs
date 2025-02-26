using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField] FirebaseManager firebaseManager;
    // Start is called before the first frame update
    [SerializeField] TMP_InputField inputEmail;
    [SerializeField] TMP_InputField inputPassword;

    [SerializeField] GameObject panelLogin;
    [SerializeField] GameObject panelInfo;
    [SerializeField] TMPro.TMP_Text textMeshPro;
    private void OnEnable()
    {
        firebaseManager.auth.StateChanged += AuthStateChanged;
    }
    private void OnDestroy()
    {
        firebaseManager.auth.StateChanged -= AuthStateChanged;
    }
    public void Register()
    {
        firebaseManager.Register(inputEmail.text,inputPassword.text);

    }
    public void AuthStateChanged(object sender, EventArgs e)
    {
        if (firebaseManager.user == null)
        {
            textMeshPro.text = "";
            panelLogin.SetActive(true);
            panelInfo.SetActive(false);
        }
        else
        {
            textMeshPro.text = firebaseManager.user.Email;
            panelLogin.SetActive(false);
            panelInfo.SetActive(true);
        }
    }
    public void Login()
    {
        firebaseManager.Login(inputEmail.text,inputPassword.text);
    }
    public void Logout()
    {
        firebaseManager.LogOut();
    }
}

