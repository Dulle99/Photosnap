import { Avatar, Box, Button, Typography } from "@mui/material";
import UserItemType from "../../../../../Types/UserTypes/UserItemType";
import { useState } from "react";
import { Navigate } from "react-router-dom";

function UserItem(prop: UserItemType) {
  const [userProfilePreview, setUserProfilePreview] = useState(false);

  const usernameClicked: React.MouseEventHandler<HTMLButtonElement> = () => {
    setUserProfilePreview(true);
  }

  return (
    <>
      {userProfilePreview ? <Navigate to={prop.username === sessionStorage.getItem('username') ? "/MyProfile" : "/UserProfile"}
        state={{ username: prop.username }} /> : ""}
      <Box>
        <Button sx={{ textTransform: "none" }} onClick={usernameClicked}>
          <Avatar src={`data:image/jpeg;base64,${prop.profilePhoto}`} />
          <Typography marginLeft={1} color={"#BA1B2A"}>{prop.username}</Typography>
        </Button>

      </Box>
    </>
  );
}

export default UserItem;