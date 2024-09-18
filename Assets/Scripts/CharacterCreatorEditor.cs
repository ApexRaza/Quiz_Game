#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestBuilder))]
public class CharacterCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestBuilder script = (TestBuilder)target;

        if (GUILayout.Button("Add Character"))
        {
            script.characters.Add(new TestBuilder.CharacterData());
        }

        if (GUILayout.Button("Create Characters"))
        {
            script.CreateCharacters();
        }

        for (int i = 0; i < script.characters.Count; i++)
        {
            var character = script.characters[i];

            GUILayout.Space(10);

            character.isFoldoutOpen = EditorGUILayout.Foldout(character.isFoldoutOpen, "Character " + (i + 1), true);
            if (character.isFoldoutOpen)
            {
                GUILayout.Label("Character " + (i + 1), EditorStyles.boldLabel);
                character.characterName = EditorGUILayout.TextField("Name", character.characterName);
                character.health = EditorGUILayout.FloatField("Health", character.health);
                character.strength = EditorGUILayout.FloatField("Strength", character.strength);

                // Display abilities
                if (character.abilityNames.Count != character.abilityValues.Count)
                {
                    // Ensure the lists stay in sync
                    while (character.abilityNames.Count < character.abilityValues.Count)
                        character.abilityValues.Add(true);
                    while (character.abilityNames.Count > character.abilityValues.Count)
                        character.abilityNames.RemoveAt(character.abilityNames.Count - 1);
                }

                if (GUILayout.Button("Add Ability"))
                {
                    character.abilityNames.Add("");
                    character.abilityValues.Add(true);
                }

                for (int j = 0; j < character.abilityNames.Count; j++)
                {
                    GUILayout.BeginHorizontal();
                    character.abilityNames[j] = EditorGUILayout.TextField("Ability", character.abilityNames[j]);
                    character.abilityValues[j] = EditorGUILayout.Toggle("Enabled", character.abilityValues[j]);
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(2);
                if (GUILayout.Button("Remove Character"))
                {
                    script.characters.RemoveAt(i);
                    break; // Exit the loop to avoid invalid index issues after removal
                }
            }
        }
    }
}
#endif