import { TaskStatus, Priority } from './enum';
import { useState } from 'react';

function Dialog ({ onClose, onAddTask}) {
    const [content, setContent] = useState("");
    const [dueDate, setDueDate] = useState("");
    const [priority, setPriority] = useState("");
    const [status, setStatus] = useState("");

    // POST
    const handleSubmit = () => {
        const task = { id: crypto.randomUUID(), content, dueDate, priority, status };
        const taskJson = JSON.stringify(task)
        console.log(taskJson);
        fetch(`${import.meta.env.VITE_API_URL}/ToDoList`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: taskJson,
            })
            .then(res => res.json())
            .then(data => {
                console.log("Task added:", data);
                onAddTask(task);
                onClose();
        });
    };


    return (
        <div className="popup">
        <h3>Add Task</h3>

            <form
                onSubmit={(e) => {
                    e.preventDefault();
                    handleSubmit();
                }}
            >
                <div className="dialog-group">
                <label htmlFor="content">Content:</label>
                <input
                    id="content"
                    type="text"
                    value={content}
                    onChange={(e) => setContent(e.target.value)}
                    required
                />
                </div>

                <div className="dialog-group">
                <label htmlFor="dueDate">Due Date:</label>
                <input
                    id="dueDate"
                    type="date"
                    value={dueDate}
                    onChange={(e) => setDueDate(e.target.value)}
                    required
                />
                </div>

                <div className="dialog-group">
                    <label htmlFor="priority">Priority:</label>
                    <select
                        id="priority"
                        value={priority}
                        onChange={(e) => setPriority(e.target.value)}
                        required
                    >
                        <option value="" disabled hidden>Select Priority</option>
                        {Object.values(Priority).map((p) => (
                        <option key={p} value={p}>{p}</option>
                        ))}
                    </select>
                </div>

                <div className="dialog-group">
                    <label htmlFor="status">Status:</label>
                    <select
                        id="status"
                        value={status}
                        onChange={(e) => setStatus(e.target.value)}
                        required
                    >
                        <option value="" disabled hidden>Select Status</option>
                        {Object.values(TaskStatus).map((s) => (
                        <option key={s} value={s}>{s}</option>
                        ))}
                    </select>
                </div>

                <div className="popup-actions">
                    <button type="submit">Submit</button>
                    <button type="button" onClick={onClose}>Cancel</button>
                </div>
            </form>
        </div>
    )
}

export default Dialog;