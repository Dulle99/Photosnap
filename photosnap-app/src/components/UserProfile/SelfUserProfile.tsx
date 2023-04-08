import { Box } from "@mui/material";
import { Container } from "@mui/system";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import UserBasicInformation from "./UserProfile_BuildingComponents/UserBasicInformation";

function SelfUserProfile(){
    let navigate = useNavigate();
    const [username, setUsername] = useState("");

    useEffect(()=>{
        let _username = sessionStorage.getItem("username");
        if(_username){
            setUsername(_username);  }
        else
            navigate(-1); 
    },[]);
    
    return (
    <>
    <Container sx={{ padding:5 }}>
        <UserBasicInformation username={username}/>
    </Container>
    
    </>
    );
}
 
export default SelfUserProfile;