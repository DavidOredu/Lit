using DapperDino.Tutorials.Lobby;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class CustomSerializer : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        MapSet mapSet = (MapSet)obj;
        //foreach (var map in mapSet.Maps)
        //{
        //    info.AddValue(, map);
        //}
        info.AddValue("maps", mapSet.Maps);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        MapSet mapSet = (MapSet)obj;
     //   mapSet.Maps = (List<string>)info.GetValue("maps", typeof(List<string>));
        obj = mapSet;
        return obj;
    }
}
