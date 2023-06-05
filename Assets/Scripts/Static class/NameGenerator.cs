using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    private static bool namesLoaded = false;
    private static List<string> words;
    // private static List<string> generatedNames;

    public static string GenerateRandomName(){
        LoadNames();

        string word1 = words[Random.Range(0, words.Count)];
        string word2 = words[Random.Range(0, words.Count)];
        while(word2 == word1){
            word2 = words[Random.Range(0, words.Count)];
        }
        string generatedName = $"{word1}-{word2}";

        //CHECK IF GENERATED NAME ALREADY EXISTS ON THE SERVER

        return generatedName;
    }

    private static void LoadNames(){
        if(!namesLoaded){
            string textFile = Resources.Load<TextAsset>("names").ToString();
            words = new List<string>(textFile.Split("\n"));
            namesLoaded = true; 
        }
    }
}
