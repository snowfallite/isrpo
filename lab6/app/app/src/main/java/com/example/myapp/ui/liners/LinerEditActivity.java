package com.example.myapp.ui.liners;

import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import androidx.appcompat.app.AppCompatActivity;
import com.example.myapp.R;
import com.example.myapp.data.dto.LinerDto;
import com.example.myapp.data.ApiClient;
import com.example.myapp.data.CruiseApi;
import com.example.myapp.ui.common.UiUtils;
import retrofit2.*;

public class LinerEditActivity extends AppCompatActivity {
    private CruiseApi api;
    private Integer linerId;
    private EditText edtName, edtCapacity, edtClass, edtYear;

    @Override
    protected void onCreate(Bundle b) {
        super.onCreate(b);
        setContentView(R.layout.activity_liner_edit);
        api = ApiClient.get(getString(R.string.base_url)).create(CruiseApi.class);
        linerId = getIntent().hasExtra("linerId") ? getIntent().getIntExtra("linerId", -1) : null;

        edtName = findViewById(R.id.edtName);
        edtCapacity = findViewById(R.id.edtCapacity);
        edtClass = findViewById(R.id.edtClass);
        edtYear = findViewById(R.id.edtYear);

        if (linerId != null && linerId > 0) load();

        Button btnSave = findViewById(R.id.btnSave);
        btnSave.setOnClickListener(v -> save());
    }

    private void load() {
        api.getLiner(linerId).enqueue(new Callback<LinerDto>() {
            @Override public void onResponse(Call<LinerDto> call, Response<LinerDto> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    LinerDto d = resp.body();
                    edtName.setText(d.name);
                    edtCapacity.setText(String.valueOf(d.capacity));
                    edtClass.setText(d._class);
                    edtYear.setText(String.valueOf(d.yearBuilt));
                } else {
                    UiUtils.toast(LinerEditActivity.this, "Failed: " + resp.code());
                }
            }
            @Override public void onFailure(Call<LinerDto> call, Throwable t) {
                UiUtils.toast(LinerEditActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private void save() {
        LinerDto dto = new LinerDto();
        dto.name = edtName.getText().toString().trim();
        dto.capacity = parseInt(edtCapacity.getText().toString());
        dto._class = edtClass.getText().toString().trim();
        dto.yearBuilt = parseInt(edtYear.getText().toString());

        if (linerId == null || linerId <= 0) {
            api.createLiner(dto).enqueue(new Callback<LinerDto>() {
                @Override public void onResponse(Call<LinerDto> call, Response<LinerDto> resp) {
                    if (resp.isSuccessful()) finish(); else UiUtils.toast(LinerEditActivity.this, "Failed: " + resp.code());
                }
                @Override public void onFailure(Call<LinerDto> call, Throwable t) {
                    UiUtils.toast(LinerEditActivity.this, "Error: " + t.getMessage());
                }
            });
        } else {
            api.updateLiner(linerId, dto).enqueue(new Callback<Void>() {
                @Override public void onResponse(Call<Void> call, Response<Void> resp) {
                    if (resp.isSuccessful()) finish(); else UiUtils.toast(LinerEditActivity.this, "Failed: " + resp.code());
                }
                @Override public void onFailure(Call<Void> call, Throwable t) {
                    UiUtils.toast(LinerEditActivity.this, "Error: " + t.getMessage());
                }
            });
        }
    }

    private int parseInt(String s) {
        try { return Integer.parseInt(s); } catch (Exception e) { return 0; }
    }
}