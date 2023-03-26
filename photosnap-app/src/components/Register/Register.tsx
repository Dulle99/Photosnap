import { Alert, AlertTitle, Box, Button, CssBaseline, Grid, TextField, Typography } from "@mui/material";
import { Container } from "@mui/system";
import { useState } from "react";
import { Navigate } from "react-router-dom";
import IUserLoggedStatusChange from "../../Interfaces/LoginAndRegisterInterfaces/IUserLogged";
import LoginRegisterResponse from "../../Types/LoginAndRegisterTypes/LoginRegisterResponse";

function Register(prop: IUserLoggedStatusChange) {
    const [name, setName] = useState("");
    const [lastname, setLastName] = useState("");
    const [biography, setBiography] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isUserLogged, setIsUserLogged] = useState(false);
    const [areFieldsValid, setAreFieldsValid] = useState(true);

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

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (name.length == 0 || lastname.length == 0 || biography.length == 0 || username.length == 0 || password.length == 0)
            setAreFieldsValid(false);
        else
            setAreFieldsValid(true);
    }

    const handlersForFirstConfig: React.ChangeEventHandler<HTMLInputElement>[] = [handleNameChange, handleLastnameChange];
    const handlersForSecondConfig: React.ChangeEventHandler<HTMLInputElement>[] = [handleUsernameChange, handlePasswordChange, handleBiographyChange];

    const firstConfigs: string[][] = [["Ime", "Ime", "ime"], ["Prezime", "Prezime", "prezime"]];
    const secondConfigs: string[][] = [["Korisničko ime", "KorisnickoIme", "korisnickoIme", "text"], ["Lozinka", "Lozinka", "lozinka", "password"], ["Biografija", "Biografija", "biografija", "text"]];
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
                    <Typography component="h1" variant="h3"> Kreiraj Photosnap nalog  </Typography>

                    <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3, textAlign: "center" }}>
                        <Grid container spacing={2}>
                            {firstConfigs.map((conf, ind) => (
                                <Grid item xs={12} sm={6} key={conf[0]}>
                                    <TextField
                                        label={conf[0]}
                                        name={conf[1]}
                                        id={conf[2]}
                                        required
                                        fullWidth
                                        autoFocus={conf[1] === "Ime" ? true : false}
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
                                        multiline= {conf[1] == "Biografija" ? true : false}
                                        rows= {conf[1] == "Biografija" ? 6 : 1}
                                    />
                                </Grid>
                            ))}
                        </Grid>


                        {areFieldsValid === false ?
                            <Alert severity="warning" sx={{marginTop: "10px"}}>
                                <AlertTitle >Upozorenje</AlertTitle>
                                <strong>Sva polja moraju biti popunjena</strong>
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
                            Kreiraj nalog
                        </Button>

                    </Box>
                </Box>
            </Container>
        </>
    );
}

export default Register;