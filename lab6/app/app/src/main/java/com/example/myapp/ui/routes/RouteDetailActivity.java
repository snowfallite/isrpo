package com.example.myapp.ui.routes;

import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import com.example.myapp.R;
import com.example.myapp.data.ApiClient;
import com.example.myapp.data.CruiseApi;
import com.example.myapp.data.dto.RouteDto;
import com.example.myapp.data.mapper.DtoMappers;
import com.example.myapp.model.CruiseRoute;
import com.example.myapp.ui.common.ConfirmDialog;
import com.example.myapp.ui.common.UiUtils;
import retrofit2.*;

public class RouteDetailActivity extends AppCompatActivity {
    private CruiseApi api;
    private int routeId;
    private CruiseRoute route;

    private TextView txtRouteName, txtRouteDetails;
    @Override
    protected void onResume() {
        super.onResume();
        load();
    }
    @Override
    protected void onCreate(Bundle b) {
        super.onCreate(b);
        setContentView(R.layout.activity_route_detail);
        routeId = getIntent().getIntExtra("routeId", -1);
        api = ApiClient.get(getString(R.string.base_url)).create(CruiseApi.class);

        txtRouteName = findViewById(R.id.txtRouteName);
        txtRouteDetails = findViewById(R.id.txtRouteDetails);

        Button btnEdit = findViewById(R.id.btnEditRoute);
        Button btnDelete = findViewById(R.id.btnDeleteRoute);

        btnEdit.setOnClickListener(v -> RouteEditActivity.start(this, routeId));
        btnDelete.setOnClickListener(v -> ConfirmDialog.show(this, "Delete route?", this::delete));

        load();
    }

    private void load() {
        api.getRoute(routeId).enqueue(new Callback<RouteDto>() {
            @Override
            public void onResponse(Call<RouteDto> call, Response<RouteDto> resp) {
                if (resp.isSuccessful() && resp.body() != null) {
                    route = DtoMappers.toModel(resp.body());
                    txtRouteName.setText(route.name);
                    txtRouteDetails.setText("Duration: " + route.durationDays + " days");
                }
            }

            @Override
            public void onFailure(Call<RouteDto> call, Throwable t) {
                UiUtils.toast(RouteDetailActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private void delete() {
        api.deleteRoute(routeId).enqueue(new Callback<Void>() {
            @Override public void onResponse(Call<Void> call, Response<Void> resp) {
                if (resp.isSuccessful()) finish();
                else UiUtils.toast(RouteDetailActivity.this, "Failed: " + resp.code());
            }
            @Override public void onFailure(Call<Void> call, Throwable t) {
                UiUtils.toast(RouteDetailActivity.this, "Error: " + t.getMessage());
            }
        });
    }
}
