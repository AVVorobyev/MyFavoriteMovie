import React, { Component } from 'react';
import "../../styles/NotFound.css";

export class NotFound extends Component {
    render() {
        return (
            <div className='notfound_conteiner'>
                <div className='notfound_header'>
                    Opps!..
                </div>
                <div className='notfound_message'>
                    404
                </div>
                <div className='notfound_message'>
                    The resource you requested was not found!
                </div>
            </div>
        )
    }
}