import './App.css';
import Card from './Card';
import Dialog from './Dialog';
import { useState, useEffect } from 'react';

function App() {
    const [showDialog, setShowDialog] = useState(false);
    const [task, setTask] = useState([]);

    // GET
    useEffect(() => {
        fetch(`${import.meta.env.VITE_API_URL}/ToDoList`)
            .then(res => res.json())
            .then(data => setTask(data))
            .catch(err => console.error(err));
    }, []);

    const addTask = (newTask) => {
        setTask(prev => [...prev, newTask]);
    };

    // DELETE
    const deleteTask = (id) => {
        fetch(`${import.meta.env.VITE_API_URL}/ToDoList/${id}`, {
            method: "DELETE",
        })
            .then(res => res.json())
            .then(data => setTask(data))
            .catch(err => console.error(err));
    };

    // UPDATE
    const updateTask = (updatedTask) => {
        fetch(`${import.meta.env.VITE_API_URL}/ToDoList/${updatedTask.id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updatedTask),
        })
            .then(res => res.json())
            .then(data => setTask(data))
            .catch(err => console.error(err));
    };

    return (
        <div>
            <header>
                <h1>My To-Do List!</h1>
            </header>

            <div>
                <Card
                    tasks={task}
                    onDelete={deleteTask}
                    onUpdate={updateTask}
                />
            </div>

            <button onClick={() => setShowDialog(true)}>
                Add Task
            </button>

            {showDialog && (
                <Dialog
                    onClose={() => setShowDialog(false)}
                    onAddTask={addTask}
                />
            )}
        </div>
    );
}

export default App;
