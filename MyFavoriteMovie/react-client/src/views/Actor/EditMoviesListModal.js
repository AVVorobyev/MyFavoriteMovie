import axios from "axios";
import React, { Component } from "react";
import { Modal, Button, Col, Form, ButtonToolbar, Row } from 'react-bootstrap';
import '../../../src/styles/MainModals.css';

export class EditMoviesListModal extends Component {
    constructor(props) {
        super(props);
        this.state = { movies: [], skip: 0, take: 5, moviesCount: 0, itemsOnPage: 5, filterString: "" }
        this.addMoviesOnPage = this.addMoviesOnPage.bind(this);
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
                movies: response.data.List,
                moviesCount: response.data.Count
            });
        }, () => {
        });
    }

    getFilteredMovieList() {
        axios({
            url: process.env.REACT_APP_API_URL_Movie + 'filter_name',
            method: "GET",
            params: {
                filter: this.state.filterString,
                skip: this.state.skip,
                take: this.state.take
            }
        }).then(response => {
            this.setState({
                movies: response.data.List,
                moviesCount: response.data.Count
            });

        }, () => {
        });
    }

    handleGetMovieList() {
        if (this.state.filterString.length === 0)
            this.getMoviesList();
        else
            this.getFilteredMovieList();
    }

    componentDidMount() {
        this.handleGetMovieList();
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.movies === this.state.movies)
            this.handleGetMovieList();
    }

    async addMoviesOnPage() {
        if (this.state.take <= this.state.moviesCount) {
            await this.setState({ take: this.state.take + this.state.itemsOnPage });

            if (this.state.filterString.length === 0)
                this.getMoviesList();
            else
                this.getFilteredMovieList();
        }
    }

    refreshPage() {
        window.location.reload();
    }

    editMoviesListButton(actorsInMovieList, actorId) {
        let flag = false;

        for (let index = 0; index < actorsInMovieList.length; index++) {
            if (this.props.actorid === actorsInMovieList[index].Id) {
                flag = true;
                break;
            }
        }

        if (!flag)
            return <Button
                onClick={() => { this.handleAddMovie(actorId) }}
                className="main_modal_btn_add">
                Add
            </Button>;
        else
            return <Button
                onClick={() => { this.handleDeleteActor(actorId) }}
                className="main_modal_btn_remove">
                Delete
            </Button>;
    }

    handleAddMovie(movieId) {
        const formData = new FormData();
        formData.append("movieId", movieId);

        axios({
            method: 'PATCH',
            url: process.env.REACT_APP_API_URL_Actor + "AddMovie",
            params: {
                actorId: this.props.actorid,
            },
            data: formData
        }).then(() => {
            if (this.state.filterString.length > 0)
                this.getFilteredMovieList();
            else
                this.getMoviesList();

        }, (error) => {
            alert("error!");
        });
    }

    handleDeleteActor(movieId) {
        const formData = new FormData();
        formData.append("movieId", movieId);

        axios({
            method: 'PATCH',
            url: process.env.REACT_APP_API_URL_Actor + "DeleteMovie",
            params: {
                actorId: this.props.actorid,
            },
            data: formData
        }).then(() => {
            if (this.state.filterString.length > 0)
                this.getFilteredMovieList();
            else
                this.getMoviesList();
        }, (error) => {
            alert("error!");
        });
    }

    render() {
        let { movies, moviesCount } = this.state;

        return (
            <div>

                <Modal
                    {...this.props}
                    size="lg"
                    aria-labelledby="contained-modal-title-vcenter"
                    centered
                    className="main_modal_container">
                    <Modal.Header>
                        <Modal.Title>
                            Edit Movie List
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Col>
                            <Form>
                                <table>
                                    <tbody>
                                        <Form.Group>
                                            <Form.Control
                                                onChange={async (e) => {
                                                    await this.setState({ filterString: e.target.value });
                                                    this.handleGetMovieList();
                                                }}
                                                type="text"
                                                placeholder="Filter"
                                                name="Filter">
                                            </Form.Control>

                                            <div className="delimiter_H10"></div>

                                        </Form.Group>
                                        {movies.map(movie =>
                                            <tr key={movie.Id}>
                                                <td>
                                                    <Form.Group>
                                                        <Row>
                                                            <ButtonToolbar>
                                                                <div className="main_modal_info">{movie.Name}</div>
                                                                {this.editMoviesListButton(movie.Actors, movie.Id)}
                                                            </ButtonToolbar>
                                                        </Row>
                                                    </Form.Group>
                                                </td>
                                            </tr>
                                        )}
                                    </tbody>
                                </table>

                                <div className="delimiter_H10"></div>

                                <div className="main_modal_center"> {movies.length} / {moviesCount}</div>

                                <div className="main_modal_center">

                                    <Button onClick={() => { this.addMoviesOnPage() }} className="main_modal_btn_more">
                                        More</Button>
                                </div>

                                <div className="delimiter_H10"></div>
                            </Form>

                        </Col>
                        <Col>

                        </Col>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={() => { this.props.onHide(); this.refreshPage(); }}>Close</Button>
                    </Modal.Footer>
                </Modal>

            </div >

        )
    }
}
