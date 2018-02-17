using System;
using System.Collections.Generic;

namespace TechJobsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create two Dictionary vars to hold info for menu and data

            // Top-level menu options
            Dictionary<string, string> actionChoices = new Dictionary<string, string>();
            actionChoices.Add("search", "Search");
            actionChoices.Add("list", "List");

            // Column options in CSV file/data
            Dictionary<string, string> columnChoices = new Dictionary<string, string>();
            columnChoices.Add("core competency", "Skill");
            columnChoices.Add("employer", "Employer");
            columnChoices.Add("location", "Location");
            columnChoices.Add("position type", "Position Type");
            columnChoices.Add("all", "All");

            Console.WriteLine("Welcome to LaunchCode's TechJobs App!");

            // Allow user to search/list until they manually quit with ctrl+c
            while (true)
            {
                /*Provides the header "view Jobs" & is the input header for GetUserSelection.
                and actionChoices of list or search */
                //Runs at the start of the program and waits for user selection 
                string actionChoice = GetUserSelection("View Jobs", actionChoices);

                if (actionChoice.Equals("list"))
                {
                    //if action choice is list, run UserSelection w/ colChoices dict
                    //Returns the user's selected key, ex. Toast 
                    string columnChoice = GetUserSelection("List", columnChoices);

                    //user looks at menu and selects list all
                    if (columnChoice.Equals("all"))
                    {
                        //FindAll, loads all dicts/data in the list and returns it.
                        PrintJobs(JobData.FindAll());
                    }
                    else
                    {
                        //list dictionary results by user selected header/column

                        //results Calls method FindAll in JobData class w/param colChoice
                        List<string> results = JobData.FindAll(columnChoice);

                        //The header below pulls column choice key with colchoice value. 
                        //  EX..                                Skill 
                        Console.WriteLine("\n*** All " + columnChoices[columnChoice] + " Values ***");
                         
                        foreach (string item in results)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
                else // choice is "search"
                {
                    // How does the user want to search (e.g. by skill or employer)
                    string columnChoice = GetUserSelection("Search", columnChoices);

                    // What is their search term?
                    Console.WriteLine("\nSearch term: ");
                    string searchTerm = Console.ReadLine();

                    List<Dictionary<string, string>>searchResults;

                    // Fetch results
                    if (columnChoice.Equals("all"))
                    {
                        //searchResults calls the class JobData, method FindByValue in JobData.cs.
                        //FindByValue parameter being user input string.
                        searchResults = JobData.FindByValue(searchTerm);
                        //call print jobs with the list of dictionaries found to be relevant/equal to user input
                        PrintJobs(searchResults);
                    }
                    else
                    {
                        searchResults = JobData.FindByColumnAndValue(columnChoice, searchTerm);
                        PrintJobs(searchResults);
                    }
                }
            }
        }

        /*
         * Returns the key of the selected item from the choices Dictionary
         */
        private static string GetUserSelection(string choiceHeader, Dictionary<string, string> choices)
        {
            //Initiates variables 
            int choiceIdx;
            bool isValidChoice = false;
            //choiceKeys = Array of strings the length 
            //of the choices/columnChoices - initializes the empty array
            string[] choiceKeys = new string[choices.Count];
            int i = 0;
            
        //choiceKeys, initializes the size of the string array.
        //The foreach loop fills in the array slots with the header/key using the index counter
            foreach (KeyValuePair<string, string> choice in choices)
            {
                
                //for each header... (bc it loops through each dictionary and pulls the key, saving the key to 'choiceKeys'
                choiceKeys[i] = choice.Key;

                //Increments after 
                i++;
            }

            //now that I have an array of header/keys(choiceKeys)...
            do
            {
                //Displays actionChoices: 'search or list by:'and menu options in the dictionary choices (
                Console.WriteLine("\n" + choiceHeader + " by:");
                
                for (int j = 0; j < choiceKeys.Length; j++) //choiceKeys - Array of strings
                {
                    Console.WriteLine(j + " - " + choices[choiceKeys[j]]); 
                    //Loops Selected dictionary actionChoices/columnChoices/key [value/choiceKeys array[position j/0] 
                    //choices[choiceKeys[0]] = choices[core competency] = Skill
                    //choiceKeys parsed through in the previous function to use here. Parsed through using for loop to pull values. 
                }
                
                //Gets user input for selection 
                string input = Console.ReadLine();
                //Converts user input into a number
                choiceIdx = int.Parse(input);

                //validates user input, not negative or out of choiceKeys length.
                if (choiceIdx < 0 || choiceIdx >= choiceKeys.Length)
                {
                    Console.WriteLine("Invalid choices. Try again.");
                }
                else
                {
                    isValidChoice = true;
                }

            } while (!isValidChoice);

            //returns the user's selection after they have been provided the menu options.
            //Ex. employer[number user selected] will return, Toast
            return choiceKeys[choiceIdx];
        }

        //Print jobs is called when the user selects search, parameter being
        //a list of dictionaries that have data relevant to user search.
        //                             someJobs = columnChoice, searchTerm
        //                                               k          v
        private static void PrintJobs(List<Dictionary<string, string>> someJobs)
        {
            bool found = false;
            
            //foreach loops through list to get dictionary from search results
            foreach (Dictionary<string, string> jobList in someJobs)
            {
                Console.WriteLine("\n*****");
                //foreach loops through dictionary to get the key/val pair.
                foreach (KeyValuePair<string, string> job in jobList)
                {
                    Console.WriteLine("{0}: {1}", job.Key, job.Value);
                    found = true;
                }
                Console.WriteLine("*****" + "\n");
            }

            if(!found)
            {
                Console.WriteLine("Sorry, no results found.");
            }
        }
    }
}


