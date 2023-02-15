import React from "react";
import { Component } from "react";
import { Button, Form } from "react-bootstrap";
import "../../styles/Main.css";
import "../../styles/Form.css";
import axios from "axios";
import { Header } from "../../components/Header";

export class Login extends Component {
    constructor(props) {
        super(props);
        this.state = { responseResult: '' };
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    async handleSubmit(e) {
        e.preventDefault();
        let result = '';

        const emailNickname = e.target.emailNickname.value;
        const password = e.target.password.value;

        const formData = new FormData();
        formData.append("EmailNickname", emailNickname);
        formData.append("Password", password);

        await axios({
            url: process.env.REACT_APP_API_URL_Auth + "Login",
            method: "POST",
            data: formData
        }).then(response => {
            if (response.data.Success) {
                alert("Success! User page will develop soon.");
            }
            else {
                result = response.data.Message;
            }
        });

        this.setState({ responseResult: result });
    }

    render() {
        let { responseResult } = this.state;

        return (
            <div className="form_container">
                <Header />

                <Form onSubmit={this.handleSubmit}
                    className="form">
                    <Form.Group id="emailNickname">
                        <div className="form_text_start">Email or Nickname</div>
                        <Form.Control required name="emailNickname" type="text" placeholder="Email or Nickname"></Form.Control>
                    </Form.Group>

                    <div className="delimiter_H5"></div>

                    <Form.Group id="password">
                        <div className="form_text_start">Password</div>
                        <Form.Control required name="password" type="password" placeholder="Password"></Form.Control>
                    </Form.Group>

                    <div className="delimiter_H5"></div>

                    <div className="form_validation_result">{responseResult}</div>

                    <div className="delimiter_H5"></div>

                    <Button type="submit">Submit</Button>

                </Form>
            </div>
        )
    }
} 