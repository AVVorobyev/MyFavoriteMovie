import React, { Component } from "react";
import Moment from "react-moment";

export default class DateFormater extends Component {
    render() {
        if (this.props.date == null)
            return "-";
        else
            return <Moment format="DD.MM.YYYY">{this.props.date}</Moment>
    }
}