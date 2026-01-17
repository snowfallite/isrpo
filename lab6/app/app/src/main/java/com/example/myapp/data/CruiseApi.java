package com.example.myapp.data;

import com.example.myapp.data.dto.*;

import java.util.List;
import retrofit2.Call;
import retrofit2.http.*;

public interface CruiseApi {

    // Liners
    @GET("api/liners")
    Call<List<LinerDto>> getLiners();

    @GET("api/liners/{id}")
    Call<LinerDto> getLiner(@Path("id") int id);

    @POST("api/liners")
    Call<LinerDto> createLiner(@Body LinerDto dto);

    @PUT("api/liners/{id}")
    Call<Void> updateLiner(@Path("id") int id, @Body LinerDto dto);

    @DELETE("api/liners/{id}")
    Call<Void> deleteLiner(@Path("id") int id);

    // Routes
    @GET("api/routes")
    Call<List<RouteDto>> getRoutes();

    @GET("api/routes/{id}")
    Call<RouteDto> getRoute(@Path("id") int id);

    // RouteLiners
    @GET("api/routeliners")
    Call<List<RouteLinerDto>> getRouteLiners();

    @POST("api/routeliners")
    Call<RouteLinerDto> createRouteLiner(@Body RouteLinerDto dto);

    @PUT("api/routeliners/{routeId}/{linerId}")
    Call<Void> updateRouteLiner(@Path("routeId") int routeId,
                                @Path("linerId") int linerId,
                                @Body RouteLinerDto dto);

    @DELETE("api/routeliners/{routeId}/{linerId}")
    Call<Void> deleteRouteLiner(@Path("routeId") int routeId,
                                @Path("linerId") int linerId);

    @POST("api/routes")
    Call<RouteDto> createRoute(@Body RouteDto dto);

    @PUT("api/routes/{id}")
    Call<Void> updateRoute(@Path("id") int id, @Body RouteDto dto);

    @DELETE("api/routes/{id}")
    Call<Void> deleteRoute(@Path("id") int id);

    // Linking
    @POST("api/routes/{routeId}/liners/{linerId}")
    Call<Void> addLinerToRoute(@Path("routeId") int routeId, @Path("linerId") int linerId);

    @DELETE("api/routes/{routeId}/liners/{linerId}")
    Call<Void> removeLinerFromRoute(@Path("routeId") int routeId, @Path("linerId") int linerId);
}