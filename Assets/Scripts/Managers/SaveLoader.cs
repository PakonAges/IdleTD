public class SaveLoader {

    //Temporary make hard refference from the insperctor
    //Later Get Save data from save file
    //Make it debuggable! So I could put any save data, or see game data in inspector
    public readonly PlayerSaveData _playerSaveData;

    public SaveLoader(PlayerSaveData playerSaveData)
    {
        //check for save
        //If it is exist -> Parse data to local Data
        //If not -> Create new one

        _playerSaveData = playerSaveData;
    }

    public MapGenerationData GetMapData()
    {
        return _playerSaveData.MapData;
    }
}
