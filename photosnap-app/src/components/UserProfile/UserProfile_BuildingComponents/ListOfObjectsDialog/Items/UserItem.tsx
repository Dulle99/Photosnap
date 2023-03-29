import { Avatar, Box, Button, Typography } from "@mui/material";
import UserItemType from "../../../../../Types/UserTypes/UserItemType";

function UserItem(prop: UserItemType) {
  //TODO: Add on click handler to open clicked user profile
  return (
    <Box>
      <Button sx={{textTransform:"none"}}>
        <Avatar src={`data:image/jpeg;base64,${prop.profilePhoto}`} />
        <Typography marginLeft={1} color={"#BA1B2A"}>{prop.username}</Typography>
      </Button>

    </Box>
  );
}

export default UserItem;