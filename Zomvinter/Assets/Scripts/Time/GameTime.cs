using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public Text TestTime;
    public float TimeScale;
    public time gameTime;
    public int day;
    // Start is called before the first frame update
    void Start()
    {
        gameTime.day = 0;
        gameTime.Gametime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        day = gameTime.day;
        gameTime.Gametime += TimeScale * Time.deltaTime;
        TestTime.text = gameTime.Gametime.ToString("F2");
        if(gameTime.Gametime >= 24.0f)
        {
            gameTime.day++;
            gameTime.Gametime = 0.0f;
        }
    }
}
