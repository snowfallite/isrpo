package com.example.myapp.ui.linersRoutes;

import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import com.example.myapp.R;
import com.example.myapp.data.ApiClient;
import com.example.myapp.data.CruiseApi;
import com.example.myapp.ui.common.UiUtils;
import retrofit2.*;

public class RouteLinerLinkActivity extends AppCompatActivity {
    private CruiseApi api;
    private EditText edtRouteId, edtLinerId;
    private Button btnLink;
    private TextView txtResult;

    @Override
    protected void onCreate(Bundle b) {
        super.onCreate(b);
        setContentView(R.layout.activity_route_liner_link);

        api = ApiClient.get(getString(R.string.base_url)).create(CruiseApi.class);

        edtRouteId = findViewById(R.id.edtRouteId);
        edtLinerId = findViewById(R.id.edtLinerId);
        btnLink = findViewById(R.id.btnLink);
        txtResult = findViewById(R.id.txtResult);

        btnLink.setOnClickListener(v -> link());
    }

    private void link() {
        int routeId = parseInt(edtRouteId.getText().toString());
        int linerId = parseInt(edtLinerId.getText().toString());

        if (routeId <= 0 || linerId <= 0) {
            UiUtils.toast(this, "Enter valid IDs");
            return;
        }

        api.addLinerToRoute(routeId, linerId).enqueue(new Callback<Void>() {
            @Override public void onResponse(Call<Void> call, Response<Void> resp) {
                if (resp.isSuccessful()) {
                    txtResult.setText("Linked: Route " + routeId + " ↔ Liner " + linerId);
                } else {
                    UiUtils.toast(RouteLinerLinkActivity.this, "Failed: " + resp.code());
                }
            }
            @Override public void onFailure(Call<Void> call, Throwable t) {
                UiUtils.toast(RouteLinerLinkActivity.this, "Error: " + t.getMessage());
            }
        });
    }

    private int parseInt(String s) {
        try { return Integer.parseInt(s); } catch (Exception e) { return 0; }
    }
}
