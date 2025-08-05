import React, { useEffect, useState } from 'react';
import './App.css';

function App() {
  const [tasks, setTasks] = useState([]);

  const fetchTasks = () => {
    fetch('http://localhost:5000/api/tasks')
      .then(res => res.json())
      .then(data => setTasks(data));
  };

  useEffect(() => {
    fetchTasks();
  }, []);

  const isUrgent = (dueDateStr) => {
    const dueDate = new Date(dueDateStr);
    const now = new Date();
    const diffHours = (dueDate - now) / (1000 * 60 * 60);
    return diffHours > 0 && diffHours < 3;
  };

  const handleComplete = async (taskId) => {
    try {
      await fetch(`http://localhost:5000/api/tasks/${taskId}/complete`, {
        method: 'POST',
      });
      fetchTasks(); // Recargar la lista tras completar
    } catch (err) {
      console.error('Error al completar la tarea:', err);
    }
  };

  return (
    <div className="container">
      <h1>Lista de tareas</h1>
      <ul>
        {tasks.map(task => {
          const baseClass = task.isCompleted ? 'completed' : '';
          const urgentClass = !task.isCompleted && isUrgent(task.dueDate) ? 'urgent' : '';
          const className = `${baseClass} ${urgentClass}`.trim();

          return (
            <li key={task.id} className={className}>
              <div className="info">
                <strong>{task.title}</strong><br />
                <small>Vence: {new Date(task.dueDate).toLocaleString()}</small>
              </div>
              <div className="status">
                {task.isCompleted ? '✅' : (
                  <button onClick={() => handleComplete(task.id)}>Completar</button>
                )}
              </div>
            </li>
          );
        })}
      </ul>
    </div>
  );
}

export default App;
