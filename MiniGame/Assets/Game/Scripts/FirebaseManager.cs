using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Authentication;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public Firebase.Auth.FirebaseAuth auth;
    public Firebase.Auth.FirebaseUser user;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance; //自動初始化
        auth.StateChanged += AuthStateChanged;
    }
    public void AuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        { 
            user = auth.CurrentUser;
            if (user != null)
            {
                print($"Login : {user.Email}");
            }
        }
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Register(string email, string password)
    { 
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
                return;
            } 
            if (task.IsCompletedSuccessfully)
            {
                print("Registered");
            }
        });
    }
    public async void Login(string email,string password)
    {
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                print(task.Exception.InnerException.Message);
            }
            if (task.IsCompletedSuccessfully)
            {
                print("Login!");
            }
            
        }
        );
    }
    public void LogOut()
    {
        auth.SignOut();
    }
    private void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
    }
}
