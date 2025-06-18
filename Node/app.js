const express = require('express');
const axios = require('axios');
const app = express();
const PORT = process.env.PORT || 3000;

// הכנס את ה-API Key שלך כאן
const API_KEY = 'rnd_tkjPrztp93YsgwnFY5NdQIyvxr8o';

app.get('/apps', async (req, res) => {
    try {
        const response = await axios.get('https://api.render.com/v1/services', {
            headers: {
                'Authorization': `Bearer ${API_KEY}`
            }
        });
        res.json(response.data); // מחזיר את רשימת האפליקציות כ-JSON
    } catch (error) {
        console.error(error);
        res.status(500).send('Error fetching data from Render API');
    }
});

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});
