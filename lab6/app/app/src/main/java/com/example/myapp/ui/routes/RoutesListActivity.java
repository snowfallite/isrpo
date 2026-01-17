package com.example.myapp.ui.routes;

import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.example.myapp.R;
import com.example.myapp.data.ApiClient;
import com.example.myapp.data.CruiseApi;
import com.example.myapp.data.dto.RouteDto;
import com.example.myapp.data.mapper.DtoMappers;
import com.example.myapp.model.CruiseRoute;
import com.example.myapp.ui.common.UiUtils;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import java.util.ArrayList;
import java.util.List;
import retrofit2.*;

public class RoutesListActivity extends AppCompatActivity {
    private RoutesAdapter adapter;
    private CruiseApi api;

    @Override
    protected void onCreate(Bundle b) {
        super.onCreate(b);
        setContentView(R.layout.activity_routes_list);

        RecyclerView rv = findViewById(R.id.recyclerRoutes);
        rv.setLayoutManager(new LinearLayoutManager(this));
        adapter = new RoutesAdapter(new ArrayList<>(), route -> {
            Intent i = new Intent(this, RouteDetailActivity.class);
            i.putExtra("routeId", route.id);
            startActivity(i);
        });
        rv.setAdapter(adapter);

        FloatingActionButton fab = findViewById(R.id.fabAddRoute);
        fab.setOnClickListener(v -> startActivity(new Intent(this, RouteEditActivity.class)));

        api = ApiClient.get(getString(R.string.base_url)).create(CruiseApi.class);
    }

    @Override
    protected void onResume() {
        super.onResume();
        load();
    }

    private void load() {
        api.getRoutes().enqueue(new Callback<List<RouteDto>>() {
            @Override
            public void onResponse(Call<List<RouteDto>> call, Response<List<RouteDto>> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    List<CruiseRoute> data = new ArrayList<>();
                    for (RouteDto dto : resp.body()) {
                        data.add(DtoMappers.toModel(dto));
                    }
                    adapter.setItems(data);
                } else {
                    UiUtils.toast(RoutesListActivity.this, "Failed: " + resp.code());
                }
            }

            @Override
            public void onFailure(Call<List<RouteDto>> call, Throwable t) {
                UiUtils.toast(RoutesListActivity.this, "Error: " + t.getMessage());
            }
        });
    }
}