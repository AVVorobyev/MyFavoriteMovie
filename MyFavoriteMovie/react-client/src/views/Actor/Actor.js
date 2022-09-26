import axios from "axios";
import React, { Component } from "react";
import { ButtonToolbar, Image, Button } from "react-bootstrap";
import { Navigate, NavLink } from "react-router-dom";
import { EditActorModal } from "./EditActorModal";
import '../../../src/Main.css';
import DateFormater from '../../components/DateFormater.js';

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

    render() {
        const { actor, avatarImage } = this.state;

        return (
            <div>
                {this.renderRedirect()}
                <div className="main_container">
                    <div className="main_inline main_image_container">
                        <Image className="main_image" src={avatarImage}></Image>
                    </div>

                    <div className="main_inline main_delimiter_W100"></div>

                    <div className="main_inline main_info_container ">
                        <div className="main_inline main_info_row main_title">{actor.Name} {actor.Surname}</div>

                        <div className="main_info_row main_info_row_about">About</div>

                        <div className="main_info_row">
                            <div className="main_inline main_info_row_property_name">Height</div>
                            <div className="main_inline main_info_row_result">{actor.Height}</div>
                        </div>

                        <div className="main_info_row">
                            <div className="main_inline main_info_row_property_name">Born</div>
                            <div className="main_inline main_info_row_result"><DateFormater date={actor.BirthDate}></DateFormater></div>
                        </div>

                        <div className="main_info_row">
                            <div className="main_inline main_info_row_property_name">Died</div>
                            <div className="main_inline main_info_row_result"><DateFormater date={actor.DeathDate}></DateFormater></div>
                        </div>
                    </div>
                </div>

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