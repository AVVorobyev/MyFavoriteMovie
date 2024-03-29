import axios from "axios";
import React, { Component } from "react";
import { Button, ButtonToolbar } from "react-bootstrap";
import { NavLink } from "react-router-dom";
import { AddActorModal } from "./AddActorModal";
import '../../../src/styles/MainLists.css';
import DateFormater from '../../components/DateFormater.js';
import { Header } from "../../components/Header/Header";
import Visible from "../../components/Auth/Visible";
import getRoleFromCookies from "../../components/Auth/GetRoleFromCookies";
import { Role } from "../../components/Auth/Roles";

export class Actors extends Component {
    constructor(props) {
        super(props);
        this.state = { actors: [], actorsCount: 0, addModalShow: false, skip: 0, take: 3 }
    }

    componentDidMount() {
        this.getActorsList();
    }

    getActorsList() {
        axios({
            url: process.env.REACT_APP_API_URL_Actor + 'Actors',
            method: "GET",
            params: {
                skip: this.state.skip,
                take: this.state.take
            }
        }).then(response => {
            if (response.data.Success === true) {
                this.setState({
                    actors: response.data.Result.List,
                    actorsCount: response.data.Result.Count
                });
            }
            else
                alert(response.Message);
        });
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.actors === this.state.actors) {
            this.getActorsList();
        }
    }

    handleNextPageChanged() {
        if (this.state.skip + this.state.take >= this.state.actorsCount)
            return;
        else {
            this.setState({ skip: this.state.skip + this.state.take });
        }
    }

    handlePreviousPageChanged() {
        if (this.state.skip <= 0)
            return;
        else {
            this.setState({ skip: this.state.skip - this.state.take });
            this.getActorsList();
        }
    }

    render() {
        let { actors } = this.state;
        let addModalClose = () => this.setState({ addModalShow: false });
        const role = getRoleFromCookies();

        return (
            <div>
                <Header />

                <h1>Actors</h1>

                <div className="main_container">
                    <table>
                        <tbody>
                            {actors?.map(actor =>
                                <tr key={actor.Id}
                                >
                                    <NavLink
                                        to={"/Actor/Actor/" + actor.Id}
                                        className="btn main_lists_navlink_btn">
                                        <td className="main_lists_title">{actor.Name}</td>
                                        <td className="main_lists_title">{actor.Surname}</td>
                                        <td className="main_lists_info">Height: {actor.Height}</td>
                                        <td className="main_lists_info">BirthDate: <DateFormater date={actor.BirthDate}></DateFormater></td>
                                        <td className="main_lists_info">DeathDate: <DateFormater date={actor.DeathDate}></DateFormater></td>
                                    </NavLink>
                                    <div className="delimiter_H10"></div>
                                </tr>
                            )}
                        </tbody>

                    </table>
                </div>

                <div className="delimiter_H10"></div>

                <ButtonToolbar>
                    <Button className="danger" onClick={() => { this.handlePreviousPageChanged(); }}>Previous</Button>

                    <div className="delimiter_W10"></div>

                    <Button className="danger" onClick={() => { this.handleNextPageChanged(); }}>Next</Button>
                </ButtonToolbar>

                <div className="delimiter_H10"></div>

                <Visible
                    component={
                        <ButtonToolbar>
                            <Button
                                onClick={() => this.setState({ addModalShow: true })}>
                                Add Actor
                            </Button>

                            <AddActorModal
                                show={this.state.addModalShow}
                                onHide={addModalClose}>
                            </AddActorModal>
                        </ButtonToolbar>
                    }
                    isVisible={role === Role.Administrator || role === Role.Moderator}>
                </Visible>
            </div>
        )
    }
}