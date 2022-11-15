import './App.css';
import {
  BrowserRouter as Router,
  Routes,
  Route,
} from "react-router-dom";
import { Movies } from './views/Movie/Movies';
import { MovieByIdWrapper } from './views/Movie/MovieByIdWrapper';
import { Actors } from './views/Actor/Actors';
import { ActorByIdWrapper } from './views/Actor/ActorByIdWrapper';
import { Registration } from './views/Auth/Registation';

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
          <Route path='/Actor/Actors' element={<Actors />} />
          <Route path='/Actor/Actor/:actorId' element={<ActorByIdWrapper />} />
          <Route path='/Auth/Registration' element={<Registration />} />
        </Routes>
      </Router>

    </div>
  );
}

export default App;
