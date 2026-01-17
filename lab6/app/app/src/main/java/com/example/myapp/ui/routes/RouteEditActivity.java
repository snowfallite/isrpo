package com.example.myapp.ui.routes;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import androidx.appcompat.app.AppCompatActivity;
import com.example.myapp.R;
import com.example.myapp.data.ApiClient;
import com.example.myapp.data.CruiseApi;
import com.example.myapp.data.dto.RouteDto;
import com.example.myapp.data.mapper.DtoMappers;
import com.example.myapp.model.CruiseRoute;
import com.example.myapp.ui.common.UiUtils;
import retrofit2.*;

public class RouteEditActivity extends AppCompatActivity {
    private static final String EXTRA_ROUTE_ID = "routeId";
    private CruiseApi api;
    private Integer routeId;
    private EditText edtName, edtDuration;

    public static void start(Context ctx, int routeId) {
        Intent i = new Intent(ctx, RouteEditActivity.class);
        i.putExtra(EXTRA_ROUTE_ID, routeId);
        ctx.startActivity(i);
    }

    @Override
    protected void onCreate(Bundle b) {
        super.onCreate(b);
        setContentView(R.layout.activity_route_edit);
        api = ApiClient.get(getString(R.string.base_url)).create(CruiseApi.class);
        routeId = getIntent().hasExtra(EXTRA_ROUTE_ID) ? getIntent().getIntExtra(EXTRA_ROUTE_ID, -1) : null;

        edtName = findViewById(R.id.edtRouteName);
        edtDuration = findViewById(R.id.edtDuration);

        if (routeId != null && routeId > 0) load();

        Button btnSave = findViewById(R.id.btnSaveRoute);
        btnSave.setOnClickListener(v -> save());
    }

    private void load() {
        api.getRoute(routeId).enqueue(new Callback<RouteDto>() {
            @Override
            public void onResponse(Call<RouteDto> call, Response<RouteDto> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    CruiseRoute route = DtoMappers.toModel(resp.body());
                    edtName.setText(route.name);
                    edtDuration.setText(String.valueOf(route.durationDays));
                }
            }

            @Override
            public void onFailure(Call<RouteDto> call, Throwable t) {
                UiUtils.toast(RouteEditActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private void save() {
        CruiseRoute r = new CruiseRoute();
        r.name = edtName.getText().toString().trim();
        r.durationDays = parseInt(edtDuration.getText().toString());

        RouteDto dto = DtoMappers.toDto(r);

        if (routeId == null || routeId <= 0) {
            api.createRoute(dto).enqueue(new Callback<RouteDto>() {
                @Override public void onResponse(Call<RouteDto> call, Response<RouteDto> resp) {
                    if (resp.isSuccessful()) finish();
                    else UiUtils.toast(RouteEditActivity.this, "Failed: " + resp.code());
                }
                @Override public void onFailure(Call<RouteDto> call, Throwable t) {
                    UiUtils.toast(RouteEditActivity.this, "Error: " + t.getMessage());
                }
            });
        } else {
            api.updateRoute(routeId, dto).enqueue(new Callback<Void>() {
                @Override public void onResponse(Call<Void> call, Response<Void> resp) {
                    if (resp.isSuccessful()) finish();
                    else UiUtils.toast(RouteEditActivity.this, "Failed: " + resp.code());
                }
                @Override public void onFailure(Call<Void> call, Throwable t) {
                    UiUtils.toast(RouteEditActivity.this, "Error: " + t.getMessage());
                }
            });
        }
    }

    private int parseInt(String s) {
        try { return Integer.parseInt(s); } catch (Exception e) { return 0; }
    }
}
