using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Newtonsoft.Json.Linq;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

namespace Crud
{
    public class CrudOperations
    {
        private readonly string BASE_URL = "https://asu-ar-app.firebaseio.com/";
        private DatabaseReference rootReference;
        public CrudOperations()
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(BASE_URL);
            // Get the root reference location of the database.
            rootReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void deleteSchedule(string database, string user, string schedule)
        {
            rootReference.Child(database).Child(user).Child(schedule).RemoveValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {

                    Debug.Log("ERROR: when accessing Data from Database");

                }
                else if (task.IsCompleted)
                {
                    Debug.Log("SUCCESS: DATA Deleted IN DATABASE");
                }
            });
        }

        public void addLinkLocation(string database, string user, string location)
        {
            rootReference.Child(database).Child(user).SetRawJsonValueAsync(location).ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                throw new Exception("ERROR while appending values to database.");

                            }
                            else if (task.IsCompleted)
                            {
                                Debug.Log("SUCCESS: DATA ADDED TO DATABASE");

                            }



                        }
        );
        }
    }
}