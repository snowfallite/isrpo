import React, { useState } from "react";

export default function TaskE() {
  const [open, setOpen] = useState(false);

  return (
    <section>
      <h2>E — Модальное окно</h2>
      <button onClick={() => setOpen(true)}>Открыть модальное окно</button>

      {open && (
        <div
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            width: "100%",
            height: "100%",
            backgroundColor: "rgba(0,0,0,0.5)",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          <div style={{ backgroundColor: "#fff", padding: "20px", borderRadius: "10px", position: "relative", width: "300px" }}>
            <button
              onClick={() => setOpen(false)}
              style={{
                position: "absolute",
                top: "10px",
                right: "10px",
                border: "none",
                background: "none",
                cursor: "pointer",
                fontSize: "16px",
              }}
            >
              ✖
            </button>
            <p style={{ textAlign: "center" }}>
              “The roots of education are bitter, but the fruit is sweet.”
            </p>
            <button onClick={() => setOpen(false)} style={{ display: "block", margin: "15px auto 0" }}>
              Закрыть
            </button>
          </div>
        </div>
      )}
    </section>
  );
}
