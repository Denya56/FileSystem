using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Device : IFileObserver
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Download { get; set; }
    public List<DeviceFile> Files { get; set; }
    public File currentFile { get; set; } = null;

    private bool isDownloading = false;
    private DeviceFile currentDevFile { get; set; }

    public Device(int id, string name, int download, List<DeviceFile> files)
    {
        Id = id;
        Name = name;
        Download = download;
        Files = files;
    }

    public void AddNewFile(File file)
    {
        if(!Files.Any(df => df.FileId == file.Id))
        {
            Files.Add(new DeviceFile(file.Id, 0));
        }
    }

    public void UpdateObserver(File file)
    {
        if (currentFile == null)
        {
            currentDevFile = Files.Find(df => df.FileId == file.Id);
            if (file.State == 0 & currentDevFile.Progress < 1f)
            {
                file.State = 1;
                StartDownload(file);
            }
        }
    }

    public void StartDownload(File file)
    {
        currentFile = file;
        isDownloading = true;
        Debug.Log($"Starting download! Device: {Name}   File: {file.Name}");
    }

    public void DownloadFile(float deltaTime)
    {
        if (isDownloading && currentFile != null)
        {
            currentDevFile.Progress += Download * deltaTime / currentFile.Size;

            if (currentDevFile.Progress >= 1f)
            {
                FinishDownload();
            }
        }
    }
    private void FinishDownload()
    {
        Debug.Log($"Finished download! Device: {Name}   File: {currentFile.Name}");
        // set current file state to 0 - file is available for download by other devices
        currentFile.State = 0;

        // unsubscribe from  observing the file. Prevents getting updates from already downloaded files
        currentFile.UnsubscribeDevice(this);


        // Reset download-related variables
        currentFile = null;
        isDownloading = false;
    }
}
