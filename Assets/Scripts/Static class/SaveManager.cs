using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class HighScore {
    public string levelName;
    public int score;
}

[Serializable]
public class Highscores {
    public HighScore[] highscores;
}

[Serializable]
public class Achievement {
    public string name;
    public bool unlocked;
}

[Serializable]
public class Achievements{
    public Achievement[] achievements;
}

public static class SaveManager
{
    private static Highscores highscores;
    private static Achievements achievements;

    public static void LoadSaves(){
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";

        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }

        if (!File.Exists($"{dir}/highscores.json")){
            InitHighscores();
        }
        if (!File.Exists($"{dir}/achievements.json")){
            InitAchievements();
        }

        string highscores_string = File.ReadAllText($"{dir}/highscores.json");
        highscores = JsonUtility.FromJson<Highscores>(highscores_string);

        string achievements_string = File.ReadAllText($"{dir}/achievements.json");
        achievements = JsonUtility.FromJson<Achievements>(achievements_string);
    }

    public static void InitSaves(){
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";
        
        if (!Directory.Exists(dir))
        {
            Debug.Log("Create dir");
            Directory.CreateDirectory(dir);
        }

        InitHighscores();
        InitAchievements();
    }

    private static void InitHighscores() {
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";

        HighScore[] hs = new HighScore[] {
            new HighScore {
                levelName = "prototype_level",
                score = 0
            },
            new HighScore {
                levelName = "level_1",
                score = 0
            },
            new HighScore {
                levelName = "level_2",
                score = 0
            },
            new HighScore {
                levelName = "level_3",
                score = 0
            },
            new HighScore {
                levelName = "level_4",
                score = 0
            },
            new HighScore {
                levelName = "level_5",
                score = 0
            }
        };

        Debug.Log(hs);
        Debug.Log(hs[0].levelName);

        Highscores highscores_data = new Highscores {
            highscores = hs
        };

        string highscoresJson = JsonUtility.ToJson(highscores_data, true);
        Debug.Log($"<color=red> {highscoresJson} </color>");
        File.WriteAllText($"{dir}/highscores.json", highscoresJson);
    }

    private static void InitAchievements(){
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";

        Achievements achievements_data = new Achievements {
            achievements = new Achievement[] {
                new Achievement {
                    name = "prototype_level_completed",
                    unlocked = false
                }
            }
        };

        string achievementsJson = JsonUtility.ToJson(achievements_data, true);
        File.WriteAllText($"{dir}/achievements.json", achievementsJson);
    }

    public static void RegisterHighscore(string levelName, int score) {
        LoadSaves();

        if (highscores == null) {
            Debug.Log("Saves not found");
            return;
        }

        Debug.Log(highscores.highscores);
        foreach(HighScore highscore in highscores.highscores) {
            if (highscore.levelName == levelName && highscore.score < score) {
                highscore.score = score;
            }
        }

        CommitSaves();
    }

    public static void RegisterAchievement(string levelName, int score) {
        LoadSaves();

        if (achievements == null) {
            Debug.Log("Saves not found");
            return;
        }

        if (levelName == "prototype_level")
        {
            UnlockAchievement("prototype_level_completed");
        }
        else if (levelName == "level_1")
        {

        }

        CommitSaves();
    }

    public static bool CheckAchievement(string achievementName) {
        LoadSaves();

        if (achievements == null) {
            return false;
        }

        foreach (Achievement achievement in achievements.achievements) {
            if (achievement.name == achievementName) {
                return achievement.unlocked;
            }
        }

        return false;
    }

    public static int GetHighscore(string levelName) {
        LoadSaves();

        if (highscores == null) {
            Debug.Log("Saves not found");
            return 0;
        }

        Debug.Log(highscores.highscores);
        foreach (HighScore highscore in highscores.highscores) {
            if (highscore.levelName == levelName) {
                return highscore.score;
            }
        }

        return 0;
    }

    private static void CommitSaves() {
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";

        string highscoresJson = JsonUtility.ToJson(highscores, true);
        File.WriteAllText($"{dir}/highscores.json", highscoresJson);

        string achievementsJson = JsonUtility.ToJson(achievements, true);
        File.WriteAllText($"{dir}/achievements.json", achievementsJson);
    }

    private static void UnlockAchievement(string achievementName) {
        foreach (Achievement achievement in achievements.achievements) {
            if (achievement.name == achievementName) {
                achievement.unlocked = true;
            }
        }
    }
}
