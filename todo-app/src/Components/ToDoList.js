import React, { useEffect, useState } from 'react';
import {
    getToDoItems,
    deleteToDoItem,
    searchToDoItems,
    createToDoItem,
    updateToDoItem
} from '../services/todoService';

function ToDoList() {
    // State hooks to manage todos, search queries, editing states, new todo input, and category filter
    const [todos, setTodos] = useState([]);
    const [searchQuery, setSearchQuery] = useState("");
    const [editingTodo, setEditingTodo] = useState(null);
    const [newTodo, setNewTodo] = useState({ id: '', title: '', description: '', category: 'personal', priority: 'low' });
    const [filteredCategory, setFilteredCategory] = useState(""); // State to filter todos by category

    // Fetch all todos on initial render
    useEffect(() => {
        loadTodos();
    }, []);

    // Debounced search to reduce unnecessary API calls while the user is typing
    useEffect(() => {
        const debounceTimeout = setTimeout(() => {
            handleSearch();
        }, 500); // 500ms debounce

        // Cleanup the timeout when the component re-renders
        return () => clearTimeout(debounceTimeout);
    }, [searchQuery]);

    // Fetch todos from the server
    const loadTodos = () => {
        getToDoItems()
            .then(response => setTodos(response.data))
            .catch(error => console.error(error));
    };

    // Handle the deletion of a todo
    const handleDelete = (id) => {
        deleteToDoItem(id)
            .then(() => loadTodos()) // Reload todos after deletion
            .catch(error => console.error(error));
    };

    // Handle the search functionality
    const handleSearch = () => {
        if (searchQuery.trim() === "") {
            loadTodos(); // If search query is empty, reload all todos
        } else {
            searchToDoItems(searchQuery)
                .then(response => setTodos(response.data))
                .catch(error => console.error(error));
        }
    };

    // Prepare todo item for editing
    const handleEdit = (todo) => {
        setEditingTodo(todo);
        setNewTodo(todo); // Prepopulate form with selected todo's data
    };

    // Handle the creation or updating of a todo item
    const handleCreateOrUpdate = () => {
        if (editingTodo) {
            // Update existing todo
            updateToDoItem(editingTodo.id, newTodo)
                .then(() => {
                    loadTodos(); // Reload todos after update
                    setEditingTodo(null); // Reset editing state
                    resetNewTodo(); // Reset the new todo form
                })
                .catch(error => console.error(error));
        } else {
            // Create new todo
            createToDoItem(newTodo)
                .then(() => {
                    loadTodos(); // Reload todos after creation
                    resetNewTodo(); // Reset the new todo form
                })
                .catch(error => console.error(error));
        }
    };

    // Helper function to reset the new todo form
    const resetNewTodo = () => {
        setNewTodo({ id: '', title: '', description: '', category: 'personal', priority: 'low' });
    };

    // Categorize todos by priority, also applying category filters if set
    const categorizeTodosByPriority = (priorityLevel) => {
        return todos.filter(todo => todo.priority === priorityLevel && (!filteredCategory || todo.category === filteredCategory));
    };

    return (
        <div>
            {/* Search input for filtering todos by title or description */}
            <input
                type="text"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                placeholder="Search todos..."
            />

            {/* Create/Edit Form for todo items */}
            <div>
                <h4>{editingTodo ? "Edit ToDo" : "Create ToDo"}</h4>

                {/* ID Field - Read-only when editing an existing todo */}
                <input
                    type="text"
                    value={newTodo.id}
                    onChange={(e) => setNewTodo({ ...newTodo, id: e.target.value })}
                    placeholder="ID"
                    disabled={!!editingTodo} // Disable ID input while editing
                />

                <input
                    type="text"
                    value={newTodo.title}
                    onChange={(e) => setNewTodo({ ...newTodo, title: e.target.value })}
                    placeholder="Title"
                />

                <input
                    type="text"
                    value={newTodo.description}
                    onChange={(e) => setNewTodo({ ...newTodo, description: e.target.value })}
                    placeholder="Description"
                />

                {/* Dropdown for selecting category */}
                <select
                    value={newTodo.category}
                    onChange={(e) => setNewTodo({ ...newTodo, category: e.target.value })}
                >
                    <option value="work">Work</option>
                    <option value="personal">Personal</option>
                </select>

                {/* Dropdown for selecting priority */}
                <select
                    value={newTodo.priority}
                    onChange={(e) => setNewTodo({ ...newTodo, priority: e.target.value })}
                >
                    <option value="high">High</option>
                    <option value="medium">Medium</option>
                    <option value="low">Low</option>
                </select>

                <button onClick={handleCreateOrUpdate}>
                    {editingTodo ? "Update" : "Create"}
                </button>

                {/* Cancel editing */}
                {editingTodo && (
                    <button onClick={() => {
                        setEditingTodo(null); // Exit editing mode
                        resetNewTodo(); // Reset the form
                    }}>
                        Cancel
                    </button>
                )}
            </div>

            {/* Category Filter Buttons for viewing todos by category */}
            <div>
                <h4>View</h4>
                <button onClick={() => setFilteredCategory('')}>View All</button>
                <button onClick={() => setFilteredCategory('work')}>View Work</button>
                <button onClick={() => setFilteredCategory('personal')}>View Personal</button>
            </div>

            {/* Display High Priority Todos */}
            <h4>High Priority</h4>
            <ul style={{ listStyleType: 'none' }}>
                {categorizeTodosByPriority('high').map(todo => (
                    <li key={todo.id}>
                        {todo.id} - {todo.title} - {todo.description} - {todo.category} - {todo.priority}
                        <button onClick={() => handleEdit(todo)}>Edit</button>
                        <button onClick={() => handleDelete(todo.id)}>Delete</button>
                    </li>
                ))}
            </ul>

            {/* Display Medium Priority Todos */}
            <h4>Medium Priority</h4>
            <ul style={{ listStyleType: 'none' }}>
                {categorizeTodosByPriority('medium').map(todo => (
                    <li key={todo.id}>
                        {todo.id} - {todo.title} - {todo.description} - {todo.category} - {todo.priority}
                        <button onClick={() => handleEdit(todo)}>Edit</button>
                        <button onClick={() => handleDelete(todo.id)}>Delete</button>
                    </li>
                ))}
            </ul>

            {/* Display Low Priority Todos */}
            <h4>Low Priority</h4>
            <ul style={{ listStyleType: 'none' }}>
                {categorizeTodosByPriority('low').map(todo => (
                    <li key={todo.id}>
                        {todo.id} - {todo.title} - {todo.description} - {todo.category} - {todo.priority}
                        <button onClick={() => handleEdit(todo)}>Edit</button>
                        <button onClick={() => handleDelete(todo.id)}>Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default ToDoList;
