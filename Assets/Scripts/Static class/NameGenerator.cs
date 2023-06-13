using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    private static bool namesLoaded = false;
    private static List<string> words;
    // private static List<string> generatedNames;

    public static string GenerateRandomName(List<string> blacklist = null){
        LoadNames();

        HashSet<string> blacklistSet = null;
        if (blacklist != null) {
            blacklistSet = new HashSet<string>(blacklist);
        }

        string result = GenerateName();
        while(blacklistSet != null && blacklistSet.Contains(result)) {
            result = GenerateName();
        }

        return result;
    }

    private static void LoadNames(){
        if (!namesLoaded){

            words = new List<string>();
            string textFile = Resources.Load<TextAsset>("names").ToString();
            //words = new List<string>(textFile.Split("\n"));
            foreach(string s in textFile.Split("\n")) {
                words.Add(s.Replace("\r", ""));
            }
            namesLoaded = true; 
        }
    }

    private static string GenerateName() {
        string word1 = words[Random.Range(0, words.Count)];
        string word2 = words[Random.Range(0, words.Count)];
        while (word2 == word1) {
            word2 = words[Random.Range(0, words.Count)];
        }
        string generatedName = $"{word1}-{word2}";
        return generatedName;
    }
}
