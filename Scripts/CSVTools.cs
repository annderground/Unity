using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// this class converts our Data in a CSV format
/// and saves it in a CSV file
/// </summary>
public class CSVTools
{
    /*public static List<string> CreateCsvTable(List<List<string>> data)
    {
        List<string> csvHeader = new List<string>();
        for (int i =1; i > 5;i++)
        {
           csvHeader.Add("Orange Data Point " ); 
        }
        
        for (int i =1; i > 5;i++)
        {
            csvHeader.Add("Blue Data Point " ); 
        }

        foreach (string name in csvHeader)
        {
            Debug.Log(name);
        }
        
        return CreateCsv(data, csvHeader);
    }*/
    
    /// <summary>
    /// this method is called to convert our Data into a CSV format
    /// </summary>
    /// <param name="data"> all of our Data (recording of collision time with the data points and targets</param>
    /// <param name="header"> these are our headers to tell us which time belongs to which data point</param>
    /// <param name="seperator"> the separator (seperator) is needed for the CSV format</param>
    /// <returns></returns>
    public static List<string> CreateCsv(List<List<string>> data, List<string> header, char seperator = ';')
    {

        //initiate List of strings to save our data in a CSV format 
        List<string> prossesedData = new List<string>();

        //initiate string to save our headers as 1 string in CSV format 
        string csvHeader = "";
        
        //convert our header into CSV format
        foreach (string eachHeader in header)
        {
            csvHeader = csvHeader + eachHeader + seperator;
        }
        
        //adds our header to our CSV formatted List
        prossesedData.Add(csvHeader);

        //go through our data row by row and convert row into 1 CSV formatted string and add it to our CSV formatted list
        foreach (List<string> row in data)
        {
            // string to convert our row into
            string perfectRow = "";
            
            //convert row into CSV formatted string
            foreach (string dataPoint in row)
            {
                perfectRow =  perfectRow + dataPoint + seperator;
            }
            
            // add CSV formatted string to CSV list
            prossesedData.Add(perfectRow);
        }
        
        // return converted data as CSV list
        return prossesedData;
    }

    /// <summary>
    /// this method is called to save our formatted data into a CSV file
    /// </summary>
    /// <param name="finalData"> this is our formatted data of all rounds</param>
    /// <param name="fileAderess"> this is where the file is saved</param>
    /// <param name="extension"> defines the format of the file (here per default set to .csv)</param>
    /// <returns>true if file is successfully saved</returns>
    public static bool SaveData(List<string> finalData, string fileAderess, string extension = ".csv")
    {
        // try the following
        try
        {
            // initiate the Writer that writes the data into the file
            // use using so the Writer will definitely be closed afterwards
            using (StreamWriter csvWriter = new StreamWriter(fileAderess + extension))
            {
                //line by line writhing the data into the file
                foreach (string row in finalData)
                {
                    csvWriter.WriteLine(row);
                }
                // closing the writer
                csvWriter.Close();
            
            }
        }
        // catch possible errors
        catch (Exception e)
        {
            // tell us what error occured
            Debug.LogError(e);
            throw;
        }
        // return true if saved successfully
        return true;
    }
}
