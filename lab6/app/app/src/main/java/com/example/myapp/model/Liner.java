package com.example.myapp.model;

import java.util.ArrayList;
import java.util.List;

public class Liner {
    public int id;
    public String name;
    public int capacity;
    public String clazz;
    public int yearBuilt;
    public List<CruiseRoute> routes = new ArrayList<>();
}