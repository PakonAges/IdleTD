using UnityEngine;
using UnityWeld.Binding;
using System.ComponentModel;
using Zenject;

[Binding]
public class CreepsInfoViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private CreepsManager _creepsManager;
    private WaveSpawner _waveSpawner;

    private string creepsAmount = string.Empty;
    private string waveNumber = string.Empty;

    [Binding]
    public string CreepsText
    {
        get { return creepsAmount; }
        set
        {
            if (creepsAmount == value)
            {
                return;
            }
            creepsAmount = value;
            OnPropertyChanged("CreepsText");
        }
    }

    [Binding]
    public string WaveNumber
    {
        get { return waveNumber; }
        set
        {
            if (waveNumber == value)
            {
                return;
            }
            waveNumber = value;
            OnPropertyChanged("WaveNumber");
        }
    }

    [Inject]
    public void Construct(  CreepsManager creepsManager,
                            WaveSpawner waveSpawner)
    {
        _waveSpawner = waveSpawner;
        _creepsManager = creepsManager;
    }

    void Start()
    {
        SetCreepsText();
        SetWaveNumber();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetCreepsText()
    {
        CreepsText = string.Format("Current Creeps: {0}", _waveSpawner.CreepsAlive.Count.ToString());
    }

    private void SetWaveNumber()
    {
        WaveNumber = string.Format("Wave: {0}", _creepsManager.DisplayWaveNum.ToString());
    }
}
