import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog, { DialogProps } from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import { useEffect, useState } from 'react';
import Comment from '../../Types/CommentTypes/CommentType';
import { Avatar, Box, Divider, TextField, Typography } from '@mui/material';
import ICommentDialog from '../../Interfaces/DialogInterfaces/ICommentDialog';
import axios from 'axios';
import { Navigate } from 'react-router-dom';

export default function CommentsDialog(prop: ICommentDialog) {
    const [open, setOpen] = useState(true);
    const [scroll, setScroll] = useState<DialogProps['scroll']>('paper');
    const [comments, setComments] = useState<Comment[]>([]);
    const [commentContent, setCommentContent] = useState("");
    const [numberOfCommentsToGet, setNumberOfPhotosToGet] = useState(30);
    const [allCommentsFetched, setAllCommentsFetched] = useState(false);
    const [usernameClickValue, setUsernameClickValue] = useState("");

  const usernameClicked: React.MouseEventHandler<HTMLButtonElement> = (e) => {
    setUsernameClickValue(e.currentTarget.id);
  }

    const handleCommentContentChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setCommentContent(e.currentTarget.value);
    }

    const handleClose = () => {
        prop.closeDialog();
    };

    async function fetchComments(numberOfComments: number) {
        const result = await axios.get<Comment[]>(`https://localhost:7053/api/Photo/GetPhotoComments/${prop.photoId}/${numberOfComments}`, {
            headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
        });

        if (result.status === 200) {
            setComments(result.data);
            if (result.data.length < numberOfComments)
                setAllCommentsFetched(true);
        }
    }

    const loadMoreComments = async () => {
        await fetchComments(numberOfCommentsToGet + 30);
        setNumberOfPhotosToGet(numberOfCommentsToGet + 30);
    }

    const AddCommentClick = async () => {
        let userUsername = window.sessionStorage.getItem('username');
        if (userUsername != null) {
            let formData = new FormData();
            formData.append("commentContent", commentContent);
            formData.append("AuthorOfCommentUsername", userUsername);
            formData.append("PhotoId", prop.photoId);

            const result = await axios.post(`https://localhost:7053/api/Photo/AddComment`, formData, {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
            });

            if (result.status === 200) {
                fetchComments(numberOfCommentsToGet);
                setCommentContent("");
            }
        }
    };

    const descriptionElementRef = React.useRef<HTMLElement>(null);
    useEffect(() => {
        if (open) {
            const { current: descriptionElement } = descriptionElementRef;
            if (descriptionElement !== null) {
                descriptionElement.focus();
            }
        }
    }, [open]);

    useEffect(() => {
        fetchComments(numberOfCommentsToGet);

    }, []);

    useEffect(() => {
        setOpen(prop.openDialog);
    }, [])

    return (
        <>
            {usernameClickValue !== "" ? <Navigate to={usernameClickValue === sessionStorage.getItem('username') ? "/MyProfile" : "/UserProfile"}
        state={{ username: usernameClickValue }} /> : ""}
            <Dialog
                open={open}
                onClose={handleClose}
                scroll={scroll}
                aria-labelledby="scroll-dialog-title"
                aria-describedby="scroll-dialog-description"
            >
                <DialogTitle id="scroll-dialog-title">Comments</DialogTitle>
                <DialogContent dividers={scroll === 'paper'} sx={{ width: 500, display: 'flex', flexDirection: 'column' }}>
                    {comments.map((comment) => (
                        <Box key={comment.commentId}>

                            <Box sx={{ display: 'flex', flexDirection: "row" }}>
                                <Button id={comment.authorOfCommentUsername} sx={{ flexDirection: "column", textTransform: "none",}}
                                 onClick={usernameClicked} >
                                    <Typography color={'#BA1B2A'}>{comment.authorOfCommentUsername} </Typography>
                                    <Avatar sx={{ width: 43, height: 43 }} src={comment.authorOfCommentProfilePhoto !== null ?
                                        `data:image/jpeg;base64,${comment.authorOfCommentProfilePhoto}` : ""} />
                                </Button>
                                <Box sx={{ display: 'flex', justifyContent:"flex-start", alignItems:'center' }}>
                                    <Typography>{comment.commentContent}</Typography>
                                </Box>
                            </Box>
                            <Divider />
                        </Box>
                    ))}
                    {allCommentsFetched === true ? "" :
                        <Box sx={{ display: 'flex', alignSelf: 'center', mt: 3 }}>
                            <Button variant="contained"
                                component="label"
                                sx={{
                                    textAlign: 'center', background: '#BA1B2A', ':hover': {
                                        bgcolor: '#E65664',
                                        color: 'FFFFFF',
                                    },
                                }}
                                onClick={loadMoreComments}>
                                <Typography >Load more</Typography>
                            </Button>
                        </Box>}
                </DialogContent>
                <DialogActions sx={{ display: "flex", flexDirection: "column", justifyContent: "left", alignItems: 'flex-start' }}>
                    <Box >
                        <Box sx={{ display: 'flex', width: 525 }} >
                            <TextField
                                margin="normal"
                                required
                                fullWidth
                                id="newComment"
                                label="Add comment"
                                name="addComment"
                                autoComplete=""
                                autoFocus
                                multiline
                                rows={2}
                                onChange={handleCommentContentChange}
                            />
                        </Box>

                        <Box>
                            <Button variant="contained"
                                component="label"
                                sx={{
                                    textAlign: 'center', background: '#BA1B2A', ':hover': {
                                        bgcolor: '#E65664',
                                        color: 'FFFFFF',
                                    },
                                }}
                                onClick={AddCommentClick}>
                                <Typography >Add Comment</Typography>
                            </Button>
                        </Box>
                    </Box>
                </DialogActions>
                <DialogActions>
                    <Button variant="contained" sx={{
                        textAlign: 'center', background: '#BA1B2A', ':hover': {
                            bgcolor: '#E65664',
                            color: 'FFFFFF',

                        },
                    }} onClick={handleClose}>Close</Button>
                </DialogActions>
            </Dialog>
        </>
    );
}
