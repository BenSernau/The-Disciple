using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verb {

    private string infinitive = "to verb";
    private string root = "verb";
    private string tpSingular = "it verbs";
    private string presentParticiple = "verbing";
    private string past = "verbed";
    private string pastParticiple = "verben";

    public Verb(string _root)
    {
        infinitive = "to " + _root;
        root = _root;
        tpSingular = _root + "s";
        presentParticiple = _root + "ing";
        past = _root + "ed";
        pastParticiple = past;
    }

    public Verb(string _root, string _past)
    {
        infinitive = "to " + _root;
        root = _root;
        tpSingular = _root + "s";
        presentParticiple = _root + "ing";
        past = _past;
        pastParticiple = _past;
    }

    public Verb(string _root, string _presentParticiple, string _past)
    {
        infinitive = "to " + _root;
        root = _root;
        tpSingular = _root + "s";
        presentParticiple = _presentParticiple;
        past = _past;
        pastParticiple = _past;
    }

    public Verb (string _root, string _presentParticiple, string _past, string _pastParticiple)
    {
        infinitive = "to " + _root;
        root = _root;
        tpSingular = _root + "s";
        presentParticiple = _presentParticiple;
        past = _past;
        pastParticiple = _pastParticiple;
    }

    public Verb(string _infinitive, string _root, string _tpSingular, string _presentParticiple, string _past, string _pastParticiple)
    {
        infinitive = _infinitive;
        root = _root;
        tpSingular = _tpSingular;
        presentParticiple = _presentParticiple;
        past = _past;
        pastParticiple = _pastParticiple;
    }

    public string verbToString()
    {
        return "infinitive: " + infinitive + " root: " + root + " tpSingular: " + tpSingular + " presentParticiple: " + presentParticiple + " past: " + past + " pastParticiple: " + pastParticiple;
    }

    public string getInfinitive()
    {
        return infinitive;
    }

    public string getRoot()
    {
        return root;
    }

    public string getTpSingular()
    {
        return tpSingular;
    }

    public string getPresentParticiple()
    {
        return presentParticiple;
    }

    public string getPast()
    {
        return past;
    }

    public string getPastParticiple()
    {
        return pastParticiple;
    }
}
