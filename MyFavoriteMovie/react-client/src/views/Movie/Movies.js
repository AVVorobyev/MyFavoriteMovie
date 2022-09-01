import axios from "axios";
import React, { Component } from "react";
import { Table } from "react-bootstrap";
import { Button } from "react-bootstrap";
import { ButtonToolbar } from "react-bootstrap";
import { AddMovieModal } from "./AddMovieModal";
import { NavLink } from "react-router-dom";

export class Movies extends Component {

    constructor(props) {
        super(props);
        this.state = { movies: [], addModalShow: false }
        this.handleMovieListChange = this.handleMovieListChange.bind(this);
    }

    getMoviesList() {
        axios.get(process.env.REACT_APP_API_URL_Movie + 'Movies')
            .then(response => {
                this.setState({ movies: response.data });
            })
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

    render() {
        const { movies } = this.state;
        let addModalClose = () => this.setState({ addModalShow: false })

        return (
            <div>
                <h1>Movies page</h1>

                <Table>
                    <tbody>
                        {movies.map(movie =>
                            <tr key={movie.Id}>
                                <NavLink
                                    to={"/Movie/Movie/" + movie.Id}
                                    className="btn">
                                    <td >{movie.Name}</td>
                                    <td >{movie.Title}</td>
                                    <td >{movie.RealeseDate}</td>
                                </NavLink>
                            </tr>
                        )}
                    </tbody>
                </Table>
                <ButtonToolbar>
                    <Button
                        onClick={() => this.setState({ addModalShow: true })}>
                        Add Movie
                    </Button>
                    <AddMovieModal show={this.state.addModalShow}
                        onHide={addModalClose}>
                    </AddMovieModal>
                </ButtonToolbar>
            </div >
        )
    }
}