using UnityEngine;
using UnityWeld.Binding;
using System.ComponentModel;
using Zenject;
using System;

[Binding]
public class CreepsInfoViewModel : MonoBehaviour, INotifyPropertyChanged, IDisposable
{
    //Injections
    private IntVariable _creepsCounter;
    private IntVariable _waveCounter;
    private SignalBus _signalBus;

    //UI elements
    private string creepsAmount = string.Empty;
    private string waveNumber = string.Empty;

    [Inject]
    public void Construct(  SignalBus signalBus,
                            PlayerData playerData)
    {
        _signalBus = signalBus;
        _creepsCounter = playerData.CurrentCreepsAlive.Variable;
        _waveCounter = playerData.CurrentWave.Variable;
    }

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

    void OnEnable()
    {
        _signalBus.Subscribe<SignalNewWave>(OnNewWave);
        _signalBus.Subscribe<SignalCreepsCounterChanged>(OnCreepsChanged);

    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<SignalNewWave>(OnNewWave);
        _signalBus.Unsubscribe<SignalCreepsCounterChanged>(OnCreepsChanged);
    }

    void OnNewWave()
    {
        RefreshWaveNumber();
    }

    void OnCreepsChanged()
    {
        RefreshCreepsText();
    }

    void Start()
    {
        RefreshCreepsText();
        RefreshWaveNumber();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void RefreshCreepsText()
    {
        CreepsText = string.Format("Current Creeps: {0}", _creepsCounter.Value.ToString());
    }

    private void RefreshWaveNumber()
    {
        WaveNumber = string.Format("Wave: {0}", _waveCounter.Value.ToString());
    }
}
