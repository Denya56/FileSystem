using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Size { get; set; }

    // 0 - available
    // 1 - taken
    // Used to define whether the file is being downloaded or no
    public int State { get; set; }

    public File(int id, string name, int size, int state)
    {
        Id = id;
        Name = name;
        Size = size;
        State = state;
    }

    private List<IFileObserver> fileObservers = new List<IFileObserver>();

    public void SubscribeDevice(IFileObserver observer)
    {
        if (!fileObservers.Contains(observer))
        {
            fileObservers.Add(observer);
        }
    }

    public void UnsubscribeDevice(IFileObserver observer)
    {
        fileObservers.Remove(observer);
    }

    public void NotifyDevices()
    {
        foreach (var observer in fileObservers)
        {
            observer.UpdateObserver(this);
        }
    }
}
