using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Model : MonoBehaviour
{
    public List<File> Files { get; set; }
    public List<Device> Devices { get; set; }
    public Transform FileButtonsHolder { get; set; }
    public Transform DevicesHolder { get; set; }
    public GameObject FileButtonPrefab;
    public GameObject DeviceFileButtonPrefab;
    public GameObject DevicePrefab;

    private void Awake()
    {
        Files = new List<File>()
        {
            new File(1, "notavirus.exe", 20971520, 0),
            new File(2, "deathstarblueprint.pdf", 10485760, 0),
            new File(3, "peterdinklagenudes.zip", 31457280, 0)
        };

        Devices = new List<Device>()
        {
            new Device(1, "Smartphone", 1048576, new List<DeviceFile>()
            {
                new DeviceFile(1, 1f)
            }),
            new Device(2, "Tablet", 2097152, new List<DeviceFile>()
            {
                new DeviceFile(2, 1f)
            }),
            new Device(3, "VR Goggles", 1572864, new List<DeviceFile>()
            {
                new DeviceFile(3, 1f)
            })
        };

        FileButtonsHolder = GameObject.Find("FileButtonsHolder").transform;
        DevicesHolder = GameObject.Find("DevicesHolder").transform;
    }
    private void Start()
    {
        CreateFileButtons();
        CreateDevicesUI();
    }

    private void Update()
    {
        foreach (var file in Files)
        {
            if (file.State == 0)
            {
                file.NotifyDevices();
            }
        }

        foreach (var device in Devices)
        {
            foreach (var deviceFile in device.Files)
            {
                GameObject deviceUI = GameObject.Find(device.Name);
                UpdateProgressBar(deviceFile, deviceUI);
            }
            device.DownloadFile(Time.deltaTime);
        }
    }

    private void UpdateProgressBar(DeviceFile deviceFile, GameObject deviceUI)
    {
        Transform fileButtonsHolder = deviceUI.transform.Find("DeviceFileList");
        File file = Files.Find(f => f.Id == deviceFile.FileId);
        var button = fileButtonsHolder.Find(file.Name);
        
        Image progressImage = button.Find("ProgressBar").GetComponent<Image>();

        if (progressImage != null)
        {
            progressImage.fillAmount = deviceFile.Progress;
        }
    }

    private void CreateFileButtons()
    {
        foreach (File file in Files)
        {
            GameObject fileButtonUI = Instantiate(FileButtonPrefab, FileButtonsHolder);

            TMP_Text buttonText = fileButtonUI.transform.GetChild(0).GetComponent<TMP_Text>();
            buttonText.text = file.Name;


            Button button = fileButtonUI.GetComponent<Button>();
            button.onClick.AddListener(() => OnFileButtonClick(file, buttonText, button));
        }
    }

    private void CreateDevicesUI()
    {
        foreach (var device in Devices)
        {
            GameObject deviceUI = Instantiate(DevicePrefab, DevicesHolder);
            deviceUI.name = device.Name;

            TMP_Text deviceNameText = deviceUI.transform.Find("DeviceName").GetComponent<TMP_Text>();
            deviceNameText.text = device.Name;

            Transform fileButtonsHolder = deviceUI.transform.Find("DeviceFileList");
            foreach (DeviceFile deviceFile in device.Files)
            {
                CreateDeviceFileListButton(deviceFile, fileButtonsHolder);
            }           
        }
    }

    private void CreateDeviceFileListButton(DeviceFile deviceFile, Transform fileButtonsHolder)
    {
        File file = Files.Find(f => f.Id == deviceFile.FileId);
        if (file != null)
        {
            GameObject deviceFileButtonUI = Instantiate(DeviceFileButtonPrefab, fileButtonsHolder);
            deviceFileButtonUI.name = file.Name;

            TMP_Text buttonText = deviceFileButtonUI.transform.GetChild(0).GetComponent<TMP_Text>();
            buttonText.text = file.Name;
            

            Image progressImage = deviceFileButtonUI.transform.Find("ProgressBar").GetComponent<Image>();

            Button button = deviceFileButtonUI.GetComponent<Button>();
        }
    }

    private void OnFileButtonClick(File selectedFile, TMP_Text buttonText, Button button)
    {
        // workaround of seelcting multiple buttons. When a File button is selected disable it (disable color is the same as selecteds)
        button.interactable = false;
        buttonText.color = Color.white;
        foreach (Device device in Devices)
        {
            selectedFile.SubscribeDevice(device);

            if (!device.Files.Any(df => df.FileId == selectedFile.Id))
            {
                DeviceFile tmp = new DeviceFile(selectedFile.Id, 0);
                device.Files.Add(tmp);

                GameObject deviceUI = GameObject.Find(device.Name);
                Transform fileButtonsHolder = deviceUI.transform.Find("DeviceFileList");
                CreateDeviceFileListButton(tmp, fileButtonsHolder);
            }
        }
    }
}
