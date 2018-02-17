using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        
        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        //search by column choice of user.
        public static List<string> FindAll(string column)
        {
            //initialize data/list of dictionary-all data
            LoadData();

            //initialize values list/string variable
            List<string> values = new List<string>();

            //foreach loop looking at each dictionary in the list of dicts
            foreach (Dictionary<string, string> job in AllJobs)
            {
                //loop pulling the key/header/var name, column, out of the  
                //dictionary job to equal the variable name aValue/string.
                //Ex. job[Skill] (pull the string value/skill) = 
                string aValue = job[column];

                //if the list, values, do not contain the value,
                //add the value to the string list 
                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            //return the string list of 
            return values;
        }

        //FindByValue parameter passed in is user input string.
        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            // load data, if not already loaded
            LoadData();

            //initialize new list of dictionaries called allSearch
            List<Dictionary<string, string>> allSearch = new List<Dictionary<string, string>>();

            //foreach loop to parse through the list of dictionaries in AllJobs, one by one (dictionary)
            //AllJobs is from LoadData.
            foreach (Dictionary<string, string> job in AllJobs)
            {
                //for the current dictionary, job, turn the dictionary into arrays
                //?????ARE arrays more preferrential for looping??? /they are fixed size so.../Yes, faster!
                //jobDescr = the value from the dictionary(job) and turns it into an array of values from dictionary. 
                string[] jobDescr = job.Values.ToArray();

                //foreach loop to search through the array line/row 
                foreach (string jobField in jobDescr)
                {
                    //if the case insensitive/lowered array line/row equals the
                    //lowered/case insens value/input from the user,
                    //add the to the dictionary/job to the list of dictionaries/allSearch
                    if (jobField.ToLower().Equals(value.ToLower()))
                    {
                        allSearch.Add(job);
                       
                    }
                }
                
            }
            //Return the list of dictionaries of relevent search results to the user input
            return allSearch;
        }


        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs)
            {
                string aValue = row[column].ToLower();

                if (aValue.Contains(value))
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {
             //if true return/exit out of the method w/o doing anything else. if not, continue. 
            if (IsDataLoaded)
            {
                return;
            }

            //initializes rows variable as an empty list with array/string inside.
            List<string[]> rows = new List<string[]>();

            //StreamReader implements the text reader.
            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                //reader.Peek pulls the characters from the CSV file but does not read it.
                //pulls until -1/empty
                while (reader.Peek() >= 0)
                {
                    //reader.ReadLine Reads the characters pulled by Peek.
                    string line = reader.ReadLine();
                    //string array var calls CSVRow method with parameter being the line just pulled and read.
                   // CSVRowToStringArray reads through the data from CSV file and converts it to a list then to an array.
                    string[] rowArrray = CSVRowToStringArray(line);
                    //if the rowArray isn't empty, add to rows list/string variable
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }
            // makes a variable called row, which is the first array/row and removes that row from the rows array.
            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            //for each string array in rows array, add them to the dictionary as a key with the header and the value of each array/row
            foreach (string[] row in rows)
            {
                //initialzies a new Dictionary, rowDict
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                //for loop to create a dictionary with each column header as the key, and each value in the array. 
                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                //add this new dictionary to 'allJobs(initialized at the start, list of dictionaries)' 
                AllJobs.Add(rowDict);
            }

            //after everything, change IsDataLoaded to be true. if data couldn't be loaded, the method was exited at the entrance point of the method.
            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
         //passes in argument of the string array var line to be string var row. 
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            //initialize variables
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the array row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                //if the char is equal to being true or a comma, add to rowValues as a string in string builder & then clear string builder.
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    //if char is equal to \, change isBetweenQuotes to true
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }

                    //if char is not equal to \, add the char to stringBuilder.
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final values to rowValues List with the argument of the string that was just created by builder & clear out the string builder.
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            //Convert the list to an array. 
            return rowValues.ToArray();
        }
    }
}
