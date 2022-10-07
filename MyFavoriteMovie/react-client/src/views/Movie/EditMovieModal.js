import axios from "axios";
import React, { Component } from "react";
import { Button, Form, Modal, Image } from "react-bootstrap";
import TimePicker from 'react-time-picker';

const defaultPosterImage = process.env.REACT_APP_Default_Images + "defaultPosterImage.jpg";
let duration;

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
        formData.append("Description", e.target.Description.value);
        formData.append("PosterFile", this.posterFileToSent);
        formData.append("ReleaseDate", e.target.ReleaseDate.value);
        formData.append("Duration", duration);

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

    formatDate(string) {
        let moment = require('moment');
        return moment(string).format("YYYY-MM-DD");
    }

    render() {
        let { newPoster } = this.state;
        duration = this.props.Duration;

        return (
            <div className='conteiner'>
                <Modal
                    {...this.props}>
                    <Modal.Header >
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

                            <Form.Group controlId="Description">
                                <Form.Label>Description</Form.Label>
                                <Form.Control type="text" name="Description"
                                    defaultValue={this.props.Description}
                                    placeholder="Description" />
                            </Form.Group>

                            <Form.Group controlId="ReleaseDate">
                                <Form.Label>Release Date:</Form.Label>
                                <Form.Control type="date" defaultValue={this.formatDate(this.props.ReleaseDate)}
                                    name="ReleaseDate" placeholder="ReleaseDate"></Form.Control>
                            </Form.Group>

                            <Form.Group controlId="Duration">
                                <Form.Label>Duration</Form.Label>
                                <TimePicker
                                    value={this.props.Duration}
                                    onChange={(e) => {
                                        duration = e
                                    }}
                                    clearIcon={null}
                                    format="HH:mm" hourPlaceholder="hh"
                                    minutePlaceholder="mm" disableClock={true} name="Duration" placeholder="Duration"></TimePicker>
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
            </div >
        )
    }
}