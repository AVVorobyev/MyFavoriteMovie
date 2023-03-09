import axios from "axios";
import Cookies from "js-cookie";
import React, { Component } from "react";
import { Button, Form, FormGroup, Image } from "react-bootstrap";
import { decodeToken } from "react-jwt";
import { NavLink } from "react-router-dom";
import '../../styles/Header.css';
import Visible from "../Auth/Visible";
import DateFormater from "../../components/DateFormater";

const defaultAvatarImage = process.env.REACT_APP_Default_Images + "defaultAvatarImage.png";
const defaultPosterImage = process.env.REACT_APP_Default_Images + "defaultPosterImage.jpg";

export class Header extends Component {
    constructor(props) {
        super(props);
        this.state = {
            filterString: "",
            nickname: undefined,
            movies: [],
            actors: [],
        }
        this.getFilterResult = this.getFilterResult.bind(this);
        this.getNickname = this.getNickname.bind(this);
    }

    componentDidMount() {
        this.getNickname();
    }

    getFilterResult(filter) {
        axios({
            url: process.env.REACT_APP_API_URL_Movie + "filter_name",
            method: "GET",
            params: {
                filter: filter,
                skip: 0,
                take: 3
            }
        }).then(response => {
            if (response.data.Success)
                this.setState({
                    movies: response.data.Result.List
                });
            else
                alert(response.data.Message);
        })

        axios({
            url: process.env.REACT_APP_API_URL_Actor + "filter_name_surname",
            method: "GET",
            params: {
                filter: filter,
                skip: 0,
                take: 3
            }
        }).then(response => {
            if (response.data.Success)
                this.setState({
                    actors: response.data.Result.List
                });
            else
                alert(response.data.Message);
        })
    }

    getNickname() {
        const token = Cookies.get("token");
        if (token == null)
            return undefined;

        this.setState({ nickname: decodeToken(token)["nickname"] });
    }

    render() {
        let { nickname, actors, movies } = this.state;

        return (
            <div className="header_container">

                <Form className="header_inline header_filter_form">
                    <Form.Group >
                        <Form.Control
                            className="header_filter_control"
                            onChange={(e) => { this.getFilterResult(e.target.value); }}
                            name="filter"
                            type="text"
                            placeholder="Filter">
                        </Form.Control>
                    </Form.Group>

                </Form>

                <div className="header_delimiter_W5 header_inline"></div>

                <div className="header_inline">
                    <Visible
                        component={<NavLink to={"/Auth/User "} className="btn btn-primary header_user_button">Hi, {nickname}</NavLink>}
                        isVisible={nickname !== undefined}
                        className="header_inline">
                    </Visible>

                    <Visible
                        component={<NavLink to={"/Auth/Login"} className="btn btn-primary">Login</NavLink>}
                        isVisible={nickname === undefined}
                        className="header_inline">
                    </Visible>
                </div>

                <div className="header_lists_background">

                    <Visible
                        component={<div className="header_center">Movies</div>}
                        isVisible={movies != null && movies.length > 0}>
                    </Visible>

                    <table>
                        <tbody>
                            {movies?.map(movie =>
                                <tr key={movie.Id}
                                    className="header_lists_tr">
                                    <NavLink
                                        className="header_lists_navlink_btn"
                                        to={"/Movie/Movie/" + movie.Id}>
                                        <td>
                                            <Visible
                                                component={<Image className="header_image" src={movie.Poster}></Image>}
                                                isVisible={movie.Poster != null} />
                                            <Visible
                                                component={<Image className="header_image" src={defaultPosterImage}></Image>}
                                                isVisible={movie.Poster == null}
                                            />
                                        </td>
                                        <td><div className="header_info">{movie.Name}</div></td>
                                        <td>{<DateFormater date={movie.ReleaseDate} />}</td>
                                    </NavLink>
                                </tr>
                            )}
                        </tbody>
                    </table>

                    <Visible
                        component={<div className="header_center">Actors</div>}
                        isVisible={actors != null && actors.length > 0}>
                    </Visible>

                    <table>
                        <tbody>
                            {actors?.map(actor =>
                                <tr key={actor.Id}
                                    className="header_lists_tr">
                                    <NavLink
                                        className="header_lists_navlink_btn"
                                        to={"/Actor/Actor/" + actor.Id}>
                                        <td>
                                            <Visible
                                                component={<Image className="header_image" src={actor.AvatarImage}></Image>}
                                                isVisible={actor.AvatarImage != null} />
                                            <Visible
                                                component={<Image className="header_image" src={defaultAvatarImage}></Image>}
                                                isVisible={actor.AvatarImage == null}
                                            />
                                        </td>
                                        <td><div className="header_info">{actor.Name} {actor.Surname}</div></td>
                                        <td>{<DateFormater date={actor.BirthDate} />}</td>
                                    </NavLink>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
            </div>
        )
    }
}