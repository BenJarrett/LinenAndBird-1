import './App.css';
import { useState, useEffect } from 'react';
import BirdList from './components/bird/BirdList';
import getAllBirds from './helpers/data/birdData';

function App() {

  const [birds, setBirds] = useState([])

  useEffect(() => {getAllBirds()
    .then(data => setBirds(data))
  },[]);

  // OR //
  // useEffect(() => getAllBirds().then(setBirds),[]); //


  return (
    <div className="App">
    <BirdList birds={birds}/>
    </div>
  );
}

export default App;
