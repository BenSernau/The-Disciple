using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameMaster : MonoBehaviour {

    public static GameMaster gameMaster;
    private static string mLevelToSave;
    public static bool mtwivalbool;
    public static bool mshrwoodsbool;
    public static bool mnmpassbool;
    public static int bossCount;

    public string mouseClick = "clickButton";

    private AudioMaster audioMast;

    void Awake()
    {
        
        if (gameMaster == null)
        {
            DontDestroyOnLoad(gameObject);
            gameMaster = this;
        }
        else if (gameMaster != this)
        {
            Destroy(gameObject);
        }       
    }

    public void startGame()
    {
        AudioMaster.instance.PlaySound(mouseClick);
        mtwivalbool = true;
        mshrwoodsbool = true;
        mnmpassbool = true;
        bossCount = 0;
        SceneManager.LoadScene("tivAlef");
    }

    public void continueGame()
    {
        AudioMaster.instance.PlaySound(mouseClick);
        Load();
        if (mLevelToSave != null)
        {
            SceneManager.LoadScene(mLevelToSave);
        }
    }

    public void Save()
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/playerinfo.dat");
        playerInfo info = new playerInfo();
        info.savedLevel = SceneManager.GetActiveScene().name;
        info.twivalbool = mtwivalbool;
        info.shrwoodsbool = mshrwoodsbool;
        info.nmpassbool = mnmpassbool;
        info.bosscountstored = bossCount;
        bFormatter.Serialize(saveFile, info);
        saveFile.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream saveFile = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
            playerInfo info = (playerInfo) bFormatter.Deserialize(saveFile);
            saveFile.Close();

            mLevelToSave = info.savedLevel;
            mtwivalbool = info.twivalbool;
            mshrwoodsbool = info.shrwoodsbool;
            mnmpassbool = info.nmpassbool;
            bossCount = info.bosscountstored;
        }
    }
    
    public void quitGame()
    {
        AudioMaster.instance.PlaySound(mouseClick);
        Save();
        SceneManager.LoadScene("mainMenu");
    }

    public void exitToDesktop()
    {
        AudioMaster.instance.PlaySound(mouseClick);
        Application.Quit();
    }

    [Serializable]
    class playerInfo
    {
        public string savedLevel;
        public bool twivalbool;
        public bool shrwoodsbool;
        public bool nmpassbool;
        public int bosscountstored;
    }
}
