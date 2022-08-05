import './App.css';
import {
  BrowserRouter as Router,
  Routes,
  Route,
} from "react-router-dom";

import {Movie} from './views/Movie/Movie';

function App() {
  return (
      <div className="container">
        <h3>
          App page
        </h3>

        <Router>
          <Routes>
            <Route path='/Movie' element={<Movie />} />

            
          </Routes>
        </Router>

      </div>
  );
}

export default App;
