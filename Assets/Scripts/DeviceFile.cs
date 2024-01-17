using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceFile
{
    public int FileId { get; set; }
    public float Progress { get; set; }

    public DeviceFile(int fileId, float progress)
    {
        FileId = fileId;
        Progress = progress;
    }
}
