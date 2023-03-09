import axios from "axios";
import React, { Component } from "react";
import DateFormater from "../../components/DateFormater";
import { Header } from "../../components/Header/Header";
import '../../styles/Main.css';

export class User extends Component {
    constructor(props) {
        super(props);
        this.state = { user: '' };
    }

    async getUserData() {
        await axios({
            url: process.env.REACT_APP_API_URL_Auth + "Profile",
            method: "GET"
        }
        ).then(response => {
            if (response.data.Success) {
                this.setState({ user: response.data.Result });
            }
        })
    }

    componentDidMount() {
        this.getUserData();
    }

    render() {
        let { user } = this.state;
        
        return (
            <div>
                <Header />
                <div className="main_container">

                    <div className="main_info_row">
                        <div className="main_inline main_info_row_property_name">Your Nickname</div>
                        <div className="main_inline main_info_row_result">{user.Nickname}</div>
                    </div>

                    <div className="main_info_row">
                        <div className="main_inline main_info_row_property_name">Surname</div>
                        <div className="main_inline main_info_row_result">{user.Name}</div>
                    </div>

                    <div className="main_info_row">
                        <div className="main_inline main_info_row_property_name">Email</div>
                        <div className="main_inline main_info_row_result">{user.Surname}</div>
                    </div>
                    
                    <div className="main_info_row">
                        <div className="main_inline main_info_row_property_name">Date of Registration</div>
                        <div className="main_inline main_info_row_result">{<DateFormater date={user.RegistrationDate} />}</div>
                    </div>

                    <div className="main_info_row">
                        <div className="main_inline main_info_row_property_name">Role</div>
                        <div className="main_inline main_info_row_result">{user.Role}</div>
                    </div>

                </div>

            </div>
        )
    }
}