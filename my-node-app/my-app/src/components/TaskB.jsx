import React, { useState } from "react";

export default function TaskB() {
  const [count, setCount] = useState(0);

  return (
    <section>
      <h2>B — Счётчик</h2>
      <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
        <button onClick={() => setCount((c) => Math.max(0, c - 1))} disabled={count === 0}>
          -
        </button>
        <span style={{ fontSize: "20px" }}>{count}</span>
        <button onClick={() => setCount((c) => c + 1)}>+</button>
        <button onClick={() => setCount(0)} style={{ marginLeft: "10px" }}>
          Сбросить
        </button>
      </div>
      <p>Кнопка уменьшения отключается, когда счётчик равен нулю.</p>
    </section>
  );
}

