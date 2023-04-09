import { Avatar, Box, Typography } from "@mui/material";
import { Container } from "@mui/system";
import { useEffect, useState } from "react";
import IListOfUsersDialog from "../../../Interfaces/UserProfile/DialogProps/IListOfUsersDialog-prop";
import IUsername from "../../../Interfaces/UserProfile/IUsername";
import { UserInformation } from "../../../Types/UserTypes/UserInformation";
import ListOfCategories from "./ListOfCategories";
import ListOfFollowersDialog from "./ListOfFollowersDialog";
import ListOfFollowingsDialog from "./ListOfFollowingsDialog";
import axios from "axios";

function UserBasicInformation(username: IUsername) {
    const [userInformation, setUserInformation] = useState<UserInformation>();
    const [renderConfigs, setRenderConfigs] = useState<IListOfUsersDialog[]>([]);


    useEffect(() => {
        const fetchUserInformation = async () => {
            const url = `https://localhost:7053/api/User/GetUserProfilePreview/${username.username}`;
            const result = await axios.get<UserInformation>(url , {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
            });

            if (result.status === 200) {
                if (result.data) {
                    console.log(result.data);
                    setUserInformation(result.data);
                }
            }
            else { }
            //else
            //something went wrong page
        }


        if(username.username !== "")
            fetchUserInformation();
    }, [username]);

    if (userInformation === undefined) {
        return <div>Loading...</div>;
    }
    else {

        return (<>
            <Box sx={{ display: "flex", flexDirection: "row" }}>
                <Box>
                    <Avatar   sx={{ width: 150, height: 150, }} src={userInformation.profilePhoto ? `data:image/jpeg;base64,${userInformation.profilePhoto}` : ""} />
                </Box>
                <Box sx={{ marginLeft: 2 }}>
                    <Typography variant="h5"> <strong> {userInformation!.username} </strong> </Typography>
                    <Box sx={{ display: "flex", flexDirection: "row" }} >
                        <Box sx={{ margin: 1 }}>
                            <ListOfFollowersDialog username={userInformation!.username} numberOfUsers={userInformation!.numberOfFollowers} />
                        </Box>
                        <Box sx={{ margin: 1 }}>
                            <ListOfFollowingsDialog username={userInformation!.username} numberOfUsers={userInformation!.numberOfFollowings} />
                        </Box>
                        <Box sx={{ margin: 1 }}>
                            <ListOfCategories username={userInformation!.username} numberOfCategories={userInformation!.numberOfCategoriesOfInterst} />
                        </Box>
                    </Box>
                    <Typography>{userInformation!.name + " " + userInformation!.lastname}</Typography>
                    <Typography>{userInformation!.biography}</Typography>
                </Box>
            </Box>

        </>);
    }
}

export default UserBasicInformation;