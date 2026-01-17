package com.example.myapp.data.mapper;

import com.example.myapp.data.dto.*;
import com.example.myapp.model.*;

import java.util.stream.Collectors;

public class DtoMappers {
    public static Liner toModel(LinerDto dto) {
        Liner m = new Liner();
        m.id = dto.id;
        m.name = dto.name;
        m.capacity = dto.capacity;
        m.clazz = dto._class;
        m.yearBuilt = dto.yearBuilt;
        if (dto.routes != null) {
            m.routes = dto.routes.stream().map(rb -> {
                CruiseRoute cr = new CruiseRoute();
                cr.id = rb.id;
                cr.name = rb.name;
                return cr;
            }).collect(Collectors.toList());
        }
        return m;
    }

    public static LinerDto toDto(Liner m) {
        LinerDto dto = new LinerDto();
        dto.id = m.id;
        dto.name = m.name;
        dto.capacity = m.capacity;
        dto._class = m.clazz;
        dto.yearBuilt = m.yearBuilt;
        return dto;
    }

    public static CruiseRoute toModel(RouteDto dto) {
        CruiseRoute r = new CruiseRoute();
        r.id = dto.id;
        r.name = dto.name;
        r.durationDays = dto.durationDays;
        if (dto.liners != null) {
            r.liners = dto.liners.stream().map(lb -> {
                CruiseRoute.LinerBrief l = new CruiseRoute.LinerBrief();
                l.id = lb.id;
                l.name = lb.name;
                return l;
            }).collect(Collectors.toList());
        }
        return r;
    }

    public static RouteDto toDto(CruiseRoute r) {
        RouteDto dto = new RouteDto();
        dto.id = r.id;
        dto.name = r.name;
        dto.durationDays = r.durationDays;
        return dto;
    }

    public static RouteLiner toModel(RouteLinerDto dto) {
        RouteLiner rl = new RouteLiner();
        rl.routeId = dto.routeId;
        rl.linerId = dto.linerId;
        rl.season = dto.season;
        rl.basePrice = dto.basePrice;

        return rl;
    }

}