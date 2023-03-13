import axios from "axios";
import React, { Component } from "react";
import { ButtonToolbar, Image, Button } from "react-bootstrap";
import { Navigate, NavLink } from "react-router-dom";
import { EditActorModal } from "./EditActorModal";
import '../../../src/styles/Main.css';
import DateFormater from '../../components/DateFormater.js';
import { EditMoviesListModal } from "./EditMoviesListModal";
import { Header } from "../../components/Header/Header";
import Visible from "../../components/Auth/Visible";
import getRoleFromCookies from "../../components/Auth/GetRoleFromCookies";
import { Role } from "../../components/Auth/Roles";

const defaultAvatarImage = process.env.REACT_APP_Default_Images + "defaultAvatarImage.png";

export class Actor extends Component {
    constructor(props) {
        super(props);
        this.state = {
            actor: [],
            redirect: false, avatarImage: [],
            editActorShow: false,
            editMoviesListShow: false
        }
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
            if (response.data.Success === true) {
                this.setState({ actor: response.data.Result });

                if (response.data.Result.AvatarImage === null || response.data.Result.AvatarImage === undefined) {
                    this.setState({ avatarImage: defaultAvatarImage });
                }
                else {
                    this.setState({ avatarImage: response.data.Result.AvatarImage })
                }
            }
            else
                alert(response.Message);
        })
    }

    componentDidMount() {
        this.getActorById();
    }

    componentDidUpdate(prevProps) {
        if (this.props.actorId !== prevProps.actorId)
            this.getActorById();
    }

    deleteActor(actor) {
        if (window.confirm('Are you sure?')) {
            axios({
                method: "DELETE",
                url: process.env.REACT_APP_API_URL_Actor + 'Delete',
                params: {
                    actorId: actor.Id
                }
            }).then(response => {
                if (response.data.Success === true)
                    this.setRedirect();
                else
                    alert(response.data.Message);
            });
        }
    }

    render() {
        const { actor, avatarImage } = this.state;
        const role = getRoleFromCookies();

        return (
            <div>
                <Header />

                {this.renderRedirect()}
                <div className="main_container">
                    <div className="main_inline main_image_container">
                        <Image className="main_image" src={avatarImage}></Image>
                    </div>

                    <div className="main_inline main_delimiter_W50"></div>

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

                    <div className="main_inline main_container_list">
                        <div className="main_info_row main_info_row_about">In roles</div>

                        <table>
                            <tbody>
                                {actor?.ActorsInMovie?.map(movie =>
                                    <tr key={movie.Id}>
                                        <NavLink
                                            to={"/Movie/Movie/" + movie.Id}
                                            className="main_navlink">
                                            <div className="main_inline main_further_info_row_result_W200">
                                                {movie.Name}
                                            </div>
                                        </NavLink>
                                    </tr>
                                )}
                            </tbody>
                        </table>

                        <div className="delimiter_H10"></div>

                        <Visible
                            component={
                                <ButtonToolbar>

                                    <Button
                                        onClick={() => this.setState({ editMoviesListShow: true })}>
                                        Edit Movies List
                                    </Button>

                                    <EditMoviesListModal
                                        show={this.state.editMoviesListShow}
                                        onHide={() => this.setState({ editMoviesListShow: false })}
                                        actorid={actor.Id}>
                                    </EditMoviesListModal>
                                </ButtonToolbar>
                            }
                            isVisible={role === Role.Administrator || role === Role.Moderator}>
                        </Visible>
                    </div>
                </div>

                <div className="delimiter_H10"></div>

                <NavLink to="/Actor/Actors" className="btn btn-primary">To Actors</NavLink>

                <div className="delimiter_H10"></div>

                <Visible
                    component={
                        <ButtonToolbar>
                            <Button
                                onClick={() => { this.deleteActor(actor); }}
                                variant="danger">
                                Delete
                            </Button>

                            <div className="main_delimiter_W10"></div>

                            <Button onClick={() => { this.setState({ editActorShow: true, actor: actor }) }}
                                variant='info'>
                                Edit
                            </Button>

                            <EditActorModal show={this.state.editActorShow}
                                onHide={() => this.setState({ editActorShow: false })}
                                Id={actor.Id}
                                Name={actor.Name}
                                Surname={actor.Surname}
                                BirthDate={actor.BirthDate}
                                DeathDate={actor.DeathDate}
                                AvatarImage={avatarImage}>
                            </EditActorModal>
                        </ButtonToolbar>
                    }
                    isVisible={role === Role.Administrator || role === Role.Moderator}>
                </Visible>
            </div>
        )
    }
}