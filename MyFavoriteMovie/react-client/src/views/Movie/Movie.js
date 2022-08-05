import axios from "axios";
import React, { Component } from "react";

import { Table } from "react-bootstrap";

export class Movie extends Component {

    constructor(props) {
        super(props);
        this.state = { movies: [] }
    }

    getMoviesList() {
        axios.get(process.env.REACT_APP_API_URL + 'Movie/GetAll')
            .then(response => {
                this.setState({ movies: response.data });
            })
    }

    componentDidMount() {
        this.getMoviesList();
    }

    render() {
        const { movies } = this.state;

        return (
            <div>
                <h1>Movie page</h1>

                <Table>
                    <tbody>
                        {movies.map(movie =>
                            <tr key={movie.Id}>
                                <td>{movie.Name}</td>
                                <td>{movie.Poster}</td>
                                <td>{movie.RealeseDate}</td>

                            </tr>
                        )}
                    </tbody>
                </Table>

            </div>

        )
    }


}