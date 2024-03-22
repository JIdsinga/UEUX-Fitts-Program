using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

public class FileIO : MonoBehaviour
{
    // Filepath to the Directory
    public string dir; 
    // References
    [SerializeField] FittsButtonHandler fittsButtonHandler;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize Filepath
        dir = Application.streamingAssetsPath + "/DataEntries/";
        CreateDirect();
    }
    // Creates a Directory if it doesn't exist
    void CreateDirect()
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }
    // Returns the ID of the subject of the experiment
    public int GetSubjectID(string directory) => Directory.GetFiles(directory).Length + 1;
    // Returns the File name of the file for a subject and initializes CSV file type
    string FileName(string directory) => "Subject_" + GetSubjectID(directory).ToString() + ".csv";
    // Creates the header for the CSV file
    string CreateHeader() => "Subject_ID;Input Device;Current Clicks;Button Width;Button Distance;Index Of Difficulty;Time Taken; Successful Press";
    // Creates the footer for the CSV file
    string CreateFooterResults() => "";
    // Parses the data from a Data Entry into a line that adheres to the CSV format
    string ParseDataEntry(DataEntry data) => 
          data.subjectID.ToString() + ";" 
        + data.inputType + ";" 
        + data.currentClicks.ToString() + ";"
        + data.buttonWidth.ToString() + ";" 
        + data.buttonDistance.ToString() + ";" 
        + data.indexOfDifficulty.ToString() + ";" 
        + data.timeTaken.ToString() + ";" 
        + data.successfulPress.ToString();
    // Writes all of the Data Entries to a file in the directory
    public void WriteToFile()
    {
        using (StreamWriter writer = new StreamWriter(Path.Combine(dir + FileName(dir))))
        {
            writer.WriteLine(CreateHeader());
            foreach (DataEntry data in fittsButtonHandler.dataEntries)
            {
                writer.WriteLine(ParseDataEntry(data));
            }
            writer.WriteLine(CreateFooterResults());
        }
    }
}
