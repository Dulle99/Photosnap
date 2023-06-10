import { Avatar, Box, Button, Typography } from "@mui/material";
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
    const [selfProfilePreview, setSelfProfilePreview] = useState(true);
    const [userIsFollowed, setUserIsFollowed] = useState(false);

    const followButtonClicked: React.MouseEventHandler<HTMLButtonElement> = async () => {
        let loggedUserUsername = window.sessionStorage.getItem('username');
        if (loggedUserUsername != null) {
            let urlType = "";
            if (userIsFollowed)
                urlType = `https://localhost:7053/api/User/Unfollow/${loggedUserUsername}/${username.username}`;
            else
                urlType = `https://localhost:7053/api/User/Follow/${loggedUserUsername}/${username.username}`;

            const result = await axios.put(urlType,undefined, {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
            });

            if (result.status === 200)
                setUserIsFollowed(!userIsFollowed);
        }
    }

    const checkIfUserIsFollowed = async () => {
        let loggedUserUsername = window.sessionStorage.getItem('username');
        if (loggedUserUsername != null) {
            const result = await axios.get<boolean>(`https://localhost:7053/api/User/IsUserFollowed/${loggedUserUsername}/${username.username}`,
                {
                    headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
                });
            if (result.data === true)
                setUserIsFollowed(true);
            else
                setUserIsFollowed(false);
        }
    }

    useEffect(() => {
        const fetchUserInformation = async () => {
            const url = `https://localhost:7053/api/User/GetUserProfilePreview/${username.username}`;
            const result = await axios.get<UserInformation>(url, {
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


        if (username.username !== "") {
            fetchUserInformation();
            let loggedUserUsername = window.sessionStorage.getItem('username');
            if (loggedUserUsername != null) {
                if (loggedUserUsername === username.username)
                    setSelfProfilePreview(true);
                else {
                    setSelfProfilePreview(false);
                    checkIfUserIsFollowed();
                }
            }
        }
    }, [username]);

    if (userInformation === undefined) {
        return <div>Loading...</div>;
    }
    else {

        return (<>
            <Box sx={{ display: "flex", flexDirection: "row" }}>
                <Box>
                    <Avatar sx={{ width: 150, height: 150, }} src={userInformation.profilePhoto ? `data:image/jpeg;base64,${userInformation.profilePhoto}` : ""} />
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

                    {selfProfilePreview === true ? "" :
                        <Box>
                            <Button onClick={followButtonClicked} variant="contained" sx={{
                                mt: 3, mb: 2, textAlign: 'center', background: '#BA1B2A', ':hover': {
                                    bgcolor: '#E65664',
                                    color: 'FFFFFF',
                                },

                            }}>{userIsFollowed ? "Following" : "Follow"}</Button>
                        </Box>}
                </Box>
            </Box>

        </>);
    }
}

export default UserBasicInformation;