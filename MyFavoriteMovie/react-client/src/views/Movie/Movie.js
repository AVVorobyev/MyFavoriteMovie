import axios from "axios";
import React, { Component } from "react";
import { Button } from "react-bootstrap";
import { NavLink, Navigate } from "react-router-dom";

export class Movie extends Component {

    constructor(props) {
        super(props);
        this.state = { movie: Object, redirect: false };
    }

    setRedirect() {
        this.setState({ redirect: true });
    }

    renderRedirect() {
        if (this.state.redirect) {
            return <Navigate to="/Movie/Movies" />
        }
    }

    getMovieById() {
        axios({
            method: 'GET',
            url: process.env.REACT_APP_API_URL_Movie + 'Movie',
            params: {
                id: this.props.movieId
            }
        }).then(response => {
            this.setState({ movie: response.data });
        })
    }

    componentDidMount() {
        this.getMovieById();
    }
    
    deleteMovie(movie) {
        console.log(movie);
        if (window.confirm('Are you sure?')) {
            axios({
                method: "DELETE",
                url: process.env.REACT_APP_API_URL_Movie + 'Delete',
                params: {
                    id: movie.Id
                }
            }).then(response => {
                alert(response.data);
                this.setRedirect();


            }, (error) => {
                alert("Error!");
            });
        }
    }

    render() {
        const { movie } = this.state;
        return (
            <div>
                {this.renderRedirect()}
                <h1>
                    Movie page MovieId = {movie.Id}
                    <h4>
                        <h5 class="text-success">{movie.Name}</h5>
                        <h5 class="text-success">{movie.Poster}</h5>
                        <h5 class="text-success">{movie.RealeseDate}</h5>
                    </h4>
                </h1>
                <NavLink to="/Movie/Movies" className="btn btn-primary">Back</NavLink>
                <Button
                    onClick={() => { this.deleteMovie(movie);  }}
                    variant='danger'>Delete</Button>
            </div>
        )
    }
}