using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

public class FileIO : MonoBehaviour
{
    public string dir;
    [SerializeField] FittsButtonHandler fittsButtonHandler;
    // Start is called before the first frame update
    void Start()
    {
        dir = Application.streamingAssetsPath + "/DataEntries/";
        CreateDirect();
    }
    void CreateDirect()
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }
    public void CreateFile() => File.Create(Path.Combine(dir + FileName(dir)));
    public int GetSubjectID(string directory) => Directory.GetFiles(directory).Length + 1;
    string FileName(string directory) => "Subject_" + GetSubjectID(directory).ToString() + ".csv";
    string CreateHeader() => "Subject_ID;Input Device;Current Clicks;Button Width;Button Distance;Index Of Difficulty;Time Taken; Successful Press";
    string CreateFooterResults() => "";
    string ParseDataEntry(DataEntry data) => 
          data.subjectID.ToString() + ";" 
        + data.inputType + ";" 
        + data.currentClicks.ToString() + ";"
        + data.buttonWidth.ToString() + ";" 
        + data.buttonDistance.ToString() + ";" 
        + data.indexOfDifficulty.ToString() + ";" 
        + data.timeTaken.ToString() + ";" 
        + data.successfulPress.ToString();
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
