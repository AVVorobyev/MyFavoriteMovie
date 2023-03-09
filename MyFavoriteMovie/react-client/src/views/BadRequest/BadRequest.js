import React, { Component } from "react";
import { Header } from "../../components/Header/Header";
import "../../styles/Errors.css";

export class BadRequest extends Component {

    render() {

        return (
            <div>
                <Header />

                <div className="error_conteiner">
                    <div className='error_header'>
                        Opps!..
                    </div>
                    <div className="error_message">
                        There was something wrong!
                        Please try again...
                    </div>
                </div>
            </div>
        )
    }
}