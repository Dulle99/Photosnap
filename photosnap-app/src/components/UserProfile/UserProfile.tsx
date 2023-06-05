import { Container } from "@mui/system";
import IUsername from "../../Interfaces/UserProfile/IUsername";
import UserBasicInformation from "./UserProfile_BuildingComponents/UserBasicInformation";
import { useLocation } from "react-router-dom";
import UserPhotosList from "../Photo/PhotoDisplay/UserPhotosList";

function UserProfile(){
    const location = useLocation().state as IUsername;
    return ( 
    <>
        <Container sx={{ padding: 5, display: "flex", flexDirection: "column" }}>
                <UserBasicInformation username={location.username} />
                <Container>
                    <UserPhotosList username={location.username} displayUserPhotos={true} />
                </Container>
        </Container>
    </> );
}
 
export default UserProfile;