import axios from "axios";
import React, { Component } from "react";
import { Modal, Button, Col, Form } from 'react-bootstrap';

export class AddMovieModal extends Component {
    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();
        axios({
            method: 'POST',
            url: process.env.REACT_APP_API_URL_Movie + "Add",
            params: {
                Name: e.target.Name.value,
                Poster: e.target.Poster.value
            }
        }).then(response => {
            alert(response.data);
        }, (error) => {
            alert("error!");
        });
    }


    render() {

        return (
            <div className="container">
                <Modal
                    {...this.props}
                    size="lg"
                    aria-labelledby="contained-modal-title-vcenter"
                    centered>

                    <Modal.Header closeButton>
                        <Modal.Title>
                            Add New Movie
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Col sm={6}>
                            <Form onSubmit={this.handleSubmit}>
                                <Form.Group controlId="Name">
                                    <Form.Label>Movie Name</Form.Label>
                                    <Form.Control type="text" name="Name" requaried placeholder="Name"></Form.Control>
                                </Form.Group>
                                <Form.Group controlId="Poster">
                                    <Form.Label>Movie Poster</Form.Label>
                                    <Form.Control type="text" name="Poster" requaried placeholder="Poster"></Form.Control>
                                </Form.Group>
                                <Form.Group>
                                    <Button variant="primary" type="submit">
                                        Add
                                    </Button>
                                </Form.Group>
                            </Form>
                        </Col>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={this.props.onHide}>Close</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        )
    }
}
