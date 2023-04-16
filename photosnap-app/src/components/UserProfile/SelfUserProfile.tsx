import { Box } from "@mui/material";
import { Container } from "@mui/system";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import UserBasicInformation from "./UserProfile_BuildingComponents/UserBasicInformation";
import UserPhotosList from "../Photo/PhotoDisplay/UserPhotosList";

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
    <Container sx={{ padding:5, display:"flex", flexDirection:"column" }}>
        <UserBasicInformation username={username}/>
        <Container>
            <UserPhotosList username={username} />
        </Container>
    </Container>
    
    </>
    );
}
 
export default SelfUserProfile;