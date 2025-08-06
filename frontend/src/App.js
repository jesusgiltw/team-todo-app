import React, { useEffect, useState } from 'react';
import './App.css';

function App() {
  const [tasks, setTasks] = useState([]);
  const [title, setTitle] = useState('');
  const [dueDate, setDueDate] = useState('');
  const [notification, setNotification] = useState(null);
  const [selectedTask, setSelectedTask] = useState(null);

  useEffect(() => {
    fetchTasks();
  }, []);

  const fetchTasks = () => {
    fetch('http://localhost:5000/api/tasks')
      .then(res => res.json())
      .then(data => {
        const sorted = [...data].sort((a, b) => {
          if (a.isCompleted && !b.isCompleted) return 1;
          if (!a.isCompleted && b.isCompleted) return -1;
          return new Date(a.dueDate) - new Date(b.dueDate);
        });
        setTasks(sorted);
      });
  };

  const handleComplete = async (id) => {
    await fetch(`http://localhost:5000/api/tasks/${id}/complete`, {
      method: 'POST',
    });
    fetchTasks();
  };

  const handleDelete = async (id) => {
    await fetch(`http://localhost:5000/api/tasks/${id}/delete`, {
      method: 'DELETE',
    });
    fetchTasks();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const due = new Date(dueDate);
    if (due <= new Date()) {
      showNotification('La fecha debe estar en el futuro.');
      return;
    }

    const res = await fetch('http://localhost:5000/api/tasks', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ title, dueDate }),
    });

    if (!res.ok) {
      showNotification('Error al crear la tarea.');
      return;
    }

    setTitle('');
    setDueDate('');
    fetchTasks();
  };

  const showNotification = (msg) => {
    setNotification(msg);
    setTimeout(() => setNotification(null), 4000);
  };

  const handleEditSubmit = async (e) => {
    e.preventDefault();
    const updatedDue = new Date(selectedTask.dueDate);
    if (updatedDue <= new Date()) {
      showNotification('La fecha debe estar en el futuro.');
      return;
    }

    await fetch(`http://localhost:5000/api/tasks/${selectedTask.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        title: selectedTask.title,
        dueDate: selectedTask.dueDate,
      }),
    });

    setSelectedTask(null);
    fetchTasks();
  };

  const formatLocalDateTime = (dateStr) => {
    const date = new Date(dateStr);
    const offset = date.getTimezoneOffset();
    const local = new Date(date.getTime() - offset * 60000);
    return local.toISOString().slice(0, 16);
  };

  return (
    <div className="container">
      <h1>Lista de tareas</h1>
      <ul>
        {tasks.map(task => {
          const now = new Date();
          const due = new Date(task.dueDate);
          const expired = !task.isCompleted && due < now;
          const urgent = !task.isCompleted && due - now <= 3 * 60 * 60 * 1000 && due > now;

          return (
            <li
              key={task.id}
              className={`
                  ${task.isCompleted ? 'completed' : ''}
                  ${expired ? 'expired' : ''}
                  ${urgent ? 'urgent' : ''}
                `}

              onClick={() => setSelectedTask(task)}
            >
              <div>
                <strong>{task.title}</strong><br />
                <small>
                  {expired
                    ? <>Vencida: {due.toLocaleString()} ⚠️</>
                    : <>Vence: {due.toLocaleString()}</>
                  }
                </small>
              </div>
              <div className="status">
                {!task.isCompleted ? (
                  expired ? (
                    <span>💀</span>
                  ) : (
                    <span>⏳</span>
                  )
                ) : (
                  <span>✅</span>
                )}
                {!task.isCompleted && (
                  <button className="complete-btn" onClick={(e) => {
                    e.stopPropagation();
                    handleComplete(task.id);
                  }}>
                    Completar
                  </button>
                )}
                <span
                  className="delete-icon"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleDelete(task.id);
                  }}
                >
                  🗑️
                </span>
              </div>
            </li>
          );
        })}
      </ul>

      <h2>Añadir nueva tarea</h2>
      <form className="task-form" onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Título"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
        />
        <input
          type="datetime-local"
          value={dueDate}
          onChange={(e) => setDueDate(e.target.value)}
          required
        />
        <button type="submit">Añadir</button>
      </form>

      {notification && (
        <div className="notification warning">
          {notification}
          <button className="close-btn" onClick={() => setNotification(null)}>×</button>
        </div>
      )}

      {selectedTask && (
        <div className="modal">
          <div className="modal-content">
            <h2>Editar tarea</h2>
            <form onSubmit={handleEditSubmit}>
              <input
                type="text"
                value={selectedTask.title}
                onChange={(e) =>
                  setSelectedTask({ ...selectedTask, title: e.target.value })
                }
              />
              <input
                type="datetime-local"
                value={formatLocalDateTime(selectedTask.dueDate)}
                onChange={(e) =>
                  setSelectedTask({ ...selectedTask, dueDate: e.target.value })
                }
              />
              <div style={{ display: 'flex', justifyContent: 'space-between', gap: '10px' }}>
                <button type="submit" className="complete-btn">Guardar</button>
                <button
                  type="button"
                  className="complete-btn"
                  onClick={() => setSelectedTask(null)}
                  style={{ backgroundColor: '#999' }}
                >
                  Cancelar
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default App;
