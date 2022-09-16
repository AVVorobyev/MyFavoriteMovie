import React from "react";
import Moment from "react-moment";

export function formatDate(string) {
    if (string == null)
        return "-";
    else
        return <Moment format="DD.MM.YYYY">{string}</Moment>
}

// export function formatDate2(string) {
//     if (string == null)
//         return "-";
//     else
//         return <Moment format="DD.MM.YYYY">{string}</Moment>
// }