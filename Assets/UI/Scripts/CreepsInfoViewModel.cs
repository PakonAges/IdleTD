using UnityEngine;
using UnityWeld.Binding;
using System.ComponentModel;
using Zenject;

[Binding]
public class CreepsInfoViewModel : MonoBehaviour, INotifyPropertyChanged
{
    //Injections
    private PlayerData _playerData;
    private SignalBus _signalBus;

    //UI elements
    private string creepsAmount = string.Empty;
    private string waveNumber = string.Empty;

    [Inject]
    public void Construct(  SignalBus signalBus,
                            PlayerData playerData)
    {
        _signalBus = signalBus;
        _playerData = playerData;
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
        CreepsText = string.Format("Current Creeps: {0}", _playerData.CurrentCreepsAlive.Value.ToString());
    }

    private void SetWaveNumber()
    {
        WaveNumber = string.Format("Wave: {0}", _playerData.CurrentWave.Value.ToString());
    }
}
