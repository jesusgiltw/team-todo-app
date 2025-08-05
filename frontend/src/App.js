import React, { useEffect, useState } from 'react';
import './App.css';

function App() {
  const [tasks, setTasks] = useState([]);

  useEffect(() => {
    fetch('http://localhost:5000/api/tasks')
      .then(res => res.json())
      .then(data => setTasks(data));
  }, []);

  return (
    <div className="container">
      <h1>Lista de tareas</h1>
      <ul>
        {tasks.map(task => (
          <li key={task.id} className={task.isCompleted ? 'completed' : ''}>
            <div className="info">
              <strong>{task.title}</strong><br />
              <small>Vence: {new Date(task.dueDate).toLocaleString()}</small>
            </div>
            <div className="status">{task.isCompleted ? '✅' : '⏳'}</div>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
