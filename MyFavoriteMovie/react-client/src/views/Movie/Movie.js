import axios from "axios";
import React, { Component } from "react";
import { Button, ButtonToolbar } from "react-bootstrap";
import { NavLink, Navigate } from "react-router-dom";
import { EditMovieModal } from "./EditMovieModal";

export class Movie extends Component {

    constructor(props) {
        super(props);
        this.state = { movie: Object, redirect: false, editMovieShow: false };
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
                    Movie page
                    <h2>MovieId = {movie.Id}</h2>
                    <h4>
                        <h5 class="text-success">{movie.Name}</h5>
                        <h5 class="text-success">{movie.Poster}</h5>
                        <h5 class="text-success">{movie.RealeseDate}</h5>
                    </h4>
                </h1>
                <ButtonToolbar>
                    <NavLink to="/Movie/Movies" className="btn btn-primary">Back</NavLink>
                    <Button onClick={() => this.setState({ editMovieShow: true, movie: movie })}
                        variant='info'>Edit</Button>
                    <Button
                        onClick={() => { this.deleteMovie(movie); }}
                        variant='danger'>Delete</Button>

                    <EditMovieModal show={this.state.editMovieShow}
                        onHide={() => this.setState({ editMovieShow: false })}
                        Id={movie.Id}
                        Name={movie.Name}
                        Poster={movie.Poster}>
                    </EditMovieModal>
                </ButtonToolbar>
            </div>
        )
    }
}