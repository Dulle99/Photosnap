import { Box, Button, Grid, Typography } from "@mui/material";
import { display } from "@mui/system";
import { useEffect, useState } from "react";
import DialogItemsType from "../../../../Interfaces/UserProfile/DialogProps/DialogItemsEnum";
import UserItemType from "../../../../Types/UserTypes/UserItemType";
import UserItem from "./Items/UserItem";
import CategoryItemType from "../../../../Types/CategoryType/CategoryItemType";
import CategoryItem from "./Items/CategoryItem";
import DialogItemsProp from "../../../../Interfaces/UserProfile/DialogProps/IDialogItems";



function DialogItems(props: DialogItemsProp) {
    const [users, setUsers] = useState<UserItemType[]>([]);
    const [categories, setCategories] = useState<CategoryItemType[]>([]);
    const [userWatchSelfProfileFlag, setUserWatchSelfProfileFlag] = useState(false);

    useEffect(() => {
        console.log(props.username);
            console.log(sessionStorage.getItem("username"));
        if (sessionStorage.getItem("username") === props.username){
            console.log(props.username);
            console.log(sessionStorage.getItem("username"));
            setUserWatchSelfProfileFlag(true);
        }
    }, []);

    useEffect(() => {
        let fetchString = "";
        if (props.itemsType === DialogItemsType.listOfFollowers) {
            let f1: UserItemType[] = [{username:"Ivan", profilePhoto: ""}, {username:"Aleks", profilePhoto: ""}]; 
            setUsers(f1);
        }
        else if (props.itemsType === DialogItemsType.listOfFollowings) {
            let f2: UserItemType[] = [{username:"Petar", profilePhoto: ""}, {username:"Nenad", profilePhoto: ""}]; 
            setUsers(f2);
        }
        else if (props.itemsType === DialogItemsType.listOfCategories) {
            let c: CategoryItemType[] = [{categoryName:"Nature", categoryColor:"#00C039"}, {categoryName:"Archiceture", categoryColor:"#897682"}];
            setCategories(c);
        }
    }, []);

    if (props.itemsType === DialogItemsType.listOfFollowers) {
        return (
            <>
                <Box sx={{ display: "flex", flexDirection: "column", justifyContent:"flex-start" }}>
                    {users.map((el, ind) => (
                        <UserItem username={el.username} profilePhoto={el.profilePhoto} key={ind} />
                    ))}
                </Box>
            </>
        );
    }
    else if (props.itemsType === DialogItemsType.listOfFollowings) {
        return (
            <>
                <Box sx={{ display: "flex", flexDirection: "column", justifyContent:"flex-start"  }}>
                    {users.map((el, ind) => (
                        <Grid key={"grid"+ind} sx={{display:"flex",flexDirection:"row", justifyContent:"space-between"}} >
                            <UserItem username={el.username} profilePhoto={el.profilePhoto} key={"user"+ind} />
                            {userWatchSelfProfileFlag===true ? <Button key={ind}> <Typography>Remove</Typography> </Button> : "" }
                        </Grid>
                    ))}

                </Box>
            </>
        );
    }
    else {
        return (
            <>
                <Box sx={{ display: "flex", flexDirection: "column", justifyContent:"flex-start"  }}>
                    {categories.map((el, ind) => (
                        <Grid key={"g"+ind} sx={{marginTop:1, display:"flex",flexDirection:"row", justifyContent:"space-between"}} >
                            <CategoryItem categoryName={el.categoryName} categoryColor={el.categoryColor} key={"user"+ind}  />
                            {userWatchSelfProfileFlag ?<Button key={ind}> <Typography>Remove</Typography> </Button> : "" }
                        </Grid>
                    ))}

                </Box>
            </>
        );
    }

}

export default DialogItems;