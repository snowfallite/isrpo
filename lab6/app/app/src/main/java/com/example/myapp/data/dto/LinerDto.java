package com.example.myapp.data.dto;

import java.util.List;

public class LinerDto {
    public int id;
    public String name;
    public int capacity;
    public String _class; // if your JSON uses "class", map via @SerializedName
    public int yearBuilt;
    public List<RouteBriefDto> routes;
}
