package com.example.myapp.ui.linersRoutes;

import android.os.Bundle;
import android.widget.*;
import androidx.appcompat.app.AppCompatActivity;
import com.example.myapp.R;
import com.example.myapp.data.ApiClient;
import com.example.myapp.data.CruiseApi;
import com.example.myapp.data.dto.LinerDto;
import com.example.myapp.data.dto.RouteDto;
import com.example.myapp.data.dto.RouteLinerDto;
import com.example.myapp.ui.common.UiUtils;
import java.util.List;
import retrofit2.*;

public class LinersRoutesActivity extends AppCompatActivity {
    private CruiseApi api;
    private EditText edtRouteId, edtLinerId, edtSeason, edtBasePrice;
    private Button btnCreate, btnUpdate, btnDelete;
    private Button btnShowLiners, btnShowRoutes, btnShowRouteLiners;
    private TextView txtOutput;

    @Override
    protected void onCreate(Bundle b) {
        super.onCreate(b);
        setContentView(R.layout.activity_liners_routes);

        api = ApiClient.get(getString(R.string.base_url)).create(CruiseApi.class);

        // поля ввода
        edtRouteId = findViewById(R.id.edtRouteId);
        edtLinerId = findViewById(R.id.edtLinerId);
        edtSeason = findViewById(R.id.edtSeason);
        edtBasePrice = findViewById(R.id.edtBasePrice);

        // кнопки CRUD
        btnCreate = findViewById(R.id.btnCreateLink);
        btnUpdate = findViewById(R.id.btnUpdateLink);
        btnDelete = findViewById(R.id.btnDeleteLink);

        // кнопки вывода
        btnShowLiners = findViewById(R.id.btnShowLiners);
        btnShowRoutes = findViewById(R.id.btnShowRoutes);
        btnShowRouteLiners = findViewById(R.id.btnShowRouteLiners);

        txtOutput = findViewById(R.id.txtOutput);

        btnCreate.setOnClickListener(v -> createLink());
        btnUpdate.setOnClickListener(v -> updateLink());
        btnDelete.setOnClickListener(v -> deleteLink());

        btnShowLiners.setOnClickListener(v -> loadLiners());
        btnShowRoutes.setOnClickListener(v -> loadRoutes());
        btnShowRouteLiners.setOnClickListener(v -> loadRouteLiners());

        loadRouteLiners(); // загрузка связей при старте
    }

    // --- CRUD ---

    private RouteLinerDto collectDto() {
        int routeId = parseInt(edtRouteId.getText().toString());
        int linerId = parseInt(edtLinerId.getText().toString());
        String season = edtSeason.getText().toString().trim();
        double price = parseDouble(edtBasePrice.getText().toString());

        if (routeId <= 0 || linerId <= 0 || season.isEmpty() || price <= 0) {
            UiUtils.toast(this, "Fill all fields correctly");
            return null;
        }
        return new RouteLinerDto(routeId, linerId, season, price);
    }

    private void createLink() {
        RouteLinerDto dto = collectDto();
        if (dto == null) return;

        api.createRouteLiner(dto).enqueue(new Callback<RouteLinerDto>() {
            @Override
            public void onResponse(Call<RouteLinerDto> call, Response<RouteLinerDto> resp) {
                if (resp.isSuccessful()) {
                    UiUtils.toast(LinersRoutesActivity.this, "Link created");
                    loadRouteLiners();
                } else {
                    UiUtils.toast(LinersRoutesActivity.this, "Failed: " + resp.code());
                }
            }
            @Override
            public void onFailure(Call<RouteLinerDto> call, Throwable t) {
                UiUtils.toast(LinersRoutesActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private void updateLink() {
        RouteLinerDto dto = collectDto();
        if (dto == null) return;

        api.updateRouteLiner(dto.routeId, dto.linerId, dto).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> resp) {
                if (resp.isSuccessful()) {
                    UiUtils.toast(LinersRoutesActivity.this, "Link updated");
                    loadRouteLiners();
                } else {
                    UiUtils.toast(LinersRoutesActivity.this, "Failed: " + resp.code());
                }
            }
            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                UiUtils.toast(LinersRoutesActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private void deleteLink() {
        int routeId = parseInt(edtRouteId.getText().toString());
        int linerId = parseInt(edtLinerId.getText().toString());

        if (routeId <= 0 || linerId <= 0) {
            UiUtils.toast(this, "Enter valid IDs");
            return;
        }

        api.deleteRouteLiner(routeId, linerId).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> resp) {
                if (resp.isSuccessful()) {
                    UiUtils.toast(LinersRoutesActivity.this, "Link deleted");
                    loadRouteLiners();
                } else {
                    UiUtils.toast(LinersRoutesActivity.this, "Failed: " + resp.code());
                }
            }
            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                UiUtils.toast(LinersRoutesActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    // --- Вывод списков ---

    private void loadLiners() {
        api.getLiners().enqueue(new Callback<List<LinerDto>>() {
            @Override
            public void onResponse(Call<List<LinerDto>> call, Response<List<LinerDto>> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    StringBuilder sb = new StringBuilder("=== Liners ===\n");
                    for (LinerDto l : resp.body()) {
                        sb.append("Liner ").append(l.id)
                                .append(": ").append(l.name)
                                .append(" | Capacity ").append(l.capacity)
                                .append(" | Class ").append(l._class)
                                .append(" | Year ").append(l.yearBuilt)
                                .append("\n");
                    }
                    txtOutput.setText(sb.toString());
                }
            }
            @Override
            public void onFailure(Call<List<LinerDto>> call, Throwable t) {
                UiUtils.toast(LinersRoutesActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private void loadRoutes() {
        api.getRoutes().enqueue(new Callback<List<RouteDto>>() {
            @Override
            public void onResponse(Call<List<RouteDto>> call, Response<List<RouteDto>> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    StringBuilder sb = new StringBuilder("=== Routes ===\n");
                    for (RouteDto r : resp.body()) {
                        sb.append("Route ").append(r.id)
                                .append(": ").append(r.name)
                                .append(" | Duration ").append(r.durationDays).append(" days\n");
                    }
                    txtOutput.setText(sb.toString());
                }
            }
            @Override
            public void onFailure(Call<List<RouteDto>> call, Throwable t) {
                UiUtils.toast(LinersRoutesActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private void loadRouteLiners() {
        api.getRouteLiners().enqueue(new Callback<List<RouteLinerDto>>() {
            @Override
            public void onResponse(Call<List<RouteLinerDto>> call, Response<List<RouteLinerDto>> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    StringBuilder sb = new StringBuilder("=== Route ↔ Liner Links ===\n");
                    for (RouteLinerDto rl : resp.body()) {
                        sb.append("Route ").append(rl.routeId)
                                .append(" ↔ Liner ").append(rl.linerId)
                                .append(" | ").append(rl.season)
                                .append(" | ₽").append(rl.basePrice)
                                .append("\n");
                    }
                    txtOutput.setText(sb.toString());
                }
            }
            @Override
            public void onFailure(Call<List<RouteLinerDto>> call, Throwable t) {
                UiUtils.toast(LinersRoutesActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    // --- утилиты ---
    private int parseInt(String s) {
        try { return Integer.parseInt(s.trim()); } catch (Exception e) { return 0; }
    }
    private double parseDouble(String s) {
        try { return Double.parseDouble(s.trim()); } catch (Exception e) { return 0.0; }
    }
}
