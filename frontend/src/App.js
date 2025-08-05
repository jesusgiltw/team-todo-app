import React, { useEffect, useState } from 'react';
import './App.css';

function App() {
  const [tasks, setTasks] = useState([]);
  const [newTitle, setNewTitle] = useState('');
  const [newDueDate, setNewDueDate] = useState('');

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
      fetchTasks();
    } catch (err) {
      console.error('Error al completar la tarea:', err);
    }
  };

  const handleAddTask = async (e) => {
    e.preventDefault();

    if (!newTitle || !newDueDate) {
      alert('Por favor, completa ambos campos');
      return;
    }

    try {
      await fetch('http://localhost:5000/api/tasks', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          title: newTitle,
          dueDate: new Date(newDueDate).toISOString()
        })
      });

      setNewTitle('');
      setNewDueDate('');
      fetchTasks();
    } catch (err) {
      console.error('Error al añadir la tarea:', err);
    }
  };

  // Ordenar tareas: pendientes primero, luego completadas, ambas por fecha
  const sortedTasks = [...tasks].sort((a, b) => {
    if (a.isCompleted !== b.isCompleted) {
      return a.isCompleted ? 1 : -1;
    }
    return new Date(a.dueDate) - new Date(b.dueDate);
  });

  return (
    <div className="container">
      <h1>Lista de tareas</h1>

      <ul>
        {sortedTasks.map(task => {
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

      <hr style={{ margin: '40px 0' }} />

      <h2>Añadir nueva tarea</h2>
      <form onSubmit={handleAddTask} className="task-form">
        <input
          type="text"
          placeholder="Título"
          value={newTitle}
          onChange={(e) => setNewTitle(e.target.value)}
        />
        <input
          type="datetime-local"
          value={newDueDate}
          onChange={(e) => setNewDueDate(e.target.value)}
        />
        <button type="submit">Añadir tarea</button>
      </form>
    </div>
  );
}

export default App;
