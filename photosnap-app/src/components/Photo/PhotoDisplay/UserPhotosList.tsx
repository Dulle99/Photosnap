import * as React from 'react';
import ImageList from '@mui/material/ImageList';
import ImageListItem from '@mui/material/ImageListItem';
import IUsername from '../../../Interfaces/UserProfile/IUsername';
import { useEffect, useState } from 'react';
import PhotoDisplay from '../../../Interfaces/Photo/IPhotoDisplay';
import PhotoCard from '../PhotoCard';
import axios from 'axios';
import { Button, Container, Typography } from '@mui/material';
import IUserPhotoListProp from '../../../Interfaces/UserProfile/IUserPhotoList-props';

function UserPhotosList(prop: IUserPhotoListProp) {
    const [photos, setPhotos] = useState<PhotoDisplay[]>([]);
    const [numberOfPhotosToGet, setNumberOfPhotosToGet] = useState(9);
    const [totalUserPhotos, setTotalUserPhotos] = useState<number>(9);
    const [allPhotosFetchedFlag, setAllPhotosFetchedFlag] = useState(false);

    const fetchTotalNumberOfPhotos = async () => {
        let fetchTotalNumberOfPhotosUrl = "";
        if(prop.displayUserPhotos === true)
            fetchTotalNumberOfPhotosUrl = `https://localhost:7053/api/User/GetTotalNumberOfUserPhotos`;
        else
            fetchTotalNumberOfPhotosUrl = `https://localhost:7053/api/User/GetTotalNumberOfUserLikedPhotos`;
        const result = await axios.get<number>(`${fetchTotalNumberOfPhotosUrl}/${prop.username}`, {
            headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
        });
        setTotalUserPhotos(result.data);
    }

    const fetchPhotos = async () => {
            let fetchPhotosUrl = "";
            if(prop.displayUserPhotos === true)
                fetchPhotosUrl = `https://localhost:7053/api/User/GetUserPhotos`;
            else
                fetchPhotosUrl = `https://localhost:7053/api/User/GetUserLikedPhotos`;

            console.log(fetchPhotosUrl);
            const result = await axios.get<PhotoDisplay[]>(`${fetchPhotosUrl}/${prop.username}/${numberOfPhotosToGet}`, {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
            });
            setPhotos(result.data);
            console.log(`slike ${photos}`)
        
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
        setAllPhotosFetchedFlag(false);
        setNumberOfPhotosToGet(9);
        setPhotos([]);
        if (prop.username != "") {
            fetchTotalNumberOfPhotos();
            fetchPhotos();
        }
    }, [prop])


    if(photos.length == 0){
        return(
        <>
            <Container sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
                <Typography>No posts</Typography>
            </Container>
        </>
    )}
    else{
    return (
        <Container sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', flexDirection: 'column' }}>
            <ImageList sx={{ width: 1000, height: 950, flexWrap: "wrap" }} cols={3} >
                {photos.map((photo, ind) => (
                    <ImageListItem key={ind} >
                        <PhotoCard photoId={photo.photoId} photo={photo.photo} description={photo.description}
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

    );}
}
export default UserPhotosList;