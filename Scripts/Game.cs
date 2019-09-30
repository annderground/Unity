using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
/// <summary>
/// this class helps to control the game: to measure time, checking if targets have been hit, and to control the rounds played
/// and to save the data collected over all the rounds
/// </summary>
public class Game : MonoBehaviour
{
    
    // Text for the applicant to see, changed during Experiment
    public TextMeshProUGUI beginText;

    //both balls to get data from their collisions
    public MoveBall ballOrange;
    public MoveBallBlue ballBlue;

    private bool start;
    
    // we need the data Points of the blue and orange field to set them active when resetting
    public GameObject orangePoint1;
    public GameObject orangePoint2;
    public GameObject orangePoint3;
    public GameObject orangePoint4;
    public GameObject orangePoint5;
    
    public GameObject bluePoint1;
    public GameObject bluePoint2;
    public GameObject bluePoint3;
    public GameObject bluePoint4;
    public GameObject bluePoint5;

    
    // these variables help to save the right time for each round
    private float _finalTime;
    private float _helperTime;
    private float _helperTime2;
    //starting time of the current round
    public static float Helper2Time;

    // these booleans check if the balls are in the target at the same time
    private bool _orange;
    private bool _blue;

    //these variables help control the run of the game in the update function
    private bool _toControlLoop;
    // determines how many rounds are played
    public int roundsToPlay;
    // counts what round one is in
    private int _currentRound;
    private int _countsRounds;

    // in these lists save the data from the datapoints,help convert it for the csv file and the headers for the csv file 
    private List<List<string>> _data;
    private List<string> _test;
    private List<string> _ourHeader;

    // the transforms are needed to reset the balls to their initial position for the next round
    private Transform _newPositionOrange;
    private Transform _newPositionBlue;
    // Start is called before the first frame update
    /// <summary>
    /// in start we initialize the variables
    /// </summary>
    void Start()
    {
        beginText.text = "Try to hit both targets at the same time!";
        //variable for time 
        _finalTime = 0;
        Helper2Time = 0;
        _helperTime2 = 0;
        //variables for data saving
        _data = new List<List<string>>();
        _test = new List<string>();
        // variables to check whether target was hit
        _orange = false;
        _blue = false;
        //variables to control the Update function
        _toControlLoop = true;
        _currentRound = 1;
        
        // we initialize _ourHeaders List indiviually
        _ourHeader = new List<string>();
        _ourHeader.Add("Number of Game");
        _ourHeader.Add("Orange Data Point 1 ");
        _ourHeader.Add("Orange Data Point 2 ");
        _ourHeader.Add("Orange Data Point 3 ");
        _ourHeader.Add("Orange Data Point 4 ");
        _ourHeader.Add("Orange Data Point 5 ");
        _ourHeader.Add("Blue Data Point 1 ");
        _ourHeader.Add("Blue Data Point 2 ");
        _ourHeader.Add("Blue Data Point 3 ");
        _ourHeader.Add("Blue Data Point 4 ");
        _ourHeader.Add("Blue Data Point 5 ");
        _ourHeader.Add("Final Time ");

        start = true;
    }

    // Update is called once per frame
    /// <summary>
    /// controls the game: checks if targets are met at the same time, starts new rounds and resets the game, computes the time, computes csv file
    /// </summary>
    void Update()
    {
        // Delay in loading 
        // so save the delay to subtract from measurements
        if (start)
        {
            
            _helperTime2 = Time.realtimeSinceStartup;
            start = false;
        }
        
        // if there is only supposed to be one round, do not go into the next if case, but in the one for the "last" round
        if (roundsToPlay == 1)
        {
            _toControlLoop = false;
        }
        
        // if both targets are met at the same time
        if (_blue && _orange && _toControlLoop)
        {
            //we compute the time needed for each round 
            _helperTime = _finalTime;
            Helper2Time = _helperTime2;
            _finalTime = Time.realtimeSinceStartup;
            _finalTime = _finalTime - Helper2Time;
            _helperTime2 = _helperTime2 + _finalTime;
            Helper2Time = _helperTime2;
            
            //as long as not all rounds are played
            if (_currentRound < roundsToPlay)
            {
                // we say that one round is finished
                Debug.Log("You made it through one round, you good Mauwuffen!");
                Debug.Log("You mastered the Labyrinth in " + _finalTime + " seconds!");

                // we also tell that to the applicant
                beginText.text = "You Made It Through Round " + _currentRound + "!";

                //we save the data from the data point of that round
                AddToList();

                // we reset the game in order to start a new round
                ResetGame();

                // we set _currentRound to the next round
                _currentRound++;

                //_toControlLoop is true as long as there is another round to play
                _toControlLoop = (_currentRound < roundsToPlay);
            }
        }
        // when both balls hit the target but there is not another round to play ("last" round)
        else if (_blue && _orange)
        {
            // we again compute the time needed to finish the game
            _helperTime = _finalTime;
            Helper2Time = _helperTime2;
            _finalTime = Time.realtimeSinceStartup;
            _finalTime = _finalTime - Helper2Time;
            _helperTime2 = _helperTime2 + _finalTime;
            Helper2Time = _helperTime2;

            // there is no next round
            if (_currentRound == roundsToPlay)
            {
                // we say that the applicant is finished
                Debug.Log("You made it through one round, you good Mauwuffen!");
                Debug.Log("You mastered the Labyrinth in " + _finalTime + " seconds!");

                //we set _currentRound to next round so that the loop stops
                _currentRound++;

                // we save our datapoint data from the last round
                AddToList();

                // because there are no rounds left, we convert our whole data in csv file format
                _test = CSVTools.CreateCsv(_data, _ourHeader);

                // we show the data on the console for us
                foreach (string row in _test)
                {
                    Debug.Log(row);
                }

                // now we save our data in a csv file
                CSVTools.SaveData(_test, Application.dataPath + "/Data/" + GUID.Generate());

            }

            // we tell the participant that the Experiment ended and to end it with Key "O"
            beginText.text = "You did it! Press O to stop the Experiment!";

            if (Input.GetKeyDown(KeyCode.O))
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        } 
    }

    /// <summary>
    /// in ResetGame we reset the game so that the next round can be played
    /// </summary>
    private void ResetGame()
    {
        // we put the orange ball on its initial position 
        _newPositionOrange = ballOrange.GetComponent<Transform>();
        _newPositionOrange.position = MoveBall.InitialPosition;

        // we put the blue ball on its initial position 
        _newPositionBlue = ballBlue.GetComponent<Transform>();
        _newPositionBlue.position = MoveBallBlue.InitialPosition;

        //we say that both targets are not hit
        _orange = false;
        _blue = false;

        // we set our datapoints active again in order to collect new data in the next round
        orangePoint1.gameObject.SetActive(true);
        orangePoint2.gameObject.SetActive(true);
        orangePoint3.gameObject.SetActive(true);
        orangePoint4.gameObject.SetActive(true);
        orangePoint5.gameObject.SetActive(true);
        
        bluePoint1.gameObject.SetActive(true);
        bluePoint2.gameObject.SetActive(true);
        bluePoint3.gameObject.SetActive(true);
        bluePoint4.gameObject.SetActive(true);
        bluePoint5.gameObject.SetActive(true);

        
        // we reset our data lists in order to use them again
        for (int i = 0; i < 5; i++)
        {
            ballBlue.timePoints[i] = "not used";
        }
        for (int i = 0; i < 5; i++)
        {
            ballOrange.timePoints[i] = "not used";
        }
    }

    /// <summary>
    /// this method saves the time from the collision of each ball with the respective datapoints and the time needed
    /// to finish the game in one List (_data)
    /// </summary>
    private void AddToList()
    {
        _countsRounds++;
        List<string> taskData = new List<string>();
        // we save the current round number
        taskData.Add(_countsRounds.ToString());
        //we save the datapoints of the orange field
        foreach(string data in ballOrange.timePoints)
        {
            taskData.Add(data.ToString());
        }
        //we save the datapoints of the blue field
        foreach (string data in ballBlue.timePoints)
        {
            taskData.Add(data.ToString());
        }
        // we add the final time neede to finish
        taskData.Add(_finalTime.ToString());
        // we save it all in one list
        _data.Add(taskData);
        
    }
    
    /// <summary>
    /// tells us if the orange ball has hit its target 
    /// </summary>
    public void OrangeArrived()
    {
        _orange = true;
    }
    
    /// <summary>
    /// tells us if the orange ball left its target
    /// </summary>
    public void OrangeLeft()
    {
        _orange = false;
    }
    /// <summary>
    /// tells us if the blue ball has hit its target
    /// </summary>
    public void BlueArrived()
    {
        _blue = true;
    }

    /// <summary>
    /// tells us if the blue ball has left its target
    /// </summary>
    public void BlueLeft()
    {
        _blue = false;
    }
}