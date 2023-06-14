using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class HighScore {
    public string levelName;
    public int score;
}

public class Highscores {
    public HighScore[] highscores;
}

public class Achievement {
    public string achievementName;
    public bool unlocked;
}

public class Achievements{
    public Achievement[] achievements;
}

public static class SaveManager
{
    private static Highscores highscores;
    private static Achievements achievements;

    public static void LoadSave(){
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";

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

    public static void InitSave(){
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";
        
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        InitHighscores();
        InitHighscores();
    }

    private static void InitHighscores() {
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";

        Highscores highscores_data = new Highscores {
            highscores = new HighScore[] {
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
            }
        };

        string highscoresJson = JsonUtility.ToJson(highscores_data, true);
        File.WriteAllText($"{dir}/Catnipped/highscores.json", highscoresJson);
    }

    private static void InitAchievements(){
        string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/Catnipped";

        Achievements achievements_data = new Achievements {
            achievements = new Achievement[] {

            }
        };

        string achievementsJson = JsonUtility.ToJson(achievements_data, true);
        File.WriteAllText($"{dir}/Catnipped/achievements.json", achievementsJson);
    }
}
