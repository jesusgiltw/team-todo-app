import React, { useEffect, useState } from 'react';

function App() {
  const [tasks, setTasks] = useState([]);

  useEffect(() => {
    fetch('http://localhost:5000/api/tasks')
      .then(res => res.json())
      .then(data => setTasks(data));
  }, []);

  return (
    <div>
      <h1>Lista de tareas</h1>
      <ul>
        {tasks.map(task => (
          <li key={task.id}>
            <strong>{task.title}</strong> - vence: {new Date(task.dueDate).toLocaleString()} - {task.isCompleted ? '✅' : '⏳'}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;