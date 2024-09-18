using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
//using UnityEditor;

//[CustomEditor(typeof(TestBuilder))]
//public class CharacterCreatorEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        TestBuilder script = (TestBuilder)target;

//        if (GUILayout.Button("Add Character"))
//        {
//            script.characters.Add(new TestBuilder.CharacterData());
//        }

//        if (GUILayout.Button("Create Characters"))
//        {
//            script.CreateCharacters();
//        }

//        for (int i = 0; i < script.characters.Count; i++)
//        {
//            var character = script.characters[i];

//            GUILayout.Space(10);

//            character.isFoldoutOpen = EditorGUILayout.Foldout(character.isFoldoutOpen, "Character " + (i + 1), true);
//            if (character.isFoldoutOpen)
//            {
//                GUILayout.Label("Character " + (i + 1), EditorStyles.boldLabel);
//                character.characterName = EditorGUILayout.TextField("Name", character.characterName);
//                character.health = EditorGUILayout.FloatField("Health", character.health);
//                character.strength = EditorGUILayout.FloatField("Strength", character.strength);

//                // Display abilities
//                if (character.abilityNames.Count != character.abilityValues.Count)
//                {
//                    // Ensure the lists stay in sync
//                    while (character.abilityNames.Count < character.abilityValues.Count)
//                        character.abilityValues.Add(true);
//                    while (character.abilityNames.Count > character.abilityValues.Count)
//                        character.abilityNames.RemoveAt(character.abilityNames.Count - 1);
//                }

//                if (GUILayout.Button("Add Ability"))
//                {
//                    character.abilityNames.Add("");
//                    character.abilityValues.Add(true);
//                }

//                for (int j = 0; j < character.abilityNames.Count; j++)
//                {
//                    GUILayout.BeginHorizontal();
//                    character.abilityNames[j] = EditorGUILayout.TextField("Ability", character.abilityNames[j]);
//                    character.abilityValues[j] = EditorGUILayout.Toggle("Enabled", character.abilityValues[j]);
//                    GUILayout.EndHorizontal();
//                }

//                GUILayout.Space(2);
//                if (GUILayout.Button("Remove Character"))
//                {
//                    script.characters.RemoveAt(i);
//                    break; // Exit the loop to avoid invalid index issues after removal
//                }
//            }
//        }
//    }
//}
#endif




public class TestBuilder : MonoBehaviour
{

    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public float health = 100f;
        public float strength = 100f;
        public List<string> abilityNames = new List<string>();
        public List<bool> abilityValues = new List<bool>();
        [HideInInspector]
        public Character character;
        public bool isFoldoutOpen;
    }

    [SerializeField]
    [HideInInspector]
    public List<CharacterData> characters = new List<CharacterData>();

    public void CreateCharacters()
    {
        foreach (var characterData in characters)
        {
            CharacterBuilder builder = new CharacterBuilder();

            characterData.character = builder
                .SetName(characterData.characterName)
                .SetHealth(characterData.health)
                .SetStrength(characterData.strength)
                .Build();

            // Set abilities
            for (int i = 0; i < characterData.abilityNames.Count; i++)
            {
                builder.SetAbility(characterData.abilityNames[i], characterData.abilityValues[i]);
            }

            Debug.Log(characterData.character);
        }
    }


}



public class Character
{
    public string Name { get; set; }
    public float Health { get; set; }
    public float Strength { get; set; }

    public Dictionary<string, bool> Abilities { get; set; } = new Dictionary<string, bool>();

    public override string ToString()
    {
        string abilities = string.Join(", ", Abilities.Select(a => $"{a.Key}: {(a.Value ? "Yes" : "No")}"));
        return $"Name: {Name},Health: {Health}, Strength: {Strength}, Abilities: [{ abilities}]";
    }


}

public class CharacterBuilder
{
    public Character character = new Character();

    public CharacterBuilder SetName(string name)
    { 
        character.Name = name;
        return this;
    }
    
    public CharacterBuilder SetHealth(float health) 
    {
        character.Health = health; 
        return this; 
    }
    public CharacterBuilder SetStrength(float strength) 
    {
        character.Strength = strength; 
        return this; 
    }

    public CharacterBuilder SetAbility(string ability, bool hasAbility)
    {
        character.Abilities[ability] = hasAbility;
        return this;
    }


    public Character Build()
    {
        return character;
    }


}


