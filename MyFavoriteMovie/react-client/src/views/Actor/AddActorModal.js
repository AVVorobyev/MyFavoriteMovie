
import axios from "axios";
import React, { Component } from "react";
import { Button, Col, Form, Modal, Image } from "react-bootstrap";

const defaultAvatarImage = process.env.REACT_APP_Default_Images + "defaultAvatarImage.png";
let avatarImageToSent;

export class AddActorModal extends Component {
    constructor(props) {
        super(props);
        this.state = { avatarPreview: defaultAvatarImage }
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleFileSelected = this.handleFileSelected.bind(this);
    }

    handleDefaultAvatar() {
        this.setState({ avatarPreview: defaultAvatarImage });
    }

    handleFileSelected(e) {
        e.preventDefault();

        avatarImageToSent = e.target.files[0];
        this.setState({ avatarPreview: URL.createObjectURL(e.target.files[0]) });
    }

    handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData();
        formData.append("Name", e.target.Name.value);
        formData.append("Surname", e.target.Surname.value);
        formData.append("Height", e.target.Height.value);
        formData.append("BirthDate", e.target.BirthDate.value);
        formData.append("DeathDate", e.target.DeathDate.value);
        formData.append("AvatarImage", avatarImageToSent);

        axios({
            url: process.env.REACT_APP_API_URL_Actor + "Add",
            method: "POST",
            data: formData
        }).then(response => {
            if (response.data.Success === false)
                alert(response.data.Message);
            else
                alert(response.data.Message);
        });
    }

    render() {
        let { avatarPreview } = this.state;

        return (
            <div>
                <Modal
                    {...this.props}
                    size="lg"
                    aria-labelledby="contained-modal-title-vcenter"
                    centered>
                    <Modal.Header closeButton>
                        <Modal.Title>Add New Actor</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Col sm={6}>
                            <Form onSubmit={this.handleSubmit}>
                                <Form.Group id="Name">
                                    <Form.Label>Name</Form.Label>
                                    <Form.Control name="Name" type="text" requaried placeholder="Name"></Form.Control>
                                </Form.Group>

                                <Form.Group id="Surname">
                                    <Form.Label>Surname</Form.Label>
                                    <Form.Control name="Surname" type="text" requaried placeholder="Surname"></Form.Control>
                                </Form.Group>

                                <Form.Group id="Height">
                                    <Form.Label>Height</Form.Label>
                                    <Form.Control name="Height" type="number" placeholder="Height"></Form.Control>
                                </Form.Group>

                                <Form.Group id="BirthDate">
                                    <Form.Label>Birth Date</Form.Label>
                                    <Form.Control name="BirthDate" type="date" placeholder="BirthDate"></Form.Control>
                                </Form.Group>

                                <Form.Group id="DeathDate">
                                    <Form.Label>Death Date</Form.Label>
                                    <Form.Control name="DeathDate" type="date" placeholder="DeathDate"></Form.Control>
                                </Form.Group>

                                <Form.Group>
                                    <Image src={avatarPreview} width="200px" height="200px"></Image>
                                    <input type="file" accept="image/*" name="image-upload" id="input" onChange={this.handleFileSelected}></input>
                                    <Button variant="primary" type="submit">
                                        Add
                                    </Button>
                                </Form.Group>
                            </Form>
                        </Col>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={() => { this.props.onHide(); this.handleDefaultAvatar(); }}>Close</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        )
    }
}

