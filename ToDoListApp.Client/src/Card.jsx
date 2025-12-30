import { useState } from 'react';

function Card({ tasks = [], onDelete, onUpdate }) {
    const [editingId, setEditingId] = useState(null);
    const [editForm, setEditForm] = useState({
        content: "",
        dueDate: "",
        priority: "",
        status: ""
    });

    const startEdit = (task) => {
        setEditingId(task.id);
        setEditForm({
            content: task.content,
            dueDate: task.dueDate,
            priority: task.priority,
            status: task.status
        });
    };

    const saveEdit = (task) => {
        onUpdate({ ...task, ...editForm });
        setEditingId(null);
    };

    return (
        <div className="card-container">
            {tasks.map((task) => {
                let priorityClass = "";
                if (task.priority === "High") priorityClass = "high";
                else if (task.priority === "Medium") priorityClass = "medium";
                else if (task.priority === "Low") priorityClass = "low";

                return (
                    <div key={task.id} className={`task-card ${priorityClass}`}>
                        {editingId === task.id ? (
                            <input
                                value={editForm.content}
                                onChange={(e) =>
                                    setEditForm({ ...editForm, content: e.target.value })
                                }
                            />
                        ) : (
                            <h3>{task.content}</h3>
                        )}

                        
                        <div className="task-info">
                            <p>
                                <strong>Due Date: </strong>

                                {editingId === task.id ? (
                                    <input
                                        type="date"
                                        value={editForm.dueDate}
                                        onChange={(e) =>
                                            setEditForm({ ...editForm, dueDate: e.target.value })
                                        }
                                    />
                                ) : (
                                    task.dueDate
                                )}
                            </p>

                            <p>
                                <strong>Priority: </strong>
                            
                                {editingId === task.id ? (
                                    <select
                                        value={editForm.priority}
                                        onChange={(e) =>
                                            setEditForm({ ...editForm, priority: e.target.value })
                                        }
                                    >
                                        <option value="High">High</option>
                                        <option value="Medium">Medium</option>
                                        <option value="Low">Low</option>
                                    </select>
                                ) :
                                    task.priority
                                }
                            </p>
                            
                            <p>
                                <strong>Status: </strong>
                            
                                {editingId === task.id ? (
                                    <select
                                        value={editForm.status}
                                        onChange={(e) =>
                                            setEditForm({ ...editForm, status: e.target.value })
                                        }
                                    >
                                        <option value="Pending">Pending</option>
                                        <option value="InProgress">InProgress</option>
                                        <option value="Completed">Completed</option>
                                    </select>
                                ) : (
                                    task.status
                                )}
                            </p>
                        </div>

                        <div className="task-actions">
                            {editingId === task.id ? (
                                <button onClick={() => saveEdit(task)}>
                                    Save
                                </button>
                            ) : (
                                <button onClick={() => startEdit(task)}>
                                    Edit
                                </button>
                            )}

                            <button onClick={() => onDelete(task.id)}>
                                Delete
                            </button>
                        </div>
                    </div>
                );
            })}
        </div>
    );
}

export default Card;
