import axios from "axios";
import React, { Component } from "react";
import { Button } from "react-bootstrap";
import { ButtonToolbar } from "react-bootstrap";
import { AddMovieModal } from "./AddMovieModal";
import { NavLink } from "react-router-dom";
import DateFormater from '../../components/DateFormater.js';
import TimeSpanFormater from "../../components/TimeSpanFormater";

export class Movies extends Component {

    constructor(props) {
        super(props);
        this.state = { movies: [], addModalShow: false, skip: 0, take: 3, movieCount: 0 }
        this.handleMovieListChange = this.handleMovieListChange.bind(this);
    }

    getMoviesList() {
        axios({
            url: process.env.REACT_APP_API_URL_Movie + 'Movies',
            method: "GET",
            params: {
                skip: this.state.skip,
                take: this.state.take
            }
        }).then(response => {
            this.setState({
                movies: response.data.Movies,
                movieCount: response.data.Count
            });
        }, () => {
        });
    }

    componentDidMount() {
        this.getMoviesList();
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.movies === this.state.movies)
            this.getMoviesList();
    }

    handleMovieListChange() {
        this.getMoviesList();
    }

    handleNextPageChanged() {
        if (this.state.skip + this.state.take >= this.state.movieCount)
            return;
        else {
            this.setState({ skip: this.state.skip + this.state.take });
        }
    }

    handlePreviousPageChanged() {
        if (this.state.skip <= 0)
            return;
        else {
            this.setState({ skip: this.state.skip - this.state.take });
        }
    }

    render() {
        const { movies } = this.state;
        let addModalClose = () => this.setState({ addModalShow: false })

        return (
            <div>
                <h1>Movies</h1>

                <div className="main_lists_container">
                    <table>
                        <tbody>
                            {movies.map(movie =>
                                <tr key={movie.Id}>
                                    <NavLink
                                        to={"/Movie/Movie/" + movie.Id}
                                        className="btn main_lists_navlink_btn">
                                        <td className="main_lists_title">{movie.Name}</td>
                                        <td className="main_lists_info">{movie.Description}</td>
                                        <td className="main_lists_info">Release Date: <DateFormater date={movie.ReleaseDate}></DateFormater></td>
                                        <td className="main_lists_info">Duration: <TimeSpanFormater span={movie.Duration}></TimeSpanFormater></td>
                                    </NavLink>

                                    <div className="delimiter_H10"></div>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>

                <div className="delimiter_H10"></div>

                <ButtonToolbar>
                    <Button
                        onClick={() => this.setState({ addModalShow: true })}>
                        Add Movie
                    </Button>
                    <AddMovieModal
                        show={this.state.addModalShow}
                        onHide={addModalClose}>
                    </AddMovieModal>

                    <Button className="danger" onClick={() => { this.handlePreviousPageChanged(); }}>Previous</Button>
                    <Button className="danger" onClick={() => { this.handleNextPageChanged(); }}>Next</Button>
                </ButtonToolbar>
            </div >
        )
    }
}