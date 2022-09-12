import axios from "axios";
import React, { Component } from "react";
import { NavLink, Table } from "react-bootstrap";

export class Actors extends Component {
    constructor(props) {
        super(props);
        this.state = { actors: [] }
    }

    componentDidMount() {
        this.getActorsList();
    }

    getActorsList() {
        console.log(process.env.REACT_APP_API_URL_Actor + 'Actors');
        axios.get(process.env.REACT_APP_API_URL_Actor + 'Actors')
            .then(response =>
                this.setState({ actors: response.data }));
    }

    render() {
        const { actors } = this.state;
        console.log(actors);

        return (
            <div>
                <h1>Actors Page</h1>
                <Table>
                    <tbody>
                        {actors.map(actor =>
                            <tr key={actor.Id}>
                                <NavLink 
                                    className="btn">
                                    <td>{actor.Name}</td>
                                    <td>{actor.Surname}</td>
                                    <td>{actor.BirthDate}</td>
                                </NavLink>
                            </tr>)}
                    </tbody>
                </Table>
            </div>
        )
    }
}