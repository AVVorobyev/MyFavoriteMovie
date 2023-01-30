import React, { Component } from "react";
import "../../styles/Errors.css";

export class TechnicalWork extends Component {

    render() {

        return (
            <div className='error_conteiner'>
                <div className='error_header'>
                    Technical works!
                </div>
                <div className='error_message'>
                    We'll get it fixed soon!
                </div>
            </div>
        )
    }
}