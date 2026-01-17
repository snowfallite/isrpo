package com.example.myapp.ui.common;

import android.content.Context;
import androidx.appcompat.app.AlertDialog;

public class ConfirmDialog {
    public interface Callback { void onConfirm(); }

    public static void show(Context ctx, String message, Callback cb) {
        new AlertDialog.Builder(ctx)
                .setMessage(message)
                .setPositiveButton("OK", (d, w) -> cb.onConfirm())
                .setNegativeButton("Cancel", null)
                .show();
    }
}