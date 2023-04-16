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
import { Menu, MenuItem } from '@mui/material';
import { useEffect, useState } from 'react';



export default function PhotoCard(prop: PhotoDisplay) {
    const [loggedUsername, setLoggedUsername] = useState("");
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose:React.MouseEventHandler<HTMLLIElement> = (e) => {
        e.preventDefault();
        setAnchorEl(null);
    };

    useEffect(() => {
        let username = window.sessionStorage.getItem('username');
        if(username != null)
            setLoggedUsername(username);
    },[])

    const options = [
        'Edit',
        'Delete',]

    return (
        <Card  >
            <CardHeader
                avatar={
                    <Avatar src={`data:image/jpeg;base64,${prop.authorProfilePhoto}`} />
                }
                
                action={
                {...loggedUsername === prop.authorUsername ?   
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
                    </> : <></>}
                }
                title={prop.authorUsername}
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
                <IconButton aria-label="like">
                    <ThumbUpIcon />
                    <Typography>{prop.numberOfLikes}</Typography>
                </IconButton>
                <IconButton aria-label="comments">
                    <CommentIcon />
                    <Typography>{prop.numberOfComments}</Typography>
                </IconButton>

            </CardContent>
        </Card>
    );
}