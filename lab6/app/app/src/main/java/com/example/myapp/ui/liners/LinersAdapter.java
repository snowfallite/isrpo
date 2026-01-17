package com.example.myapp.ui.liners;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.example.myapp.R;
import com.example.myapp.model.Liner;
import java.util.List;

public class LinersAdapter extends RecyclerView.Adapter<LinersAdapter.VH> {
    public interface OnItemClick { void onClick(Liner liner); }
    private List<Liner> items;
    private OnItemClick click;

    public LinersAdapter(List<Liner> items, OnItemClick click) {
        this.items = items;
        this.click = click;
    }

    public void setItems(List<Liner> newItems) { this.items = newItems; notifyDataSetChanged(); }

    static class VH extends RecyclerView.ViewHolder {
        TextView name, subtitle;
        VH(View v) {
            super(v);
            name = v.findViewById(R.id.txtName);
            subtitle = v.findViewById(R.id.txtSubtitle);
        }
    }

    @NonNull @Override public VH onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_liner, parent, false);
        return new VH(v);
    }

    @Override public void onBindViewHolder(@NonNull VH h, int pos) {
        Liner l = items.get(pos);
        h.name.setText(l.name);
        h.subtitle.setText("Capacity: " + l.capacity + " • Class: " + l.clazz);
        h.itemView.setOnClickListener(v -> click.onClick(l));
    }

    @Override public int getItemCount() { return items.size(); }
}
