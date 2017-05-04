using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noun
{

    private bool isProper; //This variable determines whether the noun is a proper noun.
    private string singular; //This variable is always the same as the name of the class instance.
    private string plural; //This variable is usually the same as the name of the class instance plus 's'.
    private bool beginsWithVowel;

    public Noun(string _singular, bool _beginsWithVowel) //This constructor is for most nouns.
    {
        
        isProper = false;
        singular = _singular;
        plural = _singular + "s";
        beginsWithVowel =_beginsWithVowel;
    }

    public Noun(bool _isProper, string _singular, bool _beginsWithVowel)
    {
        isProper = _isProper;
        singular = _singular;
        plural = _singular + "s";
        beginsWithVowel = _beginsWithVowel;
    }

    public Noun(bool _isProper, string _singular, string _plural, bool _beginsWithVowel) //This constructor is more complete than the previous one.
    {
        isProper = _isProper;
        singular = _singular;
        plural = _plural;
        beginsWithVowel = _beginsWithVowel;
        //usesArticle0 = _usesArticle0;
    }

    public string nounToString()
    {
        return "isProper: " + isProper + " singular: " + singular + " plural: " + plural;
    }

    public string getSingular()
    {
        return singular;
    }

    public string getPlural()
    {
        return plural;
    }

    public bool getIsProper()
    {
        return isProper;
    }

    public bool getBeginsWithVowel()
    {
        return beginsWithVowel;
    }
}
