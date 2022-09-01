import axios from "axios";
import React, { Component } from "react";
import { Button, Form, Modal, } from "react-bootstrap";

export class EditMovieModal extends Component {
    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();
        axios({
            method: 'PUT',
            url: process.env.REACT_APP_API_URL_Movie + "Update",
            params: {
                Id: e.target.Id.value,
                Name: e.target.Name.value,
                Title: e.target.Title.value
            }
        }).then(response => {
            alert(response.data);
            this.refreshPage();
        }, (error) => {
            alert("error!");
        });
    }

    refreshPage() {
        window.location.reload();
    }

    render() {
        return (
            <div className='conteiner'>
                <Modal
                    {...this.props}>
                    <Modal.Header closeButton>
                        <Modal.Title id="contained-modal-title-vcenter">
                            Edit Movie
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form onSubmit={this.handleSubmit}>
                            <Form.Group controlId="Id">
                                <Form.Label>Id</Form.Label>
                                <Form.Control type="text" name="Id" required
                                    disabled
                                    defaultValue={this.props.Id}
                                    placeholder="Id" />
                            </Form.Group>

                            <Form.Group controlId="Name">
                                <Form.Label>Name</Form.Label>
                                <Form.Control type="text" name="Name" required
                                    defaultValue={this.props.Name}
                                    placeholder="Name" />
                            </Form.Group>

                            <Form.Group controlId="Title">
                                <Form.Label>Title</Form.Label>
                                <Form.Control type="text" name="Title"
                                    defaultValue={this.props.Title}
                                    placeholder="Title" />
                            </Form.Group>
                            
                            <Form.Group>
                                <Button variant="primary" type="submit">
                                    Update
                                </Button>
                            </Form.Group>
                        </Form>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={this.props.onHide}>Close</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        )
    }
}