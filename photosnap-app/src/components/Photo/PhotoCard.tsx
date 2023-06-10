import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardMedia from '@mui/material/CardMedia';
import CardContent from '@mui/material/CardContent';
import CardActions from '@mui/material/CardActions';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import CommentIcon from '@mui/icons-material/Comment';
import Avatar from '@mui/material/Avatar';
import IconButton, { IconButtonProps } from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';

import PhotoDisplay from '../../Interfaces/Photo/IPhotoDisplay';
import { Box, Button, Chip, Menu, MenuItem } from '@mui/material';
import { useEffect, useState } from 'react';
import { Link, Navigate, Outlet, useNavigate, useNavigation } from 'react-router-dom';
import PhotoFormProps from '../../Interfaces/Photo/IPhotoForm';
import YesNoDialog from '../Dialogs/YesNoDialog';
import axios from 'axios';
import CommentsDialog from '../Dialogs/CommentsDialog';



export default function PhotoCard(prop: PhotoDisplay) {
    const nav = useNavigate();
    const [deleteButtonClicked, setDeleteButtonClicked] = useState(false);
    const [loggedUsername, setLoggedUsername] = useState("");
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [numberOfPhotoLikes, setNumberOfPhotosToGet] = useState(prop.numberOfLikes);
    const [showCommentsFlag, setShowCommentsFlag] = useState(false);
    const [userProfilePreview, setUserProfilePreview] = useState(false);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    function closeDialog(){
        setShowCommentsFlag(false);
    }
    async function deletePhoto(flag: Boolean) {
        if (flag === true) {
            console.log("delete");
            const result = await axios.delete(`https://localhost:7053/api/Photo/DeletePhoto/${prop.photoId}`, {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
            });

            if (result.status === 200)
                prop.photoDeleted(prop.photoId);
        }
        setDeleteButtonClicked(false);
    }

    const likeButtonClicked: React.MouseEventHandler<HTMLButtonElement> = async () => {
        let username = sessionStorage.getItem('username'); 
        if (username !== null) {
            const result = await axios.put<number>(`https://localhost:7053/api/Photo/LikePhotoButton/${username}/${prop.photoId}`,undefined, {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
            });

            if (result.status === 200)
                setNumberOfPhotosToGet(result.data);
        }
    }

    const usernameClicked: React.MouseEventHandler<HTMLButtonElement> = () => {
        setUserProfilePreview(true);
    }
    const commentsButtonClicked: React.MouseEventHandler<HTMLButtonElement> = () => {
        let userLogged = window.sessionStorage.getItem('token') !== null ? true : false;
        if(userLogged)
            setShowCommentsFlag(true);
    }

    const handleClose: React.MouseEventHandler<HTMLLIElement> = (e) => {
        e.preventDefault();
        if (e.currentTarget.id === "Edit") {
            nav(`/EditPhoto/${prop.photoId}`);
        }
        else if (e.currentTarget.id === "Delete") {
            setDeleteButtonClicked(true);
        }

        setAnchorEl(null);
    };

    useEffect(() => {
        let username = window.sessionStorage.getItem('username');
        if (username != null)
            setLoggedUsername(username);
    }, [])

    const options = [
        'Edit',
        'Delete',]

    return (
        <>
            {userProfilePreview ? <Navigate to={prop.authorUsername === sessionStorage.getItem('username') ? "/MyProfile" : "/UserProfile"}
                                                   state={{ username: prop.authorUsername }} /> : ""}
            {deleteButtonClicked === true ? <YesNoDialog deletePhoto={deletePhoto} dialogText='Are you sure you want to delete the photo?' /> : ""}
            {showCommentsFlag === true? <CommentsDialog photoId={prop.photoId} openDialog={showCommentsFlag} closeDialog={closeDialog} /> : ""}
            <Card  >
                <CardHeader
                    avatar={
                        <Button onClick={usernameClicked}>
                            <Avatar src={`data:image/jpeg;base64,${prop.authorProfilePhoto}`}  />
                        </Button>
                        
                    }

                    action={
                        {
                            ...loggedUsername === prop.authorUsername ?
                                <>
                                    <IconButton
                                        aria-label="more"
                                        id="long-button"
                                        aria-controls={open ? 'long-menu' : undefined}
                                        aria-expanded={open ? 'true' : undefined}
                                        aria-haspopup="true"
                                        onClick={handleClick}
                                    >
                                        :
                                    </IconButton>
                                    <Menu
                                        id="long-menu"
                                        MenuListProps={{
                                            'aria-labelledby': 'long-button',
                                        }}
                                        anchorEl={anchorEl}
                                        open={open}
                                        onClose={handleClose}
                                        PaperProps={{
                                            style: {
                                                maxHeight: 48 * 4.5,
                                                width: '20ch',
                                            },
                                        }}
                                    >
                                        {options.map((option) => (
                                            <MenuItem key={option} id={option} selected={option === 'Pyxis'} onClick={handleClose}>
                                                {option}
                                            </MenuItem>
                                        ))}
                                    </Menu>
                                </> : <></>
                        }
                    }
                    title={prop.authorUsername }
                    subheader={prop.numberOfFollowers}
                />
                <CardMedia
                    component="img"
                    height="194"
                    image={`data:image/jpeg;base64,${prop.photo}`}
                />
                <CardContent>
                    <Typography variant="body2" color="text.secondary">
                        {prop.description}
                    </Typography>
                    <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "space-between", mt: 2 }}>
                        <Box>
                            <IconButton onClick={likeButtonClicked} aria-label="like">
                                <ThumbUpIcon />
                                <Typography>{numberOfPhotoLikes}</Typography>
                            </IconButton>
                            <IconButton onClick={commentsButtonClicked} aria-label="comments">
                                <CommentIcon />
                                <Typography>{prop.numberOfComments}</Typography>
                            </IconButton>
                        </Box>
                        <Chip label={prop.categoryName} color='primary' style={{ backgroundColor: prop.categoryColor }} />
                    </Box>
                </CardContent>
            </Card>
        </>
    );
}