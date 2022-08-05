import './App.css';
import {
  BrowserRouter as Router,
  Routes,
  Route,
} from "react-router-dom";
import { Movies } from './views/Movie/Movies';
import { MovieByIdWrapper } from './views/Movie/MovieByIdWrapper';

function App() {
  return (
    <div className="container">
      <h3>
        App page
      </h3>

      <Router>
        <Routes>
          <Route path='/Movie/Movies' element={<Movies />} />
          <Route path='/Movie/Movie/:movieId' element={<MovieByIdWrapper />} />

        </Routes>
      </Router>

    </div>
  );
}

export default App;
