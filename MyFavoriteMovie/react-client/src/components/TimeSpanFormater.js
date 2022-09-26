import { Component } from "react";

export default class TimeSpanFormater extends Component {
    render() {
        if (this.props.span == null)
            return "-";
        else {
            let moment = require('moment');
            let m = moment.duration(this.props.span).asMinutes();
            return m + " min.";
        }
    }
}
