using UnityEngine;
using UnityWeld.Binding;
using System.ComponentModel;
using Zenject;

[Binding]
public class CreepsInfoViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private CreepsManager _creepsManager;
    private WaveSpawner _waveSpawner;
    private SignalBus _signalBus;

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
                            WaveSpawner waveSpawner,
                            SignalBus signalBus)
    {
        _waveSpawner = waveSpawner;
        _creepsManager = creepsManager;
        _signalBus = signalBus;
    }

    void OnEnable()
    {
        _signalBus.Subscribe<SignalNewWave>(OnNewWave);
        _signalBus.Subscribe<SignalCreepDied>(OnCreepsChanged);
        _signalBus.Subscribe<SignalCreepSpawned>(OnCreepsChanged);

    }

    void OnDisable()
    {
        _signalBus.Unsubscribe<SignalNewWave>(OnNewWave);
        _signalBus.Unsubscribe<SignalCreepDied>(OnCreepsChanged);
        _signalBus.Unsubscribe<SignalCreepSpawned>(OnCreepsChanged);
    }

    void OnNewWave()
    {
        SetWaveNumber();
    }

    void OnCreepsChanged()
    {
        SetCreepsText();
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
