using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public Text TestTime;
    public float TimeScale;
    public struct time
    {
        public float Gametime;
    }
    public time gameTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameTime.Gametime = TimeScale * Time.deltaTime;
        TestTime.text = gameTime.Gametime.ToString();
        if(gameTime.Gametime == Mathf.Epsilon)
        {
            gameTime.Gametime = 0.0f;
        }
    }
}
