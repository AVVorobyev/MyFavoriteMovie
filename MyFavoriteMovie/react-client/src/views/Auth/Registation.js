import React, { Component } from "react";
import { Button, Form } from "react-bootstrap";
import "../../styles/Main.css";
import "../../styles/Form.css";
import axios from "axios";
import validator from "validator";
import { Header } from "../../components/Header";

export class Registration extends Component {
    constructor(prop) {
        super(prop);
        this.state = { responseResult: '', emailValidationResultMessage: '', nicknameValidationResultMessage: '', passwordValidationResultMessage: '' };
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    async handleSubmit(e) {
        e.preventDefault();
        let result = '';

        const email = e.target.email.value;
        const nickname = e.target.nickname.value;
        const password = e.target.password.value;
        const confirm = e.target.confirm.value;

        if (this.state.emailValidationResultMessage !== '' ||
            this.state.nicknameValidationResultMessage !== '' ||
            this.state.passwordValidationResultMessage !== '')
            return;

        if (password !== confirm)
            this.setState({ passwordValidationResultMessage: "• Passwords do not match." });
        else {
            const formData = new FormData();
            formData.append("Nickname", nickname);
            formData.append("Email", email);
            formData.append("Password", password);

            await axios({
                url: process.env.REACT_APP_API_URL_Auth + "Registration",
                method: "POST",
                data: formData
            }).then(response => {
            }, error => {
                result = error.response.data.Result;
            });
        }

        this.setState({ responseResult: result });
    }

    emailValidation(email) {
        let result = '';

        if (!validator.isEmail(email))
            result = "• The email is in the wrong format.";

        this.setState({ emailValidationResultMessage: result });
    }

    async nicknameValidation(nickname) {
        let result = '';

        if (nickname.length < 4)
            result = "• Nickname must be longer than 3 characters."
        else {
            await axios({
                url: process.env.REACT_APP_API_URL_Auth + "Nickname",
                method: "GET",
                params: {
                    "nickname": nickname
                }
            }).then(response => {
                if (response.data.Success) {
                    if (response.data.Result === false)
                        result = "• The nickname is not unique.";
                    else if (response.data.Result === true)
                        result = '';
                }
                else {
                    result = response.data.Message;
                }
            });
        }

        this.setState({ nicknameValidationResultMessage: result });
    }

    passwordValidation(password) {
        let result = '';

        if (password.length < 8)
            result = "• Password must be longer than 7 characters.";

        this.setState({ passwordValidationResultMessage: result });
    }

    render() {
        let { responseResult,
            emailValidationResultMessage,
            nicknameValidationResultMessage,
            passwordValidationResultMessage } = this.state;

        return (
            <div className="form_container">
                <Header />

                <Form onSubmit={this.handleSubmit}
                    className="form">
                    <Form.Group id="email">
                        <div className="form_text_start">Email</div>
                        <Form.Control onChange={(e) => { this.emailValidation(e.target.value); }}
                            required name="email"
                            type="text"
                            placeholder="Email"></Form.Control>
                        <div className="form_validation_result">{emailValidationResultMessage}</div>
                    </Form.Group>

                    <div className="delimiter_H5"></div>

                    <Form.Group id="nickname">
                        <div className="form_text_start">Nickname</div>
                        <Form.Control onChange={(e) => { this.nicknameValidation(e.target.value); }}
                            required name="nickname"
                            type="text"
                            placeholder="Nickname"></Form.Control>
                        <div className="form_validation_result">{nicknameValidationResultMessage}</div>
                    </Form.Group>

                    <div className="delimiter_H5"></div>

                    <Form.Group id="password">
                        <div className="form_text_start">Password</div>
                        <Form.Control onChange={(e) => { this.passwordValidation(e.target.value); }}
                            required
                            name="password"
                            type="password" placeholder="Password"></Form.Control>
                    </Form.Group>

                    <div className="delimiter_H5"></div>

                    <Form.Group id="confirm">
                        <div className="form_text_start">Confirm</div>
                        <Form.Control required
                            name="confirm"
                            type="password"
                            placeholder="Confirm"></Form.Control>
                        <div className="form_validation_result">{passwordValidationResultMessage}</div>
                    </Form.Group>

                    <div className="delimiter_H5"></div>

                    <div className="form_validation_result">{responseResult}</div>

                    <div className="delimiter_H5"></div>

                    <Button type="submit">Submit</Button>

                </Form>
            </div >
        )
    }
}