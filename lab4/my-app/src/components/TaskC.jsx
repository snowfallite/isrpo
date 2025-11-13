import React, { useState } from "react";

export default function TaskC() {
  const [text, setText] = useState("");
  const [todos, setTodos] = useState([]);

  const addTodo = () => {
    const trimmed = text.trim();
    if (!trimmed) return;
    setTodos((prev) => [...prev, { id: Date.now(), text: trimmed }]);
    setText("");
  };

  const removeTodo = (id) => {
    setTodos((prev) => prev.filter((t) => t.id !== id));
  };

  return (
    <section>
      <h2>C — Список задач (Todo)</h2>
      <div style={{ display: "flex", gap: "10px", marginBottom: "10px" }}>
        <input
          value={text}
          onChange={(e) => setText(e.target.value)}
          placeholder="Новая задача"
          style={{ flex: 1, padding: "8px", border: "1px solid #ccc", borderRadius: "5px" }}
        />
        <button onClick={addTodo}>Добавить</button>
      </div>

      <ul style={{ listStyle: "none", padding: 0 }}>
        {todos.length === 0 && <li>Список пуст — добавьте задачу.</li>}
        {todos.map((t) => (
          <li
            key={t.id}
            onClick={() => removeTodo(t.id)}
            style={{
              padding: "8px",
              border: "1px solid #ddd",
              marginTop: "5px",
              borderRadius: "5px",
              cursor: "pointer",
            }}
            title="Кликните, чтобы удалить"
          >
            {t.text}
          </li>
        ))}
      </ul>
    </section>
  );
}
