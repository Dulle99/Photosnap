import { Avatar, Box, Typography } from "@mui/material";
import { Container } from "@mui/system";
import { useEffect, useState } from "react";
import IListOfUsersDialog from "../../../Interfaces/UserProfile/DialogProps/IListOfUsersDialog-prop";
import IUsername from "../../../Interfaces/UserProfile/IUsername";
import { UserInformation } from "../../../Types/UserTypes/UserInformation";
import ListOfCategories from "./ListOfCategories";
import ListOfFollowersDialog from "./ListOfFollowersDialog";
import ListOfFollowingsDialog from "./ListOfFollowingsDialog";

function UserBasicInformation(username: IUsername) {
    const [userInformation, setUserInformation] = useState<UserInformation>();
    const [renderConfigs, setRenderConfigs]= useState<IListOfUsersDialog[]>([]);

    useEffect(() => {
        let us: UserInformation = {
            username: "Petar99", name: "Pera", lastname: "Peric", numberOfCategoriesOfInterst: 3, numberOfFollowers: 55, numberOfFollowings: 36,
            biography: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        };
        setUserInformation(us);

        if (us.numberOfFollowers !== undefined && us.numberOfFollowings !== undefined, us.numberOfCategoriesOfInterst !== undefined) {
            
            let followers: IListOfUsersDialog = {username: us.username,  numberOfUser: us.numberOfFollowers};
            let following: IListOfUsersDialog = {username: us.username,  numberOfUser: us.numberOfFollowings};
            let list : IListOfUsersDialog[] = [];
            list.push(followers);
            list.push(following);
            setRenderConfigs(list);
        }
    }, []);

    return (<>
        <Box sx={{ display: "flex", flexDirection: "row" }}>
            <Box>
                <Avatar sx={{ width: 150, height: 150 }} />
            </Box>
            <Box sx={{ marginLeft: 2 }}>
                <Typography variant="h5"> <strong> {userInformation?.username} </strong> </Typography>
                <Box sx={{ display: "flex", flexDirection: "row" }} >
                        <Box sx={{margin: 1}}>
                            <ListOfFollowersDialog username={username.username}  numberOfUser={45} />
                        </Box>
                        <Box sx={{margin: 1}}>
                            <ListOfFollowingsDialog username={username.username}  numberOfUser={56}  />
                        </Box>
                        <Box sx={{margin: 1}}>
                            <ListOfCategories username={username.username}  numberOfCategories={6}  />
                        </Box>
                </Box>
                <Typography>{userInformation?.name + " " + userInformation?.lastname}</Typography>
                <Typography>{userInformation?.biography}</Typography>
            </Box>
        </Box>

    </>);
}

export default UserBasicInformation;