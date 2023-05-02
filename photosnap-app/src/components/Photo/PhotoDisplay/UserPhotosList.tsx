import * as React from 'react';
import ImageList from '@mui/material/ImageList';
import ImageListItem from '@mui/material/ImageListItem';
import IUsername from '../../../Interfaces/UserProfile/IUsername';
import { useEffect, useState } from 'react';
import PhotoDisplay from '../../../Interfaces/Photo/IPhotoDisplay';
import PhotoCard from '../PhotoCard';
import axios from 'axios';
import { Button, Container, Typography } from '@mui/material';

function UserPhotosList(prop: IUsername) {
    const [photos, setPhotos] = useState<PhotoDisplay[]>([]);
    const [numberOfPhotosToGet, setNumberOfPhotosToGet] = useState(9);
    const [totalUserPhotos, setTotalUserPhotos] = useState<number>(9);
    const [allPhotosFetchedFlag, setAllPhotosFetchedFlag] = useState(false);


    const fetchTotalNumberOfPhotos = async () => {
        const result = await axios.get<number>(`https://localhost:7053/api/User/GetTotalNumberOfUserPhotos/${prop.username}`, {
            headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
        });
        setTotalUserPhotos(result.data);
    }

    const fetchPhotos = async () => {
        if (!allPhotosFetchedFlag) {
            const result = await axios.get<PhotoDisplay[]>(`https://localhost:7053/api/User/GetUserPhotos/${prop.username}/${numberOfPhotosToGet}`, {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
            });
            setPhotos(result.data);
        }
        if (numberOfPhotosToGet > totalUserPhotos)
            setAllPhotosFetchedFlag(true);
        else
            setNumberOfPhotosToGet(numberOfPhotosToGet + 9);
    }

    const loadMoreButtonClick: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault();
        fetchPhotos();
    }

    function photoDeleted(photoId: string): void{
        setPhotos(photos.filter(photo => photo.photoId !== photoId));
    }

    useEffect(() => {
        if (prop.username != "") {
            fetchTotalNumberOfPhotos();
            fetchPhotos();
        }
    }, [prop])

    return (
        <Container sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
            <ImageList sx={{ width: 1000, height: 950, flexWrap: "wrap" }} cols={3} >
                {photos.map((photo, ind) => (
                    <ImageListItem key={ind} >
                        <PhotoCard photoId={photo.photoId} photo={photo.photo} description={photo.description.slice(0, 40) + "..."}
                            numberOfFollowers={photo.numberOfFollowers} numberOfLikes={photo.numberOfLikes} numberOfComments={photo.numberOfComments} photoDeleted={photoDeleted}
                            authorUsername={photo.authorUsername} authorProfilePhoto={photo.authorProfilePhoto} categoryName={photo.categoryName} categoryColor={photo.categoryColor} />
                    </ImageListItem>
                ))}
            </ImageList>

            {!allPhotosFetchedFlag ?
                <Button onClick={loadMoreButtonClick} variant="contained" sx={{
                    mt: 3, mb: 2, textAlign: 'center', background: '#BA1B2A', ':hover': {
                        bgcolor: '#E65664',
                        color: 'FFFFFF',
                    },
                }}>Load more</Button> : ""}
        </Container>

    );
}
export default UserPhotosList;