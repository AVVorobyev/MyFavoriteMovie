import axios from "axios";
import React, { Component } from "react";
import { Button, ButtonToolbar, Image } from "react-bootstrap";
import { NavLink, Navigate } from "react-router-dom";
import { EditMovieModal } from "./EditMovieModal";

const defaultPosterImage = '/Files/DefaultImages/defaultPosterImage.jpg';

export class Movie extends Component {

    constructor(props) {
        super(props);
        this.state = { movie: [], redirect: false, editMovieShow: false, posterImage: String };
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

            if (response.data.Poster === null || response.data.Poster === undefined) {
                this.setState({ posterImage: defaultPosterImage });
            }
            else {
                this.setState({ posterImage: response.data.Poster });
            }
        })
    }

    componentDidMount() {
        this.getMovieById();
    }

    deleteMovie(movie) {
        const formData = new FormData();
        formData.append("Id", movie.Id);

        if (this.state.posterImage !== defaultPosterImage) {
            
            let fileName = this.state.posterImage.split('/').pop()
            formData.append("Poster", fileName);
        }

        if (window.confirm('Are you sure?')) {
            axios({
                method: "DELETE",
                url: process.env.REACT_APP_API_URL_Movie + 'Delete',
                data: formData
            }).then(response => {
                alert(response.data);
                this.setRedirect();
            }, (error) => {
                alert("Error!");
            });
        }
    }

    render() {
        const { movie, posterImage } = this.state;

        return (
            <div>
                {this.renderRedirect()}
                <h1>
                    Movie page
                    <h2>MovieId = {movie.Id}</h2>
                    <h4>
                        <Image width={100} height={200}
                            src={posterImage}
                            alt="poster"
                        />
                        <h5 class="text-success">{movie.Name}</h5>
                        <h5 class="text-success">{movie.Title}</h5>
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
                        Title={movie.Title}
                        Poster={posterImage}>
                    </EditMovieModal>
                </ButtonToolbar>
            </div>
        )
    }
}