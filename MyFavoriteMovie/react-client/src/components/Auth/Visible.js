export default function Visible({component, isVisible}){
    return isVisible ? component : null;
}