import axios from "axios";
import React, { Component } from "react";
import { Button, Table, ButtonToolbar } from "react-bootstrap";
import { NavLink } from "react-router-dom";
import { AddActorModal } from "./AddActorModal";

export class Actors extends Component {
    constructor(props) {
        super(props);
        this.state = { actors: [], addModalShow: false }
    }

    
    componentDidMount() {
        this.getActorsList();
    }

    getActorsList() {
        axios.get(process.env.REACT_APP_API_URL_Actor + 'Actors')
            .then(response =>
                this.setState({ actors: response.data }));
    }

    componentDidUpdate(prevProps, prevState){
        if(prevState.actors === this.state.actors){
            this.getActorsList();
        }
    }

    render() {
        const { actors } = this.state;
        let addModalClose = () => this.setState({ addModalShow: false });

        return (
            <div>
                <h1>Actors Page</h1>

                <Table>
                    <tbody>
                        {actors.map(actor =>
                            <tr key={actor.Id}>
                                <NavLink
                                    to={"/Actor/Actor/" + actor.Id}
                                    className="btn">
                                    <td>{actor.Name}</td>
                                    <td>{actor.Surname}</td>
                                    <td>{actor.BirthDate}</td>
                                </NavLink>
                            </tr>
                        )}
                    </tbody>
                    <ButtonToolbar>
                        <Button
                            onClick={() => this.setState({ addModalShow: true })}>
                            Add Actor</Button>
                        <AddActorModal
                            show={this.state.addModalShow}
                            onHide={addModalClose}>

                        </AddActorModal>
                    </ButtonToolbar>
                </Table>
            </div>
        )
    }
}