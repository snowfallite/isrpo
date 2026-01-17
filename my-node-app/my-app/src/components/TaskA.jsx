
import React, { useState } from "react";

export default function TaskA() {
  const [name, setName] = useState("");

  return (
    <section>
      <h2>A — Hello {"{name}"}</h2>
      <p>Компонент принимает имя и показывает приветствие в реальном времени.</p>
      <div>
        <input
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Введите своё имя"
          style={{ width: "100%", padding: "8px", marginTop: "8px", border: "1px solid #ccc", borderRadius: "5px" }}
        />
        <div style={{ padding: "10px", marginTop: "10px", background: "#f1f1f1", borderRadius: "5px" }}>
          Hello, {name || "ноунейм"}
        </div>
      </div>
    </section>
  );
}
