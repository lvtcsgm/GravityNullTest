using UnityEngine;
using System.Collections;
using System;

public class Objective : IComparable<Objective>
{
    public string identifier;
    public string objective;
    public bool completed = false;
    public float timeCreated = 0;

    public Objective(string id, string objText, bool isComplete)
    {
        identifier = id;
        objective = objText;
        completed = isComplete;
        timeCreated = Time.time;
    }

    public int CompareTo(Objective other)
    {
        if (timeCreated > other.timeCreated)
            return 1;
        else
            return -1;
    }
}
