package com.example.myapp.ui.routes;


import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.example.myapp.R;
import com.example.myapp.model.CruiseRoute;
import java.util.List;

public class RoutesAdapter extends RecyclerView.Adapter<RoutesAdapter.VH> {
    public interface OnItemClick { void onClick(CruiseRoute route); }
    private List<CruiseRoute> items;
    private final OnItemClick click;

    public RoutesAdapter(List<CruiseRoute> items, OnItemClick click) {
        this.items = items;
        this.click = click;
    }

    public void setItems(List<CruiseRoute> newItems) {
        this.items = newItems;
        notifyDataSetChanged();
    }

    static class VH extends RecyclerView.ViewHolder {
        TextView name, subtitle;
        VH(View v) {
            super(v);
            name = v.findViewById(R.id.txtName);
            subtitle = v.findViewById(R.id.txtSubtitle);
        }
    }

    @NonNull @Override public VH onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_route, parent, false);
        return new VH(v);
    }

    @Override public void onBindViewHolder(@NonNull VH h, int pos) {
        CruiseRoute r = items.get(pos);
        h.name.setText(r.name);
        h.subtitle.setText("Duration: " + r.durationDays + " days");
        h.itemView.setOnClickListener(v -> click.onClick(r));
    }

    @Override public int getItemCount() { return items == null ? 0 : items.size(); }
}