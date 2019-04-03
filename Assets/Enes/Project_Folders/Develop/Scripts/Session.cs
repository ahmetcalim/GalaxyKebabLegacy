using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session
{
    public DateTime time;
    public double dailyPopularity;
    public List<SessionCustomer> sessionsCustomers;

    public void Activate()
    {
        time = DateTime.Now;
        sessionsCustomers = new List<SessionCustomer>();
    }
}
public class SessionCustomer
{
    public string name;
    public double average;

    public SessionCustomer(string _name,double _average)
    {
        this.name = _name;
        this.average = _average;
    }
}

