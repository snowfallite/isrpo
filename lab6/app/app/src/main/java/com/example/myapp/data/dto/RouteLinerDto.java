package com.example.myapp.data.dto;

import java.math.BigDecimal;

public class RouteLinerDto {
    public int routeId;
    public int linerId;
    public String season;
    public double basePrice;

    public RouteLinerDto(int routeId, int linerId, String season, double basePrice) {
        this.routeId = routeId;
        this.linerId = linerId;
        this.season = season;
        this.basePrice = basePrice;
    }
}


