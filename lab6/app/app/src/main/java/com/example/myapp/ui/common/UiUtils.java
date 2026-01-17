package com.example.myapp.ui.common;

import android.content.Context;
import android.widget.Toast;

public class UiUtils {
    public static void toast(Context ctx, String msg) {
        Toast.makeText(ctx, msg, Toast.LENGTH_SHORT).show();
    }
}