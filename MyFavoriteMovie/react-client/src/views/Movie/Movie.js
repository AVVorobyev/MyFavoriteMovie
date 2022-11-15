import axios from "axios";
import React, { Component } from "react";
import { Button, ButtonToolbar, Image } from "react-bootstrap";
import { NavLink, Navigate } from "react-router-dom";
import { EditMovieModal } from "./EditMovieModal";
import '../../../src/styles/Main.css';
import DateFormater from "../../components/DateFormater";
import TimeSpanFormater from "../../components/TimeSpanFormater";
import { EditActorsListModal } from "./EditActorsListModal";

const defaultPosterImage = process.env.REACT_APP_Default_Images + "defaultPosterImage.jpg";

export class Movie extends Component {

    constructor(props) {
        super(props);
        this.state = { movie: [], redirect: false, editMovieShow: false, posterImage: String, editActorsListShow: false };
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

        if (window.confirm('Are you sure?')) {
            axios({
                method: "DELETE",
                url: process.env.REACT_APP_API_URL_Movie + 'Delete',
                params : {
                    movieId : movie.Id
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
        let { movie } = this.state;
        let { posterImage } = this.state;

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
                            <div className="main_inline main_info_row_result"><DateFormater date={movie.ReleaseDate}></DateFormater></div>
                        </div>

                        <div className="main_info_row">
                            <div className="main_inline main_info_row_property_name">Duration</div>
                            <div className="main_inline main_info_row_result"><TimeSpanFormater span={movie.Duration}></TimeSpanFormater></div>
                        </div>

                        <div className="main_inline main_info_row main_info_row_result main_info_row_description"><p>{movie.Description}</p></div>
                    </div>

                    <div className="main_inline main_container_list">
                        <div className="main_info_row main_info_row_about">In roles</div>

                        <table>
                            <tbody>
                                {movie.Actors?.map(actor =>
                                    <tr key={actor.Id}>
                                        <NavLink
                                            to={"/Actor/Actor/" + actor.Id}
                                            className="main_navlink">
                                            <div className="main_inline main_further_info_row_result_W200">
                                                {actor.Name} {actor.Surname}
                                            </div>
                                        </NavLink>

                                    </tr>
                                )}
                            </tbody>
                        </table>

                        <Button
                            onClick={() => this.setState({ editActorsListShow: true })}
                        >Edit Actor List</Button>

                        <EditActorsListModal
                            show={this.state.editActorsListShow}
                            onHide={() => this.setState({ editActorsListShow: false })}
                            movieid={movie.Id}
                        ></EditActorsListModal>
                    </div>
                </div >

                <ButtonToolbar>
                    <NavLink to="/Movie/Movies" className="btn btn-primary">To Movies</NavLink>
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
            </div >
        )
    }
}