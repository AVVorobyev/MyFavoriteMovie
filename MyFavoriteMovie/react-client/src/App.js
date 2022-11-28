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
import { Login } from './views/Auth/Login';
import axios from 'axios';

axios.interceptors.request.use(
  options => {
    options.withCredentials = true;
    return options;
  }, error => {
    return Promise.reject(error);
  }
)

axios.interceptors.response.use(response => {
  response.withCredentials = true;
  return response;
}, error => {
  if (error.response.status === 401) {
    window.location = "/Auth/Login"
  }
  else
    return Promise.reject(error);
});


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
          <Route path='/Auth/Login' element={<Login />} />
        </Routes>
      </Router>

    </div>
  );
}

export default App;
