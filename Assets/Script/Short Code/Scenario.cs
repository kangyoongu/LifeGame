using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Node
{
    public string dialogue;

    [Header("누가말하는지")]
    public bool me;
    public List<Node> nextNode;
    public Node(string dialogue, List<Node> next)
    {
        this.dialogue = dialogue;
        nextNode = next;
    }
    public Node(string dialogue)
    {
        this.dialogue = dialogue;
        nextNode = new List<Node>();
    }
}
[Serializable]
public struct NodeRoot
{
    public Node node;
    public Vector2 friend;
    public int result;
}
public enum Wording : short
{
    Friend,
    Goback,
    GirlFriend,
    Farewell,
    Marry,
    Wife,
    Divorce,
    Kill
}

[Serializable]
public struct Scenario
{
    public Wording wording;
    public List<NodeRoot> node;
}
