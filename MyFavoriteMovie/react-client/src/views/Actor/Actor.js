import axios from "axios";
import React, { Component } from "react";
import { ButtonToolbar, Image, Button } from "react-bootstrap";
import Moment from "react-moment";
import { Navigate, NavLink } from "react-router-dom";
import { EditActorModal } from "./EditActorModal";

const defaultAvatarImage = process.env.REACT_APP_Default_Images + "defaultAvatarImage.png";

export class Actor extends Component {
    constructor(props) {
        super(props);
        this.state = { actor: [], redirect: false, avatarImage: [], editActorShow: false }
    }

    setRedirect() {
        this.setState({ redirect: true });
    }

    renderRedirect() {
        if (this.state.redirect)
            return <Navigate to="/Actor/Actors" />
    }

    getActorById() {
        axios({
            url: process.env.REACT_APP_API_URL_Actor + "Actor",
            method: "GET",
            params: {
                id: this.props.actorId
            }

        }).then(response => {
            this.setState({ actor: response.data });

            if (response.data.AvatarImage === null || response.data.AvatarImage === undefined) {
                this.setState({ avatarImage: defaultAvatarImage });
            }
            else {
                this.setState({ avatarImage: response.data.AvatarImage })
            }
        })
    }

    componentDidMount() {
        this.getActorById();
    }

    deleteActor(actor) {
        const formData = new FormData();
        formData.append("Id", actor.Id)

        if (this.state.avatarImage !== defaultAvatarImage) {
            let fileName = this.state.avatarImage.split('/').pop();
            formData.append("AvatarImage", fileName);
        }

        if (window.confirm('Are you sure?')) {
            axios({
                method: "DELETE",
                url: process.env.REACT_APP_API_URL_Actor + 'Delete',
                data: formData
            }).then(response => {
                alert(response.data);
                this.setRedirect();
            }, (error) => {
                alert("Error!");
            });
        }
    }

    handleDateFormat(str) {
        if (str == null)
            return "-";
        else
            return <Moment format="DD.MM.YYYY">{str}</Moment>
    }

    render() {
        const { actor, avatarImage } = this.state;

        return (
            <div>
                {this.renderRedirect()}
                <h1>Actor Page
                    <h2>ActorId = {actor.Id}</h2>
                </h1>
                <Image src={avatarImage} height={200} width={100}></Image>
                <h4>
                    <h5 className="text-success">{actor.Name}</h5>
                    <h5 className="text-success">{actor.Surname}</h5>
                    <h5 className="text-success">{actor.Height}</h5>
                    <h5 className="text-success">Born: {this.handleDateFormat(actor.BirthDate)}</h5>
                    <h5 className="text-success">Died: {this.handleDateFormat(actor.DeathDate)}</h5>
                </h4>
                <ButtonToolbar>
                    <NavLink to="/Actor/Actors" className="btn btn-primary">Back</NavLink>
                    <Button onClick={() => { this.setState({ editActorShow: true, actor: actor }) }}
                        variant='info'>
                        Edit
                    </Button>
                    <Button
                        onClick={() => { this.deleteActor(actor); }}
                        variant="danger">Delete</Button>

                    <EditActorModal show={this.state.editActorShow}
                        onHide={() => this.setState({ editActorShow: false })}
                        Id={actor.Id}
                        Name={actor.Name}
                        Surname={actor.Surname}
                        BirthDate={actor.BirthDate}
                        DeathDate={actor.DeathDate}
                        AvatarImage={avatarImage}
                    ></EditActorModal>
                </ButtonToolbar>
            </div>
        )
    }
}