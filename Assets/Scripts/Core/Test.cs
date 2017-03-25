using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

    //class variable to ensure random numbers appear when finding random dice values
    public Random randomNumbers = new Random();

    //declaring class variables for end game statistics
    public float averageNoOfRollsPerGame = 0;
    public List<int> noOfRollsPerGame = new List<int>();
    public int highestNoOfRolls = 1;
    public int lowestNoOfRolls = 0;
    public int mostCommonRoll = 0;
    public List<int> sumOfEachRoll = new List<int>();
    public float averageWinningPercentage = 0;
    public int noOfWins = 0;
    public int noOfLosses = 0;

    [ContextMenu ("DisplayIntro")]
    public void DisplayIntro()
    {
        Debug.Log("\nThe Game Craps\n");

        Debug.Log("How to play Craps:\n");

        Debug.Log("You enter the number of games you want to play.");
        Debug.Log("In each game, the shooter rolls two dice.");
        Debug.Log("If the numbers on the dice add up to 2, 3 or 12, the shooter looses.");
        Debug.Log("If the numbers on the dice add up to 7 or 11, the shooter wins.");
        Debug.Log("If the numbers on the dice add up to 4, 5, 6, 8, 9 or 10, that sets the points.");
        Debug.Log("In the latter case, the shooter continues to roll until the numbers match the points, then the shooter wins.");
        Debug.Log("Or, if a 7 is rolled, the shooter looses.\n");

        //call on method to commence asking the number of games a player would like to play
        ProcessNumberofGames();
    }

    public void ProcessNumberofGames()
    {
        int numberOfGames = 0;
        string continueGame = "";
        Debug.Log("How many games of Craps do you want to play?");

       numberOfGames = 10;

       if (numberOfGames <= 0)
       {
           NumberofGamesEntryError();
       }else
       {
           PlayGame(numberOfGames);
           //Debug.Log();
           Debug.Log("Press 'y' to continue playing");
           continueGame = "6";//Console.ReadLine();
           if (continueGame == "y" || continueGame == "Y")
           {
               ClearVariableValuesForNewGame();
               DisplayIntro();
           }
           else
           {
               Debug.Log("Exiting.");
               //Console.ReadKey();
           }
       }

    }
    
    public void NumberofGamesEntryError()
    {
    }

    //commence the actual game
    public void PlayGame(int numberOfGames)
    {
        int dice1 = 0;
        int dice2 = 0;
        int sum = 0;

        for (int i = 0; i < numberOfGames; i++)
        {
            //a blank line for display purposes
            //Debug.Log();

            dice1 = RollDice();
            dice2 = RollDice();

            Debug.Log("For round " + (i + 1) + ", the first dice has the value: " + dice1 + ", and the second dice has the value: " + dice2 + ".");
            sum = dice1 + dice2;
            Debug.Log("This gives the sum of: " + sum);

            //deal with statistical data for the end of the game
            StatisticalSumAnalysis(sum);
            //MostCommonRoll(sum);

            //process the dice results
            ProcessDiceResults(sum);

            //display purposes
            //Debug.Log();
        }
        DisplayEndGameStatistics();
    }

    //process dice results by calling on relevant method using a switch
    public void ProcessDiceResults(int sum)
    {
        switch (sum)
        {
            case 2: case 3: case 12:
                ShooterLoses();
                break;
            case 7: case 11:
                ShooterWins();
                break;
            case 4: case 5: case 6: case 8: case 9: case 10:
                PointsRound(sum);
                break;
            default:
                break;
        }
    }

    public void ShooterLoses()
    {
        Debug.Log("Sorry, shooter looses this round.");
        //calculation for end of game statistic
        NoOfLosses();
    }
    public void ShooterWins()
    {
        Debug.Log("Shooter wins this round!");
        //calculation for end of game statistic
        NoOfWins();
    }

    public void PointsRound(int sum)
    {
        int pointSum = 0;
        int dice1 = 0;
        int dice2 = 0;
        int roundsCounter = 1;
        bool matchfound = false;

        Debug.Log("Commencing points round: Rolling again as the Shooter wins the round if the dice sum matches the points value " + sum + " before a 7 is rolled. Otherwise, the shooter loses.");
        Debug.Log("");

        while (matchfound == false)
        {
            dice1 = RollDice();
            dice2 = RollDice();
            Debug.Log("Points round: The first dice has the value: " + dice1 + " and the second dice has the value: " + dice2 + ".");

            pointSum = dice1 + dice2;
            if (pointSum == 7)
            {
                Debug.Log("This gives the sum of: " + pointSum);
                ShooterLoses();
                matchfound = true;
            }
            else if (pointSum == sum)
            {
                Debug.Log("This gives the sum of: " + pointSum);
                ShooterWins();
                matchfound = true;
            }
            else
                Debug.Log("This gives the sum of: " + pointSum + ". This does not match the points value " + sum + " or a 7.  Rolling again.");
            //Console.ReadLine();

            //collect and manipulate data for end of game statistics
            roundsCounter++;
        }

        //manipulate data for end of game statistics
        StatisticalPointsAnalysis(roundsCounter);
        //MostCommonRoll(pointSum);
    }

    public void ClearVariableValuesForNewGame()
    {
        averageNoOfRollsPerGame = 0;
        List<int> noOfRollsPerGame = new List<int>();
        highestNoOfRolls = 1;
        lowestNoOfRolls = 0;
        mostCommonRoll = 0;
        List<int> sumOfEachRoll = new List<int>();
        averageWinningPercentage = 0;
        noOfWins = 0;
        noOfLosses = 0;

    }

    public int RollDice()
    {
        int dice = 0;
        dice = Random.Range(1, 7);
        return dice;
    }

    /***********statistics section methods**************/
    public void StatisticalSumAnalysis(int Sum)
    {
        LowestNoOfRolls(1);
        if ((Sum < 4 & Sum > 10) || Sum == 7)
        {
            AverageNumberOfRollsPerGame(1);
        }
    }
    public void StatisticalPointsAnalysis(int RoundsCounter)
    {
        HighestNoOfRolls(RoundsCounter);
        LowestNoOfRolls(RoundsCounter);
        AverageNumberOfRollsPerGame(RoundsCounter);
    }

    public void AverageNumberOfRollsPerGame(int Rounds)
    {
        noOfRollsPerGame.Add(Rounds);
        
        //averageNoOfRollsPerGame = (float)noOfRollsPerGame.Average();
    }
    public void HighestNoOfRolls(int roundsCounter)
    {
        if (highestNoOfRolls < roundsCounter)
            highestNoOfRolls = roundsCounter;
    }
    public void LowestNoOfRolls(int Rounds)
    {
        //If rounds == 1, then it is not a points round, and will be the lowest no of rolls
        //else, it is a points round, so set lowestNoOfRounds if appropriate
        if (Rounds == 1)
        {
            lowestNoOfRolls = 1;
        }
        else if (lowestNoOfRolls > Rounds)
        {
            lowestNoOfRolls = Rounds;
        }
        else if (lowestNoOfRolls == 0)
        {
            lowestNoOfRolls = Rounds;
        }

    }
    //public void MostCommonRoll(int Sum)
    //{
    //    //this methods finds the lowest (common) number in the list ie. if you have {2, 2, 3, 3}, or {2, 3, 4, 5} it will only display '2'.
    //    sumOfEachRoll.Add(Sum);
    //    mostCommonRoll = (from i in sumOfEachRoll
    //                group i by i into grp
    //                orderby grp.Count() descending
    //                select grp.Key).First();
    //
    //}
    public void AverageWinningPercentage()
    {
        averageWinningPercentage = (((float)noOfWins /((float)noOfWins + (float)noOfLosses)) * (float)100);     
    }

    public void NoOfWins()
    {
        noOfWins = noOfWins + 1;
    }

    public void NoOfLosses()
    {
        noOfLosses = noOfLosses + 1;
    }
    
    public void DisplayEndGameStatistics()
    {
        AverageWinningPercentage();
        Debug.Log("The average number of rolls per game was: " + averageNoOfRollsPerGame);
        Debug.Log("The highest number of rolls was: " + highestNoOfRolls);
        Debug.Log("The lowest number of rolls was: " + lowestNoOfRolls);
        Debug.Log("The most common roll was: " + mostCommonRoll);
        Debug.Log("The average winning percentage was: " + averageWinningPercentage + "%");
        Debug.Log("Total games you won: " + noOfWins);
        Debug.Log("Total games you lost: " + noOfLosses);
    }
}

