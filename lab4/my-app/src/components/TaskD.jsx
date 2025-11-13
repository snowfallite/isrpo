import React, { useState } from "react";

export default function TaskD() {
  const [color, setColor] = useState("white");

  const colors = ["red", "blue", "green", "yellow"];

  return (
    <section>
      <h2>D — Изменение цвета фона</h2>
      <div style={{ marginBottom: "10px" }}>
        {colors.map((c) => (
          <button
            key={c}
            onClick={() => setColor(c)}
            style={{ marginRight: "5px", padding: "5px 10px", borderRadius: "5px", border: "1px solid #ccc" }}
          >
            {c}
          </button>
        ))}
      </div>

      <div style={{ backgroundColor: color, padding: "20px", borderRadius: "5px" }}>
        Цвет: <strong>{color}</strong>
      </div>
    </section>
  );
}
