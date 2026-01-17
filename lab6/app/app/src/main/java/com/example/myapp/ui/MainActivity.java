package com.example.myapp.ui;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import androidx.appcompat.app.AppCompatActivity;
import com.example.myapp.R;
import com.example.myapp.ui.liners.LinersListActivity;
import com.example.myapp.ui.linersRoutes.LinersRoutesActivity;
import com.example.myapp.ui.routes.RoutesListActivity;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Button btnLinersRoutes = findViewById(R.id.btnLinersRoutes) ;
        Button btnLiners = findViewById(R.id.btnLiners);
        Button btnRoutes = findViewById(R.id.btnRoutes);

        btnLiners.setOnClickListener(v -> {
            Intent i = new Intent(MainActivity.this, LinersListActivity.class);
            startActivity(i);
        });

        btnRoutes.setOnClickListener(v -> {
            Intent i = new Intent(MainActivity.this, RoutesListActivity.class);
            startActivity(i);
        });

        btnLinersRoutes.setOnClickListener(v -> {
            Intent i = new Intent(MainActivity.this, LinersRoutesActivity.class);
            startActivity(i);
        });
    }
}
