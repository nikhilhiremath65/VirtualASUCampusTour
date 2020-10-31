

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Firebase;
using Mapbox.Json.Bson;

public class AuthController : MonoBehaviour
{


    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    private Singleton singleton;
    string userMail = "DefaultMailID";
    string userRole = "DefaultRole";
    string userName = "DefaultName";
    public Text ErrorMessage;
    public Text emailInput, passwordInput, message;
    public GameObject ErrorPanel;

    public void Awake()
    {   //Check for all the firebase dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {   //If all checks pass, then intilize firebase.
                InitilizeFirebase();
            }
            else
            {
                Debug.LogError("could not resolve all the forebase dependencies" + dependencyStatus);
            }
        });
    }


    
    private void InitilizeFirebase()
    {
        Debug.Log("Setting up firebase auth");
        auth = FirebaseAuth.DefaultInstance;
    }


    //Login button
    public void onLogin()
    {
        StartCoroutine(Login(emailInput.text, passwordInput.text));
    }

    //Anonymous Login Button
    public void Login_Anonymous()
    {
        singleton = Singleton.Instance();
        userMail = "Anonymous";
        userRole = "Anonymous";
        userName = "Anonymous";

        Debug.Log("UserEmail: " + userMail);
        singleton.setUserEmail(userMail);
        Debug.Log("Role: " + userRole);
        singleton.setUserRole(userRole);
        Debug.Log("User Name: " + userName);
        singleton.setUserName(userName);
        SceneManager.LoadScene("Prospective_DeptTour");
    }

    private IEnumerator Login(string email,string password )
    {
        var loginTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);
        if (loginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Login failed with {loginTask.Exception}");
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string msg = "Login Failed";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    msg = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    msg = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    msg = "Wrong  Password";
                    break;
                case AuthError.InvalidEmail:
                    msg = "Invalid  Email";
                    break;
                case AuthError.UserNotFound:
                    msg = "Account does not exist";
                    break;
            }
            //message.text = msg;
            ErrorMessage.text = msg;
            ErrorPanel.transform.SetAsLastSibling();
            ErrorPanel.SetActive(true);

        }
        // on successful login
        else
        {
            Firebase.Auth.FirebaseUser newUser = loginTask.Result;
            //update the text field 
            Debug.LogFormat("User signed in successfully:");
            message.text = "Login Successful";
            
            
      



            //User Role Mapping
            var mapping = new Dictionary<string, string>(){
                {"student1@gmail.com","Student"},
                { "nhiremat@gmail.com","Student"},
                { "manager@gmail.com","Manager"},
            };



            // Get the role of prospective user
            foreach (var map in mapping)
            {
                if (map.Key.Equals(newUser.Email))
                {
                    userRole = map.Value;   
                }
            }
                
            // Update singleton instances
            singleton = Singleton.Instance();
            if (newUser.Email !=null)
            {
                userMail = newUser.Email;
                userName = userMail.Split('@')[0];
            }
            Debug.Log("UserEmail: " + userMail);
            singleton.setUserEmail(userMail);
            Debug.Log("Role: " +userRole);
            singleton.setUserRole(userRole);
            Debug.Log("User Name: " + userName);
            singleton.setUserName(userName);
           
            // Scene Transition
            if (userRole.Equals("Student"))
            {
                SceneManager.LoadScene("SchedulesScene");
            }
            else if (userRole.Equals("Manager"))
            {
                SceneManager.LoadScene("ManagerTourView");
            }
            

          
        }

    }
}
