import axios from 'axios';

// Define the base API URL for the ToDo service
const API_URL = 'http://localhost:5000/api/ToDo';

// Get all ToDo items
export const getToDoItems = async () => {
    try {
        const response = await axios.get(API_URL);
        return response;
    } catch (error) {
        console.error("Error fetching ToDo items:", error);
        throw error; // Re-throw error for handling in the component
    }
};

// Get a specific ToDo item by ID
export const getToDoItem = async (id) => {
    try {
        const response = await axios.get(`${API_URL}/${id}`);
        return response;
    } catch (error) {
        console.error(`Error fetching ToDo item with ID: ${id}`, error);
        throw error; // Re-throw error for handling in the component
    }
};

// Create a new ToDo item
export const createToDoItem = async (todo) => {
    try {
        const response = await axios.post(API_URL, todo, {
            headers: {
                'Content-Type': 'application/json',
            },
        });
        return response;
    } catch (error) {
        console.error("Error creating ToDo item:", error);
        throw error; // Re-throw error for handling in the component
    }
};

// Update an existing ToDo item by ID
export const updateToDoItem = async (id, todo) => {
    try {
        const response = await axios.put(`${API_URL}/${id}`, todo);
        return response;
    } catch (error) {
        console.error(`Error updating ToDo item with ID: ${id}`, error);
        throw error; // Re-throw error for handling in the component
    }
};

// Delete a ToDo item by ID
export const deleteToDoItem = async (id) => {
    try {
        const response = await axios.delete(`${API_URL}/${id}`);
        return response;
    } catch (error) {
        console.error(`Error deleting ToDo item with ID: ${id}`, error);
        throw error; // Re-throw error for handling in the component
    }
};

// Search for ToDo items by query (e.g., title or description)
export const searchToDoItems = async (query) => {
    try {
        const response = await axios.get(`${API_URL}/search/${query}`);
        return response;
    } catch (error) {
        console.error(`Error searching ToDo items with query: ${query}`, error);
        throw error; // Re-throw error for handling in the component
    }
};
