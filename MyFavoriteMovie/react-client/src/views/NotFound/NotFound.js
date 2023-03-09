import React, { Component } from 'react';
import "../../styles/Errors.css";
import { Header } from "../../components/Header/Header";

export class NotFound extends Component {
    render() {
        return (            
            <div className='error_conteiner'>
                <Header /> 
                
                <div className='error_header'>
                    Opps!..
                </div>
                <div className='error_message'>
                    404
                </div>
                <div className='error_message'>
                    The resource you requested was not found!
                </div>
            </div>
        )
    }
}