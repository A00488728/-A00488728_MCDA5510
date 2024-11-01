// src/Home.js
import React from 'react';
import './index.css';
import exampleImage from './images/MyPhoto.jpg';

function Home({ onShowWeather }) {
    return (
        <div>
            <h1>Welcome to Anika's Webpage</h1>

            <div className="grid-container"><img src={exampleImage} alt="Description of the image" style={{ maxWidth: '20%', height: 'auto' }} /></div>
            <body>
                <p>Born in Dhaka, Bangladesh, I have lived in various cities throughout my life. At 19, I moved to Toronto to pursue my undergraduate studies, which opened up new opportunities for personal and professional growth. Currently, I work at IBM CIC Halifax as a QA Practitioner, a role I've embraced for the past three years. Now, I am eager to transition my career into software development, where I can apply my skills and passion for coding to create innovative solutions and contribute to impactful projects.</p>
                <p><button onClick={onShowWeather}>A little bit about Halifax city</button></p></body>

            
        </div>
    );
}

export default Home;
