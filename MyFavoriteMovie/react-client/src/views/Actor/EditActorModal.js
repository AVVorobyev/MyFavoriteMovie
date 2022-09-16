import axios from "axios";
import React, { Component } from "react";
import { Form, Modal, Image, Button } from "react-bootstrap";

const defaultAvatarImage = process.env.REACT_APP_Default_Images + "defaultAvatarImage.png";

export class EditActorModal extends Component {
    constructor(props) {
        super(props);
        this.state = { newAvatar: defaultAvatarImage }
        this.handleFileSelected = this.handleFileSelected.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleFileSelected(e) {
        e.preventDefault();

        this.avatarImageToSent = e.target.files[0];
        this.setState({ newAvatar: URL.createObjectURL(e.target.files[0]) });
    }

    handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData();
        formData.append("Id", e.target.Id.value);
        formData.append("Name", e.target.Name.value);
        formData.append("Surname", e.target.Surname.value);
        formData.append("BirthDate", e.target.BirthDate.value);
        formData.append("DeathDate", e.target.DeathDate.value);
        formData.append("AvatarImage", this.avatarImageToSent);

        axios({
            url: process.env.REACT_APP_API_URL_Actor + "Update",
            method: "PUT",
            data: formData
        }).then(response => {
            alert(response.data);
            this.handleRefreshPage();
        }, (error) => {
            alert("error!");
        });
    }

    handleRefreshPage() {
        window.location.reload();
    }

    handleFormatDate(str) {
        let moment = require('moment');
        return moment(str).format("YYYY-MM-DD");
    }

    render() {
        let { newAvatar } = this.state;

        return (
            <div>
                <Modal {...this.props}>
                    <Modal.Header >
                        <Modal.Title>
                            Edit Actor
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form onSubmit={this.handleSubmit}>

                            <Form.Group controlId="Id">
                                <Form.Label>Id</Form.Label>
                                <Form.Control name="Id" disabled type="text" placeholder="Id"
                                    defaultValue={this.props.Id}></Form.Control>
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Name</Form.Label>
                                <Form.Control name="Name" required type="text" placeholder="Name"
                                    defaultValue={this.props.Name}></Form.Control>
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Surname</Form.Label>
                                <Form.Control name="Surname" type="text" required placeholder="Surname"
                                    defaultValue={this.props.Surname}></Form.Control>
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Birth Date</Form.Label>
                                <Form.Control name="BirthDate" type="date" placeholder="BirthDate"
                                    defaultValue={this.handleFormatDate(this.props.BirthDate)}></Form.Control>
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Death Date</Form.Label>
                                <Form.Control name="DeathDate" type="date" placeholder="DeathDate"
                                    defaultValue={this.handleFormatDate(this.props.DeathDate)}></Form.Control>
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Old Avatar</Form.Label>
                                <Image src={this.props.AvatarImage} height={200} width={100} />
                            </Form.Group>
                            <Form.Group>
                                <Form.Label>New Avatar</Form.Label>
                                <Image height={200} width={100} src={newAvatar}></Image>
                                <input type="file" accept="image/*" name="image-upload" id="input" onChange={this.handleFileSelected}></input>
                            </Form.Group>

                            <Button variant="primary" type="submit">Update</Button>
                        </Form>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button onClick={() => { this.props.onHide(); this.setState({ newAvatar: defaultAvatarImage }) }}>Close</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        )
    }
}

