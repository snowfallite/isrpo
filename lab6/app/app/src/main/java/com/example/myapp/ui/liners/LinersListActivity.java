package com.example.myapp.ui.liners;

import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.example.myapp.R;
import com.example.myapp.data.ApiClient;
import com.example.myapp.data.CruiseApi;
import com.example.myapp.data.dto.LinerDto;
import com.example.myapp.data.mapper.DtoMappers;
import com.example.myapp.model.Liner;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import java.util.ArrayList;
import java.util.List;
import retrofit2.*;

public class LinersListActivity extends AppCompatActivity {
    private LinersAdapter adapter;
    private CruiseApi api;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_liners_list);

        RecyclerView rv = findViewById(R.id.recyclerLiners);
        rv.setLayoutManager(new LinearLayoutManager(this));
        adapter = new LinersAdapter(new ArrayList<>(), liner -> {
            Intent i = new Intent(this, LinerDetailActivity.class);
            i.putExtra("linerId", liner.id);
            startActivity(i);
        });
        rv.setAdapter(adapter);

        FloatingActionButton fab = findViewById(R.id.fabAddLiner);
        fab.setOnClickListener(v -> {
            startActivity(new Intent(this, LinerEditActivity.class));
        });

        api = ApiClient.get(getString(R.string.base_url)).create(CruiseApi.class);
    }

    @Override
    protected void onResume() {
        super.onResume();
        loadData();
    }

    private void loadData() {
        api.getLiners().enqueue(new Callback<List<LinerDto>>() {
            @Override
            public void onResponse(Call<List<LinerDto>> call, Response<List<LinerDto>> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    List<Liner> data = new ArrayList<>();
                    for (LinerDto dto : resp.body()) data.add(DtoMappers.toModel(dto));
                    adapter.setItems(data);
                }
            }
            @Override
            public void onFailure(Call<List<LinerDto>> call, Throwable t) { /* show toast */ }
        });
    }
}