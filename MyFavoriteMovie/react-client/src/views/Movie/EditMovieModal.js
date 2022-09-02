import axios from "axios";
import React, { Component } from "react";
import { Button, Form, Modal, Image } from "react-bootstrap";

const defaultPosterImage = '/Files/DefaultImages/defaultPosterImage.jpg';

export class EditMovieModal extends Component {
    constructor(props) {
        super(props);
        this.state = { newPoster: defaultPosterImage };
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleFileSelected = this.handleFileSelected.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData();
        formData.append("Id", e.target.Id.value);
        formData.append("Name", e.target.Name.value);
        formData.append("Title", e.target.Title.value);
        formData.append("PosterFile", this.posterFileToSent);

        axios({
            method: 'PUT',
            url: process.env.REACT_APP_API_URL_Movie + "Update",
            data: formData
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

    handleFileSelected(e) {
        e.preventDefault();
        this.posterFileToSent = e.target.files[0];
        this.setState({ newPoster: URL.createObjectURL(e.target.files[0]) });
    }

    render() {
        let { newPoster } = this.state;

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

                            <Form.Label>Old Poster</Form.Label>
                            <Form.Group>
                                <Image width="100px" height="100px" src={this.props.Poster} />
                            </Form.Group>

                            <Form.Group>
                                <Image width={100} height={200} src={newPoster} />
                                <input type="file" accept="image/*" name="image-upload" id="input" onChange={this.handleFileSelected} />

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