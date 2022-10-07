import axios from "axios";
import React, { Component } from "react";
import { Modal, Button, Col, Form, ButtonToolbar, Row } from 'react-bootstrap';
import '../../../src/MainModals.css';

export class EditActorsListModal extends Component {
    constructor(props) {
        super(props);
        this.state = { actors: [], skip: 0, take: 5, actorsCount: 0, itemsOnPage: 5, filterString: "" }
        this.addActorsOnPage = this.addActorsOnPage.bind(this);
    }

    getActorsList() {
        axios({
            url: process.env.REACT_APP_API_URL_Actor + 'Actors',
            method: "GET",
            params: {
                skip: this.state.skip,
                take: this.state.take
            }
        }).then(response => {
            this.setState({
                actors: response.data.List,
                actorsCount: response.data.Count
            });
        }, () => {
        });
    }

    getFilteredActorsList() {
        axios({
            url: process.env.REACT_APP_API_URL_Actor + 'filter_name_surname',
            method: "GET",
            params: {
                filter: this.state.filterString,
                skip: this.state.skip,
                take: this.state.take
            }
        }).then(response => {
            this.setState({
                actors: response.data.List,
                actorsCount: response.data.Count
            });

        }, () => {
        });
    }

    handleGetActorList() {
        if (this.state.filterString.length === 0)
            this.getActorsList();
        else
            this.getFilteredActorsList();
    }

    componentDidMount() {
        this.handleGetActorList();
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.actors === this.state.actors)
            this.handleGetActorList();
    }

    async addActorsOnPage() {
        if (this.state.take <= this.state.actorsCount) {
            await this.setState({ take: this.state.take + this.state.itemsOnPage });

            if (this.state.filterString.length === 0)
                this.getActorsList();
            else
                this.getFilteredActorsList();
        }
    }

    refreshPage() {
        window.location.reload();
    }

    editActorsListButton(actorsInMovieList, actorId) {
        let flag = false;

        for (let index = 0; index < actorsInMovieList.length; index++) {
            if (this.props.movieid === actorsInMovieList[index].Id) {
                flag = true;
                break;
            }
        }

        if (!flag)
            return <Button
                onClick={() => { this.handleAddActor(actorId) }}
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

    handleAddActor(actorId) {
        const formData = new FormData();
        formData.append("actorsId", actorId);

        axios({
            method: 'PATCH',
            url: process.env.REACT_APP_API_URL_Movie + "AddActor",
            params: {
                movieId: this.props.movieid,
            },
            data: formData
        }).then(() => {

            if (this.state.filterString.length > 0)
                this.getFilteredActorsList();
            else
                this.getActorsList();

        }, (error) => {
            alert("error!");
        });
    }

    handleDeleteActor(actorId) {
        const formData = new FormData();
        formData.append("actorsId", actorId);

        axios({
            method: 'PATCH',
            url: process.env.REACT_APP_API_URL_Movie + "DeleteActor",
            params: {
                movieId: this.props.movieid,
            },
            data: formData
        }).then(() => {
            if (this.state.filterString.length > 0)
                this.getFilteredActorsList();
            else
                this.getActorsList();
        }, (error) => {
            alert("error!");
        });
    }

    render() {
        let { actors, actorsCount } = this.state;

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
                            Edit Actors List
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Col >
                            <Form>
                                <table>
                                    <tbody>
                                        <Form.Group>
                                            <Form.Control
                                                onChange={async (e) => {
                                                    await this.setState({ filterString: e.target.value });
                                                    this.handleGetActorList();
                                                }}
                                                type="text"
                                                placeholder="Filter"
                                                name="Filter">
                                            </Form.Control>

                                            <div className="delimiter_H10"></div>

                                        </Form.Group>
                                        {actors.map(actor =>
                                            <tr key={actor.Id}>
                                                <td>
                                                    <Form.Group>
                                                        <Row>
                                                            <ButtonToolbar>
                                                                <div className="main_modal_info">{actor.Name + " " + actor.Surname}</div>
                                                                {this.editActorsListButton(actor.ActorsInMovie, actor.Id)}
                                                            </ButtonToolbar>
                                                        </Row>
                                                    </Form.Group>
                                                </td>
                                            </tr>
                                        )}
                                    </tbody>
                                </table>

                                <div className="delimiter_H10"></div>

                                <div className="main_modal_center"> {actors.length} / {actorsCount}</div>

                                <div className="main_modal_center">

                                    <Button onClick={() => { this.addActorsOnPage() }} className="main_modal_btn_more">
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