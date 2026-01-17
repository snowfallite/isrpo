package com.example.myapp.ui.liners;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import com.example.myapp.R;
import com.example.myapp.data.dto.LinerDto;
import com.example.myapp.data.ApiClient;
import com.example.myapp.data.CruiseApi;
import com.example.myapp.data.mapper.DtoMappers;
import com.example.myapp.model.Liner;
import com.example.myapp.ui.common.ConfirmDialog;
import com.example.myapp.ui.common.UiUtils;
import retrofit2.*;

public class LinerDetailActivity extends AppCompatActivity {
    private CruiseApi api;
    private int linerId;
    private Liner liner;

    @Override
    protected void onResume() {
        super.onResume();
        load();
    }
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_liner_detail);
        linerId = getIntent().getIntExtra("linerId", -1);
        api = ApiClient.get(getString(R.string.base_url)).create(CruiseApi.class);

        Button btnEdit = findViewById(R.id.btnEdit);
        Button btnDelete = findViewById(R.id.btnDelete);
        btnEdit.setOnClickListener(v -> {
            Intent i = new Intent(this, LinerEditActivity.class);
            i.putExtra("linerId", linerId);
            startActivity(i);
        });
        btnDelete.setOnClickListener(v -> ConfirmDialog.show(this, "Delete liner?", this::delete));

        load();
    }

    private void load() {
        api.getLiner(linerId).enqueue(new Callback<LinerDto>() {
            @Override public void onResponse(Call<LinerDto> call, Response<LinerDto> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    liner = DtoMappers.toModel(resp.body());
                    ((TextView)findViewById(R.id.txtName)).setText(liner.name);
                    ((TextView)findViewById(R.id.txtDetails)).setText(
                            "Capacity: " + liner.capacity + "\nClass: " + liner.clazz + "\nYear: " + liner.yearBuilt);
                } else {
                    UiUtils.toast(LinerDetailActivity.this, "Failed: " + resp.code());
                }
            }
            @Override public void onFailure(Call<LinerDto> call, Throwable t) {
                UiUtils.toast(LinerDetailActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private void delete() {
        api.deleteLiner(linerId).enqueue(new Callback<Void>() {
            @Override public void onResponse(Call<Void> call, Response<Void> resp) {
                if (resp.isSuccessful()) finish();
                else UiUtils.toast(LinerDetailActivity.this, "Failed: " + resp.code());
            }
            @Override public void onFailure(Call<Void> call, Throwable t) {
                UiUtils.toast(LinerDetailActivity.this, "Error: " + t.getMessage());
            }
        });
    }
}