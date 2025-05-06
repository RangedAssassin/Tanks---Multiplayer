using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeatherAPI : MonoBehaviour
{
    [SerializeField] private string appID;
    [SerializeField] private TextMeshProUGUI cityName;
    [SerializeField] private TextMeshProUGUI currentTemp;
    [SerializeField] private TextMeshProUGUI weatherTitle;
    [SerializeField] private TextMeshProUGUI feelsLike;
    [SerializeField] private TextMeshProUGUI humidity;

    [SerializeField] private Image weatherSprite;

    [SerializeField] private TMP_InputField searchBar;

    [SerializeField] public WeatherInfo info;

    [SerializeField] private Sprite[] weatherSprites;
    [SerializeField] private Sprite defaultIcon;

    private Dictionary<string, Sprite> iconToSprite;

    private void Start()
    {
        weatherSprite.sprite = defaultIcon;

        string[] codes = {
        "01d", "01n",
        "02d", "02n",
        "03d", "03n",
        "04d", "04n",
        "09d", "09n",
        "10d", "10n",
        "11d", "11n",
        "13d", "13n",
        "50d", "50n"
        };

        iconToSprite = new Dictionary<string, Sprite>();

        for (int i = 0; i < codes.Length; i++)
        {
            if (i < weatherSprites.Length)
            {
                iconToSprite[codes[i]] = weatherSprites[i];
            }
            else
            {
                Debug.LogWarning("Not enough sprites assigned for weather icons!");
            }
        }
    }

    public void MakeSearch()
    {
        StartCoroutine(FetchWeatherInformation());
    }

    //this will download weather info and place on the UI
    IEnumerator FetchWeatherInformation()
    {
        string location = searchBar.text;
        string apicall = "https://api.openweathermap.org/data/2.5/weather?q=" + location + "&appid=" + appID + "&units=metric";

        UnityWebRequest myRequest = UnityWebRequest.Get(apicall);
        yield return myRequest.SendWebRequest();

        info = JsonUtility.FromJson<WeatherInfo>(myRequest.downloadHandler.text);
        Debug.Log(info.main.feels_like.ToString());
        cityName.text = CapitalizeFirstLetter(searchBar.text);


        humidity.text = info.main.humidity.ToString() + "%";
        feelsLike.text = info.main.feels_like.ToString() + "ºC";
        weatherTitle.text = info.weather[0].main.ToString();
        currentTemp.text = info.main.temp.ToString() + "ºC";


        string iconCode = info.weather[0].icon; // like "01d"
        if (iconToSprite.ContainsKey(iconCode))
        {
            weatherSprite.sprite = iconToSprite[iconCode]; // convert Sprite to Texture for RawImage
        }
        else
        {
            Debug.LogWarning("Icon code not found in dictionary: " + iconCode);
        }


        Debug.Log(myRequest.downloadHandler.text);
    }

    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Capitalize the first letter and make the rest lowercase
        return char.ToUpper(input[0]) + input.Substring(1).ToLower();
    }

}

[System.Serializable] //make another class to access info - make a class for each info section
public class WeatherInfo
{
    public int id;
    public MainWeatherInfo main;
    public WeatherCondition[] weather;
}
[System.Serializable]
public class MainWeatherInfo
{
    public float temp;
    public float feels_like;
    public float humidity;
    public float pressure;
}
[System.Serializable]
public class WeatherCondition
{
    public string main;
    public string description;
    public string icon;
}
