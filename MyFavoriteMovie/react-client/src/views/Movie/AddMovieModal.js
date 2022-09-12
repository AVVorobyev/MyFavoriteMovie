import axios from "axios";
import React, { Component } from "react";
import { Modal, Button, Col, Form, Image } from 'react-bootstrap';
import TimePicker from 'react-time-picker';

const defaultPosterImage = '/Files/DefaultImages/defaultPosterImage.jpg';

export class AddMovieModal extends Component {
    constructor(props) {
        super(props);
        this.state = { posterPreview: defaultPosterImage }
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleFileSelected = this.handleFileSelected.bind(this);
        
    }

    handleDefaultPoster() {
        this.setState({ posterPreview: defaultPosterImage })
    }

    handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData();
        formData.append("Name", e.target.Name.value);
        formData.append("Title", e.target.Title.value);
        formData.append("PosterFile", this.posterFileToSent);
        formData.append("ReleaseDate", e.target.ReleaseDate.value);
        formData.append("Duration", this.duration);

        axios({
            method: 'POST',
            url: process.env.REACT_APP_API_URL_Movie + "Add",
            data: formData
        }).then(response => {
            alert(response.data);
        }, (error) => {
            alert("Error!");
        });
    }

    handleFileSelected = (e) => {
        e.preventDefault();
        this.posterFileToSent = e.target.files[0];
        this.setState({ posterPreview: URL.createObjectURL(e.target.files[0]) });
    };

    render() {
        const { posterPreview } = this.state;

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
                                    <Form.Label>Movie Name:</Form.Label>
                                    <Form.Control type="text" name="Name" requaried placeholder="Name"></Form.Control>
                                </Form.Group>

                                <Form.Group controlId="Title">
                                    <Form.Label>Movie Title:</Form.Label>
                                    <Form.Control type="text" name="Title" placeholder="Title"></Form.Control>
                                </Form.Group>

                                <Form.Group controlId="ReleaseDate">
                                    <Form.Label>Release Date:</Form.Label>
                                    <Form.Control type="date" name="ReleaseDate" placeholder="ReleaseDate"></Form.Control>
                                </Form.Group>

                                <Form.Group controlId="Duration">
                                    <Form.Label>Duration (in minutes):</Form.Label>
                                    <TimePicker
                                        onChange={(e) => {
                                            this.duration = e
                                        }}
                                        clearIcon={null}
                                        format="HH:mm" hourPlaceholder="hh"
                                        minutePlaceholder="mm" disableClock="true" name="Duration" placeholder="Duration"></TimePicker>
                                </Form.Group>

                                <Form.Group>
                                    <Image width="200px" height="200px" src={posterPreview} />
                                    <input type="file" accept="image/*" name="image-upload" id="input" onChange={this.handleFileSelected} />
                                    <Button variant="primary" type="submit">
                                        Add
                                    </Button>
                                </Form.Group>
                            </Form>
                        </Col>
                        <Col>

                        </Col>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={() => { this.props.onHide(); this.handleDefaultPoster(); }}>Close</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        )
    }
}
