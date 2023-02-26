import { Link } from "react-router-dom";
import HeaderButton from "./Header-Button";

function HeaderNonLoggedUserButtons() {
    return (
        <>
            <Link to='/Login' style={{ textDecoration: 'none' }} >
                <HeaderButton buttonName='Prijavi se' />
            </Link>


            <Link to='/Register' style={{ textDecoration: 'none' }}>
                <HeaderButton buttonName='Registruj se' />
            </Link>
        </>
    );
}

export default HeaderNonLoggedUserButtons;