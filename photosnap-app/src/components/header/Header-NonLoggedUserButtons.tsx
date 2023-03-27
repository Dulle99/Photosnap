import { Link } from "react-router-dom";
import HeaderButton from "./Header-Button";

function HeaderNonLoggedUserButtons() {
    return (
        <>
            <Link to='/Login' style={{ textDecoration: 'none' }} >
                <HeaderButton buttonName='Login' />
            </Link>


            <Link to='/Register' style={{ textDecoration: 'none' }}>
                <HeaderButton buttonName='Register' />
            </Link>
        </>
    );
}

export default HeaderNonLoggedUserButtons;