package com.example.myapp.model;

import java.util.ArrayList;
import java.util.List;

public class CruiseRoute {
    public int id;
    public String name;
    public int durationDays;
    public List<LinerBrief> liners = new ArrayList<>();

    // Local brief model for displaying liner names under route
    public static class LinerBrief {
        public int id;
        public String name;
    }
}