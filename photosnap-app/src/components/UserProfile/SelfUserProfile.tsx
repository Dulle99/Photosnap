import { Box, ToggleButton, ToggleButtonGroup } from "@mui/material";
import { Container } from "@mui/system";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import UserBasicInformation from "./UserProfile_BuildingComponents/UserBasicInformation";
import UserPhotosList from "../Photo/PhotoDisplay/UserPhotosList";

function SelfUserProfile() {
    let navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [alignment, setAlignment] = useState('myPosts');
    const [displayUserPostsFlag, setDisplayUsetPostsFlag] = useState(true);

    const handleTogleChange = (
      event: React.MouseEvent<HTMLElement>,
      newAlignment: string,
    ) => {
        if(newAlignment === "myPosts")
            setDisplayUsetPostsFlag(true);
        else
            setDisplayUsetPostsFlag(false);
      setAlignment(newAlignment);
    };

    useEffect(() => {
        let _username = sessionStorage.getItem("username");
        if (_username) {
            setUsername(_username);
        }
        else
            navigate(-1);
    }, []);

    useEffect(()=>{

    },[displayUserPostsFlag]);


    return (
        <>
            <Container sx={{ padding: 5, display: "flex", flexDirection: "column" }}>
                <UserBasicInformation username={username} />
                <Box >
                    <ToggleButtonGroup
                        color="primary"
                        value={alignment}
                        exclusive
                        onChange={handleTogleChange}
                        aria-label="Platform"
                        sx={{display:'flex', justifyContent:'space-around', alignItems:'center'}}
                    >
                        <ToggleButton value="myPosts">My posts</ToggleButton>
                        <ToggleButton value="likedPosts">Likes
                    </ToggleButton>
                        
                    </ToggleButtonGroup>
                </Box>
                <Container>
                    <UserPhotosList username={username} displayUserPhotos={displayUserPostsFlag} />
                </Container>
            </Container>

        </>
    );
}

export default SelfUserProfile;