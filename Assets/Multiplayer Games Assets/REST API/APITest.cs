using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APITest : MonoBehaviour
{

    //WeatherAPI - https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API key}

    [SerializeField] private string username;
    [SerializeField] private string score;
    [TextArea(3,7)]

    public string dataRetrieved;

    public ChuckNorrisJoke jokeRetreived;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SendScoreToDatabase());
    }

    public void UploadScore(string username, int score)
    {
        //StartCoroutine()
    }

    IEnumerator FetchData()
    {
        //to download information
        UnityWebRequest myRequest = UnityWebRequest.Get("https://api.chucknorris.io/jokes/random");
        yield return myRequest.SendWebRequest();//Async operation - needs to be run from a coroutine
        //To convert the info from the website
        Debug.Log(myRequest.downloadHandler.text);

        dataRetrieved = myRequest.downloadHandler.text;

        jokeRetreived = JsonUtility.FromJson<ChuckNorrisJoke>(dataRetrieved);
    }

    IEnumerator SendScoreToDatabase()
    {
        int randomScore = Random.Range(50, 1000);
        UnityWebRequest myRequest = UnityWebRequest.Put("https://tanks-1583-default-rtdb.firebaseio.com/Scores/blablabla.json", randomScore.ToString());
        yield return myRequest.SendWebRequest();
        Debug.Log(myRequest.result.ToString());
    }



    
}

[System.Serializable]
public class ChuckNorrisJoke
{
    public string created_at;
    public string url;
    public string value;
    
    
}

public class ScoreEntry
{
    public string username;
    public string score;
}
