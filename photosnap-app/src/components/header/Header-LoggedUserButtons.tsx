import { Link } from "react-router-dom";
import HeaderButton from "./Header-Button";


function HeaderLoggedUserButtons(){
    return (
        <>
            <Link to='/PostPhotoForm' style={{ textDecoration: 'none' }} >
                <HeaderButton buttonName='Post photo' />
            </Link>

            <Link to='/YourProfile' style={{ textDecoration: 'none' }}>
                <HeaderButton buttonName='Your profile' />
            </Link>

            <Link to='/Logout' style={{ textDecoration: 'none' }}>
                <HeaderButton buttonName='Logout' />
            </Link>
        </>
      );
}
 
export default HeaderLoggedUserButtons;