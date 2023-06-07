import { Box, Button, Container, ImageListItem, Typography } from "@mui/material";
import PhotoCard from "../Photo/PhotoCard";
import { useEffect, useState } from "react";
import PhotoDisplay from "../../Interfaces/Photo/IPhotoDisplay";
import SelectPhotoCategory from "../PhotoCategory/SelectPhotoCategory";
import axios from "axios";
import CategoryItemType from "../../Types/CategoryType/CategoryItemType";

function ExplorePhotos() {
    const [photos, setPhotos] = useState<PhotoDisplay[]>([]);
    const [numberOfPhotosToGet, setNumberOfPhotosToGet]= useState(30);
    const [selectedPhotoCategories, setSelectedPhotoCategories] = useState<string[]>([]);

    function photoDeleted(photoId: string): void {
        setPhotos(photos.filter(photo => photo.photoId !== photoId));
    }

    async function fetchPhotos(numberOfPhotosToFetch: number){
        var params = new URLSearchParams();
        selectedPhotoCategories.forEach(category => {
            params.append('categories',category);
        })
        const result = await axios.get<PhotoDisplay[]>(`https://localhost:7053/api/Photo/GetPhotoByCategories/${numberOfPhotosToFetch}`, {
                headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
                params: params
                
            });
            setPhotos(result.data);
    }

    const loadMoreButtonClick: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault();
        fetchPhotos(numberOfPhotosToGet + 30);
        setNumberOfPhotosToGet(numberOfPhotosToGet +30);
    }

    const fetchUsersPhotoIntersts = async () =>{
        const result = await axios.get<CategoryItemType[]>(`https://localhost:7053/api/User/GetUserListOfPhotoInterests/${window.sessionStorage.getItem('username')}/100`,
        {
            headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
        });

        let usersPhotoCategoryOfInterest: string[] = [];
        if(result.status === 200){
            result.data.forEach((category) =>{usersPhotoCategoryOfInterest.push(category.categoryName)});
            setSelectedPhotoCategories(usersPhotoCategoryOfInterest);
        }
    }

    useEffect(() => {
        if (selectedPhotoCategories.length >= 1) {
            fetchPhotos(30);
        }
    }, [selectedPhotoCategories])

    useEffect(()=>{
        if(window.sessionStorage.getItem('username') != null)
            fetchUsersPhotoIntersts();
        fetchPhotos(30);
    },[]);

    useEffect(() => {
        document.title = `Explore photosnap`;
    },[])

    return (<>
        <Container sx={{ display: 'flex', justifyContent: 'space-between', flexDirection: 'column', alignItems: 'center' }} >
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                <Typography component="h1"
                    variant="h3"
                    align="left"
                    color="text.primary">{"Explore photosnap"}
                </Typography>
            </Box>
            <Box sx={{ display: 'flex', justifyContent: 'left', alignSelf:'center' }}>
                <SelectPhotoCategory setSelectedPhotoCategories={setSelectedPhotoCategories} selectedPhotoCategories={selectedPhotoCategories} />
            </Box>

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
    </>);
}

export default ExplorePhotos;