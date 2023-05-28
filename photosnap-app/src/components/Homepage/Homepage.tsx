import { Box, Button, Container, Grid, ImageList, ImageListItem, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import PhotoDisplay from "../../Interfaces/Photo/IPhotoDisplay";
import axios from "axios";
import PhotoCard from "../Photo/PhotoCard";

function Homepage() {
    const [photos, setPhotos] = useState<PhotoDisplay[]>([]);
    const [numberOfPhotosToGet, setNumberOfPhotosToGet] = useState(30);

    async function fetchPhotos(numberOfPhotosToGet: number) {

        let loggedUserUsername = sessionStorage.getItem('username');
        if (loggedUserUsername !== null && loggedUserUsername !== undefined) {

            const result = await axios.get<PhotoDisplay[]>(`https://localhost:7053/api/User/GetPhotosOfFollowingUsers/${loggedUserUsername}/${numberOfPhotosToGet}`, {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
            });
            setPhotos(result.data);
        }
    }

    const loadMoreButtonClick: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault();
        fetchPhotos(numberOfPhotosToGet + 30);
        setNumberOfPhotosToGet(numberOfPhotosToGet + 30);
    }

    function photoDeleted(photoId: string): void {
        setPhotos(photos.filter(photo => photo.photoId !== photoId));
    }

    useEffect(() => {
        fetchPhotos(numberOfPhotosToGet);
    }, []);

    return (
        <>
            <Container sx={{ display: 'flex', justifyContent: 'space-between', flexDirection: 'column', alignItems: 'center' }} >
                <Typography component="h1"
                    variant="h3"
                    align="left"
                    color="text.primary">{"By your favorite"}
                </Typography>

                <Box sx={{ height: 800, width: 750 }}>
                    {photos.map((photo, ind) => (
                        <ImageListItem key={ind} sx={{ padding: 2 }} >
                            <PhotoCard photoId={photo.photoId} photo={photo.photo} description={photo.description.slice(0, 40) + "..."}
                                numberOfFollowers={photo.numberOfFollowers} numberOfLikes={photo.numberOfLikes} numberOfComments={photo.numberOfComments} photoDeleted={photoDeleted}
                                authorUsername={photo.authorUsername} authorProfilePhoto={photo.authorProfilePhoto} categoryName={photo.categoryName} categoryColor={photo.categoryColor} />
                        </ImageListItem>
                    ))}
                </Box>

                <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                    <Button onClick={loadMoreButtonClick} variant="contained" sx={{
                        mt: 3, mb: 2, textAlign: 'center', background: '#BA1B2A', ':hover': {
                            bgcolor: '#E65664',
                            color: 'FFFFFF',
                        },
                    }}>Load more</Button>
                </Box>


            </Container>
        </>
    );
}

export default Homepage;