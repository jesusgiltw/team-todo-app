import React, { useEffect, useState, useCallback } from 'react';
import './App.css';
import "@fontsource/bitter"; // peso normal
import "@fontsource/bitter/700.css"; // negrita


function Notification({ message, onClose }) {
  React.useEffect(() => {
    if (!message) return;
    const timer = setTimeout(() => {
      onClose();
    }, 3500);
    return () => clearTimeout(timer);
  }, [message, onClose]);

  if (!message) return null;

  return (
    <div className="notification warning">
      {message}
      <button className="close-btn" onClick={onClose} aria-label="Cerrar notificación">&times;</button>
    </div>
  );
}

function App() {
  const [tasks, setTasks] = useState([]);
  const [newTitle, setNewTitle] = useState('');
  const [newDueDate, setNewDueDate] = useState('');
  const [notification, setNotification] = useState('');

  const fetchTasks = useCallback(() => {
    fetch('http://localhost:5000/api/tasks')
      .then(res => res.json())
      .then(data => setTasks(data));
  }, []);

  useEffect(() => {
    fetchTasks();
  }, [fetchTasks]);

  const now = new Date();

  const isUrgent = (dueDateStr) => {
    const dueDate = new Date(dueDateStr);
    const diffHours = (dueDate - now) / (1000 * 60 * 60);
    return diffHours > 0 && diffHours < 3;
  };

  const isExpired = (dueDateStr) => {
    const dueDate = new Date(dueDateStr);
    return dueDate < now;
  };

  const showNotification = (msg) => {
    setNotification(msg);
  };

  const handleComplete = async (taskId) => {
    try {
      const res = await fetch(`http://localhost:5000/api/tasks/${taskId}/complete`, {
        method: 'POST',
      });
      if (!res.ok) {
        const errText = await res.text();
        showNotification(`Error al completar la tarea: ${errText}`);
        return;
      }
      fetchTasks();
    } catch (err) {
      console.error('Error al completar la tarea:', err);
      showNotification('Error al completar la tarea, intenta de nuevo.');
    }
  };

  const handleDelete = async (taskId) => {
    if (!window.confirm('¿Estás seguro de que quieres eliminar esta tarea?')) return;

    try {
      const res = await fetch(`http://localhost:5000/api/tasks/${taskId}/delete`, {
        method: 'DELETE',
      });
      if (!res.ok) {
        const errText = await res.text();
        showNotification(`Error al eliminar la tarea: ${errText}`);
        return;
      }
      fetchTasks();
    } catch (err) {
      console.error('Error al eliminar la tarea:', err);
      showNotification('Error al eliminar la tarea, intenta de nuevo.');
    }
  };

  const handleAddTask = async (e) => {
    e.preventDefault();

    if (!newTitle || !newDueDate) {
      showNotification('Por favor, completa ambos campos');
      return;
    }

    try {
      const res = await fetch('http://localhost:5000/api/tasks', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          title: newTitle,
          dueDate: new Date(newDueDate).toISOString()
        })
      });

      if (!res.ok) {
        const errorText = await res.text();
        showNotification(`Error al añadir la tarea: ${errorText}`);
        return;
      }

      setNewTitle('');
      setNewDueDate('');
      fetchTasks();

    } catch (err) {
      console.error('Error al añadir la tarea:', err);
      showNotification('Error al añadir la tarea, intenta de nuevo.');
    }
  };

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
          const expired = isExpired(task.dueDate);
          const urgentClass = !task.isCompleted && isUrgent(task.dueDate) ? 'urgent' : '';
          const expiredClass = (!task.isCompleted && expired) ? 'expired' : '';
          const completedClass = task.isCompleted ? 'completed' : '';
          const className = [completedClass, urgentClass, expiredClass].filter(Boolean).join(' ');

          return (
            <li key={task.id} className={className}>
              <div className="info">
                <strong>{task.title}</strong><br />
                <small>
                  {expired ? (
                    <>Vencida: {new Date(task.dueDate).toLocaleString()} ⚠️</>
                  ) : (
                    <>Vence: {new Date(task.dueDate).toLocaleString()}</>
                  )}
                </small>

              </div>
              <div className="status">
                {!task.isCompleted && expired && (
                  <span title="Tarea vencida sin completar">⚠️</span>
                )}

                {!task.isCompleted && !expired && (
                  <button className="complete-btn" onClick={() => handleComplete(task.id)}>Completar</button>
                )}

                <span
                  className="delete-icon"
                  onClick={() => handleDelete(task.id)}
                  title="Eliminar tarea"
                >
                  🗑️
                </span>
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

      <Notification message={notification} onClose={() => setNotification('')} />
    </div>
  );
}

export default App;
