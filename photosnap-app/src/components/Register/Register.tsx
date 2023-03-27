import { Alert, AlertTitle, Avatar, Box, Button, CssBaseline, Grid, TextField, Typography } from "@mui/material";
import { Container } from "@mui/system";
import axios from "axios";
import { useEffect, useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import IUserLoggedStatusChange from "../../Interfaces/LoginAndRegisterInterfaces/IUserLogged";
import LoginRegisterResponse from "../../Types/LoginAndRegisterTypes/LoginRegisterResponse";

function Register(prop: IUserLoggedStatusChange) {
    let navigate = useNavigate();
    const [formData, setFormData] = useState(new FormData);
    const [profilePhotoPreview, setProfilePhotoPreview] = useState(new Blob);
    const [name, setName] = useState("");
    const [lastname, setLastName] = useState("");
    const [biography, setBiography] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isUserLogged, setIsUserLogged] = useState(false);
    const [areFieldsValid, setAreFieldsValid] = useState(true);
    const [errorMessage, setErrorMessage] = useState("");

    const handleNameChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setName(e.currentTarget.value);
    }
    const handleLastnameChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setLastName(e.currentTarget.value);
    }
    const handleUsernameChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setUsername(e.currentTarget.value);

    }
    const handlePasswordChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setPassword(e.currentTarget.value);
    }
    const handleBiographyChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        setBiography(e.currentTarget.value);
    }

    const handleChangePicture: React.ChangeEventHandler<HTMLInputElement> = (e) => {
        if (e.target.files !== null) {
            setProfilePhotoPreview(e.target.files[0]);
            formData.delete("profilePhoto");
            let form = new FormData();
            for (let i = 0; i < e.target.files.length; i++) {
                let element = e.target.files[i];
                form.append('profilePhoto', element);
            }
            setFormData(form);
        }
    }

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        formData.delete("username"); formData.delete("name");
        formData.delete("lastname"); formData.delete("biography");
        formData.delete("password");

        if (name.length == 0 || lastname.length == 0 || biography.length == 0 || username.length == 0 || password.length == 0){
            setAreFieldsValid(false);
            setErrorMessage("all fields must be filed!");
        }
        else {
            formData.append("username", username); formData.append("name", name);
            formData.append("lastname", lastname); formData.append("biography", biography);
            formData.append("password", password);

            axios.post<LoginRegisterResponse>('https://localhost:7053/api/User/RegisterUser', formData)
                .then((response) => {
                    if (response.status === 200) {
                        setAreFieldsValid(true);
                        window.sessionStorage.setItem("token", response.data.token);
                        window.sessionStorage.setItem("username", response.data.username);
                        window.sessionStorage.setItem("profilePhoto", response.data.profilePhoto);
                        window.sessionStorage.setItem("isUserLogged", "true");

                        setIsUserLogged(true);
                        navigate(-1);
                        prop.userLogged();
                    }
                })
                .catch((err) => {
                    setErrorMessage("username is already taken!"); 
                    setAreFieldsValid(false);
                 });


        }
    }

    useEffect(() => {
        document.title = "Register";
    }, []);

    const handlersForFirstConfig: React.ChangeEventHandler<HTMLInputElement>[] = [handleNameChange, handleLastnameChange];
    const handlersForSecondConfig: React.ChangeEventHandler<HTMLInputElement>[] = [handleUsernameChange, handlePasswordChange, handleBiographyChange];

    const firstConfigs: string[][] = [["Name", "Name", "name"], ["Last name", "Lastname", "lastname"]];
    const secondConfigs: string[][] = [["Username", "Username", "username", "text"], ["Password", "Password", "password", "password"], ["Biography", "Biography", "biography", "text"]];
    return (
        <>
            <Container component="main" maxWidth="sm">
                {isUserLogged ? <Navigate to={"/homepage"} /> : ""}
                <CssBaseline />
                <Box
                    sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <Typography component="h1" variant="h4"> Create Photosnap account  </Typography>

                    <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3, textAlign: "center" }}   >
                        <Grid container spacing={2}   >
                            <Grid item xs={12} container={true} direction={"column"} display={"flex"}>

                                <Avatar
                                    sx={{ width: 200, height: 200, alignSelf: "center" }}
                                    src={URL.createObjectURL(profilePhotoPreview)} />
                                <Box width={"auto"}>

                                    <Button
                                        fullWidth={false}
                                        size="small"
                                        variant="contained"
                                        component="label"
                                        sx={{
                                            mt: 3, mb: 2, background: '#BA1B2A', ':hover': {
                                                bgcolor: '#E65664',
                                                color: 'FFFFFF',
                                            },
                                        }}
                                    >
                                        Add profile photo
                                        <input accept="image/*" type="file" hidden onChange={handleChangePicture} />
                                    </Button>
                                </Box>
                            </Grid>


                            {firstConfigs.map((conf, ind) => (
                                <Grid item xs={12} sm={6} key={conf[0]}>
                                    <TextField
                                        label={conf[0]}
                                        name={conf[1]}
                                        id={conf[2]}
                                        required
                                        fullWidth
                                        autoFocus={conf[1] === "Name" ? true : false}
                                        onChange={handlersForFirstConfig[ind]}
                                    />
                                </Grid>
                            ))}

                            {secondConfigs.map((conf, ind) => (
                                <Grid item xs={12} key={conf[2]}>
                                    <TextField
                                        label={conf[0]}
                                        name={conf[1]}
                                        id={conf[2]}
                                        type={conf[3]}
                                        required
                                        fullWidth
                                        onChange={handlersForSecondConfig[ind]}
                                        multiline={conf[1] == "Biography" ? true : false}
                                        rows={conf[1] == "Biography" ? 6 : 1}
                                    />
                                </Grid>
                            ))}
                        </Grid>


                        {areFieldsValid === false ?
                            <Alert severity="warning" sx={{ marginTop: "10px" }}>
                                <AlertTitle >Warning - <strong>{errorMessage}</strong></AlertTitle>

                            </Alert> : ""}

                        <Button
                            type="submit"
                            size="medium"
                            variant="contained"
                            sx={{
                                mt: 3, mb: 2, background: '#BA1B2A', ':hover': {
                                    bgcolor: '#E65664',
                                    color: 'FFFFFF',
                                },
                            }}
                        >
                            Create account
                        </Button>

                    </Box>
                </Box>
            </Container>
        </>
    );
}

export default Register;