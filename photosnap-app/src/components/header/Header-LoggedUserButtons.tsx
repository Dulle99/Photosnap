import { Avatar } from "@mui/material";
import { useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import HeaderButton from "./Header-Button";


function HeaderLoggedUserButtons(){
    const [profilePicture, setProfilePicture] = useState<String>();

    useEffect(() =>{
        if(sessionStorage.getItem("profilePicture") != undefined && sessionStorage.getItem("profilePicture") != null)
            setProfilePicture(`data:image/jpeg;base64,${sessionStorage.getItem('profilePicture')}`);

    }, []);
    return (
        <>
            <Link to='/PostPhoto' style={{ textDecoration: 'none' }} >
                <HeaderButton buttonName='Post photo' />
            </Link>

            <Link to='/MyProfile' style={{ textDecoration: 'none' }}>
                <HeaderButton buttonName='My profile' />
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