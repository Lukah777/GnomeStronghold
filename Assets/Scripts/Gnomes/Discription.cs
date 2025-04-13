using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

// Discription.cs
// Josiah Nistor
// Generates the discription for each Gnome
public class Discription : MonoBehaviour
{
    [SerializeField] private Personality m_personality;
    [SerializeField] private Flaw m_flaw;
    [SerializeField] private List<string> m_firstName = new List<string>() { "Lorug ", "Kelbis ", "Gradon ", "Calkas ", "Kelrug ", "Ornoa ", "Zanimyra ", "Helphina ", "Heldysa ", "Urigyra " };
    [SerializeField] private List<string> m_lastName = new List<string>() { "Quietcord", "Shadowbit", "Darkboots", "Luckybadge", "Gobblefront", "Sparklestand", "Fiddlecloak", "Applecollar", "Starkgrace", "Lighttwist" };
    [SerializeField] private List<string> m_hair = new List<string>() { "black", "brown", "blond", "gray", "red" };
    [SerializeField] private List<string> m_expression = new List<string>() { "stern", "scared", "relaxed", "tight", "lively" };
    [SerializeField] private List<string> m_eyes = new List<string>() { "blue", "green", "brown", "hazel" };
    [SerializeField] private List<string> m_build = new List<string>() { "strong", "skinny", "heavy", "average", "frail" };
    [SerializeField] private List<string> m_height = new List<string>() { "above average in height", "tall", "averagein height", "short", "below averagein height" };

    private string m_name;
    private string m_discription;

    // Randomize discription attributes
    private void Start()
    {
        int randFirstName = Random.Range(0, m_firstName.Count);
        int randLastName = Random.Range(0, m_lastName.Count);
        m_name = m_firstName[randFirstName] + m_lastName[randLastName];
        int randHair = Random.Range(0, m_hair.Count);
        int randExpression = Random.Range(0, m_expression.Count);
        int randEyes = Random.Range(0, m_eyes.Count);
        int randBuild = Random.Range(0, m_build.Count);
        int randHeight = Random.Range(0, m_height.Count);
        m_discription = m_name + " has large " + m_eyes[randEyes] + " eyes, and " + m_hair[randHair] + " hair, their face is " + m_expression[randExpression] + ". They have a " + m_build[randBuild] + " build and are " + m_height[randHeight] + ". Their personality is " + m_personality.GetPersonality() + ", though they are also " + m_flaw.GetFlaw();

        gameObject.name = m_name;
    }

    // Getters
    public string GetName() { return m_name; }
    public string GetDiscription() { return m_discription; }
}
