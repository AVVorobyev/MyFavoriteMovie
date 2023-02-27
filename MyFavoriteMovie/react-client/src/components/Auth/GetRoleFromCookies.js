import Cookies from "js-cookie";
import { Role } from "../../components/Auth/Roles";
import { decodeToken } from "react-jwt";

export default function getRoleFromCookies() {
    const token = Cookies.get("token");

    if (token == null)
        return Role.NotAuthorize;

    return decodeToken(token)["role"];
}