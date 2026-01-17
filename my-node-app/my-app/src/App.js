import React, { useState } from "react";
import TaskA from "./components/TaskA";
import TaskB from "./components/TaskB";
import TaskC from "./components/TaskC";
import TaskD from "./components/TaskD";
import TaskE from "./components/TaskE";

export default function App() {
  const [tab, setTab] = useState("A");

  return (
    <div style={{ minHeight: '100vh', backgroundColor: '#f7f7f7', padding: '20px', fontFamily: 'Consolas' }}>
      <div style={{ maxWidth: '800px', margin: '0 auto', backgroundColor: '#fff', borderRadius: '10px', boxShadow: '0 0 10px rgba(0,0,0,0.1)' }}>
        <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '15px', borderBottom: '1px solid #ddd' }}>
          <h1 style={{ fontSize: '20px', fontWeight: 'bold' }}>Вкладки</h1>
          <nav>
            {['A','B','C','D','E'].map((t) => (
              <button
                key={t}
                onClick={() => setTab(t)}
                style={{
                  margin: '0 5px',
                  padding: '5px 10px',
                  borderRadius: '5px',
                  border: '1px solid #ccc',
                  backgroundColor: tab === t ? '#4f46e5' : '#f3f3f3',
                  color: tab === t ? '#fff' : '#333',
                  cursor: 'pointer'
                }}
              >
                {t}
              </button>
            ))}
          </nav>
        </header>

        <main style={{ padding: '20px' }}>
          {tab === 'A' && <TaskA />}
          {tab === 'B' && <TaskB />}
          {tab === 'C' && <TaskC />}
          {tab === 'D' && <TaskD />}
          {tab === 'E' && <TaskE />}
        </main>

       
      </div>
    </div>
  );
}
