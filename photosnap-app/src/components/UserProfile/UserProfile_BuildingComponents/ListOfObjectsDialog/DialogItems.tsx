import { Box, Button, Divider, Grid, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import DialogItemsType from "../../../../Interfaces/UserProfile/DialogProps/DialogItemsEnum";
import UserItemType from "../../../../Types/UserTypes/UserItemType";
import UserItem from "./Items/UserItem";
import CategoryItemType from "../../../../Types/CategoryType/CategoryItemType";
import CategoryItem from "./Items/CategoryItem";
import DialogItemsProp from "../../../../Interfaces/UserProfile/DialogProps/IDialogItems";
import DeleteIcon from '@mui/icons-material/Delete';
import axios from "axios";
import SelectPhotoCategory from "../../../PhotoCategory/SelectPhotoCategory";



function DialogItems(props: DialogItemsProp) {
    const [users, setUsers] = useState<UserItemType[]>([]);
    const [categories, setCategories] = useState<CategoryItemType[]>([]);
    const [numberOfItemsToGet, setNumberOfItemsToGet] = useState(5);
    const [allItemsPulledFlag, setAllItemsPulledFlag] = useState(false);
    const [userWatchSelfProfileFlag, setUserWatchSelfProfileFlag] = useState(false);
    const [selectedPhotoCategories, setSelectedPhotoCategories] = useState<string[]>([]);

    const handleLoadMoreButton: React.MouseEventHandler<HTMLButtonElement> = (e) => {
        loadData();
    }

    const handleUnfollowUserButton: React.MouseEventHandler<HTMLButtonElement> = (e) => {
        e.preventDefault();
        axios.put(`https://localhost:7053/api/User/Unfollow/${props.username}/${e.currentTarget.id}`, undefined, {
            headers: {
                'Authorization': 'Bearer ' + window.sessionStorage.getItem("token")
            },
        });
        setUsers(users.filter(u => u.username != e.currentTarget.id));
    }

    const handleRemovePhotoInterestButton: React.MouseEventHandler<HTMLButtonElement> = (e) => {
        e.preventDefault();
        axios.put(`https://localhost:7053/api/User/RemoveCategoryOfInterest/${props.username}/${e.currentTarget.id}`, undefined, {
            headers: {
                'Authorization': 'Bearer ' + window.sessionStorage.getItem("token")
            },
        });
        setCategories(categories.filter(c => c.categoryName != e.currentTarget.id));
        setSelectedPhotoCategories(selectedPhotoCategories.filter(c => c != e.currentTarget.id));
    }

    function GetFetchUrl(): string {
        if (props.itemsType === DialogItemsType.listOfFollowings) {
            return `https://localhost:7053/api/User/GetUserListOfFollwings/${props.username}/`;
        }
        else if (props.itemsType === DialogItemsType.listOfFollowers) {
            return `https://localhost:7053/api/User/GetUserListOfFollwers/${props.username}/`;
        }
        else {
            return `https://localhost:7053/api/User/GetUserListOfPhotoInterests/${props.username}/`;
        }
    }

    const loadData = async () => {
        if (props.itemsType === DialogItemsType.listOfFollowings) {
            const result = await fetchItems<UserItemType>();
            setUsers(result);
        }
        else if (props.itemsType === DialogItemsType.listOfFollowers) {
            const result = await fetchItems<UserItemType>();
            setUsers(result);
        }
        else {
            const result = await fetchItems<CategoryItemType>();
            setCategories(result);
        }

        if (props.totalDialogItems < numberOfItemsToGet)
            setAllItemsPulledFlag(true); 
        else
            setNumberOfItemsToGet(numberOfItemsToGet + 5);
    }

    const fetchItems = async <T,>(): Promise<T[]> => {
        const result = await axios.get<T[]>(GetFetchUrl() + numberOfItemsToGet.toString(), {
            headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
        });
        return result.data;
    }

    useEffect(() => {
        if (sessionStorage.getItem("username") === props.username) {
            setUserWatchSelfProfileFlag(true);
        }
    }, []);

    useEffect(() => {
        loadData();
    }, []);

    useEffect(()=>{
        var params = new URLSearchParams();
        selectedPhotoCategories.forEach(category => {
            params.append('categoryNames',category);
        })
        axios.put(`https://localhost:7053/api/User/AddCategoryOfInterest/${props.username}`, undefined, {
            headers: {
                'Authorization': 'Bearer ' + window.sessionStorage.getItem("token")
            },
            params: params
        });
        loadData();
    }, [selectedPhotoCategories]);

    if (props.itemsType === DialogItemsType.listOfFollowers) {
        return (
            <>
                <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start" }}>
                    {users.map((el, ind) => (
                        <UserItem username={el.username} profilePhoto={el.profilePhoto} key={ind} />
                    ))}
                    {allItemsPulledFlag ? "" :
                        <Button onClick={handleLoadMoreButton} sx={{ background: '#E65664', marginTop: 5, textTransform: 'none' }}>
                            <Typography color='#FFFFFF'>Load more</Typography>
                        </Button>}
                </Box>
            </>
        );
    }
    else if (props.itemsType === DialogItemsType.listOfFollowings) {
        return (
            <>
                <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start" }}>
                    {users.map((el, ind) => (
                        <Grid key={"grid" + ind} sx={{ display: "flex", flexDirection: "row", justifyContent: "space-between", marginTop: 1 }} >
                            <UserItem username={el.username} profilePhoto={el.profilePhoto} key={"user" + ind} />
                            {userWatchSelfProfileFlag === true ?
                                <Button onClick={handleUnfollowUserButton} size="small" key={el.username} id={el.username} sx={{ background: '#E65664', marginLeft: 2, textTransform: 'none' }}>
                                    <Typography variant="body2" color={'#FFFFFF'}>Unfollow</Typography>
                                </Button> : ""}
                        </Grid>
                    ))}
                    {allItemsPulledFlag ? "" :
                        <Button onClick={handleLoadMoreButton} sx={{ background: '#E65664', marginTop: 5, textTransform: 'none' }}>
                            <Typography color='#FFFFFF'>Load more</Typography>
                        </Button>}
                </Box>

            </>
        );
    }
    else {
        return (
            <>
                <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start" }}>
                    {categories.map((el, ind) => (
                        <Grid key={"g" + ind} sx={{ marginTop: 1, display: "flex", flexDirection: "row", justifyContent: "space-between" }} >
                            <CategoryItem categoryName={el.categoryName} categoryColor={el.categoryColor} key={"user" + ind} />
                            {userWatchSelfProfileFlag ?
                                <Button onClick={handleRemovePhotoInterestButton} key={el.categoryName} id={el.categoryName} sx={{ background: '#FFFFFF', marginLeft: 2 }}>
                                    <DeleteIcon sx={{ color: '#E65664', }} />
                                </Button> : ""}
                        </Grid>
                    ))}

                    {allItemsPulledFlag ? "" :
                        <Button onClick={handleLoadMoreButton} sx={{ background: '#E65664', marginTop: 5, textTransform: 'none' }}>
                            <Typography color='#FFFFFF'>Load more</Typography>
                        </Button>}
                        <Divider />
                    <SelectPhotoCategory setSelectedPhotoCategories={setSelectedPhotoCategories} selectedPhotoCategories={[]} />
                </Box>
            </>
        );
    }

}

export default DialogItems;