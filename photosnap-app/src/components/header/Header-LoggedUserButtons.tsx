import { Avatar } from "@mui/material";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import HeaderButton from "./Header-Button";


function HeaderLoggedUserButtons(){
    const [profilePicture, setProfilePicture] = useState<String>();

    useEffect(() =>{
        if(sessionStorage.getItem("profilePicture") != undefined && sessionStorage.getItem("profilePicture") != null)
            setProfilePicture(`data:image/jpeg;base64,${sessionStorage.getItem('profilePicture')}`);

    }, []);
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

            <Avatar sx={{ width: 43, height: 43}} src={sessionStorage.getItem('profilePhoto') !== null ? 
                        `data:image/jpeg;base64,${sessionStorage.getItem('profilePhoto')}` : "" } />
        </>
      );
}
 
export default HeaderLoggedUserButtons;