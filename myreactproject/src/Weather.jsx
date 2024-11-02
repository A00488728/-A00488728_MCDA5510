import React, { useEffect, useState } from 'react';
import axios from 'axios';

import './index.css';
import exampleImage1 from './images/Halifax1.jpg';
import exampleImage2 from './images/Halifax2.jpg';
import coldImage from './images/cold.png'; // Change to your cold image path
import warmImage from './images/mild.png'; // Change to your warm image path
import sunnyImage from './images/sunny.png'; // Change to your cold image path


function Weather() {

    const handleClick = () => {
        window.location.replace('./Home'); 
    };

 
    const [temperature, setTemperature] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
   
    const [isCelsius, setIsCelsius] = useState(true);


    useEffect(() => {
        const fetchWeather = async () => {
            const apiKey = 'ace38e205ecc9173b9dbf66d85d440a4';
            const city = 'Halifax';
            const url = `https://api.openweathermap.org/data/2.5/weather?q=${city}&appid=${apiKey}&units=metric`;

            try {
                const response = await axios.get(url);
                setTemperature(response.data.main.temp);
                
            } catch (err) {
                setError('Failed to fetch weather data');
            } finally {
                setLoading(false);
            }
        };

        fetchWeather();
    }, []);


    const convertToFahrenheit = (celsius) => (celsius * 9 / 5) + 32;

    const toggleTemperature = () => {
        setIsCelsius(!isCelsius);
    };

    let displayedImage;
    if (temperature < 10) {
        displayedImage = <img src={coldImage} alt="Cold weather" style={{ width: '300px' }} />;
    } else if (temperature >= 10 && temperature < 20) {
        displayedImage = <img src={warmImage} alt="Cold weather" style={{ width: '300px' }} />;
    }
    else {
        displayedImage = <img src={sunnyImage} alt="Warm weather" style={{ width: '300px' }} />;
    }

    const text = isCelsius ? "Convert to Fahrenheit" : "Convert to Celsius";

    const displayedTemperature = isCelsius ? temperature : convertToFahrenheit(temperature);
    return (
        <><div>
            <h1>Current Temperature in Halifax</h1></div><div>{loading && <p>Loading...</p>}
                {error && <p>{error}</p>}
                {temperature !== null && (

                    <><div className="grid-container"><img src={exampleImage2} alt="Description of the image" className="image" /><img src={exampleImage1} alt="Description of the image" className="image" /></div><body><><><><><p>Halifax, the capital of Nova Scotia, Canada, is a vibrant coastal city known for its rich maritime heritage and stunning waterfront. Founded in 1749, it features historic sites like the Halifax Citadel and the Canadian Museum of Immigration at Pier 21. The waterfront is lively with shops, restaurants, and the Halifax Seaport Market. Home to several universities, Halifax is a hub for education and innovation. Its parks, such as Point Pleasant Park, offer outdoor activities, while the city's cultural scene thrives with galleries, theaters, and music festivals, making Halifax a unique blend of history, culture, and modernity.</p><h2>The current temperature in Halifax {displayedTemperature.toFixed(2)}&#x00B0;{isCelsius ? 'C' : 'F'} </h2></></><p><button onClick={toggleTemperature} className="weather-button">{text}</button>
                        <p className="weather-button">{displayedImage}</p><p><button onClick={handleClick} className="weather-button">About Me</button></p></p></><></></>
                    </body></>
                    
                )}</div></>

                

    );
}

export default Weather;
