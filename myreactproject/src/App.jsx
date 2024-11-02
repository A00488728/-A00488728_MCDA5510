// src/App.js
import React, { useState } from 'react';

import { BrowserRouter as Router, Routes, Route, useNavigate } from 'react-router-dom';
import Home from './Home';
import Weather from './Weather';
import './index.css';


function App() {
    const [showWeather, setShowWeather] = useState(false);

    const handleShowWeather = () => {
        setShowWeather(true);
    };

    const handleBackToHome = () => {
        setShowWeather(false);
    };

    return (
        <div>
            {showWeather ? (
                <Weather onBack={handleBackToHome} />
            ) : (
                <Home onShowWeather={handleShowWeather} />
            )}
            </div>
      
    );
}

export default App;
