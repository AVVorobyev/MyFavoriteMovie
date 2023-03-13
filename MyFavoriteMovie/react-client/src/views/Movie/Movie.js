import axios from "axios";
import React, { Component } from "react";
import { Button, ButtonToolbar, Image } from "react-bootstrap";
import { NavLink, Navigate } from "react-router-dom";
import { EditMovieModal } from "./EditMovieModal";
import '../../../src/styles/Main.css';
import DateFormater from "../../components/DateFormater";
import TimeSpanFormater from "../../components/TimeSpanFormater";
import { EditActorsListModal } from "./EditActorsListModal";
import { Header } from "../../components/Header/Header";
import Visible from "../../components/Auth/Visible";
import getRoleFromCookies from "../../components/Auth/GetRoleFromCookies";
import { Role } from "../../components/Auth/Roles";

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
            this.setState({ movie: response.data.Result });

            if (response.data.Result.Poster === null || response.data.Result.Poster === undefined) {
                this.setState({ posterImage: defaultPosterImage });
            }
            else {
                this.setState({ posterImage: response.data.Result.Poster });
            }
        })
    }

    componentDidMount() {
        this.getMovieById();
    }

    componentDidUpdate(prevProps) {
        if (this.props.movieId !== prevProps.movieId)
            this.getMovieById();
    }

    deleteMovie(movie) {
        if (window.confirm('Are you sure?')) {
            axios({
                method: "DELETE",
                url: process.env.REACT_APP_API_URL_Movie + 'Delete',
                params: {
                    movieId: movie.Id
                }
            }).then(response => {
                if (response.data.Success === true)
                    this.setRedirect();
                else
                    alert(response.data);
            });
        }
    }

    render() {
        let { movie } = this.state;
        let { posterImage } = this.state;
        const role = getRoleFromCookies();

        return (
            <div>
                <Header />

                {this.renderRedirect()}

                <div className="main_container">

                    <div className="main_inline main_image_container">
                        <Image
                            className="main_image"
                            src={posterImage}
                            alt="poster"
                        />
                    </div>

                    <div className="main_inline main_delimiter_W50"></div>

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

                        <div className="delimiter_H10"></div>

                        <Visible
                            component={
                                <ButtonToolbar>
                                    <Button
                                        onClick={() => this.setState({ editActorsListShow: true })}
                                    >Edit Actor List
                                    </Button>

                                    <EditActorsListModal
                                        show={this.state.editActorsListShow}
                                        onHide={() => this.setState({ editActorsListShow: false })}
                                        movieid={movie.Id}
                                    ></EditActorsListModal>
                                </ButtonToolbar>
                            }
                            isVisible={role === Role.Administrator || role === Role.Moderator}>
                        </Visible>
                    </div>
                </div >

                <div className="delimiter_H10"></div>

                <NavLink to="/Movie/Movies" className="btn btn-primary">To Movies</NavLink>

                <div className="delimiter_H10"></div>

                <Visible
                    component={
                        <ButtonToolbar>
                            <Button onClick={() => this.setState({ editMovieShow: true, movie: movie })}
                                variant='info'>
                                Edit
                            </Button>

                            <div className="main_delimiter_W10"></div>

                            <Button
                                onClick={() => { this.deleteMovie(movie); }}
                                variant='danger'>
                                Delete
                            </Button>

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
                    }
                    isVisible={role === Role.Administrator || role === Role.Moderator}>
                </Visible>
            </div >
        )
    }
}