import { Alert, AlertTitle, Box, Button, Container, CssBaseline, Grid, TextField, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import PhotoFormProps from "../../../Interfaces/Photo/IPhotoForm";
import SelectPhotoCategory from "../../PhotoCategory/SelectPhotoCategory";
import axios, { AxiosResponse } from "axios";
import { useNavigate, useParams } from "react-router-dom";
import PhotoProp from "../../../Interfaces/Photo/IPhotoProp";


function PhotoForm(prop: PhotoFormProps) {
    const navigate = useNavigate();
    const { photoId } = useParams();
    const [description, setDescription] = useState("");
    const [selectedPhotoCategories, setSelectedPhotoCategories] = useState<string[]>([]);
    const [formData, setFormData] = useState(new FormData());
    const [imageSrc, setImageSrc] = useState("");
    const [areFieldsEmpthy, setAreFieldsEmpthyFlag] = useState(false);

    function areFieldsEmpty(): boolean {
        if (description.length > 0 && selectedPhotoCategories.length === 1 && imageSrc.length > 0) {
            setAreFieldsEmpthyFlag(false);
            return false;
        }
        else {
            setAreFieldsEmpthyFlag(true);
            return true;
        }
    }

    useEffect(() => {
        if (selectedPhotoCategories.length > 1) {
            setSelectedPhotoCategories(selectedPhotoCategories.slice(1));
        }
    }, [selectedPhotoCategories])

    useEffect(() => {
        if (prop.isEditForm == true) {
            const fetchPhotoInformation = async () => {
                const result = await axios.get<PhotoProp>(`https://localhost:7053/api/Photo/GetPhotoUpdateInformation/${photoId}`, {
                    headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
                });
                setImageSrc(`data:image/jpeg;base64,${result.data.photo}`);
                setDescription(result.data.description);
                let category: string[] = [result.data.category];
                setSelectedPhotoCategories(category);
            }

            fetchPhotoInformation();
        }
        else
            setImageSrc('/Placeholders&Icons/placeholder.jpg');
    }, []);

    const handlePhotoDescriptionChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        e.preventDefault();
        setDescription(e.currentTarget.value);
    }

    const handleChangePicture: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        e.preventDefault();
        formData.delete('photo');
        if (e.target.files !== null) {
            setImageSrc(URL.createObjectURL(e.target.files[0]));
            let form = new FormData();
            for (let i = 0; i < e.target.files.length; i++) {
                let element = e.target.files[i];
                form.append('photo', element);
            }
            setFormData(form);
        }
    }

    const handleServerOperationButtonClick: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
        if (!areFieldsEmpty()) {
            formData.append('description', description);
            formData.append('category', selectedPhotoCategories[0]);

            let result: AxiosResponse<any, any>;
            if (prop.isEditForm) {
                if (photoId != undefined)
                    formData.append('photoId', photoId);
                result = await axios.put(`https://localhost:7053/api/Photo/EditPhoto`, formData, {
                    headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
                });
            }
            else {

                formData.append('photoAuthor', sessionStorage.getItem('username')!);
                result = await axios.post(`https://localhost:7053/api/Photo/PostPhoto`, formData, {
                    headers: { 'Authorization': 'Bearer ' + window.sessionStorage.getItem("token") },
                });
            }

            if (result.status === 200)
                navigate(-1);
        }

    }

    useEffect(() => {
        document.title = `Post photo`;
    },[])

    return (
        <>
            <Container component="main" maxWidth="md">
                <CssBaseline />
                <Box
                    sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'flex-start',
                    }}
                >
                    <Typography component="h1" variant="h4"> {prop.isEditForm ? "Edit photo" : "Post photo"}</Typography>
                </Box>


                <Box component="form" noValidate sx={{ mt: 3 }}>

                    <Grid container spacing={2}>
                        <Grid item xs={12} >
                            <TextField
                                label={"Photo description"}
                                name={"photoDescription"}
                                type="text"
                                multiline={true}
                                value={description}
                                rows={2}
                                required
                                fullWidth
                                onChange={handlePhotoDescriptionChange}
                            />

                            <SelectPhotoCategory selectedPhotoCategories={selectedPhotoCategories} setSelectedPhotoCategories={setSelectedPhotoCategories} />
                        </Grid>

                    </Grid>
                </Box>

                <Box sx={{ display: "flex", justifyContent: "flex-start", flexDirection: "column" }}>
                    {!prop.isEditForm ?
                        <Box>
                            <Button
                                variant="contained"
                                component="label"
                                sx={{
                                    mt: 3, mb: 2, textAlign: 'center', background: '#BA1B2A', ':hover': {
                                        bgcolor: '#E65664',
                                        color: 'FFFFFF',
                                    },
                                }}
                            >
                                Upload photo
                                <input
                                    type="file"
                                    accept="image/png, image/jpeg"
                                    hidden
                                    onChange={handleChangePicture}
                                />
                            </Button>
                        </Box >
                        : ""}

                    <Box
                        component="img"
                        sx={{
                            mt: 3, mb: 2,
                            maxHeight: { xs: 433, md: 367 },
                            maxWidth: { xs: 550, md: 450 },

                        }}
                        src={imageSrc}
                    />

                </Box>

                {areFieldsEmpthy === true ?
                    <Alert severity="warning" sx={{ mt: 2 }}>
                        <AlertTitle>Warning</AlertTitle>
                        Fill empty fields!
                    </Alert>
                    : ""}

                <Box style={{ display: "flex", justifyContent: "center" }}>
                    <Button
                        type="submit"
                        onClick={handleServerOperationButtonClick}
                        variant="contained"
                        sx={{
                            mt: 3, mb: 2, textAlign: 'center', background: '#BA1B2A', ':hover': {
                                bgcolor: '#E65664',
                                color: 'FFFFFF',
                            },
                        }}
                    >
                        {prop.isEditForm ? "Edit" : "Post"}
                    </Button>
                </Box>

            </Container>
        </>
    );
}

export default PhotoForm;