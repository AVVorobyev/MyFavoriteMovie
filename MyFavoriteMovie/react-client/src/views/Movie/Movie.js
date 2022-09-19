import axios from "axios";
import React, { Component } from "react";
import { Button, ButtonToolbar, Image } from "react-bootstrap";
import { NavLink, Navigate } from "react-router-dom";
import { EditMovieModal } from "./EditMovieModal";
import Moment from 'react-moment';
import '../../../src/Main.css';

const defaultPosterImage = process.env.REACT_APP_Default_Images + "defaultPosterImage.jpg";

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
            let fileName = this.state.posterImage.split('/').pop();
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

    formatDate(string) {
        if (string == null)
            return "-";
        else
            return <Moment format="DD.MM.YYYY">{string}</Moment>
    }

    formatTimeSpan(string) {
        if (string == null)
            return "-";
        else {
            let moment = require('moment');
            let m = moment.duration(string).asMinutes();
            return m + " min.";
        }
    }

    render() {
        const { movie, posterImage } = this.state;

        return (
            <div>
                {this.renderRedirect()}
                <div className="main_container">

                    <div className="main_inline main_image_container">
                        <Image
                            className="main_image"
                            src={posterImage}
                            alt="poster"
                        />
                    </div>

                    <div className="main_inline main_delimiter_W100"></div>

                    <div className="main_inline main_info_container">
                        <div className="main_info_row main_title">{movie.Name}</div>

                        <div className="main_info_row main_info_row_about">About</div>

                        <div className="main_info_row">
                            <div className="main_inline main_info_row_property_name">Release Date</div>
                            <div className="main_inline main_info_row_result">{this.formatDate(movie.ReleaseDate)}</div>
                        </div>

                        <div className="main_info_row">
                            <div className="main_inline main_info_row_property_name">Duration</div>
                            <div className="main_inline main_info_row_result">{this.formatTimeSpan(movie.Duration)}</div>
                        </div>

                        <div className="main_inline main_info_row main_info_row_result main_info_row_description"><p>{movie.Description}</p></div>
                    </div>
                </div>
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
                        Description={movie.Description}
                        ReleaseDate={movie.ReleaseDate}
                        Duration={movie.Duration}
                        Poster={posterImage}>
                    </EditMovieModal>
                </ButtonToolbar>
            </div>
        )
    }
}